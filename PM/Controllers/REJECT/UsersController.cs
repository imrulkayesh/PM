using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PM.Models;
using PM.ModelsV;

namespace PM.Controllers
{
    public class UsersController : Controller
    {
        private pmdbEntities db = new pmdbEntities();

        // GET: Users
        public ActionResult Index()
        {
            var p_USERS = db.P_USERS.Include(p => p.P_ROLES);
            return View(p_USERS.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            P_USERS p_USERS = db.P_USERS.Find(id);
            if (p_USERS == null)
            {
                return HttpNotFound();
            }
            return View(p_USERS);
        }

        // GET: Users/Create
        public ActionResult UserOpenForm()
        {
            ViewBag.USER_ROLE = new SelectList(db.P_ROLES, "ROLE_TEXT", "ROLE_NAME");
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserOpenForm([Bind(Include = "USER_TEXT,USER_NAME,USER_PASS,USER_MAIL,USER_MOBL,USER_IMAG,USER_ROLE")] P_USERS p_USERS)
        {
            if (ModelState.IsValid)
            {
                p_USERS.USER_TEXT = p_USERS.USER_TEXT.ToLower();
                p_USERS.USER_PASS = PM.ExtClass.EncryptDecrypt.Encrypt(p_USERS.USER_PASS);
                p_USERS.USER_ACTV = true;
                p_USERS.CDT = System.DateTime.Now;
                //p_USERS.CUSR=Session User
                db.P_USERS.Add(p_USERS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Something went wrong");
            ViewBag.USER_ROLE = new SelectList(db.P_ROLES, "ROLE_TEXT", "ROLE_NAME", p_USERS.USER_ROLE);
            return View(p_USERS);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            P_USERS p_USERS = db.P_USERS.Find(id);
            if (p_USERS == null)
            {
                return HttpNotFound();
            }
            ViewBag.USER_ROLE = new SelectList(db.P_ROLES, "ROLE_TEXT", "ROLE_NAME", p_USERS.USER_ROLE);
            return View(p_USERS);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "USER_TEXT,USER_NAME,USER_PASS,USER_MAIL,USER_MOBL,USER_IMAG,USER_ROLE,USER_ACTV,CUSR,EUSR,CDT,EDT,UDT,VER")] P_USERS p_USERS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(p_USERS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.USER_ROLE = new SelectList(db.P_ROLES, "ROLE_TEXT", "ROLE_NAME", p_USERS.USER_ROLE);
            return View(p_USERS);
        }

        // GET: Users/Block
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            P_USERS p_USERS = db.P_USERS.Find(id);
            if (p_USERS == null)
            {
                return HttpNotFound();
            }
            p_USERS.USER_ACTV = false;
            db.Entry(p_USERS).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #region UserMenuOldVersion
        //List User Menu
        public ActionResult MenuPermission(string id)
        {
            ViewBag.ParentMenuList = new SelectList(db.P_MENUP.Where(p => p.PMNU_ACTV == true), "PMNU_TEXT", "PMNU_NAME");
            var p_USERS_MENUC = db.P_USERS_MENUC.Include(p => p.P_MENUC).Include(p => p.P_USERS).Where(p => p.USER_TEXT == id).ToList();
            return View(p_USERS_MENUC);
        }
        //Populate Child Menu to get user permission
        public ActionResult getCMList(int pMNUid)
        {
            var returnCMList = from z in db.P_MENUC.Where(x => x.CMNU_PMNU == pMNUid)
                               select new { z.CMNU_TEXT, z.CMNU_NAME };
            return Json(returnCMList, JsonRequestBehavior.AllowGet);
        }
        // public ActionResult AddNewMenu()
        //{
        //    ViewBag.CMNU_TEXT = new SelectList(db.P_MENUC, "CMNU_TEXT", "CMNU_NAME");
        //    ViewBag.USER_TEXT = new SelectList(db.P_USERS, "USER_TEXT", "USER_NAME");
        //    return View();
        //}
        //Add Menu to User //Save


        #endregion
        #region MenuPermission
        //17 Nov 2019 
        //user menus
        public ActionResult UserMenuList(string id)
        {
            var MenuList = from c in db.P_MENUC
                           from p in db.P_MENUP.Where(p => p.PMNU_TEXT == c.CMNU_PMNU)
                           from u in db.P_USERS_MENUC.Where(u => u.CMNU_TEXT == c.CMNU_TEXT).Where(d => d.USER_TEXT == id).DefaultIfEmpty()
                           select new ViewUserPermissionMenu
                           {
                               PMNU_TEXT = p.PMNU_TEXT
                               ,
                               PMNU_NAME = p.PMNU_NAME
                               ,
                               CMNU_TEXT = c.CMNU_TEXT
                               ,
                               CMNU_NAME = c.CMNU_NAME
                               ,
                               USER_TEXT = u.USER_TEXT
                               ,
                               USER_MENUC = u.USER_MENUC
                               ,
                               UMP_ACTV=u.UMP_ACTV
                               ,
                               CMNU_NOTE=c.CMNU_NOTE
                           };
            return View(MenuList.ToList());
        }
        public ActionResult AddNewMenu(int menuId, string staffId , string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                P_USERS_MENUC p_USERS_MENUC = new P_USERS_MENUC();
                p_USERS_MENUC.USER_MENUC = menuId + staffId;
                p_USERS_MENUC.CMNU_TEXT = menuId;
                p_USERS_MENUC.USER_TEXT = staffId;
                p_USERS_MENUC.UMP_ACTV = true;
                db.P_USERS_MENUC.Add(p_USERS_MENUC);
                db.SaveChanges();
                return Redirect(ReturnUrl);
            }
            ModelState.AddModelError("","Getting Error");
            return Redirect(ReturnUrl);
        }
        public ActionResult AddRemoveMenu(string menuId,bool YN,string ReturnUrl)
        {           
            P_USERS_MENUC p = db.P_USERS_MENUC.Find(menuId);
            if(p==null)
            {
                return HttpNotFound();
            }
            if (YN)
            {
                p.UMP_ACTV = true;
            }
            else
            {
                p.UMP_ACTV = false;
            }
            db.Entry(p).State = EntityState.Modified;
            db.SaveChanges();
            return Redirect(ReturnUrl);
        }
        #endregion
        #region BusinessPermission
        public ActionResult UserBusinessList(string id)
        {
            var BEUNList = from b in db.P_BEUN.Where(b => b.BEUN_ACTV == true)
                           from c in db.P_COMP.Where(c => c.COMP_ACTV == true && c.COMP_TEXT == b.BEUN_COMP)
                           from u in db.P_USERS_BEUN.Where(u => u.BEUN_TEXT == b.BEUN_TEXT).Where(d => d.USER_TEXT == id).DefaultIfEmpty()
                           select new BEUNVM
                           {
                               COMP_NAME = c.COMP_NAME,
                               BEUN_USER = u.BEUN_USER,
                               BEUN_TEXT = b.BEUN_TEXT,
                               BEUN_NAME = b.BEUN_NAME,
                               USER_TEXT = u.USER_TEXT,
                               UBP_ACTV = u.UBP_ACTV
                           };


            return View(BEUNList.ToList());
        }
        public ActionResult AddNewBEUN(string beunId, string staffId, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                P_USERS_BEUN p_USERS_BEUN = new P_USERS_BEUN();
                p_USERS_BEUN.BEUN_USER = beunId + staffId;
                p_USERS_BEUN.BEUN_TEXT = beunId;
                p_USERS_BEUN.USER_TEXT = staffId;
                p_USERS_BEUN.UBP_ACTV = true;
                db.P_USERS_BEUN.Add(p_USERS_BEUN);
                db.SaveChanges();
                return Redirect(ReturnUrl);
            }
            ModelState.AddModelError("", "Getting Error");
            return Redirect(ReturnUrl);
        }
        public ActionResult AddRemoveBEUN(string beunId, bool YN, string ReturnUrl)
        {
            P_USERS_BEUN p = db.P_USERS_BEUN.Find(beunId);
            if (p == null)
            {
                return HttpNotFound();
            }
            if (YN)
            {
                p.UBP_ACTV = true;
            }
            else
            {
                p.UBP_ACTV = false;
            }
            db.Entry(p).State = EntityState.Modified;
            db.SaveChanges();
            return Redirect(ReturnUrl);
        }
        #endregion
        #region UserGroupPermission
        public ActionResult UserGroupList(string id)
        {
            var ASGRList = from g in db.P_ASGR.Where(g => g.ASGR_ACTV == true)
                           from u in db.P_USERS_ASGR.Where(u => u.ASGR_TEXT == g.ASGR_TEXT).Where(d => d.USER_TEXT == id).DefaultIfEmpty()
                           select new ASGRVM
                           {
                               ASGR_USER = u.ASGR_USER,
                               ASGR_TEXT = g.ASGR_TEXT,
                               ASGR_NAME = g.ASGR_NAME,
                               USER_TEXT = u.USER_TEXT,
                               UAP_ACTV = u.UAP_ACTV
                           };

            return View(ASGRList.ToList());
        }
        public ActionResult AddNewGroup(string asgrId, string staffId, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                P_USERS_ASGR p_USERS_ASGR = new P_USERS_ASGR();
                p_USERS_ASGR.ASGR_USER = asgrId + staffId;
                p_USERS_ASGR.ASGR_TEXT = asgrId;
                p_USERS_ASGR.USER_TEXT = staffId;
                p_USERS_ASGR.UAP_ACTV = true;
                db.P_USERS_ASGR.Add(p_USERS_ASGR);
                db.SaveChanges();
                return Redirect(ReturnUrl);
            }
            ModelState.AddModelError("", "Getting Error");
            return Redirect(ReturnUrl);
        }
        public ActionResult AddRemoveGroup(string asgrId, bool YN, string ReturnUrl)
        {
            P_USERS_ASGR p = db.P_USERS_ASGR.Find(asgrId);
            if (p == null)
            {
                return HttpNotFound();
            }
            if (YN)
            {
                p.UAP_ACTV = true;
            }
            else
            {
                p.UAP_ACTV = false;
            }
            db.Entry(p).State = EntityState.Modified;
            db.SaveChanges();
            return Redirect(ReturnUrl);
        }
        #endregion

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
