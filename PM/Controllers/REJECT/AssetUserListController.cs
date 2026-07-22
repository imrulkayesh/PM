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
    public class AssetUserListController : Controller
    {
        private pmdbEntities db = new pmdbEntities();

        // KSA Users
        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {
                var getId = Session["UserId"].ToString();

                var MyGroupList = db.P_USERS_ASGR.Where(b => b.USER_TEXT == getId && b.UAP_ACTV == true).Select(s => s.ASGR_TEXT).ToList();
                var MyBeunList = db.P_USERS_BEUN.Where(b => b.USER_TEXT == getId && b.UBP_ACTV == true).Select(s => s.BEUN_TEXT).ToList();
                var MyDept = db.P_DEPT.Where(d => MyBeunList.Contains(d.DEPT_BEUN)).Select(s => s.DEPT_TEXT).ToList();
                //  var MyAssetList = db.P_ASSET.Where(a => MyGroupList.Contains(a.ASET_ASGR) && MyDept.Contains(a.ASET_DEPT)).Where(a => a.ISBOOKED == true).Select(s => s.ASET_OID);
                var AllOwner = db.P_OWNR.Where(o => MyDept.Contains(o.OWNR_DEPT)).Include(p => p.P_AREA).Include(p => p.P_DEPT).Include(p => p.P_ASSET).Where(a => a.OWNR_ACTV == true).ToList();
                return View(AllOwner);
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Time Expired, please login!');window.location.href = '/Home/Login';</script>");
            }
        }

        // GET: AssetUserList/Create
        public ActionResult NewAssetUser()
        {
            if (Session["UserId"] != null)
            {
                var getId = Session["UserId"].ToString();

                var MyBeunList = db.P_USERS_BEUN.Where(b => b.USER_TEXT == getId && b.UBP_ACTV == true).Select(s => s.BEUN_TEXT).ToList();
                //ViewBag.OWNR_AREA = new SelectList(db.P_AREA.Where(b => b.AREA_ACTV == true && b.P_THNA.P_DIST.DIST_CUNT.Contains("KSA")), "AREA_TEXT", "AREA_NAME");
                var Areas = db.P_AREA.Where(s => s.AREA_ACTV == true && s.P_THNA.P_DIST.DIST_CUNT.Contains("KSA"))
                            .Select(s => new
                            {
                                AREA_TEXT = s.AREA_TEXT,
                                AREA_NAME = s.AREA_NAME + "-" + s.AREA_TEXT
                            })
                            .ToList();
                ViewBag.OWNR_AREA = new SelectList(Areas, "AREA_TEXT", "AREA_NAME");
                ViewBag.AllComp = new SelectList(db.P_BEUN.Where(c=>c.BEUN_ACTV==true && c.P_COMP.COMP_CUNT.Contains("KSA")),"BEUN_TEXT", "BEUN_NAME");
                return View();
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Time Expired, please login!');window.location.href = '/Home/Login';</script>");
            }
            //          ViewBag.OWNR_AREA = new SelectList(db.P_AREA, "AREA_TEXT", "AREA_NAME");
            //            ViewBag.OWNR_DEPT = new SelectList(db.P_DEPT, "DEPT_TEXT", "DEPT_NAME");
        }
        public ActionResult getDept(string CompId)
        {
            if (Session["UserId"] != null)
            {
              //  var getId = Session["UserId"].ToString();                
                var returnAsetList = from q in db.P_DEPT
                                     where q.DEPT_BEUN == CompId
                                     && q.DEPT_ACTV == true
                                     select new { q.DEPT_TEXT, q.DEPT_NAME };
                return Json(returnAsetList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Time Expired, please login!');window.location.href = '/Home/Login';</script>");
            }
        }
        public ActionResult Autocomplete(string term)
        {
            var Asset = db.P_OWNR.Select(a => a.OWNR_OID).ToList();
            var filteredItems = Asset.Where(
                item => item.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
                );
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }

        // POST: AssetUserList/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewAssetUser([Bind(Include = "OWNR_OID,OWNR_TEXT,OWNR_USER,OWNR_NAME,OWNR_CONTACT,OWNR_ADDRESS,OWNR_IMAG,OWNR_TAKE,OWNR_RELASE,OWNR_AST_IMAG,OWNR_DED_IMAG,OWNR_TYPE,OWNR_RESIGN,OWNR_RES_DATE,OWNR_NOTE,OWNR_ASET,OWNR_DEPT,OWNR_AREA,OWNR_ACTV,CUSR,EUSR,CDT,EDT,UDT,VER,OWNR_EMAIL")] P_OWNR p_OWNR, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var GetSeq = db.P_SEQ.Where(a => a.SEQ_TEXT.Equals(10)).FirstOrDefault().SEQ_NUMBER;
                int OID = ((Convert.ToInt32(GetSeq) + 1));

                if (file != null && file.ContentLength > 0 && file.ContentLength < 2100000) //Max length 2100000 bytes = 2 MB
                {
                    var Upload_Path = db.P_SEQ.Where(a => a.SEQ_TEXT == 700).FirstOrDefault().SEQ_NAME;//find from App Setup table
                    var File_Ext = System.IO.Path.GetFileNameWithoutExtension(file.ContentType);
                    if (File_Ext == "jpg" || File_Ext == "png" || File_Ext == "jpeg")
                    {
                        var newFileName = Guid.NewGuid().ToString() + "." + File_Ext;
                        //store the file inside ~/App_Data/uploads folder
                        var path = System.IO.Path.Combine(Server.MapPath(Upload_Path), newFileName);
                        file.SaveAs(path);
                        //Picture detail Save to database
                        p_OWNR.OWNR_IMAG = Upload_Path + "/" + newFileName;
                    }
                }else
                {
                    TempData["msg"] = "<div id='snackbar'>Image upload failed!</div><script>var x = document.getElementById('snackbar');  x.className = 'show success'; setTimeout(function() { x.className = x.className.replace('show', ''); }, 3000);</script>";
                }
                p_OWNR.OWNR_OID = OID.ToString();
                p_OWNR.OWNR_TYPE = "Staff";
                p_OWNR.OWNR_RESIGN = "N";
                p_OWNR.OWNR_ASET = "0";
                p_OWNR.OWNR_ACTV = true;
                db.P_OWNR.Add(p_OWNR);
                db.SaveChanges();
                //Update Last Number
                P_SEQ p_SEQ = db.P_SEQ.Find(10);
                p_SEQ.SEQ_NUMBER = OID;
                db.Entry(p_SEQ).State = EntityState.Modified;
                db.SaveChanges();
                //success messages
                TempData["msg"] = "<div id='snackbar'>New user added succefully.</div><script>var x = document.getElementById('snackbar');  x.className = 'show warning'; setTimeout(function() { x.className = x.className.replace('show', ''); }, 3000);</script>";
                return RedirectToAction("Index");
            }
            ViewBag.OWNR_AREA = new SelectList(db.P_AREA, "AREA_TEXT", "AREA_NAME", p_OWNR.OWNR_AREA);
            ViewBag.OWNR_DEPT = new SelectList(db.P_DEPT, "DEPT_TEXT", "DEPT_NAME", p_OWNR.OWNR_DEPT);
            //failed messages
            return View(p_OWNR);
        }

        // GET: AssetUserList/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            P_OWNR p_OWNR = db.P_OWNR.Find(id);
            if (p_OWNR == null)
            {
                return HttpNotFound();
            }
            ViewBag.OWNR_AREA = new SelectList(db.P_AREA, "AREA_TEXT", "AREA_NAME", p_OWNR.OWNR_AREA);
            ViewBag.OWNR_ASET = new SelectList(db.P_ASSET, "ASET_OID", "ASET_TEXT", p_OWNR.OWNR_ASET);
            ViewBag.OWNR_DEPT = new SelectList(db.P_DEPT, "DEPT_TEXT", "DEPT_NAME", p_OWNR.OWNR_DEPT);
            return View(p_OWNR);
        }

        // POST: AssetUserList/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OWNR_OID,OWNR_TEXT,OWNR_USER,OWNR_NAME,OWNR_CONTACT,OWNR_ADDRESS,OWNR_IMAG,OWNR_TAKE,OWNR_RELASE,OWNR_AST_IMAG,OWNR_DED_IMAG,OWNR_TYPE,OWNR_RESIGN,OWNR_RES_DATE,OWNR_NOTE,OWNR_ASET,OWNR_DEPT,OWNR_AREA,OWNR_ACTV,CUSR,EUSR,CDT,EDT,UDT,VER,OWNR_EMAIL")] P_OWNR p_OWNR)
        {
            if (ModelState.IsValid)
            {
                db.Entry(p_OWNR).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OWNR_AREA = new SelectList(db.P_AREA, "AREA_TEXT", "AREA_NAME", p_OWNR.OWNR_AREA);
            ViewBag.OWNR_ASET = new SelectList(db.P_ASSET, "ASET_OID", "ASET_TEXT", p_OWNR.OWNR_ASET);
            ViewBag.OWNR_DEPT = new SelectList(db.P_DEPT, "DEPT_TEXT", "DEPT_NAME", p_OWNR.OWNR_DEPT);
            return View(p_OWNR);
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
