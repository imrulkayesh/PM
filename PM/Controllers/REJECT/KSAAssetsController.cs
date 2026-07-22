using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PM.Models;

namespace PM.Controllers
{
    public class KSAAssetsController : Controller
    {
        private pmdbEntities db = new pmdbEntities();

        // GET: KSAAssets
        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {
                var getId = Session["UserId"].ToString();

                var MyGroupList = db.P_USERS_ASGR.Where(b => b.USER_TEXT == getId && b.UAP_ACTV == true).Select(s => s.ASGR_TEXT).ToList();
                var MyBeunList = db.P_USERS_BEUN.Where(b => b.USER_TEXT == getId && b.UBP_ACTV == true).Select(s => s.BEUN_TEXT).ToList();
                var MyDept = db.P_DEPT.Where(d => MyBeunList.Contains(d.DEPT_BEUN)).Select(s => s.DEPT_TEXT).ToList();
                var MyAssetList = db.P_ASSET.Where(a => MyGroupList.Contains(a.ASET_ASGR) && MyDept.Contains(a.ASET_DEPT)).Where(e => e.ASET_ACTV == true && e.ISBOOKED == false)
                    .Include(p => p.P_AREA).Include(p => p.P_ASGR).Include(p => p.P_BRAND).Include(p => p.P_DEPT).Include(p => p.P_ASSET_DAMAGE)
                    .Take(10).ToList();
                return View(MyAssetList);
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Time Expired, please login!');window.location.href = '/Home/Login';</script>");
            }
        }

        public ActionResult getDept(string BeuId)
        {
            var returnDept = from q in db.P_DEPT
                             where q.DEPT_BEUN == BeuId
                             select new { q.DEPT_TEXT, q.DEPT_NAME };
            return Json(returnDept, JsonRequestBehavior.AllowGet);
        }

        // GET: KSAAssets/Create
        public ActionResult NewAsset()
        {
            if (Session["UserId"] != null)
            {
                var getId = Session["UserId"].ToString();

                var MyGroupList = db.P_USERS_ASGR.Where(b => b.USER_TEXT == getId && b.UAP_ACTV == true).Select(s => s.ASGR_TEXT).ToList();
                ViewBag.ASET_ASGR = new SelectList(db.P_ASGR.Where(n => MyGroupList.Contains(n.ASGR_TEXT) && n.ASGR_ACTV==true), "ASGR_TEXT", "ASGR_NAME");
                ViewBag.ASET_BRND = new SelectList(db.P_BRAND.Where(b => b.BRND_ACTV == true), "BRND_TEXT", "BRND_NAME");
                ViewBag.ASET_AREA = new SelectList(db.P_AREA.Where(b => b.AREA_ACTV == true && b.P_THNA.P_DIST.DIST_CUNT.Contains("KSA")), "AREA_TEXT", "AREA_NAME");
                var MyBEUN = db.P_USERS_BEUN.Where(b => b.USER_TEXT == getId && b.UBP_ACTV == true).Select(s => s.BEUN_TEXT).ToList();
                ViewBag.AllBEUN = new SelectList(db.P_BEUN.Where(u => MyBEUN.Contains(u.BEUN_TEXT)), "BEUN_TEXT", "BEUN_NAME");
                ViewBag.ASET_DEPT = new SelectList(db.P_DEPT, "DEPT_TEXT", "DEPT_NAME");
                return View();
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Time Expired, please login!');window.location.href = '/Home/Login';</script>");
            }
        }

        // POST: KSAAssets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewAsset([Bind(Include = "ASET_OID,ASET_TEXT,ASET_NAME,ASET_MODL,ASET_TYPE,ENGN_PONO,CHSS_GRN,ASET_CPCT,ASET_PRIC,REG_DATE,ASET_IMAG,FUEL_TYPE,BODY_TYPE,ASET_HIGT,ASET_WIGT,ASET_COLR,MFG_YEAR,ASET_NOTE,ISBOOKED,ASET_ASGR,ASET_DEPT,ASET_AREA,ASET_BRND,ASET_ACTV,CUSR,EUSR,CDT,EDT,UDT,VER,ASET_CLSE")] P_ASSET p_ASSET, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0 && file.ContentLength < 2100000) //Max length 2100000 bytes = 2 MB
                {
                    var Upload_Path = db.P_SEQ.Where(a => a.SEQ_TEXT == 701).FirstOrDefault().SEQ_NAME;//find from App Setup table
                    var File_Ext = System.IO.Path.GetFileNameWithoutExtension(file.ContentType);
                    if (File_Ext == "jpg" || File_Ext == "png" || File_Ext == "jpeg")
                    {
                        var newFileName = Guid.NewGuid().ToString() + "." + File_Ext;
                        //store the file inside ~/App_Data/uploads folder
                        var path = System.IO.Path.Combine(Server.MapPath(Upload_Path), newFileName);
                        file.SaveAs(path);
                        //Picture detail Save to database
                        p_ASSET.ASET_IMAG = Upload_Path + "/" + newFileName;
                    }
                }
                var GetSeq = db.P_SEQ.Where(a => a.SEQ_TEXT.Equals(20)).FirstOrDefault().SEQ_NUMBER;
                int OID = ((Convert.ToInt32(GetSeq) + 1));
                p_ASSET.ASET_OID = OID.ToString();
                p_ASSET.ISBOOKED = false;
                p_ASSET.ASET_ACTV = true;
                p_ASSET.ASET_CLSE = "N";
                p_ASSET.CDT = System.DateTime.Now;
                db.P_ASSET.Add(p_ASSET);
                db.SaveChanges();
                //Update Last Number
                P_SEQ p_SEQ = db.P_SEQ.Find(20);
                p_SEQ.SEQ_NUMBER = OID;
                db.Entry(p_SEQ).State = EntityState.Modified;
                db.SaveChanges();
                //Failed Messages
                return RedirectToAction("Index");
            }
            //Failed Messages
            //return RedirectToAction("Index");
            return View(p_ASSET);
        }

        // GET: KSAAssets/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            P_ASSET p_ASSET = db.P_ASSET.Find(id);
            if (p_ASSET == null)
            {
                return HttpNotFound();
            }
            ViewBag.ASET_AREA = new SelectList(db.P_AREA, "AREA_TEXT", "AREA_NAME", p_ASSET.ASET_AREA);
            ViewBag.ASET_ASGR = new SelectList(db.P_ASGR, "ASGR_TEXT", "ASGR_NAME", p_ASSET.ASET_ASGR);
            ViewBag.ASET_BRND = new SelectList(db.P_BRAND, "BRND_TEXT", "BRND_NAME", p_ASSET.ASET_BRND);
            ViewBag.ASET_DEPT = new SelectList(db.P_DEPT, "DEPT_TEXT", "DEPT_NAME", p_ASSET.ASET_DEPT);
            ViewBag.ASET_OID = new SelectList(db.P_ASSET_DAMAGE, "ASET_OID", "DMG_APP", p_ASSET.ASET_OID);
            return View(p_ASSET);
        }

        // POST: KSAAssets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ASET_OID,ASET_TEXT,ASET_NAME,ASET_MODL,ASET_TYPE,ENGN_PONO,CHSS_GRN,ASET_CPCT,ASET_PRIC,REG_DATE,ASET_IMAG,FUEL_TYPE,BODY_TYPE,ASET_HIGT,ASET_WIGT,ASET_COLR,MFG_YEAR,ASET_NOTE,ISBOOKED,ASET_ASGR,ASET_DEPT,ASET_AREA,ASET_BRND,ASET_ACTV,CUSR,EUSR,CDT,EDT,UDT,VER,ASET_CLSE")] P_ASSET p_ASSET)
        {
            if (ModelState.IsValid)
            {
                db.Entry(p_ASSET).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ASET_AREA = new SelectList(db.P_AREA, "AREA_TEXT", "AREA_NAME", p_ASSET.ASET_AREA);
            ViewBag.ASET_ASGR = new SelectList(db.P_ASGR, "ASGR_TEXT", "ASGR_NAME", p_ASSET.ASET_ASGR);
            ViewBag.ASET_BRND = new SelectList(db.P_BRAND, "BRND_TEXT", "BRND_NAME", p_ASSET.ASET_BRND);
            ViewBag.ASET_DEPT = new SelectList(db.P_DEPT, "DEPT_TEXT", "DEPT_NAME", p_ASSET.ASET_DEPT);
            ViewBag.ASET_OID = new SelectList(db.P_ASSET_DAMAGE, "ASET_OID", "DMG_APP", p_ASSET.ASET_OID);
            return View(p_ASSET);
        }

        public ActionResult AddNewAcce(string id,string AsgrId)
        {
            ViewBag.ACCE_ASAC = new SelectList(db.P_ASAC.Where(c=>c.ASAC_ASGR==AsgrId), "ASAC_TEXT", "ASAC_NAME");
            ViewBag.ACCE_ASET = new SelectList(db.P_ASSET.Where(a=>a.ASET_OID==id), "ASET_OID", "ASET_NAME");
            //ViewBag.ACCE_FUSER = new SelectList(db.P_OWNR, "OWNR_OID", "OWNR_TEXT");
            //ViewBag.ACCE_TRNS = new SelectList(db.P_ASSET_TRNS, "TRNS_TEXT", "TRNS_FUSER");
            //ViewBag.ACCE_TUSER = new SelectList(db.P_OWNR, "OWNR_OID", "OWNR_TEXT");
            //            return View();
            return PartialView("AddNewAcce");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewAcce(P_ASSET_ACCE p_ASSET_ACCE)
        {
            if (ModelState.IsValid)
            {
                p_ASSET_ACCE.ACCE_TEXT = p_ASSET_ACCE.ACCE_ASET + p_ASSET_ACCE.ACCE_ASAC + System.DateTime.Now.Day;
                p_ASSET_ACCE.ACCE_FUSER = "0";
                p_ASSET_ACCE.ACCE_TUSER = "0";
                p_ASSET_ACCE.ACCE_TRNS = "";
                p_ASSET_ACCE.ACCE_ACTV = true;
                db.P_ASSET_ACCE.Add(p_ASSET_ACCE);
                db.SaveChanges();
                //success
                return RedirectToAction("Index");
            }
            //failed
            return View();
        }
        public ActionResult AcceList(string id)
        {
            var AllAcce = db.P_ASSET_ACCE.Where(a => a.ACCE_ASET == id && a.ACCE_ACTV == true).ToList();
            return View(AllAcce);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
