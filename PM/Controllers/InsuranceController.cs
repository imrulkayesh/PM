using PM.BLL;
using PM.Model;
using PM.ModelsV;
using PM.MyAuth;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.OleDb;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace PM.Controllers
{

    [Authorize]
    [CustomHttpReffer]
    public class InsuranceController : Controller
    {
        private PMEntities db = new PMEntities();
        private PMExcelDataLoader EDL = new PMExcelDataLoader();
        private ReturnPermission RP = new ReturnPermission();
        private GE GE = new GE();
        [KAuth(111)]
        public ActionResult Index()
        {
            //var INSU = (from i in db.A_INSUH
            //            where i.ACT == true
            //            select new
            //            InsuranceVM
            //            {
            //                INSU_ID=i.INSU_ID,
            //                INSU_ASET = i.A_ASET_DATA.ASET_CODE_NAME,
            //                INSU_POLICY = i.INSU_POLICY,
            //                INSU_ASET_VALUE = i.INSU_ASET_VALUE ?? 0,
            //                INSU_PREMIUM = i.INSU_PREMIUM ?? 0,
            //                INSU_COMP = i.INSU_COMP,
            //                INSU_COMP_ADDRESS = i.INSU_COMP_ADDRESS,
            //                INSU_NOTE = i.INSU_NOTE,
            //                INSU_REG_START = i.INSU_REG_START ?? DateTime.Now,
            //               INSU_REG_END=i.INSU_REG_END ?? DateTime.Now
            //           }).ToList();

            string U = Session["UserId"].ToString();

            var INSU = (from i in db.A_INSUH
                        join a in db.VW_USER_ASSET_ID_STRING on i.INSU_ASET equals a.ASET_ID
                        where i.ACT == true && a.UG_USER == U
                        select new InsuranceVM
                        {
                            INSU_ID = i.INSU_ID,
                            INSU_ASET = i.A_ASET_DATA.ASET_CODE_NAME,
                            INSU_POLICY = i.INSU_POLICY,
                            INSU_ASET_VALUE = i.INSU_ASET_VALUE ?? 0,
                            INSU_PREMIUM = i.INSU_PREMIUM ?? 0,
                            INSU_COMP = i.INSU_COMP,
                            INSU_COMP_ADDRESS = i.INSU_COMP_ADDRESS,
                            INSU_NOTE = i.INSU_NOTE,
                            INSU_REG_START = i.INSU_REG_START ?? DateTime.Now,
                            INSU_REG_END = i.INSU_REG_END ?? DateTime.Now
                        }).ToList();

            return View(INSU);
        }
        [KAuth(111)]
        [HttpGet]
        public ActionResult NewInsurance()
        {
            var model = new ExcelInputVM();
            return View(model);
        }
        [KAuth(111)]
        [HttpPost]
        public ActionResult NewInsurance(ExcelInputVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["msg"] = "swal('Ohh..','Select a valid excel file','error');";
                return View(model);
            }
            const string MME = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string excelExtention = string.Empty;
            excelExtention = System.IO.Path.GetExtension(model.MyExcelFile.FileName).ToLower();
            string FileType = model.MyExcelFile.ContentType;
            if (excelExtention != ".xlsx" || FileType != MME)
            {
                TempData["msg"] = "swal('Ohh..','Select a valid excel file','error');";
                return View(model);
            }
            DataTable dt = EDL.NewInsuranceExcelData(model.MyExcelFile.InputStream, false);
            string strContent = EDL.ConvertDataTableToHTMLTable(dt);
            model.MSExcelTable = strContent;
            TempData["msg"] = "swal('Success..','Excel fill loaded','success');";
            return View(model);
        }
        [KAuth(111)]
        [HttpPost]
        public ActionResult SaveNewInsurance(InsuranceVM[] ExlData)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            string result = "Policy data is not complete!";
            if (ModelState.IsValid)
            {
                int SuccessCount = 0;
                List<string> MyAsset = new List<string>(RP.MyAssetListString(UserId: U, UserType: T));
                foreach (var item in ExlData)
                {
                    bool IsValidDate = item.INSU_REG_START >= item.INSU_REG_END;
                    if (IsValidDate)
                    {
                        result = string.Format("{0} New Policy uploaded \n {1}-{2} Insurance date is not valid!", SuccessCount, item.INSU_ASET, item.INSU_POLICY);
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    string AsetId = RP.AsetByName(item.INSU_ASET).ToString();
                    bool IsOk = MyAsset.Contains(AsetId);
                    if (IsOk)
                    {
                        List<A_INSUH> OldData = db.A_INSUH.Where(o => o.INSU_ASET == AsetId).ToList();
                        bool IsRunning = OldData.Where(r => r.INSU_REG_END >= item.INSU_REG_START).Any();
                        if (IsRunning == true)
                        {
                            result = string.Format("{0} New Policy uploaded \n {1}-{2} Insurance already running!", SuccessCount, item.INSU_ASET, item.INSU_POLICY);
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                        A_INSUH IsExist = db.A_INSUH.Where(p => p.INSU_POLICY == item.INSU_POLICY).FirstOrDefault();
                        if (IsExist == null)
                        {
                            string INS_ID = GE.FifteenDigitId("INSU", AsetId, item.INSU_REG_START);
                            A_INSUH H = new A_INSUH();
                            H.INSU_ID = INS_ID;
                            H.INSU_ASET = AsetId;
                            H.INSU_POLICY = item.INSU_POLICY;
                            H.INSU_ASET_VALUE = item.INSU_ASET_VALUE;
                            H.INSU_PREMIUM = item.INSU_PREMIUM;
                            H.INSU_COMP = item.INSU_COMP;
                            H.INSU_COMP_ADDRESS = item.INSU_COMP_ADDRESS ?? "Unknown";
                            H.INSU_NOTE = item.INSU_NOTE ?? "NA";
                            H.INSU_REG_START = item.INSU_REG_START;
                            H.INSU_REG_END = item.INSU_REG_END;
                            H.ACT = true;
                            H.CDT = DateTime.Now;
                            H.CDU = U;
                            H.VAR = 1;
                            H.REMIND = false;
                            A_INSUD D = new A_INSUD();
                            D.INSU_ID = INS_ID;
                            D.INSU_LINE = 1;
                            D.INSU_ASET_VALUE = item.INSU_ASET_VALUE;
                            D.INSU_PREMIUM = item.INSU_PREMIUM;
                            D.INSU_AMOUNT = item.INSU_PREMIUM + item.INSU_VAT_AMOUNT;
                            D.INSU_VAT_AMOUNT = item.INSU_VAT_AMOUNT;
                            D.INSU_DATE = item.INSU_DATE;
                            D.INSU_PAID = "Y";
                            D.ACT = true;
                            D.CDT = DateTime.Now;
                            D.CDU = U;
                            D.UDT = DateTime.Now;
                            D.UDU = U;
                            D.VAR = 1;
                            db.A_INSUH.Add(H);
                            db.A_INSUD.Add(D);
                            db.SaveChanges();
                            SuccessCount = SuccessCount + 1;
                        }
                        else
                        {
                            result = string.Format("{0} New Policy uploaded \n {1}-{2} Invalid policy number!", SuccessCount, item.INSU_ASET, item.INSU_POLICY);
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        result = string.Format("{0} New Policy uploaded \n {1} Unable to access Asset!", SuccessCount, item.INSU_ASET);
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                result = string.Format("{0} Policy uploaded!", SuccessCount);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [KAuth(111)]
        [HttpGet]
        public ActionResult UpdatePolicy()
        {
            var model = new ExcelInputVM();
            return View(model);
        }
        [HttpPost]
        public ActionResult UpdatePolicy(ExcelInputVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["msg"] = "swal('Ohh..','Select a valid excel file','error');";
                return View(model);
            }
            const string MME = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string excelExtention = string.Empty;
            excelExtention = System.IO.Path.GetExtension(model.MyExcelFile.FileName).ToLower();
            string FileType = model.MyExcelFile.ContentType;
            if (excelExtention != ".xlsx" || FileType != MME)
            {
                TempData["msg"] = "swal('Ohh..','Select a valid excel file','error');";
                return View(model);
            }
            DataTable dt = EDL.UpdateInsuranceExcelData(model.MyExcelFile.InputStream, false);
            string strContent = EDL.ConvertDataTableToHTMLTable(dt);
            model.MSExcelTable = strContent;
            TempData["msg"] = "swal('Success..','Excel fill loaded','success');";
            return View(model);
        }
        [KAuth(111)]
        [HttpPost]
        public ActionResult SaveUpdateInsurance(UpdateInsuranceSave[] ExlData)
        {

            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            string result = "Policy data is not complete!";
            if (ModelState.IsValid)
            {
                int SuccessCount = 0;
                List<string> MyAsset = new List<string>(RP.MyAssetListString(UserId: U, UserType: T));
                foreach (var item in ExlData)
                {
                    bool IsValidDate = item.INSU_REG_START >= item.INSU_REG_END;
                    if (IsValidDate)
                    {
                        result = string.Format("{0} Policy uploaded \n {1}-{2} Insurance date is not valid!", SuccessCount, item.INSU_ASET, item.INSU_POLICY);
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    string AsetId = RP.AsetByName(item.INSU_ASET).ToString();
                    bool IsOk = MyAsset.Contains(AsetId);
                    if (IsOk)
                    {
                        A_INSUH IsExist = db.A_INSUH.Where(p => p.INSU_POLICY == item.INSU_POLICY && p.INSU_ASET == AsetId).FirstOrDefault();
                        if (IsExist != null)
                        {
                            if (IsExist.INSU_REG_END >= item.INSU_REG_START)
                            {
                                result = string.Format("{0} Policy updated \n {1}-{2} Invalid policy date!", SuccessCount, item.INSU_ASET, item.INSU_POLICY);
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                            IsExist.INSU_ASET_VALUE = item.INSU_ASET_VALUE;
                            IsExist.INSU_PREMIUM = item.INSU_PREMIUM;
                            IsExist.INSU_NOTE = item.INSU_NOTE;
                            IsExist.INSU_REG_START = item.INSU_REG_START;
                            IsExist.INSU_REG_END = item.INSU_REG_END;
                            IsExist.UDT = DateTime.Now;
                            IsExist.UDU = U;
                            IsExist.VAR = IsExist.VAR + 1;
                            IsExist.REMIND = false;
                            var Total = db.A_INSUD.Where(d => d.INSU_ID == IsExist.INSU_ID).ToList();
                            bool IsPaidSameDate = Total.Where(t => t.INSU_DATE == item.INSU_DATE).Any();
                            if (IsPaidSameDate)
                            {
                                result = string.Format("{0} Policy updated \n {1}-{2} policy already paid!", SuccessCount, item.INSU_ASET, item.INSU_POLICY);
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                            A_INSUD D = new A_INSUD();
                            D.INSU_ID = IsExist.INSU_ID;
                            D.INSU_LINE = Total.Count + 1;
                            D.INSU_ASET_VALUE = item.INSU_ASET_VALUE;
                            D.INSU_PREMIUM = item.INSU_PREMIUM;
                            D.INSU_AMOUNT = item.INSU_PREMIUM + item.INSU_VAT_AMOUNT;
                            D.INSU_VAT_AMOUNT = item.INSU_VAT_AMOUNT;
                            D.INSU_DATE = item.INSU_DATE;
                            D.INSU_PAID = "Y";
                            D.ACT = true;
                            D.CDT = DateTime.Now;
                            D.CDU = U;
                            D.UDT = DateTime.Now;
                            D.UDU = U;
                            D.VAR = 1;
                            db.Entry(IsExist).State = EntityState.Modified;
                            db.A_INSUD.Add(D);
                            db.SaveChanges();
                            SuccessCount = SuccessCount + 1;
                        }
                        else
                        {
                            result = string.Format("{0} Policy updated \n {1}-{2} Invalid policy number!", SuccessCount, item.INSU_ASET, item.INSU_POLICY);
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        result = string.Format("{0} Policy updated \n {1} Unable to access Asset!", SuccessCount, item.INSU_ASET);
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                result = string.Format("{0} Policy updated!", SuccessCount);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [KAuth(111)]
        public ActionResult ViewPolicyDetail(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Detail_Data = from d in db.A_INSUD
                              where d.INSU_ID == id
                              group new { d } by d.INSU_ID into g
                              select new
                              {
                                  INSUH = g.Key,
                                  PAmount = g.Sum(x => x.d.INSU_PREMIUM),
                                  IAmount = g.Sum(x => x.d.INSU_AMOUNT),
                                  VAmount = g.Sum(x => x.d.INSU_VAT_AMOUNT)
                              };

            ViewBag.INSUH = Detail_Data.FirstOrDefault().INSUH;
            ViewBag.PAmount = Detail_Data.FirstOrDefault().PAmount;
            ViewBag.IAmount = Detail_Data.FirstOrDefault().IAmount;
            ViewBag.VAmount = Detail_Data.FirstOrDefault().VAmount;

            var VI = (from d in db.A_INSUD
                      where d.INSU_ID == id
                      select new InsuranceDetail
                      {
                          INSU_LINE = d.INSU_LINE,
                          INSU_PREMIUM = d.INSU_PREMIUM ?? 0,
                          INSU_AMOUNT = d.INSU_AMOUNT ?? 0,
                          INSU_VAT_AMOUNT = d.INSU_VAT_AMOUNT ?? 0,
                          INSU_DATE = d.INSU_DATE
                      }).ToList();
            return PartialView(VI);
        }

        [HttpGet]
        public ActionResult TestExcel()
        {
            return View();
        }
        [HttpPost]
        public ActionResult TestExcel(ExcelInputVM model)
        {
            if (ModelState.IsValid)
            {
                string excelExtention = System.IO.Path.GetExtension(model.MyExcelFile.FileName).ToLower();
                string filePath = System.IO.Path.GetFullPath(model.MyExcelFile.FileName).ToLower();
                string conStr = "";
                switch (excelExtention)
                {
                    case ".xls": //Excel 97-03
                        conStr = ConfigurationManager.ConnectionStrings["Exl03ConStr"].ConnectionString;
                        break;
                    case ".xlsx": //Excel 07-13
                        conStr = ConfigurationManager.ConnectionStrings["Exl07ConStr"].ConnectionString;
                        break;
                }
                conStr = String.Format(conStr, filePath);
                OleDbConnection connExcel = new OleDbConnection(conStr);
                OleDbCommand cmdExcel = new OleDbCommand();
                OleDbDataAdapter oda = new OleDbDataAdapter();
                DataTable dt = new DataTable();
                cmdExcel.Connection = connExcel;

                //Get the name of First Sheet
                connExcel.Open();
                DataTable dtExcelSchema;
                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                connExcel.Close();

                //Read Data from First Sheet
                connExcel.Open();
                cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
                oda.SelectCommand = cmdExcel;
                oda.Fill(dt);
                connExcel.Close();

                List<ImportFile> dataList = new List<ImportFile>();
                dataList = (from DataRow dr in dt.Rows
                            select new ImportFile()
                            {
                                COL1 = dr["COL1"].ToString(),
                                COL2 = dr["COL2"].ToString(),
                                COL3 = dr["COL3"].ToString(),
                                COL4 = Convert.ToDateTime(dr["COL4"]),
                                COL5 = dr["COL5"].ToString()
                            }).ToList();
            }
            return View();
        }

        [KAuth(112)]
        public ActionResult InsuranceBD(string AsetName, string Operation)
        {
            string U = Session["UserId"].ToString();
            var INSU = (from i in db.A_INSUH
                        join u in db.VW_USER_ASSET_ID_STRING on i.INSU_ASET equals u.ASET_ID
                        join a in db.A_ASET_DATA on i.INSU_ASET equals a.ASET_ID
                        where i.ACT == true && u.UG_USER == U
                        select new InsuranceVMBD
                        {
                            INSU_ID = i.INSU_ID,
                            INSU_ASET = i.A_ASET_DATA.ASET_CODE_NAME,
                            INSU_POLICY = i.INSU_POLICY,
                            ASET_CAPACITY = a.ASET_CAPACITY,
                            INSU_PREMIUM = i.INSU_PREMIUM ?? 0,
                            INSU_COMP = i.INSU_COMP,
                            INSU_COMP_ADDRESS = i.INSU_COMP_ADDRESS,
                            INSU_NOTE = i.INSU_NOTE,
                            INSU_REG_START = i.INSU_REG_START ?? DateTime.Now,
                            INSU_REG_END = i.INSU_REG_END ?? DateTime.Now
                        }).ToList();
            if (!string.IsNullOrWhiteSpace(AsetName))
            {
                return View(INSU.Where(a => a.INSU_ASET == AsetName).ToList());
            }
            else
            {
                return View(INSU);
            }


        }
        [KAuth(112)]
        public ActionResult NewInsuranceBD()
        {
            var model = new ExcelInputVM();
            return View(model);
        }
        [KAuth(112)]
        [HttpPost]
        public ActionResult NewInsuranceBD(ExcelInputVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["msg"] = "swal('Ohh..','Select a valid excel file','error');";
                return View(model);
            }
            const string MME = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string excelExtention = string.Empty;
            excelExtention = System.IO.Path.GetExtension(model.MyExcelFile.FileName).ToLower();
            string FileType = model.MyExcelFile.ContentType;
            if (excelExtention != ".xlsx" || FileType != MME)
            {
                TempData["msg"] = "swal('Ohh..','Select a valid excel file','error');";
                return View(model);
            }
            DataTable dt = EDL.NewInsuranceExcelData(model.MyExcelFile.InputStream, false);
            string strContent = EDL.ConvertDataTableToHTMLTable(dt);
            model.MSExcelTable = strContent;
            TempData["msg"] = "swal('Success..','Excel fill loaded','success');";
            return View(model);
        }
        [KAuth(112)]
        [HttpPost]
        public ActionResult SaveNewInsuranceBD(InsuranceVM[] ExlData)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            string result = "Policy data is not complete!";
            if (ModelState.IsValid)
            {
                int SuccessCount = 0;
                List<string> MyAsset = new List<string>(RP.MyAssetListString(UserId: U, UserType: T));
                foreach (var item in ExlData)
                {
                    bool IsValidDate = item.INSU_REG_START >= item.INSU_REG_END;
                    if (IsValidDate)
                    {
                        result = string.Format("{0} New Policy uploaded \n {1}-{2} Insurance date is not valid!", SuccessCount, item.INSU_ASET, item.INSU_POLICY);
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    string AsetId = RP.AsetByName(item.INSU_ASET).ToString();
                    bool IsOk = MyAsset.Contains(AsetId);
                    if (IsOk)
                    {
                        List<A_INSUH> OldData = db.A_INSUH.Where(o => o.INSU_ASET == AsetId).ToList();
                        bool IsRunning = OldData.Where(r => r.INSU_REG_END >= item.INSU_REG_START).Any();
                        if (IsRunning == true)
                        {
                            result = string.Format("{0} New Policy uploaded \n {1}-{2} Insurance already running!", SuccessCount, item.INSU_ASET, item.INSU_POLICY);
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                        A_INSUH IsExist = db.A_INSUH.Where(p => p.INSU_POLICY == item.INSU_POLICY).FirstOrDefault();
                        if (IsExist == null)
                        {
                            string INS_ID = GE.FifteenDigitId("INSU", AsetId, item.INSU_REG_START);
                            A_INSUH H = new A_INSUH();
                            H.INSU_ID = INS_ID;
                            H.INSU_ASET = AsetId;
                            H.INSU_POLICY = item.INSU_POLICY;
                            H.INSU_ASET_VALUE = item.INSU_ASET_VALUE;
                            H.INSU_PREMIUM = item.INSU_PREMIUM;
                            H.INSU_COMP = item.INSU_COMP;
                            H.INSU_COMP_ADDRESS = item.INSU_COMP_ADDRESS ?? "Unknown";
                            H.INSU_NOTE = item.INSU_NOTE ?? "NA";
                            H.INSU_REG_START = item.INSU_REG_START;
                            H.INSU_REG_END = item.INSU_REG_END;
                            H.ACT = true;
                            H.CDT = DateTime.Now;
                            H.CDU = U;
                            H.VAR = 1;
                            H.REMIND = false;
                            A_INSUD D = new A_INSUD();
                            D.INSU_ID = INS_ID;
                            D.INSU_LINE = 1;
                            D.INSU_ASET_VALUE = item.INSU_ASET_VALUE;
                            D.INSU_PREMIUM = item.INSU_PREMIUM;
                            D.INSU_AMOUNT = item.INSU_PREMIUM + item.INSU_VAT_AMOUNT;
                            D.INSU_VAT_AMOUNT = item.INSU_VAT_AMOUNT;
                            D.INSU_DATE = item.INSU_DATE;
                            D.INSU_PAID = "Y";
                            D.ACT = true;
                            D.CDT = DateTime.Now;
                            D.CDU = U;
                            D.UDT = DateTime.Now;
                            D.UDU = U;
                            D.VAR = 1;
                            db.A_INSUH.Add(H);
                            db.A_INSUD.Add(D);
                            db.SaveChanges();
                            SuccessCount = SuccessCount + 1;
                        }
                        else
                        {
                            result = string.Format("{0} New Policy uploaded \n {1}-{2} Invalid policy number!", SuccessCount, item.INSU_ASET, item.INSU_POLICY);
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        result = string.Format("{0} New Policy uploaded \n {1} Unable to access Asset!", SuccessCount, item.INSU_ASET);
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                result = string.Format("{0} Policy uploaded!", SuccessCount);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [KAuth(112)]
        public ActionResult ViewPolicyDetailBD(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string Sql1 = string.Format(@"select d.insu_id,nvl(sum(d.insu_premium),0)insu_premium,nvl(sum(d.insu_amount),0)insu_amount,nvl(sum(d.insu_vat_amount),0)insu_vat_amount from a_insud d where d.insu_id ='INSU0000000294' group by d.insu_id", id);
            var Detail_Data = db.Database.SqlQuery<cls_ViewPolicyDetailBD>(sql: Sql1).ToList().FirstOrDefault();
            //var Detail_Data = from d in db.A_INSUD
            //                  where d.INSU_ID == id
            //                  group new { d } by d.INSU_ID into g
            //                  select new
            //                  {
            //                      INSUH = g.Key,
            //                      PAmount = g.Sum(x => x.d.INSU_PREMIUM),
            //                      IAmount = g.Sum(x => x.d.INSU_AMOUNT),
            //                      VAmount = g.Sum(x => x.d.INSU_VAT_AMOUNT)
            //                  };

            //ViewBag.INSUH = Detail_Data.FirstOrDefault().INSUH;
            //ViewBag.PAmount = Detail_Data.FirstOrDefault().PAmount;
            //ViewBag.IAmount = Detail_Data.FirstOrDefault().IAmount;
            //ViewBag.VAmount = Detail_Data.FirstOrDefault().VAmount;

            ViewBag.INSUH = Detail_Data.INSU_ID;
            ViewBag.PAmount = Detail_Data.INSU_PREMIUM;
            ViewBag.IAmount = Detail_Data.INSU_AMOUNT;
            ViewBag.VAmount = Detail_Data.INSU_VAT_AMOUNT;


            string Sql2 = string.Format(@"select d.insu_line,d.insu_premium,d.insu_amount ,d.insu_vat_amount,d.insu_date
from a_insud d where d.insu_id ='{0}'", id);

            var VI = db.Database.SqlQuery<InsuranceDetail>(sql: Sql2).ToList();

            //var VI = (from d in db.A_INSUD
            //          where d.INSU_ID == id
            //          select new InsuranceDetail
            //          {
            //              INSU_LINE = d.INSU_LINE,
            //              INSU_PREMIUM = d.INSU_PREMIUM ?? 0,
            //              INSU_AMOUNT = d.INSU_AMOUNT ?? 0,
            //              INSU_VAT_AMOUNT = d.INSU_VAT_AMOUNT ?? 0,
            //              INSU_DATE = d.INSU_DATE
            //          }).ToList();
            return PartialView(VI);
        }

        [KAuth(112)]
        public ActionResult ViewPolicyDetailByVehicleBD(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string Sql = string.Format(@"select i.insu_id,a.aset_code_name,a.aset_capacity,a.aset_mfg_year,
i.insu_policy,i.insu_premium,i.insu_comp,i.insu_reg_start,i.insu_reg_end,i.act
from a_insuh i
join a_aset_data a on i.insu_aset=a.aset_id
where insu_aset=(select distinct( insu_aset) from a_insuh where insu_id='{0}')", id);
            var data = db.Database.SqlQuery<cls_ViewPolicyDetailByVehicleBD>(Sql).ToList();
            return PartialView(data);
        }

        //public static List<Dictionary<string, string>> GetTableRows(DataTable dtData)
        //{
        //    List<Dictionary<string, string>> lstRows = new List<Dictionary<string, string>>();
        //    Dictionary<string, string> dictRow = null;

        //    foreach (DataRow dr in dtData.Rows)
        //    {
        //        dictRow = new Dictionary<string, string>();
        //        foreach (DataColumn col in dtData.Columns)
        //        {
        //            dictRow.Add(col.ColumnName, dr[col].ToString());
        //        }
        //        lstRows.Add(dictRow);
        //    }
        //    return lstRows;
        //}



        //public JsonResult GetData(string sql )
        //{
        //    List<Dictionary<string, string>> dictioneryList = new List<Dictionary<string, string>>();
        //    DataTable dataTable = new DataTable();
        //    //SqlCommand cmd = new SqlCommand();
        //    //SqlDataAdapter adpt = new SqlDataAdapter();
        //    //cmd.Connection = (SqlConnection)db.Database.Connection;
        //    //cmd.CommandText = sql;
        //    //adpt.SelectCommand = cmd;
        //    //dataTable.Reset();
        //    //adpt.Fill(dataTable);

        //    dictioneryList.AddRange(GetTableRows(dataTable));
        //    return Json(dictioneryList, JsonRequestBehavior.AllowGet);
        //}

    }
    public class ImportFile
    {
        public string COL1 { get; set; }
        public string COL2 { get; set; }
        public string COL3 { get; set; }
        public DateTime COL4 { get; set; }
        public string COL5 { get; set; }
    }

    public class cls_ViewPolicyDetailByVehicleBD
    {
        public string insu_id { get; set; }
        public string aset_code_name { get; set; }
        public string aset_capacity { get; set; }
        public Nullable<int> aset_mfg_year { get; set; }
        public string insu_policy { get; set; }
        public Nullable<int> insu_premium { get; set; }
        public string insu_comp { get; set; }
        public Nullable<DateTime> insu_reg_start { get; set; }
        public Nullable<DateTime> insu_reg_end { get; set; }
        public Nullable<Int16> act { get; set; }
    }

    public class cls_ViewPolicyDetailBD
    {
        public string INSU_ID { get; set; }
        public int INSU_PREMIUM { get; set; }
        public int INSU_AMOUNT { get; set; }
        public int INSU_VAT_AMOUNT { get; set; }
    }
}