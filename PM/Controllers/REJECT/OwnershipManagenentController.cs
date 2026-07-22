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
    public class OwnershipManagenentController : Controller
    {
        private pmdbEntities db = new pmdbEntities();

        // GET: OwnershipManagenent
        public ActionResult Index()
        {
            var p_ASSET_DAMAGE = db.P_ASSET_DAMAGE.Include(p => p.P_ASSET).Include(p => p.P_OWNR);
            return View(p_ASSET_DAMAGE.ToList());
        }

        // GET: OwnershipManagenent/Create
        public ActionResult Application(string id)
        {
            if (Session["UserId"] != null)
            {

                List<SelectListItem> MoreAction = new List<SelectListItem>();
                MoreAction.Add(new SelectListItem() { Text = "Ownership", Value = "O" });
                MoreAction.Add(new SelectListItem() { Text = "Sales", Value = "S" });
                MoreAction.Add(new SelectListItem() { Text = "Damage", Value = "D" });
                MoreAction.Add(new SelectListItem() { Text = "Lost", Value = "L" });

                ViewBag.DMG_TYPE = new SelectList(MoreAction, "Value", "Text");
                ViewBag.ASET_OID = new SelectList(db.P_ASSET.Where(a => a.ASET_OID == id), "ASET_OID", "ASET_TEXT");
                ViewBag.DMG_USR = new SelectList(db.P_OWNR.Where(o => o.OWNR_ASET == id && o.OWNR_ACTV == true), "OWNR_OID", "OWNR_USER");
                return View();
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Time Expired, please login!');window.location.href = '/Home/Login';</script>");
            }
        }

        // POST: OwnershipManagenent/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Application([Bind(Include = "ASET_OID,DMG_APP,DMG_APP_USER,DMG_APP_DATE,DMG_AUTH,DMG_AUTH_USER,DMG_AUTH_DATE,DMG_DATE,DMG_SRV_DURA,DMG_PRICE,DMG_PRIC_DIFF,DMG_USR,DMG_ACTV,DMG_TYPE")] P_ASSET_DAMAGE p_ASSET_DAMAGE)
        {
            if (Session["UserId"] != null)
            {
                var getId = Session["UserId"].ToString();

                if (ModelState.IsValid)
                {
                    p_ASSET_DAMAGE.DMG_APP_USER = getId;
                    p_ASSET_DAMAGE.DMG_APP_DATE = System.DateTime.Now.Date;
                    p_ASSET_DAMAGE.DMG_ACTV = false;
                    if (p_ASSET_DAMAGE.DMG_USR == null) // if no user found set default user
                    {
                        p_ASSET_DAMAGE.DMG_USR = "0";
                    }
                    if (p_ASSET_DAMAGE.DMG_PRICE == null)
                    {
                        p_ASSET_DAMAGE.DMG_PRICE = 0;
                    }
                    if (p_ASSET_DAMAGE.DMG_DATE != null)
                    {
                        db.P_ASSET_DAMAGE.Add(p_ASSET_DAMAGE);
                        db.SaveChanges();
                    }
                    //Calculate Days and Prices
                    var param1 = new OracleParameter("ASET_ID", OracleDbType.Int32, p_ASSET_DAMAGE.ASET_OID, ParameterDirection.Input);
                    db.Database.ExecuteSqlCommand("BEGIN PRO_DAMAGE_LOST_OWNERSHIP(:ASSETOID); end;", param1);
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Time Expired, please login!');window.location.href = '/Home/Login';</script>");
            }
            //with Messages
            return View();
        }

        public ActionResult Approval(string id)
        {
            P_ASSET_DAMAGE p_ASSET_DAMAGE = db.P_ASSET_DAMAGE.Find(id);
            return View(p_ASSET_DAMAGE);
        }
        [HttpPost]
        public ActionResult Approval(string ASET_OID, string DMG_AUTH, int DMG_PRICE)
        {
            if (Session["UserId"] != null)
            {
                var getId = Session["UserId"].ToString();
                if (ASET_OID == null || ASET_OID == "")
                {
                    return RedirectToAction("Index");
                }
                if (DMG_AUTH == null || DMG_AUTH == "")
                {
                    DMG_AUTH = "Approved";
                }
                if (DMG_PRICE <=0)
                {
                    DMG_PRICE =0;
                }
                P_ASSET_DAMAGE p_ASSET_DAMAGE = db.P_ASSET_DAMAGE.Find(ASET_OID);

                p_ASSET_DAMAGE.DMG_AUTH = DMG_AUTH;
                p_ASSET_DAMAGE.DMG_AUTH_DATE = System.DateTime.Now.Date;
                p_ASSET_DAMAGE.DMG_AUTH_USER = getId;
                p_ASSET_DAMAGE.DMG_ACTV = true;
                db.Entry(p_ASSET_DAMAGE).State = EntityState.Modified;
                db.SaveChanges();
                //
                //Calculate Days and Prices
                var param1 = new OracleParameter("ASET_ID", OracleDbType.Int32, p_ASSET_DAMAGE.ASET_OID, ParameterDirection.Input);
                db.Database.ExecuteSqlCommand("BEGIN PRO_DAMAGE_LOST_OWNERSHIP(:ASSETOID); end;", param1);
                //Set Damage in P_Asset
                db.Database.ExecuteSqlCommand("BEGIN PRO_DAMAGE_LOST_OWNERSHIP_APP(:ASSETOID); end;", param1);
                return RedirectToAction("Index");
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Time Expired, please login!');window.location.href = '/Home/Login';</script>");
            }
        }

        //// GET: OwnershipManagenent/Edit/5
        //public ActionResult Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    P_ASSET_DAMAGE p_ASSET_DAMAGE = db.P_ASSET_DAMAGE.Find(id);
        //    if (p_ASSET_DAMAGE == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.ASET_OID = new SelectList(db.P_ASSET, "ASET_OID", "ASET_TEXT", p_ASSET_DAMAGE.ASET_OID);
        //    ViewBag.DMG_USR = new SelectList(db.P_OWNR, "OWNR_OID", "OWNR_TEXT", p_ASSET_DAMAGE.DMG_USR);
        //    return View(p_ASSET_DAMAGE);
        //}

        //// POST: OwnershipManagenent/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ASET_OID,DMG_APP,DMG_APP_USER,DMG_APP_DATE,DMG_AUTH,DMG_AUTH_USER,DMG_AUTH_DATE,DMG_DATE,DMG_SRV_DURA,DMG_PRICE,DMG_PRIC_DIFF,DMG_USR,DMG_ACTV,DMG_TYPE")] P_ASSET_DAMAGE p_ASSET_DAMAGE)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(p_ASSET_DAMAGE).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.ASET_OID = new SelectList(db.P_ASSET, "ASET_OID", "ASET_TEXT", p_ASSET_DAMAGE.ASET_OID);
        //    ViewBag.DMG_USR = new SelectList(db.P_OWNR, "OWNR_OID", "OWNR_TEXT", p_ASSET_DAMAGE.DMG_USR);
        //    return View(p_ASSET_DAMAGE);
        //}

        // GET: OwnershipManagenent/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            P_ASSET_DAMAGE p_ASSET_DAMAGE = db.P_ASSET_DAMAGE.Find(id);
            if (p_ASSET_DAMAGE == null)
            {
                return HttpNotFound();
            }
            return View(p_ASSET_DAMAGE);
        }

        // POST: OwnershipManagenent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            P_ASSET_DAMAGE p_ASSET_DAMAGE = db.P_ASSET_DAMAGE.Find(id);
            db.P_ASSET_DAMAGE.Remove(p_ASSET_DAMAGE);
            db.SaveChanges();
            return RedirectToAction("Index");
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
