using Newtonsoft.Json;
using PM.BLL;
using PM.Model;
using PM.ModelsV;
using PM.MyAuth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace PM.Controllers
{
    [Authorize]
    [CustomHttpReffer]
    public class AssetManagerController : Controller
    {
        private PMEntities db = new PMEntities();
        ReturnPermission RP = new ReturnPermission();
        GE Sequence = new GE();
        private JsonReaderPMController PMC = new JsonReaderPMController();
        private PMExcelDataLoader EDL = new PMExcelDataLoader();

        [KAuth(101)]
        public ActionResult Index(string Asset, string Operation)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            #region OLD
            //List<string> MyAssetList = RP.MyAssetListString(UserId: U, UserType: T);

            //var AsetList = from a in db.A_ASET_DATA
            //               where a.ACT == true
            //               && MyAssetList.Contains(a.ASET_ID)
            //               select new CarVM
            //               {
            //                   ASET_ID = a.ASET_ID,
            //                   ASET_TAG = a.ASET_TAG,
            //                   ASET_CODE_NAME = a.ASET_CODE_NAME,
            //                   ASET_AREA = a.A_AREA.AREA_NAME,//area
            //                   ASET_BODY = a.A_BODY.BODY_NAME,//body
            //                   ASET_BRAND = a.A_BRAND.BRAND_NAME,//body
            //                   ASET_FUEL = a.A_FUEL.FUEL_NAME,//fuel
            //                   ASET_COLOR = a.A_COLOR.COLOR_NAME,//color
            //                   ASET_GROUP = a.A_GROUP.GROUP_NAME,//group
            //                   ASET_MODEL = a.ASET_MODEL,
            //                   ASET_PO_ENGINE = a.ASET_PO_ENGINE,
            //                   ASET_GRN_CHASIS = a.ASET_GRN_CHASIS,
            //                   ASET_CAPACITY = a.ASET_CAPACITY,
            //                   ASET_PRICE = a.ASET_PRICE,
            //                   ASET_REG_DATE = a.ASET_REG_DATE,
            //                   ASET_HEIGHT = a.ASET_HEIGHT,
            //                   ASET_WEIGHT = a.ASET_WEIGHT,
            //                   ASET_MFG_YEAR = a.ASET_MFG_YEAR,
            //                   ASET_IMAGE = a.ASET_IMAGE,
            //                   ASET_NOTE = a.ASET_NOTE,
            //                   ASET_USED = a.ASET_USED,
            //                   ACT = a.ACT,
            //                   DEPARTMENT_V = a.A_DEPARTMENT.DEPARTMENT_NAME,//depeartment
            //                   BUSINESS_V = a.A_DEPARTMENT.A_BUSINESS.BUSINESS_NAME,
            //                   COMPANY_V = a.A_DEPARTMENT.A_BUSINESS.A_COMPANY.COMPANY_NAME
            //               }; 
            #endregion

            string sql_1 = @"SELECT A.ASET_ID,A.ASET_TAG,A.ASET_CODE_NAME,AREA.AREA_NAME ASET_AREA,BOD.BODY_NAME ASET_BODY,BRAND.BRAND_NAME ASET_BRAND,
                            FUEL.FUEL_NAME ASET_FUEL,CLO.COLOR_NAME ASET_COLOR,GRP.GROUP_NAME ASET_GROUP,A.ASET_MODEL,A.ASET_PO_ENGINE,A.ASET_GRN_CHASIS,
                            A.ASET_PRICE,A.ASET_CAPACITY,A.ASET_PRICE,A.ASET_REG_DATE,A.ASET_HEIGHT,A.ASET_WEIGHT,A.ASET_MFG_YEAR,A.ASET_IMAGE,A.ASET_NOTE,A.ASET_USED,
                            A.ACT,DEP.DEPARTMENT_NAME DEPARTMENT_V,BUSS.BUSINESS_NAME BUSINESS_V,COMP.COMPANY_NAME COMPANY_V
                            FROM A_ASET_DATA A --ON T6.ASET_ID=T.AU_ASET AND T6.ACT=1
                            INNER JOIN A_USERS_GROUP T5 ON A.ASET_GROUP=T5.UG_GROUP AND T5.UG_USER='" + U + @"' AND T5.ACT=1
                            INNER JOIN A_DEPARTMENT T7 ON A.ASET_DEPARTMENT=T7.DEPARTMENT_ID AND T7.ACT=1
                            INNER JOIN A_BUSINESS T8 ON T7.DEPARTMENT_BUSINESS=T8.BUSINESS_ID AND T8.ACT=1
                            INNER JOIN A_USERS_BUS T9 ON T8.BUSINESS_ID =T9.UB_BUSINESS AND T9.UB_USER=T5.UG_USER AND T9.ACT=1 
                            INNER JOIN (SELECT DISTINCT(AREA_ID) FROM (SELECT T.AREA_ID
                            FROM A_AREA T
                            INNER JOIN A_THANA T1 ON T.AREA_THANA=T1.THANA_ID AND T1.ACT=1
                            INNER JOIN A_DISTRICT T2 ON T1.THANA_DISTRICT=T2.DISTRICT_ID AND T2.ACT=1
                            INNER JOIN A_USERS_COUNTRY T3 ON T2.DISTRICT_COUNTRY=T3.COUNTRY_ID AND T3.ACT=1 AND T3.USER_TEXT='" + U + @"'
                            WHERE T.ACT=1
                            UNION ALL
                            SELECT T.UA_AREA
                            FROM A_USERS_AREA T WHERE T.ACT=1 AND T.UA_USER='" + U + @"'
                            )) T10 ON A.ASET_AREA=T10.AREA_ID                          
                            INNER JOIN PM.A_AREA AREA ON AREA.AREA_ID = A.ASET_AREA
                            INNER JOIN PM.A_BODY BOD ON A.ASET_BODY = BOD.BODY_ID
                            INNER JOIN PM.A_BRAND BRAND ON A.ASET_BRAND = BRAND.BRAND_ID
                            INNER JOIN PM.A_FUEL FUEL ON  A.ASET_FUEL = FUEL.FUEL_ID
                            INNER JOIN PM.A_COLOR CLO ON A.ASET_COLOR = CLO.COLOR_ID
                            INNER JOIN PM.A_GROUP GRP ON A.ASET_GROUP = GRP.GROUP_ID
                            INNER JOIN PM.A_DEPARTMENT DEP ON A.ASET_DEPARTMENT = DEP.DEPARTMENT_ID
                            INNER JOIN PM.A_BUSINESS BUSS ON DEP.DEPARTMENT_BUSINESS = BUSS.BUSINESS_ID
                            INNER JOIN PM.A_COMPANY COMP ON BUSS.BUSINESS_COMPANY = COMP.COMPANY_ID
                            WHERE  A.ACT = 1";
            var AsetList = db.Database.SqlQuery<CarVM>(sql: sql_1);

            if (!string.IsNullOrEmpty(Asset))
            {
                var R = AsetList.Where(a => Asset.Contains(a.ASET_ID)
                     || Asset.Contains(a.ASET_TAG)
                     || Asset.Contains(a.ASET_CODE_NAME)
                     || Asset.Contains(a.DEPARTMENT_V))
                    .ToList();
                if (!string.IsNullOrEmpty(Operation))
                {
                    ViewBag.Message = "Search Result";
                }
                return View(R);
            }
            else
            {
                ViewBag.Message = "100 Free Asset";
                var R = AsetList.Where(a => a.ASET_USED == false).Take(500).ToList();
                return View(R);
            }
        }

        [KAuth(101)]
        public ActionResult NewFridge()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            ViewBag.ASET_AREA = new SelectList(RP.MyAreaList(UserId: U, UserType: T), "AREA_ID", "AREA_NAME");
            ViewBag.ASET_BODY = new SelectList(RP.BodyList(), "BODY_ID", "BODY_NAME");
            ViewBag.ASET_BRAND = new SelectList(RP.BrandList(), "BRAND_ID", "BRAND_NAME");
            ViewBag.ASET_COLOR = new SelectList(RP.ColorList(), "COLOR_ID", "COLOR_NAME");
            ViewBag.ASET_FUEL = new SelectList(RP.FuelList(), "FUEL_ID", "FUEL_NAME");
            ViewBag.ASET_BUSINESS = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
            ViewBag.ASET_GROUP = new SelectList(RP.MyGroup(UserId: U, UserType: T), "GROUP_ID", "GROUP_NAME");
            return View();
        }

        [KAuth(101)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewFridge(NewPRANAsset NA)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (ModelState.IsValid)
            {
                A_ASET_DATA ad = new A_ASET_DATA();
                ad.ASET_TAG = NA.ASET_TAG;
                ad.ASET_CODE_NAME = NA.ASET_CODE_NAME;
                ad.ASET_DEPARTMENT = NA.ASET_DEPARTMENT;
                ad.ASET_AREA = NA.ASET_AREA;
                ad.ASET_BODY = NA.ASET_BODY;
                ad.ASET_BRAND = NA.ASET_BRAND;
                ad.ASET_FUEL = NA.ASET_FUEL;
                ad.ASET_COLOR = NA.ASET_COLOR;
                ad.ASET_GROUP = NA.ASET_GROUP;
                ad.ASET_MODEL = NA.ASET_MODEL ?? "N/A";
                ad.ASET_PO_ENGINE = NA.ASET_PO_ENGINE ?? "0";
                ad.ASET_GRN_CHASIS = NA.ASET_GRN_CHASIS ?? "0";
                ad.ASET_CAPACITY = NA.ASET_CAPACITY ?? "0";
                ad.ASET_PRICE = NA.ASET_PRICE ?? 0;
                ad.ASET_REG_DATE = NA.ASET_REG_DATE ?? DateTime.Now;
                ad.ASET_HEIGHT = "0";
                ad.ASET_WEIGHT = "0";
                ad.ASET_MFG_YEAR = NA.ASET_MFG_YEAR ?? Convert.ToInt16(DateTime.Now.Year);
                ad.ASET_IMAGE = "~/";
                ad.ASET_NOTE = NA.ASET_NOTE;
                ad.ASET_USED = false;
                ad.ACT = true;
                ad.CDT = DateTime.Now;
                ad.CDU = U;
                ad.VAR = 1;
                //Save Aset Data
                ad.ASET_ID = Sequence.GetSeq(10).ToString();
                db.A_ASET_DATA.Add(ad);
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Asset saved succefully','success');";
                return RedirectToAction("Index", new { Asset = ad.ASET_CODE_NAME });
            }
            ViewBag.ASET_AREA = new SelectList(RP.MyAreaList(UserId: U, UserType: T), "AREA_ID", "AREA_NAME");
            ViewBag.ASET_BODY = new SelectList(RP.BodyList(), "BODY_ID", "BODY_NAME");
            ViewBag.ASET_BRAND = new SelectList(RP.BrandList(), "BRAND_ID", "BRAND_NAME");
            ViewBag.ASET_COLOR = new SelectList(RP.ColorList(), "COLOR_ID", "COLOR_NAME");
            ViewBag.ASET_FUEL = new SelectList(RP.FuelList(), "FUEL_ID", "FUEL_NAME");
            ViewBag.ASET_BUSINESS = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
            ViewBag.ASET_GROUP = new SelectList(RP.MyGroup(UserId: U, UserType: T), "GROUP_ID", "GROUP_NAME");
            TempData["msg"] = "swal('Ohh..','Asset saved failed','error');";
            return View(NA);
        }


        [KAuth(101)]
        public ActionResult NewCar()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            ViewBag.ASET_AREA = new SelectList(RP.MyAreaList(UserId: U, UserType: T), "AREA_ID", "AREA_NAME");
            ViewBag.ASET_BODY = new SelectList(RP.BodyList(), "BODY_ID", "BODY_NAME");
            ViewBag.ASET_BRAND = new SelectList(RP.BrandList(), "BRAND_ID", "BRAND_NAME");
            ViewBag.ASET_COLOR = new SelectList(RP.ColorList(), "COLOR_ID", "COLOR_NAME");
            ViewBag.ASET_FUEL = new SelectList(RP.FuelList(), "FUEL_ID", "FUEL_NAME");
            ViewBag.ASET_BUSINESS = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
            ViewBag.ASET_GROUP = new SelectList(RP.MyGroup(UserId: U, UserType: T), "GROUP_ID", "GROUP_NAME");
            return View();
        }

        [KAuth(101)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewCar(NewCar NC)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (ModelState.IsValid)
            {
                A_ASET_DATA ad = new A_ASET_DATA();
                ad.ASET_TAG = NC.ASET_TAG;
                ad.ASET_CODE_NAME = NC.ASET_CODE_NAME;
                ad.ASET_DEPARTMENT = NC.ASET_DEPARTMENT;
                ad.ASET_AREA = NC.ASET_AREA;
                ad.ASET_BODY = NC.ASET_BODY;
                ad.ASET_BRAND = NC.ASET_BRAND;
                ad.ASET_FUEL = NC.ASET_FUEL;
                ad.ASET_COLOR = NC.ASET_COLOR;
                ad.ASET_GROUP = NC.ASET_GROUP;
                ad.ASET_MODEL = NC.ASET_MODEL ?? "N/A";
                ad.ASET_PO_ENGINE = NC.ASET_PO_ENGINE ?? "0";
                ad.ASET_GRN_CHASIS = NC.ASET_GRN_CHASIS ?? "0";
                ad.ASET_CAPACITY = NC.ASET_CAPACITY ?? "0";
                ad.ASET_PRICE = NC.ASET_PRICE ?? 0;
                ad.ASET_REG_DATE = NC.ASET_REG_DATE ?? DateTime.Now;
                ad.ASET_HEIGHT = "0";
                ad.ASET_WEIGHT = "0";
                ad.ASET_MFG_YEAR = NC.ASET_MFG_YEAR ?? Convert.ToInt16(DateTime.Now.Year);
                ad.ASET_IMAGE = "~/";
                ad.ASET_NOTE = NC.ASET_NOTE;
                ad.ASET_USED = false;
                ad.ACT = true;
                ad.CDT = DateTime.Now;
                ad.CDU = U;
                ad.VAR = 1;
                //Save Aset Data
                ad.ASET_ID = Sequence.GetSeq(10).ToString();
                db.A_ASET_DATA.Add(ad);
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Car saved succefully','success');";
                return RedirectToAction("Index", new { Asset = ad.ASET_CODE_NAME });
            }
            ViewBag.ASET_AREA = new SelectList(RP.MyAreaList(UserId: U, UserType: T), "AREA_ID", "AREA_NAME");
            ViewBag.ASET_BODY = new SelectList(RP.BodyList(), "BODY_ID", "BODY_NAME");
            ViewBag.ASET_BRAND = new SelectList(RP.BrandList(), "BRAND_ID", "BRAND_NAME");
            ViewBag.ASET_COLOR = new SelectList(RP.ColorList(), "COLOR_ID", "COLOR_NAME");
            ViewBag.ASET_FUEL = new SelectList(RP.FuelList(), "FUEL_ID", "FUEL_NAME");
            ViewBag.ASET_BUSINESS = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
            ViewBag.ASET_GROUP = new SelectList(RP.MyGroup(UserId: U, UserType: T), "GROUP_ID", "GROUP_NAME");
            TempData["msg"] = "swal('Ohh..','Car saved failed','error');";
            return View(NC);
        }

        [KAuth(101)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadExcel(HttpPostedFileBase excelFile)
        {
            string U = Session["UserId"]?.ToString();
            string T = Session["UserRole"]?.ToString();

            if (excelFile == null || excelFile.ContentLength == 0)
            {
                TempData["msg"] = "swal('Error','Please select Excel file','error');";
                return RedirectToAction("NewCar");
            }

            if (!excelFile.FileName.EndsWith(".xlsx"))
            {
                TempData["msg"] = "swal('Error','Only .xlsx file allowed','error');";
                return RedirectToAction("NewCar");
            }

            try
            {
                // ================= MASTER DATA LOAD =================
                var areaList = RP.MyAreaList(UserId: U, UserType: T).ToList();
                var bodyList = RP.BodyList().ToList();
                var brandList = RP.BrandList().ToList();
                var colorList = RP.ColorList().ToList();
                var fuelList = RP.FuelList().ToList();
                var groupList = RP.MyGroup(UserId: U, UserType: T).ToList();
                var deptList = RP.DepartmentList().ToList();

                List<string> errorList = new List<string>();
                List<A_ASET_DATA> validData = new List<A_ASET_DATA>();


                using (var package = new OfficeOpenXml.ExcelPackage(excelFile.InputStream))
                {
                    var ws = package.Workbook.Worksheets.First();

                    if (ws.Dimension == null)
                    {
                        TempData["msg"] = "swal('Error','Excel is empty','error');";
                        return RedirectToAction("NewCar");
                    }

                    int rowCount = ws.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        string rowError = "";

                        string assetName = ws.Cells[row, 2].Text.Trim();
                        if (string.IsNullOrEmpty(assetName))
                            continue;

                        bool exists = db.A_ASET_DATA.Any(x => x.ASET_CODE_NAME == assetName);
                        if (exists)
                            continue;

                        A_ASET_DATA ad = new A_ASET_DATA();
                        ad.ASET_ID = Sequence.GetSeq(10).ToString();

                        ad.ASET_TAG = ws.Cells[row, 1].Text.Trim();
                        ad.ASET_CODE_NAME = assetName;

                        ad.ASET_MODEL = ws.Cells[row, 3].Text.Trim() ?? "N/A";
                        ad.ASET_PO_ENGINE = ws.Cells[row, 4].Text.Trim() ?? "0";
                        ad.ASET_GRN_CHASIS = ws.Cells[row, 5].Text.Trim() ?? "0";
                        ad.ASET_CAPACITY = ws.Cells[row, 6].Text.Trim() ?? "0";

                        decimal price;
                        decimal.TryParse(ws.Cells[row, 7].Text.Trim(), out price);
                        ad.ASET_PRICE = (int?)price;

                        DateTime regDate;
                        DateTime.TryParse(ws.Cells[row, 8].Text.Trim(), out regDate);
                        ad.ASET_REG_DATE = regDate == DateTime.MinValue ? DateTime.Now : regDate;

                        // ================= VALIDATION HELPERS =================

                        // DEPARTMENT
                        string deptName = ws.Cells[row, 9].Text.Trim();
                        var dept = deptList.FirstOrDefault(x => x.DEPARTMENT_NAME.ToUpper() == deptName.ToUpper());
                        if (dept == null)
                            rowError += $"Invalid Department ({deptName}), ";
                        else
                            ad.ASET_DEPARTMENT = dept.DEPARTMENT_ID;

                        // AREA
                        string areaName = ws.Cells[row, 10].Text.Trim();
                        var area = areaList.FirstOrDefault(x => x.AREA_NAME.ToUpper() == areaName.ToUpper());
                        if (area == null)
                            rowError += $"Invalid Area ({areaName}), ";
                        else
                            ad.ASET_AREA = area.AREA_ID;

                        // BRAND
                        string brandName = ws.Cells[row, 11].Text.Trim();
                        var brand = brandList.FirstOrDefault(x => x.BRAND_NAME.ToUpper() == brandName.ToUpper());
                        if (brand == null)
                            rowError += $"Invalid Brand ({brandName}), ";
                        else
                            ad.ASET_BRAND = brand.BRAND_ID;

                        // BODY
                        string bodyName = ws.Cells[row, 12].Text.Trim();
                        var body = bodyList.FirstOrDefault(x => x.BODY_NAME.ToUpper() == bodyName.ToUpper());
                        if (body == null)
                            rowError += $"Invalid Body ({bodyName}), ";
                        else
                            ad.ASET_BODY = body.BODY_ID;

                        // FUEL
                        string fuelName = ws.Cells[row, 13].Text.Trim();
                        var fuel = fuelList.FirstOrDefault(x => x.FUEL_NAME.ToUpper() == fuelName.ToUpper());
                        if (fuel == null)
                            rowError += $"Invalid Fuel ({fuelName}), ";
                        else
                            ad.ASET_FUEL = fuel.FUEL_ID;

                        // GROUP
                        string groupName = ws.Cells[row, 14].Text.Trim();
                        var group = groupList.FirstOrDefault(x => x.Group_Name.ToUpper() == groupName.ToUpper());
                        if (group == null)
                            rowError += $"Invalid Group ({groupName}), ";
                        else
                            ad.ASET_GROUP = group.Group_ID;

                        // COLOR
                        string colorName = ws.Cells[row, 15].Text.Trim();
                        var color = colorList.FirstOrDefault(x => x.COLOR_NAME.ToUpper() == colorName.ToUpper());
                        if (color == null)
                            rowError += $"Invalid Color ({colorName}), ";
                        else
                            ad.ASET_COLOR = color.COLOR_ID;

                        // YEAR
                        int year;
                        int.TryParse(ws.Cells[row, 16].Text.Trim(), out year);
                        ad.ASET_MFG_YEAR = (short?)(year == 0 ? DateTime.Now.Year : year);

                        ad.ASET_NOTE = ws.Cells[row, 17].Text.Trim();

                        // DEFAULT
                        ad.ASET_HEIGHT = "0";
                        ad.ASET_WEIGHT = "0";
                        ad.ASET_IMAGE = "~/";
                        ad.ASET_USED = false;
                        ad.ACT = true;
                        ad.CDT = DateTime.Now;
                        ad.CDU = U;
                        ad.VAR = 1;

                        // ================= RESULT HANDLING =================

                        if (!string.IsNullOrEmpty(rowError))
                        {
                            errorList.Add($"Row {row}: {rowError}");
                        }
                        else
                        {
                            validData.Add(ad);
                        }
                    }
                }

                // ================= IF ERROR FOUND =================
                if (errorList.Count > 0)
                {
                    string errorMsg = string.Join(" | ", errorList.Take(10));

                    TempData["msg"] = "swal('Validation Error','" + errorMsg.Replace("'", "") + "','error');";
                    return RedirectToAction("NewCar");
                }

                // ================= SAVE VALID DATA =================
                db.A_ASET_DATA.AddRange(validData);
                db.SaveChanges();

                TempData["msg"] = "swal('Success','Excel uploaded successfully','success');";
            }
            catch (Exception ex)
            {
                string error = ex.Message;

                if (ex.InnerException != null)
                    error += " | " + ex.InnerException.Message;

                if (ex.InnerException?.InnerException != null)
                    error += " | " + ex.InnerException.InnerException.Message;

                TempData["msg"] = "swal('Error','" + error.Replace("'", "") + "','error');";
            }

            return RedirectToAction("NewCar");
        }

        public ActionResult DownloadExcelTemplate()
        {
            string filePath = Server.MapPath("~/Template/AssetUploadTemplate.xlsx");

            if (!System.IO.File.Exists(filePath))
            {
                return HttpNotFound("File not found");
            }

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "AssetUploadTemplate.xlsx"
            );
        }

        [KAuth(101)]
        public ActionResult EditCar(string id)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (id == null)
            {
                TempData["msg"] = "swal('Opss..','Try with valid car','info');";
                return RedirectToAction("Index");
            }
            A_ASET_DATA ad = db.A_ASET_DATA.Find(id);
            if (ad == null)
            {
                TempData["msg"] = "swal('Ohh..','No car found','warning');";
                return RedirectToAction("Index");
            }
            CarEdit ce = new CarEdit();
            ce.ASET_ID = ad.ASET_ID;
            ce.ASET_CODE_NAME = ad.ASET_CODE_NAME;
            ViewBag.ASET_BUSINESS = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
            ViewBag.ASET_DEPARTMENT = new SelectList(RP.DepartmentById(ad.ASET_DEPARTMENT), "DEPARTMENT_ID", "DEPARTMENT_NAME");
            ViewBag.ASET_AREA = new SelectList(RP.MyAreaList(UserId: U, UserType: T), "AREA_ID", "AREA_NAME");
            // ViewBag.ASET_AREA = new SelectList(RP.MyAreaListByID(ad.ASET_AREA), "AREA_ID", "AREA_NAME");
            ce.ASET_NOTE = ad.ASET_NOTE;
            ce.ASET_HEIGHT = ad.ASET_HEIGHT;
            return PartialView("_EditCar", ce);
        }

        [KAuth(101)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCar(CarEdit CE)
        {
            if (ModelState.IsValid)
            {
                string U = Session["UserId"].ToString();
                A_ASET_DATA ad = db.A_ASET_DATA.Find(CE.ASET_ID);
                ad.ASET_DEPARTMENT = CE.ASET_DEPARTMENT;
                ad.ASET_AREA = CE.ASET_AREA;
                ad.ASET_NOTE = CE.ASET_NOTE;
                ad.ASET_HEIGHT = CE.ASET_HEIGHT;
                ad.UDT = DateTime.Now;
                ad.UDU = U;
                ad.VAR = ad.VAR + 1;
                db.Entry(ad).State = EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Car data updated','success');";
                return RedirectToAction("Index", new { Asset = CE.ASET_ID });
            }
            TempData["msg"] = "swal('Ohh..','Car data not updated','warning');";
            return RedirectToAction("Index", new { Asset = CE.ASET_ID });
        }

        [KAuth(102)]
        public ActionResult AssetAcceList()
        {
            string U = Session["UserId"].ToString();
            ViewBag.ASET_BUSINESS = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
            return View();
        }

        [KAuth(102)]
        public ActionResult ShowAccessoreis(string Asset, string AssetId)
        {
            var AcceList = from a in db.A_ASET_ACCE
                           select new AssetAcceVM
                           {
                               ASET_ID = a.A_ASET_DATA.ASET_CODE_NAME,
                               ASET_ID_V = a.ASET_ID,
                               ACCE_ID = a.A_ACCE_ITEMS.ACCE_NAME,
                               ACCE_ID_V = a.ACCE_ID,
                               AAI_DESC = a.AAI_DESC,
                               AAI_QUANTITY = a.AAI_QUANTITY ?? 0,
                               AAI_RATE = a.AAI_RATE ?? 0,
                               Amount = a.AAI_QUANTITY * a.AAI_RATE,
                               ACT = a.ACT
                           };
            if (!string.IsNullOrEmpty(AssetId))
            {
                var R = AcceList.Where(a => a.ASET_ID_V == AssetId).ToList();
                //added new
                return View("_ShowAccessoreis", R);
            }

            if (!string.IsNullOrEmpty(Asset))
            {
                //Search Result
                var R = AcceList.Where(a => a.ASET_ID == Asset).ToList();
                return View("_ShowAccessoreis", R);
            }
            //no data
            return View(AcceList.Take(0).ToList());
        }

        [KAuth(102)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAcceItem(AssetAcceVM AI)
        {
            string U = Session["UserId"].ToString();

            if (ModelState.IsValid)
            {
                A_ASET_ACCE a_ASET_ACCE = new A_ASET_ACCE();
                a_ASET_ACCE.ASET_ID = AI.ASET_ID;
                a_ASET_ACCE.ACCE_ID = AI.ACCE_ID;
                a_ASET_ACCE.AAI_DESC = AI.AAI_DESC ?? "New Item";
                a_ASET_ACCE.AAI_QUANTITY = AI.AAI_QUANTITY;
                a_ASET_ACCE.AAI_RATE = AI.AAI_RATE;
                a_ASET_ACCE.ACT = false;
                a_ASET_ACCE.CDT = DateTime.Now;
                a_ASET_ACCE.CDU = U;
                a_ASET_ACCE.VAR = 1;
                db.A_ASET_ACCE.Add(a_ASET_ACCE);
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Item added, Please approve','success');";
                return RedirectToAction("AssetAcceList");
            }
            TempData["msg"] = "swal('Ohh..','Unable to add new item','error');";
            return RedirectToAction("AssetAcceList");
        }

        [KAuth(102)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(AccesoreisDelete AD)
        {
            if (ModelState.IsValid)
            {
                A_ASET_ACCE a_ASET_ACCE = db.A_ASET_ACCE.Where(i => i.ACCE_ID == AD.ACCE_IDD && i.ASET_ID == AD.ASET_IDD).FirstOrDefault();
                if (a_ASET_ACCE != null)
                {
                    db.A_ASET_ACCE.Remove(a_ASET_ACCE);
                    db.SaveChanges();
                    TempData["msg"] = "swal('Success..','Deleted successfully','success');";
                    return RedirectToAction("AssetAcceList");
                }
                TempData["msg"] = "swal('Opps..','Not found','warning');";
                return RedirectToAction("AssetAcceList");
            }
            TempData["msg"] = "swal('Ohh..','Can't Delete Approved','error');";
            return RedirectToAction("AssetAcceList");
        }
        [KAuth(102)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApproveItem(AccesoreisApprove AD)
        {
            string U = Session["UserId"].ToString();
            if (ModelState.IsValid)
            {
                A_ASET_ACCE a_ASET_ACCE = db.A_ASET_ACCE.Where(i => i.ACCE_ID == AD.ACCE_IDA && i.ASET_ID == AD.ASET_IDA).FirstOrDefault();
                if (a_ASET_ACCE != null)
                {
                    a_ASET_ACCE.ACT = true;
                    a_ASET_ACCE.UDT = DateTime.Now;
                    a_ASET_ACCE.UDU = U;
                    a_ASET_ACCE.VAR = a_ASET_ACCE.VAR + 1;
                    db.Entry(a_ASET_ACCE).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["msg"] = "swal('Success..','Approved successfully','success');";
                    return RedirectToAction("AssetAcceList");
                }
                TempData["msg"] = "swal('Opps..','Not found','warning');";
                return RedirectToAction("AssetAcceList");
            }
            TempData["msg"] = "swal('Ohh..','Not Approved','error');";
            return RedirectToAction("AssetAcceList");
        }


        #region Accesoreis_Adjustment
        [KAuth(104)]
        public ActionResult AccesoriesAdj()
        {
            string U = Session["UserId"].ToString();
            ViewBag.ASET_BUSINESS = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
            List<SelectListItem> TT = new List<SelectListItem>();
            TT.Add(new SelectListItem() { Value = "P", Text = "Add" });
            TT.Add(new SelectListItem() { Value = "N", Text = "Remove" });
            ViewBag.AUA_TYPE = TT;
            return View();
        }
        [KAuth(104)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdjustmentPost(AssetAcceAdjVM AI)
        {
            string U = Session["UserId"].ToString();
            if (ModelState.IsValid)
            {
                A_ASET_ACCE AA = db.A_ASET_ACCE.Where(a => a.ASET_ID == AI.ASET_ID && a.ACCE_ID == AI.ACCE_ID).FirstOrDefault();
                if (AI.AUA_TYPE == "P")
                {
                    AA.AAI_QUANTITY = Convert.ToByte(AA.AAI_QUANTITY + AI.AAI_QUANTITY);
                }
                else // N
                {
                    AA.AAI_QUANTITY = Convert.ToByte(AA.AAI_QUANTITY - AI.AAI_QUANTITY);
                }
                var OldRate = AA.AAI_QUANTITY * AA.AAI_RATE;
                var NewRate = AI.AAI_QUANTITY * AI.AAI_RATE;
                var TotalPrice = OldRate + NewRate;
                var TotalQty = AA.AAI_QUANTITY + AI.AAI_QUANTITY;
                var Rate = TotalPrice / TotalQty;
                AA.AAI_RATE = Rate;
                AA.AAI_DESC = AI.AUA_DESC;
                AA.ACT = true;
                AA.UDU = U;
                AA.UDT = DateTime.Now;
                AA.VAR = AA.VAR + 1;

                var AU = db.A_ASET_USER.Where(u => u.AU_ASET == AA.ASET_ID && u.ACT == true && (u.AU_EAUSER == null || u.AU_EADATE == null)).ToList();
                //var AssetUser = "0";
                var TransferId = "0";
                if (AU.Count != 0)
                {
                    //AssetUser = AU.FirstOrDefault().AU_USER;
                    TransferId = AU.FirstOrDefault().AU_ID;
                }
                else
                {
                    TempData["msg"] = "swal('Ohh..','You Can't make adjustment without asset user','error');";
                    return RedirectToAction("AccesoriesAdj", new { AssetId = AA.ASET_ID });
                }
                var Counter = db.A_ASET_USR_ACE.Where(t => t.AUA_AU == TransferId).ToList(); //Get Already added Items
                A_ASET_USR_ACE nAUA = new A_ASET_USR_ACE();
                nAUA.AUA_AU = TransferId;
                nAUA.AUA_LINE = Convert.ToByte(Counter.Count + 1);
                nAUA.AUA_ACCE_ID = AI.ACCE_ID;
                nAUA.AUA_DESC = AI.AUA_DESC;
                nAUA.AUA_QTY = AA.AAI_QUANTITY;
                nAUA.AUA_RATE = Rate;
                nAUA.AUA_TYPE = AI.AUA_TYPE;
                nAUA.ACT = true;
                nAUA.CDT = DateTime.Now;
                nAUA.CDU = U;
                nAUA.VAR = 1;

                db.Entry(AA).State = EntityState.Modified; // Asset Accesoreis Adjustment
                db.A_ASET_USR_ACE.Add(nAUA); //Add to User Accessories List

                //A_ASET_UA AUA = new A_ASET_UA();
                //string ID = Sequence.GetSeq(13).ToString();
                //AUA.AUA_ID = ID;
                //AUA.AUA_AU = TransferId;
                //AUA.AUA_ACCE = AI.ACCE_ID;
                //AUA.AUA_QTY = AA.AAI_QUANTITY;
                //AUA.AUA_RATE = Rate;
                //AUA.AUA_TYPE = AI.AUA_TYPE;
                //AUA.ACT = true;
                //AUA.CDT = DateTime.Now;
                //AUA.CDU = U;
                //AUA.VAR = 1;
                //db.A_ASET_UA.Add(AUA);
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Adjustment completed','success');";
                return RedirectToAction("AccesoriesAdj", new { AssetId = AA.ASET_ID });
            }

            ViewBag.ASET_BUSINESS = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
            List<SelectListItem> TT = new List<SelectListItem>();
            TT.Add(new SelectListItem() { Value = "P", Text = "Add" });
            TT.Add(new SelectListItem() { Value = "N", Text = "Remove" });
            ViewBag.AUA_TYPE = TT;
            TempData["msg"] = "swal('Ohh..','Adjustment failed','error');";
            return View("AccesoriesAdj");
        }
        #endregion

        #region AssetDamageApplication
        public List<DismissList> ASET_DISSMISS_LIST(List<string> ASET)
        {
            var asetDismiss = (from r in db.A_ASET_APP
                               join a in db.A_AREA on r.RECEIVE_AREA equals a.AREA_ID
                               join u in db.A_USERS on r.REQU_USER equals u.USER_TEXT
                               //  join d in db.A_USERS on r.APPR_USER equals d.USER_TEXT
                               // join i in db.A_INSUD on r.INSURACE_ID equals i.INSU_ID
                               //where r.INSURACE_LINE == i.INSU_LINE
                               ///&& ASET.Contains(r.ASET_ID)
                               select new DismissList
                               {
                                   ASET_ID = r.ASET_ID,
                                   ASET_CODE_NAME = r.A_ASET_DATA.ASET_CODE_NAME,
                                   SERVICE_MONTH = r.SERVICE_MONTH,
                                   ASET_VALUE = r.ASET_VALUE,
                                   ASET_USER = r.A_OWNR.OWNR_USER,
                                   APPLY_TYPE = r.APPLY_TYPE,
                                   APPLY_DATE = r.APPLY_DATE,
                                   APPLY_REASON = r.APPLY_REASON,
                                   RECEIVE_TYPE = r.RECEIVE_TYPE,
                                   RECEIVE_DETAIL = r.RECEIVE_DETAIL,
                                   RECEIVE_DATE = r.RECEIVE_DATE,
                                   ASET_AREA = a.AREA_NAME,
                                   INSURANCE_ID = r.INSURACE_ID,
                                   INSURANCE_LINE = r.INSURACE_LINE,
                                   INSU_ASET_VALUE = r.A_INSUH.A_INSUD.Where(d => d.INSU_ID == r.INSURACE_ID && d.INSU_LINE == r.INSURACE_LINE).FirstOrDefault().INSU_ASET_VALUE, // i.INSU_ASET_VALUE,
                                   INSU_POLICY = r.A_INSUH.INSU_POLICY,// i.A_INSUH.INSU_POLICY,
                                   INSURANCE_COMP = r.A_INSUH.INSU_COMP,//i.A_INSUH.INSU_COMP,
                                   APPLY_USER = u.USER_NAME,
                                   APPROVE_USER = r.APPR_USER,
                                   APPROVE_DATE = r.APPR_DATE,
                                   DISMISS_STATUS = r.ACT
                               }).ToList();
            return asetDismiss;
        }
        [KAuth(103)]
        public ActionResult AssetDismiss()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            //List<string> ASET = RP.MyAssetListString(UserId: U, UserType: T);
            List<string> EMPTY = new List<string>();
            var ADL = ASET_DISSMISS_LIST(ASET: EMPTY);
            return View(ADL);
        }
        [KAuth(103)]
        [HttpGet]
        public ActionResult NewDismiss()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            List<string> ASET = RP.MyAssetListString(UserId: U, UserType: T);
            ViewBag.ASET_ID = new SelectList(RP.AsetByListId(AssetId: ASET), "ASET_ID", "ASET_NAME_TAG");
            List<SelectListItem> TT = new List<SelectListItem>();
            TT.Add(new SelectListItem() { Value = "DMAGE", Text = "Damage" });
            TT.Add(new SelectListItem() { Value = "SALE", Text = "Sales" });
            TT.Add(new SelectListItem() { Value = "LOST", Text = "Lost" });
            ViewBag.APPLY_TYPE = TT;
            List<SelectListItem> CT = new List<SelectListItem>();
            CT.Add(new SelectListItem() { Value = "0", Text = "Non Insurance" });
            CT.Add(new SelectListItem() { Value = "1", Text = "Insurance" });
            ViewBag.IsIC = CT;
            ViewBag.RECEIVE_AREA = new SelectList(RP.MyAreaList(UserId: U, UserType: T), "AREA_ID", "AREA_NAME");
            return View();
        }
        [KAuth(103)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewDismiss(NewDismiss ND)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            string MSG = "swal('Opss...','Termination not applied','warning');";
            if (ModelState.IsValid)
            {
                bool IsOff = false;
                bool IsInsOK = true;
                A_ASET_APP AA = new A_ASET_APP();
                AA.ASET_ID = ND.ASET_ID;
                AA.ASET_VALUE = 1;
                AA.APPLY_DATE = ND.APPLY_DATE;
                AA.APPLY_REASON = ND.APPLY_REASON;
                AA.APPLY_TYPE = ND.APPLY_TYPE;
                AA.SERVICE_MONTH = Sequence.GetAssetServiceMonth(id: ND.ASET_ID, ADATE: ND.APPLY_DATE);//get month from ASET_DATA REG Date to APPLY DATE
                AA.RECEIVE_AREA = ND.RECEIVE_AREA;
                AA.REQU_USER = U;//Session User
                AA.ACT = false;
                AA.CDT = DateTime.Now;
                AA.CDU = U;
                AA.VAR = 1;
                //To get Current User
                //User Same Function for Case & Penalty aanndd Asset Dismiss
                JsonResult JR = PMC.CaseAndPenaltyUserData(ND.ASET_ID, pdt: ND.APPLY_DATE);
                string json = JsonConvert.SerializeObject(JR.Data);
                JRdata dto = JsonConvert.DeserializeObject<JRdata>(json);
                if (dto.success)
                {
                    AA.OWNR_ID = dto.USER_NAME;//get from ASET_USER Table
                }
                else
                {
                    AA.OWNR_ID = "0"; // If user not found set as 0 / not define user;
                }
                if (ND.IsIC == "1")
                {
                    //JsonResult IR = ;
                    string InsuranceJSON = JsonConvert.SerializeObject(PMC.GetInsuranceData(ND.ASET_ID, DTE: ND.APPLY_DATE).Data);
                    DismissInsurance ito = JsonConvert.DeserializeObject<DismissInsurance>(InsuranceJSON);
                    if (ito.Success)
                    {
                        AA.INSURACE_ID = ito.INSU_ID;
                        AA.INSURACE_LINE = ito.INSU_LINE;
                        AA.INSURACE_COMP = ito.INSU_COMP;
                    }
                    else
                    {
                        IsInsOK = false;
                        MSG = "swal('Ohh...','No Insurance Data Found','error');";
                    }
                }
                if (IsInsOK)
                {
                    IsOff = Sequence.OnOffAsset(AssetId: ND.ASET_ID, VAL: false);
                    if (IsOff)
                    {
                        db.A_ASET_APP.Add(AA);
                        db.SaveChanges();
                        TempData["msg"] = "swal('Success...','Termination Applied','success');";
                        return Redirect("AssetDismiss");
                    }
                }
            }
            TempData["msg"] = MSG;
            List<string> ASET = RP.MyAssetListString(UserId: U, UserType: T);
            ViewBag.ASET_ID = new SelectList(RP.AsetByListId(AssetId: ASET), "ASET_ID", "ASET_NAME_TAG");
            List<SelectListItem> TT = new List<SelectListItem>();
            TT.Add(new SelectListItem() { Value = "DMAGE", Text = "Damage" });
            TT.Add(new SelectListItem() { Value = "SALE", Text = "Sales" });
            TT.Add(new SelectListItem() { Value = "LOST", Text = "Lost" });
            ViewBag.APPLY_TYPE = TT;
            List<SelectListItem> CT = new List<SelectListItem>();
            CT.Add(new SelectListItem() { Value = "0", Text = "Non Insurance" });
            CT.Add(new SelectListItem() { Value = "1", Text = "Insurance" });
            ViewBag.IsIC = CT;
            ViewBag.RECEIVE_AREA = new SelectList(RP.MyAreaList(UserId: U, UserType: T), "AREA_ID", "AREA_NAME");
            return View();
        }
        public JsonResult GetInsurance(string id)
        {
            string DataI = "No Data Found";
            A_INSUH H = db.A_INSUH.Where(h => h.ACT == true && h.INSU_ASET == id).FirstOrDefault();
            if (H != null)
            {
                A_INSUD D = db.A_INSUD.OrderByDescending(d => d.INSU_LINE).Where(d => d.INSU_ID == H.INSU_ID).FirstOrDefault();
                DataI = string.Format("Insurance:{0},Policy:{1},Company:{2},Line:{3},Value:{4},Premium:{5},Date:{6}", H.INSU_ID, H.INSU_POLICY, H.INSU_COMP, D.INSU_LINE, D.INSU_ASET_VALUE, D.INSU_AMOUNT, D.INSU_DATE);
            }

            string Wd, Wa, Wu, Sec, Ad, Aa, Au, Pri;
            var fromAssetData = db.A_ASET_DATA.Where(a => a.ACT == true && a.ASET_ID == id).ToList();
            if (fromAssetData.Count == 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            Ad = Wd = fromAssetData.First().ASET_DEPARTMENT;
            Aa = Wa = fromAssetData.First().ASET_AREA;
            Au = Wu = "0";
            Pri = Sec = RP.InspectionAndJobData(Ad, Aa, Au);

            var fromHandOver = db.A_ASET_USER.Where(a => a.AU_ASET == id && (a.AU_ERUSER == null || a.AU_ERUSER == "")).ToList();
            if (fromHandOver.Count > 0)
            {
                Wd = fromHandOver.FirstOrDefault().AU_DEPT;
                Wa = fromHandOver.FirstOrDefault().AU_AREA;
                Wu = fromHandOver.FirstOrDefault().AU_USER;
                Sec = RP.InspectionAndJobData(Wd, Wa, Wu);
            }
            return Json(new { DataI = DataI, prim = Pri, seco = Sec }, JsonRequestBehavior.AllowGet);
        }
        [KAuth(103)]
        public ActionResult UpdateDismiss(string id)
        {
            ViewBag.ASET_ID = id;
            List<SelectListItem> AT = new List<SelectListItem>();
            AT.Add(new SelectListItem() { Value = "CASH", Text = "Cash" });
            AT.Add(new SelectListItem() { Value = "CHEQUE", Text = "Cheque" });
            AT.Add(new SelectListItem() { Value = "BANK TRANSFER", Text = "Bank Transfer" });
            AT.Add(new SelectListItem() { Value = "ADJUSTMENT", Text = "Adjustment" });
            ViewBag.RECEIVE_TYPE = AT;
            return PartialView();
        }
        [KAuth(103)]
        [HttpPost]
        public ActionResult UpdateDismiss(UpdateDismiss UD)
        {
            string U = Session["UserId"].ToString();
            string MSG = "swal('Opss...','Termination not update','warning');";
            if (ModelState.IsValid)
            {
                A_ASET_APP AA = db.A_ASET_APP.Where(a => a.ASET_ID == UD.ASET_ID && a.ACT == false).FirstOrDefault();
                if (AA != null)
                {
                    AA.ASET_VALUE = UD.ASET_VALUE;
                    AA.RECEIVE_TYPE = UD.RECEIVE_TYPE;
                    AA.RECEIVE_DETAIL = UD.RECEIVE_DETAIL;
                    AA.RECEIVE_DATE = UD.RECEIVE_DATE;
                    AA.UDU = U;
                    AA.UDT = DateTime.Now;
                    AA.ACT = true;
                    AA.VAR = AA.VAR + 1;
                    db.Entry(AA).State = EntityState.Modified;
                    db.SaveChanges();
                    MSG = "swal('Success...','Termination updated, wait for approval','success');";
                }
            }
            TempData["msg"] = MSG;
            return RedirectToAction("AssetDismiss");
        }
        [KAuth(105)]
        public ActionResult AssetDismissApproval()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            //List<string> ASET = RP.MyAssetListString(UserId: U, UserType: T);
            List<string> EMPTY = new List<string>();
            var ADL = ASET_DISSMISS_LIST(ASET: EMPTY);
            return View(ADL.Where(d => d.DISMISS_STATUS == true && d.APPROVE_USER == null));
        }
        [KAuth(105)]
        public ActionResult ADApp(string id)
        {
            string U = Session["UserId"].ToString();
            string MSG = "swal('Opss...','Termination not approved','warning');";
            if (id != null)
            {
                A_ASET_APP AA = db.A_ASET_APP.Where(a => a.ASET_ID == id && a.APPR_USER == null && a.ACT == true).FirstOrDefault();
                if (AA != null)
                {
                    AA.UDU = U;
                    AA.UDT = DateTime.Now;
                    AA.APPR_USER = U;
                    AA.APPR_DATE = DateTime.Now;
                    AA.VAR = AA.VAR + 1;
                    db.Entry(AA).State = EntityState.Modified;
                    db.SaveChanges();
                    MSG = "swal('Success...','Termination approved','success');";
                }
            }
            TempData["msg"] = MSG;
            return RedirectToAction("AssetDismissApproval");
        }
        #endregion

        [KAuth(106)]
        public ActionResult FridgeExcel()
        {
            var model = new ExcelInputVM();
            return View(model);
        }

        [KAuth(106)]
        [HttpPost]
        public ActionResult FridgeExcel(ExcelInputVM model)
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
            DataTable dt = EDL.FridgeExcelData(model.MyExcelFile.InputStream, false);
            string strContent = EDL.ConvertDataTableToHTMLTable(dt);
            model.MSExcelTable = strContent;
            TempData["msg"] = "swal('Success..','Excel fill loaded for checking','success');";
            return View(model);
        }
        [KAuth(106)]
        public ActionResult SaveFridgeExcel(FridgeExcel[] ExlData)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            string result = "Asset data is not complete!";
            bool IsValidExcelData = true; //start condition as failed
            if (ModelState.IsValid)
            {
                //Check Excel Data Validation
                List<string> MyDepartments = RP.MyDeptListString(UserId: U, UserType: T);
                List<string> MyGroup = RP.MyGroupString(UserId: U, UserType: T);
                List<string> MyArea = RP.MyAreaListString(UserId: U, UserType: T);

                foreach (var item in ExlData)
                {
                    if (IsValidExcelData)
                    {
                        #region Check_Data_Validation
                        bool IsValidName = Sequence.IsExist_ASSET_CODE_NAME(item.ASET_CODE_NAME.Trim());
                        if (IsValidName)
                        {
                            result = string.Format("{0} :Name is already in used, try another Name", item.ASET_CODE_NAME);
                            IsValidExcelData = false;
                        }
                        if (IsValidExcelData)
                        {
                            bool IsValidDepartment = MyDepartments.Contains(item.ASET_DEPARTMENT);
                            if (!IsValidDepartment)
                            {
                                result = string.Format("{0} :Department Code {1} is Invalid, try with Valid Department", item.ASET_CODE_NAME, item.ASET_DEPARTMENT);
                                IsValidExcelData = false;
                            }
                        }
                        if (IsValidExcelData)
                        {
                            bool IsValidGroup = MyGroup.Contains(item.ASET_GROUP);
                            if (!IsValidGroup)
                            {
                                result = string.Format("{0} :Group Code {1} is Invalid, try with Valid Group", item.ASET_CODE_NAME, item.ASET_GROUP);
                                IsValidExcelData = false;
                            }
                        }
                        if (IsValidExcelData)
                        {
                            bool IsValidArea = MyArea.Contains(item.ASET_AREA);
                            if (!IsValidArea)
                            {
                                result = string.Format("{0} :Area Code {1} is Invalid, try with Valid Area", item.ASET_CODE_NAME, item.ASET_AREA);
                                IsValidExcelData = false;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        return Json(result, JsonRequestBehavior.AllowGet);

                    }
                }
                if (IsValidExcelData)
                {
                    #region Start_Insertion //If Valid start Insert to database
                    int Count = 0;
                    foreach (var item in ExlData)
                    {
                        A_ASET_DATA ad = new A_ASET_DATA();
                        ad.ASET_TAG = item.ASET_CODE_NAME.Trim();
                        ad.ASET_CODE_NAME = item.ASET_CODE_NAME.Trim();
                        ad.ASET_DEPARTMENT = item.ASET_DEPARTMENT;
                        ad.ASET_AREA = item.ASET_AREA;
                        ad.ASET_BODY = "BY0000";
                        ad.ASET_BRAND = item.ASET_BRAND;
                        ad.ASET_FUEL = "FL0000";
                        ad.ASET_COLOR = "CL0000";
                        ad.ASET_GROUP = item.ASET_GROUP;
                        ad.ASET_MODEL = item.ASET_MODEL ?? "N/A";
                        ad.ASET_PO_ENGINE = item.ASET_PO_ENGINE ?? "0";
                        ad.ASET_GRN_CHASIS = item.ASET_GRN_CHASIS ?? "0";
                        ad.ASET_CAPACITY = item.ASET_CAPACITY ?? "0";
                        ad.ASET_PRICE = item.ASET_PRICE ?? 0;
                        ad.ASET_REG_DATE = item.ASET_REG_DATE ?? DateTime.Now;
                        ad.ASET_HEIGHT = "0";
                        ad.ASET_WEIGHT = "0";
                        ad.ASET_MFG_YEAR = item.ASET_MFG_YEAR ?? Convert.ToInt16(DateTime.Now.Year);
                        ad.ASET_IMAGE = "~/";
                        ad.ASET_NOTE = item.ASET_NOTE;
                        ad.ASET_USED = false;
                        ad.ACT = true;
                        ad.CDT = DateTime.Now;
                        ad.CDU = U;
                        ad.VAR = 1;
                        //Save Aset Data
                        ad.ASET_ID = Sequence.GetSeq(10).ToString();
                        db.A_ASET_DATA.Add(ad);
                        Count = Count + 1;
                    }
                    db.SaveChanges();
                    result = string.Format("{0} :New Asset Saved", Count);
                    return Json(result, JsonRequestBehavior.AllowGet);
                    #endregion
                }
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                          .Where(y => y.Count > 0)
                          .ToList();
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        [KAuth(107)]
        public ActionResult NewMobilePhone()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            ViewBag.ASET_AREA = new SelectList(RP.MyAreaList(UserId: U, UserType: T), "AREA_ID", "AREA_NAME");
            ViewBag.ASET_BODY = new SelectList(RP.BodyList(), "BODY_ID", "BODY_NAME");
            ViewBag.ASET_BRAND = new SelectList(RP.BrandList(), "BRAND_ID", "BRAND_NAME");
            ViewBag.ASET_COLOR = new SelectList(RP.ColorList(), "COLOR_ID", "COLOR_NAME");
            ViewBag.ASET_BUSINESS = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
            ViewBag.ASET_GROUP = new SelectList(RP.MyGroup(UserId: U, UserType: T), "GROUP_ID", "GROUP_NAME");
            return View();

        }

        [KAuth(107)]
        [HttpPost]
        public ActionResult NewMobilePhone(NewMobilePhone NMP)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (ModelState.IsValid)
            {
                A_ASET_DATA ad = new A_ASET_DATA();
                ad.ASET_TAG = NMP.ASET_TAG ?? NMP.ASET_CODE_NAME;
                ad.ASET_CODE_NAME = NMP.ASET_CODE_NAME;
                ad.ASET_DEPARTMENT = NMP.ASET_DEPARTMENT;
                ad.ASET_AREA = NMP.ASET_AREA;
                ad.ASET_BODY = NMP.ASET_BODY;
                ad.ASET_BRAND = NMP.ASET_BRAND;
                ad.ASET_FUEL = "FL0000"; //NC.ASET_FUEL;
                ad.ASET_COLOR = NMP.ASET_COLOR;
                ad.ASET_GROUP = "GR0006";//NMP.ASET_GROUP;//NC.ASET_GROUP;
                ad.ASET_MODEL = NMP.ASET_MODEL ?? "N/A";
                ad.ASET_PO_ENGINE = NMP.ASET_PO_ENGINE ?? "0";
                ad.ASET_GRN_CHASIS = NMP.ASET_GRN_CHASIS ?? "0";
                ad.ASET_CAPACITY = "0";
                ad.ASET_PRICE = NMP.ASET_PRICE ?? 0;
                ad.ASET_REG_DATE = NMP.ASET_REG_DATE ?? DateTime.Now;
                ad.ASET_HEIGHT = "0";
                ad.ASET_WEIGHT = "0";
                ad.ASET_MFG_YEAR = Convert.ToInt16(DateTime.Now.Year);
                ad.ASET_IMAGE = "~/";
                ad.ASET_NOTE = NMP.ASET_NOTE;
                ad.ASET_USED = false;
                ad.ACT = true;
                ad.CDT = DateTime.Now;
                ad.CDU = U;
                ad.VAR = 1;
                //Save Aset Data
                ad.ASET_ID = Sequence.GetSeq(10).ToString();
                db.A_ASET_DATA.Add(ad);
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Mobile saved succefully','success');";
                return RedirectToAction("NewMobilePhone");
            }
            ViewBag.ASET_AREA = new SelectList(RP.MyAreaList(UserId: U, UserType: T), "AREA_ID", "AREA_NAME");
            ViewBag.ASET_BODY = new SelectList(RP.BodyList(), "BODY_ID", "BODY_NAME");
            ViewBag.ASET_BRAND = new SelectList(RP.BrandList(), "BRAND_ID", "BRAND_NAME");
            ViewBag.ASET_COLOR = new SelectList(RP.ColorList(), "COLOR_ID", "COLOR_NAME");
            ViewBag.ASET_BUSINESS = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
            ViewBag.ASET_GROUP = new SelectList(RP.MyGroup(UserId: U, UserType: T), "GROUP_ID", "GROUP_NAME");
            return View(NMP);
        }

        [KAuth(108)]
        public ActionResult BikeList(string Asset, string Operation)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            //   List<AssetManager_ID> MyAssetList = RP.MyAssetListString_New(UserId: U, UserType: T);


            //List<string> MyAssetList = RP.MyAssetListString(UserId: U, UserType: T);

            var AsetList = (from a in db.VW_USER_ASSET_LIST.Where(s => s.UG_USER == U)
                                //from a in db.A_ASET_DATA.AsEnumerable()
                                //join b in MyAssetList on a.ASET_ID equals b.AsetID
                                //where a.ACT == true
                                //&& MyAssetList.Contains(a.ASET_ID)
                            select new CarVM
                            {
                                ASET_ID = a.ASET_ID,
                                ASET_TAG = a.ASET_TAG,
                                ASET_CODE_NAME = a.ASET_CODE_NAME,
                                ASET_AREA = a.AREA_NAME,//area
                                ASET_BODY = a.BODY_NAME,//body
                                ASET_BRAND = a.BRAND_NAME,//body
                                ASET_FUEL = a.FUEL_NAME,//fuel
                                ASET_COLOR = a.COLOR_NAME,//color
                                ASET_GROUP = a.GROUP_NAME,//group
                                ASET_MODEL = a.ASET_MODEL,
                                ASET_PO_ENGINE = a.ASET_PO_ENGINE,
                                ASET_GRN_CHASIS = a.ASET_GRN_CHASIS,
                                ASET_CAPACITY = a.ASET_CAPACITY,
                                ASET_PRICE = a.ASET_PRICE,
                                ASET_REG_DATE = a.ASET_REG_DATE,
                                ASET_HEIGHT = a.ASET_HEIGHT,
                                ASET_WEIGHT = a.ASET_WEIGHT,
                                ASET_MFG_YEAR = a.ASET_MFG_YEAR,
                                ASET_IMAGE = a.ASET_IMAGE,
                                ASET_NOTE = a.ASET_NOTE,
                                ASET_USED = a.ASET_USED,
                                ACT = a.ACT,
                                DEPARTMENT_V = a.DEPARTMENT_NAME,//depeartment
                                BUSINESS_V = a.BUSINESS_NAME,
                                COMPANY_V = a.COMPANY_NAME
                            });
            if (!string.IsNullOrEmpty(Asset))
            {
                var R = AsetList.Where(a => Asset.Contains(a.ASET_ID)
                     || Asset.Contains(a.ASET_TAG)
                     || Asset.Contains(a.ASET_CODE_NAME)
                     || Asset.Contains(a.ASET_CODE_NAME.Substring(a.ASET_CODE_NAME.Length - 4)))
                    .ToList();
                if (!string.IsNullOrEmpty(Operation))
                {
                    ViewBag.Message = "Search Result";
                }
                return View(R);
            }
            else
            {
                ViewBag.Message = "Shown 100 Free Bike of ";// + AsetList.Count ;
                var R = AsetList.Where(a => a.ASET_USED == false).Take(100).ToList();
                return View(R);
            }
        }


        [KAuth(108)]
        public ActionResult NewBike()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            ViewBag.ASET_AREA = new SelectList(RP.MyAreaList(UserId: U, UserType: T), "AREA_ID", "AREA_NAME");
            ViewBag.ASET_BODY = new SelectList(RP.BodyList(), "BODY_ID", "BODY_NAME");
            ViewBag.ASET_BRAND = new SelectList(RP.BrandList(), "BRAND_ID", "BRAND_NAME");
            ViewBag.ASET_COLOR = new SelectList(RP.ColorList(), "COLOR_ID", "COLOR_NAME");
            ViewBag.ASET_FUEL = new SelectList(RP.FuelList(), "FUEL_ID", "FUEL_NAME");
            ViewBag.ASET_BUSINESS = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
            ViewBag.ASET_GROUP = new SelectList(RP.MyGroup(UserId: U, UserType: T), "GROUP_ID", "GROUP_NAME");
            return View();
        }

        [KAuth(108)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewBike(NewCar NC)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (ModelState.IsValid)
            {
                A_ASET_DATA ad = new A_ASET_DATA();
                ad.ASET_TAG = NC.ASET_TAG;
                ad.ASET_CODE_NAME = NC.ASET_CODE_NAME;
                ad.ASET_DEPARTMENT = NC.ASET_DEPARTMENT;
                ad.ASET_AREA = NC.ASET_AREA;
                ad.ASET_BODY = NC.ASET_BODY;
                ad.ASET_BRAND = NC.ASET_BRAND;
                ad.ASET_FUEL = NC.ASET_FUEL;
                ad.ASET_COLOR = NC.ASET_COLOR;
                ad.ASET_GROUP = NC.ASET_GROUP;
                ad.ASET_MODEL = NC.ASET_MODEL ?? "N/A";
                ad.ASET_PO_ENGINE = NC.ASET_PO_ENGINE ?? "0";
                ad.ASET_GRN_CHASIS = NC.ASET_GRN_CHASIS ?? "0";
                ad.ASET_CAPACITY = NC.ASET_CAPACITY ?? "0";
                ad.ASET_PRICE = NC.ASET_PRICE ?? 0;
                ad.ASET_REG_DATE = NC.ASET_REG_DATE ?? DateTime.Now;
                ad.ASET_HEIGHT = "0";
                ad.ASET_WEIGHT = "0";
                ad.ASET_MFG_YEAR = NC.ASET_MFG_YEAR ?? Convert.ToInt16(DateTime.Now.Year);
                ad.ASET_IMAGE = "~/";
                ad.ASET_NOTE = NC.ASET_NOTE;
                ad.ASET_USED = false;
                ad.ACT = true;
                ad.CDT = DateTime.Now;
                ad.CDU = U;
                ad.VAR = 1;
                //Save Aset Data
                ad.ASET_ID = Sequence.GetSeq(10).ToString();
                db.A_ASET_DATA.Add(ad);
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Bike saved succefully','success');";
                return RedirectToAction("BikeList", new { Asset = ad.ASET_CODE_NAME });
            }
            ViewBag.ASET_AREA = new SelectList(RP.MyAreaList(UserId: U, UserType: T), "AREA_ID", "AREA_NAME");
            ViewBag.ASET_BODY = new SelectList(RP.BodyList(), "BODY_ID", "BODY_NAME");
            ViewBag.ASET_BRAND = new SelectList(RP.BrandList(), "BRAND_ID", "BRAND_NAME");
            ViewBag.ASET_COLOR = new SelectList(RP.ColorList(), "COLOR_ID", "COLOR_NAME");
            ViewBag.ASET_FUEL = new SelectList(RP.FuelList(), "FUEL_ID", "FUEL_NAME");
            ViewBag.ASET_BUSINESS = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
            ViewBag.ASET_GROUP = new SelectList(RP.MyGroup(UserId: U, UserType: T), "GROUP_ID", "GROUP_NAME");
            TempData["msg"] = "swal('Ohh..','Bike saved failed','error');";
            return View(NC);
        }

        [KAuth(108)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadExcelBike(HttpPostedFileBase excelFile)
        {
            string U = Session["UserId"]?.ToString();
            string T = Session["UserRole"]?.ToString();

            if (excelFile == null || excelFile.ContentLength == 0)
            {
                TempData["msg"] = "swal('Error','Please select Excel file','error');";
                return RedirectToAction("NewBike");
            }

            if (!excelFile.FileName.EndsWith(".xlsx"))
            {
                TempData["msg"] = "swal('Error','Only .xlsx file allowed','error');";
                return RedirectToAction("NewBike");
            }

            try
            {
                // ================= MASTER DATA LOAD =================
                var areaList = RP.MyAreaList(UserId: U, UserType: T).ToList();
                var bodyList = RP.BodyList().ToList();
                var brandList = RP.BrandList().ToList();
                var colorList = RP.ColorList().ToList();
                var fuelList = RP.FuelList().ToList();
                var groupList = RP.MyGroup(UserId: U, UserType: T).ToList();
                var deptList = RP.DepartmentList().ToList();

                List<string> errorList = new List<string>();
                List<A_ASET_DATA> validData = new List<A_ASET_DATA>();


                using (var package = new OfficeOpenXml.ExcelPackage(excelFile.InputStream))
                {
                    var ws = package.Workbook.Worksheets.First();

                    if (ws.Dimension == null)
                    {
                        TempData["msg"] = "swal('Error','Excel is empty','error');";
                        return RedirectToAction("NewBike");
                    }

                    int rowCount = ws.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        string rowError = "";

                        string assetName = ws.Cells[row, 2].Text.Trim();
                        if (string.IsNullOrEmpty(assetName))
                            continue;

                        bool exists = db.A_ASET_DATA.Any(x => x.ASET_CODE_NAME == assetName);
                        if (exists)
                            continue;

                        A_ASET_DATA ad = new A_ASET_DATA();
                        ad.ASET_ID = Sequence.GetSeq(10).ToString();

                        ad.ASET_TAG = ws.Cells[row, 1].Text.Trim();
                        ad.ASET_CODE_NAME = assetName;

                        ad.ASET_MODEL = ws.Cells[row, 3].Text.Trim() ?? "N/A";
                        ad.ASET_PO_ENGINE = ws.Cells[row, 4].Text.Trim() ?? "0";
                        ad.ASET_GRN_CHASIS = ws.Cells[row, 5].Text.Trim() ?? "0";
                        ad.ASET_CAPACITY = ws.Cells[row, 6].Text.Trim() ?? "0";

                        decimal price;
                        decimal.TryParse(ws.Cells[row, 7].Text.Trim(), out price);
                        ad.ASET_PRICE = (int?)price;

                        DateTime regDate;
                        DateTime.TryParse(ws.Cells[row, 8].Text.Trim(), out regDate);
                        ad.ASET_REG_DATE = regDate == DateTime.MinValue ? DateTime.Now : regDate;

                        // ================= VALIDATION HELPERS =================

                        // DEPARTMENT
                        string deptName = ws.Cells[row, 9].Text.Trim();
                        var dept = deptList.FirstOrDefault(x => x.DEPARTMENT_NAME.ToUpper() == deptName.ToUpper());
                        if (dept == null)
                            rowError += $"Invalid Department ({deptName}), ";
                        else
                            ad.ASET_DEPARTMENT = dept.DEPARTMENT_ID;

                        // AREA
                        string areaName = ws.Cells[row, 10].Text.Trim();
                        var area = areaList.FirstOrDefault(x => x.AREA_NAME.ToUpper() == areaName.ToUpper());
                        if (area == null)
                            rowError += $"Invalid Area ({areaName}), ";
                        else
                            ad.ASET_AREA = area.AREA_ID;

                        // BRAND
                        string brandName = ws.Cells[row, 11].Text.Trim();
                        var brand = brandList.FirstOrDefault(x => x.BRAND_NAME.ToUpper() == brandName.ToUpper());
                        if (brand == null)
                            rowError += $"Invalid Brand ({brandName}), ";
                        else
                            ad.ASET_BRAND = brand.BRAND_ID;

                        // BODY
                        string bodyName = ws.Cells[row, 12].Text.Trim();
                        var body = bodyList.FirstOrDefault(x => x.BODY_NAME.ToUpper() == bodyName.ToUpper());
                        if (body == null)
                            rowError += $"Invalid Body ({bodyName}), ";
                        else
                            ad.ASET_BODY = body.BODY_ID;

                        // FUEL
                        string fuelName = ws.Cells[row, 13].Text.Trim();
                        var fuel = fuelList.FirstOrDefault(x => x.FUEL_NAME.ToUpper() == fuelName.ToUpper());
                        if (fuel == null)
                            rowError += $"Invalid Fuel ({fuelName}), ";
                        else
                            ad.ASET_FUEL = fuel.FUEL_ID;

                        // GROUP
                        string groupName = ws.Cells[row, 14].Text.Trim();
                        var group = groupList.FirstOrDefault(x => x.Group_Name.ToUpper() == groupName.ToUpper());
                        if (group == null)
                            rowError += $"Invalid Group ({groupName}), ";
                        else
                            ad.ASET_GROUP = group.Group_ID;

                        // COLOR
                        string colorName = ws.Cells[row, 15].Text.Trim();
                        var color = colorList.FirstOrDefault(x => x.COLOR_NAME.ToUpper() == colorName.ToUpper());
                        if (color == null)
                            rowError += $"Invalid Color ({colorName}), ";
                        else
                            ad.ASET_COLOR = color.COLOR_ID;

                        // YEAR
                        int year;
                        int.TryParse(ws.Cells[row, 16].Text.Trim(), out year);
                        ad.ASET_MFG_YEAR = (short?)(year == 0 ? DateTime.Now.Year : year);

                        ad.ASET_NOTE = ws.Cells[row, 17].Text.Trim();

                        // DEFAULT
                        ad.ASET_HEIGHT = "0";
                        ad.ASET_WEIGHT = "0";
                        ad.ASET_IMAGE = "~/";
                        ad.ASET_USED = false;
                        ad.ACT = true;
                        ad.CDT = DateTime.Now;
                        ad.CDU = U;
                        ad.VAR = 1;

                        // ================= RESULT HANDLING =================

                        if (!string.IsNullOrEmpty(rowError))
                        {
                            errorList.Add($"Row {row}: {rowError}");
                        }
                        else
                        {
                            validData.Add(ad);
                        }
                    }
                }

                // ================= IF ERROR FOUND =================
                if (errorList.Count > 0)
                {
                    string errorMsg = string.Join(" | ", errorList.Take(10));

                    TempData["msg"] = "swal('Validation Error','" + errorMsg.Replace("'", "") + "','error');";
                    return RedirectToAction("NewBike");
                }

                // ================= SAVE VALID DATA =================
                db.A_ASET_DATA.AddRange(validData);
                db.SaveChanges();

                TempData["msg"] = "swal('Success','Excel uploaded successfully','success');";
            }
            catch (Exception ex)
            {
                string error = ex.Message;

                if (ex.InnerException != null)
                    error += " | " + ex.InnerException.Message;

                if (ex.InnerException?.InnerException != null)
                    error += " | " + ex.InnerException.InnerException.Message;

                TempData["msg"] = "swal('Error','" + error.Replace("'", "") + "','error');";
            }

            return RedirectToAction("NewBike");
        }

        public ActionResult DownloadExcelTemplateBike()
        {
            string filePath = Server.MapPath("~/Template/AssetUploadTemplateBike.xlsx");

            if (!System.IO.File.Exists(filePath))
            {
                return HttpNotFound("File not found");
            }

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "AssetUploadTemplate.xlsx"
            );
        }

        [KAuth(109)]
        public ActionResult BlikeClosing()
        {
            List<SelectListItem> TT = new List<SelectListItem>();
            TT.Add(new SelectListItem() { Value = "DMAGE", Text = "Damage" });
            TT.Add(new SelectListItem() { Value = "SALE", Text = "Sales" });
            TT.Add(new SelectListItem() { Value = "LOST", Text = "Lost" });
            TT.Add(new SelectListItem() { Value = "OWNR", Text = "Ownership" });
            TT.Add(new SelectListItem() { Value = "TRANSFER", Text = "Transfer" });
            ViewBag.APPLY_TYPE = TT;
            return View();
        }

        [KAuth(109)]
        [HttpPost]
        public ActionResult BlikeClosing(A_ASET_APP bc)
        {
            string U = Session["UserId"].ToString();
            if (ModelState.IsValid)
            {
                //get asset id

                string AssetId = string.Empty;
                string Sql = string.Format(@"select a.aset_id 
                        from pm.vw_aset_by_users a
                        where a.act=1 and a.aset_code_name='{0}' and a.ug_user='{1}'", bc.ASET_ID, U); //code name
                List<string> Assets = db.Database.SqlQuery<string>(sql: Sql).ToList();
                if (Assets.Count() <= 0)
                {
                    TempData["msg"] = "swal('Error..','Asset not found or not permitted to access','error');";
                    return Redirect("BlikeClosing");
                }
                AssetId = Assets.FirstOrDefault().ToString();

                //get user
                JsonResult JR = PMC.CaseAndPenaltyUserData(AssetId, pdt: bc.APPLY_DATE ?? DateTime.Now);
                string json = JsonConvert.SerializeObject(JR.Data);
                JRdata dto = JsonConvert.DeserializeObject<JRdata>(json);
                if (dto.success == false && bc.APPLY_TYPE == "OWNR")
                {
                    TempData["msg"] = "swal('Failed...','Ownership not applied, Assing user with bike first, then try again for Ownership','error');";
                    return Redirect("BlikeClosing");
                }
                else
                {
                    bc.OWNR_ID = dto.USER_NAME;//get from ASET_USER Table
                }
                if (bc.OWNR_ID == null)
                {
                    bc.OWNR_ID = "0";
                }

                Sql = string.Format("update pm.a_aset_data set act='0' where aset_id='{0}'", AssetId);
                db.Database.ExecuteSqlCommand(sql: Sql);
                Sql = string.Format(@"update pm.a_aset_user set act='0' where au_aset='{0}'", AssetId);
                db.Database.ExecuteSqlCommand(sql: Sql);

                A_ASET_APP ap = new A_ASET_APP();
                ap.ASET_ID = AssetId;
                ap.OWNR_ID = bc.OWNR_ID;
                ap.ASET_VALUE = bc.ASET_VALUE ?? 0;
                ap.APPLY_DATE = bc.APPLY_DATE;
                ap.APPLY_REASON = bc.APPLY_REASON;
                ap.APPLY_TYPE = bc.APPLY_TYPE;
                ap.SERVICE_MONTH = Sequence.GetAssetServiceMonth(id: AssetId, ADATE: bc.APPLY_DATE ?? DateTime.Now);//get month from ASET_DATA REG Date to APPLY DATE
                ap.RECEIVE_TYPE = "";
                ap.RECEIVE_DETAIL = "";
                ap.REQU_USER = U;
                ap.APPR_USER = U;
                ap.APPR_DATE = DateTime.Now;
                ap.ACT = true;
                ap.CDT = DateTime.Now;
                ap.CDU = U;
                ap.VAR = 1;
                db.A_ASET_APP.Add(ap);
                db.SaveChanges();
                TempData["msg"] = "swal('Success...','Closing Applied','success');";
                return Redirect("BlikeClosing");
            }
            TempData["msg"] = "swal('Failed...','Closing not Applied','error');";
            return Redirect("BlikeClosing");
        }

        public JsonResult BlikeClosing_Data(string id)
        {
            string Wd, Wa, Wu, Sec, Ad, Aa, Au, Pri;
            var fromAssetData = db.A_ASET_DATA.Where(a => a.ACT == true && a.ASET_CODE_NAME == id).ToList();
            if (fromAssetData.Count == 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            Ad = Wd = fromAssetData.First().ASET_DEPARTMENT;
            Aa = Wa = fromAssetData.First().ASET_AREA;
            Au = Wu = "0";
            Pri = Sec = RP.InspectionAndJobData(Ad, Aa, Au);
            string AssetId = fromAssetData.First().ASET_ID;

            var fromHandOver = db.A_ASET_USER.Where(a => a.AU_ASET == AssetId && (a.AU_ERUSER == null || a.AU_ERUSER == "")).ToList();
            if (fromHandOver.Count > 0)
            {
                Wd = fromHandOver.FirstOrDefault().AU_DEPT;
                Wa = fromHandOver.FirstOrDefault().AU_AREA;
                Wu = fromHandOver.FirstOrDefault().AU_USER;
                Sec = RP.InspectionAndJobData(Wd, Wa, Wu);
            }
            return Json(new { prim = Pri, seco = Sec }, JsonRequestBehavior.AllowGet);
        }



        [KAuth(108)]
        public ActionResult EditBike(string id)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (id == null)
            {
                TempData["msg"] = "swal('Opss..','Try with valid bike','info');";
                return RedirectToAction("BikeList");
            }
            A_ASET_DATA ad = db.A_ASET_DATA.Find(id);
            if (ad == null)
            {
                TempData["msg"] = "swal('Ohh..','No bike found','warning');";
                return RedirectToAction("BikeList");
            }
            CarEdit ce = new CarEdit();
            ce.ASET_ID = ad.ASET_ID;
            ce.ASET_CODE_NAME = ad.ASET_CODE_NAME;
            ViewBag.ASET_BUSINESS = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
            ViewBag.ASET_DEPARTMENT = new SelectList(RP.DepartmentById(ad.ASET_DEPARTMENT), "DEPARTMENT_ID", "DEPARTMENT_NAME");
            ViewBag.ASET_AREA = new SelectList(RP.MyAreaList(UserId: U, UserType: T), "AREA_ID", "AREA_NAME");
            ce.ASET_NOTE = ad.ASET_NOTE;
            return PartialView("_EditBike", ce);
        }

        [KAuth(108)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBike(CarEdit CE)
        {
            if (ModelState.IsValid)
            {
                string U = Session["UserId"].ToString();
                A_ASET_DATA ad = db.A_ASET_DATA.Find(CE.ASET_ID);
                ad.ASET_DEPARTMENT = CE.ASET_DEPARTMENT;
                ad.ASET_AREA = CE.ASET_AREA;
                ad.ASET_NOTE = CE.ASET_NOTE;
                ad.UDT = DateTime.Now;
                ad.UDU = U;
                ad.VAR = ad.VAR + 1;
                db.Entry(ad).State = EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Bike data updated','success');";
                return RedirectToAction("BikeList", new { Asset = CE.ASET_ID });
            }
            TempData["msg"] = "swal('Ohh..','Bike data not updated','warning');";
            return RedirectToAction("BikeList", new { Asset = CE.ASET_ID });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // ----------------  new code here ---------------------
        [KAuth(101)]
        public ActionResult Vehicle()

        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            return View();
        }

        [KAuth(101)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewVehicle(NewVehicleData NA)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (ModelState.IsValid)
            {
                VehicleVM ad = new VehicleVM();
                ad.ID = NA.ID;
                ad.NAME = NA.NAME;
                ad.MODEL = NA.MODEL;
                ad.CREATEDATE = DateTime.Now;

                //Save Aset Data
                //ad.ASET_ID = Sequence.GetSeq(10).ToString();
                //db.Add(ad);
                db.VehicleVM.Add(ad);
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Asset saved succefully','success');";
                return RedirectToAction("Index", new { Asset = ad.NAME });
            }
            //ViewBag.ASET_AREA = new SelectList(RP.MyAreaList(UserId: U, UserType: T), "AREA_ID", "AREA_NAME");
            //ViewBag.ASET_BODY = new SelectList(RP.BodyList(), "BODY_ID", "BODY_NAME");
            //ViewBag.ASET_BRAND = new SelectList(RP.BrandList(), "BRAND_ID", "BRAND_NAME");
            //ViewBag.ASET_COLOR = new SelectList(RP.ColorList(), "COLOR_ID", "COLOR_NAME");
            //ViewBag.ASET_FUEL = new SelectList(RP.FuelList(), "FUEL_ID", "FUEL_NAME");
            //ViewBag.ASET_BUSINESS = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
            //ViewBag.ASET_GROUP = new SelectList(RP.MyGroup(UserId: U, UserType: T), "GROUP_ID", "GROUP_NAME");
            //TempData["msg"] = "swal('Ohh..','Asset saved failed','error');";
            return View(NA);
        }

        // --------------- Depot ---------------------------
        [KAuth(113)]
        public ActionResult NewDepot()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            ViewBag.ASET_AREA = new SelectList(RP.MyAreaList(UserId: U, UserType: T), "AREA_ID", "AREA_NAME");
            ViewBag.ASET_BUSINESS = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
            ViewBag.ASET_GROUP = new SelectList(RP.MyGroup(UserId: U, UserType: T), "GROUP_ID", "GROUP_NAME");
            return View();

        }

        [KAuth(113)]
        [HttpPost]
        public ActionResult NewDepot(NewDepot ND)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (ModelState.IsValid)
            {
                A_ASET_DATA ad = new A_ASET_DATA();
                ad.ASET_TAG = ND.ASET_TAG ?? ND.ASET_CODE_NAME;
                ad.ASET_CODE_NAME = ND.ASET_CODE_NAME;
                ad.ASET_DEPARTMENT = ND.ASET_DEPARTMENT;
                ad.ASET_AREA = ND.ASET_AREA;
                ad.ASET_BODY = "BY0000";
                ad.ASET_BRAND = "BR0000";
                ad.ASET_COLOR = "CL0000";
                ad.ASET_FUEL = "FL0000"; //NC.ASET_FUEL;
                ad.ASET_GROUP = "GR0009";//NMP.ASET_GROUP;//NC.ASET_GROUP;
                ad.ASET_MODEL = ND.ASET_MODEL ?? "N/A";
                ad.ASET_PO_ENGINE = ND.ASET_PO_ENGINE ?? "0";
                ad.ASET_GRN_CHASIS = ND.ASET_GRN_CHASIS ?? "0";
                ad.ASET_CAPACITY = ND.ASET_CAPACITY;
                ad.ASET_PRICE = ND.ASET_PRICE ?? 0;
                ad.ASET_REG_DATE = ND.ASET_REG_DATE ?? DateTime.Now;
                ad.ASET_HEIGHT = "0";
                ad.ASET_WEIGHT = "0";
                ad.ASET_MFG_YEAR = Convert.ToInt16(DateTime.Now.Year);
                ad.ASET_IMAGE = "~/";
                ad.ASET_NOTE = ND.ASET_NOTE;
                ad.ASET_USED = false;
                ad.ACT = true;
                ad.CDT = DateTime.Now;
                ad.CDU = U;
                ad.VAR = 1;
                //Save Aset Data
                ad.ASET_ID = Sequence.GetSeq(10).ToString();
                db.A_ASET_DATA.Add(ad);
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Depot saved succefully','success');";
                return RedirectToAction("NewDepot");
            }
            ViewBag.ASET_AREA = new SelectList(RP.MyAreaList(UserId: U, UserType: T), "AREA_ID", "AREA_NAME");
            ViewBag.ASET_BUSINESS = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
            ViewBag.ASET_GROUP = new SelectList(RP.MyGroup(UserId: U, UserType: T), "GROUP_ID", "GROUP_NAME");
            return View(ND);
        }
    }
}
