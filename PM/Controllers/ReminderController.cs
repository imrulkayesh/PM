using PM.BLL;
using PM.Model;
using PM.ModelsV;
using PM.MyAuth;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace PM.Controllers
{
    [Authorize]
    [CustomHttpReffer]
    public class ReminderController : Controller
    {
        private PMEntities db = new PMEntities();
        ReturnPermission RP = new ReturnPermission();
        GE Sequence = new GE();
        [KAuth(801)]
        public ActionResult Index(string Asset, string Operation)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            #region old
            //List<string> ASET = RP.MyAssetListString(UserId: U, UserType: T);
            //var DBR = (from d in db.VW_DATE_NOTIF
            //           where ASET.Contains(d.ASET_ID)
            //           select new ReminderVM
            //           {
            //               REMIND_ID = d.REMIND_ID,
            //               ASET_ID = d.ASET_ID,
            //               ASET_TAG = d.ASET_TAG,
            //               ITEM_ID = d.ITEM_ID,
            //               ITEM_NAME = d.ITEM_NAME,
            //               C_DATE = d.C_DATE,
            //               N_DATE = d.N_DATE,
            //               ELAPSED = d.ELAPSED,
            //               REMIND = d.REMIND,
            //               NOTIF_FIRST = d.NOTIF_FIRST,
            //               NOTIF_FINAL = d.NOTIF_FINAL,
            //               REMINDER_NOTE = d.REMINDER_NOTE,
            //               NOTIF_TYPE = d.NOTIF_TYPE,
            //               NOTE = d.NOTE,
            //               COMPLETED = d.COMPLETED
            //           }
            //                           ).ToList(); 
            #endregion

            string sql_1 = @"SELECT 
                            dv.REMIND_ID,dv.ASET_ID,dv.ASET_TAG,dv.ITEM_ID,dv.ITEM_NAME,dv.C_DATE,
                            dv.N_DATE,dv.ELAPSED,dv.REMIND,dv.NOTIF_FIRST,dv.NOTIF_FINAL,
                            dv.REMINDER_NOTE,dv.NOTIF_TYPE,dv.NOTE,dv.COMPLETED
                            FROM 
                            PM.VW_DATE_NOTIF DV
                            INNER JOIN A_ASET_DATA T6 ON T6.ASET_ID=DV.ASET_ID AND T6.ACT=1
                            INNER JOIN A_USERS_GROUP T5 ON T6.ASET_GROUP=T5.UG_GROUP AND T5.UG_USER='" + U + @"' AND T5.ACT=1
                            INNER JOIN A_DEPARTMENT T7 ON T6.ASET_DEPARTMENT=T7.DEPARTMENT_ID AND T7.ACT=1
                            INNER JOIN A_BUSINESS T8 ON T7.DEPARTMENT_BUSINESS=T8.BUSINESS_ID AND T8.ACT=1
                            INNER JOIN A_USERS_BUS T9 ON T8.BUSINESS_ID =T9.UB_BUSINESS AND T9.UB_USER=T5.UG_USER AND T9.ACT=1 
                            INNER JOIN (SELECT DISTINCT(AREA_ID) FROM (SELECT T.AREA_ID
                            FROM A_AREA T
                            INNER JOIN A_THANA T1 ON T.AREA_THANA=T1.THANA_ID AND T1.ACT=1
                            INNER JOIN A_DISTRICT T2 ON T1.THANA_DISTRICT=T2.DISTRICT_ID AND T2.ACT=1
                            INNER JOIN A_USERS_COUNTRY T3 ON T2.DISTRICT_COUNTRY=T3.COUNTRY_ID AND T3.ACT=1 AND T3.USER_TEXT='" + U + @"'
                            WHERE T.ACT=1
                            UNION ALL
                            SELECT T.UA_AREA
                            FROM A_USERS_AREA T WHERE T.ACT=1 AND T.UA_USER='" + U + @"'
                            )) T10 ON T6.ASET_AREA=T10.AREA_ID";
            var DBR = db.Database.SqlQuery<ReminderVM>(sql: sql_1).ToList();

            if (!string.IsNullOrEmpty(Asset))
            {
                var R = DBR.Where(a => Asset.Contains(a.ASET_ID)
                     || Asset.Contains(a.ASET_TAG)
                     || Asset.Contains(a.ITEM_ID)
                     || Asset.Contains(a.ITEM_NAME))
                    .ToList();
                if (!string.IsNullOrEmpty(Operation))
                {
                    ViewBag.Message = "Search Result";
                }
                return View(R);
            }
            else
            {
                ViewBag.Message = "All Document";
                var R = DBR.OrderBy(a => a.REMINDER_NOTE).ToList();
                return View(R);
            }

        }

        [KAuth(801)]
        public ActionResult NewReminder()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            ViewBag.ASET_BUSINESS = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
            ViewBag.ASET_GROUP = new SelectList(RP.MyGroup(UserId: U, UserType: T), "GROUP_ID", "GROUP_NAME");
            return View();
        }

        //[KAuth(802)]
        //public ActionResult NewDocumentReminder()
        //{
        //    string U = Session["UserId"].ToString();
        //    string T = Session["UserRole"].ToString();
        //    ViewBag.ASET_BUSINESS = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
        //    ViewBag.ASET_GROUP = new SelectList(RP.MyGroup(UserId: U, UserType: T), "GROUP_ID", "GROUP_NAME");
        //    return View();
        //}

        [KAuth(801)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewReminder(newReminderVM RV)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            if (ModelState.IsValid)
            {
                int fileSize = RV.FileX.ContentLength;
                if (fileSize <= 0 || fileSize > 2100000)
                {
                    TempData["msg"] = "swal('Opps..','File is too larger, Max file size 2MB','warning');";
                    ViewBag.ASET_BUSINESS = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
                    ViewBag.ASET_GROUP = new SelectList(RP.MyGroup(UserId: U, UserType: T), "GROUP_ID", "GROUP_NAME");
                    return View();
                }

                if (RV.C_DATE > RV.N_DATE)
                {
                    TempData["msg"] = "swal('Opss..','End Date must be greater than Start Date','warning');";
                    ViewBag.ASET_BUSINESS = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
                    ViewBag.ASET_GROUP = new SelectList(RP.MyGroup(UserId: U, UserType: T), "GROUP_ID", "GROUP_NAME");
                    return View();
                }
                A_REMINDER R = new A_REMINDER();
                R.ASET_ID = RV.ASET_ID;
                R.ITEM_ID = RV.ITEM_ID;
                R.C_DATE = RV.C_DATE;
                R.N_DATE = RV.N_DATE;
                R.NOTE = RV.NOTE;
                R.COMPLETED = "N";
                R.REMIND = "0";
                R.REMIND_ID = Sequence.GetSeq(14).ToString(); //reminder Id
                R.CDU = U;
                R.CDT = DateTime.Now;
                R.VAR = 1;
                db.A_REMINDER.Add(R); //Reminder Data

                A_REMINDER_IMG I = new A_REMINDER_IMG();
                I.IMG_ID = Guid.NewGuid().ToString();
                I.IMG_NAME = RV.FileX.FileName;
                if (RV.ASET_GROUP == "GR0005")
                {
                    I.IMG_PATH = Sequence.GetImgPath(102);//get file path PTT
                }
                else
                {
                    I.IMG_PATH = Sequence.GetImgPath(100);//get file path
                }

                I.IMG_REMINDER = R.REMIND_ID; //reminder Id
                db.A_REMINDER_IMG.Add(I);

                //upload file to server
                var path = System.IO.Path.Combine(Server.MapPath(I.IMG_PATH), I.IMG_ID);
                RV.FileX.SaveAs(path);
                TempData["msg"] = "swal('Success..','Reminder started successfully','success');";
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            ViewBag.ASET_BUSINESS = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
            ViewBag.ASET_GROUP = new SelectList(RP.MyGroup(UserId: U, UserType: T), "GROUP_ID", "GROUP_NAME");
            TempData["msg"] = "swal('Ohh..','Reminder added failed','error');";
            return View();
        }

        [KAuth(801)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateRange(UpdateReminder UR)
        {
            string U = Session["UserId"].ToString();
            if (ModelState.IsValid)
            {
                if (UR.C_DATE > UR.N_DATE)
                {
                    TempData["msg"] = "swal('Opps..','End Date must be greater than Start Date','warning');";
                    return RedirectToAction("Index");
                }
                A_REMINDER AR = db.A_REMINDER.Find(UR.REMIND_ID);
                AR.C_DATE = UR.C_DATE;
                AR.N_DATE = UR.N_DATE;
                AR.REMIND = "0";
                AR.UDU = U;
                AR.UDT = DateTime.Now;
                AR.VAR = AR.VAR + 1;
                db.Entry(AR).State = EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Date Range Updated','success');";
                return RedirectToAction("Index");
            }
            TempData["msg"] = "swal('Ohh..','Reminder update failed','error');";
            return RedirectToAction("Index");
        }
        [KAuth(801)]
        public ActionResult Download(string id)
        {
            var dbDATA = db.A_REMINDER_IMG.Where(a => a.IMG_REMINDER == id);

            if (dbDATA.ToList().Count == 0)
            {
                TempData["msg"] = "swal('Opps..','File not found','warning');";
                return RedirectToAction("Index");
            }
            var Upload_Path = dbDATA.FirstOrDefault().IMG_PATH;
            var GetFileGUIDname = dbDATA.FirstOrDefault().IMG_ID;
            var GetFileOrgName = dbDATA.FirstOrDefault().IMG_NAME;
            var CombinePath = System.IO.Path.Combine(Server.MapPath(Upload_Path), GetFileGUIDname);
            try
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(CombinePath);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, GetFileOrgName);
            }
            catch
            {
                TempData["msg"] = "swal('Ohh..','Error in File','error');";
                return RedirectToAction("Index");
            }
        }

        //[Authorize(Roles = "ADMIN,FUSER,")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult DelReminder(DeleteReminder DR)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        A_REMINDER AR = db.A_REMINDER.Find(DR.REMIND_DEL); //delete Reminder
        //        if (AR == null)
        //        {
        //            return HttpNotFound();
        //        }
        //        db.A_REMINDER.Remove(AR);

        //        A_REMINDER_IMG AI = db.A_REMINDER_IMG.Where(i => i.IMG_REMINDER == DR.REMIND_DEL).FirstOrDefault();
        //        if (AI != null)
        //        {
        //            db.A_REMINDER_IMG.Remove(AI); //delete reminder image
        //        }
        //        db.SaveChanges();

        //        string fullPath = Request.MapPath(AI.IMG_PATH + "//" + AI.IMG_ID);
        //        if (System.IO.File.Exists(fullPath))
        //        {
        //            System.IO.File.SetAttributes(fullPath, FileAttributes.Normal);
        //            System.IO.File.Delete(fullPath);
        //        }
        //        TempData["msg"] = "swal('Success..','Reminder deleted','success');";
        //        return RedirectToAction("Index");
        //    }
        //    TempData["msg"] = "swal('Ohh..','Reminder delete failed','error');";
        //    return RedirectToAction("Index");

        //}

        [KAuth(801)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Completed(CompleteReminder CR)
        {
            string U = Session["UserId"].ToString();
            if (ModelState.IsValid)
            {
                A_REMINDER AR = db.A_REMINDER.Find(CR.REMIND_COM);
                AR.COMPLETED = "Y";
                AR.UDU = U;
                AR.UDT = DateTime.Now;
                AR.VAR = AR.VAR + 1;
                db.Entry(AR).State = EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Reminder completed','success');";
                return RedirectToAction("Index");
            }
            TempData["msg"] = "swal('Opps..','Update failed','error');";
            return RedirectToAction("Index");
        }
    }
}