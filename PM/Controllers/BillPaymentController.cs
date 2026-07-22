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
    public class BillPaymentController : Controller
    {
        private PMEntities db = new PMEntities();
        private GE ge = new GE();
        private ReturnPermission RP = new ReturnPermission();
        [KAuth(405)]
        public ActionResult Index(string BillNumber, string Operation)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            #region OLD
            //List<string> ASET = RP.MyAssetListString(UserId: U, UserType: T);
            //var BP = (from b in db.A_BILL
            //          where ASET.Contains(b.A_JOBH.JOBH_ASET)
            //          select new BillPayment
            //          {
            //              BILH_ID = b.BILH_ID,
            //              BILH_DATE = b.BILH_DATE,
            //              BILH_NOTE = b.BILH_NOTE,
            //              BILH_REF = b.BILH_REF,
            //              JOBH_ID = b.JOBH_ID,
            //              BILH_RECEIVER = b.BILH_RECEIVER,
            //              BILH_PAID = b.BILH_PAID,
            //              BILH_IPAY = b.BILH_IPAY ?? 0,
            //              BILH_UPAY = b.BILH_UPAY ?? 0,
            //              BILH_CPAY = b.BILH_CPAY
            //          }).OrderByDescending(b => b.BILH_DATE); 
            #endregion

            string sql_1 = @"SELECT 
                            BILL.BILH_ID,BILL.BILH_DATE,BILL.BILH_NOTE,BILL.BILH_REF,BILL.JOBH_ID,
                            BILL.BILH_RECEIVER,BILL.BILH_PAID,NVL(BILL.BILH_IPAY,0) BILH_IPAY,
                            NVL(BILL.BILH_UPAY , 0) BILH_UPAY,NVL(BILL.BILH_CPAY,0) BILH_CPAY
                            FROM 
                            PM.A_BILL BILL
                            INNER JOIN PM.A_JOBH JOBS ON BILL.JOBH_ID = JOBS.JOBH_ID AND JOBS.ACT = 1
                            inner join PM.A_ASET_DATA T6 ON T6.ASET_ID=JOBS.JOBH_ASET AND T6.ACT=1
                            INNER JOIN PM.A_USERS_GROUP T5 ON T6.ASET_GROUP=T5.UG_GROUP AND T5.UG_USER='" + U + @"' AND T5.ACT=1
                            INNER JOIN PM.A_DEPARTMENT T7 ON T6.ASET_DEPARTMENT=T7.DEPARTMENT_ID AND T7.ACT=1
                            INNER JOIN PM.A_BUSINESS T8 ON T7.DEPARTMENT_BUSINESS=T8.BUSINESS_ID AND T8.ACT=1
                            INNER JOIN PM.A_USERS_BUS T9 ON T8.BUSINESS_ID =T9.UB_BUSINESS AND T9.UB_USER=T5.UG_USER AND T9.ACT=1 
                            INNER JOIN (SELECT DISTINCT(AREA_ID) FROM (SELECT T.AREA_ID
                            FROM PM.A_AREA T
                            INNER JOIN PM.A_THANA T1 ON T.AREA_THANA=T1.THANA_ID AND T1.ACT=1
                            INNER JOIN PM.A_DISTRICT T2 ON T1.THANA_DISTRICT=T2.DISTRICT_ID AND T2.ACT=1
                            INNER JOIN PM.A_USERS_COUNTRY T3 ON T2.DISTRICT_COUNTRY=T3.COUNTRY_ID AND T3.ACT=1 AND T3.USER_TEXT='" + U + @"'
                            WHERE T.ACT=1
                            UNION ALL
                            SELECT T.UA_AREA
                            FROM PM.A_USERS_AREA T WHERE T.ACT=1 AND T.UA_USER='" + U + @"'
                            )) T10 ON T6.ASET_AREA=T10.AREA_ID
                            ORDER BY BILL.BILH_DATE DESC";
            var BP = db.Database.SqlQuery<BillPayment>(sql: sql_1).ToList();


            if (!string.IsNullOrEmpty(BillNumber))
            {
                return View(BP.Where(x => x.BILH_ID == BillNumber).ToList());
            }
            else
            {
                return View(BP.Take(10).ToList());
            }
        }
        [KAuth(405)]
        [HttpGet]
        public ActionResult NewPayment()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            List<string> ASET = RP.MyAssetListString(UserId: U, UserType: T);
            ViewBag.BILH_ASET = new SelectList(RP.AsetByListId(AssetId: ASET), "ASET_ID", "ASET_NAME_TAG");
            return View();
        }
        [KAuth(405)]
        public JsonResult getJD(string id)
        {
            decimal CPAY = 0, IPAY = 0, UPAY = 0, TPAY = 0;
            if (id != null)
            {
                var JD = db.A_JOBH.Where(j => j.JOBH_ID == id).ToList();
                if (JD.Count > 0)
                {
                    CPAY = JD.First().JOBH_CPAY ?? 0;
                    IPAY = JD.First().JOBH_IPAY ?? 0;
                    UPAY = JD.First().JOBH_UPAY ?? 0;
                    TPAY = CPAY - (IPAY + UPAY);
                }
                return Json(new { CPAY, IPAY, UPAY, TPAY }, JsonRequestBehavior.AllowGet);

            }
            return Json(new { CPAY, IPAY, UPAY, TPAY }, JsonRequestBehavior.AllowGet);
        }
        [KAuth(405)]
        public List<decimal> getJDList(string id)
        {
            List<decimal> X = new List<decimal>();
            decimal CPAY = 0, IPAY = 0, UPAY = 0;
            if (id != null)
            {
                var JD = db.A_JOBH.Where(j => j.JOBH_ID == id).ToList();
                if (JD.Count > 0)
                {
                    CPAY = JD.First().JOBH_CPAY ?? 0;
                    IPAY = JD.First().JOBH_IPAY ?? 0;
                    UPAY = JD.First().JOBH_UPAY ?? 0;
                    X.Add(UPAY);
                    X.Add(IPAY);
                    X.Add(CPAY);
                }
                return X;
            }
            return X;
        }
        [KAuth(405)]
        public JsonResult VData(string id)
        {
            string VDATA = "No Data Found";
            if (id == null || id == "")
            {
                return Json(new { VDATA }, JsonRequestBehavior.AllowGet);
            }
            var JOBH = db.A_JOBH.Find(id);
            VDATA = RP.InspectionAndJobData(JOBH.JOBH_DEPARTMENT, JOBH.JOBH_AREA, JOBH.JOBH_OWNR);
            return Json(new { VDATA }, JsonRequestBehavior.AllowGet);
        }
        [KAuth(405)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewPayment(NewBillPayment NBP)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            DateTime IDATE = Convert.ToDateTime(NBP.BILH_DATE);
            if (ModelState.IsValid)
            {
                A_BILL B = new A_BILL();
                string GetSeq = NBP.BILH_ASET;
                string id = ge.FifteenDigitId("BILL", GetSeq, dt: IDATE);
                B.BILH_ID = id;// Need Id
                B.BILH_DATE = NBP.BILH_DATE;
                B.BILH_NOTE = NBP.BILH_NOTE;
                B.BILH_REF = NBP.BILH_REF;
                B.JOBH_ID = NBP.JOBH_ID;
                B.BILH_RECEIVER = NBP.BILH_RECEIVER;
                B.BILH_PAID = "Y";
                List<decimal> result = getJDList(NBP.JOBH_ID);
                B.BILH_UPAY = result[0];
                B.BILH_IPAY = result[1];
                B.BILH_CPAY = result[2];
                B.BILH_DISC = 0;
                B.ACT = true;
                B.CDT = DateTime.Now;
                B.CDU = U;
                B.UDT = DateTime.Now;
                B.VAR = 1;
                db.A_BILL.Add(B);
                A_JOBH JH = db.A_JOBH.Find(NBP.JOBH_ID);
                JH.JOBH_BILL = "Y";
                JH.UDT = DateTime.Now;
                JH.UDU = U;
                JH.VAR = JH.VAR + 1;
                db.Entry(JH).State = EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Bill is Paid','success');";
                return RedirectToAction("Index");
                //??save data to user deduction  table
            }
            TempData["msg"] = "swal('Opps..','Bill was not Paid','warning');";
            return View();
        }
        [KAuth(405)]
        public ActionResult BillPrintNow(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var BP = db.A_BILL.Find(id);
            var USERS = db.A_USERS.ToList();//.Where(u => u.ACT == true).ToList();
            if (BP != null)
            {
                BillPrint b = new BillPrint();
                b.BILH_ID = BP.BILH_ID;
                b.ASET_CODE_NAME = BP.A_JOBH.A_INSPH.A_ASET_DATA.ASET_CODE_NAME;
                b.BILH_MILEAGE = BP.A_JOBH.JOBH_MILEAGE;
                b.BILL_USER = USERS.Where(u => u.USER_TEXT == BP.A_JOBH.CDU).First().USER_NAME;
                b.ASET_USER = db.A_OWNR.Find(BP.A_JOBH.JOBH_OWNR).OWNR_USER;
                b.INSP_DATE = BP.A_JOBH.A_INSPH.INSPH_DATE;
                b.JOB_DATE = BP.A_JOBH.JOBH_DATE;
                b.INSP_BY = BP.A_JOBH.A_INSPH.INSPH_IUSER;
                b.INSP_NOTE = BP.A_JOBH.A_INSPH.INSPH_NOTE;
                b.JOB_VENDOR = BP.A_JOBH.JOBH_VENDOR;
                b.JOB_NOTE = BP.A_JOBH.JOBH_NOTE;
                b.INSPH_ID = BP.A_JOBH.A_INSPH.INSPH_ID;
                b.JOBH_ID = BP.A_JOBH.JOBH_ID;
                b.INSP_APP = USERS.Where(u => u.USER_TEXT == BP.A_JOBH.A_INSPH.INSPH_APBY).First().USER_NAME;
                b.JOBH_APP = USERS.Where(u => u.USER_TEXT == BP.A_JOBH.JOBH_APBY).First().USER_NAME;
                b.PAID_BY = USERS.Where(u => u.USER_TEXT == BP.CDU).First().USER_NAME;
                b.USER_PAY = BP.BILH_UPAY;
                b.INSU_PAY = BP.BILH_IPAY;
                b.BILH_CPAY = BP.BILH_CPAY;
                b.REF_NO = BP.BILH_REF;
                b.BILL_DATE = BP.BILH_DATE;
                b.BILLH_NOTE = BP.BILH_NOTE;
                List<BillPrintINS> L_I = new List<BillPrintINS>();
                List<BillPrintJOB> L_J = new List<BillPrintJOB>();
                var INSP_ITEMS = db.A_INSPD.Where(i => i.INSPH_ID == BP.A_JOBH.INSPH_ID).ToList();
                b.iTotal = INSP_ITEMS.Sum(s => s.INSPD_IRATE * s.INSPD_IQTY).Value;
                foreach (var item in INSP_ITEMS)
                {
                    BillPrintINS II = new BillPrintINS();
                    II.iITEM_NAME = item.A_ITEM_LIST.ITEM_NAME;
                    II.iQty = item.INSPD_IQTY;
                    II.iRate = item.INSPD_IRATE;
                    II.iAmount = item.INSPD_IQTY * item.INSPD_IRATE;
                    L_I.Add(II);
                }
                var JOBD_ITEMS = db.A_JOBD.Where(j => j.JOBH_ID == BP.JOBH_ID).ToList();
                b.jTotal = JOBD_ITEMS.Sum(s => s.JOBD_JQTY * s.JOBD_JRATE).Value;
                foreach (var item in JOBD_ITEMS)
                {
                    BillPrintJOB II = new BillPrintJOB();
                    II.jITEM_NAME = item.A_ITEM_LIST.ITEM_NAME;
                    II.jQty = item.JOBD_JQTY;
                    II.jRate = item.JOBD_JRATE;
                    II.jAmount = item.JOBD_JQTY * item.JOBD_JRATE;
                    L_J.Add(II);
                }
                b.INSP_ITEM = L_I;
                b.JOBH_ITEM = L_J;
                return View(b);
            }
            return View();
        }

        [KAuth(405)]
        public JsonResult BillStory(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                TempData["msg"] = "swal('Opps..','please enter staff id','info');";
                return Json(new { data = "" });
            }
            string Sql = string.Format(@"select b.bilh_id,b.bilh_date, b.bilh_note, b.bilh_ref,b.jobh_id, b.bilh_receiver,
                b.bilh_paid,nvl(b.bilh_ipay,0)bilh_ipay,nvl( b.bilh_upay,0)bilh_upay,nvl( b.bilh_cpay,0)bilh_cpay
                from a_bill b
                join a_jobh j on b.jobh_id=j.jobh_id
                where j.jobh_aset='{0}'
                and ROWNUM <= '10' order by bilh_date desc", id);
            var _data = db.Database.SqlQuery<BillPayment>(Sql).ToList();
            return Json(new { data = _data }, JsonRequestBehavior.AllowGet);
        }
        #region Bundle_Bill_Payment
        [KAuth(405)]
        public ActionResult BundlePayment()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            InspectionController IC = new InspectionController();
            List<BundleInspectionApp> BJ = IC.BJA(APP: "Y", BILL: "N", User: U, Type: T);
            return View(BJ);
        }
        [KAuth(405)]
        [HttpPost]
        public ActionResult BundlePayment(BundleBillAuto BBN)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (ModelState.IsValid)
            {
                DateTime IDATE = DateTime.Now.Date;

                List<BundleJobcardAuto> INSPH_ASET = (from x in db.VW_BNDL_JOBC.Where(s => s.JOBD_NOTE == BBN.BUNDLE_ID && s.JOBH_APP == "Y" && s.JOBH_BILL == "N")
                                                      select new BundleJobcardAuto
                                                      {
                                                          INSP_ID = x.JOBH_ID, //INSP_ID is Job Card Number
                                                          ASET_ID = x.JOBH_ASET,
                                                          JOB_DATE = x.JOBH_DATE

                                                      }
                                      ).ToList();
                //?? Inspection take Inspection Id and Job Card take Asset Id
                foreach (var item in INSPH_ASET)
                {
                    string isExist = ge.FifteenDigitId("BILL", item.ASET_ID, dt: IDATE);
                    A_BILL AB = db.A_BILL.Find(isExist);
                    if (AB != null)
                    {
                        TempData["msg"] = string.Format("swal('Ohh..','{0} Bill already paid today' ,'info');", isExist);
                        return RedirectToAction("BundlePayment");
                    }
                }
                foreach (var item in INSPH_ASET)
                {
                    A_BILL B = new A_BILL();
                    string GetSeq = item.ASET_ID;
                    string id = ge.FifteenDigitId("BILL", GetSeq, dt: IDATE);
                    B.BILH_ID = id;// Need Id
                    //B.BILH_DATE = IDATE;
                    B.BILH_DATE = item.JOB_DATE; 
                    B.BILH_NOTE = BBN.BILL_NOTE;
                    B.BILH_REF = BBN.BUNDLE_ID;
                    B.JOBH_ID = item.INSP_ID; //INSP_ID is Job Card number
                    B.BILH_RECEIVER = BBN.RECEIVER_NAME;
                    B.BILH_PAID = "Y";
                    List<decimal> result = getJDList(item.INSP_ID);//INSP_ID is Job Card number
                    B.BILH_UPAY = result[0];
                    B.BILH_IPAY = result[1];
                    B.BILH_CPAY = result[2];
                    B.BILH_DISC = 0;
                    B.ACT = true;
                    B.CDT = DateTime.Now;
                    B.CDU = U;
                    B.UDT = DateTime.Now;
                    B.VAR = 1;
                    db.A_BILL.Add(B);
                    A_JOBH JH = db.A_JOBH.Find(item.INSP_ID);//INSP_ID is Job Card number
                    JH.JOBH_BILL = "Y";
                    JH.UDT = DateTime.Now;
                    JH.UDU = U;
                    JH.VAR = JH.VAR + 1;
                    db.Entry(JH).State = EntityState.Modified;
                }
                db.SaveChanges();
                TempData["msg"] = string.Format("swal('Success..','{0} Bills are Paid','success');", INSPH_ASET.Count());
                return RedirectToAction("Index");
                //??save data to user deduction  table
            }
            TempData["msg"] = "swal('Opps..','Bill was not Paid','warning');";
            return View();
        }
        #endregion

    }
}