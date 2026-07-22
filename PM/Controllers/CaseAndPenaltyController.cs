using Newtonsoft.Json;
using PM.BLL;
using PM.Model;
using PM.ModelsV;
using PM.MyAuth;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace PM.Controllers
{
    [Authorize]
    [CustomHttpReffer]
    public class CaseAndPenaltyController : Controller
    {
        private PMEntities db = new PMEntities();
        private ReturnPermission RP = new ReturnPermission();
        private GE GE = new GE();
        private JsonReaderPMController PMC = new JsonReaderPMController();
        private InstantEmailSender IES = new InstantEmailSender();
        public List<CasePenlatyView> CPV(List<string> ASET, string CaseNo, int T)
        {
            var AP = (from p in db.A_PLEA
                      from u in db.A_USERS.Where(x => p.PLEA_APP == x.USER_TEXT).DefaultIfEmpty()
                      where p.ACT == true
                      && ASET.Contains(p.PLEA_ASET)
                      select new CasePenlatyView
                      {
                          PLEA_ID = p.PLEA_ID,
                          PLEA_DFND = p.PLEA_DFND,
                          PLEA_PAID_BY = p.PLEA_PAID_BY,
                          PLEA_DATE = p.PLEA_DATE,
                          PLEA_REF = p.PLEA_REF,
                          PLEA_REASON = p.A_PLEA_REASON.REASON_NAME,
                          PLEA_DESC = p.PLEA_DESCRIPTION,
                          PLEA_AMNT = p.PLEA_AMNT,
                          PLEA_LOCATION = p.PLEA_LOCATION,
                          PLEA_OWNR = p.A_OWNR.OWNR_USER,
                          PLEA_MILEAGE = p.PLEA_MILEAGE,
                          PLEA_ASET = p.A_ASET_DATA.ASET_CODE_NAME,
                          PLEA_DEPT = p.A_DEPARTMENT.DEPARTMENT_NAME,
                          PLEA_AREA = p.A_AREA.AREA_NAME,
                          PLEA_STATUS = p.PLEA_STATUS,
                          PLEA_APP = u.USER_NAME,//p.PLEA_APP,
                          PLEA_APP_DATE = p.PLEA_APP_DATE
                      }).OrderByDescending(p => p.PLEA_DATE);

            //string sql_1 = @"SELECT A.ASET_ID,A.ASET_TAG,A.ASET_CODE_NAME,AREA.AREA_NAME ASET_AREA,BOD.BODY_NAME ASET_BODY,BRAND.BRAND_NAME ASET_BRAND,
            //                FUEL.FUEL_NAME ASET_FUEL,CLO.COLOR_NAME ASET_COLOR,GRP.GROUP_NAME ASET_GROUP,A.ASET_MODEL,A.ASET_PO_ENGINE,A.ASET_GRN_CHASIS,
            //                A.ASET_PRICE,A.ASET_CAPACITY,A.ASET_PRICE,A.ASET_REG_DATE,A.ASET_HEIGHT,A.ASET_WEIGHT,A.ASET_MFG_YEAR,A.ASET_IMAGE,A.ASET_NOTE,A.ASET_USED,
            //                A.ACT,DEP.DEPARTMENT_NAME DEPARTMENT_V,BUSS.BUSINESS_NAME BUSINESS_V,COMP.COMPANY_NAME COMPANY_V
            //                FROM A_ASET_DATA A 
            //                INNER JOIN PM.A_DEPARTMENT D ON A.ASET_DEPARTMENT = D.DEPARTMENT_ID AND D.ACT = 1 
            //                INNER JOIN PM.A_BUSINESS B ON D.DEPARTMENT_BUSINESS = B.BUSINESS_ID AND B.ACT = 1 
            //                INNER JOIN  PM.A_USERS_BUS P ON B.BUSINESS_ID = P.UB_BUSINESS AND P.ACT = 1 AND P.UB_USER = '" + U + @"'
            //                INNER JOIN  PM.A_USERS_GROUP R ON A.ASET_GROUP = R.UG_GROUP AND R.UG_USER = '" + U + @"'
            //                INNER JOIN PM.A_GROUP G ON R.UG_GROUP = G.GROUP_ID  AND R.ACT = 1
            //                INNER JOIN PM.A_AREA ARR ON ARR.AREA_ID = A.ASET_AREA AND ARR.ACT = 1
            //                INNER JOIN PM.A_USERS_COUNTRY C ON  C.ACT =1 AND  C.USER_TEXT = '" + U + @"'
            //                INNER JOIN PM.A_COUNTRY CO ON CO.COUNTRY_ID = C.COUNTRY_ID AND CO.ACT = 1
            //                INNER JOIN PM.A_DISTRICT DC ON DC.DISTRICT_COUNTRY = CO.COUNTRY_ID AND DC.ACT = 1
            //                INNER JOIN PM.A_THANA TH ON TH.THANA_DISTRICT = DC.DISTRICT_ID AND ARR.AREA_THANA = TH.THANA_ID  AND TH.ACT = 1
            //                INNER JOIN PM.A_AREA AREA ON AREA.AREA_ID = A.ASET_AREA
            //                INNER JOIN PM.A_BODY BOD ON A.ASET_BODY = BOD.BODY_ID
            //                INNER JOIN PM.A_BRAND BRAND ON A.ASET_BRAND = BRAND.BRAND_ID
            //                INNER JOIN PM.A_FUEL FUEL ON  A.ASET_FUEL = FUEL.FUEL_ID
            //                INNER JOIN PM.A_COLOR CLO ON A.ASET_COLOR = CLO.COLOR_ID
            //                INNER JOIN PM.A_GROUP GRP ON A.ASET_GROUP = GRP.GROUP_ID
            //                INNER JOIN PM.A_DEPARTMENT DEP ON A.ASET_DEPARTMENT = DEP.DEPARTMENT_ID
            //                INNER JOIN PM.A_BUSINESS BUSS ON DEP.DEPARTMENT_BUSINESS = BUSS.BUSINESS_ID
            //                INNER JOIN PM.A_COMPANY COMP ON BUSS.BUSINESS_COMPANY = COMP.COMPANY_ID
            //                WHERE  A.ACT = 1";

            //var AP = db.Database.SqlQuery<CasePenlatyView>(sql: sql_1);

            if (!string.IsNullOrWhiteSpace(CaseNo))
            {
                return AP.Where(x => x.PLEA_REF == CaseNo).ToList();
            }
            else
            {
                return AP.Take(T).ToList();
            }
        }
        public List<CasePenlatyView> CASE(String U, string CaseNo, int T)
        {


            //string sql_1 = @"SELECT                      
            //                P.PLEA_ID,P.PLEA_DFND,P.PLEA_PAID_BY,P.PLEA_DATE,P.PLEA_REF,RSN.REASON_NAME PLEA_REASON,P.PLEA_DESCRIPTION,
            //                P.PLEA_AMNT,P.PLEA_LOCATION,OWNER.OWNR_USER PLEA_OWNR,P.PLEA_MILEAGE,ASS.ASET_CODE_NAME PLEA_ASET,DPT.DEPARTMENT_NAME PLEA_DEPT,
            //                AREA.AREA_NAME PLEA_AREA,P.PLEA_STATUS,X.USER_NAME PLEA_APP,P.PLEA_APP_DATE
            //                FROM
            //                PM.A_PLEA P
            //                INNER JOIN PM.A_ASET_DATA T6 ON P.PLEA_ASET = T6.ASET_ID AND T6.ACT = 1
            //                INNER JOIN PM.A_USERS X ON P.PLEA_APP = X.USER_TEXT
            //                INNER JOIN A_USERS_GROUP T5 ON T6.ASET_GROUP=T5.UG_GROUP AND T5.UG_USER='"+ U + @"' AND T5.ACT=1
            //                INNER JOIN A_DEPARTMENT T7 ON T6.ASET_DEPARTMENT=T7.DEPARTMENT_ID AND T7.ACT=1
            //                INNER JOIN A_BUSINESS T8 ON T7.DEPARTMENT_BUSINESS=T8.BUSINESS_ID AND T8.ACT=1
            //                INNER JOIN A_USERS_BUS T9 ON T8.BUSINESS_ID =T9.UB_BUSINESS AND T9.UB_USER=T5.UG_USER AND T9.ACT=1 
            //                INNER JOIN (SELECT DISTINCT(AREA_ID) FROM (SELECT T.AREA_ID
            //                FROM A_AREA T
            //                INNER JOIN A_THANA T1 ON T.AREA_THANA=T1.THANA_ID AND T1.ACT=1
            //                INNER JOIN A_DISTRICT T2 ON T1.THANA_DISTRICT=T2.DISTRICT_ID AND T2.ACT=1
            //                INNER JOIN A_USERS_COUNTRY T3 ON T2.DISTRICT_COUNTRY=T3.COUNTRY_ID AND T3.ACT=1 AND T3.USER_TEXT='" + U + @"'
            //                WHERE T.ACT=1
            //                UNION ALL
            //                SELECT T.UA_AREA
            //                FROM A_USERS_AREA T WHERE T.ACT=1 AND T.UA_USER='" + U + @"'
            //                )) T10 ON T6.ASET_AREA=T10.AREA_ID
            //                INNER JOIN PM.A_PLEA_REASON RSN ON RSN.REASON_ID = P.PLEA_REASON_ID AND RSN.ACT = 1
            //                INNER JOIN PM.A_OWNR OWNER ON OWNER.OWNR_ID = P.PLEA_OWNR  AND OWNER.ACT = 1
            //                INNER JOIN PM.A_ASET_DATA ASS ON ASS.ASET_ID = P.PLEA_ASET AND ASS.ACT = 1
            //                INNER JOIN PM.A_DEPARTMENT DPT ON DPT.DEPARTMENT_ID = P.PLEA_DEPT AND DPT.ACT = 1
            //                INNER JOIN PM.A_AREA AREA ON AREA.AREA_ID = P.PLEA_AREA  AND AREA.ACT = 1
            //                WHERE  P.ACT = 1
            //                ORDER BY P.PLEA_DATE DESC";

            string sql_1 = @"SELECT                      
                            P.PLEA_ID,P.PLEA_DFND,P.PLEA_PAID_BY,P.PLEA_DATE,P.PLEA_REF,RSN.REASON_NAME PLEA_REASON,P.PLEA_DESCRIPTION,
                            P.PLEA_AMNT,P.PLEA_LOCATION,OWNER.OWNR_USER PLEA_OWNR,P.PLEA_MILEAGE,ASS.ASET_CODE_NAME PLEA_ASET,DPT.DEPARTMENT_NAME PLEA_DEPT,
                            AREA.AREA_NAME PLEA_AREA,P.PLEA_STATUS,
                            p.PLEA_APP,
                            P.PLEA_APP_DATE
                            FROM
                            PM.A_PLEA P
                            INNER JOIN PM.A_ASET_DATA T6 ON P.PLEA_ASET = T6.ASET_ID AND T6.ACT = 1
                           
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
                            )) T10 ON T6.ASET_AREA=T10.AREA_ID
                            INNER JOIN PM.A_PLEA_REASON RSN ON RSN.REASON_ID = P.PLEA_REASON_ID AND RSN.ACT = 1
                            INNER JOIN PM.A_OWNR OWNER ON OWNER.OWNR_ID = P.PLEA_OWNR  AND OWNER.ACT = 1
                            INNER JOIN PM.A_ASET_DATA ASS ON ASS.ASET_ID = P.PLEA_ASET AND ASS.ACT = 1
                            INNER JOIN PM.A_DEPARTMENT DPT ON DPT.DEPARTMENT_ID = P.PLEA_DEPT AND DPT.ACT = 1
                            INNER JOIN PM.A_AREA AREA ON AREA.AREA_ID = P.PLEA_AREA  AND AREA.ACT = 1
                            WHERE  P.ACT = 1
                            ORDER BY P.PLEA_DATE DESC";

            var AP = db.Database.SqlQuery<CasePenlatyView>(sql: sql_1);

            if (!string.IsNullOrWhiteSpace(CaseNo))
            {
                return AP.Where(x => x.PLEA_REF == CaseNo).ToList();
            }
            else
            {
                return AP.Take(T).ToList();
            }
        }
        [Authorize]
        public ActionResult Index(string CaseNo, string Operation)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            //List<string> ASET = RP.MyAssetListString(UserId: U, UserType: T);
            //var AP = CPV(ASET: ASET, CaseNo: CaseNo, T: 10);
            var AP = CASE(U, CaseNo: CaseNo, T: 500);
            return View(AP);
        }


        #region Case_Entry_601
        [HttpGet]
        [KAuth(601)]
        public ActionResult NewCase()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            List<string> ASET = RP.MyAssetListString(UserId: U, UserType: T);
            ViewBag.PLEA_ASET = new SelectList(RP.AsetByListId(AssetId: ASET), "ASET_ID", "ASET_NAME_TAG");
            List<SelectListItem> DFND = new List<SelectListItem>();
            DFND.Add(new SelectListItem { Text = "Plate Number", Value = "ASSET", Selected = true });
            DFND.Add(new SelectListItem { Text = "Iquma Number", Value = "USER" });
            ViewBag.PLEA_DFND = DFND;
            List<SelectListItem> PPB = new List<SelectListItem>();
            PPB.Add(new SelectListItem { Text = "User Pay", Value = "USER", Selected = true });
            PPB.Add(new SelectListItem { Text = "Compnay Pay", Value = "COMPANY" });
            ViewBag.PLEA_PAID_BY = PPB;
            ViewBag.PLEA_REASON_ID = new SelectList(db.A_PLEA_REASON.Where(r => r.ACT == true), "REASON_ID", "REASON_NAME");
            return View();
        }
        [HttpGet]
        [Authorize]
        public JsonResult getDetail(string id, DateTime pdt)
        {
            string MSG = "No Data Found!";
            string DEPT = "";
            string AREA = "";
            string USER = "";
            A_ASET_USER AU = db.A_ASET_USER.Where(u => u.AU_ASET == id && (pdt > u.AU_SRDATE && pdt < u.AU_ERDATE)).FirstOrDefault();
            if (AU == null)
            {
                A_ASET_USER AUL = db.A_ASET_USER.Where(u => u.AU_ASET == id && u.ACT == true).FirstOrDefault();
                if (AUL == null)
                {
                    return Json(new { success = false, MSG = MSG }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    DEPT = AUL.AU_DEPT;
                    AREA = AUL.AU_AREA;
                    USER = AUL.AU_USER;
                }
            }
            else
            {
                DEPT = AU.AU_DEPT;
                AREA = AU.AU_AREA;
                USER = AU.AU_USER;
            }
            A_DEPARTMENT AD = db.A_DEPARTMENT.Find(DEPT);
            if (AD == null)
            {
                MSG = "No Department Found!";
                return Json(new { success = false, MSG = MSG }, JsonRequestBehavior.AllowGet);
            }
            string DEPT_NAME = AD.DEPARTMENT_NAME;
            string BU_NAME = AD.A_BUSINESS.BUSINESS_NAME;
            A_AREA AA = db.A_AREA.Find(AREA);
            if (AA == null)
            {
                MSG = "No Area Found!";
                return Json(new { success = false, MSG = MSG }, JsonRequestBehavior.AllowGet);
            }
            string AREA_NAME = AA.AREA_NAME;
            A_OWNR AO = db.A_OWNR.Find(USER);
            if (AO == null)
            {
                MSG = "No User Found!";
                return Json(new { success = false, MSG = MSG }, JsonRequestBehavior.AllowGet);
            }
            string USER_NAME = AO.OWNR_USER;
            string IQUMA_NO = AO.OWNR_NAME;
            MSG = string.Format("User:{0},Iquma:{1},Area:{2},Department:{3},Business:{4}",
                USER_NAME, IQUMA_NO, AREA_NAME, DEPT_NAME, BU_NAME);
            return Json(new { success = true, MSG = MSG }, JsonRequestBehavior.AllowGet);
        }
        [KAuth(601)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewCase(CaseAndPenalty CP)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (ModelState.IsValid)
            {
                bool isExist = db.A_PLEA.Where(x => x.PLEA_REF.ToLower() == CP.PLEA_REF.ToLower()).Any();
                if (isExist)
                {
                    TempData["msg"] = "swal('Opps..','error in violence number','warning');";
                    return RedirectToAction("Index");
                }
                JsonResult JR = PMC.CaseAndPenaltyUserData(CP.PLEA_ASET, pdt: CP.PLEA_DATE);
                string json = JsonConvert.SerializeObject(JR.Data);
                JRdata dto = JsonConvert.DeserializeObject<JRdata>(json);
                if (dto.success)
                {
                    A_PLEA P = new A_PLEA();
                    var id = GE.TwentyDigitId("PLEA", CP.PLEA_ASET, CP.PLEA_DATE);
                    P.PLEA_ID = id;
                    P.PLEA_DFND = CP.PLEA_DFND;
                    P.PLEA_PAID_BY = CP.PLEA_PAID_BY;
                    P.PLEA_DATE = CP.PLEA_DATE;
                    P.PLEA_REF = CP.PLEA_REF;
                    P.PLEA_DESCRIPTION = CP.PLEA_DESC;
                    P.PLEA_AMNT = CP.PLEA_AMNT;
                    P.PLEA_LOCATION = CP.PLEA_LOCATION;
                    P.PLEA_OWNR = dto.USER_NAME; //User from User Assgined
                    P.PLEA_MILEAGE = CP.PLEA_MILEAGE;
                    P.PLEA_ASET = CP.PLEA_ASET;
                    P.PLEA_DEPT = dto.DEPT_NAME; //Department from User Assgined
                    P.PLEA_AREA = dto.AREA_NAME;//Area from User Assgined
                    P.PLEA_REASON_ID = CP.PLEA_REASON_ID; //reason id
                    P.PLEA_STATUS = "N";
                    P.ACT = true;
                    P.CDU = U;
                    P.CDT = DateTime.Now;
                    P.VAR = 1;
                    db.A_PLEA.Add(P);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception Ex)
                    {
                        TempData["msg"] = string.Format("swal('Ohh..','{0}','error');", Ex.Message);
                        return RedirectToAction("Index");
                    }
                    A_PLEA NP = db.A_PLEA.Find(id);
                    string pleaId = id;
                    string userId = db.A_OWNR.Find(NP.PLEA_OWNR).OWNR_USER;
                    int Amnt = NP.PLEA_AMNT ?? 0;
                    string Rsn = db.A_PLEA_REASON.Find(NP.PLEA_REASON_ID).REASON_NAME;
                    string Ast = db.A_ASET_DATA.Find(NP.PLEA_ASET).ASET_CODE_NAME;
                    IES.Send_Email_KSA_Plea_Approval(Plea: pleaId, User: userId, Amount: Amnt, Reason: Rsn, Asset: Ast);
                    TempData["msg"] = "swal('Ohh..','Case entry done without eMail','info');";
                    return RedirectToAction("Index");
                }
            }
            List<string> ASET = RP.MyAssetListString(UserId: U, UserType: T);
            ViewBag.PLEA_ASET = new SelectList(RP.AsetByListId(AssetId: ASET), "ASET_ID", "ASET_NAME_TAG");
            List<SelectListItem> DFND = new List<SelectListItem>();
            DFND.Add(new SelectListItem { Text = "Plate Number", Value = "ASSET", Selected = true });
            DFND.Add(new SelectListItem { Text = "Iquma Number", Value = "USER" });
            ViewBag.PLEA_DFND = DFND;
            List<SelectListItem> PPB = new List<SelectListItem>();
            PPB.Add(new SelectListItem { Text = "User Pay", Value = "USER", Selected = true });
            PPB.Add(new SelectListItem { Text = "Compnay Pay", Value = "COMPANY" });
            ViewBag.PLEA_PAID_BY = PPB;
            ViewBag.PLEA_REASON_ID = new SelectList(db.A_PLEA_REASON.Where(r => r.ACT == true), "REASON_ID", "REASON_NAME");
            TempData["msg"] = "swal('Opps..','Case entry not done','warning');";
            return View();
        }
        #endregion  

        #region Approval_602
        [KAuth(602)]
        [HttpGet]
        public ActionResult PleaApproval()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            //List<string> ASET = RP.MyAssetListString(UserId: U, UserType: T);
            //var AP = CPV(ASET: ASET, CaseNo: "", T: 50).Where(p => p.PLEA_STATUS == "N" && (p.PLEA_APP == null || p.PLEA_APP == ""));
            //var AP = CASE(U, CaseNo: "", T: 50).Where(p => p.PLEA_STATUS == "N" && (p.PLEA_APP == null || p.PLEA_APP == ""));
            var AP = CASE(U, CaseNo: "", T: 500).Where(p => p.PLEA_STATUS == "N" && (p.PLEA_APP == null || p.PLEA_APP == ""));
            return View(AP);
        }
        [KAuth(602)]
        [HttpPost]
        public ActionResult PleaApproval(string id)
        {
            string U = Session["UserId"].ToString();
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("PleaApproval");
            }
            A_PLEA AP = db.A_PLEA.Where(p => p.PLEA_STATUS == "N" && (p.PLEA_APP == null || p.PLEA_APP == "") && p.PLEA_ID == id && p.ACT == true).FirstOrDefault();
            if (AP == null)
            {
                return HttpNotFound();
            }
            AP.PLEA_APP = U;
            AP.PLEA_APP_DATE = DateTime.Now.Date;
            AP.UDU = U;
            AP.UDT = DateTime.Now;
            AP.VAR = AP.VAR + 1;
            db.Entry(AP).State = EntityState.Modified;
            db.SaveChanges();
            TempData["msg"] = "swal('Success..','Case Approved','success');";
            return Redirect(Request.UrlReferrer.ToString());
        }
        #endregion

        #region Accounts_603

        public ActionResult PleaUpdate()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            //List<string> ASET = RP.MyAssetListString(UserId: U, UserType: T);
            //var AP = CPV(ASET: ASET, CaseNo: "", T: 50).Where(p => p.PLEA_STATUS == "N" && !string.IsNullOrEmpty(p.PLEA_APP));
            var AP = CASE(U, CaseNo: "", T: 500).Where(p => p.PLEA_STATUS == "N" && !string.IsNullOrEmpty(p.PLEA_APP));
            return View(AP);
        }

        [KAuth(603)]
        [HttpGet]
        public ActionResult UpdatePlea(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            A_PLEA AP = db.A_PLEA.Where(p => p.PLEA_STATUS == "N" && p.PLEA_ID == id && !string.IsNullOrEmpty(p.PLEA_APP)).FirstOrDefault();
            if (AP == null)
            {
                return HttpNotFound();
            }
            return PartialView(AP);
        }
        [KAuth(603)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePlea(CasePleaUpdate CPU)
        {
            string U = Session["UserId"].ToString();
            if (ModelState.IsValid)
            {
                A_PLEA AP = db.A_PLEA.Where(p => p.PLEA_STATUS == "N" && p.PLEA_ID == CPU.PLEA_ID && !string.IsNullOrEmpty(p.PLEA_APP)).FirstOrDefault();
                if (AP == null)
                {
                    return HttpNotFound();
                }
                AP.PLEA_DESCRIPTION = CPU.PLEA_DESCRIPTION;
                AP.PLEA_STATUS = "Y";
                AP.UDU = U;
                AP.UDT = DateTime.Now;
                AP.VAR = AP.VAR + 1;
                db.Entry(AP).State = EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Case data updated','success');";
                return Redirect(Request.UrlReferrer.ToString());
            }
            return RedirectToAction("Index");
        }
        #endregion
    }
}