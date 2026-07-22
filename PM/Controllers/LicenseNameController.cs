using PM.BLL;
using PM.Model;
using PM.ModelsV;
using PM.MyAuth;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;


namespace PM.Controllers
{
    [Authorize]
    [CustomHttpReffer]
    public class LicenseNameController : Controller
    {
        private PMEntities db = new PMEntities();
        ReturnPermission RP = new ReturnPermission();
        GE Sequence = new GE();
        //private JsonReaderPMController PMC = new JsonReaderPMController();
        //private PMExcelDataLoader EDL = new PMExcelDataLoader();

        [KAuth(2001)]
        [HttpGet]
        public ActionResult Index()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            string sql_1 = @"SELECT a.LICENSE_NAME_ID,a.LICENSE_NAME,b.LICENSE_TYPE_NAME,a.LICENSE_TYPE_ID,a.APPLICATION_DAYS,a.NOTIFY_DAYS,a.REMARKS,a.ACT 
                             FROM LIC_LICENSE_NAME a LEFT JOIN LIC_LICENSE_TYPE b ON a.LICENSE_TYPE_ID = b.LICENSE_TYPE_ID where a.ACT=1";
            var LicenseNameData = db.Database.SqlQuery<LicenseVM>(sql: sql_1);
            ViewBag.Message = "License Name List";
            var R = LicenseNameData.Take(500).ToList();
            return View(R);
        }

        [KAuth(2001)]
        public ActionResult NewLicense()
        {
            ViewBag.LICENSE_TYPE_ID = new SelectList(RP.LicenseTypeNameList(), "LICENSE_TYPE_ID", "LICENSE_TYPE_NAME");
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            return View();
        }

        [KAuth(2001)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewLicense(LicenseVM LN)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (ModelState.IsValid)
            {
                LIC_LICENSE_NAME LICN = new LIC_LICENSE_NAME();
                LICN.LICENSE_NAME = LN.LICENSE_NAME;
                LICN.LICENSE_TYPE_ID = LN.LICENSE_TYPE_ID;
                LICN.APPLICATION_DAYS = LN.APPLICATION_DAYS;
                LICN.NOTIFY_DAYS = LN.NOTIFY_DAYS;
                LICN.REMARKS = LN.REMARKS;
                LICN.ACT = true;
                LICN.CDT = DateTime.Now;
                LICN.CDU = U;
                LICN.LICENSE_NAME_ID = Sequence.GetSeq(25).ToString();
                db.LIC_LICENSE_NAME.Add(LICN);
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Saved succefully','success');";
                return RedirectToAction("NewLicense", new { LicenseName = LICN.LICENSE_NAME_ID });
            }
            TempData["msg"] = "swal('Ohh..','Saved failed','error');";
            return View();
        }
    }
}