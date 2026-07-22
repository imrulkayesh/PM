using PM.BLL;
using PM.Model;
using PM.ModelsV;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PM.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private GE ge = new GE();
        [Authorize]
        public ActionResult Index()
        {
            //ViewBag.clientMachineName = (Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName);
            //var xxx = Request.UserHostName;
            //ViewBag.s = Dns.GetHostEntry(Request.ServerVariables["REMOTE_HOST"]).HostName;
            //ViewBag.s2 = Dns.GetHostEntry(Request.ServerVariables["REMOTE_USER"]).HostName;
            //ViewBag.s3 = Request.UserHostAddress;
            //ViewBag.s4 = this.Request.ServerVariables["REMOTE_ADDR"];
            //ViewBag.s5 = Request.ServerVariables["REMOTE_HOST"];
            //ViewBag.s6 = System.Web.HttpContext.Current.Session.Timeout;
            return View();
        }

        public ActionResult TestPrint()
        {
            return View();
        }

        //get ? view login page
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            HttpRequest req = System.Web.HttpContext.Current.Request;
            if (req.Browser.Browser.ToUpper() == "INTERNETEXPLORER" || req.Browser.Browser.ToUpper() == "IE")
            {
                ViewBag.Message = "Sorry! Your browser is not compatible";
            }
            if (Convert.ToDouble(req.Browser.Version) <= 72.0)
            {
                ViewBag.Message = "Sorry! Your browser version is not compatible";
            }
            ViewBag.BN = req.Browser.Browser;
            ViewBag.BV = req.Browser.Version;
            return View();
        }

        //post ? access user to app
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLoginVM userloginvm, string rurl)
        {

            string WLCM = "swal('Welcome!', {  buttons: false,  timer: 1500,});";

            if (ModelState.IsValid)
            {
                bool UserFound = false;
                using (PMEntities _dbEntity = new PMEntities())
                {
                    UserFound = _dbEntity.A_USERS.Where(x => x.USER_TEXT.ToLower() == userloginvm.StaffId.Trim().ToLower()).ToList().Any(); //validating the user name in p_users table whether the user text is exist or not  
                    if (UserFound)
                    {
                        var EncryptCrendential = PM.ExtClass.EncryptDecrypt.Encrypt(userloginvm.Passwords);
                        var Valid_User = _dbEntity.A_USERS.Where(a => a.ACT == true && a.USER_TEXT.Equals(userloginvm.StaffId.ToLower()) && a.USER_PASS.Equals(EncryptCrendential)).FirstOrDefault();
                        if (Valid_User != null)
                        {
                            #region Create Session
                            Session["UserId"] = Valid_User.USER_TEXT;
                            Session["UserName"] = Valid_User.USER_NAME;
                            Session["UserRole"] = Valid_User.USER_ROLE;
                            Session["UserLocation"] = Valid_User.USER_LOCATION;
                            #endregion

                            #region User Menu List
                            List<UserMenuVM> _UserMenu = _dbEntity.A_USERS_MENUC.Where(x => x.USER_TEXT == userloginvm.StaffId.Trim().ToLower() && x.ACT == true).Where(x => x.A_MENUC.ACT == true).OrderBy(x => x.CMNU_TEXT).Select(x => new UserMenuVM
                            {
                                MENUP_TEXT = x.A_MENUC.A_MENUP.PMNU_TEXT,
                                MENUP_NAME = x.A_MENUC.A_MENUP.PMNU_NAME,
                                MENUP_ICON = x.A_MENUC.A_MENUP.PMNU_ICON,
                                MENUC_TEXT = x.CMNU_TEXT,
                                MENUC_NAME = x.A_MENUC.CMNU_NAME,
                                MENUC_ICON = x.A_MENUC.CMNU_ICON,
                                MENUC_CONTROLLER = x.A_MENUC.CMNU_CONT,
                                MENUC_ACTION = x.A_MENUC.CMNU_ACTN,
                                //MENUU_ID = x.A_MENUC.CMNU_TEXT,
                                MENUU_USER = x.USER_TEXT
                            }
                                                            ).ToList();
                            Session["UserMenuMaster"] = _UserMenu;
                            #endregion

                            #region Check_User_Pass_Same

                            if (userloginvm.StaffId == userloginvm.Passwords)
                            {
                                WLCM = "swal('Opps..','Its seems you are using too weak password','warning');";
                            }

                            #endregion
                            bool IsProperLogout = CreateLoginLog(userloginvm.StaffId.Trim().ToLower(), true);


                            if (IsProperLogout)
                            {
                                WLCM = "swal('Opps..','You are currently logged in on another device, Sign out when you’re done','info');";
                            }

                            //#region start check already login session
                            //var LoggedSession = _dbEntity.P_USERS_LOG.Where(l => l.USER_TEXT == Valid_User.USER_TEXT && l.USER_LOGGED == true).ToList();
                            //bool loggged = LoggedSession.Any();
                            //if (loggged)
                            //{
                            //    if (LoggedSession.FirstOrDefault().USER_SESSION_ID == System.Web.HttpContext.Current.Session.SessionID)
                            //    {
                            //        return RedirectToAction("Index", "Home");
                            //    }
                            //    else if (LoggedSession.FirstOrDefault().USER_LOG_PC == Dns.GetHostEntry(Request.ServerVariables["REMOTE_HOST"]).HostName)
                            //    {
                            //        // Session["SessionId"] = System.Web.HttpContext.Current.Session.SessionID;
                            //       // Session["SessionId"] = LoggedSession.FirstOrDefault().USER_SESSION_ID;
                            //        return RedirectToAction("Index", "Home");
                            //    }
                            //    else
                            //    {
                            //        return Content("<html><head></head><body><p>Already logged in from another PC.</p><a href ='/Home/Login'>try again</a></body></html>");
                            //    }
                            //}
                            //else
                            //{
                            //    P_USERS_LOG USERS_LOG = new P_USERS_LOG();
                            //    USERS_LOG.USER_LOG_ID = Guid.NewGuid().ToString();
                            //    USERS_LOG.USER_TEXT = Valid_User.USER_TEXT;
                            //    USERS_LOG.USER_LOG_IP = Request.UserHostAddress;
                            //    USERS_LOG.USER_LOG_PC = Dns.GetHostEntry(Request.ServerVariables["REMOTE_HOST"]).HostName;
                            //    USERS_LOG.USER_LOG_UTC = System.DateTime.Now;
                            //    USERS_LOG.USER_SESSION_ID = System.Web.HttpContext.Current.Session.SessionID;
                            //    USERS_LOG.USER_LOGGED = true;
                            //    _dbEntity.P_USERS_LOG.Add(USERS_LOG);
                            //    _dbEntity.SaveChanges();
                            //}
                            //#endregion
                            FormsAuthentication.SetAuthCookie(Valid_User.USER_TEXT, false);
                            TempData["msg"] = WLCM;
                            //if (string.IsNullOrEmpty(returnUrl))
                            //{
                            //    returnUrl= HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["returnUrl"];
                            //}
                            if (Url.IsLocalUrl(rurl) && rurl.Length > 1 && rurl.StartsWith("/")
                                                          && !rurl.StartsWith("//") && !rurl.StartsWith("/\\"))
                            {
                                return Redirect(rurl);
                            }
                            else
                            {
                                return RedirectToAction("Index");
                            }
                        }
                        else
                        {
                            ViewBag.Message = "Please enter the valid credentials!...";
                            return View(userloginvm);
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Please enter the valid credentials!...";
                        return View(userloginvm);
                    }
                }
            }
            return View(userloginvm);
        }
        //get ? clear session
        [Authorize]
        public ActionResult Logout()
        {
            if (Session["UserId"] != null)
            {
                string usr = Session["UserId"].ToString();
                CreateLoginLog(usr, false);
            }
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Login");
        }
        public bool CreateLoginLog(string UserId, bool Type)
        {
            bool rt = false;
            using (PMEntities _dbEntity = new PMEntities())
            {
                var SessionId = System.Web.HttpContext.Current.Session.SessionID;
                var MachineName = Environment.MachineName;
                var MachineUser = Environment.UserName;
                DateTime dt = DateTime.Now;
                var request = System.Web.HttpContext.Current.Request;
                HttpBrowserCapabilities bc = System.Web.HttpContext.Current.Request.Browser;


                A_USERS_LOGIN UL = _dbEntity.A_USERS_LOGIN.Find(UserId);
                if (UL == null) //First Login
                {
                    A_USERS_LOGIN NL = new A_USERS_LOGIN();
                    NL.USER_TEXT = UserId;
                    NL.USER_IP = Request.UserHostAddress;
                    NL.USER_PC = MachineName;
                    NL.PC_USER = MachineUser;
                    NL.SESSION_ID = SessionId;
                    NL.BROWSER_NAME = bc.Browser;
                    NL.BROWSER_VERSION = bc.Version;
                    NL.SESSION_ACT = Type;
                    NL.CDT = dt;
                    if (request.Browser.IsMobileDevice)
                    {
                        NL.PC_TYPE = "MOBILE";
                    }
                    else
                    {
                        NL.PC_TYPE = "COMPUTER";
                    }
                    _dbEntity.A_USERS_LOGIN.Add(NL);
                }
                else
                {
                    if (UL.SESSION_ACT == true)
                    {
                        rt = true;
                    }

                    UL.USER_IP = Request.UserHostAddress;
                    UL.USER_PC = MachineName;
                    UL.PC_USER = MachineUser;
                    UL.SESSION_ID = SessionId;
                    UL.BROWSER_NAME = bc.Browser;
                    UL.BROWSER_VERSION = bc.Version;
                    UL.SESSION_ACT = Type;
                    UL.UDT = dt;
                    if (request.Browser.IsMobileDevice)
                    {
                        UL.PC_TYPE = "MOBILE";
                    }
                    else
                    {
                        UL.PC_TYPE = "COMPUTER";
                    }
                    _dbEntity.Entry(UL).State = EntityState.Modified;
                }
                _dbEntity.SaveChanges();
                return rt;
            }
        }

        public ActionResult RecPas()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RecPas(RecPass CV)
        {
            if (ModelState.IsValid)
            {
                using (PMEntities db = new PMEntities())
                {
                    var UserId = CV.USER_CRD.ToLower();
                    A_USERS AU = db.A_USERS.Find(UserId);
                    if (AU != null)
                    {
                        //var xx = "0";
                        if (CV.UPDT_PASS)
                        {
                            var EncryptCrendential = PM.ExtClass.EncryptDecrypt.Encrypt(UserId);
                            AU.USER_PASS = EncryptCrendential;
                            AU.UDT = DateTime.Now;
                            AU.VAR = AU.VAR + 1;
                            db.Entry(AU).State = EntityState.Modified;
                            db.SaveChanges();
                            ViewBag.PassWord = "Credential Updated: " + UserId;
                        }
                        else
                        {
                            var EncryptCrendential = PM.ExtClass.EncryptDecrypt.Decrypt(AU.USER_PASS);
                            ViewBag.PassWord = "Credential: " + EncryptCrendential;
                        }
                    }
                    else
                    {
                        ViewBag.PassWord = "No User Data Found";
                    }
                }
            }
            return View(CV);
        }

        ////get ? change password view
        [Authorize]
        public ActionResult ChangePassword()
        {
            ViewBag.StaffName = Session["UserName"];
            return View();
        }
        ////post ? change password
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePassVM CV)
        {
            if (ModelState.IsValid)
            {
                var UserId = Session["UserId"];
                var EncryptCrendential = PM.ExtClass.EncryptDecrypt.Encrypt(CV.Passwords);
                if (CV.Password == CV.Passwords)
                {
                    using (PMEntities db = new PMEntities())
                    {
                        A_USERS U = db.A_USERS.Find(UserId);
                        if (U == null)
                        {
                            return HttpNotFound();
                        }
                        U.USER_PASS = EncryptCrendential;
                        U.UDT = DateTime.Now;
                        U.VAR = U.VAR + 1;
                        db.Entry(U).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["msg"] = "swal('Success..','Password changed successfully','success');";
                        return RedirectToAction("Index");
                    }
                }

            }
            ModelState.AddModelError("", errorMessage: "Invalid User/Pass");
            return View();
        }
        [AllowAnonymous]
        public ActionResult Forbidden_403()
        {
            TempData["msg"] = "swal('Opps..','Access denied','error');";
            return View();
        }
        public ActionResult MyProfile()
        {
            if (Session["UserId"] != null)
            {
                var UserId = Session["UserId"];
                using (PMEntities db = new PMEntities())
                {
                    A_USERS U = db.A_USERS.Find(UserId);
                    if (U == null)
                    {
                        return HttpNotFound();
                    }
                    UserProfile UP = new UserProfile();
                    UP.USER_ID = U.USER_TEXT;
                    UP.USER_NAME = U.USER_NAME;
                    UP.USER_EMAIL = U.USER_MAIL;
                    UP.USER_MOBILE = U.USER_MOBL;
                    UP.USER_DATE = U.CDT;
                    return View(UP);
                }
            }
            return Redirect("Login");
        }
        [HttpPost]
        public ActionResult MyProfile(UserProfile UP)
        {
            string Err = "Email is Required";
            if (ModelState.IsValid)
            {
                using (PMEntities db = new PMEntities())
                {
                    A_USERS U = db.A_USERS.Find(UP.USER_ID);
                    if (U == null)
                    {
                        return HttpNotFound();
                    }

                    #region Upload_Picture
                    if (UP.ProfileImage != null)
                    {
                        var Upload_Path = ge.GetImgPath(104);//get file path
                        if (UP.ProfileImage.ContentLength <= 0 || UP.ProfileImage.ContentLength > 1048576)
                        {
                            Err = string.Format("{0} File not uploaded, File is too larger, Max file size 1MB", UP.ProfileImage.FileName);
                            ModelState.AddModelError("", errorMessage: Err);
                            return View("MyProfile", UP);
                        }
                        //check that the file is of the permitted file type
                        string fileExtension = Path.GetExtension(UP.ProfileImage.FileName);
                        fileExtension = fileExtension.ToLower();
                        string[] acceptedFileTypes = new string[1];
                        bool acceptFile = false;
                        //should we accept the file?
                        if (fileExtension == ".jpg")
                        {
                            //accept the file, yay!
                            acceptFile = true;
                        }
                        if (!acceptFile)
                        {
                            Err = string.Format("{0} File not uploaded, File format not valid, accept only *.jpg", UP.ProfileImage.FileName);
                            ModelState.AddModelError("", errorMessage: Err);
                            return View("MyProfile", UP);
                        }

                        U.USER_IMAG = Upload_Path;
                        var path = System.IO.Path.Combine(Server.MapPath(Upload_Path), UP.USER_ID + " .jpg");
                        UP.ProfileImage.SaveAs(path); //upload file to server
                    }
                    #endregion
                    #region Change_Password
                    if (UP.Password != null)
                    {
                        var isValid = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,15}$");
                        if (!isValid.IsMatch(UP.Password))
                        {
                            Err = "Password should contain at least one lower case, one upper case letter, one number and length between 6 and 15 character";
                            ModelState.AddModelError("", errorMessage: Err);
                            return View("MyProfile", UP);
                        }

                        //if (UP.Password.Length <= 5)
                        //{
                        //    Err = "Password is too short, minimum 6 char";
                        //    ModelState.AddModelError("", errorMessage: Err);
                        //    return View("MyProfile", UP);
                        //}
                        //if(UP.Password != UP.Passwords)
                        //{
                        //    Err = "Confirm Password not matched";
                        //    ModelState.AddModelError("", errorMessage: Err);
                        //    return View("MyProfile", UP);
                        //}
                        var EncryptCrendential = PM.ExtClass.EncryptDecrypt.Encrypt(UP.Passwords);
                        U.USER_PASS = EncryptCrendential;
                    }

                    #endregion

                    U.USER_MAIL = UP.USER_EMAIL;
                    U.USER_MOBL = UP.USER_MOBILE;
                    U.UDU = UP.USER_ID;
                    U.UDT = DateTime.Now;
                    U.VAR = U.VAR + 1;
                    db.Entry(U).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["msg"] = "swal('Success..','Profile Updated successfully','success');";
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError("", errorMessage: Err);
            return View("MyProfile", UP);
        }
    }
}