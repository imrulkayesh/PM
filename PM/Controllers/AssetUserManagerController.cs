using PM.BLL;
using PM.Model;
using PM.ModelsV;
using PM.MyAuth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PM.Controllers
{
    [Authorize]
    [CustomHttpReffer]
    public class AssetUserManagerController : Controller
    {
        private PMEntities db = new PMEntities();
        ReturnPermission RP = new ReturnPermission();
        GE Sequence = new GE();
        InstantEmailSender IES = new InstantEmailSender();
        private PMExcelDataLoader EDL = new PMExcelDataLoader();
        public List<AssetUserManagerVM> IndexData(string User, string UserType)
        {
            #region OLD
            //List<string> AssetList = RP.MyAssetListString(UserId: User, UserType: UserType);
            //var TransferList = (from a in db.A_ASET_USER
            //                    where a.ACT == true
            //                    && AssetList.Contains(a.AU_ASET)
            //                    from TU in db.A_USERS.Where(x => a.AU_SRUSER == x.USER_TEXT).DefaultIfEmpty()
            //                    from TA in db.A_USERS.Where(x => a.AU_SAUSER == x.USER_TEXT).DefaultIfEmpty()
            //                    from RR in db.A_USERS.Where(x => a.AU_ERUSER == x.USER_TEXT).DefaultIfEmpty()
            //                    from RA in db.A_USERS.Where(x => a.AU_EAUSER == x.USER_TEXT).DefaultIfEmpty()
            //                    select new AssetUserManagerVM
            //                    {
            //                        AU_ID = a.AU_ID,
            //                        AU_ASET = a.A_ASET_DATA.ASET_CODE_NAME,//a.AU_ASET,
            //                        AU_USER_U = a.AU_USER,//Staff Id
            //                        AU_USER = a.A_OWNR.OWNR_USER,// a.AU_USER,
            //                        AU_DEPT = a.A_DEPARTMENT.DEPARTMENT_NAME,//depeartment
            //                        AU_AREA = a.A_AREA.AREA_NAME,//area
            //                        AU_SRDATE = a.AU_SRDATE,
            //                        AU_SRUSER = TU.USER_NAME,//a.AU_SRUSER,
            //                        AU_SADATE = a.AU_SADATE,
            //                        AU_SAUSER = TA.USER_NAME,// a.AU_SAUSER,
            //                        //AU_SAUSER = a.AU_SAUSER,
            //                        AU_SNOTE = a.AU_SNOTE,
            //                        AU_ERDATE = a.AU_ERDATE,
            //                        AU_ERUSER = RR.USER_NAME,//a.AU_ERUSER,
            //                                                 // AU_ERUSER = a.AU_ERUSER,
            //                        AU_EADATE = a.AU_EADATE,
            //                        AU_EAUSER = RA.USER_NAME,// a.AU_EAUSER,
            //                        //AU_EAUSER = a.AU_EAUSER,
            //                        AU_ENOTE = a.AU_ENOTE,
            //                        AU_TRNSTYPE = a.AU_TRNSTYPE
            //                    }).Take(100).ToList().OrderByDescending(x => x.AU_TRNSTYPE);
            //return TransferList.ToList(); 
            #endregion


            string sql_1 = @"SELECT T.AU_ID,T6.ASET_CODE_NAME AU_ASET,T.AU_USER AU_USER_U,T11.OWNR_USER AU_USER,T12.DEPARTMENT_NAME AU_DEPT,T13.AREA_NAME AU_AREA,T.AU_SRDATE,
                    T1.USER_NAME AU_SRUSER,T.AU_SADATE,T2.USER_NAME AU_SAUSER,T.AU_SNOTE,T.AU_ERDATE,T3.USER_NAME AU_ERUSER,T.AU_EADATE,T4.USER_NAME,T.AU_ENOTE,T.AU_TRNSTYPE
                    FROM A_ASET_USER T
                    INNER JOIN A_ASET_DATA T6 ON T6.ASET_ID=T.AU_ASET AND T6.ACT=1
                    INNER JOIN A_OWNR T11 ON T.AU_USER=T11.OWNR_ID
                    INNER JOIN A_DEPARTMENT T12 ON T.AU_DEPT=T12.DEPARTMENT_ID
                    INNER JOIN A_AREA T13 ON T.AU_AREA=T13.AREA_ID
                    INNER JOIN A_USERS_GROUP T5 ON T6.ASET_GROUP=T5.UG_GROUP AND T5.UG_USER='" + User + @"' AND T5.ACT=1
                    INNER JOIN A_DEPARTMENT T7 ON T6.ASET_DEPARTMENT=T7.DEPARTMENT_ID AND T7.ACT=1
                    INNER JOIN A_BUSINESS T8 ON T7.DEPARTMENT_BUSINESS=T8.BUSINESS_ID AND T8.ACT=1
                    INNER JOIN A_USERS_BUS T9 ON T8.BUSINESS_ID =T9.UB_BUSINESS AND T9.UB_USER=T5.UG_USER AND T9.ACT=1 
                    INNER JOIN (SELECT DISTINCT(AREA_ID) FROM (SELECT T.AREA_ID
                    FROM A_AREA T
                    INNER JOIN A_THANA T1 ON T.AREA_THANA=T1.THANA_ID AND T1.ACT=1
                    INNER JOIN A_DISTRICT T2 ON T1.THANA_DISTRICT=T2.DISTRICT_ID AND T2.ACT=1
                    INNER JOIN A_USERS_COUNTRY T3 ON T2.DISTRICT_COUNTRY=T3.COUNTRY_ID AND T3.ACT=1 AND T3.USER_TEXT='" + User + @"'
                    WHERE T.ACT=1
                    UNION ALL
                    SELECT T.UA_AREA
                    FROM A_USERS_AREA T WHERE T.ACT=1 AND T.UA_USER='" + User + @"'
                    )) T10 ON T6.ASET_AREA=T10.AREA_ID
                    LEFT JOIN A_USERS T1 ON T.AU_SRUSER=T1.USER_TEXT
                    LEFT JOIN A_USERS T2 ON T.AU_SAUSER=T2.USER_TEXT
                    LEFT JOIN A_USERS T3 ON T.AU_ERUSER=T3.USER_TEXT
                    LEFT JOIN A_USERS T4 ON T.AU_EAUSER=T4.USER_TEXT
                    WHERE T.ACT=1 
                    ORDER BY T.AU_TRNSTYPE DESC";
            return db.Database.SqlQuery<AssetUserManagerVM>(sql: sql_1).Take(500).ToList();
        }

        [KAuth(301)]
        public ActionResult Index(string AssetUser, string Operation)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            //List<AssetUserManagerVM> TransferList = IndexData(User: U, UserType: T).ToList();
            var TransferList = db.VW_ASSETUSERMANAGERVM.Where(u => u.UG_USER == U);
            if (!string.IsNullOrEmpty(AssetUser))
            {
                var R = TransferList.Where(a => AssetUser.Contains(a.OWNR_TEXT)
                     || AssetUser.Contains(a.OWNR_TEXT) || AssetUser.Contains(a.ASET_CODE_NAME)
                     || AssetUser.Contains(a.ASET_CODE_NAME.Substring(a.ASET_CODE_NAME.Length - 4))
                     )
                    .ToList();
                if (!string.IsNullOrEmpty(Operation))
                {
                    ViewBag.Message = "Search Results";
                }
                return View(R);
            }
            else
            {
                ViewBag.Message = "100 Transfer List";
                var R = TransferList.Take(100).ToList();
                return View(R);
            }
        }

        // old query

        //    public ActionResult SearchOldData(string AssetUser, string Operation)
        //    {
        //        string U = Session["UserId"].ToString();
        //        string Sql = string.Format(@"select au.au_id, a.aset_code_name, o.ownr_text, o.ownr_user,
        //      d.department_name, r.area_name, au.au_srdate, a1.user_name t_user,
        //      au.au_sadate, a2.user_name t_approver, au.au_snote, au.au_erdate,
        //      a3.user_name r_user, au.au_eadate, a4.user_name r_approver,
        //      au.au_enote, v.ug_user, au.au_trnstype
        // from a_aset_user au join a_area r on au.au_area = r.area_id
        //      join a_department d on au.au_dept = d.department_id
        //      join a_aset_data a on au.au_aset = a.aset_id
        //      join a_ownr o on au.au_user = o.ownr_id
        //      join vw_user_asset_id_string v on au.au_aset = v.aset_id
        //      left join a_users a1 on au.au_sruser = a1.user_text
        //      left join a_users a2 on au.au_sauser = a2.user_text
        //      left join a_users a3 on au.au_eruser = a3.user_text
        //      left join a_users a4 on au.au_eauser = a4.user_text
        //where a.aset_code_name like'%{0}' and v.ug_user='{1}'", AssetUser, U);
        //        List<VW_ASSETUSERMANAGERVM> OldData = db.Database.SqlQuery<VW_ASSETUSERMANAGERVM>(Sql).ToList();
        //        return View("Index", OldData);
        //    }

        public ActionResult SearchOldData(string AssetUser, string Operation)
        {
            string U = Session["UserId"].ToString();
            string Sql = string.Format(@"select au.au_id, a.aset_code_name, o.ownr_text, o.ownr_user,
          d.department_name, r.area_name, au.au_srdate, a1.user_name t_user,
          au.au_sadate, a2.user_name t_approver, au.au_snote, au.au_erdate,
          a3.user_name r_user, au.au_eadate, a4.user_name r_approver,
          au.au_enote, v.ug_user, au.au_trnstype,b.BRAND_NAME,
          bo.BODY_NAME,
          a.ASET_MODEL
     from a_aset_user au join a_area r on au.au_area = r.area_id
          join a_department d on au.au_dept = d.department_id
          join a_aset_data a on au.au_aset = a.aset_id
          join a_ownr o on au.au_user = o.ownr_id
          JOIN a_brand b ON a.ASET_BRAND = b.BRAND_ID
          JOIN a_body bo ON a.ASET_BODY = bo.BODY_ID
          join vw_user_asset_id_string v on au.au_aset = v.aset_id
          left join a_users a1 on au.au_sruser = a1.user_text
          left join a_users a2 on au.au_sauser = a2.user_text
          left join a_users a3 on au.au_eruser = a3.user_text
          left join a_users a4 on au.au_eauser = a4.user_text
    where a.aset_code_name like'%{0}' and v.ug_user='{1}'", AssetUser, U);
            List<VW_ASSETUSERMANAGERVM> OldData = db.Database.SqlQuery<VW_ASSETUSERMANAGERVM>(Sql).ToList();
            return View("Index", OldData);
        }

        [KAuth(301)]
        public ActionResult NewAssetUser()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            ViewBag.F_ASET_BUSINESS = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
            ViewBag.T_ASET_BUSINESS = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
            ViewBag.U_ASET_BUSINESS = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
            ViewBag.F_ASET_GROUP = new SelectList(RP.MyGroup(UserId: U, UserType: T), "GROUP_ID", "GROUP_NAME");
            ViewBag.U_ASET_AREA = new SelectList(RP.MyAreaList(UserId: U, UserType: T), "AREA_ID", "AREA_NAME");
            ViewBag.T_ASET_AREA = new SelectList(RP.MyAreaList(UserId: U, UserType: T), "AREA_ID", "AREA_NAME");
            List<SelectListItem> TRNSTYPE = new List<SelectListItem>();
            TRNSTYPE.Add(new SelectListItem { Text = "General", Value = "G", Selected = true });
            TRNSTYPE.Add(new SelectListItem { Text = "Short Period", Value = "S" });
            ViewBag.AU_TRNSTYPE = TRNSTYPE;
            return View();
        }
        [KAuth(301)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewAssetUser(StartNew SN)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (ModelState.IsValid)
            {
                string MSG = "swal('Success..','Transfer started successfully, Waiting for Approval','success');";
                A_ASET_USER AU = new A_ASET_USER();
                AU.AU_ASET = SN.F_ASET_ID;
                AU.AU_USER = SN.U_ASET_USER;
                AU.AU_DEPT = SN.T_ASET_DEPARTMENT;
                AU.AU_AREA = SN.T_ASET_AREA;
                AU.AU_SRDATE = SN.AU_SRDATE;// DateTime.Now.Date;
                AU.AU_SRUSER = U;
                AU.AU_SNOTE = SN.ASET_NOTE;
                AU.AU_SMAILAGE = SN.AU_SMILEAGE;
                AU.AU_TRNSTYPE = SN.AU_TRNSTYPE;
                AU.ACT = true;
                AU.CDT = DateTime.Now;
                AU.CDU = U;
                AU.VAR = 1;
                if (SN.AU_TRNSTYPE == "S")
                {
                    MSG = "swal('Success..','Transfer started successfully, With Auto Approval','success');";
                    if (!string.IsNullOrEmpty(SN.ASET_NOTE) && SN.ASET_NOTE.Length >= 37)
                        AU.AU_SNOTE = "ShortPeriod-" + SN.ASET_NOTE.Substring(0, 37);
                    else
                        AU.AU_SNOTE = "ShortPeriod-" + SN.ASET_NOTE;
                    AU.AU_SADATE = DateTime.Now.Date;
                    AU.AU_SAUSER = U;
                    AU.UDT = DateTime.Now;
                    AU.UDU = U;
                }

                //get Master Id
                AU.AU_ID = Sequence.GetSeq(12).ToString();

                //Check Accessories unpost or not
                var GetAcceList = db.A_ASET_ACCE.Where(a => a.ASET_ID == SN.F_ASET_ID).ToList();
                if (GetAcceList.Count != 0)
                {
                    bool IsUnpostAcce = GetAcceList.Where(i => i.ACT == false).Any();
                    if (IsUnpostAcce)
                    {
                        TempData["msg"] = "swal('Opps..','Some accesoreis not approved','warning');";
                        return RedirectToAction("NewAssetUser");
                    }
                    ////get Accesories
                    //string ID = Sequence.GetSeq(13).ToString();
                    //foreach (var item in GetAcceList)
                    //{
                    //    A_ASET_UA AUA = new A_ASET_UA();
                    //    AUA.AUA_ID = ID;
                    //    AUA.AUA_AU = AU.AU_ID;
                    //    AUA.AUA_ACCE = item.ACCE_ID;
                    //    AUA.AUA_QTY = item.AAI_QUANTITY;
                    //    AUA.AUA_RATE = item.AAI_RATE;
                    //    AUA.AUA_TYPE = "T";
                    //    AUA.ACT = true;
                    //    AUA.CDT = DateTime.Now;
                    //    AUA.CDU = U;
                    //    AUA.VAR = 1;
                    //    db.A_ASET_UA.Add(AUA);
                    //    ID = (Convert.ToInt32(ID) + 1).ToString();
                    //}
                    ////update Accessories Oid
                    //Sequence.PostSeq(id: 13, val: Convert.ToInt32(ID));
                    byte Counter = 1;
                    foreach (var item in GetAcceList)
                    {
                        A_ASET_USR_ACE AUA = new A_ASET_USR_ACE();
                        AUA.AUA_AU = AU.AU_ID;
                        AUA.AUA_LINE = Counter;
                        AUA.AUA_ACCE_ID = item.ACCE_ID;
                        AUA.AUA_QTY = item.AAI_QUANTITY;
                        AUA.AUA_RATE = item.AAI_RATE;
                        AUA.AUA_TYPE = "T";
                        AUA.ACT = true;
                        AUA.CDT = DateTime.Now;
                        AUA.CDU = U;
                        AUA.VAR = 1;
                        db.A_ASET_USR_ACE.Add(AUA);
                        Counter = Convert.ToByte(Counter + 1);
                    }
                }
                db.A_ASET_USER.Add(AU);
                bool Used = Sequence.SetAssetUsedUnused(AssetId: SN.F_ASET_ID, VAL: true);
                bool User = Sequence.SetOwnerBookUnbook(OwnrId: SN.U_ASET_USER, VAL: true);
                if (Used && User)
                {
                    db.SaveChanges();
                    IES.Send_Email_KSA_Asset_HandOver(Request_Type: "H", Transfer_Id: AU.AU_ID); //send Email
                    TempData["msg"] = MSG;
                    return RedirectToAction("Index");
                }
            }
            TempData["msg"] = "swal('Ohh..','Transfer failed','error');";
            return RedirectToAction("NewAssetUser");
        }
        [KAuth(301)]
        public ActionResult getUserCurrentData(string id)
        {
            string MSG = "";
            var CurrentList = db.A_ASET_USER.Where(u => u.AU_USER == id && u.ACT == true).ToList();
            foreach (var item in CurrentList)
            {
                string ACN = db.A_ASET_DATA.Find(item.AU_ASET).ASET_CODE_NAME;
                MSG = MSG + " @" + ACN;
            }
            return Json(new { InfoData = MSG }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getAssetOrUserInfo(string id, string selct)
        {
            string MSG = "";
            string AssetSql = string.Format(@"select   (a.aset_code_name || ' / ' || b.business_name || ' / ' || d.department_name || ' / ' || g.group_name) aset_data
            from a_aset_data a 
            join a_group g on a.aset_group=g.group_id
            join a_department d on a.aset_department=d.department_id
            join a_business b on d.department_business=b.business_id
            where a.aset_code_name like '%{0}'", id);

            string UserSql = string.Format(@"select (b.business_name || ' / ' || d.department_name || ' / ' || a.area_name || ' / ' || o.ownr_text || ' / ' || o.ownr_user) user_data
            from a_ownr o
            join a_department d on o.ownr_dept=d.department_id
            join a_business b on d.department_business=b.business_id
            join  a_area a on o.ownr_area=a.area_id
            where o.ownr_text='{0}'", id);

            if (selct == "vasset")
            {
                var A_Data = db.Database.SqlQuery<string>(AssetSql).ToList();
                if (A_Data != null)
                {
                    foreach (var item in A_Data)
                    {
                        MSG = MSG + "Asset Info: " + item + " \n ";
                    }
                }
                else
                {
                    MSG = "Asset information not found";
                }
            }
            else if (selct == "vstaff")
            {
                var A_Data = db.Database.SqlQuery<string>(UserSql).ToList();
                if (A_Data != null)
                {
                    MSG = "User Info: " + A_Data.FirstOrDefault();
                }
                else
                {
                    MSG = "User information not found";
                }
            }
            return Json(new { InfoData = MSG }, JsonRequestBehavior.AllowGet);
        }

        [KAuth(304)]
        public ActionResult AssetUserAssignQuick()
        {
            return View();
        }

        [KAuth(304)]
        [HttpPost]
        public ActionResult AssetUserAssignQuick(AssetUserAssignQuick auaq)
        {
            if (ModelState.IsValid)
            {
                string AssetId = string.Empty;
                AssetUserAssignQuick_Selection us = new AssetUserAssignQuick_Selection();//user selected
                string U = Session["UserId"].ToString();
                string Sql = string.Format(@"select a.aset_id 
from pm.vw_aset_by_users a
where a.act=1 and a.aset_used=0 and a.aset_code_name='{0}' and a.ug_user='{1}'", auaq.AU_ASET, U);
                List<string> Assets = db.Database.SqlQuery<string>(sql: Sql).ToList();
                if (Assets.Count() <= 0)
                {
                    TempData["msg"] = "swal('Error..','Asset not found or not permitted to access','error');";
                    return View(auaq);
                }
                AssetId = Assets.FirstOrDefault().ToString();
                Sql = string.Format(@"select o.ownr_id,o.ownr_dept,o.ownr_area
  from pm.vw_ownr_by_users o
  where o.ownr_type='Staff' and o.ownr_resign='N' and o.act='1' and o.ownr_text='{0}' and o.ub_user='{1}'", auaq.AU_USER, U);
                List<AssetUserAssignQuick_Selection> Users = db.Database.SqlQuery<AssetUserAssignQuick_Selection>(sql: Sql).ToList();
                if (Users.Count() <= 0)
                {
                    TempData["msg"] = "swal('Error..','User not found or not permitted to access','error');";
                    return View(auaq);
                }
                us = Users.FirstOrDefault();
                //string TrnsId = Sequence.GetSeq(12).ToString();
                //Sql = string.Format(@"insert into a_aset_user(au_id,au_aset,au_user,au_dept,au_area,au_srdate,au_sruser,au_snote,au_smailage,au_trnstype,act,cdt,cdu,var) values({0},'{1}','{2}','{3}','{4}',to_date('{5}','DD/MM/YYYY HH:MI:SS AM'),'{6}','{7}','0','G',1,to_date('{8}','DD/MM/YYYY HH:MI:SS AM'),'{9}','1')",
                //    TrnsId, AssetId, us.ownr_id, us.ownr_dept, us.ownr_area, auaq.AU_SRDATE, U, auaq.AU_ENOTE, DateTime.Now, U);

                A_ASET_USER AU = new A_ASET_USER();
                AU.AU_ASET = AssetId;
                AU.AU_USER = us.ownr_id;
                AU.AU_DEPT = us.ownr_dept;
                AU.AU_AREA = us.ownr_area;
                AU.AU_SRDATE = auaq.AU_SRDATE.Date;
                AU.AU_SRUSER = U;
                AU.AU_SNOTE = auaq.AU_ENOTE;
                AU.AU_SMAILAGE = "0";
                AU.AU_TRNSTYPE = "G";
                AU.ACT = true;
                AU.CDT = DateTime.Now;
                AU.CDU = U;
                AU.VAR = 1;

                bool Used = Sequence.SetAssetUsedUnused(AssetId: AssetId, VAL: true);
                bool User = Sequence.SetOwnerBookUnbook(OwnrId: us.ownr_id, VAL: true);
                if (Used && User)
                {

                    //get Master Id
                    AU.AU_ID = Sequence.GetSeq(12).ToString();

                    //Check Accessories unpost or not
                    var GetAcceList = db.A_ASET_ACCE.Where(a => a.ASET_ID == AssetId).ToList();
                    if (GetAcceList.Count != 0)
                    {
                        //    bool IsUnpostAcce = GetAcceList.Where(i => i.ACT == false).Any();
                        //    if (IsUnpostAcce)
                        //    {
                        //        TempData["msg"] = "swal('Opps..','Some accesoreis not approved','warning');";
                        //        return RedirectToAction("NewAssetUser");
                        //    }
                        byte Counter = 1;
                        foreach (var item in GetAcceList)
                        {
                            A_ASET_USR_ACE AUA = new A_ASET_USR_ACE();
                            AUA.AUA_AU = AU.AU_ID;
                            AUA.AUA_LINE = Counter;
                            AUA.AUA_ACCE_ID = item.ACCE_ID;
                            AUA.AUA_QTY = item.AAI_QUANTITY;
                            AUA.AUA_RATE = item.AAI_RATE;
                            AUA.AUA_TYPE = "T";
                            AUA.ACT = true;
                            AUA.CDT = DateTime.Now;
                            AUA.CDU = U;
                            AUA.VAR = 1;
                            db.A_ASET_USR_ACE.Add(AUA);
                            Counter = Convert.ToByte(Counter + 1);
                        }
                    }
                    db.A_ASET_USER.Add(AU);
                    db.SaveChanges();
                    IES.Send_Email_KSA_Asset_HandOver(Request_Type: "H", Transfer_Id: AU.AU_ID); //send Email
                    TempData["msg"] = "swal('Success..','Transfer started successfully, Waiting for Approval','success');";
                    return RedirectToAction("AssetUserAssignQuick");
                }
            }
            return View(auaq);
        }


        [KAuth(301)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            A_ASET_USER a_ASET_USER = db.A_ASET_USER.Find(id);
            if (a_ASET_USER == null)
            {
                return HttpNotFound();
            }
            A_ASET_USER AU = db.A_ASET_USER.Find(id);
            string Aset = AU.AU_ASET;
            string User = AU.AU_USER;
            db.A_ASET_USER.Remove(AU);
            var AUA = db.A_ASET_USR_ACE.Where(a => a.AUA_AU == id).ToList(); //Changes Old Table to New Table Accesoreis
            db.A_ASET_USR_ACE.RemoveRange(AUA); //delete all accelist associated by transfer id
            var IMGS = db.A_ASET_USER_IMG.Where(i => i.IMG_AU == id).ToList();// delete images
            db.A_ASET_USER_IMG.RemoveRange(IMGS);
            db.SaveChanges();
            Sequence.SetAssetUsedUnused(Aset, false);
            TempData["msg"] = "swal('Success..','Transfer deleted','success');";
            return RedirectToAction("Index");
        }

        public ActionResult ShowAccessoreis(string id)
        {
            var AcceList = from a in db.A_ASET_ACCE
                           where a.ASET_ID == id
                           select new AssetAcceVM
                           {
                               ASET_ID = a.A_ASET_DATA.ASET_CODE_NAME,
                               ASET_ID_V = a.ASET_ID,
                               ACCE_ID = a.A_ACCE_ITEMS.ACCE_NAME,
                               ACCE_ID_V = a.ACCE_ID,
                               AAI_DESC = a.AAI_DESC,
                               AAI_QUANTITY = a.AAI_QUANTITY ?? 0,
                               AAI_RATE = a.AAI_RATE ?? 0,
                               Amount = a.AAI_QUANTITY * a.AAI_RATE,
                               ACT = a.ACT
                           };
            return PartialView("_ShowAccessoreis", AcceList.ToList());
        }

        [HttpGet]
        public ActionResult UploadFile(string id)
        {
            ViewBag.TrnsId = id;
            ViewBag.CountFiles = db.A_ASET_USER_IMG.Where(a => a.IMG_AU == id).Count();
            return PartialView();
        }

        [HttpPost]
        public ActionResult UploadFile()
        {
            string ID = Request["uploadername"];
            var CountFiles = db.A_ASET_USER_IMG.Where(a => a.IMG_AU == ID).Count();

            var Upload_Path = Sequence.GetImgPath(101);//get file path
            HttpFileCollectionBase files = Request.Files;
            if (files.Count > 2)
            {
                TempData["msg"] = "swal('Ohh..','You can't upload more then 2 files','warning');";
                return RedirectToAction("Index");
            }
            if ((CountFiles + files.Count) > 3)
            {
                TempData["msg"] = "swal('Ohh..','You can't upload more then 3 files','warning');";
                return RedirectToAction("Index");
            }
            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFileBase file = files[i];

                int fileSize = file.ContentLength;
                if (fileSize <= 0 || fileSize > 2100000)
                {
                    TempData["msg"] = string.Format("swal('Opps..','{0} File uploaded, but current File is too larger, Max file size 2MB','warning');", i);
                    return RedirectToAction("Index");
                }
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
                A_ASET_USER_IMG I = new A_ASET_USER_IMG();
                I.IMG_ID = Guid.NewGuid().ToString();
                I.IMG_NAME = fname;
                I.IMG_PATH = Upload_Path;//get file path
                I.IMG_AU = ID;
                db.A_ASET_USER_IMG.Add(I);
                db.SaveChanges();
                //upload file to server
                var path = System.IO.Path.Combine(Server.MapPath(I.IMG_PATH), I.IMG_ID);
                file.SaveAs(path);
            }
            return Json(new { fc = files.Count }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewFiles(string id)
        {
            var Files = db.A_ASET_USER_IMG.Where(f => f.IMG_AU == id).ToList();
            return PartialView("_ViewFiles", Files);
        }

        public ActionResult Download(string id)
        {
            var dbDATA = db.A_ASET_USER_IMG.Where(a => a.IMG_ID == id);

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
        [KAuth(301)]
        [HttpGet]
        public ActionResult ReturnAsset(int id)
        {
            ViewBag.TrnsId = id;
            return PartialView();
        }
        [KAuth(301)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReturnAsset(EndNew EN)
        {
            string U = Session["UserId"].ToString();
            if (ModelState.IsValid)
            {
                string MSG = "swal('Success..','Retrun request generated, Wait for Approval','success');";
                bool Used = false;
                A_ASET_USER AU = db.A_ASET_USER.Find(EN.AU_ID);
                if (AU == null)
                {
                    TempData["msg"] = "swal('Ohh..','Data not found','error');";
                    return RedirectToAction("Index");
                }
                //no need for all bike also KSA disabled
                //if (Convert.ToInt32(AU.AU_SMAILAGE) >= Convert.ToInt32(EN.AU_EMAILAGE)) //compare with user input
                //{
                //    TempData["msg"] = "swal('Ohh..','Start Mileage can not larger than End Mileage','error');";
                //    return RedirectToAction("Index");
                //}

                AU.AU_ERDATE = EN.AU_ERDATE;// DateTime.Now.Date;
                AU.AU_ERUSER = U;
                AU.AU_EMAILAGE = EN.AU_EMAILAGE ?? "0";
                AU.AU_ENOTE = EN.AU_ENOTE ?? "Return Asset";
                AU.UDT = DateTime.Now;
                AU.UDU = U;
                AU.VAR = AU.VAR + 1;
                db.Entry(AU).State = EntityState.Modified;
                if (AU.AU_TRNSTYPE == "S")
                {
                    MSG = "swal('Success..','Retrun request generated, with Auto Approval','success');";
                    if (!string.IsNullOrEmpty(EN.AU_ENOTE) && EN.AU_ENOTE.Length >= 37)
                        AU.AU_ENOTE = "ShortPeriod-" + EN.AU_ENOTE.Substring(0, 37);
                    else
                        AU.AU_ENOTE = "ShortPeriod-" + EN.AU_ENOTE;

                    AU.AU_EADATE = DateTime.Now.Date;
                    AU.AU_EAUSER = U;
                    AU.ACT = false;
                    //when return asset it's set as unbooked
                    Used = Sequence.SetAssetUsedUnused(AssetId: AU.AU_ASET, VAL: false);
                    if (Used)
                    {
                        db.SaveChanges();
                        TempData["msg"] = MSG;
                        IES.Send_Email_KSA_Asset_HandOver(Request_Type: "R", Transfer_Id: AU.AU_ID); //send Email
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    db.SaveChanges();
                    TempData["msg"] = MSG;
                    IES.Send_Email_KSA_Asset_HandOver(Request_Type: "R", Transfer_Id: AU.AU_ID); //send Email
                    return RedirectToAction("Index");
                }
            }
            TempData["msg"] = "swal('Ohh..','Unable to return','error');";
            return RedirectToAction("Index");
        }
        [KAuth(302)]
        public ActionResult TransferWaitingList()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            //  List<string> MyAssets = RP.MyAssetListString(UserId: U, UserType: T);
            //var TransferList = (from a in db.A_ASET_USER
            //                    where a.ACT == true
            //                    && MyAssets.Contains(a.AU_ASET)
            //                    && a.AU_SAUSER == null
            //                    || (a.AU_ERUSER != null && a.AU_EAUSER == null)
            //                    from TU in db.A_USERS.Where(x => a.AU_SRUSER == x.USER_TEXT).DefaultIfEmpty()
            //                    from TA in db.A_USERS.Where(x => a.AU_SAUSER == x.USER_TEXT).DefaultIfEmpty()
            //                    from RR in db.A_USERS.Where(x => a.AU_ERUSER == x.USER_TEXT).DefaultIfEmpty()
            //                    from RA in db.A_USERS.Where(x => a.AU_EAUSER == x.USER_TEXT).DefaultIfEmpty()
            //                    select new AssetUserManagerVM
            //                    {
            //                        AU_ID = a.AU_ID,
            //                        AU_ASET = a.A_ASET_DATA.ASET_CODE_NAME,//a.AU_ASET,
            //                        AU_USER = a.A_OWNR.OWNR_USER,// a.AU_USER,
            //                        AU_DEPT = a.A_DEPARTMENT.DEPARTMENT_NAME,//depeartment
            //                        AU_AREA = a.A_AREA.AREA_NAME,//area
            //                        AU_SRDATE = a.AU_SRDATE,
            //                        //AU_SRUSER = u.USER_NAME,
            //                        AU_SRUSER = TU.USER_NAME,
            //                        AU_SADATE = a.AU_SADATE,
            //                        //AU_SAUSER = a.AU_SAUSER,
            //                        AU_SAUSER = TA.USER_NAME,
            //                        AU_SNOTE = a.AU_SNOTE,
            //                        AU_ERDATE = a.AU_ERDATE,
            //                        //AU_ERUSER = a.AU_ERUSER,
            //                        AU_ERUSER = RR.USER_NAME,
            //                        AU_EADATE = a.AU_EADATE,
            //                        //AU_EAUSER = a.AU_EAUSER,
            //                        AU_EAUSER = RA.USER_NAME,
            //                        AU_ENOTE = a.AU_ENOTE
            //                    }).ToList();

            var TransferList = db.VW_ASSETUSERMANAGERVM.Where(x => x.UG_USER == U).Where(u => u.T_APPROVER == null || (u.R_USER != null && u.R_APPROVER == null)).ToList();
            ViewBag.Message = "Waiting For Transfer Approval";
            //var R = TransferList.Where(u => u.UG_USER == U).ToList();
            //var RR = R.Where(u => u.T_APPROVER == null || (u.R_USER != null && u.R_APPROVER == null)).ToList();
            //&& u.T_APPROVER == null || (u.R_USER != null && u.R_APPROVER == null)).ToList();

            //var demo = db.VW_ASSETUSERMANAGERVM.Where(x => x.UG_USER == U).Where(u => u.T_APPROVER == null || (u.R_USER != null && u.R_APPROVER == null)).ToList();
            return View(TransferList);
        }
        [KAuth(302)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TransferApprove(string id, string TT)
        {
            string U = Session["UserId"].ToString();
            string Msg = "swal('Opps..','Not Approved','error');";
            A_ASET_USER AU = db.A_ASET_USER.Find(id);
            bool Used = false;
            if (AU == null)
            {
                TempData["msg"] = "swal('Ohh..','Data not found','error');";
                return View();
            }
            if (TT == "H")
            {
                AU.AU_SADATE = DateTime.Now.Date;
                AU.AU_SAUSER = U;
                AU.UDT = DateTime.Now;
                AU.UDU = U;
                AU.VAR = AU.VAR + 1;
                db.Entry(AU).State = EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Transfer Approved','success');";
                return RedirectToAction("TransferWaitingList");
            }
            else if (TT == "R")
            {
                AU.AU_EADATE = DateTime.Now.Date;
                AU.AU_EAUSER = U;
                AU.ACT = false;
                Msg = "swal('Success..','Return Not Approved','success');";
                //when return asset it's set as unbooked
                Used = Sequence.SetAssetUsedUnused(AssetId: AU.AU_ASET, VAL: false);
                AU.UDT = DateTime.Now;
                AU.UDU = U;
                AU.VAR = AU.VAR + 1;
                db.Entry(AU).State = EntityState.Modified;
                if (Used)
                {
                    db.SaveChanges();
                    Msg = "swal('Success..','Return Approved','success');";
                }
                TempData["msg"] = Msg;
                return RedirectToAction("TransferWaitingList");
            }
            else
            {
                TempData["msg"] = Msg;
                return RedirectToAction("TransferWaitingList");
            }

        }

        public ActionResult TransferFormA(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HandOverFormPrint HFP = new HandOverFormPrint();
            var gd = RP.TransferInfo(id);

            HFP.MILEAGE = gd[0];
            HFP.H_USER = gd[1];
            HFP.HO_DATE = Convert.ToDateTime(gd[2]);
            HFP.NOTES = gd[3];
            HFP.O_DEPARTMENT = gd[4];
            HFP.O_BUSINESS = gd[5];
            HFP.O_COMP = gd[6];
            HFP.O_AREA = gd[7];

            HFP.ASET_TAG = gd[8];
            HFP.ASET_NAME = gd[9];
            HFP.A_DEPARTMENT = gd[10];
            HFP.A_BUSINESS = gd[11];
            HFP.A_COMP = gd[12];
            HFP.A_AREA = gd[13];

            HFP.T_STAFF_ID = gd[14];
            HFP.T_STAFF_NAME = gd[15];
            HFP.T_IQAMA_NO = gd[16];
            HFP.T_DEPARTMENT = gd[17];
            HFP.T_BUSINESS = gd[18];
            HFP.T_COMP = gd[19];
            HFP.T_AREA = gd[20];

            List<A_ASET_USR_ACE> AA = db.A_ASET_USR_ACE.Where(i => i.AUA_AU == id).ToList();
            if (AA.Count > 0)
            {
                var ITEMS = from a in AA
                            join i in db.A_ACCE_ITEMS on a.AUA_ACCE_ID equals i.ACCE_ID
                            orderby a.AUA_ACCE_ID
                            select new
                            {
                                a.AUA_LINE,
                                a.AUA_ACCE_ID,
                                i.ACCE_NAME,
                                a.AUA_QTY,
                                a.AUA_DESC,
                            };
                List<HOFPItems> LI = new List<HOFPItems>();
                foreach (var item in ITEMS)
                {
                    HOFPItems I = new HOFPItems();
                    I.SL_NO = item.AUA_LINE;
                    I.ITEM_ID = item.AUA_ACCE_ID;
                    I.ITEM_NAME = item.ACCE_NAME;
                    I.ITEM_QTY = item.AUA_QTY;
                    I.ITEM_NOTE = item.AUA_DESC;
                    LI.Add(I);
                }
                HFP.HOFP_ITEMS = LI;
            }
            return View(HFP);
        }

        //public ActionResult tm()
        //{
        //    for (int i = 41; i <= 49; i++)
        //    {
        //        IES.Send_Email_KSA_Asset_HandOver(Request_Type: "H", Transfer_Id: i.ToString()); //send Email
        //    }
        //    return Content("OK");
        //}

        //private string insertToNewTableAcceItems()
        //{
        //    var Old = db.A_ASET_USER.ToList();
        //    int  A=0;
        //    int T = 0;
        //    foreach (var item in Old)
        //    {
        //        var ITEMS = db.A_ASET_UA.Where(i => i.AUA_AU == item.AU_ID).ToList();
        //        byte c = 1;
        //        foreach(var itm in ITEMS)
        //        {
        //            A_ASET_USR_ACE AUA = new A_ASET_USR_ACE();
        //            AUA.AUA_AU = item.AU_ID;
        //            AUA.AUA_LINE = c;
        //            AUA.AUA_ACCE_ID = itm.AUA_ACCE;
        //            AUA.AUA_DESC = itm.AUA_DESC;
        //            AUA.AUA_QTY = itm.AUA_QTY;
        //            AUA.AUA_RATE = itm.AUA_RATE;
        //            AUA.AUA_TYPE = itm.AUA_TYPE;
        //            AUA.ACT = itm.ACT;
        //            AUA.CDT = itm.CDT;
        //            AUA.CDU = itm.CDU;
        //            AUA.UDT = itm.UDT;
        //            AUA.UDU = itm.UDU;
        //            AUA.VAR = itm.VAR;
        //            db.A_ASET_USR_ACE.Add(AUA);
        //            c = Convert.ToByte(c + 1);
        //            A = A + 1;
        //        }
        //        db.SaveChanges();
        //        T = T + 1;
        //    }
        //    string R = string.Format("Transfer {0}  and Items {1}", T, A);
        //    return R;
        //}


        [KAuth(303)]
        public ActionResult FridgeOutletList()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            List<AssetUserManagerVM> TransferList = IndexData(User: U, UserType: T).ToList();
            //  var TransferList = db.VW_ASSETUSERMANAGERVM.Where(u => u.UG_USER == U).ToList();
            ViewBag.Message = "Transfer List";
            return View(TransferList);
        }

        [KAuth(303)]
        public ActionResult FridgeOutletExcel()
        {
            var model = new ExcelInputVM();
            return View(model);
        }
        [KAuth(303)]
        [HttpPost]
        public ActionResult FridgeOutletExcel(ExcelInputVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["msg"] = "swal('Ohh..','Select a valid excel file','error');";
                return View(model);
            }
            const string MME = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string excelExtention = string.Empty;
            excelExtention = System.IO.Path.GetExtension(model.MyExcelFile.FileName).ToLower();
            string FileType = model.MyExcelFile.ContentType;
            if (excelExtention != ".xlsx" || FileType != MME)
            {
                TempData["msg"] = "swal('Ohh..','Select a valid excel file','error');";
                return View(model);
            }
            DataTable dt = EDL.FridgeOutletExcel(model.MyExcelFile.InputStream, false);
            string strContent = EDL.ConvertDataTableToHTMLTable(dt);
            model.MSExcelTable = strContent;
            TempData["msg"] = "swal('Success..','Excel fill loaded for checking','success');";
            return View(model);
        }
        [KAuth(303)]
        public ActionResult SaveFridgeOutletExcel(FridgeOutletExcel[] ExlData)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            string result = "Excel data is not complete!";
            bool IsValidExcelData = true; //start condition as failed
            if (ModelState.IsValid)
            {
                //Check Excel Data Validation
                foreach (var item in ExlData)
                {
                    if (IsValidExcelData)
                    {
                        string ASET_ID = Sequence.Asset_Id_By_Fridge_Code(item.FRIDGE_CODE.Trim());
                        string OWNER_ID = Sequence.Outlet_Id_By_Outlet_Code(item.OUTLET_CODE.Trim());

                        if (item.TRANSFER_TYPE.ToLower() == "give")
                        {
                            List<string> FREE_MyAssetListString = RP.FREE_MyAssetListString(UserId: U, UserType: T);
                            List<string> FREE_Retailers = RP.FREE_Retailers(UserId: U, UserType: T);

                            bool IsValid = FREE_MyAssetListString.Contains(ASET_ID);
                            if (!IsValid)
                            {
                                result = string.Format("{0} :Asset not found or Free!", item.FRIDGE_CODE);
                                IsValidExcelData = false;
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                            bool IsValidOutlet = FREE_Retailers.Contains(OWNER_ID);
                            if (!IsValidOutlet)
                            {
                                result = string.Format("{0} :Outlet not found or not Permitted!", item.OUTLET_CODE);
                                IsValidExcelData = false;
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else if (item.TRANSFER_TYPE.ToLower() == "take")
                        {
                            List<string> MyAssetList = RP.MyAssetListString(UserId: U, UserType: T);
                            if (MyAssetList.Contains(ASET_ID))
                            {
                                bool IsValid = db.A_ASET_USER.Where(u => u.AU_ASET == ASET_ID && string.IsNullOrEmpty(u.AU_ERUSER) && u.ACT == true).Any();
                                if (!IsValid)
                                {
                                    result = string.Format("{0} :Asset/User not found or booked!", item.FRIDGE_CODE);
                                    IsValidExcelData = false;
                                    return Json(result, JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                        else
                        {
                            result = string.Format("{0} :Excel data is not complete!", item.FRIDGE_CODE);
                            IsValidExcelData = false;
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                }

                int Give = 0, Take = 0, Err = 0;
                foreach (var item in ExlData)
                {
                    AsetId_DepartmentId_AreaId y = Sequence.ADA_Fridge_Code(item.FRIDGE_CODE.Trim());
                    string OWNER_ID = Sequence.Outlet_Id_By_Outlet_Code(item.OUTLET_CODE.Trim());
                    if (y != null || OWNER_ID != "")
                    {
                        if (item.TRANSFER_TYPE.ToLower() == "give")
                        {
                            A_ASET_USER AU = new A_ASET_USER();
                            AU.AU_ASET = y.ASET_ID;
                            AU.AU_USER = OWNER_ID;
                            AU.AU_DEPT = y.DEPARTMENT_ID;
                            AU.AU_AREA = y.AREA_ID;
                            AU.AU_SRDATE = item.TRANSFER_DATE;
                            AU.AU_SRUSER = U;
                            AU.AU_SNOTE = item.NOTE;
                            AU.AU_TRNSTYPE = "G";
                            AU.ACT = true;
                            AU.CDT = DateTime.Now;
                            AU.CDU = U;
                            AU.VAR = 1;
                            AU.AU_SADATE = DateTime.Now.Date;
                            AU.AU_SAUSER = U;
                            AU.AU_ID = Sequence.GetSeq(12).ToString();
                            db.A_ASET_USER.Add(AU);
                            bool Used = Sequence.SetAssetUsedUnused(AssetId: y.ASET_ID, VAL: true);
                            bool User = Sequence.SetOwnerBookUnbook(OwnrId: OWNER_ID, VAL: true);
                            db.SaveChanges();
                            Give = Give + 1;
                        }
                        else
                        {
                            A_ASET_USER AU = db.A_ASET_USER.Where(x => x.AU_ASET == y.ASET_ID && x.AU_USER == OWNER_ID && x.ACT == true).FirstOrDefault();
                            if (AU != null)
                            {
                                AU.AU_ERDATE = item.TRANSFER_DATE;
                                AU.AU_ERUSER = U;
                                AU.AU_ENOTE = item.NOTE;
                                AU.UDT = DateTime.Now;
                                AU.UDU = U;
                                AU.VAR = AU.VAR + 1;
                                AU.AU_EADATE = DateTime.Now.Date;
                                AU.AU_EAUSER = U;
                                AU.ACT = false;
                                db.Entry(AU).State = EntityState.Modified;
                                //when return asset it's set as unbooked
                                Sequence.SetAssetUsedUnused(AssetId: AU.AU_ASET, VAL: false);
                                db.SaveChanges();
                                Take = Take + 1;
                            }
                        }
                    }
                    else
                    {
                        Err = Err + 1;
                    }
                }
                result = string.Format("Give: {0}, Take: {1}, Error: {3}", Give, Take, Err);
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                          .Where(y => y.Count > 0)
                          .ToList();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
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
