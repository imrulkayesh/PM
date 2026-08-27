using Newtonsoft.Json;
using PM.BLL;
using PM.Model;
using PM.ModelsV;
using PM.MyAuth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;


namespace PM.Controllers
{
    [Authorize]
    [CustomHttpReffer]
    public class LicenseController : Controller
    {
        private PMEntities db = new PMEntities();
        ReturnPermission RP = new ReturnPermission();
        GE Sequence = new GE();
        private JsonReaderPMController PMC = new JsonReaderPMController();

        [KAuth(2002)]
        public ActionResult Index()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            string sql_1 = @"SELECT b.LICENSE_NAME, a.DOCUMENT_NAME, a.DAYS, a.REMARKS, a.ACT FROM LIC_LICENSE_DOCUMENT a  LEFT JOIN LIC_LICENSE_NAME b ON a.LICENSE_NAME_ID = b.LICENSE_NAME_ID WHERE a.ACT=1";
            var LicenseDocumentNameData = db.Database.SqlQuery<LicenseDocumnetVM>(sql: sql_1);
            ViewBag.Message = "License Document Name List";
            var R = LicenseDocumentNameData.Take(500).ToList();
            return View(R);
        }

        [KAuth(2002)]
        [HttpGet]
        public ActionResult NewLicenseDocument()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            ViewBag.LICENSE_NAME_ID = new SelectList(RP.LicenseTypeList(), "LICENSE_NAME_ID", "LICENSE_NAME");
            return View();
        }

        [KAuth(2002)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewLicenseDocument(LicenseDocumnetVM LD)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (ModelState.IsValid)
            {
                LIC_LICENSE_DOCUMENT LND = new LIC_LICENSE_DOCUMENT();
                LND.LICENSE_NAME_ID = LD.LICENSE_NAME_ID;
                LND.DOCUMENT_NAME = LD.DOCUMENT_NAME;
                LND.DAYS = LD.DAYS;
                LND.REMARKS = LD.REMARKS;
                LND.ACT = true;
                LND.CDT = DateTime.Now;
                LND.CDU = U;
                LND.LICENSE_DOCUMENT_ID = Sequence.GetSeq(26).ToString();
                db.LIC_LICENSE_DOCUMENT.Add(LND);
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Saved succefully','success');";
                return RedirectToAction("NewLicenseDocument", new { LicenseDocument = LND.LICENSE_DOCUMENT_ID });
            }
            TempData["msg"] = "swal('Ohh..','Saved failed','error');";
            return View();
        }

        [KAuth(2003)]
        public ActionResult IssuingAuthority()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            string sql_1 = @"SELECT a.AUTHORITY_NAME, a.ADDRESS,a.MOBILE,a.EMAIL,a.ACT FROM LIC_ISSUING_AUTHORITY a  WHERE a.ACT=1";
            var IssuingAuthorityData = db.Database.SqlQuery<IssuingAuthorityVM>(sql: sql_1);
            ViewBag.Message = "License Issuing Authority List";
            var R = IssuingAuthorityData.Take(500).ToList();
            return View(R);
        }
        [KAuth(2003)]
        [HttpGet]
        public ActionResult NewIssuingAuthority()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            return View();
        }

        [KAuth(2003)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewIssuingAuthority(IssuingAuthorityVM LIAV)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (ModelState.IsValid)
            {
                LIC_ISSUING_AUTHORITY LIA = new LIC_ISSUING_AUTHORITY();
                LIA.AUTHORITY_NAME = LIAV.AUTHORITY_NAME;
                LIA.ADDRESS = LIAV.ADDRESS;
                LIA.MOBILE = LIAV.MOBILE;
                LIA.EMAIL = LIAV.EMAIL;
                LIA.ACT = true;
                LIA.CDT = DateTime.Now;
                LIA.CDU = U;
                LIA.ISSUING_AUTHORITY_ID = Sequence.GetSeq(27).ToString();
                db.LIC_ISSUING_AUTHORITY.Add(LIA);
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Saved succefully','success');";
                return RedirectToAction("NewIssuingAuthority", new { IssuingAuthority = LIA.ISSUING_AUTHORITY_ID });
            }
            TempData["msg"] = "swal('Ohh..','Saved failed','error');";
            return View();
        }

        [KAuth(2004)]
        public ActionResult LicenseStatus()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            return View();
        }

        [KAuth(2004)]
        public ActionResult NewLicenseStatus()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            return View();
        }

        [KAuth(2006)]
        public ActionResult UserLocationList()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            string UL = string.IsNullOrEmpty(Session["UserLocation"]?.ToString()) ? "0" : Session["UserLocation"].ToString();

                string sql_1 = @"select U.USER_TEXT,U.USER_NAME,U.USER_LOCATION, L.LOCATION_NAME from A_USERS U , LIC_LOCATION L where U.USER_LOCATION= L.LIC_LOCATION_ID";
                var LicenseList = db.Database.SqlQuery<LicenseUserLocationVM>(sql: sql_1);
                ViewBag.Message = "User Location List";
                var R = LicenseList.Take(500).ToList();
                return View(R);
        }

        [KAuth(2008)]
        [HttpGet]
        public ActionResult UserLocation()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            ViewBag.LIC_LOCATION_ID = new SelectList(RP.LocationList(), "LIC_LOCATION_ID", "LOCATION_NAME");
            ViewBag.USER_TEXT = new SelectList(RP.UserList(), "USER_TEXT", "USER_NAME");
            
            return View();
        }

        [KAuth(2008)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserLocation(LicenseUserLocationVM LD)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (ModelState.IsValid)
            {
                string USER_ID = LD.USER_TEXT;
                A_USERS UL = db.A_USERS.Find(USER_ID);

                UL.USER_LOCATION = LD.LIC_LOCATION_ID;

                db.Entry(UL).State = EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','User wise Location saved successfully','success');";
                return RedirectToAction("UserLocation", new { UserId = LD.USER_TEXT });
            }
            TempData["msg"] = "swal('Ohh..','Saved failed','error');";
            return View();
        }

        [KAuth(2010)]
        public ActionResult LicenseCompany()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            string sql_1 = @"SELECT a.COMPANY_ID, a.COMPANY_NAME,a.SHORT_NAME,a.ACT FROM LIC_COMPANY a  WHERE a.ACT=1";
            var LicenseCompanyData = db.Database.SqlQuery<LicenseCompanyVM>(sql: sql_1);
            ViewBag.Message = "License Company List";
            var R = LicenseCompanyData.Take(500).ToList();
            return View(R);
        }
        [KAuth(2010)]
        [HttpGet]
        public ActionResult NewLicenseCompany()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            return View();
        }

        [KAuth(2010)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewLicenseCompany(LicenseCompanyVM LCV)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (ModelState.IsValid)
            {
                LIC_COMPANY LC = new LIC_COMPANY();
                LC.COMPANY_NAME = LCV.COMPANY_NAME;
                LC.SHORT_NAME = LCV.SHORT_NAME;
                LC.ACT = true;
                LC.COMPANY_ID = Sequence.GetSeq(33).ToString();
                db.LIC_COMPANY.Add(LC);
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Saved succefully','success');";
                return RedirectToAction("NewLicenseCompany", new { LicenseCompany = LC.COMPANY_ID });
            }
            TempData["msg"] = "swal('Ohh..','Saved failed','error');";
            return View();
        }

        [KAuth(2011)]
        public ActionResult LicenseLocation()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            string sql_1 = @"SELECT a.LIC_LOCATION_ID, a.LOCATION_NAME,a.SHORT_NAME,a.ADDRESS,a.ACT FROM LIC_LOCATION a  WHERE a.ACT=1";
            var LicenseLocationData = db.Database.SqlQuery<LicenseLocationVM>(sql: sql_1);
            ViewBag.Message = "Location List";
            var R = LicenseLocationData.Take(500).ToList();
            return View(R);
        }
        [KAuth(2011)]
        [HttpGet]
        public ActionResult NewLicenseLocation()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            return View();
        }

        [KAuth(2011)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewLicenseLocation(LicenseLocationVM LLV)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (ModelState.IsValid)
            {
                LIC_LOCATION LL = new LIC_LOCATION();
                LL.LOCATION_NAME = LLV.LOCATION_NAME;
                LL.SHORT_NAME = LLV.SHORT_NAME;
                LL.ADDRESS = LLV.ADDRESS;
                LL.ACT = true;
                LL.LIC_LOCATION_ID = Sequence.GetSeq(34).ToString();
                db.LIC_LOCATION.Add(LL);
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Saved succefully','success');";
                return RedirectToAction("NewLicenseLocation", new { LicenseCompany = LL.LIC_LOCATION_ID });
            }
            TempData["msg"] = "swal('Ohh..','Saved failed','error');";
            return View();
        }

        [KAuth(2006)]
        public ActionResult LicenseMaster()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            string UL = string.IsNullOrEmpty(Session["UserLocation"]?.ToString()) ? "0" : Session["UserLocation"].ToString();

            if (T == "10")
            {
                string sql_1 = @"select DISTINCT L.LICENSE_ID, L.LICENSE_NUMBER, C.COMPANY_NAME, LO.LOCATION_NAME, ITEM.ITEM_NAME, LN.LICENSE_NAME, LAN.AUTHORITY_NAME, L.ISSUE_DATE, L.EXPIRE_DATE, L.LICENSE_HOLDER_MOBILE,L.LICENSE_HOLDER_EMAIL, 
                            L.LICENSE_FEE,L.ENTERTAINMENT_FEE,L.REMARK,L.APPLICATION_DATE, LS.LICENSE_STATUS_NAME,L.CDU
                            from LIC_LICENSE L
                            left join LIC_COMPANY C ON C.COMPANY_ID = L.COMPANY_ID
                            left join LIC_LOCATION LO ON LO.LIC_LOCATION_ID = L.LIC_LOCATION_ID
                            left join LIC_LICENSE_NAME LN ON L.LICENSE_NAME_ID  =  LN.LICENSE_NAME_ID
                            left join LIC_ISSUING_AUTHORITY LAN ON L.ISSUING_AUTHORITY_ID  =  LAN.ISSUING_AUTHORITY_ID
                            left join LIC_STATUS LS ON L.LICENSE_STATUS_ID  =  LS.LICENSE_STATUS_ID
                            LEFT JOIN A_ITEM_MASTER ITEM ON ITEM.ITEM_ID = L.ITEM_ID 
                            WHERE L.ACT = 1";
                var LicenseList = db.Database.SqlQuery<LicenseMasterNM>(sql: sql_1);
                ViewBag.Message = "License List";
                var R = LicenseList.Take(3500).ToList();
                return View(R);
            }
            else
            {
                string sql_1 = @"select DISTINCT L.LICENSE_ID, L.LICENSE_NUMBER, C.COMPANY_NAME, LO.LOCATION_NAME, ITEM.ITEM_NAME, LN.LICENSE_NAME, LAN.AUTHORITY_NAME, L.ISSUE_DATE, L.EXPIRE_DATE, L.LICENSE_HOLDER_MOBILE,L.LICENSE_HOLDER_EMAIL, 
                            L.LICENSE_FEE,L.ENTERTAINMENT_FEE,L.REMARK,L.APPLICATION_DATE, LS.LICENSE_STATUS_NAME,L.CDU
                            from LIC_LICENSE L
                            left join LIC_COMPANY C ON C.COMPANY_ID = L.COMPANY_ID
                            left join LIC_LOCATION LO ON LO.LIC_LOCATION_ID = L.LIC_LOCATION_ID
                            left join LIC_LICENSE_NAME LN ON L.LICENSE_NAME_ID  =  LN.LICENSE_NAME_ID
                            left join LIC_ISSUING_AUTHORITY LAN ON L.ISSUING_AUTHORITY_ID  =  LAN.ISSUING_AUTHORITY_ID
                            left join LIC_STATUS LS ON L.LICENSE_STATUS_ID  =  LS.LICENSE_STATUS_ID
                            LEFT JOIN A_ITEM_MASTER ITEM ON ITEM.ITEM_ID = L.ITEM_ID 
                            left join A_USERS U  ON U.USER_LOCATION  =  L.LIC_LOCATION_ID
                            WHERE L.ACT = 1 AND L.LIC_LOCATION_ID in (" + UL + @")";
                var LicenseList = db.Database.SqlQuery<LicenseMasterNM>(sql: sql_1);
                ViewBag.Message = "License List";
                var R = LicenseList.Take(500).ToList();
                return View(R);
            } 
        }

        [KAuth(2006)]
        public ActionResult NewLicense()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            ViewBag.COMPANY_ID = new SelectList(RP.LICCompanyList(), "COMPANY_ID", "COMPANY_NAME");
            ViewBag.LIC_LOCATION_ID = new SelectList(RP.LocationList(), "LIC_LOCATION_ID", "LOCATION_NAME");
            ViewBag.ISSUING_AUTHORITY_ID = new SelectList(RP.IssuingAuthorityList(), "ISSUING_AUTHORITY_ID", "AUTHORITY_NAME");
            ViewBag.LICENSE_NAME_ID = new SelectList(RP.LicenseTypeList(), "LICENSE_NAME_ID", "LICENSE_NAME");
            ViewBag.LICENSE_STATUS_ID = new SelectList(RP.LicenseStatusList(), "LICENSE_STATUS_ID", "LICENSE_STATUS_NAME");
            ViewBag.ITEM_ID = new SelectList(RP.ItemList(), "ITEM_ID", "ITEM_NAME");

            return View();
        }

        [KAuth(2006)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewLicense(LicenseMasterNM LM)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            if (LM.FileX != null && LM.FileX.ContentLength > 0)
            {

                int fileSize = LM.FileX.ContentLength;
                if (fileSize <= 0 || fileSize > 10500000)
                {
                    TempData["msg"] = "swal('Opps..','File is too larger, Max file size 2MB','warning');";
                    ViewBag.COMPANY_ID = new SelectList(RP.LICCompanyList(), "COMPANY_ID", "COMPANY_NAME");
                    ViewBag.LIC_LOCATION_ID = new SelectList(RP.LocationList(), "LIC_LOCATION_ID", "LOCATION_NAME");
                    ViewBag.ISSUING_AUTHORITY_ID = new SelectList(RP.IssuingAuthorityList(), "ISSUING_AUTHORITY_ID", "AUTHORITY_NAME");
                    ViewBag.LICENSE_NAME_ID = new SelectList(RP.LicenseTypeList(), "LICENSE_NAME_ID", "LICENSE_NAME");
                    ViewBag.LICENSE_STATUS_ID = new SelectList(RP.LicenseStatusList(), "LICENSE_STATUS_ID", "LICENSE_STATUS_NAME");
                    ViewBag.ITEM_ID = new SelectList(RP.ItemList(), "ITEM_ID", "ITEM_NAME");
                    return View();
                }
            }

            if (ModelState.IsValid)
            {
                LIC_LICENSE L = new LIC_LICENSE();

                L.COMPANY_ID = LM.COMPANY_ID;
                L.LIC_LOCATION_ID = LM.LIC_LOCATION_ID;
                L.ITEM_ID = LM.ITEM_ID;
                L.LICENSE_NAME_ID = LM.LICENSE_NAME_ID;
                L.LICENSE_NUMBER = LM.LICENSE_NUMBER;
                L.ISSUING_AUTHORITY_ID = LM.ISSUING_AUTHORITY_ID;
                L.ISSUE_DATE = LM.ISSUE_DATE;
                L.EXPIRE_DATE = LM.EXPIRE_DATE;
                L.LICENSE_HOLDER_MOBILE = LM.LICENSE_HOLDER_MOBILE;
                L.LICENSE_HOLDER_EMAIL = LM.LICENSE_HOLDER_EMAIL;
                L.LICENSE_FEE = LM.LICENSE_FEE;
                L.ENTERTAINMENT_FEE = LM?.ENTERTAINMENT_FEE ?? 0;

                L.REMARK = LM.REMARK;
                L.LICENSE_STATUS_ID = LM.LICENSE_STATUS_ID;
                L.APPLICATION_DATE = LM.APPLICATION_DATE;
                L.ACT = true;
                L.CDT = DateTime.Now;
                L.CDU = U;
                L.LICENSE_ID = Sequence.GetSeq(24).ToString();
                db.LIC_LICENSE.Add(L);

                if (LM.FileX != null && LM.FileX.ContentLength > 0)
                {
                    LIC_LICENSE_IMG I = new LIC_LICENSE_IMG();
                    I.LICENSE_IMG_ID = Guid.NewGuid().ToString();
                    I.IMG_NAME = LM.FileX.FileName;
                    I.ACT = true;
                    I.CDT = DateTime.Now;
                    I.IMG_PATH = Sequence.GetImgPath(107);
                    I.LICENSE_ID = L.LICENSE_ID;
                    db.LIC_LICENSE_IMG.Add(I);

                    //upload file to server
                    var path = System.IO.Path.Combine(Server.MapPath(I.IMG_PATH), I.LICENSE_IMG_ID);
                    LM.FileX.SaveAs(path);
                }

                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Saved succefully','success');";
                return RedirectToAction("NewLicense", new { License = L.LICENSE_ID });
            }

            ViewBag.COMPANY_ID = new SelectList(RP.LICCompanyList(), "COMPANY_ID", "COMPANY_NAME");
            ViewBag.ISSUING_AUTHORITY_ID = new SelectList(RP.IssuingAuthorityList(), "ISSUING_AUTHORITY_ID", "AUTHORITY_NAME");
            ViewBag.LICENSE_NAME_ID = new SelectList(RP.LicenseTypeList(), "LICENSE_NAME_ID", "LICENSE_NAME");
            ViewBag.LICENSE_STATUS_ID = new SelectList(RP.LicenseStatusList(), "LICENSE_STATUS_ID", "LICENSE_STATUS_NAME");
            ViewBag.ITEM_ID = new SelectList(RP.ItemList(), "ITEM_ID", "ITEM_NAME");
            TempData["msg"] = "swal('Ohh..','Saved failed','error');";
            return View(LM);

        }
        [KAuth(2006)]
        [HttpGet]
        public ActionResult StatusUpdate(string id)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (id == null)
            {
                TempData["msg"] = "swal('Opss..','Try with valid data','info');";
                return RedirectToAction("LicenseMaster");
            }
            LIC_LICENSE LIC = db.LIC_LICENSE.Find(id);
            if (LIC == null)
            {
                TempData["msg"] = "swal('Ohh..','No Liceense found','warning');";
                return RedirectToAction("LicenseMaster");
            }
            LicStatusVM LICV = new LicStatusVM();
            LICV.COMPANY_ID = LIC.COMPANY_ID;
            LICV.LIC_LOCATION_ID = LIC.LIC_LOCATION_ID;
            LICV.LICENSE_NAME_ID = LIC.LICENSE_NAME_ID;
            LICV.LICENSE_NUMBER = LIC.LICENSE_NUMBER;
            LICV.ISSUING_AUTHORITY_ID = LIC.ISSUING_AUTHORITY_ID;
            LICV.ITEM_ID = LIC.ITEM_ID;
            LICV.LICENSE_FEE = LIC.LICENSE_FEE;
            LICV.ENTERTAINMENT_FEE = LIC.ENTERTAINMENT_FEE;

            LICV.ISSUE_DATE = LIC.ISSUE_DATE;
            LICV.EXPIRE_DATE = LIC.EXPIRE_DATE;


            LICV.LICENSE_HOLDER_EMAIL = LIC.LICENSE_HOLDER_EMAIL;
            LICV.LICENSE_HOLDER_MOBILE = LIC.LICENSE_HOLDER_MOBILE;
            LICV.LICENSE_STATUS_ID = LIC.LICENSE_STATUS_ID;
            LICV.REMARK = LIC.REMARK;
           
            LICV.LICENSE_ID = LIC.LICENSE_ID;

            ViewBag.COMPANY_ID = new SelectList(RP.LICCompanyList(), "COMPANY_ID", "COMPANY_NAME");
            ViewBag.LIC_LOCATION_ID = new SelectList(RP.LocationList(), "LIC_LOCATION_ID", "LOCATION_NAME");
            ViewBag.ISSUING_AUTHORITY_ID = new SelectList(RP.IssuingAuthorityList(), "ISSUING_AUTHORITY_ID", "AUTHORITY_NAME");
            ViewBag.LICENSE_NAME_ID = new SelectList(RP.LicenseTypeList(), "LICENSE_NAME_ID", "LICENSE_NAME");
            ViewBag.LICENSE_STATUS_ID = new SelectList(RP.LicenseStatusList(), "LICENSE_STATUS_ID", "LICENSE_STATUS_NAME");
            ViewBag.ITEM_ID = new SelectList(RP.ItemList(), "ITEM_ID", "ITEM_NAME");

            return PartialView("_StatusUpdate", LICV);
        }

        [KAuth(2006)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StatusUpdate(LicStatusVM LI)
        {
            if (ModelState.IsValid)
            {
                string U = Session["UserId"].ToString();
                if (LI.FileX != null && LI.FileX.ContentLength > 0)
                {

                    int fileSize = LI.FileX.ContentLength;
                    if (fileSize <= 0 || fileSize > 10500000)
                    {
                        TempData["msg"] = "swal('Opps..','File is too larger, Max file size 2MB','warning');";
                        ViewBag.COMPANY_ID = new SelectList(RP.LICCompanyList(), "COMPANY_ID", "COMPANY_NAME");
                        ViewBag.LIC_LOCATION_ID = new SelectList(RP.LocationList(), "LIC_LOCATION_ID", "LOCATION_NAME");
                        ViewBag.ISSUING_AUTHORITY_ID = new SelectList(RP.IssuingAuthorityList(), "ISSUING_AUTHORITY_ID", "AUTHORITY_NAME");
                        ViewBag.LICENSE_NAME_ID = new SelectList(RP.LicenseTypeList(), "LICENSE_NAME_ID", "LICENSE_NAME");
                        ViewBag.LICENSE_STATUS_ID = new SelectList(RP.LicenseStatusList(), "LICENSE_STATUS_ID", "LICENSE_STATUS_NAME");
                        ViewBag.ITEM_ID = new SelectList(RP.ItemList(), "ITEM_ID", "ITEM_NAME");
                        return View();
                    }
                }

                LIC_LICENSE LIC = db.LIC_LICENSE.Find(LI.LICENSE_ID);

                LIC.COMPANY_ID = LI.COMPANY_ID;
                LIC.LIC_LOCATION_ID = LI.LIC_LOCATION_ID;
                LIC.LICENSE_NAME_ID = LI.LICENSE_NAME_ID;
                LIC.LICENSE_NUMBER = LI.LICENSE_NUMBER;
                LIC.ISSUING_AUTHORITY_ID = LI.ISSUING_AUTHORITY_ID;
                LIC.ITEM_ID = LI.ITEM_ID;
                LIC.LICENSE_FEE = LI.LICENSE_FEE;
                LIC.ENTERTAINMENT_FEE = LI.ENTERTAINMENT_FEE;

                if (LI.ISSUE_DATE != null)
                {
                    LIC.ISSUE_DATE = LI.ISSUE_DATE;
                }
                //if (LI.EXPIRE_DATE != null)
                //{
                //    LIC.EXPIRE_DATE = LI.EXPIRE_DATE;
                //}

                LIC.LICENSE_HOLDER_EMAIL = LI.LICENSE_HOLDER_EMAIL;
                LIC.LICENSE_HOLDER_MOBILE = LI.LICENSE_HOLDER_MOBILE;
                LIC.LICENSE_STATUS_ID = LI.LICENSE_STATUS_ID;
                LIC.REMARK = LI.REMARK;

                if (LI.FileX != null && LI.FileX.ContentLength > 0)
                {
                    LIC_LICENSE_IMG I = new LIC_LICENSE_IMG();
                    I.LICENSE_IMG_ID = Guid.NewGuid().ToString();
                    I.IMG_NAME = LI.FileX.FileName;
                    I.ACT = true;
                    I.CDT = DateTime.Now;
                    I.IMG_PATH = Sequence.GetImgPath(107);
                    I.LICENSE_ID = LI.LICENSE_ID;
                    db.LIC_LICENSE_IMG.Add(I);

                    var path = System.IO.Path.Combine(Server.MapPath(I.IMG_PATH), I.LICENSE_IMG_ID);
                    LI.FileX.SaveAs(path);
                }

                LIC.UDT = DateTime.Now;
                LIC.UDU = U;
                db.Entry(LIC).State = EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','License data updated','success');";
                return RedirectToAction("LicenseMaster", new { LicenseId = LI.LICENSE_ID });
            }
            TempData["msg"] = "swal('Ohh..','Data not updated','warning');";
            return RedirectToAction("LicenseMaster", new { LicenseId = LI.LICENSE_ID });
        }

        [HttpGet]
        [KAuth(2006)]
        public ActionResult RenewLicense(string id)
        {
            var license = db.LIC_LICENSE
                .FirstOrDefault(x => x.LICENSE_ID == id);

            if (license == null)
            {
                return HttpNotFound();
            }

            var model = new LicenseRenewVM
            {
                LICENSE_ID = license.LICENSE_ID,

                COMPANY_ID = license.COMPANY_ID,
                LIC_LOCATION_ID = license.LIC_LOCATION_ID,

                LICENSE_NAME_ID = license.LICENSE_NAME_ID,
                LICENSE_NUMBER = license.LICENSE_NUMBER,

                //ISSUE_DATE = license.ISSUE_DATE,

                //OLD_EXPIRE_DATE = license.EXPIRE_DATE,

                // DateTime? -> string, formatted explicitly (culture-independent)
                ISSUE_DATE = license.ISSUE_DATE.HasValue
                ? license.ISSUE_DATE.Value.ToString("dd/MM/yyyy")
                : "",

                OLD_EXPIRE_DATE = license.EXPIRE_DATE.HasValue
                ? license.EXPIRE_DATE.Value.ToString("dd/MM/yyyy")
                : "",

                LICENSE_FEE = license.LICENSE_FEE,
                ENTERTAINMENT_FEE = license.ENTERTAINMENT_FEE,

                LICENSE_STATUS_ID = license.LICENSE_STATUS_ID,

                REMARK = ""
            };

            ViewBag.COMPANY_ID =
                new SelectList(
                    RP.LICCompanyList(),
                    "COMPANY_ID",
                    "COMPANY_NAME",
                    model.COMPANY_ID
                );

            ViewBag.LIC_LOCATION_ID =
                new SelectList(
                    RP.LocationList(),
                    "LIC_LOCATION_ID",
                    "LOCATION_NAME",
                    model.LIC_LOCATION_ID
                );

            ViewBag.LICENSE_NAME_ID =
                new SelectList(
                    RP.LicenseTypeList(),
                    "LICENSE_NAME_ID",
                    "LICENSE_NAME",
                    model.LICENSE_NAME_ID
                );

            ViewBag.LICENSE_STATUS_ID =
                new SelectList(
                    RP.LicenseStatusList(),
                    "LICENSE_STATUS_ID",
                    "LICENSE_STATUS_NAME",
                    model.LICENSE_STATUS_ID
                );

            return PartialView("_RenewLicense", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [KAuth(2006)]
        public ActionResult RenewLicense(LicenseRenewVM model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
               .Where(x => x.Value.Errors.Count > 0)
               .Select(x => new {
                   Field = x.Key,
                   Errors = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
               });

                System.Diagnostics.Debug.WriteLine(
                    Newtonsoft.Json.JsonConvert.SerializeObject(errors));

                // TEMP: surface it in the swal message so you can see it on the server
                TempData["msg"] =
                    "swal('Debug', '" +
                    Newtonsoft.Json.JsonConvert.SerializeObject(errors).Replace("'", "\\'") +
                    "', 'warning');";

                return RedirectToAction("LicenseMaster", new { LicenseId = model.LICENSE_ID });
            }

            // ----------------------------------------
            // PARSE NEW_EXPIRE_DATE (string -> DateTime), culture-safe
            // ----------------------------------------
            DateTime? newExpireDate = ParseDate(model.NEW_EXPIRE_DATE);

            if (!newExpireDate.HasValue)
            {
                TempData["msg"] =
                    "swal('Opps..','New expire date is required','warning');";

                return RedirectToAction("LicenseMaster", new { LicenseId = model.LICENSE_ID });
            }

            string userId = Session["UserId"].ToString();

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    LIC_LICENSE license = db.LIC_LICENSE.Find(model.LICENSE_ID);

                    if (license == null)
                    {
                        TempData["msg"] = "swal('Opps..','License not found','warning');";
                        return RedirectToAction("LicenseMaster", new { LicenseId = model.LICENSE_ID });
                    }

                    if (license.EXPIRE_DATE.HasValue &&
                        newExpireDate.Value <= license.EXPIRE_DATE.Value)
                    {
                        TempData["msg"] =
                            "swal('Opps..','New expire date must be greater than current expire date','warning');";

                        return RedirectToAction("LicenseMaster", new { LicenseId = model.LICENSE_ID });
                    }

                    if (model.FileX != null && model.FileX.ContentLength > 0)
                    {
                        if (model.FileX.ContentLength > 10500000)
                        {
                            TempData["msg"] =
                                "swal('Opps..','File is too large. Maximum file size is 10 MB','warning');";

                            return RedirectToAction("LicenseMaster", new { LicenseId = model.LICENSE_ID });
                        }
                    }

                    LIC_LICENSE_RENEWAL_HISTORY history = new LIC_LICENSE_RENEWAL_HISTORY();
                    history.LIC_RENEWAL_ID = Sequence.GetSeq(40).ToString();
                    history.LICENSE_ID = license.LICENSE_ID;
                    history.COMPANY_ID = license.COMPANY_ID;
                    history.LIC_LOCATION_ID = license.LIC_LOCATION_ID;
                    history.LICENSE_NUMBER = license.LICENSE_NUMBER;
                    history.LICENSE_NAME_ID = license.LICENSE_NAME_ID;
                    history.OLD_ISSUE_DATE = license.ISSUE_DATE;        // DateTime? from DB — untouched
                    history.OLD_EXPIRE_DATE = license.EXPIRE_DATE;      // DateTime? from DB — untouched
                    history.NEW_ISSUE_DATE = license.ISSUE_DATE;        // DateTime? from DB — untouched
                    history.NEW_EXPIRE_DATE = newExpireDate.Value;      // parsed DateTime — matches entity type exactly
                    history.RENEWAL_DATE = DateTime.Now;
                    history.LICENSE_FEE = model.LICENSE_FEE ?? license.LICENSE_FEE;
                    history.ENTERTAINMENT_FEE = model.ENTERTAINMENT_FEE ?? license.ENTERTAINMENT_FEE;
                    history.LICENSE_STATUS_ID = model.LICENSE_STATUS_ID ?? license.LICENSE_STATUS_ID;
                    history.REMARK = model.REMARK;
                    history.CDU = userId;
                    history.CDT = DateTime.Now;
                    history.ACT = 1;

                    db.LIC_LICENSE_RENEWAL_HISTORY.Add(history);

                    if (model.FileX != null && model.FileX.ContentLength > 0)
                    {
                        LIC_LICENSE_IMG I = new LIC_LICENSE_IMG();
                        I.LICENSE_IMG_ID = Guid.NewGuid().ToString();
                        I.IMG_NAME = model.FileX.FileName;
                        I.ACT = true;
                        I.CDT = DateTime.Now;
                        I.IMG_PATH = Sequence.GetImgPath(107);
                        I.LICENSE_ID = model.LICENSE_ID;
                        db.LIC_LICENSE_IMG.Add(I);

                        var path = System.IO.Path.Combine(Server.MapPath(I.IMG_PATH), I.LICENSE_IMG_ID);
                        model.FileX.SaveAs(path);
                    }

                    license.EXPIRE_DATE = newExpireDate.Value;   // parsed DateTime — matches entity type exactly

                    if (model.LICENSE_FEE.HasValue)
                        license.LICENSE_FEE = (int)model.LICENSE_FEE.Value;

                    if (model.ENTERTAINMENT_FEE.HasValue)
                        license.ENTERTAINMENT_FEE = (int)model.ENTERTAINMENT_FEE.Value;

                    if (!string.IsNullOrEmpty(model.LICENSE_STATUS_ID))
                        license.LICENSE_STATUS_ID = model.LICENSE_STATUS_ID;

                    license.REMARK = model.REMARK;
                    license.UDT = DateTime.Now;
                    license.UDU = userId;

                    db.Entry(license).State = EntityState.Modified;
                    db.SaveChanges();
                    transaction.Commit();

                    TempData["msg"] = "swal('Success..','License renewed successfully','success');";
                    return RedirectToAction("LicenseMaster", new { LicenseId = model.LICENSE_ID });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    TempData["msg"] = "swal('Error..','License renewal failed','error');";
                    return RedirectToAction("LicenseMaster", new { LicenseId = model.LICENSE_ID });
                }
            }
        }

        public ActionResult DownloadAttachFile(string id)
        {
            var dbDATA = db.LIC_LICENSE_IMG
                .Where(a => a.LICENSE_ID == id)
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
            var GetFileGUIDname = latestImage.LICENSE_IMG_ID;
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

        // ----------------------------------------
        // Helper: culture-safe manual date parser
        // Tries dd/MM/yyyy (app convention) and yyyy-MM-dd (HTML5 <input type="date">)
        // ----------------------------------------
        private DateTime? ParseDate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            DateTime result;
            string[] formats = { "dd/MM/yyyy", "yyyy-MM-dd" };

            if (DateTime.TryParseExact(value, formats,
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out result))
            {
                return result;
            }

            return null;
        }
    }

    
}