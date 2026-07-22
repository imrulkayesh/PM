using PM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PM.Controllers
{
    public class UserPermissionController : Controller
    {
        pmdbEntities db = new pmdbEntities();
        // GET: UserPermission
        public ActionResult Index()
        {
            using (pmdbEntities db = new pmdbEntities())
            {
                var UserList = db.P_USERS.ToList();
                return View(UserList);
            }
        }
        // Get ? New user
        public ActionResult New()
        {
            ViewBag.USER_DEPT = new SelectList(db.P_DEPT.Where(d => d.DEPT_ACTV == true), "DEPT_TEXT", "DEPT_TEXT");
            ViewBag.USER_COMP = new SelectList(db.P_COMP.Where(c => c.COMP_ACTV == true), "COMP_TEXT", "COMP_TEXT");
            ViewBag.USER_LEVL = new SelectList(db.P_USERS_LEVEL.Where(l => l.LEVEL_ACTV == true), "LEVEL_ID", "LEVEL_TEXT");
            return View();
        }
        //Post ? new user
        [HttpPost]
        public ActionResult New([Bind(Include = "USER_TEXT,USER_NAME,USER_PASS,USER_CONT,USER_LEVL,USER_TYPE,USER_DEPT,USER_COMP,USER_ADMN,USER_ACTV")] MakeUserVM mu)
        {
            var EncryptCrendential = PM.ExtClass.EncryptDecrypt.Encrypt(mu.USER_PASS);
            using (pmdbEntities db = new pmdbEntities())
            {
                P_USERS PU = new P_USERS();
                PU.USER_TEXT = mu.USER_TEXT;
                PU.USER_NAME = mu.USER_NAME;
                PU.USER_PASS = EncryptCrendential;
                PU.USER_CONT = mu.USER_CONT;
                PU.USER_LEVL = mu.USER_LEVL;
                PU.USER_TYPE = mu.USER_TYPE;
                PU.USER_DEPT = mu.USER_DEPT;
                PU.USER_COMP = mu.USER_COMP;
                PU.USER_ADMN = mu.USER_ADMN;
                PU.USER_ACTV = true;
                db.P_USERS.Add(PU);
                db.SaveChanges();
                TempData["MSG"] = "OK! Save successfully.";
                return RedirectToAction("Index");
            }

        }
    }
}