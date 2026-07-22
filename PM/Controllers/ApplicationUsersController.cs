using PM.BLL;
using PM.ExtClass;
using PM.Model;
using PM.ModelsV;
using PM.MyAuth;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace PM.Controllers
{
    [Authorize]
    [CustomHttpReffer]
    public class ApplicationUsersController : Controller
    {
        private PMEntities db = new PMEntities();
        private InstantEmailSender IES = new InstantEmailSender();
        ReturnPermission RP = new ReturnPermission();
        [KAuth(1000)]
        public ActionResult Index()
        {
            var Users = db.A_USERS.ToList();
            return View(Users);
        }
        [KAuth(1000)]
        public ActionResult NewCreate()
        {
            return View();
        }
        [KAuth(1000)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewCreate(A_USERS AU)
        {
            string U = Session["UserId"].ToString();
            if (ModelState.IsValid)
            {
                //s A_USERS U = new A_USERS();
                AU.USER_TEXT = AU.USER_TEXT.ToLower();
                A_USERS isExist = db.A_USERS.Find(AU.USER_TEXT);
                if (isExist != null)
                {
                    TempData["msg"] = "swal('Ohh..','User already exist','info');";
                    return View();
                }
                string DefaultPass = AU.USER_PASS.ToLower();
                AU.USER_PASS = EncryptDecrypt.Encrypt(DefaultPass);
                AU.ACT = true;
                AU.CDT = DateTime.Now;
                AU.CDU = U;
                AU.VAR = 1;
                AU.USER_ROLE = 0;
                db.A_USERS.Add(AU);
                db.SaveChanges();
                if (!string.IsNullOrEmpty(AU.USER_MAIL))
                {
                    bool MailOK = IES.Send_Email_User_Open(AU.USER_NAME, AU.USER_TEXT, DefaultPass, AU.USER_MAIL);
                    if (MailOK == true)
                    {
                        TempData["msg"] = "swal('Success..','User saved succefully and Mail sent','success');";
                    }
                    else
                    {
                        TempData["msg"] = "swal('Opps..','User saved succefully and Mail sent failed','info');";
                    }
                }
                else
                {
                    TempData["msg"] = "swal('Ohh..','User saved succefully and Mail not sent','warning');";
                }
                return RedirectToAction("Index");
            }
            TempData["msg"] = "swal('Ohh..','User open failed','error');";
            return View();
        }
        [KAuth(1001)]
        [HttpGet]
        public ActionResult UserRole()
        {
            ViewBag.ROLE_ID = new SelectList(db.A_ROLES.Where(r => r.ACT == true && r.ROLE_TEXT < 10), "ROLE_TEXT", "ROLE_DESC");
            return View();
        }
        [KAuth(1001)]
        [HttpPost]
        public ActionResult UserRole(string USER_ID, int ROLE_ID)
        {
            string U = Session["UserId"].ToString();
            string NU = USER_ID.ToLower();
            string R = Session["UserRole"].ToString();
            List<UserRoleList> UR = new List<UserRoleList>();
            if (R == "10")
            {
                UR = (from r in db.A_USERS_ROLES
                      where r.USER_TEXT == NU //Admin User // 78278
                      select new UserRoleList
                      {
                          USER_NAME = r.A_USERS.USER_NAME,
                          ROLE_ID = r.ROLE_TEXT,
                          ROLE_NAME = r.A_ROLES.ROLE_DESC,
                          STATUS = r.ACT
                      }).ToList();
            }
            else
            {
                UR = (from r in db.A_USERS_ROLES
                      where r.USER_TEXT == NU
                      && r.A_USERS.USER_ROLE == 9 // 9 for KSA
                      select new UserRoleList
                      {
                          USER_NAME = r.A_USERS.USER_NAME,
                          ROLE_ID = r.ROLE_TEXT,
                          ROLE_NAME = r.A_ROLES.ROLE_DESC,
                          STATUS = r.ACT
                      }).ToList();
            }
            ViewBag.ROLE_ID = new SelectList(db.A_ROLES.Where(r => r.ACT == true && r.ROLE_TEXT < 10), "ROLE_TEXT", "ROLE_DESC", ROLE_ID);
            if (UR.Count > 0)
            {
                ViewBag.ROLE_TEXT = ROLE_ID;
                ViewBag.USER_TEXT = NU;
                ViewBag.USER_ID = NU;
            }
            return View(UR);
        }
        [KAuth(1001)]
        public ActionResult AR_Role(string USER_TEXT, int? ROLE_TEXT)
        {
            string UA = Session["UserId"].ToString();
            if (USER_TEXT != "" && ROLE_TEXT != null)
            {
                string U_ID = USER_TEXT.ToLower();
                A_USERS U = db.A_USERS.Find(U_ID);
                if (U != null)
                {
                    A_USERS_ROLES AR = db.A_USERS_ROLES.Where(u => u.USER_TEXT == U_ID && u.ROLE_TEXT == ROLE_TEXT).FirstOrDefault();
                    if (AR != null)
                    {
                        if (AR.ACT == true)
                        {
                            AR.ACT = false;
                        }
                        else
                        {
                            AR.ACT = true;
                        }
                        AR.UDT = DateTime.Now;
                        AR.UDU = UA;
                        db.Entry(AR).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        A_USERS_ROLES AUR = new A_USERS_ROLES();
                        AUR.USER_TEXT = U_ID;
                        AUR.ROLE_TEXT = ROLE_TEXT ?? 0;
                        AUR.ACT = true;
                        AUR.CDT = DateTime.Now;
                        AUR.CDU = UA;
                        db.A_USERS_ROLES.Add(AUR);
                        db.SaveChanges();
                    }
                    TempData["msg"] = "swal('Success..','Role saved successfully','success');";
                    return Redirect("UserRole");
                }
            }
            TempData["msg"] = "swal('Error..','Role not saved','error');";
            return Redirect("UserRole");
        }
        [KAuth(1002)]
        [HttpGet]
        public ActionResult UserGroup()
        {
            return View();
        }
        [KAuth(1002)]
        [HttpPost]
        public ActionResult UserGroup(string USER_ID)
        {
            string U = Session["UserId"].ToString();
            string NU = USER_ID.ToLower();
            string UR = Session["UserRole"].ToString();
            if (USER_ID != "")
            {
                List<UserGroupList> UG = new List<UserGroupList>();

                if (UR == "10")
                {
                    A_USERS AU = db.A_USERS.Where(u => u.USER_TEXT == NU && u.ACT == true).FirstOrDefault();
                    if (AU != null)
                    {
                        UG = (from r in db.A_GROUP
                              where r.ACT == true
                              from K in db.A_USERS_GROUP.Where(x => r.GROUP_ID == x.UG_GROUP
                              && x.UG_USER == NU).DefaultIfEmpty()
                              select new UserGroupList
                              {
                                  USER_NAME = K.UG_USER,
                                  GROUP_ID = r.GROUP_ID,
                                  GROUP_NAME = r.GROUP_NAME,
                                  STATUS = K.ACT
                              }).ToList();
                        ViewBag.USER_ID = NU;
                        ViewBag.USER_NAME = AU.USER_NAME;
                    }
                }
                else
                {
                    A_USERS AU = db.A_USERS.Where(u => u.USER_TEXT == NU && u.USER_ROLE == 9 && u.ACT == true).FirstOrDefault();
                    if (AU != null)
                    {
                        UG = (from r in db.A_GROUP
                              where r.ACT == true && r.GROUP_ID == "GR0001"
                              from K in db.A_USERS_GROUP.Where(x => r.GROUP_ID == x.UG_GROUP
             && x.UG_USER == NU).DefaultIfEmpty()
                              select new UserGroupList
                              {
                                  USER_NAME = K.UG_USER,
                                  GROUP_ID = r.GROUP_ID,
                                  GROUP_NAME = r.GROUP_NAME,
                                  STATUS = K.ACT
                              }).ToList();
                        ViewBag.USER_ID = NU;
                        ViewBag.USER_NAME = AU.USER_NAME;
                    }

                }
                return View(UG);
            }
            return RedirectToAction("UserGroup");
        }
        [KAuth(1002)]
        public ActionResult AR_Group(string USER_ID, string GROUP_TEXT)
        {
            string UA = Session["UserId"].ToString();
            if (USER_ID != "" && GROUP_TEXT != null)
            {
                string U_ID = USER_ID.ToLower();
                A_USERS U = db.A_USERS.Find(U_ID);
                if (U != null)
                {
                    A_USERS_GROUP AG = db.A_USERS_GROUP.Where(u => u.UG_USER == U_ID && u.UG_GROUP == GROUP_TEXT).FirstOrDefault();
                    if (AG != null)
                    {
                        if (AG.ACT == true)
                        {
                            AG.ACT = false;
                        }
                        else
                        {
                            AG.ACT = true;
                        }
                        AG.UDT = DateTime.Now;
                        AG.UDU = UA;
                        db.Entry(AG).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        A_USERS_GROUP AUG = new A_USERS_GROUP();
                        AUG.UG_USER = U_ID;
                        AUG.UG_GROUP = GROUP_TEXT;
                        AUG.ACT = true;
                        AUG.CDT = DateTime.Now;
                        AUG.CDU = UA;
                        db.A_USERS_GROUP.Add(AUG);
                        db.SaveChanges();
                    }
                    return Json(new { success = true, responseText = "Group saved successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false, responseText = "Group not saved" }, JsonRequestBehavior.AllowGet);
        }
        [KAuth(1003)]
        public ActionResult UserBusiness()
        {
            return View();
        }
        [KAuth(1003)]
        [HttpPost]
        public ActionResult UserBusiness(string USER_ID)
        {
            string U = Session["UserId"].ToString();
            string NU = USER_ID.ToLower();
            string UR = Session["UserRole"].ToString();
            List<UserBusinessList> UB = new List<UserBusinessList>();
            if (UR == "10")
            {
                A_USERS AU = db.A_USERS.Where(u => u.USER_TEXT == NU && u.ACT == true).FirstOrDefault();
                if (AU != null)
                {
                    //UB = (from r in db.A_BUSINESS
                    //      where r.ACT == true
                    //      from K in db.A_USERS_BUS.Where(x => r.BUSINESS_ID == x.UB_BUSINESS
                    //      && x.UB_USER == NU).DefaultIfEmpty()
                    //      select new UserBusinessList
                    //      {
                    //          USER_NAME = K.UB_USER,
                    //          BUSINESS_ID = r.BUSINESS_ID,
                    //          BUSINESS_NAME = r.BUSINESS_NAME,
                    //          STATUS = K.ACT
                    //      }).ToList();
                    string Sql1 = string.Format(@"select p.ub_user user_name, b.business_id,b.business_name,p.act status
                            from pm.a_business b
                            left outer join pm.a_users_bus p on b.business_id=p.ub_business
                            where b.act='1'
                            and p.ub_user='{0}'", NU);
                    UB = db.Database.SqlQuery<UserBusinessList>(sql: Sql1).ToList();

                    ViewBag.USER_ID = NU;
                    ViewBag.USER_NAME = AU.USER_NAME;
                }
            }
            else
            {
                A_USERS AU = db.A_USERS.Where(u => u.USER_TEXT == NU && u.USER_ROLE == 9 && u.ACT == true).FirstOrDefault();
                if (AU != null)
                {
                    //UB = (from r in db.A_BUSINESS
                    //      where r.ACT == true && r.A_COMPANY.A_COUNTRY.COUNTRY_ID == "KSA"
                    //      from K in db.A_USERS_BUS.Where(x => r.BUSINESS_ID == x.UB_BUSINESS
                    //      && x.UB_USER == NU).DefaultIfEmpty()
                    //      select new UserBusinessList
                    //      {
                    //          USER_NAME = K.UB_USER,
                    //          BUSINESS_ID = r.BUSINESS_ID,
                    //          BUSINESS_NAME = r.BUSINESS_NAME,
                    //          STATUS = K.ACT
                    //      }).ToList();
                    string Sql2 = string.Format(@"select p.ub_user user_name, b.business_id,b.business_name,p.act status
                        from pm.a_business b
                        join pm.a_users_bus op on b.business_id=op.ub_business and op.ub_user='{0}' and op.act='1'
                        left outer join pm.a_users_bus p on b.business_id=p.ub_business and p.ub_user='{1}'
                        where b.act='1'", U, NU);
                    UB = db.Database.SqlQuery<UserBusinessList>(sql: Sql2).ToList();
                    ViewBag.USER_ID = NU;
                    ViewBag.USER_NAME = AU.USER_NAME;

                }
            }
            return View(UB);
        }
        [KAuth(1003)]
        public ActionResult AR_Business(string USER_ID, string BUSINESS_TEXT)
        {
            string UA = Session["UserId"].ToString();
            if (USER_ID != "" && BUSINESS_TEXT != null)
            {
                string U_ID = USER_ID.ToLower();
                A_USERS U = db.A_USERS.Find(U_ID);
                if (U != null)
                {
                    A_USERS_BUS AB = db.A_USERS_BUS.Where(u => u.UB_USER == U_ID && u.UB_BUSINESS == BUSINESS_TEXT).FirstOrDefault();
                    if (AB != null)
                    {
                        if (AB.ACT == true)
                        {
                            AB.ACT = false;
                        }
                        else
                        {
                            AB.ACT = true;
                        }
                        AB.UDT = DateTime.Now;
                        AB.UDU = UA;
                        db.Entry(AB).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        A_USERS_BUS AUB = new A_USERS_BUS();
                        AUB.UB_USER = U_ID;
                        AUB.UB_BUSINESS = BUSINESS_TEXT;
                        AUB.ACT = true;
                        AUB.CDT = DateTime.Now;
                        AUB.CDU = UA;
                        db.A_USERS_BUS.Add(AUB);
                        db.SaveChanges();
                    }
                    return Json(new { success = true, responseText = "Business saved successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false, responseText = "Business not saved" }, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public ActionResult UserMenu()
        //{
        //    string U = Session["UserId"].ToString();
        //    if (U == "78278")
        //    {
        //        ViewBag.MENUC_ID = new SelectList(db.A_MENUC.Where(g => g.ACT == true), "CMNU_TEXT", "CMNU_NAME");
        //    }
        //    else
        //    {
        //        ViewBag.MENUC_ID = new SelectList(db.A_MENUC.Where(r => r.ACT == true && r.CMNU_NOTE == "KSA"), "CMNU_TEXT", "CMNU_NAME");
        //    }
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult UserMenu(string USER_ID, string MENUC_ID)
        //{
        //    string U = Session["UserId"].ToString();
        //    string NU = USER_ID.ToLower();
        //    List<UserMenuList> UM = new List<UserMenuList>();
        //    if (U == "78278")
        //    {
        //        UM = (from r in db.A_USERS_MENUC
        //              join a in db.A_ROLES on r.A_MENUC.CMNU_ROLE equals a.ROLE_TEXT
        //              where r.USER_TEXT == NU //Admin User // 78278
        //              orderby r.A_MENUC.A_MENUP.PMNU_TEXT
        //              select new UserMenuList
        //              {
        //                  USER_NAME = r.A_USERS.USER_NAME,
        //                  MENUP_NAME = r.A_MENUC.A_MENUP.PMNU_NAME,
        //                  MENUC_ID = r.CMNU_TEXT,
        //                  MENUC_NAME = r.A_MENUC.CMNU_NAME,
        //                  MENUC_ROLE = a.ROLE_DESC,
        //                  STATUS = r.ACT
        //              }).ToList();
        //        ViewBag.MENUC_ID = new SelectList(db.A_MENUC.Where(g => g.ACT == true), "CMNU_TEXT", "CMNU_NAME");
        //    }
        //    else
        //    {
        //        UM = (from r in db.A_USERS_MENUC
        //              join a in db.A_ROLES on r.A_MENUC.CMNU_ROLE equals a.ROLE_TEXT
        //              where r.USER_TEXT == NU
        //              && r.A_USERS.USER_ROLE == 9 // 9 for KSA
        //              select new UserMenuList
        //              {
        //                  USER_NAME = r.A_USERS.USER_NAME,
        //                  MENUP_NAME = r.A_MENUC.A_MENUP.PMNU_NAME,
        //                  MENUC_ID = r.CMNU_TEXT,
        //                  MENUC_NAME = r.A_MENUC.CMNU_NAME,
        //                  MENUC_ROLE = a.ROLE_DESC,
        //                  STATUS = r.ACT
        //              }).ToList();
        //        ViewBag.MENUC_ID = new SelectList(db.A_MENUC.Where(r => r.ACT == true && r.CMNU_NOTE == "KSA"), "CMNU_TEXT", "CMNU_NAME");
        //    }
        //    if (UM.Count > 0)
        //    {
        //        ViewBag.MENUC_TEXT = MENUC_ID;
        //        ViewBag.USER_TEXT = NU;
        //        ViewBag.USER_ID = NU;
        //    }
        //    return View(UM);
        //}
        //public ActionResult AR_Menu(string USER_TEXT, int MENUC_TEXT)
        //{
        //    string UA = Session["UserId"].ToString();
        //    if (USER_TEXT != "" && MENUC_TEXT != null)
        //    {
        //        string U_ID = USER_TEXT.ToLower();
        //        A_USERS U = db.A_USERS.Find(U_ID);
        //        if (U != null)
        //        {
        //            A_USERS_MENUC AM = db.A_USERS_MENUC.Where(u => u.USER_TEXT == U_ID && u.CMNU_TEXT == MENUC_TEXT).FirstOrDefault();
        //            if (AM != null)
        //            {
        //                if (AM.ACT == true)
        //                {
        //                    AM.ACT = false;
        //                }
        //                else
        //                {
        //                    AM.ACT = true;
        //                }
        //                AM.UDT = DateTime.Now;
        //                AM.UDU = UA;
        //                db.Entry(AM).State = EntityState.Modified;
        //                db.SaveChanges();
        //            }
        //            else
        //            {
        //                A_USERS_MENUC AUB = new A_USERS_MENUC();
        //                AUB.USER_TEXT = U_ID;
        //                AUB.CMNU_TEXT = MENUC_TEXT;
        //                AUB.ACT = true;
        //                AUB.CDT = DateTime.Now;
        //                AUB.CDU = UA;
        //                db.A_USERS_MENUC.Add(AUB);
        //                db.SaveChanges();
        //            }
        //            TempData["msg"] = "swal('Success..','Menu saved successfully','success');";
        //            return Redirect("UserMenu");
        //        }
        //    }
        //    TempData["msg"] = "swal('Error..','Menu not saved','error');";
        //    return Redirect("UserMenu");
        //}
        [KAuth(1005)]
        [HttpGet]
        public ActionResult UserArea()
        {
            return View();
        }
        [KAuth(1005)]
        [HttpPost]
        public ActionResult UserArea(string USER_ID)
        {
            string U = Session["UserId"].ToString();
            string UI = USER_ID.ToLower();
            A_USERS AU = db.A_USERS.Where(u => u.USER_TEXT == UI && u.USER_ROLE == 9 && u.ACT == true).FirstOrDefault();
            if (AU != null)
            {
                //         var ABC = (from C in db.A_AREA
                //                    where C.A_THANA.A_DISTRICT.A_COUNTRY.COUNTRY_ID == "KSA"
                //                   && C.ACT == true
                //                    from K in db.A_USERS_AREA.Where(x => C.AREA_ID == x.UA_AREA
                //                    && x.UA_USER == UI
                //                    ).DefaultIfEmpty()
                //                    select new UserAreaList
                //                    {
                //                        USER_NAME = K.UA_USER,
                //                        AREA_ID = C.AREA_ID,
                //                        AREA_NAME = C.AREA_NAME,
                //                        STATUS = K.ACT
                //                    }).ToList();
                string Sql = string.Format(@"select np.ua_user user_name, a.area_id, a.area_name, np.act status
                    from pm.a_area a join pm.a_thana t on a.area_thana = t.thana_id and t.act = '1'
                    join pm.a_district d on t.thana_district = d.district_id and d.act = '1'
                    join pm.a_country c on d.district_country = c.country_id and c.act = '1'
                    join pm.a_users_country ac on c.country_id = ac.country_id and ac.act = '1'
                    and ac.user_text = '{0}'
                    left outer join pm.a_users_area np on a.area_id = np.ua_area and np.ua_user = '{1}'", U, UI);
                var ABC = db.Database.SqlQuery<UserAreaList>(sql: Sql).ToList();

                ViewBag.USER_ID = UI;
                ViewBag.USER_NAME = AU.USER_NAME;
                return View("UserArea", ABC);
            }
            return View("UserArea");
        }
        [KAuth(1005)]
        public ActionResult AR_Area(string USER_ID, string AREA_TEXT)
        {
            string UA = Session["UserId"].ToString();
            if (USER_ID != "" && AREA_TEXT != null)
            {
                string U_ID = USER_ID.ToLower();
                A_USERS U = db.A_USERS.Find(U_ID);
                if (U != null)
                {
                    A_USERS_AREA AA = db.A_USERS_AREA.Where(u => u.UA_USER == U_ID && u.UA_AREA == AREA_TEXT).FirstOrDefault();
                    if (AA != null)
                    {
                        if (AA.ACT == true)
                        {
                            AA.ACT = false;
                        }
                        else
                        {
                            AA.ACT = true;
                        }
                        AA.UDT = DateTime.Now;
                        AA.UDU = UA;
                        db.Entry(AA).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        A_USERS_AREA UAR = new A_USERS_AREA();
                        UAR.UA_AREA = AREA_TEXT;
                        UAR.UA_USER = U_ID;
                        UAR.ACT = true;
                        UAR.CDT = DateTime.Now;
                        UAR.CDU = UA;
                        db.A_USERS_AREA.Add(UAR);
                        db.SaveChanges();
                    }
                    return Json(new { success = true, responseText = "Area saved successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false, responseText = "Area not saved" }, JsonRequestBehavior.AllowGet);
        }
        [KAuth(1004)]
        [HttpGet]
        public ActionResult SetUserMenu()
        {
            return View();
        }
        [KAuth(1004)]
        [HttpPost]
        public ActionResult SetUserMenu(string USER_ID)
        {
            string U = Session["UserId"].ToString();
            string UI = USER_ID.ToLower();
            string UR = Session["UserRole"].ToString();
            if (UR == "10")//U == "78278" || U == "307015"
            {
                A_USERS AU = db.A_USERS.Where(u => u.USER_TEXT == UI).FirstOrDefault();
                if (AU != null)
                {
                    var ABC = (from C in db.VW_PMENU_CMENU
                               from K in db.A_USERS_MENUC.Where(x => C.CMNU_TEXT == x.CMNU_TEXT
                               && x.USER_TEXT == UI
                               ).DefaultIfEmpty()
                               select new SetUserMenu
                               {
                                   PMNU_TEXT = C.PMNU_TEXT,
                                   PMNU_NAME = C.PMNU_NAME,
                                   CMNU_TEXT = C.CMNU_TEXT,
                                   CMNU_NAME = C.CMNU_NAME,
                                   USER_TEXT = K.USER_TEXT,
                                   MENU_STATUS = K.ACT
                               }).ToList();
                    ViewBag.USER_ID = UI;
                    ViewBag.USER_NAME = AU.USER_NAME;
                    return View("SetUserMenu", ABC);
                }

            }
            else
            {
                A_USERS AU = db.A_USERS.Where(u => u.USER_TEXT == UI && u.USER_ROLE == 9 && u.ACT == true).FirstOrDefault();
                if (AU != null)
                {
                    var ABC = (from C in db.VW_PMENU_CMENU
                               where C.CMNU_NOTE == "KSA"
                               from K in db.A_USERS_MENUC.Where(x => C.CMNU_TEXT == x.CMNU_TEXT
                               && x.USER_TEXT == UI
                               ).DefaultIfEmpty()
                               select new SetUserMenu
                               {
                                   PMNU_TEXT = C.PMNU_TEXT,
                                   PMNU_NAME = C.PMNU_NAME,
                                   CMNU_TEXT = C.CMNU_TEXT,
                                   CMNU_NAME = C.CMNU_NAME,
                                   USER_TEXT = K.USER_TEXT,
                                   MENU_STATUS = K.ACT
                               }).ToList();
                    ViewBag.USER_ID = UI;
                    ViewBag.USER_NAME = AU.USER_NAME;
                    return View("SetUserMenu", ABC);
                }
            }
            return View("SetUserMenu");
        }
        [KAuth(1004)]
        public ActionResult AR_UserMenu(string USER_ID, int MENU_ID)
        {
            string UA = Session["UserId"].ToString();
            if (!string.IsNullOrEmpty(USER_ID))
            {
                string U_ID = USER_ID.ToLower();
                A_USERS U = db.A_USERS.Find(U_ID);
                if (U != null)
                {
                    A_USERS_MENUC AM = db.A_USERS_MENUC.Where(u => u.USER_TEXT == U_ID && u.CMNU_TEXT == MENU_ID).FirstOrDefault();
                    if (AM != null)
                    {
                        if (AM.ACT == true)
                        {
                            AM.ACT = false;
                        }
                        else
                        {
                            AM.ACT = true;
                        }
                        AM.UDT = DateTime.Now;
                        AM.UDU = UA;
                        db.Entry(AM).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        A_USERS_MENUC AUB = new A_USERS_MENUC();
                        AUB.USER_TEXT = U_ID;
                        AUB.CMNU_TEXT = MENU_ID;
                        AUB.ACT = true;
                        AUB.CDT = DateTime.Now;
                        AUB.CDU = UA;
                        db.A_USERS_MENUC.Add(AUB);
                        db.SaveChanges();
                    }
                    return Json(new { success = true, responseText = "Menu saved successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false, responseText = "Menu not saved" }, JsonRequestBehavior.AllowGet);
        }
        [KAuth(1006)]
        public ActionResult UserEmail()
        {
            DropDownListFor_UserEmail();
            return View();
        }
        [KAuth(1006)]
        [HttpPost]
        public ActionResult UserEmail(UserEmailList obj)
        {
            string sql_1 = "";
            DropDownListFor_UserEmail();
            if (obj.ACT_B) { obj.ACT = 1; } else { obj.ACT = 0; }
            if (Request.Form["Submit"] != null)
            {
                if (ModelState.IsValid)
                {
                    if ((obj.MAIL_TYPE == "INSPECTION" || obj.MAIL_TYPE == "JOBCARD") && !string.IsNullOrWhiteSpace(obj.AREA_ID))
                    { //Without Business
                        sql_1 = string.Format(@"SELECT EM.EMAIL_ID,EM.EMAIL_DISPLAY,EM.KEEP_IN,EM.MAIL_TYPE,EM.MAIL_GROUP, EM.MAIL_BUSINESS,EM.AREA_ID,EM.ACT
                        FROM A_EMAIL EM WHERE EM.EMAIL_ID='{0}' AND EM.MAIL_GROUP='{1}'
                        AND EM.MAIL_TYPE='{2}' AND EM.AREA_ID='{3}'", obj.EMAIL_ID, obj.MAIL_GROUP, obj.MAIL_TYPE, obj.AREA_ID);
                        var _data = db.Database.SqlQuery<UserEmailList>(sql_1).ToList();
                        if (_data.Count == 1)
                        {
                            sql_1 = string.Format(@"UPDATE A_EMAIL EM SET ACT='{0}' 
                            WHERE EM.EMAIL_ID='{1}' AND EM.MAIL_GROUP='{2}' AND EM.MAIL_TYPE='{3}' AND EM.AREA_ID='{4}' AND EM.KEEP_IN='{5}'",
                            obj.ACT, obj.EMAIL_ID, obj.MAIL_GROUP, obj.MAIL_TYPE, obj.AREA_ID, obj.KEEP_IN);
                        }
                        else
                        {
                            sql_1 = string.Format(@"insert into a_email(EMAIL_ID,EMAIL_DISPLAY,KEEP_IN,MAIL_GROUP,MAIL_BUSINESS,ACT,MAIL_TYPE,AREA_ID)
                                                   VALUES('{0}','{1}','{2}','{3}','',1,'{4}','{5}')",
                                                   obj.EMAIL_ID, obj.EMAIL_DISPLAY, obj.KEEP_IN, obj.MAIL_GROUP, obj.MAIL_TYPE, obj.AREA_ID);
                        }
                        db.Database.ExecuteSqlCommand(sql_1);
                    }
                    else if ((obj.MAIL_TYPE == "INSURANCE" || obj.MAIL_TYPE == "IQAMA" || obj.MAIL_TYPE == "REMINDER" || obj.MAIL_TYPE == "TRNS_RETN") && !string.IsNullOrWhiteSpace(obj.MAIL_BUSINESS))
                    { //Without Area
                        sql_1 = string.Format(@"SELECT EM.EMAIL_ID,EM.EMAIL_DISPLAY,EM.KEEP_IN,EM.MAIL_TYPE,EM.MAIL_GROUP, EM.MAIL_BUSINESS,EM.AREA_ID,EM.ACT
                        FROM A_EMAIL EM WHERE EM.EMAIL_ID='{0}' AND EM.MAIL_GROUP='{1}'
                        AND EM.MAIL_TYPE='{2}' AND EM.MAIL_BUSINESS='{3}'", obj.EMAIL_ID, obj.MAIL_GROUP, obj.MAIL_TYPE, obj.MAIL_BUSINESS);
                        var _data = db.Database.SqlQuery<UserEmailList>(sql_1).ToList();
                        if (_data.Count == 1)
                        {
                            sql_1 = string.Format(@"UPDATE A_EMAIL EM SET ACT='{0}' 
                            WHERE EM.EMAIL_ID='{1}' AND EM.MAIL_GROUP='{2}' AND EM.MAIL_TYPE='{3}' AND EM.MAIL_BUSINESS='{4}' AND EM.KEEP_IN='{5}'",
                            obj.ACT, obj.EMAIL_ID, obj.MAIL_GROUP, obj.MAIL_TYPE, obj.MAIL_BUSINESS, obj.KEEP_IN);
                        }
                        else
                        {
                            sql_1 = string.Format(@"insert into a_email(EMAIL_ID,EMAIL_DISPLAY,KEEP_IN,MAIL_GROUP,MAIL_BUSINESS,ACT,MAIL_TYPE,AREA_ID)
                                                   VALUES('{0}','{1}','{2}','{3}','{4}',1,'{5}','')",
                                                   obj.EMAIL_ID, obj.EMAIL_DISPLAY, obj.KEEP_IN, obj.MAIL_GROUP, obj.MAIL_BUSINESS, obj.MAIL_TYPE);
                        }
                        db.Database.ExecuteSqlCommand(sql_1);
                    }
                    else if (obj.MAIL_TYPE == "PLEA")
                    {// Without BU, AREA, Group only

                    }
                }
                else
                {
                    var errors = ModelState.Select(x => x.Value.Errors)
                                           .Where(y => y.Count > 0)
                                           .ToList();
                }
            }
            else if (Request.Form["Find"] != null)
            {
                sql_1 = string.Format(@"SELECT EM.EMAIL_ID,EM.EMAIL_DISPLAY,EM.KEEP_IN,EM.MAIL_TYPE,EM.MAIL_GROUP, EM.MAIL_BUSINESS,EM.AREA_ID,EM.ACT
                FROM A_EMAIL EM WHERE EM.EMAIL_ID='{0}'", obj.EMAIL_ID);
                var _data = db.Database.SqlQuery<UserEmailList>(sql_1).ToList();
                obj.User_Email_List = _data;
            }
            ModelState.Clear();
            return View(obj);
        }
        private void DropDownListFor_UserEmail()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            List<string> _ki = new List<string>()
            {
                "TO",
                "CC"
            };
            List<string> _mt = new List<string>()
            {
                //"ADMIN",
                "INSPECTION",
                "INSURANCE",
                "IQAMA",
                "JOBCARD",
                //"PLEA",
                "REMINDER",
                "TRNS_RETN"
            };
            ViewBag.KEEP_IN = new SelectList(_ki);
            ViewBag.MAIL_TYPE = new SelectList(_mt);
            ViewBag.MAIL_BUSINESS = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
            ViewBag.MAIL_GROUP = new SelectList(RP.MyGroup(UserId: U, UserType: T), "GROUP_ID", "GROUP_NAME");
            ViewBag.AREA_ID = new SelectList(RP.MyAreaList(UserId: U, UserType: T), "AREA_ID", "AREA_NAME");
        }
    }
}