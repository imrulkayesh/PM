using PM.BLL;
using PM.Model;
using PM.ModelsV;
using PM.MyAuth;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

using System.Data;



namespace PM.Controllers
{
    [Authorize]
    [CustomHttpReffer]
    public class InsuranceManagerController : Controller
    {
        private PMEntities db = new PMEntities();
        ReturnPermission RP = new ReturnPermission();
        GE Sequence = new GE();

        [KAuth(114)]
        public ActionResult Index()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            string sql_1 = @"select a.INSURANCE_TYPE_ID, a.INSURANCE_TYPE_NAME, a.REMARKS from INS_INSURANCE_TYPE a where a.ACT=1";
            var InsuranceTypeData = db.Database.SqlQuery<InsuranceManagerVM>(sql: sql_1);
            ViewBag.Message = "Insurance Type List";
            //var R = InsuranceTypeData.Where(a => a.ACT == true).Take(500).ToList();
            var R = InsuranceTypeData.Take(500).ToList();
            return View(R);
        }

        [KAuth(114)]
        public ActionResult NewInsuranceType()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            return View();
        }
        [KAuth(114)]
        [HttpPost]
        public ActionResult NewInsuranceType(InsuranceManagerVM IT)
        {
            string U = Session["UserID"].ToString();
            string T = Session["UserRole"].ToString();
            if (ModelState.IsValid)
            {
                INS_INSURANCE_TYPE INS_T = new INS_INSURANCE_TYPE();
                INS_T.INSURANCE_TYPE_NAME = IT.INSURANCE_TYPE_NAME;
                INS_T.REMARKS = IT.REMARKS;
                INS_T.ACT = true;
                INS_T.INSURANCE_TYPE_ID = Sequence.GetSeq(21).ToString();
                db.INS_INSRANCE_TYPE.Add(INS_T);
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Saved succefully','success');";
                return RedirectToAction("NewInsuranceType", new { InsType = INS_T.INSURANCE_TYPE_ID });
            }
            TempData["msg"] = "swal('Ohh..','Saved failed','error');";
            return View();
        }
        [KAuth(115)]
        public ActionResult InsuranceCompany()
        {
            string U = Session["UserID"].ToString();
            string T = Session["UserRole"].ToString();
            string sql_1 = @"SELECT a.INSURANCE_COMPANY_ID, a.INS_COMPANY_NAME, a.MOBILE, a.EMAIL, a.ADDRESS FROM INS_INSURANCE_COMPANY a where a.ACT=1";
            var InsCompanyData = db.Database.SqlQuery<InsuranceCompanyVM>(sql: sql_1);
            ViewBag.Message = "Insurance Company List";
            var R = InsCompanyData.Take(500).ToList();
            return View(R);
        }

        [KAuth(115)]
        public ActionResult NewInsuranceCompany()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            return View();
        }

        [KAuth(115)]
        [HttpPost]
        public ActionResult NewInsuranceCompany(InsuranceCompanyVM IC)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (ModelState.IsValid)
            {
                INS_INSURANCE_COMPANY INS_T = new INS_INSURANCE_COMPANY();
                INS_T.INS_COMPANY_NAME = IC.INS_COMPANY_NAME;
                INS_T.ADDRESS = IC.ADDRESS;
                INS_T.MOBILE = IC.MOBILE;
                INS_T.EMAIL = IC.EMAIL;
                INS_T.ACT = true;
                INS_T.INSURANCE_COMPANY_ID = Sequence.GetSeq(22).ToString();
                db.INS_INSURANCE_COMPANY.Add(INS_T);
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Saved succefully','success');";
                return RedirectToAction("NewInsuranceCompany", new { InsType = INS_T.INSURANCE_COMPANY_ID });
            }
            TempData["msg"] = "swal('Ohh..','Saved failed','error');";
            return View();
        }

        [KAuth(116)]
        public ActionResult ClaimType()
        {
            string U = Session["UserID"].ToString();
            string T = Session["UserRole"].ToString();
            string sql_1 = @"select a.CLAIM_TYPE_ID, a.CLAIM_NAME, a.REMARK from INS_CLAIM_TYPE a where a.ACT=1";
            var InsClaimTypeData = db.Database.SqlQuery<ClaimTypeVM>(sql: sql_1);
            ViewBag.Message = "Claim Type List";
            var R = InsClaimTypeData.Take(500).ToList();
            return View(R);
        }

        [KAuth(116)]
        public ActionResult NewClaimType()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            return View();
        }

        [KAuth(116)]
        [HttpPost]
        public ActionResult NewClaimType(ClaimTypeVM CT)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (ModelState.IsValid)
            {
                INS_CLAIM_TYPE INS_C_T = new INS_CLAIM_TYPE();
                INS_C_T.CLAIM_NAME = CT.CLAIM_NAME;
                INS_C_T.REMARK = CT.REMARK;
                INS_C_T.ACT = true;
                INS_C_T.CLAIM_TYPE_ID = Sequence.GetSeq(23).ToString();
                db.INS_CLAIM_TYPE.Add(INS_C_T);
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Saved succefully','success');";
                return RedirectToAction("NewClaimType", new { InsType = INS_C_T.CLAIM_TYPE_ID });
            }
            TempData["msg"] = "swal('Ohh..','Saved failed','error');";
            return View();
        }

        [KAuth(117)]
        public ActionResult ClaimStatus()
        {
            return View();
        }

        [KAuth(117)]
        public ActionResult NewClaimStatus()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            return View();
        }
        [KAuth(118)]
        public ActionResult InsuranceMaster()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            string sql_1 = @"SELECT a.INSURANCE_MST_ID,
                               b.COMPANY_NAME,
                               c.INSURANCE_TYPE_NAME,
                               d.INS_COMPANY_NAME,
                               a.INSURANCE_NUMBER,
                               a.INSURANE_DATE,
                               a.INSURANCE_EXPAIR_DATE,
                               a.INSURANCE_AMOUNT,
                               a.NET_PREMIUM,
                               a.VAT,
                               a.STAMP,
                               ( a.NET_PREMIUM + a.VAT + a.STAMP ) AS TOTAL_PREMIUM,
                               a.KICK_BACK_AMOUNT,
                               a.KICK_BACK_PERCENTAGE,
                               (( a.NET_PREMIUM + a.VAT + a.STAMP ) - a.KICK_BACK_AMOUNT) AS ACTUAL_PAID_AMOUNT,
                               a.KICK_BACK_STATUS,
                               a.KICKBACK_FROM,
                               a.REQUEST_BY,
                               a.DESCRIPTION,
                               a.INSURANCE_STATUS
                          FROM INS_INSURANCE_MST a 
                          LEFT JOIN INS_COMPANY b ON b.INS_COMPANY_ID = a.COMPANY_ID
                          LEFT JOIN INS_INSURANCE_TYPE c ON c.INSURANCE_TYPE_ID = a.INSURANCE_TYPE_ID
                          LEFT JOIN INS_INSURANCE_COMPANY d ON d.INSURANCE_COMPANY_ID = a.INSURANCE_COMPANY_ID
                         WHERE a.ACT = 1 AND a.INSURANCE_STATUS = 1";
            var AsetList = db.Database.SqlQuery<InsuranceMasterVM>(sql: sql_1);

            //if (!string.IsNullOrEmpty(Asset))
            //{
            //    var R = AsetList.Where(a => Asset.Contains(a.ASET_ID)
            //         || Asset.Contains(a.ASET_TAG)
            //         || Asset.Contains(a.ASET_CODE_NAME)
            //         || Asset.Contains(a.DEPARTMENT_V))
            //        .ToList();
            //    if (!string.IsNullOrEmpty(Operation))
            //    {
            //        ViewBag.Message = "Search Result";
            //    }
            //    return View(R);
            //}
            //else
            //{
            ViewBag.Message = "Insurance List";
            //var R = AsetList.Where(a => a.ACT == true).Take(500).ToList();
            var R = AsetList.Take(500).ToList();
            return View(R);
            //}

            //return View();
        }
        
        [KAuth(121)]
        public ActionResult InsuranceList()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            string sql_1 = @"SELECT a.INSURANCE_MST_ID,
                               b.COMPANY_NAME,
                               c.INSURANCE_TYPE_NAME,
                               d.INS_COMPANY_NAME,
                               a.INSURANCE_NUMBER,
                               a.INSURANE_DATE,
                               a.INSURANCE_EXPAIR_DATE,
                               a.INSURANCE_AMOUNT,
                               a.NET_PREMIUM,
                               a.VAT,
                               a.STAMP,
                               ( a.NET_PREMIUM + a.VAT + a.STAMP ) AS TOTAL_PREMIUM,
                               a.KICK_BACK_AMOUNT,
                               a.KICK_BACK_PERCENTAGE,
                               (( a.NET_PREMIUM + a.VAT + a.STAMP ) - a.KICK_BACK_AMOUNT) AS ACTUAL_PAID_AMOUNT,
                               a.KICK_BACK_STATUS,
                               a.KICKBACK_FROM,
                               a.REQUEST_BY,
                               a.DESCRIPTION,
                               a.INSURANCE_STATUS
                          FROM INS_INSURANCE_MST a 
                          LEFT JOIN INS_COMPANY b ON b.INS_COMPANY_ID = a.COMPANY_ID
                          LEFT JOIN INS_INSURANCE_TYPE c ON c.INSURANCE_TYPE_ID = a.INSURANCE_TYPE_ID
                          LEFT JOIN INS_INSURANCE_COMPANY d ON d.INSURANCE_COMPANY_ID = a.INSURANCE_COMPANY_ID
                         WHERE a.ACT = 1 AND a.INSURANCE_STATUS = 1";
            var AsetList = db.Database.SqlQuery<InsuranceMasterVM>(sql: sql_1);

            ViewBag.Message = "Insurance List";
            var R = AsetList.Take(500).ToList();
            return View(R);

        }

        [KAuth(118)]
        public ActionResult NewInsuranceMaster()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            //ViewBag.COMPANY_ID = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
            ViewBag.INSURANCE_TYPE_ID = new SelectList(RP.InsuranceTypeList(), "INSURANCE_TYPE_ID", "INSURANCE_TYPE_NAME");
            ViewBag.INSURANCE_COMPANY_ID = new SelectList(RP.InsuranceCompanyList(), "INSURANCE_COMPANY_ID", "INS_COMPANY_NAME");
            ViewBag.COMPANY_ID = new SelectList(RP.CompanyList(), "INS_COMPANY_ID", "COMPANY_NAME");
            return View();
        }

        [KAuth(118)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewInsuranceMaster(InsuranceMasterVM IM)
        {
            if (Session["UserId"] == null || Session["UserRole"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();


            if (IM.FileX != null && IM.FileX.ContentLength > 0)
            {
                int fileSize = IM.FileX.ContentLength;
                if (fileSize <= 0 || fileSize > 8100000)
                {
                    TempData["msg"] = "swal('Opps..','File is too larger, Max file size 2MB','warning');";
                    ViewBag.COMPANY_ID = new SelectList(RP.CompanyList(), "INS_COMPANY_ID", "COMPANY_NAME");
                    ViewBag.INSURANCE_TYPE_ID = new SelectList(RP.InsuranceTypeList(), "INSURANCE_TYPE_ID", "INSURANCE_TYPE_NAME");
                    ViewBag.INSURANCE_COMPANY_ID = new SelectList(RP.InsuranceCompanyList(), "INSURANCE_COMPANY_ID", "INS_COMPANY_NAME");
                    return View();
                }
            }


            if (ModelState.IsValid)
            {
                INS_INSURANCE_MST Imd = new INS_INSURANCE_MST();
                Imd.COMPANY_ID = IM.COMPANY_ID;
                Imd.INSURED_ITEM = IM.INSURED_ITEM;
                Imd.BENIFICIARY_DETAILS = IM.BENIFICIARY_DETAILS;
                Imd.OCCUPATION = IM.OCCUPATION;
                Imd.INSURANCE_TYPE_ID = IM.INSURANCE_TYPE_ID;
                Imd.INSURANCE_COMPANY_ID = IM.INSURANCE_COMPANY_ID;
                Imd.INSURANCE_NUMBER = IM.INSURANCE_NUMBER;
                Imd.INSURANE_DATE = IM.INSURANE_DATE;
                Imd.INSURANCE_EXPAIR_DATE = IM.INSURANCE_EXPAIR_DATE;
                //Imd.INSURANCE_AMOUNT = IM.INSURANCE_AMOUNT;
                //Imd.NET_PREMIUM = IM.NET_PREMIUM;
                //Imd.VAT = IM.VAT;
                //Imd.STAMP = IM.STAMP;
                Imd.INSURANCE_AMOUNT = IM.INSURANCE_AMOUNT ?? 0;
                Imd.NET_PREMIUM = IM.NET_PREMIUM ?? 0;
                Imd.VAT = IM.VAT ?? 0;
                Imd.STAMP = IM.STAMP ?? 0;

                Imd.REQUEST_BY = IM.REQUEST_BY;
                Imd.DESCRIPTION = IM.DESCRIPTION;
                Imd.INSURANCE_IMAGE = "~/";
                Imd.INSURANCE_STATUS = true;
                Imd.ACT = true;
                Imd.CDT = DateTime.Now;
                Imd.CDU = U;
                Imd.VAR = 1;
                Imd.CLAIM_STATUS = false;
                //Save Insurance Data
                Imd.INSURANCE_MST_ID = Sequence.GetSeq(18).ToString();
                db.INS_INSURANCE_MST.Add(Imd);

                if (IM.FileX != null && IM.FileX.ContentLength > 0)
                {

                    INS_INSURANCE_IMG I = new INS_INSURANCE_IMG();
                    I.INSURANCE_IMG_ID = Guid.NewGuid().ToString();
                    I.IMG_NAME = IM.FileX.FileName;
                    I.ACT = true;
                    I.IMG_PATH = Sequence.GetImgPath(106);
                    I.INSURANCE_MST_ID = Imd.INSURANCE_MST_ID; //reminder Id
                    db.INS_INSURANCE_IMG.Add(I);
                    //upload file to server
                    var path = System.IO.Path.Combine(Server.MapPath(I.IMG_PATH), I.INSURANCE_IMG_ID);
                    IM.FileX.SaveAs(path);
                }




                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Saved succefully','success');";
                return RedirectToAction("NewInsuranceMaster", new { Asset = Imd.INSURANCE_MST_ID });
            }

            ViewBag.COMPANY_ID = new SelectList(RP.CompanyList(), "INS_COMPANY_ID", "COMPANY_NAME");
            ViewBag.INSURANCE_TYPE_ID = new SelectList(RP.InsuranceTypeList(), "INSURANCE_TYPE_ID", "INSURANCE_TYPE_NAME");
            ViewBag.INSURANCE_COMPANY_ID = new SelectList(RP.InsuranceCompanyList(), "INSURANCE_COMPANY_ID", "INS_COMPANY_NAME");
            TempData["msg"] = "swal('Ohh..','Saved failed','error');";
            return View(IM);
        }

        [KAuth(118)]
        [HttpGet]
        public ActionResult Update(string id)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (id == null)
            {
                TempData["msg"] = "swal('Opss..','Try with valid data','info');";
                return RedirectToAction("InsuranceMaster");
            }
            INS_INSURANCE_MST INS = db.INS_INSURANCE_MST.Find(id);
            if (INS == null)
            {
                TempData["msg"] = "swal('Ohh..','No Insurance found','warning');";
                return RedirectToAction("InsuranceMaster");
            }
            InsuranceUpdateVM INUDT = new InsuranceUpdateVM();

            INUDT.INSURANCE_MST_ID = INS.INSURANCE_MST_ID;
            INUDT.COMPANY_ID = INS.COMPANY_ID;
            INUDT.BENIFICIARY_DETAILS = INS.BENIFICIARY_DETAILS;
            INUDT.INSURANCE_TYPE_ID = INS.INSURANCE_TYPE_ID;
            INUDT.INSURANCE_COMPANY_ID = INS.INSURANCE_COMPANY_ID;
            INUDT.INSURED_ITEM = INS.INSURED_ITEM;
            INUDT.OCCUPATION = INS.OCCUPATION;
            INUDT.INSURANCE_NUMBER = INS.INSURANCE_NUMBER;
            //INUDT.INSURANE_DATE = INS.INSURANE_DATE;
            //INUDT.INSURANCE_EXPAIR_DATE = INS.INSURANCE_EXPAIR_DATE;
            INUDT.INSURANCE_AMOUNT = INS.INSURANCE_AMOUNT;
            INUDT.NET_PREMIUM = INS.NET_PREMIUM;
            INUDT.VAT = INS.VAT;
            INUDT.STAMP = INS.STAMP;
            INUDT.REQUEST_BY = INS.REQUEST_BY;
            INUDT.DESCRIPTION = INS.DESCRIPTION;



            ViewBag.COMPANY_ID = new SelectList(RP.CompanyList(), "INS_COMPANY_ID", "COMPANY_NAME");
            ViewBag.INSURANCE_TYPE_ID = new SelectList(RP.InsuranceTypeList(), "INSURANCE_TYPE_ID", "INSURANCE_TYPE_NAME");
            ViewBag.INSURANCE_COMPANY_ID = new SelectList(RP.InsuranceCompanyList(), "INSURANCE_COMPANY_ID", "INS_COMPANY_NAME");

            return PartialView("_Update", INUDT);
        }

        [KAuth(118)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(InsuranceUpdateVM INSUDT)
        {
            if (ModelState.IsValid)
            {
                string U = Session["UserId"].ToString();
                if (INSUDT.FileX != null && INSUDT.FileX.ContentLength > 0)
                {

                    int fileSize = INSUDT.FileX.ContentLength;
                    if (fileSize <= 0 || fileSize > 8100000)
                    {
                        TempData["msg"] = "swal('Opps..','File is too larger, Max file size 2MB','warning');";
                        ViewBag.COMPANY_ID = new SelectList(RP.CompanyList(), "INS_COMPANY_ID", "COMPANY_NAME");
                        ViewBag.INSURANCE_TYPE_ID = new SelectList(RP.InsuranceTypeList(), "INSURANCE_TYPE_ID", "INSURANCE_TYPE_NAME");
                        ViewBag.INSURANCE_COMPANY_ID = new SelectList(RP.InsuranceCompanyList(), "INSURANCE_COMPANY_ID", "INS_COMPANY_NAME");
                        return View();
                    }
                }

                INS_INSURANCE_MST IMU = db.INS_INSURANCE_MST.Find(INSUDT.INSURANCE_MST_ID);
                IMU.COMPANY_ID = INSUDT.COMPANY_ID;
                IMU.BENIFICIARY_DETAILS = INSUDT.BENIFICIARY_DETAILS;
                IMU.INSURANCE_TYPE_ID = INSUDT.INSURANCE_TYPE_ID;
                IMU.INSURANCE_COMPANY_ID = INSUDT.INSURANCE_COMPANY_ID;
                IMU.INSURED_ITEM = INSUDT.INSURED_ITEM;
                IMU.OCCUPATION = INSUDT.OCCUPATION;
                IMU.INSURANCE_NUMBER = INSUDT.INSURANCE_NUMBER;

                if (INSUDT.INSURANE_DATE != null)
                {
                    IMU.INSURANE_DATE = INSUDT.INSURANE_DATE;
                }
                if (INSUDT.INSURANCE_EXPAIR_DATE != null)
                {
                    IMU.INSURANCE_EXPAIR_DATE = INSUDT.INSURANCE_EXPAIR_DATE;
                }

                IMU.INSURANCE_AMOUNT = INSUDT.INSURANCE_AMOUNT;
                IMU.NET_PREMIUM = INSUDT.NET_PREMIUM;
                IMU.VAT = INSUDT.VAT;
                IMU.STAMP = INSUDT.STAMP;
                IMU.REQUEST_BY = INSUDT.REQUEST_BY;
                IMU.DESCRIPTION = INSUDT.DESCRIPTION;
                IMU.INSURANCE_MST_ID = INSUDT.INSURANCE_MST_ID;

                if (INSUDT.FileX != null && INSUDT.FileX.ContentLength > 0)
                {
                    INS_INSURANCE_IMG I = new INS_INSURANCE_IMG();
                    I.INSURANCE_IMG_ID = Guid.NewGuid().ToString();
                    I.IMG_NAME = INSUDT.FileX.FileName;
                    I.ACT = true;
                    I.CDT = DateTime.Now;
                    I.IMG_PATH = Sequence.GetImgPath(106);
                    I.INSURANCE_MST_ID = INSUDT.INSURANCE_MST_ID; 
                    db.INS_INSURANCE_IMG.Add(I);
                    //upload file to server
                    var path = System.IO.Path.Combine(Server.MapPath(I.IMG_PATH), I.INSURANCE_IMG_ID);
                    INSUDT.FileX.SaveAs(path);
                }
                IMU.UDT = DateTime.Now;
                IMU.UDU = U;
                db.Entry(IMU).State = EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Insurance data updated','success');";
                return RedirectToAction("InsuranceMaster", new { InsuranceId = INSUDT.INSURANCE_MST_ID });
            }
            TempData["msg"] = "swal('Ohh..','Data not updated','warning');";
            return RedirectToAction("InsuranceMaster", new { InsuranceId = INSUDT.INSURANCE_MST_ID });
        }

        [KAuth(119)]
        public ActionResult InsuranceDetail()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            return View();
        }

        [KAuth(118)]
        [HttpGet]
        public ActionResult KickBack(string id)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (id == null)
            {
                TempData["msg"] = "swal('Opss..','Try with valid data','info');";
                return RedirectToAction("Index");
            }
            INS_INSURANCE_MST ad = db.INS_INSURANCE_MST.Find(id);
            if (ad == null)
            {
                TempData["msg"] = "swal('Ohh..','No Insurance found','warning');";
                return RedirectToAction("Index");
            }

            KickBackVM KB = new KickBackVM();
            KB.INSURANCE_MST_ID = ad.INSURANCE_MST_ID;
            KB.INSURANCE_NUMBER = ad.INSURANCE_NUMBER;
            KB.KICK_BACK_PERCENTAGE = ad.KICK_BACK_PERCENTAGE;
            KB.KICKBACK_FROM = ad.KICKBACK_FROM;
            ViewBag.KICK_BACK_STATUS = new SelectList(RP.KickBackStatusList(), "KICK_BACK_STATUS_ID", "KICK_BACK_STATUS_NAME", ad.KICK_BACK_STATUS);
            //ViewBag.KICK_BACK_STATUS = new SelectList(RP.KickBackStatusById(ad.KICK_BACK_STATUS), "KICK_BACK_STATUS_ID", "KICK_BACK_STATUS_NAME");
            return PartialView("_KickBack", KB);
        }
        [KAuth(118)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KickBack(KickBackVM KB)
        {
            if (ModelState.IsValid)
            {
                string U = Session["UserId"].ToString();
                INS_INSURANCE_MST InsM = db.INS_INSURANCE_MST.Find(KB.INSURANCE_MST_ID);
                InsM.KICK_BACK_STATUS = KB.KICK_BACK_STATUS;
                InsM.KICK_BACK_PERCENTAGE = KB.KICK_BACK_PERCENTAGE;
                InsM.KICKBACK_FROM = KB.KICKBACK_FROM;

                if (KB.KICKBACK_FROM == "1")
                {
                    InsM.KICK_BACK_AMOUNT = (KB.KICK_BACK_PERCENTAGE * InsM.NET_PREMIUM) / 100;
                }
                else
                {
                    //InsM.KICK_BACK_AMOUNT = (KB.KICK_BACK_PERCENTAGE * (InsM.NET_PREMIUM + InsM.VAT + InsM.STAMP)) / 100;
                    decimal total = InsM.NET_PREMIUM + InsM.VAT + InsM.STAMP;
                    Nullable<decimal> total_amount = KB.KICK_BACK_PERCENTAGE * total;
                    InsM.KICK_BACK_AMOUNT = total_amount / 100;
                }

                InsM.UDT = DateTime.Now;
                InsM.UDU = U;
                InsM.VAR = KB.VAR + 1;
                db.Entry(InsM).State = EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Data updated','success');";
                return RedirectToAction("InsuranceMaster", new { InsuranceId = KB.INSURANCE_MST_ID });
            }
            TempData["msg"] = "swal('Ohh..','Data not updated','warning');";
            return RedirectToAction("InsuranceMaster", new { InsuranceId = KB.INSURANCE_MST_ID });
        }

        [KAuth(118)]
        public ActionResult NewClaim()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            string sql_1 = @"SELECT a.INSURANCE_MST_ID,
                               b.COMPANY_NAME,
                               c.INSURANCE_TYPE_NAME,
                               d.INS_COMPANY_NAME,
                               a.INSURANCE_NUMBER,
                               a.INSURANE_DATE,
                               a.INSURANCE_EXPAIR_DATE,
                               a.INSURANCE_AMOUNT,
                               a.NET_PREMIUM,
                               a.VAT,
                               a.STAMP,
                               ( a.NET_PREMIUM + a.VAT + a.STAMP ) AS TOTAL_PREMIUM,
                               a.KICK_BACK_AMOUNT,
                               a.KICK_BACK_PERCENTAGE,
                               (( a.NET_PREMIUM + a.VAT + a.STAMP ) - a.KICK_BACK_AMOUNT) AS ACTUAL_PAID_AMOUNT,
                               a.KICK_BACK_STATUS,
                               a.KICKBACK_FROM,
                               a.REQUEST_BY,
                               a.DESCRIPTION,
                               a.INSURANCE_STATUS
                          FROM INS_INSURANCE_MST a 
                          LEFT JOIN INS_COMPANY b ON b.INS_COMPANY_ID = a.COMPANY_ID
                          LEFT JOIN INS_INSURANCE_TYPE c ON c.INSURANCE_TYPE_ID = a.INSURANCE_TYPE_ID
                          LEFT JOIN INS_INSURANCE_COMPANY d ON d.INSURANCE_COMPANY_ID = a.INSURANCE_COMPANY_ID
                         WHERE a.ACT = 1 AND a.INSURANCE_STATUS = 1";
            var AsetList = db.Database.SqlQuery<InsuranceMasterVM>(sql: sql_1);

            //if (!string.IsNullOrEmpty(Asset))
            //{
            //    var R = AsetList.Where(a => Asset.Contains(a.ASET_ID)
            //         || Asset.Contains(a.ASET_TAG)
            //         || Asset.Contains(a.ASET_CODE_NAME)
            //         || Asset.Contains(a.DEPARTMENT_V))
            //        .ToList();
            //    if (!string.IsNullOrEmpty(Operation))
            //    {
            //        ViewBag.Message = "Search Result";
            //    }
            //    return View(R);
            //}
            //else
            //{
            ViewBag.Message = "Claim List";
            //var R = AsetList.Where(a => a.ACT == true).Take(500).ToList();
            var R = AsetList.Take(500).ToList();
            return View(R);
            //}

            //return View();
        }

        [KAuth(118)]
        public ActionResult Claim(string id)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            if (id == null)
            {
                TempData["msg"] = "swal('Opss..','Try with valid data','info');";
                return RedirectToAction("Index");
            }
            INS_INSURANCE_MST ad = db.INS_INSURANCE_MST.Find(id);
            if (ad == null)
            {
                TempData["msg"] = "swal('Ohh..','No Insurance found','warning');";
                return RedirectToAction("Index");
            }

            ClaimVM C = new ClaimVM();
            C.INSURANCE_MST_ID = ad.INSURANCE_MST_ID;
            C.INSURANCE_NUMBER = ad.INSURANCE_NUMBER;
            C.INSURANCE_AMOUNT = ad.INSURANCE_AMOUNT;
            ViewBag.CLAIM_TYPE_LIST = new SelectList(RP.ClaimTypeList(), "CLAIM_TYPE_ID", "CLAIM_NAME");
            ViewBag.CLAIM_STATUS_LIST = new SelectList(RP.ClaimStatusList(), "CLAIM_STATUS_ID", "CLAIM_STATUS_NAME");

            return PartialView("_Claim", C);
        }
        [KAuth(118)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Claim(ClaimVM C)
        {
            if (ModelState.IsValid)
            {
                string U = Session["UserId"].ToString();

                INS_CLAIM INS_CLM = new INS_CLAIM();
                INS_CLM.INSURANCE_MST_ID = C.INSURANCE_MST_ID;
                INS_CLM.CLAIM_TYPE_ID = C.CLAIM_TYPE_LIST;
                INS_CLM.CLAIM_DATE = C.CLAIM_DATE;
                INS_CLM.CLAIM_LOCATION = C.CLAIM_LOCATION;
                INS_CLM.LOSS_ITEM = C.LOSS_ITEM;
                INS_CLM.CLAIM_AMOUNT = C.CLAIM_AMOUNT;
                INS_CLM.CLAIM_STATUS_ID = C.CLAIM_STATUS_LIST;
                INS_CLM.INS_AMT_LOSS_ITEM = C.INS_AMT_LOSS_ITEM;
                INS_CLM.REMARK = C.REMARK;

                INS_CLM.ACT = true;
                INS_CLM.CDT = DateTime.Now;
                INS_CLM.CDU = U;
                INS_CLM.CLAIM_ID = Sequence.GetSeq(19).ToString();
                db.INS_CLAIM.Add(INS_CLM);

                //INS_INSURANCE_MST IM = new INS_INSURANCE_MST();
                INS_INSURANCE_MST IM = db.INS_INSURANCE_MST.Find(C.INSURANCE_MST_ID);
                IM.CLAIM_STATUS = true;
                db.Entry(IM).State = EntityState.Modified;

                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Claim Saved succefully','success');";
                return RedirectToAction("NewClaim", new { Insurance = INS_CLM.CLAIM_ID });
            }
            TempData["msg"] = "swal('Ohh..','Saved failed','error');";
            return View(C);
        }

        [KAuth(118)]
        [HttpGet]
        public ActionResult Delete(string id)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (id == null)
            {
                TempData["msg"] = "swal('Opss..','Try with valid data','info');";
                return RedirectToAction("Index");
            }
            INS_INSURANCE_MST ad = db.INS_INSURANCE_MST.Find(id);
            if (ad == null)
            {
                TempData["msg"] = "swal('Ohh..','No Insurance found','warning');";
                return RedirectToAction("Index");
            }

            DeleteVM D = new DeleteVM();
            D.INSURANCE_MST_ID = ad.INSURANCE_MST_ID;
            D.INSURANCE_NUMBER = ad.INSURANCE_NUMBER;

            ViewBag.KICK_BACK_STATUS = new SelectList(RP.KickBackStatusList(), "KICK_BACK_STATUS_ID", "KICK_BACK_STATUS_NAME", ad.KICK_BACK_STATUS);
            //ViewBag.KICK_BACK_STATUS = new SelectList(RP.KickBackStatusById(ad.KICK_BACK_STATUS), "KICK_BACK_STATUS_ID", "KICK_BACK_STATUS_NAME");
            return PartialView("Delete", D);
        }
        [KAuth(118)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(DeleteVM D)
        {
            string U = Session["UserId"].ToString();
            if (ModelState.IsValid)
            {
                INS_INSURANCE_MST InsM = db.INS_INSURANCE_MST.Find(D.INSURANCE_MST_ID);
                InsM.ACT = false;
                InsM.UDU = U;
                InsM.UDT = DateTime.Now;
                db.Entry(InsM).State = EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Data Deleted.','success');";
                return RedirectToAction("InsuranceMaster", new { InsuranceId = D.INSURANCE_MST_ID });
            }
            TempData["msg"] = "swal('Ohh..','Data not updated','warning');";
            return RedirectToAction("InsuranceMaster", new { InsuranceId = D.INSURANCE_MST_ID });
        }

        [KAuth(120)]
        public ActionResult ClaimList()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            string sql_1 = @" SELECT CT.CLAIM_NAME,
               C.CLAIM_DATE,
               I.INSURANCE_NUMBER,
               B.COMPANY_NAME,
               C.CLAIM_LOCATION,
               C.LOSS_ITEM,
               C.CLAIM_AMOUNT,
               C.INS_AMT_LOSS_ITEM,
               CS.CLAIM_STATUS_NAME,
               IT.INSURANCE_TYPE_NAME,
               IC.INS_COMPANY_NAME,
               C.REMARK,
               I.INSURANCE_AMOUNT ,
               C.CLAIM_ID,
               C.INSURANCE_MST_ID,
               C.CLAIM_RECEIVE_AMOUNT,
               C.CLAIM_STATUS_ID
          FROM INS_CLAIM  C
               LEFT JOIN INS_CLAIM_TYPE CT ON C.CLAIM_TYPE_ID = CT.CLAIM_TYPE_ID
               LEFT JOIN INS_INSURANCE_MST I ON C.INSURANCE_MST_ID = I.INSURANCE_MST_ID
               LEFT JOIN INS_CLAIM_STATUS CS ON C.CLAIM_STATUS_ID = CS.CLAIM_STATUS_ID
               INNER JOIN INS_COMPANY B ON I.COMPANY_ID = B.INS_COMPANY_ID
               INNER JOIN INS_INSURANCE_TYPE IT ON I.INSURANCE_TYPE_ID = IT.INSURANCE_TYPE_ID
               INNER JOIN INS_INSURANCE_COMPANY IC ON I.INSURANCE_COMPANY_ID = IC.INSURANCE_COMPANY_ID
               WHERE C.ACT=1 AND I.ACT=1 AND C.CLAIM_STATUS_ID in (1,3)";
            var ClaimList = db.Database.SqlQuery<ClaimVM>(sql: sql_1);

            ViewBag.Message = "Claim List";
            var R = ClaimList.Take(500).ToList();
            return View(R);
        }

        [KAuth(120)]
        public ActionResult ClaimReceive(string id)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            if (id == null)
            {
                TempData["msg"] = "swal('Opss..','Try with valid data','info');";
                return RedirectToAction("Index");
            }

            INS_CLAIM ad = db.INS_CLAIM.Find(id);
            if (ad == null)
            {
                TempData["msg"] = "swal('Ohh..','No Insurance found','warning');";
                return RedirectToAction("Index");
            }

            //ClaimVM C = new ClaimVM();
            ClaimReceive CR = new ClaimReceive();
            CR.CLAIM_AMOUNT = ad.CLAIM_AMOUNT;
            CR.CLAIM_ID = ad.CLAIM_ID;
            ViewBag.CLAIM_STATUS_LIST = new SelectList(RP.ClaimStatusList(), "CLAIM_STATUS_ID", "CLAIM_STATUS_NAME");

            return PartialView("_ClaimReceive", CR);
        }

        [KAuth(120)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ClaimReceive(ClaimReceive C)
        {
            if (ModelState.IsValid)
            {
                string U = Session["UserId"].ToString();

                INS_CLAIM_RECEIVE CLM_R = new INS_CLAIM_RECEIVE();
                CLM_R.RECEIVE_AMOUNT = C.RECEIVE_AMOUNT;
                CLM_R.RECEIVEREMARK = C.RECEIVEREMARK;
                CLM_R.CLAIM_ID = C.CLAIM_ID;
                CLM_R.RECEIVE_DATE = C.RECEIVE_DATE;

                CLM_R.ACT = true;
                CLM_R.CDT = DateTime.Now;
                CLM_R.CDU = U;
                CLM_R.CLAIM_RECEIVE_ID = Sequence.GetSeq(20).ToString();
                db.INS_CLAIM_RECEIVE.Add(CLM_R);

                INS_CLAIM IM = db.INS_CLAIM.Find(C.CLAIM_ID);
                IM.CLAIM_STATUS_ID = C.CLAIM_STATUS_LIST;
                IM.CLAIM_RECEIVE_AMOUNT = (IM.CLAIM_RECEIVE_AMOUNT + C.RECEIVE_AMOUNT);
                db.Entry(IM).State = EntityState.Modified;

                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Claim Received Succefully','success');";
                return RedirectToAction("ClaimList", new { Insurance = CLM_R.CLAIM_ID });
            }
            TempData["msg"] = "swal('Ohh..','Saved failed','error');";
            return View(C);
        }

        [KAuth(3102)]
        public ActionResult AllClaimList()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            string sql_1 = @" SELECT CT.CLAIM_NAME,
               C.CLAIM_DATE,
               I.INSURANCE_NUMBER,
               B.COMPANY_NAME,
               C.CLAIM_LOCATION,
               C.LOSS_ITEM,
               C.CLAIM_AMOUNT,
               C.INS_AMT_LOSS_ITEM,
               CS.CLAIM_STATUS_NAME,
               IT.INSURANCE_TYPE_NAME,
               IC.INS_COMPANY_NAME,
               C.REMARK,
               I.INSURANCE_AMOUNT ,
               C.CLAIM_ID,
               C.INSURANCE_MST_ID,
               C.CLAIM_RECEIVE_AMOUNT,
               C.CLAIM_STATUS_ID
          FROM INS_CLAIM  C
               LEFT JOIN INS_CLAIM_TYPE CT ON C.CLAIM_TYPE_ID = CT.CLAIM_TYPE_ID
               LEFT JOIN INS_INSURANCE_MST I ON C.INSURANCE_MST_ID = I.INSURANCE_MST_ID
               LEFT JOIN INS_CLAIM_STATUS CS ON C.CLAIM_STATUS_ID = CS.CLAIM_STATUS_ID
               INNER JOIN INS_COMPANY B ON I.COMPANY_ID = B.INS_COMPANY_ID
               INNER JOIN INS_INSURANCE_TYPE IT ON I.INSURANCE_TYPE_ID = IT.INSURANCE_TYPE_ID
               INNER JOIN INS_INSURANCE_COMPANY IC ON I.INSURANCE_COMPANY_ID = IC.INSURANCE_COMPANY_ID
               WHERE C.ACT=1 AND I.ACT=1";
            var ClaimList = db.Database.SqlQuery<ClaimVM>(sql: sql_1);

            ViewBag.Message = "All Claim List";
            var R = ClaimList.Take(500).ToList();
            return View(R);
        }

        public ActionResult DownloadAttachFile(string id)
        {
            var dbDATA = db.INS_INSURANCE_IMG
                .Where(a => a.INSURANCE_MST_ID == id)
                .OrderByDescending(a => a.CDT)
                .ToList();

            if (dbDATA == null || !dbDATA.Any())
            {
                TempData["msg"] = "swal('Oops..','File not found','warning');";
                return Redirect(Request.UrlReferrer.ToString());
            }

            var latestImage = dbDATA.FirstOrDefault();
            if (latestImage == null)
            {
                TempData["msg"] = "swal('Oops..','No latest image found','warning');";
                return Redirect(Request.UrlReferrer.ToString());
            }

            var Upload_Path = latestImage.IMG_PATH;
            var GetFileGUIDname = latestImage.INSURANCE_IMG_ID;
            var GetFileOrgName = latestImage.IMG_NAME;
            var CombinePath = System.IO.Path.Combine(Server.MapPath(Upload_Path), GetFileGUIDname);

            try
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(CombinePath);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, GetFileOrgName);
            }
            catch (Exception ex)
            {
                TempData["msg"] = "swal('Ohh..','Error in File: " + ex.Message + "','error');";
                return Redirect(Request.UrlReferrer.ToString());
            }


        }

    }
}