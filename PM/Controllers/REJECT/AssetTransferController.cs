using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PM.Models;
using System.IO;
using Oracle.ManagedDataAccess.Client;

namespace PM.Controllers
{
    public class AssetTransferController : Controller
    {
        private pmdbEntities db = new pmdbEntities();

        // GET: AssetTransfer
        public ActionResult Index()
        {
            var p_ASSET_TRNS = db.P_ASSET_TRNS.Include(p => p.P_AREA)
                .Include(p => p.P_AREA1)
                .Include(p => p.P_ASSET)
                .Include(p => p.P_DEPT)
                .Include(p => p.P_OWNR)
                .Include(p => p.P_DEPT1)
                .Include(p => p.P_OWNR1)
                .Where(p => p.TRNS_POST == false)
                .Take(20);
            return View(p_ASSET_TRNS.ToList());
        }

        public ActionResult AddDocuments(string id)
        {
            ViewBag.TRNS_TEXT = id;
            return PartialView("_AddDocuments");
        }
        [HttpPost]
        public ActionResult AddDocuments()
        {
            var Upload_Path = db.P_SEQ.Where(a => a.SEQ_TEXT == 702).FirstOrDefault().SEQ_NAME;//find from App Setup table
            HttpFileCollectionBase files = Request.Files;
            for (int i = 0; i < files.Count; i++)
            {
                P_ASSET_TRNS_IMG p_ASSET_TRNS_IMG = new P_ASSET_TRNS_IMG();
                p_ASSET_TRNS_IMG.IMG_TRNS = Request["uploadername"];
                HttpPostedFileBase file = files[i];
                string fname;
                // Checking for Internet Explorer      
                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                    fname = testfiles[testfiles.Length - 1];
                }
                else
                {
                    fname = file.FileName;
                }

                var File_Ext = System.IO.Path.GetFileNameWithoutExtension(file.ContentType);
                var newFileName = Guid.NewGuid().ToString() + "." + File_Ext;
                p_ASSET_TRNS_IMG.IMG_NEWN = newFileName;
                p_ASSET_TRNS_IMG.IMG_OLDN = fname;
                p_ASSET_TRNS_IMG.IMG_PATH = Upload_Path + "/" + newFileName;
                db.P_ASSET_TRNS_IMG.Add(p_ASSET_TRNS_IMG);
                db.SaveChanges();
                // Get the complete folder path and store the file inside it.    
                var path = System.IO.Path.Combine(Server.MapPath(Upload_Path), newFileName);
                file.SaveAs(path);
            }
            //success messages
            return Json("Your files uploaded successfully", JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewDocuments(string id)
        {
            var Docs = db.P_ASSET_TRNS_IMG.Where(a => a.IMG_TRNS == id).ToList();
            return PartialView("_ViewDocuments", Docs);
        }
        public ActionResult DownloadDocuments(string fPath, string fName)
        {
            var CombinePath = System.IO.Path.Combine(Server.MapPath(fPath));
            string img = fPath.Split('.').Last();
            try
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(CombinePath);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fName + "." + img);
            }
            catch
            {
                return View();
            }
        }
        public ActionResult SubmitForApp(string id) //submit for approve
        {
            var p_ASSET_TRNS = db.P_ASSET_TRNS.Where(a => a.TRNS_TEXT == id).ToList().FirstOrDefault();
            p_ASSET_TRNS.TRNS_ACTV = true;
            db.Entry(p_ASSET_TRNS).State = EntityState.Modified;
            db.SaveChanges();
            return Json("Request submitted successfully", JsonRequestBehavior.AllowGet);
        }
        public ActionResult ApproveList() //waitting for approval
        {
            var WaittingForApproval = db.P_ASSET_TRNS
                .Where(a => a.TRNS_ACTV == true && a.TRNS_POST == false).ToList();
            return View(WaittingForApproval);
        }
        public ActionResult ApproveSubmitForApp(string id) //approve by approvar
        {
            if (Session["UserId"] != null)
            {
                var getId = Session["UserId"].ToString();

                //inactive previous transfer
                string  AssetId = db.P_ASSET_TRNS.Where(a => a.TRNS_TEXT == id).FirstOrDefault().TRNS_ASET;
                var ActiveTransferList = db.P_ASSET_TRNS.Where(a => a.TRNS_EXPR == true && a.TRNS_ASET == AssetId).ToList();
                if (ActiveTransferList != null) //if not new transfer
                {
                    foreach (var item in ActiveTransferList)
                    {
                        item.TRNS_EXPR = false;
                        db.Entry(item).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                }

                // else { no need disable previous transfer }

                //Approve by User
                var p_ASSET_TRNS = db.P_ASSET_TRNS.Where(a => a.TRNS_TEXT == id).ToList().FirstOrDefault();
                p_ASSET_TRNS.TRNS_POST = true;
                p_ASSET_TRNS.TRNS_EXPR = true;
                p_ASSET_TRNS.TRNS_AUSER = getId; //approve user
                db.Entry(p_ASSET_TRNS).State = EntityState.Modified;
                db.SaveChanges();
                return Json("Request approved successfully", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Time Expired, please login!');window.location.href = '/Home/Login';</script>");
            }
        }
        // GET: AssetTransfer/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            P_ASSET_TRNS p_ASSET_TRNS = db.P_ASSET_TRNS.Find(id);
            if (p_ASSET_TRNS == null)
            {
                return HttpNotFound();
            }
            return View(p_ASSET_TRNS);
        }

        // GET: AssetTransfer/Create
        
        public ActionResult NewTransfer(string NewTransferTypes)
        {
            if (Session["UserId"] != null)
            {
                var getId = Session["UserId"].ToString();

                var MyGroupList = db.P_USERS_ASGR.Where(b => b.USER_TEXT == getId && b.UAP_ACTV == true).Select(s => s.ASGR_TEXT).ToList();
                var MyBeunList = db.P_USERS_BEUN.Where(b => b.USER_TEXT == getId && b.UBP_ACTV == true).Select(s => s.BEUN_TEXT).ToList();
                var MyDept = db.P_DEPT.Where(d => MyBeunList.Contains(d.DEPT_BEUN)).Select(s => s.DEPT_TEXT).ToList();
                if (NewTransferTypes == "NewAsset")
                {
                    ViewBag.TRNS_ASET = new SelectList(db.P_ASSET.Where(a => MyGroupList.Contains(a.ASET_ASGR) && MyDept.Contains(a.ASET_DEPT)).Where(e => e.ASET_ACTV == true && e.ISBOOKED == false), "ASET_OID", "ASET_TEXT").Take(10);
                }
                else if (NewTransferTypes == "xxx")
                {
                    ViewBag.TRNS_ASET = new SelectList(db.P_ASSET.Where(a => MyGroupList.Contains(a.ASET_ASGR) && MyDept.Contains(a.ASET_DEPT)).Where(e => e.ASET_ACTV == true && e.ISBOOKED == true), "ASET_OID", "ASET_TEXT").Take(10);
                }else
                {
                    List<SelectListItem> TRNS_ASET = new List<SelectListItem>();
                    TRNS_ASET.Add(new SelectListItem() { Text = "Default", Value = "0" });
                    ViewBag.TRNS_ASET = new SelectList(TRNS_ASET, "Value", "Text");
                }

                //   ViewBag.TRNS_FDEPO = new SelectList(db.P_AREA, "AREA_TEXT", "AREA_NAME");
                ViewBag.TRNS_TDEPO = new SelectList(db.P_AREA.Where(d => d.AREA_ACTV == true && d.P_THNA.P_DIST.P_COUNTRY.CUNT_TEXT.Contains("KSA")), "AREA_TEXT", "AREA_NAME");
                //List<SelectListItem> FromUser = new List<SelectListItem>();
                //FromUser.Add(new SelectListItem() { Text = "Default", Value = "0" });
                //ViewBag.TRNS_FUSER = new SelectList(FromUser, "Value", "Text");
                //  ViewBag.TRNS_FDEPT = new SelectList(db.P_DEPT, "DEPT_TEXT", "DEPT_NAME");
                // ViewBag.TRNS_FUSER = new SelectList(db.P_OWNR, "OWNR_OID", "OWNR_TEXT");               
                var MyBEUN = db.P_USERS_BEUN.Where(b => b.USER_TEXT == getId && b.UBP_ACTV == true).Select(s => s.BEUN_TEXT).ToList(); //fixed id
                ViewBag.TRNS_TCOMP = new SelectList(db.P_BEUN.Where(u => MyBEUN.Contains(u.BEUN_TEXT)), "BEUN_TEXT", "BEUN_NAME");
                //ViewBag.TRNS_TDEPT = new SelectList(db.P_DEPT, "DEPT_TEXT", "DEPT_NAME");
                ViewBag.TRNS_TUSER = new SelectList(db.P_OWNR, "OWNR_OID", "OWNR_TEXT");
                return View();
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Time Expired, please login!');window.location.href = '/Home/Login';</script>");
            }
        }

        // POST: AssetTransfer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewTransfer([Bind(Include = "TRNS_TEXT,TRNS_FUSER,TRNS_TUSER,TRNS_CUSER,TRNS_AUSER,TRNS_ASET,TRNS_FDEPT,TRNS_TDEPT,TRNS_FDEPO,TRNS_TDEPO,TRNS_DATE,TRNS_NOTE,TRNS_POST,TRNS_ACTV,CUSR,EUSR,CDT,EDT,UDT,VER")] P_ASSET_TRNS p_ASSET_TRNS)
        {
            if (ModelState.IsValid)
            {
                p_ASSET_TRNS.TRNS_TEXT = p_ASSET_TRNS.TRNS_FUSER + p_ASSET_TRNS.TRNS_TUSER + p_ASSET_TRNS.TRNS_ASET + System.DateTime.Now.Day;
                p_ASSET_TRNS.TRNS_CUSER = "78278";
                db.P_ASSET_TRNS.Add(p_ASSET_TRNS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TRNS_FDEPO = new SelectList(db.P_AREA, "AREA_TEXT", "AREA_NAME", p_ASSET_TRNS.TRNS_FDEPO);
            ViewBag.TRNS_TDEPO = new SelectList(db.P_AREA, "AREA_TEXT", "AREA_NAME", p_ASSET_TRNS.TRNS_TDEPO);
            ViewBag.TRNS_ASET = new SelectList(db.P_ASSET, "ASET_OID", "ASET_TEXT", p_ASSET_TRNS.TRNS_ASET);
            ViewBag.TRNS_FDEPT = new SelectList(db.P_DEPT, "DEPT_TEXT", "DEPT_NAME", p_ASSET_TRNS.TRNS_FDEPT);
            ViewBag.TRNS_FUSER = new SelectList(db.P_OWNR, "OWNR_OID", "OWNR_TEXT", p_ASSET_TRNS.TRNS_FUSER);
            ViewBag.TRNS_TDEPT = new SelectList(db.P_DEPT, "DEPT_TEXT", "DEPT_NAME", p_ASSET_TRNS.TRNS_TDEPT);
            ViewBag.TRNS_TUSER = new SelectList(db.P_OWNR, "OWNR_OID", "OWNR_TEXT", p_ASSET_TRNS.TRNS_TUSER);
            return View(p_ASSET_TRNS);
        }
        public ActionResult getAssetData(string id)
        {
            var Asset = db.P_ASSET.Where(a => a.ASET_OID == id).ToList();
            var getAcceGroup = Asset.Select(s => s.ASET_ASGR);
            //get all accesoreis item by asset group
            var getCurrentAcceList = db.P_ASSET_ACCE.Where(a => a.ACCE_ACTV == true && a.ACCE_ASET == id).ToList().Select(s => s.ACCE_ASAC);
            //get rest acceorise item without already added, remove duplicate
            var getListWithoutCurrent = new SelectList(db.P_ASAC.Where(a => getAcceGroup.Contains(a.ASAC_ASGR))
                .Where(b => !getCurrentAcceList.Contains(b.ASAC_TEXT)), "ASAC_TEXT", "ASAC_NAME");
            
            //get Current User
            var GetLastTransfer = db.P_ASSET_TRNS.Where(t => t.TRNS_ASET == id && t.TRNS_EXPR==true).ToList();
            List<SelectListItem> getFromUserList = new List<SelectListItem>();
            List<SelectListItem> returnCompDept = new List<SelectListItem>();
            List<SelectListItem> retrunAreaDepot = new List<SelectListItem>();
            if (GetLastTransfer.Count==0) // Get User from transfer History
            {
                //return default user for first transfer
                getFromUserList.Add(new SelectListItem() { Text = "Default", Value = "0" });
                //return default department for first transfer
                var Dept = Asset.Select(s => s.ASET_DEPT);
                var Department = db.P_DEPT.Where(d => Dept.Contains(d.DEPT_TEXT)).ToList();
                foreach(var item in Department)
                {
                    //get from single Department by Company List by Vehicle
                    returnCompDept.Add(new SelectListItem() { Text = item.DEPT_NAME, Value = item.DEPT_TEXT });
                }

                var Area = Asset.Select(s => s.ASET_AREA);
                //get from depot single List by Vehicle 
                var AreaDepot = db.P_AREA.Where(a => Area.Contains(a.AREA_TEXT)).ToList();
                foreach (var item in AreaDepot)
                {
                    //get from single Department by Company List by Vehicle
                    retrunAreaDepot.Add(new SelectListItem() { Text = item.AREA_NAME, Value = item.AREA_TEXT });
                }
            }
            else
            {
                var OnlyUserID = GetLastTransfer.Select(s => s.TRNS_TUSER); //select only user Id from Transfer List
                var FromUserData = db.P_OWNR.Where(o => OnlyUserID.Contains(o.OWNR_OID)).ToList();// filter user by selected id
                foreach (var item in FromUserData)
                {
                    getFromUserList.Add(new SelectListItem() { Text = item.OWNR_TEXT, Value = item.OWNR_OID });
                }
                //return  department for vehicle  last transfer 
                var Dept = GetLastTransfer.Select(s => s.TRNS_TDEPT);
                var Department = db.P_DEPT.Where(d => Dept.Contains(d.DEPT_TEXT)).ToList();
                foreach (var item in Department)
                {
                    //get from single Department by Company List by Vehicle last transfer
                    returnCompDept.Add(new SelectListItem() { Text = item.DEPT_NAME, Value = item.DEPT_TEXT });
                }

                var Area = GetLastTransfer.Select(s => s.TRNS_TDEPO);
                //get from depot single List by Vehicle transfer
                var AreaDepot = db.P_AREA.Where(a => Area.Contains(a.AREA_TEXT)).ToList();
                foreach (var item in AreaDepot)
                {
                    //get from single Department by Company List by Vehicle last transfer
                    retrunAreaDepot.Add(new SelectListItem() { Text = item.AREA_NAME, Value = item.AREA_TEXT });
                }

            }
            return Json(new { DeptList = returnCompDept, DepotList = retrunAreaDepot, NewAcceList = getListWithoutCurrent, FromUserList= getFromUserList }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveTransfer(string TRNS_ASET, string TRNS_DATE, string TRNS_NOTE, string TRNS_TUSER, string TRNS_TDEPT, string TRNS_TDEPO,string TRNS_FUSER,string TRNS_FDEPT,string TRNS_FDEPO, TransferAcceItems[] acceitemlist)
        {//string TRNS_POST,
            if (Session["UserId"] != null)
            {
                var getId = Session["UserId"].ToString();

                if (ModelState.IsValid)
                {
                    //bool IsNewTrns = true;
                    //string CurrentUser = "0";
                    //if (IsNewTrns != true)
                    //{
                    //    CurrentUser = db.P_ASSET_TRNS.Where(a => a.TRNS_ACTV == true && a.TRNS_POST == true).FirstOrDefault().TRNS_TUSER; //old transfer user
                    //}

                  //  var Asset = db.P_ASSET.Where(a => a.ASET_OID == TRNS_ASET).ToList();
                    DateTime TrnDate = DateTime.Parse(TRNS_DATE);

                    P_ASSET_TRNS p_ASSET_TRNS = new P_ASSET_TRNS();
                    p_ASSET_TRNS.TRNS_TEXT = TRNS_ASET + TRNS_TUSER + System.DateTime.Now.Day;
                    p_ASSET_TRNS.TRNS_FUSER = TRNS_FUSER;// CurrentUser;//new transfer user '0'
                    p_ASSET_TRNS.TRNS_TUSER = TRNS_TUSER;
                    p_ASSET_TRNS.TRNS_CUSER = getId;
                    p_ASSET_TRNS.TRNS_ASET = TRNS_ASET;
                    p_ASSET_TRNS.TRNS_FDEPT = TRNS_FDEPT;// Asset.Select(s => s.ASET_DEPT).FirstOrDefault();
                    p_ASSET_TRNS.TRNS_TDEPT = TRNS_TDEPT;
                    p_ASSET_TRNS.TRNS_FDEPO = TRNS_FDEPO;// Asset.Select(s => s.ASET_AREA).FirstOrDefault();
                    p_ASSET_TRNS.TRNS_TDEPO = TRNS_TDEPO;
                    p_ASSET_TRNS.TRNS_DATE = TrnDate;
                    p_ASSET_TRNS.TRNS_NOTE = TRNS_NOTE;
                    p_ASSET_TRNS.TRNS_POST = false;
                    p_ASSET_TRNS.TRNS_ACTV = false;
                    p_ASSET_TRNS.TRNS_EXPR = false;
                    db.P_ASSET_TRNS.Add(p_ASSET_TRNS);
                    db.SaveChanges();
                    //Booking booked > Asset Marked as Booked
                    var param1 = new OracleParameter("ASET_ID", OracleDbType.Int32, TRNS_ASET, ParameterDirection.Input);
                    var param2 = new OracleParameter("BOOKING", OracleDbType.Int32, 1, ParameterDirection.Input);
                    db.Database.ExecuteSqlCommand("BEGIN P_ISBOOKED_BOOKING(:ASET_ID, :BOOKING); end;", param1, param2);
                    //remove Old Acce List
                    var p_ASSET_ACCE_r = db.P_ASSET_ACCE.Where(a => a.ACCE_ASET == TRNS_ASET).ToList();
                    foreach (var item in p_ASSET_ACCE_r)
                    {
                        item.ACCE_ACTV = false;
                        db.Entry(item).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    //Add New Acce List
                    foreach (var item in acceitemlist)
                    {
                        P_ASSET_ACCE p_ASSET_ACCE = new P_ASSET_ACCE();
                        p_ASSET_ACCE.ACCE_TEXT = TRNS_ASET + TRNS_TUSER + System.DateTime.Now.Day + item.ACCE_ASAC;
                        p_ASSET_ACCE.ACCE_ASAC = item.ACCE_ASAC;
                        p_ASSET_ACCE.ACCE_ASET = TRNS_ASET;
                        p_ASSET_ACCE.ACCE_FUSER = TRNS_FUSER;//CurrentUser;//new transfer user '0'
                        p_ASSET_ACCE.ACCE_TUSER = TRNS_TUSER;
                        p_ASSET_ACCE.ACCE_TRNS = TRNS_ASET + TRNS_TUSER + System.DateTime.Now.Day;
                        p_ASSET_ACCE.ACCE_QTY = item.ACCE_QTY;
                        p_ASSET_ACCE.ACCE_PRICE = item.ACCE_PRICE;
                        p_ASSET_ACCE.ACCE_DATE = System.DateTime.Now.Date;
                        p_ASSET_ACCE.ACCE_NOTE = item.ACCE_NOTE;
                        p_ASSET_ACCE.ACCE_ACTV = true;
                        db.P_ASSET_ACCE.Add(p_ASSET_ACCE);
                    }
                    db.SaveChanges();
                    //Success Messages
                 //   TempData["msg"] = "<div id='snackbar'>Transfer draft successfully.</div><script>var x = document.getElementById('snackbar');  x.className = 'show success'; setTimeout(function() { x.className = x.className.replace('show', ''); }, 3000);</script>";
                    //return Json("Transfer draft succesfully!", JsonRequestBehavior.AllowGet);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                TempData["msg"] = "<div id='snackbar'>Transfer not saved.</div><script>var x = document.getElementById('snackbar');  x.className = 'show danger'; setTimeout(function() { x.className = x.className.replace('show', ''); }, 3000);</script>";
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Time Expired, please login!');window.location.href = '/Home/Login';</script>");
            }
        }
        public ActionResult getToDeptList(string id)
        {
            var retunDeptList = new SelectList(db.P_DEPT.Where(d => d.DEPT_BEUN == id && d.DEPT_ACTV == true), "DEPT_TEXT", "DEPT_NAME");
            return Json(new { retunDeptList = retunDeptList }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ShowAccessories(string id)
        {
            var allAcce = db.P_ASSET_ACCE.Where(a => a.ACCE_ASET == id && a.ACCE_ACTV == true).ToList();
            return PartialView("_ShowAccessories", allAcce);
        }

        // GET: AssetTransfer/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            P_ASSET_TRNS p_ASSET_TRNS = db.P_ASSET_TRNS.Find(id);
            if (p_ASSET_TRNS == null)
            {
                return HttpNotFound();
            }
            ViewBag.TRNS_FDEPO = new SelectList(db.P_AREA, "AREA_TEXT", "AREA_NAME", p_ASSET_TRNS.TRNS_FDEPO);
            ViewBag.TRNS_TDEPO = new SelectList(db.P_AREA, "AREA_TEXT", "AREA_NAME", p_ASSET_TRNS.TRNS_TDEPO);
            ViewBag.TRNS_ASET = new SelectList(db.P_ASSET, "ASET_OID", "ASET_TEXT", p_ASSET_TRNS.TRNS_ASET);
            ViewBag.TRNS_FDEPT = new SelectList(db.P_DEPT, "DEPT_TEXT", "DEPT_NAME", p_ASSET_TRNS.TRNS_FDEPT);
            ViewBag.TRNS_FUSER = new SelectList(db.P_OWNR, "OWNR_OID", "OWNR_TEXT", p_ASSET_TRNS.TRNS_FUSER);
            ViewBag.TRNS_TDEPT = new SelectList(db.P_DEPT, "DEPT_TEXT", "DEPT_NAME", p_ASSET_TRNS.TRNS_TDEPT);
            ViewBag.TRNS_TUSER = new SelectList(db.P_OWNR, "OWNR_OID", "OWNR_TEXT", p_ASSET_TRNS.TRNS_TUSER);
            return View(p_ASSET_TRNS);
        }

        // POST: AssetTransfer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TRNS_TEXT,TRNS_FUSER,TRNS_TUSER,TRNS_CUSER,TRNS_AUSER,TRNS_ASET,TRNS_FDEPT,TRNS_TDEPT,TRNS_FDEPO,TRNS_TDEPO,TRNS_DATE,TRNS_NOTE,TRNS_POST,TRNS_ACTV,CUSR,EUSR,CDT,EDT,UDT,VER")] P_ASSET_TRNS p_ASSET_TRNS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(p_ASSET_TRNS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TRNS_FDEPO = new SelectList(db.P_AREA, "AREA_TEXT", "AREA_NAME", p_ASSET_TRNS.TRNS_FDEPO);
            ViewBag.TRNS_TDEPO = new SelectList(db.P_AREA, "AREA_TEXT", "AREA_NAME", p_ASSET_TRNS.TRNS_TDEPO);
            ViewBag.TRNS_ASET = new SelectList(db.P_ASSET, "ASET_OID", "ASET_TEXT", p_ASSET_TRNS.TRNS_ASET);
            ViewBag.TRNS_FDEPT = new SelectList(db.P_DEPT, "DEPT_TEXT", "DEPT_NAME", p_ASSET_TRNS.TRNS_FDEPT);
            ViewBag.TRNS_FUSER = new SelectList(db.P_OWNR, "OWNR_OID", "OWNR_TEXT", p_ASSET_TRNS.TRNS_FUSER);
            ViewBag.TRNS_TDEPT = new SelectList(db.P_DEPT, "DEPT_TEXT", "DEPT_NAME", p_ASSET_TRNS.TRNS_TDEPT);
            ViewBag.TRNS_TUSER = new SelectList(db.P_OWNR, "OWNR_OID", "OWNR_TEXT", p_ASSET_TRNS.TRNS_TUSER);
            return View(p_ASSET_TRNS);
        }

        // GET: AssetTransfer/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            P_ASSET_TRNS p_ASSET_TRNS = db.P_ASSET_TRNS.Find(id);
            if (p_ASSET_TRNS == null)
            {
                return HttpNotFound();
            }
            return View(p_ASSET_TRNS);
        }

        // POST: AssetTransfer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            P_ASSET_TRNS p_ASSET_TRNS = db.P_ASSET_TRNS.Find(id);
            db.P_ASSET_TRNS.Remove(p_ASSET_TRNS);
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
