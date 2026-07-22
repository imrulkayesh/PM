using PM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PM.Controllers
{
    public class MenuController : Controller
    {
        pmdbEntities db = new pmdbEntities();

        // GET: Menu
        public ActionResult Index()
        {
            return View();
        }
        //Show All Parent Menu
        public ActionResult ParentMenu()
        {
            using (pmdbEntities db = new pmdbEntities())
            {
                var pMenusList = db.MENUPs.OrderBy(o=>o.MENUP_ID).ToList();
                return View(pMenusList);
            }
        }
        //get ? new parent menu
        public ActionResult NewParentMenu()
        {
            return View();
        }
        //post ? new parent menu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveParentMenu([Bind(Include = "MENUP_ID, MENUP_TEXT, MENUP_NAME,MENUP_ICON,MENUP_ACTV")]ParentMenuVM pm)
        {
            using (pmdbEntities db = new pmdbEntities())
            {
                MENUP MP = new MENUP();
                MP.MENUP_ID = pm.MENUP_ID;
                MP.MENUP_TEXT = pm.MENUP_TEXT;
                MP.MENUP_NAME = pm.MENUP_NAME;
                MP.MENUP_ICON = pm.MENUP_ICON;
                MP.MENUP_ACTV = true;
                db.MENUPs.Add(MP);
                db.SaveChanges();
                TempData["MSG"] = "OK! Save successfully.";
                return RedirectToAction("ParentMenu");
            }
        }
        //Tolist ? all child menu
        public ActionResult ChildMenu()
        {
            using (pmdbEntities db = new pmdbEntities())
            {
                var childmenu = db.MENUCs.OrderBy(c=>c.MENUC_ID).ToList();
                return View(childmenu);
            }
        }
        //get ? new child menu
        public ActionResult NewChildMenu()
        {

            ViewBag.MENUC_MENUP_ID = new SelectList(db.MENUPs.Where(p => p.MENUP_ACTV == true), "MENUP_ID", "MENUP_NAME");
            return View();

        }
        //post ? new Child menu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewChildMenu([Bind(Include = "MENUC_ID,MENUC_MENUP_ID,MENUC_TEXT,MENUC_NAME,MENUC_ICON,MENUC_CONTROLLER,MENUC_ACTION,MENUC_ACTV")]ChildMenuVM cm)
        {
            using (pmdbEntities db = new pmdbEntities())
            {
                MENUC MC = new MENUC();
                MC.MENUC_ID = cm.MENUC_ID;
                MC.MENUC_MENUP_ID = cm.MENUC_MENUP_ID;
                MC.MENUC_TEXT = cm.MENUC_TEXT;
                MC.MENUC_NAME = cm.MENUC_NAME;
                MC.MENUC_ICON = cm.MENUC_ICON;
                MC.MENUC_CONTROLLER = cm.MENUC_CONTROLLER;
                MC.MENUC_ACTION = cm.MENUC_ACTION;
                MC.MENUC_ACTV = true;
                db.MENUCs.Add(MC);
                db.SaveChanges();
                TempData["MSG"] = "OK! Save successfully.";
                return RedirectToAction("ChildMenu");
            }
        }
    }
}