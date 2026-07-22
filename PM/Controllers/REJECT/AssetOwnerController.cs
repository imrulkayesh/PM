using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PM.Models;
using Oracle.ManagedDataAccess.Client;

namespace PM.Controllers
{
    public class AssetOwnerController : Controller
    {
        private pmdbEntities db = new pmdbEntities();

        // GET: AssetOwner
        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {
                var getId = Session["UserId"].ToString();

                var MyGroupList = db.P_USERS_ASGR.Where(b => b.USER_TEXT == getId && b.UAP_ACTV == true).Select(s => s.ASGR_TEXT).ToList();
                var MyBeunList = db.P_USERS_BEUN.Where(b => b.USER_TEXT == getId && b.UBP_ACTV == true).Select(s => s.BEUN_TEXT).ToList();
                var MyDept = db.P_DEPT.Where(d => MyBeunList.Contains(d.DEPT_BEUN)).Select(s => s.DEPT_TEXT).ToList();
                var MyAssetList = db.P_ASSET.Where(a => MyGroupList.Contains(a.ASET_ASGR) && MyDept.Contains(a.ASET_DEPT)).Where(a => a.ISBOOKED == true).Select(s => s.ASET_OID);
                var AllOwner = db.P_OWNR.Where(o => MyAssetList.Contains(o.OWNR_ASET)).Include(p => p.P_AREA).Include(p => p.P_DEPT).Include(p => p.P_ASSET).Where(a=>a.OWNR_ACTV==true).ToList();// && o.OWNR_RES_DATE == null
                return View(AllOwner);
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Time Expired, please login!');window.location.href = '/Home/Login';</script>");
            }
        }
        public ActionResult getSuggession(string term)
        {
            if (Session["UserId"] != null)
            {
                var getId = Session["UserId"].ToString();

                var MyGroupList = db.P_USERS_ASGR.Where(b => b.USER_TEXT == getId && b.UAP_ACTV == true).Select(s => s.ASGR_TEXT).ToList();
                var MyBeunList = db.P_USERS_BEUN.Where(b => b.USER_TEXT == getId && b.UBP_ACTV == true).Select(s => s.BEUN_TEXT).ToList();
                var MyDept = db.P_DEPT.Where(d => MyBeunList.Contains(d.DEPT_BEUN)).Select(s => s.DEPT_TEXT).ToList();
                var MyAssetList = db.P_ASSET.Where(a => MyGroupList.Contains(a.ASET_ASGR) && MyDept.Contains(a.ASET_DEPT)).Select(s => s.ASET_NAME).ToList();
                var filteredItems = MyAssetList.Where(
                  item => item.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
                  );
                return Json(filteredItems, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Time Expired, please login!');window.location.href = '/Home/Login';</script>");
            }
        }
        public ActionResult SingleOwnerView(string AssetName)
        {
            if (Session["UserId"] != null)
            {
                var getId = Session["UserId"].ToString();

                var MyGroupList = db.P_USERS_ASGR.Where(b => b.USER_TEXT == getId && b.UAP_ACTV == true).Select(s => s.ASGR_TEXT).ToList();
                var MyBeunList = db.P_USERS_BEUN.Where(b => b.USER_TEXT == getId && b.UBP_ACTV == true).Select(s => s.BEUN_TEXT).ToList();
                var MyDept = db.P_DEPT.Where(d => MyBeunList.Contains(d.DEPT_BEUN)).Select(s => s.DEPT_TEXT).ToList();
                var MyAssetList = db.P_ASSET.Where(a => MyGroupList.Contains(a.ASET_ASGR) && MyDept.Contains(a.ASET_DEPT)).Where(a => a.ASET_NAME == AssetName && a.ISBOOKED == true).Select(s => s.ASET_OID);
                var SingleOwner = db.P_OWNR.Where(o => MyAssetList.Contains(o.OWNR_ASET)).Where(a => a.OWNR_ACTV == true).ToList();// && o.OWNR_RES_DATE == null
                return PartialView("_SingleOwnerView", SingleOwner);
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Time Expired, please login!');window.location.href = '/Home/Login';</script>");
            }
        }
        public ActionResult ReleaseOwner(string id)
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
            return View(p_OWNR);
        }
        [HttpPost]
        public ActionResult ReleaseOwner(P_OWNR p_OWNR)
        {
            if (ModelState.IsValid)
            {
                if (p_OWNR.OWNR_ACTV != null && p_OWNR.OWNR_ACTV == false)
                {
                    p_OWNR.OWNR_RESIGN = "Y";
                    p_OWNR.OWNR_RES_DATE = System.DateTime.UtcNow;
                    p_OWNR.OWNR_ACTV = false;
                    p_OWNR.EDT = System.DateTime.Now;
                    p_OWNR.UDT = System.DateTime.Now;
                    //Asset Marked as Free [reassing to another retailer]
                    var param1 = new OracleParameter("ASET_ID", OracleDbType.Int32, p_OWNR.OWNR_ASET, ParameterDirection.Input);
                    var param2 = new OracleParameter("BOOKING", OracleDbType.Int32, 0, ParameterDirection.Input);
                    db.Database.ExecuteSqlCommand("BEGIN P_ISBOOKED_BOOKING(:ASET_ID, :BOOKING); end;", param1, param2);
                }
                db.Entry(p_OWNR).State = EntityState.Modified;
                db.SaveChanges();
                //return Content(@"<body>
                //       <script type='text/javascript'>
                //         window.close();
                //       </script>
                //     </body> ");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        public ActionResult MakeDamage(string id)
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
            p_OWNR.OWNR_RESIGN = "Y";
            p_OWNR.OWNR_RES_DATE = System.DateTime.UtcNow;
            p_OWNR.OWNR_ACTV = false;
            p_OWNR.EDT = System.DateTime.Now;
            p_OWNR.UDT = System.DateTime.Now;
            db.Entry(p_OWNR).State = EntityState.Modified;
            db.SaveChanges();
            //Asset Marked as Damages
            var param1 = new OracleParameter("ASET_ID", OracleDbType.Int32, p_OWNR.OWNR_ASET, ParameterDirection.Input);
            var param2 = new OracleParameter("DAMAGED", OracleDbType.Int32, 0, ParameterDirection.Input);
            db.Database.ExecuteSqlCommand("BEGIN P_ASET_ACTV(:ASET_ID, :DAMAGED); end;", param1, param2);
            return RedirectToAction("Index");
        }

        // GET: AssetOwner/Create
        public ActionResult NewOwner()
        {
            if (Session["UserId"] != null)
            {
                var getId = Session["UserId"].ToString();

                var MyBeunList = db.P_USERS_BEUN.Where(b => b.USER_TEXT == getId && b.UBP_ACTV == true).Select(s => s.BEUN_TEXT).ToList();
                ViewBag.OWNR_AREA = new SelectList(db.P_AREA.Where(b => b.AREA_ACTV == true && b.P_THNA.P_DIST.DIST_CUNT.Contains("BD")), "AREA_TEXT", "AREA_NAME");
                ViewBag.OWNR_DEPT = new SelectList(db.P_DEPT.Where(e => MyBeunList.Contains(e.DEPT_BEUN)), "DEPT_TEXT", "DEPT_NAME");
                return View();
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Time Expired, please login!');window.location.href = '/Home/Login';</script>");
            }
        }

        public ActionResult getAssetList(string deptId)
        {
            if (Session["UserId"] != null)
            {
                var getId = Session["UserId"].ToString();
                var MyGroupList = db.P_USERS_ASGR.Where(b => b.USER_TEXT == getId && b.UAP_ACTV == true).Select(s => s.ASGR_TEXT).ToList();
                var returnAsetList = from q in db.P_ASSET
                                     where q.ASET_DEPT == deptId
                                     && q.ASET_ACTV == true
                                     && q.ISBOOKED == false
                                     && MyGroupList.Contains(q.ASET_ASGR)
                                     select new { q.ASET_OID, q.ASET_TEXT };
                return Json(returnAsetList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Time Expired, please login!');window.location.href = '/Home/Login';</script>");
            }
        }

        public ActionResult Autocomplete(string term)
        {
            var Asset = db.P_OWNR.Select(a => a.OWNR_USER).ToList();
            var filteredItems = Asset.Where(
                item => item.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
                );
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }
        // POST: AssetOwner/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewOwner([Bind(Include = "OWNR_OID,OWNR_TEXT,OWNR_USER,OWNR_NAME,OWNR_CONTACT,OWNR_ADDRESS,OWNR_IMAG,OWNR_TAKE,OWNR_RELASE,OWNR_AST_IMAG,OWNR_DED_IMAG,OWNR_TYPE,OWNR_RESIGN,OWNR_RES_DATE,OWNR_NOTE,OWNR_ASET,OWNR_DEPT,OWNR_AREA,OWNR_ACTV,CUSR,EUSR,CDT,EDT,UDT,VER")] P_OWNR p_OWNR, IEnumerable<HttpPostedFileBase> files)
        {
            var GetSeq = db.P_SEQ.Where(a => a.SEQ_TEXT.Equals(10)).FirstOrDefault().SEQ_NUMBER;
            int OID = ((Convert.ToInt32(GetSeq) + 1));
            if (ModelState.IsValid)
            {
                if (p_OWNR.OWNR_ASET == null || p_OWNR.OWNR_ASET == "0")
                {
                    return RedirectToAction("NewOwner");
                }
                var Upload_Path = db.P_SEQ.Where(a => a.SEQ_TEXT == 500).FirstOrDefault().SEQ_NAME;//find from App Setup table
                foreach (var file in files) //find selected pictures
                {
                    if (file != null)
                    {
                        var File_Ext = System.IO.Path.GetFileNameWithoutExtension(file.ContentType);
                        if (File_Ext == "jpg" || File_Ext == "png" || File_Ext == "jpeg")
                        {
                            if (file.ContentLength > 0 && file.ContentLength < 2100000) //Max length 2100000 bytes = 2 MB and not 0
                            {
                                var newFileName = Guid.NewGuid().ToString() + "." + File_Ext;
                                var path = System.IO.Path.Combine(Server.MapPath(Upload_Path), newFileName);
                                file.SaveAs(path);
                                //Picture path Save to database
                                if (files.ElementAt(0) != null)
                                {
                                    p_OWNR.OWNR_IMAG = Upload_Path + "/" + newFileName;
                                }
                                if (files.ElementAt(1) != null)
                                {
                                    p_OWNR.OWNR_AST_IMAG = Upload_Path + "/" + newFileName;
                                }
                                if (files.ElementAt(2) != null)
                                {
                                    p_OWNR.OWNR_DED_IMAG = Upload_Path + "/" + newFileName;
                                }
                            }
                        }
                    }
                }
                p_OWNR.OWNR_TYPE = "Retailer";
                p_OWNR.OWNR_OID = OID.ToString();
                p_OWNR.OWNR_TEXT = OID.ToString(); //AutoSet
                p_OWNR.OWNR_RESIGN = "N";
                p_OWNR.OWNR_ACTV = true;
                p_OWNR.CDT = System.DateTime.Now;
                db.P_OWNR.Add(p_OWNR);
                //Update Last Number
                P_SEQ p_SEQ = db.P_SEQ.Find(10);
                p_SEQ.SEQ_NUMBER = OID;
                db.Entry(p_SEQ).State = EntityState.Modified;
                db.SaveChanges();
                //Asset Marked as Booked
                var param1 = new OracleParameter("ASET_ID", OracleDbType.Int32, p_OWNR.OWNR_ASET, ParameterDirection.Input);
                var param2 = new OracleParameter("BOOKING", OracleDbType.Int32, 1, ParameterDirection.Input);
                db.Database.ExecuteSqlCommand("BEGIN P_ISBOOKED_BOOKING(:ASET_ID, :BOOKING); end;", param1, param2);
                return RedirectToAction("Index");
            }
            ViewBag.OWNR_AREA = new SelectList(db.P_AREA, "AREA_TEXT", "AREA_NAME", p_OWNR.OWNR_AREA);
            ViewBag.OWNR_DEPT = new SelectList(db.P_DEPT, "DEPT_TEXT", "DEPT_NAME", p_OWNR.OWNR_DEPT);
            ViewBag.OWNR_ASET = new SelectList(db.P_ASSET.Where(a => a.ISBOOKED == false), "ASET_OID", "ASET_TEXT");
            return View(p_OWNR);
        }

        // GET: AssetOwner/Edit/5
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
            ViewBag.ASET_AREA = new SelectList(db.P_AREA.Where(b => b.AREA_ACTV == true && b.P_THNA.P_DIST.DIST_CUNT.Contains("BD")), "AREA_TEXT", "AREA_NAME");
            ViewBag.OWNR_DEPT = new SelectList(db.P_DEPT, "DEPT_TEXT", "DEPT_NAME", p_OWNR.OWNR_DEPT);
            ViewBag.OWNR_ASET = new SelectList(db.P_ASSET, "ASET_OID", "ASET_TEXT", p_OWNR.OWNR_ASET);
            return View(p_OWNR);
        }

        // POST: AssetOwner/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OWNR_OID,OWNR_TEXT,OWNR_USER,OWNR_NAME,OWNR_CONTACT,OWNR_ADDRESS,OWNR_IMAG,OWNR_TAKE,OWNR_RELASE,OWNR_AST_IMAG,OWNR_DED_IMAG,OWNR_TYPE,OWNR_RESIGN,OWNR_RES_DATE,OWNR_NOTE,OWNR_ASET,OWNR_DEPT,OWNR_AREA,OWNR_ACTV,CUSR,EUSR,CDT,EDT,UDT,VER")] P_OWNR p_OWNR)
        {
            if (ModelState.IsValid)
            {
                db.Entry(p_OWNR).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OWNR_AREA = new SelectList(db.P_AREA, "AREA_TEXT", "AREA_NAME", p_OWNR.OWNR_AREA);
            ViewBag.OWNR_DEPT = new SelectList(db.P_DEPT, "DEPT_TEXT", "DEPT_NAME", p_OWNR.OWNR_DEPT);
            ViewBag.OWNR_ASET = new SelectList(db.P_ASSET, "ASET_OID", "ASET_TEXT", p_OWNR.OWNR_ASET);
            return View(p_OWNR);
        }

        // GET: AssetOwner/Delete/5
        public ActionResult Deletex(string id)
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
            return View(p_OWNR);
        }

        // POST: AssetOwner/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            P_OWNR p_OWNR = db.P_OWNR.Find(id);
            db.P_OWNR.Remove(p_OWNR);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ViewImages(string id, string imgtype)
        {

            var dbDATA = db.P_OWNR.Where(a => a.OWNR_OID == id);
            var Upload_Path = "NO.png";
            if (imgtype == "DC")
            {
                Upload_Path = dbDATA.SingleOrDefault().OWNR_DED_IMAG;
                imgtype = "Deed Copy";
            }
            if (imgtype == "AP")
            {
                Upload_Path = dbDATA.SingleOrDefault().OWNR_AST_IMAG;
                imgtype = "Asset Picture";

            }
            if (imgtype == "OP")
            {
                Upload_Path = dbDATA.SingleOrDefault().OWNR_IMAG;
                imgtype = "Owner Picture";

            }
            string img = Upload_Path.Split('.').Last();
            var CombinePath = System.IO.Path.Combine(Server.MapPath(Upload_Path));
            try
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(CombinePath);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, imgtype + "." + img);
            }
            catch
            {
                return View();
            }


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
