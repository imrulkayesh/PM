using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Oracle.ManagedDataAccess.Client;
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
using System.Web.Mvc;

namespace PM.Controllers
{
    [Authorize]
    [CustomHttpReffer]
    public class SummaryController : Controller
    {
        private PMEntities db = new PMEntities();
        GE Sequence = new GE();
        [KAuth(12)]
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Task_Status_Data()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            var X = db.A_SUMMARY_VIEW.Where(x => x.SV_ID == "fc3c869e-1041-42fd-8f6d-af664b601a2a").FirstOrDefault();
            List<object> chartData = new List<object>();
            chartData.Add(new object[] { "Name", "Data" });
            chartData.Add(new object[] { X.DATA_AN, (double)X.DATA_A });
            chartData.Add(new object[] { X.DATA_BN, (double)X.DATA_B });
            chartData.Add(new object[] { X.DATA_CN, (double)X.DATA_C });
            chartData.Add(new object[] { X.DATA_DN, (double)X.DATA_D });
            chartData.Add(new object[] { X.DATA_EN, (double)X.DATA_E });
            return Json(chartData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Report_Status_Data()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            var X = db.A_SUMMARY_VIEW.Where(x => x.SV_ID == "7b1b97cc-5f0c-4309-9024-78a8ce9554c5").FirstOrDefault();
            List<object> chartData = new List<object>();
            chartData.Add(new object[] { "Name", "Data" });
            chartData.Add(new object[] { X.DATA_AN, (double)X.DATA_A });
            chartData.Add(new object[] { X.DATA_BN, (double)X.DATA_B });
            return Json(chartData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Insp_Job_Bill()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            var X = db.A_SUMMARY_VIEW.Where(x => x.SV_ID == "9907f537-a406-465b-a74f-002ba75002a6").FirstOrDefault();
            List<object> chartData = new List<object>();
            chartData.Add(new object[] { "Name", "Data" });
            chartData.Add(new object[] { X.DATA_AN, (double)X.DATA_A });
            //chartData.Add(new object[] { X.DATA_BN, (double)X.DATA_B });
            chartData.Add(new object[] { X.DATA_CN, (double)X.DATA_C });
            chartData.Add(new object[] { X.DATA_DN, (double)X.DATA_D });
            return Json(chartData, JsonRequestBehavior.AllowGet);
        }





        #region KSA_Admin_Dashboard
        [KAuth(11)]
        public ActionResult KSA_ADMIN_DASHBOARD()
        {
            string TopItemSQL = @"SELECT ITEM_ID,CNT,I.ITEM_NAME
                        FROM   (SELECT   INSPD_ITEM , COUNT(INSPD_ITEM) AS CNT
                        FROM     A_INSPD 
                        GROUP BY INSPD_ITEM
                        ORDER BY 2 DESC)
                        JOIN A_ITEM_LIST I ON INSPD_ITEM=I.ITEM_ID
                        WHERE  ROWNUM <= 10";
            string TopAsetSQL = @"SELECT D.ASET_TAG,CNT,D.ASET_CODE_NAME FROM (
                        SELECT U.AU_ASET,COUNT(U.AU_ASET) CNT FROM A_ASET_USER U
                        JOIN A_ASET_DATA A ON U.AU_ASET=A.ASET_ID 
                        WHERE A.ACT='1' AND A.ASET_GROUP='GR0001' AND A.ASET_ID<>'0'
                        GROUP BY U.AU_ASET
                        ORDER BY 2 DESC)
                        JOIN A_ASET_DATA D ON AU_ASET=ASET_ID
                        WHERE ROWNUM <=10";
            string TopStaff = @"SELECT OWNR_TEXT,CNT,OWNR_USER FROM(
                        SELECT AU_USER,COUNT(AU_USER) CNT FROM A_ASET_USER U
                        JOIN A_OWNR O ON U.AU_USER=O.OWNR_ID 
                        WHERE O.OWNR_TYPE='Staff' AND O.ACT='1'
                        GROUP BY U.AU_USER
                        ORDER BY 2 DESC)
                        JOIN A_OWNR X ON AU_USER=X.OWNR_ID
                        WHERE ROWNUM <=10";

            List<A_SUMMARY_VIEW> KSA = db.A_SUMMARY_VIEW.Where(x => x.SV_USER == "KSA").ToList();
            Update_Date _UD = new Update_Date();
            _UD.CAR_DATE = KSA.Where(x => x.SV_TABLE == "PRO_SUM_FOUR").First().UDT;
            _UD.STAFF_DATE = KSA.Where(x => x.SV_TABLE == "PRO_SUM_FIVE").First().UDT;
            _UD.POLICE_DATE = KSA.Where(x => x.SV_TABLE == "PRO_SUM_SIX").First().UDT;
            _UD.CL_DATE = KSA.Where(x => x.SV_TABLE == "PRO_SUM_SEVEN").First().UDT;
            _UD.CM_DATE = KSA.Where(x => x.SV_TABLE == "PRO_SUM_EIGHT").First().UDT;

            _UD.AWA_DATE = KSA.Where(x => x.SV_ID == "AWC").FirstOrDefault()?.UDT;
            _UD.AWC_DATE = KSA.Where(x => x.SV_ID == "AWJ").FirstOrDefault()?.UDT;
            _UD.IQUMA_DATE = KSA.Where(x => x.SV_ID == "IQUMA").FirstOrDefault()?.UDT;

            var AWC = (from d in KSA
                       where d.SV_ID == "AWC"
                       orderby d.DATA_DN
                       group d by d.DATA_CN into g
                       select new Asset_Body_Area
                       {
                           AREA_NAME = g.Max(x => x.DATA_DN),
                           CNT = g.Sum(x => x.DATA_A)
                       }).ToList();

            var AWJ = (from d in KSA
                       where d.SV_ID == "AWJ"
                       orderby d.DATA_AN
                       select new Area_Wise_Cost
                       {
                           AREA_NAME = d.DATA_AN,
                           INSD = d.DATA_A,
                           JOBD = d.DATA_B,
                           INSD_1 = d.DATA_C,
                           JOBD_1 = d.DATA_D
                       }
                       ).ToList();


            SummaryViewKSA _SummaryViewKSA = new SummaryViewKSA();
            _SummaryViewKSA.topTenItem = TopTenItem(TopItemSQL);
            _SummaryViewKSA.topAsset = TopTenAsset(TopAsetSQL);
            _SummaryViewKSA.topStaff = TopTenStaff(TopStaff);
            _SummaryViewKSA.updateDate = _UD;
            _SummaryViewKSA.bodyArea = AWC;
            _SummaryViewKSA.costArea = AWJ;
            _SummaryViewKSA.expireList = iqumaExpired();

            return View(_SummaryViewKSA);
        }

        //[KAuth(13)]
        //public ActionResult AUDIT_DASHBOARD()
        //{
        //    //string TopItemSQL = @"SELECT ITEM_ID,CNT,I.ITEM_NAME
        //    //            FROM   (SELECT   INSPD_ITEM , COUNT(INSPD_ITEM) AS CNT
        //    //            FROM     A_INSPD 
        //    //            GROUP BY INSPD_ITEM
        //    //            ORDER BY 2 DESC)
        //    //            JOIN A_ITEM_LIST I ON INSPD_ITEM=I.ITEM_ID
        //    //            WHERE  ROWNUM <= 10";
        //    //string TopAsetSQL = @"SELECT D.ASET_TAG,CNT,D.ASET_CODE_NAME FROM (
        //    //            SELECT U.AU_ASET,COUNT(U.AU_ASET) CNT FROM A_ASET_USER U
        //    //            JOIN A_ASET_DATA A ON U.AU_ASET=A.ASET_ID 
        //    //            WHERE A.ACT='1' AND A.ASET_GROUP='GR0001' AND A.ASET_ID<>'0'
        //    //            GROUP BY U.AU_ASET
        //    //            ORDER BY 2 DESC)
        //    //            JOIN A_ASET_DATA D ON AU_ASET=ASET_ID
        //    //            WHERE ROWNUM <=10";
        //    //string TopStaff = @"SELECT OWNR_TEXT,CNT,OWNR_USER FROM(
        //    //            SELECT AU_USER,COUNT(AU_USER) CNT FROM A_ASET_USER U
        //    //            JOIN A_OWNR O ON U.AU_USER=O.OWNR_ID 
        //    //            WHERE O.OWNR_TYPE='Staff' AND O.ACT='1'
        //    //            GROUP BY U.AU_USER
        //    //            ORDER BY 2 DESC)
        //    //            JOIN A_OWNR X ON AU_USER=X.OWNR_ID
        //    //            WHERE ROWNUM <=10";

        //    string AuditorWiseAuditReportCountSQL = @"SELECT  A.AUDITOR_STAFF_ID, A.AUDITOR_NAME, 
        //                 COUNT(R.AUDIT_RPT_ID)                             AS TOTAL_REPORT,
        //                NVL(SUM(R.WORK_SCOPE), 0)                         AS TOTAL_WORK_SCOPE,
        //                NVL(SUM(R.DIRECT_SAVING_AMOUNT), 0)               AS TOTAL_DIRECT_SAVING_AMOUNT,
        //                NVL(SUM(R.INDIRECT_SAVING_AMOUNT), 0)             AS TOTAL_INDIRECT_SAVING_AMOUNT,
        //                NVL(SUM(R.DEMOTION), 0)                           AS TOTAL_DEMOTION,
        //                NVL(SUM(R.DEVELOPMENT), 0)                        AS TOTAL_DEVELOPMENT,
        //                NVL(SUM(R.JOB_SACK_OUT), 0)                       AS TOTAL_JOB_SACK_OUT,
        //                NVL(SUM(R.TRANSFER), 0)                           AS TOTAL_TRANSFER,
        //                NVL(SUM(R.WARNING_LETTER), 0)                     AS TOTAL_WARNING_LETTER,
        //                NVL(SUM(R.PENALTY), 0)                            AS TOTAL_PENALTY,
        //                NVL(SUM(R.HOLD_PAYMENT), 0)                       AS TOTAL_HOLD_PAYMENT,
        //                NVL(SUM(R.SUPPLIER_BLACKLIST), 0)                 AS TOTAL_SUPPLIER_BLACKLIST,
        //                NVL(SUM(R.INEFFICIENCY_IDENTIFIED), 0)            AS TOTAL_INEFFICIENCY_IDENTIFIED,
        //                NVL(SUM(R.CORRUPTION_IDENTIFIED), 0)              AS TOTAL_CORRUPTION_IDENTIFIED
        //                FROM AUDIT_REPORT R 
        //                LEFT JOIN AUDIT_AUDITOR A ON R.CDU = A.AUDITOR_STAFF_ID
        //                WHERE R.ACT = 1
        //                GROUP BY A.AUDITOR_STAFF_ID, A.AUDITOR_NAME
        //                ORDER BY TOTAL_REPORT DESC";

        //    string SegmentConpamyGroupWiseAuditReportCountSQL = @"SELECT 
        //            CG.COMPANY_GROUP_NAME,
        //            TA.TASK_CATEGORY_NAME,
        //            COUNT(R.AUDIT_RPT_ID)                             AS TOTAL_REPORT,
        //            NVL(SUM(R.WORK_SCOPE), 0)                         AS TOTAL_WORK_SCOPE,
        //            NVL(SUM(R.DIRECT_SAVING_AMOUNT), 0)               AS TOTAL_DIRECT_SAVING_AMOUNT,
        //            NVL(SUM(R.INDIRECT_SAVING_AMOUNT), 0)             AS TOTAL_INDIRECT_SAVING_AMOUNT,
        //            NVL(SUM(R.DEMOTION), 0)                           AS TOTAL_DEMOTION,
        //            NVL(SUM(R.DEVELOPMENT), 0)                        AS TOTAL_DEVELOPMENT,
        //            NVL(SUM(R.JOB_SACK_OUT), 0)                       AS TOTAL_JOB_SACK_OUT,
        //            NVL(SUM(R.TRANSFER), 0)                           AS TOTAL_TRANSFER,
        //            NVL(SUM(R.WARNING_LETTER), 0)                     AS TOTAL_WARNING_LETTER,
        //            NVL(SUM(R.PENALTY), 0)                            AS TOTAL_PENALTY,
        //            NVL(SUM(R.HOLD_PAYMENT), 0)                       AS TOTAL_HOLD_PAYMENT,
        //            NVL(SUM(R.SUPPLIER_BLACKLIST), 0)                 AS TOTAL_SUPPLIER_BLACKLIST,
        //            NVL(SUM(R.INEFFICIENCY_IDENTIFIED), 0)            AS TOTAL_INEFFICIENCY_IDENTIFIED,
        //            NVL(SUM(R.CORRUPTION_IDENTIFIED), 0)              AS TOTAL_CORRUPTION_IDENTIFIED
        //        FROM 
        //            AUDIT_REPORT R
        //        LEFT JOIN 
        //            AUDIT_TASK_CATEGORY TA ON R.TASK_CATEGORY_ID = TA.TASK_CATEGORY_ID
        //        LEFT JOIN 
        //            AUDIT_TASK AT ON R.TASK_ID = AT.TASK_ID
        //        INNER JOIN 
        //            C_COMPANY_GROUP CG ON CG.COMPANY_GROUP_ID = AT.COMPANY_GROUP_ID
        //        WHERE 
        //            R.ACT = 1
        //        GROUP BY 
        //            CG.COMPANY_GROUP_NAME,
        //            TA.TASK_CATEGORY_NAME
        //        ORDER BY 
        //            CG.COMPANY_GROUP_NAME, 
        //            TA.TASK_CATEGORY_NAME";

        //    string CompanyNameCompanyGroupWiseAuditReportCountSQL = @"
        //       SELECT 
        //            CG.COMPANY_GROUP_NAME,
        //            C.COMPANY_NAME,
        //            COUNT(R.AUDIT_RPT_ID)                             AS TOTAL_REPORT,
        //            NVL(SUM(R.WORK_SCOPE), 0)                         AS TOTAL_WORK_SCOPE,
        //            NVL(SUM(R.DIRECT_SAVING_AMOUNT), 0)               AS TOTAL_DIRECT_SAVING_AMOUNT,
        //            NVL(SUM(R.INDIRECT_SAVING_AMOUNT), 0)             AS TOTAL_INDIRECT_SAVING_AMOUNT,
        //            NVL(SUM(R.DEMOTION), 0)                           AS TOTAL_DEMOTION,
        //            NVL(SUM(R.DEVELOPMENT), 0)                        AS TOTAL_DEVELOPMENT,
        //            NVL(SUM(R.JOB_SACK_OUT), 0)                       AS TOTAL_JOB_SACK_OUT,
        //            NVL(SUM(R.TRANSFER), 0)                           AS TOTAL_TRANSFER,
        //            NVL(SUM(R.WARNING_LETTER), 0)                     AS TOTAL_WARNING_LETTER,
        //            NVL(SUM(R.PENALTY), 0)                            AS TOTAL_PENALTY,
        //            NVL(SUM(R.HOLD_PAYMENT), 0)                       AS TOTAL_HOLD_PAYMENT,
        //            NVL(SUM(R.SUPPLIER_BLACKLIST), 0)                 AS TOTAL_SUPPLIER_BLACKLIST,
        //            NVL(SUM(R.INEFFICIENCY_IDENTIFIED), 0)            AS TOTAL_INEFFICIENCY_IDENTIFIED,
        //            NVL(SUM(R.CORRUPTION_IDENTIFIED), 0)              AS TOTAL_CORRUPTION_IDENTIFIED
        //        FROM 
        //            AUDIT_REPORT R

        //        LEFT JOIN 
        //            AUDIT_TASK AT ON R.TASK_ID = AT.TASK_ID
        //        INNER JOIN 
        //            C_COMPANY_GROUP CG ON CG.COMPANY_GROUP_ID = AT.COMPANY_GROUP_ID
        //        INNER JOIN 
        //            C_COMPANY C ON C.COMPANY_ID = AT.COMPANY_ID
        //        WHERE 
        //            R.ACT = 1
        //        GROUP BY 
        //            CG.COMPANY_GROUP_NAME,
        //            C.COMPANY_NAME
        //        ORDER BY 
        //            CG.COMPANY_GROUP_NAME, 
        //            C.COMPANY_NAME";

        //    string CompanyGroupWiseAuditReportCountSQL = @"SELECT 
        //                CG.COMPANY_GROUP_NAME,
        //                COUNT(CASE 
        //                          WHEN AT.REPORT_CREATE_STATUS = 1 AND AT.REPORT_STATUS_ID in (1,2,3,4) 
        //                          THEN 1 
        //                     END) AS TOTAL_REPORT,
        //                COUNT(CASE 
        //                          WHEN AT.REPORT_CREATE_STATUS = 1 AND AT.REPORT_STATUS_ID = 1 AND R.REPORT_STATUS_ID=1
        //                          THEN 1 
        //                     END) AS TOTAL_COMPLETED_WITH_REPORT,
        //                COUNT(CASE 
        //                          WHEN AT.REPORT_CREATE_STATUS = 1 AND AT.REPORT_STATUS_ID = 2 AND R.REPORT_STATUS_ID=2
        //                          THEN 1 
        //                     END) AS TOTAL_COMPLETED_WITHOUT_REPORT 
        //                FROM AUDIT_REPORT R
        //                LEFT JOIN AUDIT_TASK AT 
        //                    ON R.TASK_ID = AT.TASK_ID
        //                INNER JOIN C_COMPANY_GROUP CG 
        //                    ON CG.COMPANY_GROUP_ID = AT.COMPANY_GROUP_ID
        //                WHERE R.ACT = 1
        //                GROUP BY CG.COMPANY_GROUP_NAME
        //                ORDER BY TOTAL_REPORT DESC";

        //    List<A_SUMMARY_VIEW> KSA = db.A_SUMMARY_VIEW.Where(x => x.SV_USER == "AUDIT").ToList();
        //    Update_Date _UD = new Update_Date();

        //    var totalTaskCount = db.Database.SqlQuery<int>("SELECT COUNT(TASK_ID) AS TOTAL_TASK FROM AUDIT_TASK WHERE ACT = 1").FirstOrDefault();
        //    _UD.TOTAL_TASK = totalTaskCount;

        //    var totalAuditReportCount = db.Database.SqlQuery<int>("SELECT COUNT(AUDIT_RPT_ID) AS TOTAL_REPORT FROM AUDIT_REPORT WHERE ACT=1").FirstOrDefault();
        //    _UD.TOTAL_AUDIT_REPORT = totalAuditReportCount;


        //    var totalAuditorCount = db.Database.SqlQuery<int>("SELECT COUNT(AUDITOR_ID) AS TOTAL_AUDITOR  from AUDIT_AUDITOR WHERE ACT=1").FirstOrDefault();
        //    _UD.TOTAL_AUDITOR = totalAuditorCount;


        //    _UD.TASK_STATUS_DATE = KSA.Where(x => x.SV_TABLE == "AUDIT_TASK_STATUS_DATA").First().UDT;
        //    _UD.REPORT_STATUS_DATE = KSA.Where(x => x.SV_TABLE == "AUDIT_REPORT_STATUS").First().UDT;
        //    _UD.REPORT_SENT_STATUS_DATE = KSA.Where(x => x.SV_TABLE == "AUDIT_REPORT_SENT_SUM").First().UDT;
        //    _UD.COMPANY_GROUP_DATE = KSA.Where(x => x.SV_TABLE == "COMPAY_GROUP_WISE_REPORT_SUM").First().UDT;
        //    _UD.CURRENT_LAST_MONTH_DATE = KSA.Where(x => x.SV_TABLE == "CURRENT_LAST_MONTH_REPORT_SUM").First().UDT;



        //    //var AWC = (from d in KSA
        //    //           where d.SV_ID == "AWC"
        //    //           orderby d.DATA_DN
        //    //           group d by d.DATA_CN into g
        //    //           select new Asset_Body_Area
        //    //           {
        //    //               AREA_NAME = g.Max(x => x.DATA_DN),
        //    //               CNT = g.Sum(x => x.DATA_A)
        //    //           }).ToList();

        //    //var AWJ = (from d in KSA
        //    //           where d.SV_ID == "AWJ"
        //    //           orderby d.DATA_AN
        //    //           select new Area_Wise_Cost
        //    //           {
        //    //               AREA_NAME = d.DATA_AN,
        //    //               INSD = d.DATA_A,
        //    //               JOBD = d.DATA_B,
        //    //               INSD_1 = d.DATA_C,
        //    //               JOBD_1 = d.DATA_D
        //    //           }
        //    //           ).ToList();


        //    SummaryViewKSA _SummaryViewKSA = new SummaryViewKSA();
        //    _SummaryViewKSA.AuditorWiseAuditReportCount = AuditorWiseAuditReportCountData(AuditorWiseAuditReportCountSQL);
        //    _SummaryViewKSA.SegmentConpamyGroupWiseAuditReportCount = SegmentConpamyGroupWiseAuditReportCountData(SegmentConpamyGroupWiseAuditReportCountSQL);
        //    _SummaryViewKSA.CompanyNameCompanyGroupWiseAuditReportCount = CompanyNameCompanyGroupWiseAuditReportCountData(CompanyNameCompanyGroupWiseAuditReportCountSQL);
        //    _SummaryViewKSA.CompanyGroupWiseAuditReportCount = CompanyGroupWiseAuditReportCountData(CompanyGroupWiseAuditReportCountSQL);



        //    //_SummaryViewKSA.topTenItem = TopTenItem(TopItemSQL);
        //    //_SummaryViewKSA.topAsset = TopTenAsset(TopAsetSQL);
        //    //_SummaryViewKSA.topStaff = TopTenStaff(TopStaff);
        //    _SummaryViewKSA.updateDate = _UD;
        //    //_SummaryViewKSA.bodyArea = AWC;
        //    //_SummaryViewKSA.costArea = AWJ;
        //    //_SummaryViewKSA.expireList = iqumaExpired();

        //    return View(_SummaryViewKSA);
        //}

        [KAuth(13)]
        public ActionResult AUDIT_DASHBOARD(int? year)
        {
            int selectedYear = year ?? DateTime.Now.Year;
            ViewBag.SelectedYear = selectedYear;

            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            AUDIT_LOG AL = new AUDIT_LOG();
            AL.LOG_ID =  Sequence.GetSeq(37).ToString();
            AL.CMNU_TEXT = 13;
            AL.USER_ID = U;
            AL.CDT = DateTime.Now;
            db.AUDIT_LOG.Add(AL);
            db.SaveChanges();

            if (T == "10")
            {

                string AuditorWiseAuditReportCountSQL = @"SELECT  A.AUDITOR_STAFF_ID, A.AUDITOR_NAME, 
                         COUNT(R.AUDIT_RPT_ID)                             AS TOTAL_REPORT,
                         COUNT (
                         CASE
                             WHEN     AT.REPORT_CREATE_STATUS = 1
                                  AND AT.REPORT_STATUS_ID IN (1, 3)
                                  AND R.REPORT_STATUS_ID IN (1, 3)
                             THEN
                                 1
                         END)       AS TOTAL_WITH_REPORT,
                         
                         COUNT (
                          CASE
                              WHEN     AT.REPORT_CREATE_STATUS = 1
                                   AND AT.REPORT_STATUS_ID IN (2, 4)
                                   AND R.REPORT_STATUS_ID IN (2, 4)
                              THEN
                                  1
                          END)       AS TOTAL_WITHOUT_REPORT,
                        ROUND(NVL(SUM(R.WORK_SCOPE), 0) / 10000000, 3)  AS TOTAL_WORK_SCOPE,
                        ROUND(NVL(SUM(R.DIRECT_SAVING_AMOUNT), 0) / 10000000, 3)   AS TOTAL_DIRECT_SAVING_AMOUNT,
                        ROUND(NVL(SUM(R.INDIRECT_SAVING_AMOUNT), 0)  / 10000000, 3)   AS TOTAL_INDIRECT_SAVING_AMOUNT,
                        NVL(SUM(R.DEMOTION), 0)                           AS TOTAL_DEMOTION,
                        NVL(SUM(R.DEVELOPMENT), 0)                        AS TOTAL_DEVELOPMENT,
                        NVL(SUM(R.JOB_SACK_OUT), 0)                       AS TOTAL_JOB_SACK_OUT,
                        NVL(SUM(R.TRANSFER), 0)                           AS TOTAL_TRANSFER,
                        NVL(SUM(R.WARNING_LETTER), 0)                     AS TOTAL_WARNING_LETTER,
                        NVL(SUM(R.PENALTY), 0)                            AS TOTAL_PENALTY,
                        NVL(SUM(R.HOLD_PAYMENT), 0)                       AS TOTAL_HOLD_PAYMENT,
                        NVL(SUM(R.INEFFICIENCY_IDENTIFIED), 0)            AS TOTAL_INEFFICIENCY_IDENTIFIED,
                        NVL(SUM(R.CORRUPTION_IDENTIFIED), 0)              AS TOTAL_CORRUPTION_IDENTIFIED
                        FROM AUDIT_REPORT R 
                        LEFT JOIN AUDIT_AUDITOR A ON R.CDU = A.AUDITOR_STAFF_ID
                        LEFT JOIN AUDIT_TASK AT ON R.TASK_ID = AT.TASK_ID
                        WHERE R.ACT = 1
                        
                        AND EXTRACT(YEAR FROM R.CDT) =  '" + selectedYear + @"'

                        GROUP BY A.AUDITOR_STAFF_ID, A.AUDITOR_NAME
                        ORDER BY TOTAL_REPORT DESC";

                //string ReportSummarySQL = @"SELECT COMPANY_GROUP_ID,COMPANY_GROUP_NAME,TOTAL_NO_OF_REPORT,LAST_MONTH_TOTAL_REPORT,THIS_MONTH_TOTAL_REPORT,
                //                           TOTAL_WITH_REPORT,LAST_MONTH_WITH_REPORT,THIS_MONTH_WITH_REPORT,
                //                           TOTAL_WITHOUT_REPORT,LAST_MONTH_WITHOUT_REPORT,THIS_MONTH_WITHOUT_REPORT
                //                           FROM VW_ADT_COM_GROUP_RPT";

                string ReportSummarySQL = @"SELECT CG.COMPANY_GROUP_ID,
                                         CG.COMPANY_GROUP_NAME,
                                         COUNT (CASE
                                                    WHEN     AT.REPORT_CREATE_STATUS = 1
                                                         AND AT.REPORT_STATUS_ID IN (1,
                                                                                     2,
                                                                                     3,
                                                                                     4)
                                                    THEN
                                                        1
                                                END)    AS TOTAL_NO_OF_REPORT,
                                         COUNT (
                                             CASE
                                                 WHEN     AT.REPORT_CREATE_STATUS = 1
                                                      AND AT.REPORT_STATUS_ID IN (1,
                                                                                  2,
                                                                                  3,
                                                                                  4)
                                                      AND R.CDT >= ADD_MONTHS (TRUNC (SYSDATE, 'MM'), -1)
                                                      AND R.CDT < TRUNC (SYSDATE, 'MM')
                                                 THEN
                                                     1
                                             END)       AS LAST_MONTH_TOTAL_REPORT,
                                         COUNT (CASE
                                                    WHEN     AT.REPORT_CREATE_STATUS = 1
                                                         AND AT.REPORT_STATUS_ID IN (1,
                                                                                     2,
                                                                                     3,
                                                                                     4)
                                                         AND R.CDT >= TRUNC (SYSDATE, 'MM')
                                                         AND R.CDT < ADD_MONTHS (TRUNC (SYSDATE, 'MM'), 1)
                                                    THEN
                                                        1
                                                END)    AS THIS_MONTH_TOTAL_REPORT,
                                         COUNT (
                                             CASE
                                                 WHEN     AT.REPORT_CREATE_STATUS = 1
                                                      AND AT.REPORT_STATUS_ID IN (1, 3)
                                                      AND R.REPORT_STATUS_ID IN (1, 3)
                                                 THEN
                                                     1
                                             END)       AS TOTAL_WITH_REPORT,
                                         COUNT (
                                             CASE
                                                 WHEN     AT.REPORT_CREATE_STATUS = 1
                                                      AND AT.REPORT_STATUS_ID IN (1, 3)
                                                      AND R.REPORT_STATUS_ID IN (1, 3)
                                                      AND R.CDT >= ADD_MONTHS (TRUNC (SYSDATE, 'MM'), -1)
                                                      AND R.CDT < TRUNC (SYSDATE, 'MM')
                                                 THEN
                                                     1
                                             END)       AS LAST_MONTH_WITH_REPORT,
                                         COUNT (
                                             CASE
                                                 WHEN     AT.REPORT_CREATE_STATUS = 1
                                                      AND AT.REPORT_STATUS_ID IN (1, 3)
                                                      AND R.REPORT_STATUS_ID IN (1, 3)
                                                      AND R.CDT >= TRUNC (SYSDATE, 'MM')
                                                      AND R.CDT < ADD_MONTHS (TRUNC (SYSDATE, 'MM'), 1)
                                                 THEN
                                                     1
                                             END)       AS THIS_MONTH_WITH_REPORT,
                                         COUNT (
                                             CASE
                                                 WHEN     AT.REPORT_CREATE_STATUS = 1
                                                      AND AT.REPORT_STATUS_ID IN (2, 4)
                                                      AND R.REPORT_STATUS_ID IN (2, 4)
                                                 THEN
                                                     1
                                             END)       AS TOTAL_WITHOUT_REPORT,
                                         COUNT (
                                             CASE
                                                 WHEN     AT.REPORT_CREATE_STATUS = 1
                                                      AND AT.REPORT_STATUS_ID IN (2, 4)
                                                      AND R.REPORT_STATUS_ID IN (2, 4)
                                                      AND R.CDT >= ADD_MONTHS (TRUNC (SYSDATE, 'MM'), -1)
                                                      AND R.CDT < TRUNC (SYSDATE, 'MM')
                                                 THEN
                                                     1
                                             END)       AS LAST_MONTH_WITHOUT_REPORT,
                                         COUNT (
                                             CASE
                                                 WHEN     AT.REPORT_CREATE_STATUS = 1
                                                      AND AT.REPORT_STATUS_ID IN (2, 4)
                                                      AND R.REPORT_STATUS_ID IN (2, 4)
                                                      AND R.CDT >= TRUNC (SYSDATE, 'MM')
                                                      AND R.CDT < ADD_MONTHS (TRUNC (SYSDATE, 'MM'), 1)
                                                 THEN
                                                     1
                                             END)       AS THIS_MONTH_WITHOUT_REPORT
                                    FROM AUDIT_REPORT R
                                         LEFT JOIN AUDIT_TASK AT ON R.TASK_ID = AT.TASK_ID
                                         INNER JOIN C_COMPANY_GROUP CG
                                             ON CG.COMPANY_GROUP_ID = AT.COMPANY_GROUP_ID
                                   WHERE R.ACT = 1 
                                   AND EXTRACT(YEAR FROM R.CDT) =  '" + selectedYear + @"'
                                GROUP BY CG.COMPANY_GROUP_ID, CG.COMPANY_GROUP_NAME
                                ORDER BY CG.COMPANY_GROUP_ID";


                string CategoryCGWiseTotalReportSQL = @"SELECT *
                                            FROM (
                                                SELECT 
                                                    TA.TASK_CATEGORY_NAME,
                                                    COUNT(R.AUDIT_RPT_ID) AS TOTAL_REPORT_CATEGORY,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '1' THEN 1 ELSE 0 END) AS PRAN,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '2' THEN 1 ELSE 0 END) AS RFL,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '3' THEN 1 ELSE 0 END) AS CTL,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '4' THEN 1 ELSE 0 END) AS PDL,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '5' THEN 1 ELSE 0 END) AS GROUP_CS
                                                FROM 
                                                    AUDIT_REPORT R
                                                LEFT JOIN 
                                                    AUDIT_TASK_CATEGORY TA 
                                                    ON R.TASK_CATEGORY_ID = TA.TASK_CATEGORY_ID
                                                LEFT JOIN 
                                                    AUDIT_TASK AT 
                                                    ON R.TASK_ID = AT.TASK_ID
                                                INNER JOIN 
                                                    C_COMPANY_GROUP CG 
                                                    ON CG.COMPANY_GROUP_ID = AT.COMPANY_GROUP_ID
                                                WHERE 
                                                    R.ACT = 1
                                                    AND EXTRACT(YEAR FROM R.CDT) =  '" + selectedYear + @"'
                                                GROUP BY 
                                                    TA.TASK_CATEGORY_NAME
                                                ORDER BY 
                                                    TOTAL_REPORT_CATEGORY DESC
                                            )WHERE ROWNUM <= 5";

                string CategoryWiseWorkScopeTotalReportSQL = @"SELECT *
                                                                FROM (
                                                                    SELECT 
                                                                        TA.TASK_CATEGORY_NAME,
                                                                        ROUND(NVL(SUM(R.WORK_SCOPE), 0) / 10000000, 2) AS TOTAL_WORK_SCOPE,

                                                                        ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '1' 
                                                                                       THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS PRAN,

                                                                        ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '2' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS RFL,
                                                                        ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '3' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS CTL,
                                                                        ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '4' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS PDL,
                                                                        ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '5' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS GROUP_CS
                                                                    FROM AUDIT_REPORT R
                                                                        LEFT JOIN AUDIT_TASK_CATEGORY TA 
                                                                            ON R.TASK_CATEGORY_ID = TA.TASK_CATEGORY_ID
                                                                        LEFT JOIN AUDIT_TASK AT 
                                                                            ON R.TASK_ID = AT.TASK_ID
                                                                        INNER JOIN C_COMPANY_GROUP CG 
                                                                            ON CG.COMPANY_GROUP_ID = AT.COMPANY_GROUP_ID
                                                                    WHERE R.ACT = 1 AND EXTRACT(YEAR FROM R.CDT) =  '" + selectedYear + @"'
                                                                    GROUP BY TA.TASK_CATEGORY_NAME
                                                                    ORDER BY TOTAL_WORK_SCOPE DESC
                                                                )
                                                                WHERE ROWNUM <= 5";

                string BUCGWiseTotalReportSQL = @"SELECT *
                                            FROM (
                                                SELECT 
                                                    C.COMPANY_NAME,
                                                    COUNT(R.AUDIT_RPT_ID) AS TOTAL_REPORT_BU,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '1' THEN 1 ELSE 0 END) AS PRAN,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '2' THEN 1 ELSE 0 END) AS RFL,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '3' THEN 1 ELSE 0 END) AS CTL,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '4' THEN 1 ELSE 0 END) AS PDL,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '5' THEN 1 ELSE 0 END) AS GROUP_CS
                                                FROM 
                                                    AUDIT_REPORT R
                                                LEFT JOIN 
                                                    AUDIT_TASK AT 
                                                    ON R.TASK_ID = AT.TASK_ID
                                              INNER JOIN 
                                                    C_COMPANY_GROUP CG 
                                                    ON CG.COMPANY_GROUP_ID = AT.COMPANY_GROUP_ID
                                                INNER JOIN 
                                                     C_COMPANY C ON C.COMPANY_ID = AT.COMPANY_ID
                                                WHERE 
                                                    R.ACT = 1 AND EXTRACT(YEAR FROM R.CDT) =  '" + selectedYear + @"'
                                                GROUP BY 
                                                     C.COMPANY_NAME
                                                ORDER BY 
                                                    TOTAL_REPORT_BU DESC
                                            )
                                            WHERE ROWNUM <= 5";

                string BUCGWiseTotalWorkScopeReportSQL = @" SELECT *
                                                    FROM (
                                                        SELECT 
                                                            C.COMPANY_NAME,
                                                            ROUND(NVL(SUM(R.WORK_SCOPE), 0) / 10000000, 2)  AS TOTAL_WORK_SCOPE,
                                                            ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '1' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS PRAN,
                                                            ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '2' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS RFL,
                                                            ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '3' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS CTL,
                                                            ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '4' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS PDL,
                                                            ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '5' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS GROUP_CS
                                                        FROM 
                                                            AUDIT_REPORT R
                                                        LEFT JOIN 
                                                            AUDIT_TASK AT 
                                                            ON R.TASK_ID = AT.TASK_ID
                                                      INNER JOIN 
                                                            C_COMPANY_GROUP CG 
                                                            ON CG.COMPANY_GROUP_ID = AT.COMPANY_GROUP_ID
                                                        INNER JOIN 
                                                             C_COMPANY C ON C.COMPANY_ID = AT.COMPANY_ID
                                                        WHERE 
                                                            R.ACT = 1 AND EXTRACT(YEAR FROM R.CDT) =  '" + selectedYear + @"'
                                                        GROUP BY 
                                                             C.COMPANY_NAME
                                                        ORDER BY 
                                                            TOTAL_WORK_SCOPE DESC
                                                    )
                                                    WHERE ROWNUM <= 5";

                List<A_SUMMARY_VIEW> KSA = db.A_SUMMARY_VIEW.Where(x => x.SV_USER == "AUDIT").ToList();
                Update_Date _UD = new Update_Date();

                var totalTaskCount = db.Database.SqlQuery<int>("SELECT COUNT(TASK_ID) AS TOTAL_TASK FROM AUDIT_TASK WHERE ACT = 1 AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_TASK = totalTaskCount;

                var totalAuditReportCount = db.Database.SqlQuery<int>("SELECT COUNT(AUDIT_RPT_ID) AS TOTAL_REPORT FROM AUDIT_REPORT WHERE ACT=1 AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_AUDIT_REPORT = totalAuditReportCount;


                var totalAuditorCount = db.Database.SqlQuery<int>("SELECT COUNT(AUDITOR_ID) AS TOTAL_AUDITOR  from AUDIT_AUDITOR WHERE ACT=1").FirstOrDefault();
                _UD.TOTAL_AUDITOR = totalAuditorCount;

                var totalDirectSavingAmtSum = db.Database.SqlQuery<decimal>("SELECT  NVL(SUM(DIRECT_SAVING_AMOUNT), 0) AS TOTAL_DIRECT_SAVING_AMOUNT from AUDIT_REPORT WHERE ACT=1 AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_DIRECT_SAVING_AMOUNT = Math.Round(totalDirectSavingAmtSum / 10000000, 3);

                var totalInDirectSavingAmtSum = db.Database.SqlQuery<decimal>("SELECT  NVL(SUM(INDIRECT_SAVING_AMOUNT), 0) AS TOTAL_INDIRECT_SAVING_AMOUNT from AUDIT_REPORT WHERE ACT=1 AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_INDIRECT_SAVING_AMOUNT = Math.Round(totalInDirectSavingAmtSum / 10000000, 3);

                var totalSavingAmtSum = db.Database.SqlQuery<decimal>("SELECT (NVL(SUM(DIRECT_SAVING_AMOUNT), 0) + NVL(SUM(INDIRECT_SAVING_AMOUNT), 0)) AS TOTAL_SAVING_AMOUNT from AUDIT_REPORT WHERE ACT=1 AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_SAVING_AMOUNT = Math.Round(totalSavingAmtSum / 10000000, 2);

                var totalDemotionSum = db.Database.SqlQuery<int>("SELECT NVL(SUM(DEMOTION), 0) AS TOTAL_DEMOTION  from AUDIT_REPORT WHERE ACT = 1 AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_DEMOTION = totalDemotionSum;

                var totalDevelopmentSum = db.Database.SqlQuery<decimal>("SELECT NVL(SUM(DEVELOPMENT), 0) AS TOTAL_DEVELOPMENT  from AUDIT_REPORT WHERE ACT = 1 AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_DEVELOPMENT = Math.Round(totalDevelopmentSum / 10000000, 2);

                var totalJobSackOutSum = db.Database.SqlQuery<int>("SELECT NVL(SUM(JOB_SACK_OUT), 0) AS TOTAL_JOB_SACK_OUT from AUDIT_REPORT WHERE ACT = 1 AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_JOB_SACK_OUT = totalJobSackOutSum;

                var totalTransferSum = db.Database.SqlQuery<int>("SELECT NVL(SUM(TRANSFER), 0) AS TOTAL_TRANSFER from AUDIT_REPORT WHERE ACT = 1 AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_TRANSFER = totalTransferSum;

                var totalWarningLatterSum = db.Database.SqlQuery<int>("SELECT NVL(SUM(WARNING_LETTER), 0) AS TOTAL_WARNING_LETTER from AUDIT_REPORT WHERE ACT = 1 AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_WARNING_LETTER = totalWarningLatterSum;

                var totalPenaltySum = db.Database.SqlQuery<decimal>("SELECT  NVL(SUM(PENALTY), 0) AS TOTAL_PENALTY from AUDIT_REPORT WHERE ACT = 1 AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_PENALTY = totalPenaltySum;


                var totalHoldPaymentSum = db.Database.SqlQuery<decimal>("SELECT  NVL(SUM(HOLD_PAYMENT), 0) AS TOTAL_HOLD_PAYMENT from AUDIT_REPORT WHERE ACT = 1 AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_HOLD_PAYMENT = Math.Round(totalHoldPaymentSum / 100000, 2);

                var totalSupplierBlackListSum = 0;
                _UD.TOTAL_SUPPLIER_BLACKLIST = totalSupplierBlackListSum;

                //var totalSupplierBlackListSum = db.Database.SqlQuery<int>("SELECT  NVL(SUM(SUPPLIER_BLACKLIST), 0) AS TOTAL_SUPPLIER_BLACKLIST from AUDIT_REPORT WHERE ACT = 1").FirstOrDefault();
                //_UD.TOTAL_SUPPLIER_BLACKLIST = totalSupplierBlackListSum;

                var totalInefficiencyIdentifiedSum = db.Database.SqlQuery<decimal>("SELECT  NVL(SUM(INEFFICIENCY_IDENTIFIED), 0) AS TOTAL_INEFFICIENCY_IDENTIFIED from AUDIT_REPORT WHERE ACT = 1 AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_INEFFICIENCY_IDENTIFIED = Math.Round(totalInefficiencyIdentifiedSum / 10000000, 2);

                var totalCorruptionIdentifiedSum = db.Database.SqlQuery<decimal>("SELECT  NVL(SUM(CORRUPTION_IDENTIFIED), 0) AS TOTAL_CORRUPTION_IDENTIFIED from AUDIT_REPORT WHERE ACT = 1 AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_CORRUPTION_IDENTIFIED = Math.Round(totalCorruptionIdentifiedSum / 10000000, 2);

                var totalWorkScopeSum = db.Database.SqlQuery<decimal>("SELECT  NVL(SUM(WORK_SCOPE), 0) AS TOTAL_WORK_SCOPE from AUDIT_REPORT WHERE ACT=1 AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_WORK_SCOPE = Math.Round(totalWorkScopeSum / 10000000, 3);

                var totalReportSentCount = db.Database.SqlQuery<int>("SELECT  COUNT(AUDIT_RPT_ID) AS TOTAL_REPORT_SENT FROM AUDIT_REPORT WHERE ACT = 1 AND RPT_SENT_DATE IS NOT NULL AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_REPORT_SENT = totalReportSentCount;

                var totalAuditWithReportCount = db.Database.SqlQuery<int>("" +
                    "SELECT COUNT ( CASE  WHEN     AT.REPORT_CREATE_STATUS = 1 AND AT.REPORT_STATUS_ID IN (1, 3)  AND R.REPORT_STATUS_ID IN (1, 3) THEN   1 END) AS TOTAL_WITH_REPORT FROM  AUDIT_REPORT R LEFT JOIN AUDIT_TASK AT ON R.TASK_ID = AT.TASK_ID WHERE R.ACT=1 AND EXTRACT(YEAR FROM R.CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_AUDIT_WITH_REPORT = totalAuditWithReportCount;

                var totalAuditWithoutReportCount = db.Database.SqlQuery<int>("" +
                    "SELECT COUNT ( CASE  WHEN     AT.REPORT_CREATE_STATUS = 1 AND AT.REPORT_STATUS_ID IN (2, 4)  AND R.REPORT_STATUS_ID IN (2, 4) THEN   1 END) AS TOTAL_WITHOUT_REPORT FROM  AUDIT_REPORT R LEFT JOIN AUDIT_TASK AT ON R.TASK_ID = AT.TASK_ID WHERE R.ACT=1 AND EXTRACT(YEAR FROM R.CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_AUDIT_WITHOUT_REPORT = totalAuditWithoutReportCount;




                _UD.TASK_STATUS_DATE = KSA.Where(x => x.SV_TABLE == "AUDIT_TASK_STATUS_DATA").First().UDT;
                _UD.REPORT_STATUS_DATE = KSA.Where(x => x.SV_TABLE == "AUDIT_REPORT_STATUS").First().UDT;
                _UD.REPORT_SENT_STATUS_DATE = KSA.Where(x => x.SV_TABLE == "AUDIT_REPORT_SENT_SUM").First().UDT;
                _UD.COMPANY_GROUP_DATE = KSA.Where(x => x.SV_TABLE == "COMPAY_GROUP_WISE_REPORT_SUM").First().UDT;
                _UD.CURRENT_LAST_MONTH_DATE = KSA.Where(x => x.SV_TABLE == "CURRENT_LAST_MONTH_REPORT_SUM").First().UDT;


                SummaryViewKSA _SummaryViewKSA = new SummaryViewKSA();
                _SummaryViewKSA.AuditorWiseAuditReportCount = AuditorWiseAuditReportCountData(AuditorWiseAuditReportCountSQL);
                _SummaryViewKSA.ReportSummary = ReportSummaryData(ReportSummarySQL);
                _SummaryViewKSA.CategoryCGWiseTotalReport = GetTop5Category(CategoryCGWiseTotalReportSQL);
                //var CategoryWiseWorkScopeTotalReportData = db.Database.SqlQuery<CategoryWiseWorkScopeTotalReport>(CategoryWiseWorkScopeTotalReportSQL).ToList();
                _SummaryViewKSA.CategoryWiseWorkScopeTotalReport = GetTopWorkScope5Category(CategoryWiseWorkScopeTotalReportSQL);

                //var totalCorruptionIdentifiedSum = db.Database.SqlQuery<decimal>("SELECT  NVL(SUM(CORRUPTION_IDENTIFIED), 0) AS TOTAL_CORRUPTION_IDENTIFIED from AUDIT_REPORT WHERE ACT = 1").FirstOrDefault();
                //_UD.TOTAL_CORRUPTION_IDENTIFIED = Math.Round(totalCorruptionIdentifiedSum / 10000000, 2);

                _SummaryViewKSA.BUCGWiseTotalReport = GetTop5BU(BUCGWiseTotalReportSQL);
                _SummaryViewKSA.BUCGWiseTotalWorkScopeReport = GetTop5WorkScopeBU(BUCGWiseTotalWorkScopeReportSQL);

                _SummaryViewKSA.UserRole = T;





                _SummaryViewKSA.updateDate = _UD;

            return View(_SummaryViewKSA);
            }
            else
            {
                string AuditorWiseAuditReportCountSQL = @"SELECT  A.AUDITOR_STAFF_ID, A.AUDITOR_NAME, 
                        COUNT(R.AUDIT_RPT_ID)                             AS TOTAL_REPORT,
                        
                        COUNT (
                         CASE
                             WHEN     AT.REPORT_CREATE_STATUS = 1
                                  AND AT.REPORT_STATUS_ID IN (1, 3)
                                  AND R.REPORT_STATUS_ID IN (1, 3)
                             THEN
                                 1
                         END)       AS TOTAL_WITH_REPORT,
                         
                         COUNT (
                          CASE
                              WHEN     AT.REPORT_CREATE_STATUS = 1
                                   AND AT.REPORT_STATUS_ID IN (2, 4)
                                   AND R.REPORT_STATUS_ID IN (2, 4)
                              THEN
                                  1
                          END)       AS TOTAL_WITHOUT_REPORT,

                        ROUND(NVL(SUM(R.WORK_SCOPE), 0) / 10000000, 3)  AS TOTAL_WORK_SCOPE,
                        ROUND(NVL(SUM(R.DIRECT_SAVING_AMOUNT), 0) / 10000000, 3)   AS TOTAL_DIRECT_SAVING_AMOUNT,
                        ROUND(NVL(SUM(R.INDIRECT_SAVING_AMOUNT), 0)  / 10000000, 3)   AS TOTAL_INDIRECT_SAVING_AMOUNT,
                        NVL(SUM(R.DEMOTION), 0)                           AS TOTAL_DEMOTION,
                        NVL(SUM(R.DEVELOPMENT), 0)                        AS TOTAL_DEVELOPMENT,
                        NVL(SUM(R.JOB_SACK_OUT), 0)                       AS TOTAL_JOB_SACK_OUT,
                        NVL(SUM(R.TRANSFER), 0)                           AS TOTAL_TRANSFER,
                        NVL(SUM(R.WARNING_LETTER), 0)                     AS TOTAL_WARNING_LETTER,
                        NVL(SUM(R.PENALTY), 0)                            AS TOTAL_PENALTY,
                        NVL(SUM(R.HOLD_PAYMENT), 0)                       AS TOTAL_HOLD_PAYMENT,
                        NVL(SUM(R.INEFFICIENCY_IDENTIFIED), 0)            AS TOTAL_INEFFICIENCY_IDENTIFIED,
                        NVL(SUM(R.CORRUPTION_IDENTIFIED), 0)              AS TOTAL_CORRUPTION_IDENTIFIED
                        FROM AUDIT_REPORT R 
                        LEFT JOIN AUDIT_AUDITOR A ON R.CDU = A.AUDITOR_STAFF_ID
                        LEFT JOIN AUDIT_TASK AT ON R.TASK_ID = AT.TASK_ID
                        WHERE R.ACT = 1 AND R.CDU='" + U + @"' 

                         AND EXTRACT(YEAR FROM R.CDT) =  '" + selectedYear + @"'

                        GROUP BY A.AUDITOR_STAFF_ID, A.AUDITOR_NAME
                        ORDER BY TOTAL_REPORT DESC";

                //string ReportSummarySQL = @"SELECT CDU, COMPANY_GROUP_ID,COMPANY_GROUP_NAME,TOTAL_NO_OF_REPORT,LAST_MONTH_TOTAL_REPORT,THIS_MONTH_TOTAL_REPORT,
                //                           TOTAL_WITH_REPORT,LAST_MONTH_WITH_REPORT,THIS_MONTH_WITH_REPORT,
                //                           TOTAL_WITHOUT_REPORT,LAST_MONTH_WITHOUT_REPORT,THIS_MONTH_WITHOUT_REPORT
                //                           FROM VW_ADT_CG_AUDITOR_RPT WHERE CDU='" + U + @"' ";
                string ReportSummarySQL = @"SELECT CG.COMPANY_GROUP_ID,
                                             CG.COMPANY_GROUP_NAME,
                                             R.CDU,
                                             COUNT (CASE
                                                        WHEN     AT.REPORT_CREATE_STATUS = 1
                                                             AND AT.REPORT_STATUS_ID IN (1,
                                                                                         2,
                                                                                         3,
                                                                                         4)
                                                        THEN
                                                            1
                                                    END)    AS TOTAL_NO_OF_REPORT,
                                             COUNT (
                                                 CASE
                                                     WHEN     AT.REPORT_CREATE_STATUS = 1
                                                          AND AT.REPORT_STATUS_ID IN (1,
                                                                                      2,
                                                                                      3,
                                                                                      4)
                                                          AND R.CDT >= ADD_MONTHS (TRUNC (SYSDATE, 'MM'), -1)
                                                          AND R.CDT < TRUNC (SYSDATE, 'MM')
                                                     THEN
                                                         1
                                                 END)       AS LAST_MONTH_TOTAL_REPORT,
                                             COUNT (CASE
                                                        WHEN     AT.REPORT_CREATE_STATUS = 1
                                                             AND AT.REPORT_STATUS_ID IN (1,
                                                                                         2,
                                                                                         3,
                                                                                         4)
                                                             AND R.CDT >= TRUNC (SYSDATE, 'MM')
                                                             AND R.CDT < ADD_MONTHS (TRUNC (SYSDATE, 'MM'), 1)
                                                        THEN
                                                            1
                                                    END)    AS THIS_MONTH_TOTAL_REPORT,
                                             COUNT (
                                                 CASE
                                                     WHEN     AT.REPORT_CREATE_STATUS = 1
                                                          AND AT.REPORT_STATUS_ID IN (1, 3)
                                                          AND R.REPORT_STATUS_ID IN (1, 3)
                                                     THEN
                                                         1
                                                 END)       AS TOTAL_WITH_REPORT,
                                             COUNT (
                                                 CASE
                                                     WHEN     AT.REPORT_CREATE_STATUS = 1
                                                          AND AT.REPORT_STATUS_ID IN (1, 3)
                                                          AND R.REPORT_STATUS_ID IN (1, 3)
                                                          AND R.CDT >= ADD_MONTHS (TRUNC (SYSDATE, 'MM'), -1)
                                                          AND R.CDT < TRUNC (SYSDATE, 'MM')
                                                     THEN
                                                         1
                                                 END)       AS LAST_MONTH_WITH_REPORT,
                                             COUNT (
                                                 CASE
                                                     WHEN     AT.REPORT_CREATE_STATUS = 1
                                                          AND AT.REPORT_STATUS_ID IN (1, 3)
                                                          AND R.REPORT_STATUS_ID IN (1, 3)
                                                          AND R.CDT >= TRUNC (SYSDATE, 'MM')
                                                          AND R.CDT < ADD_MONTHS (TRUNC (SYSDATE, 'MM'), 1)
                                                     THEN
                                                         1
                                                 END)       AS THIS_MONTH_WITH_REPORT,
                                             COUNT (
                                                 CASE
                                                     WHEN     AT.REPORT_CREATE_STATUS = 1
                                                          AND AT.REPORT_STATUS_ID IN (2, 4)
                                                          AND R.REPORT_STATUS_ID IN (2, 4)
                                                     THEN
                                                         1
                                                 END)       AS TOTAL_WITHOUT_REPORT,
                                             COUNT (
                                                 CASE
                                                     WHEN     AT.REPORT_CREATE_STATUS = 1
                                                          AND AT.REPORT_STATUS_ID IN (2, 4)
                                                          AND R.REPORT_STATUS_ID IN (2, 4)
                                                          AND R.CDT >= ADD_MONTHS (TRUNC (SYSDATE, 'MM'), -1)
                                                          AND R.CDT < TRUNC (SYSDATE, 'MM')
                                                     THEN
                                                         1
                                                 END)       AS LAST_MONTH_WITHOUT_REPORT,
                                             COUNT (
                                                 CASE
                                                     WHEN     AT.REPORT_CREATE_STATUS = 1
                                                          AND AT.REPORT_STATUS_ID IN (2, 4)
                                                          AND R.REPORT_STATUS_ID IN (2, 4)
                                                          AND R.CDT >= TRUNC (SYSDATE, 'MM')
                                                          AND R.CDT < ADD_MONTHS (TRUNC (SYSDATE, 'MM'), 1)
                                                     THEN
                                                         1
                                                 END)       AS THIS_MONTH_WITHOUT_REPORT
                                        FROM AUDIT_REPORT R
                                             LEFT JOIN AUDIT_TASK AT ON R.TASK_ID = AT.TASK_ID
                                             INNER JOIN C_COMPANY_GROUP CG
                                                 ON CG.COMPANY_GROUP_ID = AT.COMPANY_GROUP_ID
                                       WHERE R.ACT = 1 AND R.CDU='" + U + @"' AND EXTRACT(YEAR FROM R.CDT) =  '" + selectedYear + @"'
                                    GROUP BY CG.COMPANY_GROUP_ID, CG.COMPANY_GROUP_NAME, R.CDU
                                    ORDER BY CG.COMPANY_GROUP_ID";

                string CategoryCGWiseTotalReportSQL = @"SELECT *
                                            FROM (
                                                SELECT 
                                                    TA.TASK_CATEGORY_NAME,
                                                    COUNT(R.AUDIT_RPT_ID) AS TOTAL_REPORT_CATEGORY,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '1' THEN 1 ELSE 0 END) AS PRAN,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '2' THEN 1 ELSE 0 END) AS RFL,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '3' THEN 1 ELSE 0 END) AS CTL,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '4' THEN 1 ELSE 0 END) AS PDL,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '5' THEN 1 ELSE 0 END) AS GROUP_CS
                                                FROM 
                                                    AUDIT_REPORT R
                                                LEFT JOIN 
                                                    AUDIT_TASK_CATEGORY TA 
                                                    ON R.TASK_CATEGORY_ID = TA.TASK_CATEGORY_ID
                                                LEFT JOIN 
                                                    AUDIT_TASK AT 
                                                    ON R.TASK_ID = AT.TASK_ID
                                                INNER JOIN 
                                                    C_COMPANY_GROUP CG 
                                                    ON CG.COMPANY_GROUP_ID = AT.COMPANY_GROUP_ID
                                                WHERE 
                                                    R.ACT = 1 AND R.CDU='" + U + @"'  AND EXTRACT(YEAR FROM R.CDT) =  '" + selectedYear + @"'
                                                GROUP BY 
                                                    TA.TASK_CATEGORY_NAME
                                                ORDER BY 
                                                    TOTAL_REPORT_CATEGORY DESC
                                            )WHERE ROWNUM <= 5";

                string CategoryWiseWorkScopeTotalReportSQL = @"SELECT *
                                            FROM (
                                                SELECT 
                                                    TA.TASK_CATEGORY_NAME,
                                                    ROUND(NVL(SUM(R.WORK_SCOPE), 0) / 10000000, 2) AS TOTAL_WORK_SCOPE,
                                                    ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '1' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS PRAN,
                                                    ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '2' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS RFL,
                                                    ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '3' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS CTL,
                                                    ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '4' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS PDL,
                                                    ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '5' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS GROUP_CS
                                                FROM 
                                                    AUDIT_REPORT R
                                                LEFT JOIN 
                                                    AUDIT_TASK_CATEGORY TA 
                                                    ON R.TASK_CATEGORY_ID = TA.TASK_CATEGORY_ID
                                                LEFT JOIN 
                                                    AUDIT_TASK AT 
                                                    ON R.TASK_ID = AT.TASK_ID
                                                INNER JOIN 
                                                    C_COMPANY_GROUP CG 
                                                    ON CG.COMPANY_GROUP_ID = AT.COMPANY_GROUP_ID
                                                WHERE 
                                                    R.ACT = 1 AND R.CDU='" + U + @"'  AND EXTRACT(YEAR FROM R.CDT) =  '" + selectedYear + @"'
                                                GROUP BY 
                                                    TA.TASK_CATEGORY_NAME
                                                ORDER BY 
                                                    TOTAL_WORK_SCOPE DESC
                                            )
                                            WHERE ROWNUM <= 5";
                string BUCGWiseTotalReportSQL = @"SELECT *
                                            FROM (
                                                SELECT 
                                                    C.COMPANY_NAME,
                                                    COUNT(R.AUDIT_RPT_ID) AS TOTAL_REPORT_BU,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '1' THEN 1 ELSE 0 END) AS PRAN,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '2' THEN 1 ELSE 0 END) AS RFL,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '3' THEN 1 ELSE 0 END) AS CTL,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '4' THEN 1 ELSE 0 END) AS PDL,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '5' THEN 1 ELSE 0 END) AS GROUP_CS
                                                FROM 
                                                    AUDIT_REPORT R
                                                LEFT JOIN 
                                                    AUDIT_TASK AT 
                                                    ON R.TASK_ID = AT.TASK_ID
                                              INNER JOIN 
                                                    C_COMPANY_GROUP CG 
                                                    ON CG.COMPANY_GROUP_ID = AT.COMPANY_GROUP_ID
                                                INNER JOIN 
                                                     C_COMPANY C ON C.COMPANY_ID = AT.COMPANY_ID
                                                WHERE 
                                                    R.ACT = 1 AND R.CDU='" + U + @"'  AND EXTRACT(YEAR FROM R.CDT) =  '" + selectedYear + @"'
                                                GROUP BY 
                                                     C.COMPANY_NAME
                                                ORDER BY 
                                                    TOTAL_REPORT_BU DESC
                                            )
                                            WHERE ROWNUM <= 5";

                string BUCGWiseTotalWorkScopeReportSQL = @"SELECT *
                                                    FROM (
                                                        SELECT 
                                                            C.COMPANY_NAME,
                                                            ROUND(NVL(SUM(R.WORK_SCOPE), 0) / 10000000, 2)  AS TOTAL_WORK_SCOPE,
                                                            ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '1' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS PRAN,
                                                            ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '2' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS RFL,
                                                            ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '3' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS CTL,
                                                            ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '4' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS PDL,
                                                            ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '5' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS GROUP_CS
                                                        FROM 
                                                            AUDIT_REPORT R
                                                        LEFT JOIN 
                                                            AUDIT_TASK AT 
                                                            ON R.TASK_ID = AT.TASK_ID
                                                      INNER JOIN 
                                                            C_COMPANY_GROUP CG 
                                                            ON CG.COMPANY_GROUP_ID = AT.COMPANY_GROUP_ID
                                                        INNER JOIN 
                                                             C_COMPANY C ON C.COMPANY_ID = AT.COMPANY_ID
                                                        WHERE 
                                                            R.ACT = 1 AND R.CDU='" + U + @"'  AND EXTRACT(YEAR FROM R.CDT) =  '" + selectedYear + @"'
                                                        GROUP BY 
                                                             C.COMPANY_NAME
                                                        ORDER BY 
                                                            TOTAL_WORK_SCOPE DESC
                                                    )
                                                    WHERE ROWNUM <= 5";

                List<A_SUMMARY_VIEW> KSA = db.A_SUMMARY_VIEW.Where(x => x.SV_USER == "AUDIT").ToList();
                Update_Date _UD = new Update_Date();

                var totalTaskCount = db.Database.SqlQuery<int>("SELECT COUNT(TASK_ID) AS TOTAL_TASK FROM AUDIT_TASK WHERE ACT = 1 AND CDU= '" + U + @"'  AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"' ").FirstOrDefault();
                _UD.TOTAL_TASK = totalTaskCount;

                var totalAuditReportCount = db.Database.SqlQuery<int>("SELECT COUNT(AUDIT_RPT_ID) AS TOTAL_REPORT FROM AUDIT_REPORT WHERE ACT=1 AND CDU= '" + U + @"' AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"' ").FirstOrDefault();
                _UD.TOTAL_AUDIT_REPORT = totalAuditReportCount;
                var totalAuditorCount = db.Database.SqlQuery<int>("SELECT COUNT(AUDITOR_ID) AS TOTAL_AUDITOR  from AUDIT_AUDITOR WHERE ACT=1").FirstOrDefault();
                _UD.TOTAL_AUDITOR = totalAuditorCount;

                var totalDirectSavingAmtSum = db.Database.SqlQuery<decimal>("SELECT  NVL(SUM(DIRECT_SAVING_AMOUNT), 0) AS TOTAL_DIRECT_SAVING_AMOUNT from AUDIT_REPORT WHERE ACT=1 AND CDU= '" + U + @"' AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_DIRECT_SAVING_AMOUNT = Math.Round(totalDirectSavingAmtSum / 10000000, 3);

                var totalInDirectSavingAmtSum = db.Database.SqlQuery<decimal>("SELECT  NVL(SUM(INDIRECT_SAVING_AMOUNT), 0) AS TOTAL_INDIRECT_SAVING_AMOUNT from AUDIT_REPORT WHERE ACT=1 AND CDU= '" + U + @"' AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"' ").FirstOrDefault();
                _UD.TOTAL_INDIRECT_SAVING_AMOUNT = Math.Round(totalInDirectSavingAmtSum / 10000000, 3);

                var totalSavingAmtSum = db.Database.SqlQuery<decimal>("SELECT (NVL(SUM(DIRECT_SAVING_AMOUNT), 0) + NVL(SUM(INDIRECT_SAVING_AMOUNT), 0)) AS TOTAL_SAVING_AMOUNT from AUDIT_REPORT WHERE ACT=1 AND CDU= '" + U + @"' AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_SAVING_AMOUNT = Math.Round(totalSavingAmtSum / 10000000, 2);

                var totalDemotionSum = db.Database.SqlQuery<int>("SELECT NVL(SUM(DEMOTION), 0) AS TOTAL_DEMOTION  from AUDIT_REPORT WHERE ACT = 1 AND CDU= '" + U + @"' AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_DEMOTION = totalDemotionSum;

                var totalDevelopmentSum = db.Database.SqlQuery<decimal>("SELECT NVL(SUM(DEVELOPMENT), 0) AS TOTAL_DEVELOPMENT  from AUDIT_REPORT WHERE ACT = 1 AND CDU= '" + U + @"' AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_DEVELOPMENT = Math.Round(totalDevelopmentSum / 10000000, 2);

                var totalJobSackOutSum = db.Database.SqlQuery<int>("SELECT NVL(SUM(JOB_SACK_OUT), 0) AS TOTAL_JOB_SACK_OUT from AUDIT_REPORT WHERE ACT = 1 AND CDU= '" + U + @"' AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"' ").FirstOrDefault();
                _UD.TOTAL_JOB_SACK_OUT = totalJobSackOutSum;

                var totalTransferSum = db.Database.SqlQuery<int>("SELECT NVL(SUM(TRANSFER), 0) AS TOTAL_TRANSFER from AUDIT_REPORT WHERE ACT = 1 AND CDU= '" + U + @"' AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"' ").FirstOrDefault();
                _UD.TOTAL_TRANSFER = totalTransferSum;

                var totalWarningLatterSum = db.Database.SqlQuery<int>("SELECT NVL(SUM(WARNING_LETTER), 0) AS TOTAL_WARNING_LETTER from AUDIT_REPORT WHERE ACT = 1 AND CDU= '" + U + @"' AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"' ").FirstOrDefault();
                _UD.TOTAL_WARNING_LETTER = totalWarningLatterSum;

                var totalPenaltySum = db.Database.SqlQuery<decimal>("SELECT  NVL(SUM(PENALTY), 0) AS TOTAL_PENALTY from AUDIT_REPORT WHERE ACT = 1 AND CDU= '" + U + @"' AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"' ").FirstOrDefault();
                _UD.TOTAL_PENALTY = totalPenaltySum;


                var totalHoldPaymentSum = db.Database.SqlQuery<decimal>("SELECT  NVL(SUM(HOLD_PAYMENT), 0) AS TOTAL_HOLD_PAYMENT from AUDIT_REPORT WHERE ACT = 1 AND CDU= '" + U + @"' AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"' ").FirstOrDefault();
                _UD.TOTAL_HOLD_PAYMENT = Math.Round(totalHoldPaymentSum / 100000, 2);

                var totalSupplierBlackListSum = 0;
                _UD.TOTAL_SUPPLIER_BLACKLIST = totalSupplierBlackListSum;

                //var totalSupplierBlackListSum = db.Database.SqlQuery<int>("SELECT  NVL(SUM(SUPPLIER_BLACKLIST), 0) AS TOTAL_SUPPLIER_BLACKLIST from AUDIT_REPORT WHERE ACT = 1 AND CDU= '" + U + @"'").FirstOrDefault();
                //_UD.TOTAL_SUPPLIER_BLACKLIST = totalSupplierBlackListSum;

                var totalInefficiencyIdentifiedSum = db.Database.SqlQuery<decimal>("SELECT  NVL(SUM(INEFFICIENCY_IDENTIFIED), 0) AS TOTAL_INEFFICIENCY_IDENTIFIED from AUDIT_REPORT WHERE ACT = 1 AND CDU= '" + U + @"' AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"' ").FirstOrDefault();
                _UD.TOTAL_INEFFICIENCY_IDENTIFIED = Math.Round(totalInefficiencyIdentifiedSum / 10000000, 2);

                var totalCorruptionIdentifiedSum = db.Database.SqlQuery<decimal>("SELECT  NVL(SUM(CORRUPTION_IDENTIFIED), 0) AS TOTAL_CORRUPTION_IDENTIFIED from AUDIT_REPORT WHERE ACT = 1 AND CDU= '" + U + @"' AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"' ").FirstOrDefault();
                _UD.TOTAL_CORRUPTION_IDENTIFIED = Math.Round(totalCorruptionIdentifiedSum / 10000000, 2);

                var totalWorkScopeSum = db.Database.SqlQuery<decimal>("SELECT  NVL(SUM(WORK_SCOPE), 0) AS TOTAL_WORK_SCOPE from AUDIT_REPORT WHERE ACT=1 AND CDU= '" + U + @"' AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"' ").FirstOrDefault();
                _UD.TOTAL_WORK_SCOPE = Math.Round(totalWorkScopeSum / 10000000, 3);

                var totalReportSentCount = db.Database.SqlQuery<int>("SELECT  COUNT(AUDIT_RPT_ID) AS TOTAL_REPORT_SENT FROM AUDIT_REPORT WHERE ACT = 1 AND CDU= '" + U + @"' AND RPT_SENT_DATE IS NOT NULL AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"' ").FirstOrDefault();
                _UD.TOTAL_REPORT_SENT = totalReportSentCount;

                var totalAuditWithReportCount = db.Database.SqlQuery<int>("" +
                   "SELECT COUNT ( CASE  WHEN     AT.REPORT_CREATE_STATUS = 1 AND AT.REPORT_STATUS_ID IN (1, 3)  AND R.REPORT_STATUS_ID IN (1, 3) THEN   1 END) AS TOTAL_WITH_REPORT FROM  AUDIT_REPORT R LEFT JOIN AUDIT_TASK AT ON R.TASK_ID = AT.TASK_ID WHERE R.ACT=1 AND R.CDU= '" + U + @"' AND EXTRACT(YEAR FROM R.CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_AUDIT_WITH_REPORT = totalAuditWithReportCount;

                var totalAuditWithoutReportCount = db.Database.SqlQuery<int>("" +
                    "SELECT COUNT ( CASE  WHEN     AT.REPORT_CREATE_STATUS = 1 AND AT.REPORT_STATUS_ID IN (2, 4)  AND R.REPORT_STATUS_ID IN (2, 4) THEN   1 END) AS TOTAL_WITHOUT_REPORT FROM  AUDIT_REPORT R LEFT JOIN AUDIT_TASK AT ON R.TASK_ID = AT.TASK_ID WHERE R.ACT=1 AND R.CDU= '" + U + @"' AND EXTRACT(YEAR FROM R.CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_AUDIT_WITHOUT_REPORT = totalAuditWithoutReportCount;


                _UD.TASK_STATUS_DATE = KSA.Where(x => x.SV_TABLE == "AUDIT_TASK_STATUS_DATA").First().UDT;
                _UD.REPORT_STATUS_DATE = KSA.Where(x => x.SV_TABLE == "AUDIT_REPORT_STATUS").First().UDT;
                _UD.REPORT_SENT_STATUS_DATE = KSA.Where(x => x.SV_TABLE == "AUDIT_REPORT_SENT_SUM").First().UDT;
                _UD.COMPANY_GROUP_DATE = KSA.Where(x => x.SV_TABLE == "COMPAY_GROUP_WISE_REPORT_SUM").First().UDT;
                _UD.CURRENT_LAST_MONTH_DATE = KSA.Where(x => x.SV_TABLE == "CURRENT_LAST_MONTH_REPORT_SUM").First().UDT;


                SummaryViewKSA _SummaryViewKSA = new SummaryViewKSA();
                _SummaryViewKSA.AuditorWiseAuditReportCount = AuditorWiseAuditReportCountData(AuditorWiseAuditReportCountSQL);
                _SummaryViewKSA.ReportSummary = ReportSummaryData(ReportSummarySQL);
                _SummaryViewKSA.CategoryCGWiseTotalReport = GetTop5Category(CategoryCGWiseTotalReportSQL);
                _SummaryViewKSA.CategoryWiseWorkScopeTotalReport = GetTopWorkScope5Category(CategoryWiseWorkScopeTotalReportSQL);
                _SummaryViewKSA.BUCGWiseTotalReport = GetTop5BU(BUCGWiseTotalReportSQL);
                _SummaryViewKSA.BUCGWiseTotalWorkScopeReport = GetTop5WorkScopeBU(BUCGWiseTotalWorkScopeReportSQL);

                _SummaryViewKSA.updateDate = _UD;

                return View(_SummaryViewKSA);
            }
        }

        [KAuth(13)]
        public ActionResult AUDIT_DASHBOARD_NEW(int? year)
        {
            int selectedYear = year ?? DateTime.Now.Year;
            ViewBag.SelectedYear = selectedYear;

            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            AUDIT_LOG AL = new AUDIT_LOG();
            AL.LOG_ID = Sequence.GetSeq(37).ToString();
            AL.CMNU_TEXT = 13;
            AL.USER_ID = U;
            AL.CDT = DateTime.Now;
            db.AUDIT_LOG.Add(AL);
            db.SaveChanges();

            if (T == "10")
            {

                string AuditorWiseAuditReportCountSQL = @"SELECT  A.AUDITOR_STAFF_ID, A.AUDITOR_NAME, 
                         COUNT(R.AUDIT_RPT_ID)                             AS TOTAL_REPORT,
                         COUNT (
                         CASE
                             WHEN     AT.REPORT_CREATE_STATUS = 1
                                  AND AT.REPORT_STATUS_ID IN (1, 3)
                                  AND R.REPORT_STATUS_ID IN (1, 3)
                             THEN
                                 1
                         END)       AS TOTAL_WITH_REPORT,
                         
                         COUNT (
                          CASE
                              WHEN     AT.REPORT_CREATE_STATUS = 1
                                   AND AT.REPORT_STATUS_ID IN (2, 4)
                                   AND R.REPORT_STATUS_ID IN (2, 4)
                              THEN
                                  1
                          END)       AS TOTAL_WITHOUT_REPORT,
                        ROUND(NVL(SUM(R.WORK_SCOPE), 0) / 10000000, 2)  AS TOTAL_WORK_SCOPE,
                        ROUND(NVL(SUM(R.DIRECT_SAVING_AMOUNT), 0) / 10000000, 2)   AS TOTAL_DIRECT_SAVING_AMOUNT,
                        ROUND(NVL(SUM(R.INDIRECT_SAVING_AMOUNT), 0)  / 10000000, 2)   AS TOTAL_INDIRECT_SAVING_AMOUNT,
                        NVL(SUM(R.DEMOTION), 0)                           AS TOTAL_DEMOTION,
                        NVL(SUM(R.DEVELOPMENT), 0)                        AS TOTAL_DEVELOPMENT,
                        NVL(SUM(R.JOB_SACK_OUT), 0)                       AS TOTAL_JOB_SACK_OUT,
                        NVL(SUM(R.TRANSFER), 0)                           AS TOTAL_TRANSFER,
                        NVL(SUM(R.WARNING_LETTER), 0)                     AS TOTAL_WARNING_LETTER,
                        NVL(SUM(R.PENALTY), 0)                            AS TOTAL_PENALTY,
                        NVL(SUM(R.HOLD_PAYMENT), 0)                       AS TOTAL_HOLD_PAYMENT,
                        NVL(SUM(R.INEFFICIENCY_IDENTIFIED), 0)            AS TOTAL_INEFFICIENCY_IDENTIFIED,
                        NVL(SUM(R.CORRUPTION_IDENTIFIED), 0)              AS TOTAL_CORRUPTION_IDENTIFIED
                        FROM AUDIT_REPORT R 
                        LEFT JOIN AUDIT_AUDITOR A ON R.CDU = A.AUDITOR_STAFF_ID
                        LEFT JOIN AUDIT_TASK AT ON R.TASK_ID = AT.TASK_ID
                        WHERE R.ACT = 1
                        
                        AND EXTRACT(YEAR FROM R.CDT) =  '" + selectedYear + @"'

                        GROUP BY A.AUDITOR_STAFF_ID, A.AUDITOR_NAME
                        ORDER BY TOTAL_REPORT DESC";

                //string ReportSummarySQL = @"SELECT COMPANY_GROUP_ID,COMPANY_GROUP_NAME,TOTAL_NO_OF_REPORT,LAST_MONTH_TOTAL_REPORT,THIS_MONTH_TOTAL_REPORT,
                //                           TOTAL_WITH_REPORT,LAST_MONTH_WITH_REPORT,THIS_MONTH_WITH_REPORT,
                //                           TOTAL_WITHOUT_REPORT,LAST_MONTH_WITHOUT_REPORT,THIS_MONTH_WITHOUT_REPORT
                //                           FROM VW_ADT_COM_GROUP_RPT";

                string ReportSummarySQL = @"SELECT CG.COMPANY_GROUP_ID,
                                         CG.COMPANY_GROUP_NAME,
                                         COUNT (CASE
                                                    WHEN     AT.REPORT_CREATE_STATUS = 1
                                                         AND AT.REPORT_STATUS_ID IN (1,
                                                                                     2,
                                                                                     3,
                                                                                     4)
                                                    THEN
                                                        1
                                                END)    AS TOTAL_NO_OF_REPORT,
                                         COUNT (
                                             CASE
                                                 WHEN     AT.REPORT_CREATE_STATUS = 1
                                                      AND AT.REPORT_STATUS_ID IN (1,
                                                                                  2,
                                                                                  3,
                                                                                  4)
                                                      AND R.CDT >= ADD_MONTHS (TRUNC (SYSDATE, 'MM'), -1)
                                                      AND R.CDT < TRUNC (SYSDATE, 'MM')
                                                 THEN
                                                     1
                                             END)       AS LAST_MONTH_TOTAL_REPORT,
                                         COUNT (CASE
                                                    WHEN     AT.REPORT_CREATE_STATUS = 1
                                                         AND AT.REPORT_STATUS_ID IN (1,
                                                                                     2,
                                                                                     3,
                                                                                     4)
                                                         AND R.CDT >= TRUNC (SYSDATE, 'MM')
                                                         AND R.CDT < ADD_MONTHS (TRUNC (SYSDATE, 'MM'), 1)
                                                    THEN
                                                        1
                                                END)    AS THIS_MONTH_TOTAL_REPORT,
                                         COUNT (
                                             CASE
                                                 WHEN     AT.REPORT_CREATE_STATUS = 1
                                                      AND AT.REPORT_STATUS_ID IN (1, 3)
                                                      AND R.REPORT_STATUS_ID IN (1, 3)
                                                 THEN
                                                     1
                                             END)       AS TOTAL_WITH_REPORT,
                                         COUNT (
                                             CASE
                                                 WHEN     AT.REPORT_CREATE_STATUS = 1
                                                      AND AT.REPORT_STATUS_ID IN (1, 3)
                                                      AND R.REPORT_STATUS_ID IN (1, 3)
                                                      AND R.CDT >= ADD_MONTHS (TRUNC (SYSDATE, 'MM'), -1)
                                                      AND R.CDT < TRUNC (SYSDATE, 'MM')
                                                 THEN
                                                     1
                                             END)       AS LAST_MONTH_WITH_REPORT,
                                         COUNT (
                                             CASE
                                                 WHEN     AT.REPORT_CREATE_STATUS = 1
                                                      AND AT.REPORT_STATUS_ID IN (1, 3)
                                                      AND R.REPORT_STATUS_ID IN (1, 3)
                                                      AND R.CDT >= TRUNC (SYSDATE, 'MM')
                                                      AND R.CDT < ADD_MONTHS (TRUNC (SYSDATE, 'MM'), 1)
                                                 THEN
                                                     1
                                             END)       AS THIS_MONTH_WITH_REPORT,
                                         COUNT (
                                             CASE
                                                 WHEN     AT.REPORT_CREATE_STATUS = 1
                                                      AND AT.REPORT_STATUS_ID IN (2, 4)
                                                      AND R.REPORT_STATUS_ID IN (2, 4)
                                                 THEN
                                                     1
                                             END)       AS TOTAL_WITHOUT_REPORT,
                                         COUNT (
                                             CASE
                                                 WHEN     AT.REPORT_CREATE_STATUS = 1
                                                      AND AT.REPORT_STATUS_ID IN (2, 4)
                                                      AND R.REPORT_STATUS_ID IN (2, 4)
                                                      AND R.CDT >= ADD_MONTHS (TRUNC (SYSDATE, 'MM'), -1)
                                                      AND R.CDT < TRUNC (SYSDATE, 'MM')
                                                 THEN
                                                     1
                                             END)       AS LAST_MONTH_WITHOUT_REPORT,
                                         COUNT (
                                             CASE
                                                 WHEN     AT.REPORT_CREATE_STATUS = 1
                                                      AND AT.REPORT_STATUS_ID IN (2, 4)
                                                      AND R.REPORT_STATUS_ID IN (2, 4)
                                                      AND R.CDT >= TRUNC (SYSDATE, 'MM')
                                                      AND R.CDT < ADD_MONTHS (TRUNC (SYSDATE, 'MM'), 1)
                                                 THEN
                                                     1
                                             END)       AS THIS_MONTH_WITHOUT_REPORT
                                    FROM AUDIT_REPORT R
                                         LEFT JOIN AUDIT_TASK AT ON R.TASK_ID = AT.TASK_ID
                                         INNER JOIN C_COMPANY_GROUP CG
                                             ON CG.COMPANY_GROUP_ID = AT.COMPANY_GROUP_ID
                                   WHERE R.ACT = 1 
                                   AND EXTRACT(YEAR FROM R.CDT) =  '" + selectedYear + @"'
                                GROUP BY CG.COMPANY_GROUP_ID, CG.COMPANY_GROUP_NAME
                                ORDER BY CG.COMPANY_GROUP_ID";


                string CategoryCGWiseTotalReportSQL = @"SELECT *
                                            FROM (
                                                SELECT 
                                                    TA.TASK_CATEGORY_NAME,
                                                    COUNT(R.AUDIT_RPT_ID) AS TOTAL_REPORT_CATEGORY,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '1' THEN 1 ELSE 0 END) AS PRAN,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '2' THEN 1 ELSE 0 END) AS RFL,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '3' THEN 1 ELSE 0 END) AS CTL,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '4' THEN 1 ELSE 0 END) AS PDL,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '5' THEN 1 ELSE 0 END) AS GROUP_CS
                                                FROM 
                                                    AUDIT_REPORT R
                                                LEFT JOIN 
                                                    AUDIT_TASK_CATEGORY TA 
                                                    ON R.TASK_CATEGORY_ID = TA.TASK_CATEGORY_ID
                                                LEFT JOIN 
                                                    AUDIT_TASK AT 
                                                    ON R.TASK_ID = AT.TASK_ID
                                                INNER JOIN 
                                                    C_COMPANY_GROUP CG 
                                                    ON CG.COMPANY_GROUP_ID = AT.COMPANY_GROUP_ID
                                                WHERE 
                                                    R.ACT = 1
                                                    AND EXTRACT(YEAR FROM R.CDT) =  '" + selectedYear + @"'
                                                GROUP BY 
                                                    TA.TASK_CATEGORY_NAME
                                                ORDER BY 
                                                    TOTAL_REPORT_CATEGORY DESC
                                            )WHERE ROWNUM <= 5";

                string CategoryWiseWorkScopeTotalReportSQL = @"SELECT *
                                                                FROM (
                                                                    SELECT 
                                                                        TA.TASK_CATEGORY_NAME,
                                                                        ROUND(NVL(SUM(R.WORK_SCOPE), 0) / 10000000, 2) AS TOTAL_WORK_SCOPE,

                                                                        ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '1' 
                                                                                       THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS PRAN,

                                                                        ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '2' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS RFL,
                                                                        ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '3' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS CTL,
                                                                        ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '4' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS PDL,
                                                                        ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '5' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS GROUP_CS
                                                                    FROM AUDIT_REPORT R
                                                                        LEFT JOIN AUDIT_TASK_CATEGORY TA 
                                                                            ON R.TASK_CATEGORY_ID = TA.TASK_CATEGORY_ID
                                                                        LEFT JOIN AUDIT_TASK AT 
                                                                            ON R.TASK_ID = AT.TASK_ID
                                                                        INNER JOIN C_COMPANY_GROUP CG 
                                                                            ON CG.COMPANY_GROUP_ID = AT.COMPANY_GROUP_ID
                                                                    WHERE R.ACT = 1 AND EXTRACT(YEAR FROM R.CDT) =  '" + selectedYear + @"'
                                                                    GROUP BY TA.TASK_CATEGORY_NAME
                                                                    ORDER BY TOTAL_WORK_SCOPE DESC
                                                                )
                                                                WHERE ROWNUM <= 5";

                string BUCGWiseTotalReportSQL = @"SELECT *
                                            FROM (
                                                SELECT 
                                                    C.COMPANY_NAME,
                                                    COUNT(R.AUDIT_RPT_ID) AS TOTAL_REPORT_BU,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '1' THEN 1 ELSE 0 END) AS PRAN,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '2' THEN 1 ELSE 0 END) AS RFL,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '3' THEN 1 ELSE 0 END) AS CTL,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '4' THEN 1 ELSE 0 END) AS PDL,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '5' THEN 1 ELSE 0 END) AS GROUP_CS
                                                FROM 
                                                    AUDIT_REPORT R
                                                LEFT JOIN 
                                                    AUDIT_TASK AT 
                                                    ON R.TASK_ID = AT.TASK_ID
                                              INNER JOIN 
                                                    C_COMPANY_GROUP CG 
                                                    ON CG.COMPANY_GROUP_ID = AT.COMPANY_GROUP_ID
                                                INNER JOIN 
                                                     C_COMPANY C ON C.COMPANY_ID = AT.COMPANY_ID
                                                WHERE 
                                                    R.ACT = 1 AND EXTRACT(YEAR FROM R.CDT) =  '" + selectedYear + @"'
                                                GROUP BY 
                                                     C.COMPANY_NAME
                                                ORDER BY 
                                                    TOTAL_REPORT_BU DESC
                                            )
                                            WHERE ROWNUM <= 5";

                string BUCGWiseTotalWorkScopeReportSQL = @" SELECT *
                                                    FROM (
                                                        SELECT 
                                                            C.COMPANY_NAME,
                                                            ROUND(NVL(SUM(R.WORK_SCOPE), 0) / 10000000, 2)  AS TOTAL_WORK_SCOPE,
                                                            ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '1' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS PRAN,
                                                            ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '2' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS RFL,
                                                            ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '3' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS CTL,
                                                            ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '4' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS PDL,
                                                            ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '5' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS GROUP_CS
                                                        FROM 
                                                            AUDIT_REPORT R
                                                        LEFT JOIN 
                                                            AUDIT_TASK AT 
                                                            ON R.TASK_ID = AT.TASK_ID
                                                      INNER JOIN 
                                                            C_COMPANY_GROUP CG 
                                                            ON CG.COMPANY_GROUP_ID = AT.COMPANY_GROUP_ID
                                                        INNER JOIN 
                                                             C_COMPANY C ON C.COMPANY_ID = AT.COMPANY_ID
                                                        WHERE 
                                                            R.ACT = 1 AND EXTRACT(YEAR FROM R.CDT) =  '" + selectedYear + @"'
                                                        GROUP BY 
                                                             C.COMPANY_NAME
                                                        ORDER BY 
                                                            TOTAL_WORK_SCOPE DESC
                                                    )
                                                    WHERE ROWNUM <= 5";

                List<A_SUMMARY_VIEW> KSA = db.A_SUMMARY_VIEW.Where(x => x.SV_USER == "AUDIT").ToList();
                Update_Date _UD = new Update_Date();

                var totalTaskCount = db.Database.SqlQuery<int>("SELECT COUNT(TASK_ID) AS TOTAL_TASK FROM AUDIT_TASK WHERE ACT = 1 AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_TASK = totalTaskCount;

                var totalAuditReportCount = db.Database.SqlQuery<int>("SELECT COUNT(AUDIT_RPT_ID) AS TOTAL_REPORT FROM AUDIT_REPORT WHERE ACT=1 AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_AUDIT_REPORT = totalAuditReportCount;


                var totalAuditorCount = db.Database.SqlQuery<int>("SELECT COUNT(AUDITOR_ID) AS TOTAL_AUDITOR  from AUDIT_AUDITOR WHERE ACT=1").FirstOrDefault();
                _UD.TOTAL_AUDITOR = totalAuditorCount;

                var totalDirectSavingAmtSum = db.Database.SqlQuery<decimal>("SELECT  NVL(SUM(DIRECT_SAVING_AMOUNT), 0) AS TOTAL_DIRECT_SAVING_AMOUNT from AUDIT_REPORT WHERE ACT=1 AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_DIRECT_SAVING_AMOUNT = Math.Round(totalDirectSavingAmtSum / 10000000, 2);

                var totalInDirectSavingAmtSum = db.Database.SqlQuery<decimal>("SELECT  NVL(SUM(INDIRECT_SAVING_AMOUNT), 0) AS TOTAL_INDIRECT_SAVING_AMOUNT from AUDIT_REPORT WHERE ACT=1 AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_INDIRECT_SAVING_AMOUNT = Math.Round(totalInDirectSavingAmtSum / 10000000, 2);

                var totalSavingAmtSum = db.Database.SqlQuery<decimal>("SELECT (NVL(SUM(DIRECT_SAVING_AMOUNT), 0) + NVL(SUM(INDIRECT_SAVING_AMOUNT), 0)) AS TOTAL_SAVING_AMOUNT from AUDIT_REPORT WHERE ACT=1 AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_SAVING_AMOUNT = Math.Round(totalSavingAmtSum / 10000000, 2);

                var totalDemotionSum = db.Database.SqlQuery<int>("SELECT NVL(SUM(DEMOTION), 0) AS TOTAL_DEMOTION  from AUDIT_REPORT WHERE ACT = 1 AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_DEMOTION = totalDemotionSum;

                var totalDevelopmentSum = db.Database.SqlQuery<decimal>("SELECT NVL(SUM(DEVELOPMENT), 0) AS TOTAL_DEVELOPMENT  from AUDIT_REPORT WHERE ACT = 1 AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_DEVELOPMENT = Math.Round(totalDevelopmentSum / 10000000, 2);

                var totalJobSackOutSum = db.Database.SqlQuery<int>("SELECT NVL(SUM(JOB_SACK_OUT), 0) AS TOTAL_JOB_SACK_OUT from AUDIT_REPORT WHERE ACT = 1 AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_JOB_SACK_OUT = totalJobSackOutSum;

                var totalTransferSum = db.Database.SqlQuery<int>("SELECT NVL(SUM(TRANSFER), 0) AS TOTAL_TRANSFER from AUDIT_REPORT WHERE ACT = 1 AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_TRANSFER = totalTransferSum;

                var totalWarningLatterSum = db.Database.SqlQuery<int>("SELECT NVL(SUM(WARNING_LETTER), 0) AS TOTAL_WARNING_LETTER from AUDIT_REPORT WHERE ACT = 1 AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_WARNING_LETTER = totalWarningLatterSum;

                var totalPenaltySum = db.Database.SqlQuery<decimal>("SELECT  NVL(SUM(PENALTY), 0) AS TOTAL_PENALTY from AUDIT_REPORT WHERE ACT = 1 AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_PENALTY = totalPenaltySum;


                var totalHoldPaymentSum = db.Database.SqlQuery<decimal>("SELECT  NVL(SUM(HOLD_PAYMENT), 0) AS TOTAL_HOLD_PAYMENT from AUDIT_REPORT WHERE ACT = 1 AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_HOLD_PAYMENT = Math.Round(totalHoldPaymentSum / 100000, 2);

                var totalSupplierBlackListSum = 0;
                _UD.TOTAL_SUPPLIER_BLACKLIST = totalSupplierBlackListSum;

                //var totalSupplierBlackListSum = db.Database.SqlQuery<int>("SELECT  NVL(SUM(SUPPLIER_BLACKLIST), 0) AS TOTAL_SUPPLIER_BLACKLIST from AUDIT_REPORT WHERE ACT = 1").FirstOrDefault();
                //_UD.TOTAL_SUPPLIER_BLACKLIST = totalSupplierBlackListSum;

                var totalInefficiencyIdentifiedSum = db.Database.SqlQuery<decimal>("SELECT  NVL(SUM(INEFFICIENCY_IDENTIFIED), 0) AS TOTAL_INEFFICIENCY_IDENTIFIED from AUDIT_REPORT WHERE ACT = 1 AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_INEFFICIENCY_IDENTIFIED = Math.Round(totalInefficiencyIdentifiedSum / 10000000, 2);

                var totalCorruptionIdentifiedSum = db.Database.SqlQuery<decimal>("SELECT  NVL(SUM(CORRUPTION_IDENTIFIED), 0) AS TOTAL_CORRUPTION_IDENTIFIED from AUDIT_REPORT WHERE ACT = 1 AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_CORRUPTION_IDENTIFIED = Math.Round(totalCorruptionIdentifiedSum / 10000000, 2);

                var totalWorkScopeSum = db.Database.SqlQuery<decimal>("SELECT  NVL(SUM(WORK_SCOPE), 0) AS TOTAL_WORK_SCOPE from AUDIT_REPORT WHERE ACT=1 AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_WORK_SCOPE = Math.Round(totalWorkScopeSum / 10000000, 2);

                var totalReportSentCount = db.Database.SqlQuery<int>("SELECT  COUNT(AUDIT_RPT_ID) AS TOTAL_REPORT_SENT FROM AUDIT_REPORT WHERE ACT = 1 AND RPT_SENT_DATE IS NOT NULL AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_REPORT_SENT = totalReportSentCount;




                _UD.TASK_STATUS_DATE = KSA.Where(x => x.SV_TABLE == "AUDIT_TASK_STATUS_DATA").First().UDT;
                _UD.REPORT_STATUS_DATE = KSA.Where(x => x.SV_TABLE == "AUDIT_REPORT_STATUS").First().UDT;
                _UD.REPORT_SENT_STATUS_DATE = KSA.Where(x => x.SV_TABLE == "AUDIT_REPORT_SENT_SUM").First().UDT;
                _UD.COMPANY_GROUP_DATE = KSA.Where(x => x.SV_TABLE == "COMPAY_GROUP_WISE_REPORT_SUM").First().UDT;
                _UD.CURRENT_LAST_MONTH_DATE = KSA.Where(x => x.SV_TABLE == "CURRENT_LAST_MONTH_REPORT_SUM").First().UDT;


                SummaryViewKSA _SummaryViewKSA = new SummaryViewKSA();
                _SummaryViewKSA.AuditorWiseAuditReportCount = AuditorWiseAuditReportCountData(AuditorWiseAuditReportCountSQL);
                _SummaryViewKSA.ReportSummary = ReportSummaryData(ReportSummarySQL);
                _SummaryViewKSA.CategoryCGWiseTotalReport = GetTop5Category(CategoryCGWiseTotalReportSQL);
                //var CategoryWiseWorkScopeTotalReportData = db.Database.SqlQuery<CategoryWiseWorkScopeTotalReport>(CategoryWiseWorkScopeTotalReportSQL).ToList();
                _SummaryViewKSA.CategoryWiseWorkScopeTotalReport = GetTopWorkScope5Category(CategoryWiseWorkScopeTotalReportSQL);

                //var totalCorruptionIdentifiedSum = db.Database.SqlQuery<decimal>("SELECT  NVL(SUM(CORRUPTION_IDENTIFIED), 0) AS TOTAL_CORRUPTION_IDENTIFIED from AUDIT_REPORT WHERE ACT = 1").FirstOrDefault();
                //_UD.TOTAL_CORRUPTION_IDENTIFIED = Math.Round(totalCorruptionIdentifiedSum / 10000000, 2);

                _SummaryViewKSA.BUCGWiseTotalReport = GetTop5BU(BUCGWiseTotalReportSQL);
                _SummaryViewKSA.BUCGWiseTotalWorkScopeReport = GetTop5WorkScopeBU(BUCGWiseTotalWorkScopeReportSQL);

                _SummaryViewKSA.UserRole = T;





                _SummaryViewKSA.updateDate = _UD;

                return View(_SummaryViewKSA);
            }
            else
            {
                string AuditorWiseAuditReportCountSQL = @"SELECT  A.AUDITOR_STAFF_ID, A.AUDITOR_NAME, 
                        COUNT(R.AUDIT_RPT_ID)                             AS TOTAL_REPORT,
                         COUNT (
                         CASE
                             WHEN     AT.REPORT_CREATE_STATUS = 1
                                  AND AT.REPORT_STATUS_ID IN (1, 3)
                                  AND R.REPORT_STATUS_ID IN (1, 3)
                             THEN
                                 1
                         END)       AS TOTAL_WITH_REPORT,
                         
                         COUNT (
                          CASE
                              WHEN     AT.REPORT_CREATE_STATUS = 1
                                   AND AT.REPORT_STATUS_ID IN (2, 4)
                                   AND R.REPORT_STATUS_ID IN (2, 4)
                              THEN
                                  1
                          END)       AS TOTAL_WITHOUT_REPORT,
                        ROUND(NVL(SUM(R.WORK_SCOPE), 0) / 10000000, 2)  AS TOTAL_WORK_SCOPE,
                        ROUND(NVL(SUM(R.DIRECT_SAVING_AMOUNT), 0) / 10000000, 2)   AS TOTAL_DIRECT_SAVING_AMOUNT,
                        ROUND(NVL(SUM(R.INDIRECT_SAVING_AMOUNT), 0)  / 10000000, 2)   AS TOTAL_INDIRECT_SAVING_AMOUNT,
                        NVL(SUM(R.DEMOTION), 0)                           AS TOTAL_DEMOTION,
                        NVL(SUM(R.DEVELOPMENT), 0)                        AS TOTAL_DEVELOPMENT,
                        NVL(SUM(R.JOB_SACK_OUT), 0)                       AS TOTAL_JOB_SACK_OUT,
                        NVL(SUM(R.TRANSFER), 0)                           AS TOTAL_TRANSFER,
                        NVL(SUM(R.WARNING_LETTER), 0)                     AS TOTAL_WARNING_LETTER,
                        NVL(SUM(R.PENALTY), 0)                            AS TOTAL_PENALTY,
                        NVL(SUM(R.HOLD_PAYMENT), 0)                       AS TOTAL_HOLD_PAYMENT,
                        NVL(SUM(R.INEFFICIENCY_IDENTIFIED), 0)            AS TOTAL_INEFFICIENCY_IDENTIFIED,
                        NVL(SUM(R.CORRUPTION_IDENTIFIED), 0)              AS TOTAL_CORRUPTION_IDENTIFIED
                        FROM AUDIT_REPORT R 
                        LEFT JOIN AUDIT_AUDITOR A ON R.CDU = A.AUDITOR_STAFF_ID
                        LEFT JOIN AUDIT_TASK AT ON R.TASK_ID = AT.TASK_ID
                        WHERE R.ACT = 1 AND R.CDU='" + U + @"' 

                         AND EXTRACT(YEAR FROM R.CDT) =  '" + selectedYear + @"'

                        GROUP BY A.AUDITOR_STAFF_ID, A.AUDITOR_NAME
                        ORDER BY TOTAL_REPORT DESC";

                //string ReportSummarySQL = @"SELECT CDU, COMPANY_GROUP_ID,COMPANY_GROUP_NAME,TOTAL_NO_OF_REPORT,LAST_MONTH_TOTAL_REPORT,THIS_MONTH_TOTAL_REPORT,
                //                           TOTAL_WITH_REPORT,LAST_MONTH_WITH_REPORT,THIS_MONTH_WITH_REPORT,
                //                           TOTAL_WITHOUT_REPORT,LAST_MONTH_WITHOUT_REPORT,THIS_MONTH_WITHOUT_REPORT
                //                           FROM VW_ADT_CG_AUDITOR_RPT WHERE CDU='" + U + @"' ";
                string ReportSummarySQL = @"SELECT CG.COMPANY_GROUP_ID,
                                             CG.COMPANY_GROUP_NAME,
                                             R.CDU,
                                             COUNT (CASE
                                                        WHEN     AT.REPORT_CREATE_STATUS = 1
                                                             AND AT.REPORT_STATUS_ID IN (1,
                                                                                         2,
                                                                                         3,
                                                                                         4)
                                                        THEN
                                                            1
                                                    END)    AS TOTAL_NO_OF_REPORT,
                                             COUNT (
                                                 CASE
                                                     WHEN     AT.REPORT_CREATE_STATUS = 1
                                                          AND AT.REPORT_STATUS_ID IN (1,
                                                                                      2,
                                                                                      3,
                                                                                      4)
                                                          AND R.CDT >= ADD_MONTHS (TRUNC (SYSDATE, 'MM'), -1)
                                                          AND R.CDT < TRUNC (SYSDATE, 'MM')
                                                     THEN
                                                         1
                                                 END)       AS LAST_MONTH_TOTAL_REPORT,
                                             COUNT (CASE
                                                        WHEN     AT.REPORT_CREATE_STATUS = 1
                                                             AND AT.REPORT_STATUS_ID IN (1,
                                                                                         2,
                                                                                         3,
                                                                                         4)
                                                             AND R.CDT >= TRUNC (SYSDATE, 'MM')
                                                             AND R.CDT < ADD_MONTHS (TRUNC (SYSDATE, 'MM'), 1)
                                                        THEN
                                                            1
                                                    END)    AS THIS_MONTH_TOTAL_REPORT,
                                             COUNT (
                                                 CASE
                                                     WHEN     AT.REPORT_CREATE_STATUS = 1
                                                          AND AT.REPORT_STATUS_ID IN (1, 3)
                                                          AND R.REPORT_STATUS_ID IN (1, 3)
                                                     THEN
                                                         1
                                                 END)       AS TOTAL_WITH_REPORT,
                                             COUNT (
                                                 CASE
                                                     WHEN     AT.REPORT_CREATE_STATUS = 1
                                                          AND AT.REPORT_STATUS_ID IN (1, 3)
                                                          AND R.REPORT_STATUS_ID IN (1, 3)
                                                          AND R.CDT >= ADD_MONTHS (TRUNC (SYSDATE, 'MM'), -1)
                                                          AND R.CDT < TRUNC (SYSDATE, 'MM')
                                                     THEN
                                                         1
                                                 END)       AS LAST_MONTH_WITH_REPORT,
                                             COUNT (
                                                 CASE
                                                     WHEN     AT.REPORT_CREATE_STATUS = 1
                                                          AND AT.REPORT_STATUS_ID IN (1, 3)
                                                          AND R.REPORT_STATUS_ID IN (1, 3)
                                                          AND R.CDT >= TRUNC (SYSDATE, 'MM')
                                                          AND R.CDT < ADD_MONTHS (TRUNC (SYSDATE, 'MM'), 1)
                                                     THEN
                                                         1
                                                 END)       AS THIS_MONTH_WITH_REPORT,
                                             COUNT (
                                                 CASE
                                                     WHEN     AT.REPORT_CREATE_STATUS = 1
                                                          AND AT.REPORT_STATUS_ID IN (2, 4)
                                                          AND R.REPORT_STATUS_ID IN (2, 4)
                                                     THEN
                                                         1
                                                 END)       AS TOTAL_WITHOUT_REPORT,
                                             COUNT (
                                                 CASE
                                                     WHEN     AT.REPORT_CREATE_STATUS = 1
                                                          AND AT.REPORT_STATUS_ID IN (2, 4)
                                                          AND R.REPORT_STATUS_ID IN (2, 4)
                                                          AND R.CDT >= ADD_MONTHS (TRUNC (SYSDATE, 'MM'), -1)
                                                          AND R.CDT < TRUNC (SYSDATE, 'MM')
                                                     THEN
                                                         1
                                                 END)       AS LAST_MONTH_WITHOUT_REPORT,
                                             COUNT (
                                                 CASE
                                                     WHEN     AT.REPORT_CREATE_STATUS = 1
                                                          AND AT.REPORT_STATUS_ID IN (2, 4)
                                                          AND R.REPORT_STATUS_ID IN (2, 4)
                                                          AND R.CDT >= TRUNC (SYSDATE, 'MM')
                                                          AND R.CDT < ADD_MONTHS (TRUNC (SYSDATE, 'MM'), 1)
                                                     THEN
                                                         1
                                                 END)       AS THIS_MONTH_WITHOUT_REPORT
                                        FROM AUDIT_REPORT R
                                             LEFT JOIN AUDIT_TASK AT ON R.TASK_ID = AT.TASK_ID
                                             INNER JOIN C_COMPANY_GROUP CG
                                                 ON CG.COMPANY_GROUP_ID = AT.COMPANY_GROUP_ID
                                       WHERE R.ACT = 1 AND R.CDU='" + U + @"' AND EXTRACT(YEAR FROM R.CDT) =  '" + selectedYear + @"'
                                    GROUP BY CG.COMPANY_GROUP_ID, CG.COMPANY_GROUP_NAME, R.CDU
                                    ORDER BY CG.COMPANY_GROUP_ID";

                string CategoryCGWiseTotalReportSQL = @"SELECT *
                                            FROM (
                                                SELECT 
                                                    TA.TASK_CATEGORY_NAME,
                                                    COUNT(R.AUDIT_RPT_ID) AS TOTAL_REPORT_CATEGORY,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '1' THEN 1 ELSE 0 END) AS PRAN,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '2' THEN 1 ELSE 0 END) AS RFL,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '3' THEN 1 ELSE 0 END) AS CTL,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '4' THEN 1 ELSE 0 END) AS PDL,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '5' THEN 1 ELSE 0 END) AS GROUP_CS
                                                FROM 
                                                    AUDIT_REPORT R
                                                LEFT JOIN 
                                                    AUDIT_TASK_CATEGORY TA 
                                                    ON R.TASK_CATEGORY_ID = TA.TASK_CATEGORY_ID
                                                LEFT JOIN 
                                                    AUDIT_TASK AT 
                                                    ON R.TASK_ID = AT.TASK_ID
                                                INNER JOIN 
                                                    C_COMPANY_GROUP CG 
                                                    ON CG.COMPANY_GROUP_ID = AT.COMPANY_GROUP_ID
                                                WHERE 
                                                    R.ACT = 1 AND R.CDU='" + U + @"'  AND EXTRACT(YEAR FROM R.CDT) =  '" + selectedYear + @"'
                                                GROUP BY 
                                                    TA.TASK_CATEGORY_NAME
                                                ORDER BY 
                                                    TOTAL_REPORT_CATEGORY DESC
                                            )WHERE ROWNUM <= 5";

                string CategoryWiseWorkScopeTotalReportSQL = @"SELECT *
                                            FROM (
                                                SELECT 
                                                    TA.TASK_CATEGORY_NAME,
                                                    ROUND(NVL(SUM(R.WORK_SCOPE), 0) / 10000000, 2) AS TOTAL_WORK_SCOPE,
                                                    ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '1' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS PRAN,
                                                    ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '2' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS RFL,
                                                    ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '3' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS CTL,
                                                    ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '4' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS PDL,
                                                    ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '5' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS GROUP_CS
                                                FROM 
                                                    AUDIT_REPORT R
                                                LEFT JOIN 
                                                    AUDIT_TASK_CATEGORY TA 
                                                    ON R.TASK_CATEGORY_ID = TA.TASK_CATEGORY_ID
                                                LEFT JOIN 
                                                    AUDIT_TASK AT 
                                                    ON R.TASK_ID = AT.TASK_ID
                                                INNER JOIN 
                                                    C_COMPANY_GROUP CG 
                                                    ON CG.COMPANY_GROUP_ID = AT.COMPANY_GROUP_ID
                                                WHERE 
                                                    R.ACT = 1 AND R.CDU='" + U + @"'  AND EXTRACT(YEAR FROM R.CDT) =  '" + selectedYear + @"'
                                                GROUP BY 
                                                    TA.TASK_CATEGORY_NAME
                                                ORDER BY 
                                                    TOTAL_WORK_SCOPE DESC
                                            )
                                            WHERE ROWNUM <= 5";
                string BUCGWiseTotalReportSQL = @"SELECT *
                                            FROM (
                                                SELECT 
                                                    C.COMPANY_NAME,
                                                    COUNT(R.AUDIT_RPT_ID) AS TOTAL_REPORT_BU,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '1' THEN 1 ELSE 0 END) AS PRAN,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '2' THEN 1 ELSE 0 END) AS RFL,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '3' THEN 1 ELSE 0 END) AS CTL,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '4' THEN 1 ELSE 0 END) AS PDL,
                                                    SUM(CASE WHEN AT.COMPANY_GROUP_ID = '5' THEN 1 ELSE 0 END) AS GROUP_CS
                                                FROM 
                                                    AUDIT_REPORT R
                                                LEFT JOIN 
                                                    AUDIT_TASK AT 
                                                    ON R.TASK_ID = AT.TASK_ID
                                              INNER JOIN 
                                                    C_COMPANY_GROUP CG 
                                                    ON CG.COMPANY_GROUP_ID = AT.COMPANY_GROUP_ID
                                                INNER JOIN 
                                                     C_COMPANY C ON C.COMPANY_ID = AT.COMPANY_ID
                                                WHERE 
                                                    R.ACT = 1 AND R.CDU='" + U + @"'  AND EXTRACT(YEAR FROM R.CDT) =  '" + selectedYear + @"'
                                                GROUP BY 
                                                     C.COMPANY_NAME
                                                ORDER BY 
                                                    TOTAL_REPORT_BU DESC
                                            )
                                            WHERE ROWNUM <= 5";

                string BUCGWiseTotalWorkScopeReportSQL = @"SELECT *
                                                    FROM (
                                                        SELECT 
                                                            C.COMPANY_NAME,
                                                            ROUND(NVL(SUM(R.WORK_SCOPE), 0) / 10000000, 2)  AS TOTAL_WORK_SCOPE,
                                                            ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '1' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS PRAN,
                                                            ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '2' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS RFL,
                                                            ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '3' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS CTL,
                                                            ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '4' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS PDL,
                                                            ROUND(SUM(CASE WHEN AT.COMPANY_GROUP_ID = '5' THEN R.WORK_SCOPE ELSE 0 END) / 10000000, 2) AS GROUP_CS
                                                        FROM 
                                                            AUDIT_REPORT R
                                                        LEFT JOIN 
                                                            AUDIT_TASK AT 
                                                            ON R.TASK_ID = AT.TASK_ID
                                                      INNER JOIN 
                                                            C_COMPANY_GROUP CG 
                                                            ON CG.COMPANY_GROUP_ID = AT.COMPANY_GROUP_ID
                                                        INNER JOIN 
                                                             C_COMPANY C ON C.COMPANY_ID = AT.COMPANY_ID
                                                        WHERE 
                                                            R.ACT = 1 AND R.CDU='" + U + @"'  AND EXTRACT(YEAR FROM R.CDT) =  '" + selectedYear + @"'
                                                        GROUP BY 
                                                             C.COMPANY_NAME
                                                        ORDER BY 
                                                            TOTAL_WORK_SCOPE DESC
                                                    )
                                                    WHERE ROWNUM <= 5";

                List<A_SUMMARY_VIEW> KSA = db.A_SUMMARY_VIEW.Where(x => x.SV_USER == "AUDIT").ToList();
                Update_Date _UD = new Update_Date();

                var totalTaskCount = db.Database.SqlQuery<int>("SELECT COUNT(TASK_ID) AS TOTAL_TASK FROM AUDIT_TASK WHERE ACT = 1 AND CDU= '" + U + @"'  AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"' ").FirstOrDefault();
                _UD.TOTAL_TASK = totalTaskCount;

                var totalAuditReportCount = db.Database.SqlQuery<int>("SELECT COUNT(AUDIT_RPT_ID) AS TOTAL_REPORT FROM AUDIT_REPORT WHERE ACT=1 AND CDU= '" + U + @"' AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"' ").FirstOrDefault();
                _UD.TOTAL_AUDIT_REPORT = totalAuditReportCount;
                var totalAuditorCount = db.Database.SqlQuery<int>("SELECT COUNT(AUDITOR_ID) AS TOTAL_AUDITOR  from AUDIT_AUDITOR WHERE ACT=1").FirstOrDefault();
                _UD.TOTAL_AUDITOR = totalAuditorCount;

                var totalDirectSavingAmtSum = db.Database.SqlQuery<decimal>("SELECT  NVL(SUM(DIRECT_SAVING_AMOUNT), 0) AS TOTAL_DIRECT_SAVING_AMOUNT from AUDIT_REPORT WHERE ACT=1 AND CDU= '" + U + @"' AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_DIRECT_SAVING_AMOUNT = Math.Round(totalDirectSavingAmtSum / 10000000, 2);

                var totalInDirectSavingAmtSum = db.Database.SqlQuery<decimal>("SELECT  NVL(SUM(INDIRECT_SAVING_AMOUNT), 0) AS TOTAL_INDIRECT_SAVING_AMOUNT from AUDIT_REPORT WHERE ACT=1 AND CDU= '" + U + @"' AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"' ").FirstOrDefault();
                _UD.TOTAL_INDIRECT_SAVING_AMOUNT = Math.Round(totalInDirectSavingAmtSum / 10000000, 2);

                var totalSavingAmtSum = db.Database.SqlQuery<decimal>("SELECT (NVL(SUM(DIRECT_SAVING_AMOUNT), 0) + NVL(SUM(INDIRECT_SAVING_AMOUNT), 0)) AS TOTAL_SAVING_AMOUNT from AUDIT_REPORT WHERE ACT=1 AND CDU= '" + U + @"' AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_SAVING_AMOUNT = Math.Round(totalSavingAmtSum / 10000000, 2);

                var totalDemotionSum = db.Database.SqlQuery<int>("SELECT NVL(SUM(DEMOTION), 0) AS TOTAL_DEMOTION  from AUDIT_REPORT WHERE ACT = 1 AND CDU= '" + U + @"' AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_DEMOTION = totalDemotionSum;

                var totalDevelopmentSum = db.Database.SqlQuery<decimal>("SELECT NVL(SUM(DEVELOPMENT), 0) AS TOTAL_DEVELOPMENT  from AUDIT_REPORT WHERE ACT = 1 AND CDU= '" + U + @"' AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"'").FirstOrDefault();
                _UD.TOTAL_DEVELOPMENT = Math.Round(totalDevelopmentSum / 10000000, 2);

                var totalJobSackOutSum = db.Database.SqlQuery<int>("SELECT NVL(SUM(JOB_SACK_OUT), 0) AS TOTAL_JOB_SACK_OUT from AUDIT_REPORT WHERE ACT = 1 AND CDU= '" + U + @"' AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"' ").FirstOrDefault();
                _UD.TOTAL_JOB_SACK_OUT = totalJobSackOutSum;

                var totalTransferSum = db.Database.SqlQuery<int>("SELECT NVL(SUM(TRANSFER), 0) AS TOTAL_TRANSFER from AUDIT_REPORT WHERE ACT = 1 AND CDU= '" + U + @"' AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"' ").FirstOrDefault();
                _UD.TOTAL_TRANSFER = totalTransferSum;

                var totalWarningLatterSum = db.Database.SqlQuery<int>("SELECT NVL(SUM(WARNING_LETTER), 0) AS TOTAL_WARNING_LETTER from AUDIT_REPORT WHERE ACT = 1 AND CDU= '" + U + @"' AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"' ").FirstOrDefault();
                _UD.TOTAL_WARNING_LETTER = totalWarningLatterSum;

                var totalPenaltySum = db.Database.SqlQuery<decimal>("SELECT  NVL(SUM(PENALTY), 0) AS TOTAL_PENALTY from AUDIT_REPORT WHERE ACT = 1 AND CDU= '" + U + @"' AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"' ").FirstOrDefault();
                _UD.TOTAL_PENALTY = totalPenaltySum;


                var totalHoldPaymentSum = db.Database.SqlQuery<decimal>("SELECT  NVL(SUM(HOLD_PAYMENT), 0) AS TOTAL_HOLD_PAYMENT from AUDIT_REPORT WHERE ACT = 1 AND CDU= '" + U + @"' AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"' ").FirstOrDefault();
                _UD.TOTAL_HOLD_PAYMENT = Math.Round(totalHoldPaymentSum / 100000, 2);

                var totalSupplierBlackListSum = 0;
                _UD.TOTAL_SUPPLIER_BLACKLIST = totalSupplierBlackListSum;

                //var totalSupplierBlackListSum = db.Database.SqlQuery<int>("SELECT  NVL(SUM(SUPPLIER_BLACKLIST), 0) AS TOTAL_SUPPLIER_BLACKLIST from AUDIT_REPORT WHERE ACT = 1 AND CDU= '" + U + @"'").FirstOrDefault();
                //_UD.TOTAL_SUPPLIER_BLACKLIST = totalSupplierBlackListSum;

                var totalInefficiencyIdentifiedSum = db.Database.SqlQuery<decimal>("SELECT  NVL(SUM(INEFFICIENCY_IDENTIFIED), 0) AS TOTAL_INEFFICIENCY_IDENTIFIED from AUDIT_REPORT WHERE ACT = 1 AND CDU= '" + U + @"' AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"' ").FirstOrDefault();
                _UD.TOTAL_INEFFICIENCY_IDENTIFIED = Math.Round(totalInefficiencyIdentifiedSum / 10000000, 2);

                var totalCorruptionIdentifiedSum = db.Database.SqlQuery<decimal>("SELECT  NVL(SUM(CORRUPTION_IDENTIFIED), 0) AS TOTAL_CORRUPTION_IDENTIFIED from AUDIT_REPORT WHERE ACT = 1 AND CDU= '" + U + @"' AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"' ").FirstOrDefault();
                _UD.TOTAL_CORRUPTION_IDENTIFIED = Math.Round(totalCorruptionIdentifiedSum / 10000000, 2);

                var totalWorkScopeSum = db.Database.SqlQuery<decimal>("SELECT  NVL(SUM(WORK_SCOPE), 0) AS TOTAL_WORK_SCOPE from AUDIT_REPORT WHERE ACT=1 AND CDU= '" + U + @"' AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"' ").FirstOrDefault();
                _UD.TOTAL_WORK_SCOPE = Math.Round(totalWorkScopeSum / 10000000, 2);

                var totalReportSentCount = db.Database.SqlQuery<int>("SELECT  COUNT(AUDIT_RPT_ID) AS TOTAL_REPORT_SENT FROM AUDIT_REPORT WHERE ACT = 1 AND CDU= '" + U + @"' AND RPT_SENT_DATE IS NOT NULL AND EXTRACT(YEAR FROM CDT) =  '" + selectedYear + @"' ").FirstOrDefault();
                _UD.TOTAL_REPORT_SENT = totalReportSentCount;


                _UD.TASK_STATUS_DATE = KSA.Where(x => x.SV_TABLE == "AUDIT_TASK_STATUS_DATA").First().UDT;
                _UD.REPORT_STATUS_DATE = KSA.Where(x => x.SV_TABLE == "AUDIT_REPORT_STATUS").First().UDT;
                _UD.REPORT_SENT_STATUS_DATE = KSA.Where(x => x.SV_TABLE == "AUDIT_REPORT_SENT_SUM").First().UDT;
                _UD.COMPANY_GROUP_DATE = KSA.Where(x => x.SV_TABLE == "COMPAY_GROUP_WISE_REPORT_SUM").First().UDT;
                _UD.CURRENT_LAST_MONTH_DATE = KSA.Where(x => x.SV_TABLE == "CURRENT_LAST_MONTH_REPORT_SUM").First().UDT;


                SummaryViewKSA _SummaryViewKSA = new SummaryViewKSA();
                _SummaryViewKSA.AuditorWiseAuditReportCount = AuditorWiseAuditReportCountData(AuditorWiseAuditReportCountSQL);
                _SummaryViewKSA.ReportSummary = ReportSummaryData(ReportSummarySQL);
                _SummaryViewKSA.CategoryCGWiseTotalReport = GetTop5Category(CategoryCGWiseTotalReportSQL);
                _SummaryViewKSA.CategoryWiseWorkScopeTotalReport = GetTopWorkScope5Category(CategoryWiseWorkScopeTotalReportSQL);
                _SummaryViewKSA.BUCGWiseTotalReport = GetTop5BU(BUCGWiseTotalReportSQL);
                _SummaryViewKSA.BUCGWiseTotalWorkScopeReport = GetTop5WorkScopeBU(BUCGWiseTotalWorkScopeReportSQL);

                _SummaryViewKSA.updateDate = _UD;

                return View(_SummaryViewKSA);
            }
        }

        [KAuth(13)]
        public ActionResult AUDIT_DASHBOARD_TWO()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (T == "10")
            {

                string SegmentConpamyGroupWiseAuditReportCountSQL = @"SELECT 
                    CG.COMPANY_GROUP_NAME,
                    TA.TASK_CATEGORY_NAME,
                    COUNT(R.AUDIT_RPT_ID)                             AS TOTAL_REPORT,
                    NVL(SUM(R.WORK_SCOPE), 0)                         AS TOTAL_WORK_SCOPE,
                    NVL(SUM(R.DIRECT_SAVING_AMOUNT), 0)               AS TOTAL_DIRECT_SAVING_AMOUNT,
                    NVL(SUM(R.INDIRECT_SAVING_AMOUNT), 0)             AS TOTAL_INDIRECT_SAVING_AMOUNT,
                    NVL(SUM(R.DEMOTION), 0)                           AS TOTAL_DEMOTION,
                    NVL(SUM(R.DEVELOPMENT), 0)                        AS TOTAL_DEVELOPMENT,
                    NVL(SUM(R.JOB_SACK_OUT), 0)                       AS TOTAL_JOB_SACK_OUT,
                    NVL(SUM(R.TRANSFER), 0)                           AS TOTAL_TRANSFER,
                    NVL(SUM(R.WARNING_LETTER), 0)                     AS TOTAL_WARNING_LETTER,
                    NVL(SUM(R.PENALTY), 0)                            AS TOTAL_PENALTY,
                    NVL(SUM(R.HOLD_PAYMENT), 0)                       AS TOTAL_HOLD_PAYMENT,
                    NVL(SUM(R.INEFFICIENCY_IDENTIFIED), 0)            AS TOTAL_INEFFICIENCY_IDENTIFIED,
                    NVL(SUM(R.CORRUPTION_IDENTIFIED), 0)              AS TOTAL_CORRUPTION_IDENTIFIED
                FROM 
                    AUDIT_REPORT R
                LEFT JOIN 
                    AUDIT_TASK_CATEGORY TA ON R.TASK_CATEGORY_ID = TA.TASK_CATEGORY_ID
                LEFT JOIN 
                    AUDIT_TASK AT ON R.TASK_ID = AT.TASK_ID
                INNER JOIN 
                    C_COMPANY_GROUP CG ON CG.COMPANY_GROUP_ID = AT.COMPANY_GROUP_ID
                WHERE 
                    R.ACT = 1
                GROUP BY 
                    CG.COMPANY_GROUP_NAME,
                    TA.TASK_CATEGORY_NAME
                ORDER BY 
                    CG.COMPANY_GROUP_NAME, 
                    TA.TASK_CATEGORY_NAME";

                string CompanyNameCompanyGroupWiseAuditReportCountSQL = @"
               SELECT 
                    CG.COMPANY_GROUP_NAME,
                    C.COMPANY_NAME,
                    COUNT(R.AUDIT_RPT_ID)                             AS TOTAL_REPORT,
                    NVL(SUM(R.WORK_SCOPE), 0)                         AS TOTAL_WORK_SCOPE,
                    NVL(SUM(R.DIRECT_SAVING_AMOUNT), 0)               AS TOTAL_DIRECT_SAVING_AMOUNT,
                    NVL(SUM(R.INDIRECT_SAVING_AMOUNT), 0)             AS TOTAL_INDIRECT_SAVING_AMOUNT,
                    NVL(SUM(R.DEMOTION), 0)                           AS TOTAL_DEMOTION,
                    NVL(SUM(R.DEVELOPMENT), 0)                        AS TOTAL_DEVELOPMENT,
                    NVL(SUM(R.JOB_SACK_OUT), 0)                       AS TOTAL_JOB_SACK_OUT,
                    NVL(SUM(R.TRANSFER), 0)                           AS TOTAL_TRANSFER,
                    NVL(SUM(R.WARNING_LETTER), 0)                     AS TOTAL_WARNING_LETTER,
                    NVL(SUM(R.PENALTY), 0)                            AS TOTAL_PENALTY,
                    NVL(SUM(R.HOLD_PAYMENT), 0)                       AS TOTAL_HOLD_PAYMENT,
                    NVL(SUM(R.INEFFICIENCY_IDENTIFIED), 0)            AS TOTAL_INEFFICIENCY_IDENTIFIED,
                    NVL(SUM(R.CORRUPTION_IDENTIFIED), 0)              AS TOTAL_CORRUPTION_IDENTIFIED
                FROM 
                    AUDIT_REPORT R
                
                LEFT JOIN 
                    AUDIT_TASK AT ON R.TASK_ID = AT.TASK_ID
                INNER JOIN 
                    C_COMPANY_GROUP CG ON CG.COMPANY_GROUP_ID = AT.COMPANY_GROUP_ID
                INNER JOIN 
                    C_COMPANY C ON C.COMPANY_ID = AT.COMPANY_ID
                WHERE 
                    R.ACT = 1
                GROUP BY 
                    CG.COMPANY_GROUP_NAME,
                    C.COMPANY_NAME
                ORDER BY 
                    CG.COMPANY_GROUP_NAME, 
                    C.COMPANY_NAME";

                string CompanyGroupWiseAuditReportCountSQL = @"SELECT 
                        CG.COMPANY_GROUP_NAME,
                        COUNT(CASE 
                                  WHEN AT.REPORT_CREATE_STATUS = 1 AND AT.REPORT_STATUS_ID in (1,2,3,4) 
                                  THEN 1 
                             END) AS TOTAL_REPORT,
                        COUNT(CASE 
                                  WHEN AT.REPORT_CREATE_STATUS = 1 AND AT.REPORT_STATUS_ID = 1 AND R.REPORT_STATUS_ID=1
                                  THEN 1 
                             END) AS TOTAL_COMPLETED_WITH_REPORT,
                        COUNT(CASE 
                                  WHEN AT.REPORT_CREATE_STATUS = 1 AND AT.REPORT_STATUS_ID = 2 AND R.REPORT_STATUS_ID=2
                                  THEN 1 
                             END) AS TOTAL_COMPLETED_WITHOUT_REPORT 
                        FROM AUDIT_REPORT R
                        LEFT JOIN AUDIT_TASK AT 
                            ON R.TASK_ID = AT.TASK_ID
                        INNER JOIN C_COMPANY_GROUP CG 
                            ON CG.COMPANY_GROUP_ID = AT.COMPANY_GROUP_ID
                        WHERE R.ACT = 1
                        GROUP BY CG.COMPANY_GROUP_NAME
                        ORDER BY TOTAL_REPORT DESC";

                string AuditorWiseAuditFollowUpReportCountSQL = @"SELECT 
                      AA.AUDITOR_NAME, 
                      COUNT(R.AUDIT_RPT_ID) AS TOTAL_REPORT,
                      COUNT(ARF.AUDIT_RPT_ID) AS TOTAL_FOLLOWUP,
                      SUM(CASE WHEN ARF.FOLLOWUP_NUMBER = '1' THEN 1 ELSE 0 END) AS FIRST_FOLLOWUP,
                      SUM(CASE WHEN ARF.FOLLOWUP_NUMBER = '2' THEN 1 ELSE 0 END) AS SECOND_FOLLOWUP,
                      SUM(CASE WHEN ARF.FOLLOWUP_NUMBER = '3' THEN 1 ELSE 0 END) AS THIRD_FOLLOWUP,
                      SUM(ARF.TOTAL_FINDINGS) AS TOTAL_FINDINGS,
                      SUM(ARF.ACTION_FINDINGS) AS ACTION_DONE,
                      SUM(ARF.NO_ACTION_FINDINGS) AS NO_ACTION,
                      ROUND((SUM(ARF.TOTAL_FINDINGS) - (SUM(ARF.ACTION_FINDINGS) + SUM(ARF.NO_ACTION_FINDINGS)) / COUNT(ARF.AUDIT_RPT_ID)),2) ACTION_PROGRESS,
                      ROUND(SUM(ARF.ACTION_PERCENTAGE),2) AS ACTION_PERCENTAGE
      
                  FROM 
                      AUDIT_REPORT R
                  LEFT JOIN 
                      AUDIT_AUDITOR AA 
                      ON AA.AUDITOR_STAFF_ID = R.CDU
                  LEFT JOIN AUDIT_REPORT_FOLLOWUP ARF
                      ON ARF.AUDIT_RPT_ID = R.AUDIT_RPT_ID
                  WHERE 
                      R.ACT = 1 
                  GROUP BY 
                       AA.AUDITOR_NAME
                  ORDER BY 
                      TOTAL_REPORT DESC";

                string BUWiseAuditFollowUpReportStatusCountSQL = @"  SELECT 
  
                  C.COMPANY_NAME,
                  COUNT(R.AUDIT_RPT_ID) AS TOTAL_REPORT,
                  COUNT(ARF.AUDIT_RPT_ID) AS TOTAL_FOLLOWUP,
                  SUM(CASE WHEN ARF.FOLLOWUP_NUMBER = '1' THEN 1 ELSE 0 END) AS FIRST_FOLLOWUP,
                  SUM(CASE WHEN ARF.FOLLOWUP_NUMBER = '2' THEN 1 ELSE 0 END) AS SECOND_FOLLOWUP,
                  SUM(CASE WHEN ARF.FOLLOWUP_NUMBER = '3' THEN 1 ELSE 0 END) AS THIRD_FOLLOWUP,
                  SUM(ARF.TOTAL_FINDINGS) AS TOTAL_FINDINGS,
                  SUM(ARF.ACTION_FINDINGS) AS ACTION_DONE,
                  SUM(ARF.NO_ACTION_FINDINGS) AS NO_ACTION,
                  ROUND((SUM(ARF.TOTAL_FINDINGS) - (SUM(ARF.ACTION_FINDINGS)+SUM(ARF.NO_ACTION_FINDINGS)) / COUNT(ARF.AUDIT_RPT_ID)),2) ACTION_PROGRESS,
                  ROUND(SUM(ARF.ACTION_PERCENTAGE),2) AS ACTION_PERCENTAGE
      
              FROM 
                  AUDIT_REPORT R
              LEFT JOIN 
                  AUDIT_TASK AT 
                  ON R.TASK_ID = AT.TASK_ID
              LEFT JOIN AUDIT_REPORT_FOLLOWUP ARF
                  ON ARF.AUDIT_RPT_ID = R.AUDIT_RPT_ID
              INNER JOIN 
                  C_COMPANY_GROUP CG 
                  ON CG.COMPANY_GROUP_ID = AT.COMPANY_GROUP_ID
              INNER JOIN 
                   C_COMPANY C ON C.COMPANY_ID = AT.COMPANY_ID
              WHERE 
                  R.ACT = 1 
              GROUP BY 
                   C.COMPANY_NAME
              ORDER BY 
                  TOTAL_REPORT DESC";


                List<A_SUMMARY_VIEW> KSA = db.A_SUMMARY_VIEW.Where(x => x.SV_USER == "AUDIT").ToList();
                Update_Date _UD = new Update_Date();

 
                _UD.TASK_STATUS_DATE = KSA.Where(x => x.SV_TABLE == "AUDIT_TASK_STATUS_DATA").First().UDT;
                _UD.REPORT_STATUS_DATE = KSA.Where(x => x.SV_TABLE == "AUDIT_REPORT_STATUS").First().UDT;
                _UD.REPORT_SENT_STATUS_DATE = KSA.Where(x => x.SV_TABLE == "AUDIT_REPORT_SENT_SUM").First().UDT;
                _UD.COMPANY_GROUP_DATE = KSA.Where(x => x.SV_TABLE == "COMPAY_GROUP_WISE_REPORT_SUM").First().UDT;
                _UD.CURRENT_LAST_MONTH_DATE = KSA.Where(x => x.SV_TABLE == "CURRENT_LAST_MONTH_REPORT_SUM").First().UDT;


                SummaryViewKSA _SummaryViewKSA = new SummaryViewKSA();
                _SummaryViewKSA.SegmentConpamyGroupWiseAuditReportCount = SegmentConpamyGroupWiseAuditReportCountData(SegmentConpamyGroupWiseAuditReportCountSQL);
                _SummaryViewKSA.CompanyNameCompanyGroupWiseAuditReportCount = CompanyNameCompanyGroupWiseAuditReportCountData(CompanyNameCompanyGroupWiseAuditReportCountSQL);
                _SummaryViewKSA.CompanyGroupWiseAuditReportCount = CompanyGroupWiseAuditReportCountData(CompanyGroupWiseAuditReportCountSQL);
                _SummaryViewKSA.AuditorWiseAuditFollowUpReportCount = AuditorWiseAuditFollowUpReportCountData(AuditorWiseAuditFollowUpReportCountSQL);
                _SummaryViewKSA.BUWiseAuditFollowUpReportStatusCount = BUWiseAuditFollowUpReportStatusCountData(BUWiseAuditFollowUpReportStatusCountSQL);

                _SummaryViewKSA.updateDate = _UD;

                return View(_SummaryViewKSA);
            }
            else
            {

                string SegmentConpamyGroupWiseAuditReportCountSQL = @"SELECT 
                    CG.COMPANY_GROUP_NAME,
                    TA.TASK_CATEGORY_NAME,
                    COUNT(R.AUDIT_RPT_ID)                             AS TOTAL_REPORT,
                    NVL(SUM(R.WORK_SCOPE), 0)                         AS TOTAL_WORK_SCOPE,
                    NVL(SUM(R.DIRECT_SAVING_AMOUNT), 0)               AS TOTAL_DIRECT_SAVING_AMOUNT,
                    NVL(SUM(R.INDIRECT_SAVING_AMOUNT), 0)             AS TOTAL_INDIRECT_SAVING_AMOUNT,
                    NVL(SUM(R.DEMOTION), 0)                           AS TOTAL_DEMOTION,
                    NVL(SUM(R.DEVELOPMENT), 0)                        AS TOTAL_DEVELOPMENT,
                    NVL(SUM(R.JOB_SACK_OUT), 0)                       AS TOTAL_JOB_SACK_OUT,
                    NVL(SUM(R.TRANSFER), 0)                           AS TOTAL_TRANSFER,
                    NVL(SUM(R.WARNING_LETTER), 0)                     AS TOTAL_WARNING_LETTER,
                    NVL(SUM(R.PENALTY), 0)                            AS TOTAL_PENALTY,
                    NVL(SUM(R.HOLD_PAYMENT), 0)                       AS TOTAL_HOLD_PAYMENT,
                    NVL(SUM(R.SUPPLIER_BLACKLIST), 0)                 AS TOTAL_SUPPLIER_BLACKLIST,
                    NVL(SUM(R.INEFFICIENCY_IDENTIFIED), 0)            AS TOTAL_INEFFICIENCY_IDENTIFIED,
                    NVL(SUM(R.CORRUPTION_IDENTIFIED), 0)              AS TOTAL_CORRUPTION_IDENTIFIED
                FROM 
                    AUDIT_REPORT R
                LEFT JOIN 
                    AUDIT_TASK_CATEGORY TA ON R.TASK_CATEGORY_ID = TA.TASK_CATEGORY_ID
                LEFT JOIN 
                    AUDIT_TASK AT ON R.TASK_ID = AT.TASK_ID
                INNER JOIN 
                    C_COMPANY_GROUP CG ON CG.COMPANY_GROUP_ID = AT.COMPANY_GROUP_ID
                WHERE 
                    R.ACT = 1
                GROUP BY 
                    CG.COMPANY_GROUP_NAME,
                    TA.TASK_CATEGORY_NAME
                ORDER BY 
                    CG.COMPANY_GROUP_NAME, 
                    TA.TASK_CATEGORY_NAME";

                string CompanyNameCompanyGroupWiseAuditReportCountSQL = @"
               SELECT 
                    CG.COMPANY_GROUP_NAME,
                    C.COMPANY_NAME,
                    COUNT(R.AUDIT_RPT_ID)                             AS TOTAL_REPORT,
                    NVL(SUM(R.WORK_SCOPE), 0)                         AS TOTAL_WORK_SCOPE,
                    NVL(SUM(R.DIRECT_SAVING_AMOUNT), 0)               AS TOTAL_DIRECT_SAVING_AMOUNT,
                    NVL(SUM(R.INDIRECT_SAVING_AMOUNT), 0)             AS TOTAL_INDIRECT_SAVING_AMOUNT,
                    NVL(SUM(R.DEMOTION), 0)                           AS TOTAL_DEMOTION,
                    NVL(SUM(R.DEVELOPMENT), 0)                        AS TOTAL_DEVELOPMENT,
                    NVL(SUM(R.JOB_SACK_OUT), 0)                       AS TOTAL_JOB_SACK_OUT,
                    NVL(SUM(R.TRANSFER), 0)                           AS TOTAL_TRANSFER,
                    NVL(SUM(R.WARNING_LETTER), 0)                     AS TOTAL_WARNING_LETTER,
                    NVL(SUM(R.PENALTY), 0)                            AS TOTAL_PENALTY,
                    NVL(SUM(R.HOLD_PAYMENT), 0)                       AS TOTAL_HOLD_PAYMENT,
                    NVL(SUM(R.SUPPLIER_BLACKLIST), 0)                 AS TOTAL_SUPPLIER_BLACKLIST,
                    NVL(SUM(R.INEFFICIENCY_IDENTIFIED), 0)            AS TOTAL_INEFFICIENCY_IDENTIFIED,
                    NVL(SUM(R.CORRUPTION_IDENTIFIED), 0)              AS TOTAL_CORRUPTION_IDENTIFIED
                FROM 
                    AUDIT_REPORT R
                
                LEFT JOIN 
                    AUDIT_TASK AT ON R.TASK_ID = AT.TASK_ID
                INNER JOIN 
                    C_COMPANY_GROUP CG ON CG.COMPANY_GROUP_ID = AT.COMPANY_GROUP_ID
                INNER JOIN 
                    C_COMPANY C ON C.COMPANY_ID = AT.COMPANY_ID
                WHERE 
                    R.ACT = 1
                GROUP BY 
                    CG.COMPANY_GROUP_NAME,
                    C.COMPANY_NAME
                ORDER BY 
                    CG.COMPANY_GROUP_NAME, 
                    C.COMPANY_NAME";

                string CompanyGroupWiseAuditReportCountSQL = @"SELECT 
                        CG.COMPANY_GROUP_NAME,
                        COUNT(CASE 
                                  WHEN AT.REPORT_CREATE_STATUS = 1 AND AT.REPORT_STATUS_ID in (1,2,3,4) 
                                  THEN 1 
                             END) AS TOTAL_REPORT,
                        COUNT(CASE 
                                  WHEN AT.REPORT_CREATE_STATUS = 1 AND AT.REPORT_STATUS_ID = 1 AND R.REPORT_STATUS_ID=1
                                  THEN 1 
                             END) AS TOTAL_COMPLETED_WITH_REPORT,
                        COUNT(CASE 
                                  WHEN AT.REPORT_CREATE_STATUS = 1 AND AT.REPORT_STATUS_ID = 2 AND R.REPORT_STATUS_ID=2
                                  THEN 1 
                             END) AS TOTAL_COMPLETED_WITHOUT_REPORT 
                        FROM AUDIT_REPORT R
                        LEFT JOIN AUDIT_TASK AT 
                            ON R.TASK_ID = AT.TASK_ID
                        INNER JOIN C_COMPANY_GROUP CG 
                            ON CG.COMPANY_GROUP_ID = AT.COMPANY_GROUP_ID
                        WHERE R.ACT = 1
                        GROUP BY CG.COMPANY_GROUP_NAME
                        ORDER BY TOTAL_REPORT DESC";

                List<A_SUMMARY_VIEW> KSA = db.A_SUMMARY_VIEW.Where(x => x.SV_USER == "AUDIT").ToList();
                Update_Date _UD = new Update_Date();

                _UD.TASK_STATUS_DATE = KSA.Where(x => x.SV_TABLE == "AUDIT_TASK_STATUS_DATA").First().UDT;
                _UD.REPORT_STATUS_DATE = KSA.Where(x => x.SV_TABLE == "AUDIT_REPORT_STATUS").First().UDT;
                _UD.REPORT_SENT_STATUS_DATE = KSA.Where(x => x.SV_TABLE == "AUDIT_REPORT_SENT_SUM").First().UDT;
                _UD.COMPANY_GROUP_DATE = KSA.Where(x => x.SV_TABLE == "COMPAY_GROUP_WISE_REPORT_SUM").First().UDT;
                _UD.CURRENT_LAST_MONTH_DATE = KSA.Where(x => x.SV_TABLE == "CURRENT_LAST_MONTH_REPORT_SUM").First().UDT;


                SummaryViewKSA _SummaryViewKSA = new SummaryViewKSA();

                _SummaryViewKSA.SegmentConpamyGroupWiseAuditReportCount = SegmentConpamyGroupWiseAuditReportCountData(SegmentConpamyGroupWiseAuditReportCountSQL);
                _SummaryViewKSA.CompanyNameCompanyGroupWiseAuditReportCount = CompanyNameCompanyGroupWiseAuditReportCountData(CompanyNameCompanyGroupWiseAuditReportCountSQL);
                _SummaryViewKSA.CompanyGroupWiseAuditReportCount = CompanyGroupWiseAuditReportCountData(CompanyGroupWiseAuditReportCountSQL);

                _SummaryViewKSA.updateDate = _UD;

                return View(_SummaryViewKSA);
            }
        }

        [HttpGet]
        public ActionResult ViewCategoryWisetDetails(string id, string Post)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}

            //var reportData = (
            //         from r in db.AUDIT_REPORT
            //         join ta in db.AUDIT_TASK_CATEGORY on r.TASK_CATEGORY_ID equals ta.TASK_CATEGORY_ID into taJoin
            //         from ta in taJoin.DefaultIfEmpty()
            //         join at in db.AUDIT_TASK on r.TASK_ID equals at.TASK_ID into atJoin
            //         from at in atJoin.DefaultIfEmpty()
            //         join cg in db.C_COMPANY_GROUP on at.COMPANY_GROUP_ID equals cg.COMPANY_GROUP_ID

            //         where r.ACT == true

            //         group r by new { ta.TASK_CATEGORY_ID, ta.TASK_CATEGORY_NAME } into g

            //         orderby g.Count() descending

            //         select new CategoryCGWiseDetails
            //         {
            //             TASK_CATEGORY_ID = g.Key.TASK_CATEGORY_ID,
            //             TASK_CATEGORY_NAME = g.Key.TASK_CATEGORY_NAME,
            //             TOTAL_REPORT_CATEGORY = g.Count()
            //         }).ToList();

            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            if (T == "10")
            {
                var preGroupData = (
                        from r in db.AUDIT_REPORT
                        join ta in db.AUDIT_TASK_CATEGORY on r.TASK_CATEGORY_ID equals ta.TASK_CATEGORY_ID into taJoin
                        from ta in taJoin.DefaultIfEmpty()
                        join at in db.AUDIT_TASK on r.TASK_ID equals at.TASK_ID into atJoin
                        from at in atJoin.DefaultIfEmpty()
                        join cg in db.C_COMPANY_GROUP on at.COMPANY_GROUP_ID equals cg.COMPANY_GROUP_ID
                        where r.ACT == true
                        select new
                        {
                            TASK_CATEGORY_NAME = ta.TASK_CATEGORY_NAME ?? "Unknown",
                            COMPANY_GROUP_NAME = cg.COMPANY_GROUP_NAME,
                            TASK_CATEGORY_ID = cg.COMPANY_GROUP_ID
                        }).ToList();  // Materialize to memory

                        var reportData = preGroupData
                .GroupBy(x => x.TASK_CATEGORY_NAME)
                .OrderByDescending(g => g.Count())
                .Select(g => new CategoryCGWiseDetails
                {
                    TASK_CATEGORY_NAME = g.Key,
                    TOTAL_REPORT_CATEGORY = g.Count(),
                    PRAN = g.Count(x => x.COMPANY_GROUP_NAME == "PRAN"),
                    RFL = g.Count(x => x.COMPANY_GROUP_NAME == "RFL"),
                    CTL = g.Count(x => x.COMPANY_GROUP_NAME == "CTL"),
                    PDL = g.Count(x => x.COMPANY_GROUP_NAME == "PDL"),
                    GROUP_CS = g.Count(x => x.COMPANY_GROUP_NAME == "Group CS")
                }).ToList();

                return PartialView(reportData);
            }else
            {
                var preGroupData = (
                       from r in db.AUDIT_REPORT
                       join ta in db.AUDIT_TASK_CATEGORY on r.TASK_CATEGORY_ID equals ta.TASK_CATEGORY_ID into taJoin
                       from ta in taJoin.DefaultIfEmpty()
                       join at in db.AUDIT_TASK on r.TASK_ID equals at.TASK_ID into atJoin
                       from at in atJoin.DefaultIfEmpty()
                       join cg in db.C_COMPANY_GROUP on at.COMPANY_GROUP_ID equals cg.COMPANY_GROUP_ID
                       where r.ACT == true && r.CDU == U
                       select new
                       {
                           TASK_CATEGORY_NAME = ta.TASK_CATEGORY_NAME ?? "Unknown",
                           COMPANY_GROUP_NAME = cg.COMPANY_GROUP_NAME,
                           TASK_CATEGORY_ID = cg.COMPANY_GROUP_ID
                       }).ToList();  // Materialize to memory

                        var reportData = preGroupData
                .GroupBy(x => x.TASK_CATEGORY_NAME)
                .OrderByDescending(g => g.Count())
                .Select(g => new CategoryCGWiseDetails
                {
                    TASK_CATEGORY_NAME = g.Key,
                    TOTAL_REPORT_CATEGORY = g.Count(),
                    PRAN = g.Count(x => x.COMPANY_GROUP_NAME == "PRAN"),
                    RFL = g.Count(x => x.COMPANY_GROUP_NAME == "RFL"),
                    CTL = g.Count(x => x.COMPANY_GROUP_NAME == "CTL"),
                    PDL = g.Count(x => x.COMPANY_GROUP_NAME == "PDL"),
                    GROUP_CS = g.Count(x => x.COMPANY_GROUP_NAME == "Group CS")
                }).ToList();

               return PartialView(reportData);
            }
        }

        [KAuth(2009)]
        public ActionResult LicenseDashboard()
        {
            string LocationCompanyWiseLicenseCountSQL = @"SELECT  LO.LOCATION_NAME, C.COMPANY_NAME, L.COMPANY_ID, L.LIC_LOCATION_ID,  COUNT(L.LICENSE_ID) AS Total 
                                  FROM LIC_LICENSE  L
                                       LEFT JOIN LIC_COMPANY C ON C.COMPANY_ID = L.COMPANY_ID
                                       LEFT JOIN LIC_LOCATION LO ON LO.LIC_LOCATION_ID = L.LIC_LOCATION_ID
                                 WHERE L.ACT = 1
                                 GROUP BY LO.LOCATION_NAME,C.COMPANY_NAME, L.COMPANY_ID, L.LIC_LOCATION_ID
                                 ORDER BY Total DESC";

            string LicenseExpireSQL = @"SELECT LICENSE_ID,LOCATION_NAME,LICENSE_HOLDER_MOBILE,LICENSE_NUMBER,ISSUE_DATE,EXPIRE_DATE, REMAINING    
                                         from VW_LIC_EXPIRE_DATE_COUNT 
                                         ORDER BY REMAINING";

            List<A_SUMMARY_VIEW> KSA = db.A_SUMMARY_VIEW.Where(x => x.SV_USER == "AUDIT").ToList();
            Update_Date _UD = new Update_Date();

            var totalLicenseCount = db.Database.SqlQuery<int>("SELECT COUNT(LICENSE_ID) AS TOTAL_LICENSE FROM LIC_LICENSE WHERE ACT = 1").FirstOrDefault();
            _UD.TOTAL_LICENSE = totalLicenseCount;

            var totalLocationCount = db.Database.SqlQuery<int>("SELECT COUNT(LIC_LOCATION_ID) AS TOTAL_LOCATION FROM LIC_LOCATION WHERE ACT = 1").FirstOrDefault();
            _UD.TOTAL_LOCATION = totalLocationCount;

            var totalEntryLicenseCount = db.Database.SqlQuery<int>("SELECT  COUNT (distinct LIC_LOCATION_ID) AS Entry_Company from LIC_LICENSE WHERE ACT = 1").FirstOrDefault();
            _UD.TOTAL_ENTRY_LOCATION = totalEntryLicenseCount;

            var totalCompanyCount = db.Database.SqlQuery<int>("SELECT COUNT(COMPANY_ID) AS TOTAL_COMPANY FROM LIC_COMPANY WHERE ACT = 1").FirstOrDefault();
            _UD.TOTAL_COMPANY = totalCompanyCount;

            var totalEntryCompanyCount = db.Database.SqlQuery<int>("SELECT COUNT (distinct COMPANY_ID) AS Entry_Company from LIC_LICENSE WHERE ACT = 1").FirstOrDefault();
            _UD.TOTAL_ENTRY_COMPANY = totalEntryCompanyCount;


            SummaryViewKSA _SummaryViewKSA = new SummaryViewKSA();
            _SummaryViewKSA.locationCompanyWiseLicenseCount = LocationCompanyWiseLicenseCountData(LocationCompanyWiseLicenseCountSQL);
            _SummaryViewKSA.licenseExpireInformation = LicenseExpireData(LicenseExpireSQL);
            _SummaryViewKSA.updateDate = _UD;
            //_SummaryViewKSA.expireList = iqumaExpired();

            return View(_SummaryViewKSA);
        }

        [HttpGet]
        public ActionResult ViewLocationWisetDetails(string id, string LId, string Post)
        {
            if (id == null || LId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var VI = (from d in db.LIC_LICENSE
                      join LN in db.LIC_LICENSE_NAME on d.LICENSE_NAME_ID equals LN.LICENSE_NAME_ID
                      join LAN in db.LIC_ISSUING_AUTHORITY  on d.ISSUING_AUTHORITY_ID equals LAN.ISSUING_AUTHORITY_ID
                      join LS in db.LIC_STATUS on d.LICENSE_STATUS_ID equals LS.LICENSE_STATUS_ID
                      join ITEM in db.A_ITEM_MASTER on d.ITEM_ID equals ITEM.ITEM_ID into itemJoin
                      from ITEM in itemJoin.DefaultIfEmpty()

                      where d.COMPANY_ID == id && d.LIC_LOCATION_ID == LId
                      select new LicenseDetailsInformation
                      {
                          LICENSE_ID = d.LICENSE_ID,
                          LICENSE_NUMBER = d.LICENSE_NUMBER,
                          LICENSE_NAME = LN.LICENSE_NAME,
                          AUTHORITY_NAME = LAN.AUTHORITY_NAME,
                          ISSUE_DATE = d.ISSUE_DATE,
                          EXPIRE_DATE = d.EXPIRE_DATE,
                          LICENSE_HOLDER_MOBILE = d.LICENSE_HOLDER_MOBILE,
                          LICENSE_HOLDER_EMAIL = d.LICENSE_HOLDER_EMAIL,
                          LICENSE_FEE = d.LICENSE_FEE,

                          ENTERTAINMENT_FEE = d.ENTERTAINMENT_FEE,
                          ITEM_ID = d.ITEM_ID,
                          LICENSE_STATUS_ID = LS.LICENSE_STATUS_NAME,
                          REMARK = d.REMARK,
                          APPLICATION_DATE = d.APPLICATION_DATE,
                          CDU = d.CDU

                          // Add other properties as needed
                      }).ToList();

            return PartialView(VI);
        }

        [HttpGet]
        public ActionResult ViewLicenseWisetDetails(string id, string Post)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var VI = (from d in db.LIC_LICENSE
                      join LN in db.LIC_LICENSE_NAME on d.LICENSE_NAME_ID equals LN.LICENSE_NAME_ID

                      join LO in db.LIC_LOCATION on d.LIC_LOCATION_ID equals LO.LIC_LOCATION_ID
                      join C in db.LIC_COMPANY on d.COMPANY_ID equals C.COMPANY_ID

                      join LAN in db.LIC_ISSUING_AUTHORITY on d.ISSUING_AUTHORITY_ID equals LAN.ISSUING_AUTHORITY_ID
                      join LS in db.LIC_STATUS on d.LICENSE_STATUS_ID equals LS.LICENSE_STATUS_ID
                      join ITEM in db.A_ITEM_MASTER on d.ITEM_ID equals ITEM.ITEM_ID into itemJoin
                      from ITEM in itemJoin.DefaultIfEmpty()

                      where d.LICENSE_ID == id 
                      select new LicenseDetailsInformation
                      {
                          LICENSE_ID = d.LICENSE_ID,
                          LICENSE_NUMBER = d.LICENSE_NUMBER,
                          LICENSE_NAME = LN.LICENSE_NAME,

                          LOCATION_NAME = LO.LOCATION_NAME,
                          COMPANY_NAME = C.COMPANY_NAME,

                          AUTHORITY_NAME = LAN.AUTHORITY_NAME,
                          ISSUE_DATE = d.ISSUE_DATE,
                          EXPIRE_DATE = d.EXPIRE_DATE,
                          LICENSE_HOLDER_MOBILE = d.LICENSE_HOLDER_MOBILE,
                          LICENSE_HOLDER_EMAIL = d.LICENSE_HOLDER_EMAIL,
                          LICENSE_FEE = d.LICENSE_FEE,

                          ENTERTAINMENT_FEE = d.ENTERTAINMENT_FEE,
                          ITEM_ID = d.ITEM_ID,
                          LICENSE_STATUS_ID = LS.LICENSE_STATUS_NAME,
                          REMARK = d.REMARK,
                          APPLICATION_DATE = d.APPLICATION_DATE,
                          CDU = d.CDU

                          // Add other properties as needed
                      }).ToList();

            return PartialView(VI);
        }

        //[HttpGet]
        //public ActionResult ViewAuditorWisetDetails(string id, string Post)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    var VI = (from d in db.AUDIT_REPORT
        //              join AT in db.AUDIT_TASK on d.TASK_ID equals AT.TASK_ID
        //              join U in db.A_USERS on d.CDU equals U.USER_TEXT
        //              join L in db.C_LOCATION on AT.LOCATION_ID equals L.LOCATION_ID
        //              join G in db.C_COMPANY_GROUP on AT.COMPANY_GROUP_ID equals G.COMPANY_GROUP_ID
        //              join COM in db.C_COMPANY on AT.COMPANY_ID equals COM.COMPANY_ID
        //              join DEP in db.C_DEPARTMENT on AT.DEPARTMENT_ID equals DEP.DEPARTMENT_ID

        //              where d.COMPANY_ID == id
        //                    && d.ACT = "1"
        //                    && d.APPROVE_SENT_STATUS = "N"
        //                    && AT.REPORT_CREATE_STATUS = 1
        //                    && AT.TASK_STATUS = 5
        //                    && AT.REPORT_STATUS_ID = 1

        //              select new AuditReportDetailsInfo
        //              {
        //                  AUDIT_RPT_ID = d.AUDIT_RPT_ID,
        //                  RPT_SENT_DATE = d.RPT_SENT_DATE,
        //                  TASK_ID = AT.TASK_ID,
        //                  AUDIT_RPT_NAME = d.AUDIT_RPT_NAME,

        //                  LOCATION_NAME = L.LOCATION_NAME,
        //                  COMPANY_GROUP_NAME = G.COMPANY_GROUP_NAME,
        //                  COMPANY_NAME = COM.COMPANY_NAME,
        //                  DEPARTMENT_NAME = DEP.DEPARTMENT_NAME,

        //                  WORK_SCOPE = d.WORK_SCOPE,
        //                  WORK_OPPORTUNITY = d.WORK_OPPORTUNITY,
        //                  USER_NAME = U.USER_NAME,
        //              }).ToList();

        //    return PartialView(VI);
        //}

        //[HttpGet]
        //public ActionResult ViewAuditorWisetDetails(string id, string Post)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    var VI = (from d in db.AUDIT_REPORT
        //              join AT in db.AUDIT_TASK on d.TASK_ID equals AT.TASK_ID
        //              join U in db.A_USERS on d.CDU equals U.USER_TEXT
        //              join L in db.C_LOCATION on AT.LOCATION_ID equals L.LOCATION_ID
        //              join G in db.C_COMPANY_GROUP on AT.COMPANY_GROUP_ID equals G.COMPANY_GROUP_ID
        //              join COM in db.C_COMPANY on AT.COMPANY_ID equals COM.COMPANY_ID
        //              join DEP in db.C_DEPARTMENT on AT.DEPARTMENT_ID equals DEP.DEPARTMENT_ID


        //              select new AuditReportDetailsInfo
        //              {
        //                  AUDIT_RPT_ID = d.AUDIT_RPT_ID,
        //                  RPT_SENT_DATE = d.RPT_SENT_DATE,
        //                  TASK_ID = AT.TASK_ID,
        //                  AUDIT_RPT_NAME = d.AUDIT_RPT_NAME,

        //                  LOCATION_NAME = L.LOCATION_NAME,
        //                  COMPANY_GROUP_NAME = G.COMPANY_GROUP_NAME,
        //                  COMPANY_NAME = COM.COMPANY_NAME,
        //                  DEPARTMENT_NAME = DEP.DEPARTMENT_NAME,

        //                  WORK_SCOPE = d.WORK_SCOPE,
        //                  WORK_OPPORTUNITY = d.WORK_OPPORTUNITY,
        //                  USER_NAME = U.USER_NAME,
        //              }).ToList();

        //    return PartialView(VI);
        //}



        //public JsonResult AUDIT_TASK_STATUS_DATA()
        //{
        //    var X = db.A_SUMMARY_VIEW.Where(x => x.SV_TABLE == "AUDIT_TASK_STATUS_DATA").FirstOrDefault();
        //    List<object> chartData = new List<object>();
        //    chartData.Add(new object[] { "Name", "Data" });
        //    chartData.Add(new object[] { X.DATA_AN, (double)X.DATA_A });
        //    chartData.Add(new object[] { X.DATA_BN, (double)X.DATA_B });
        //    chartData.Add(new object[] { X.DATA_CN, (double)X.DATA_C });
        //    chartData.Add(new object[] { X.DATA_DN, (double)X.DATA_D });
        //    chartData.Add(new object[] { X.DATA_EN, (double)X.DATA_E });
        //    return Json(chartData, JsonRequestBehavior.AllowGet);
        //}


        //public JsonResult AUDIT_TASK_STATUS_DATA(int? year)
        //{
        //    int selectedYear = year ?? DateTime.Now.Year;
        //    ViewBag.SelectedYear = selectedYear;

        //    string U = Session["UserId"].ToString();
        //    string T = Session["UserRole"].ToString();

        //    List<object> chartData = new List<object>();
        //    chartData.Add(new object[] { "Name", "Data" });

        //    string sql = "";

        //    if (T == "10")   // admin or full access
        //    {
        //        sql = @"
        //    SELECT 'Not Started' AS NAME, COUNT(*) AS TOTAL 
        //    FROM AUDIT_TASK WHERE TASK_STATUS = 1 AND ACT = 1 AND EXTRACT(YEAR FROM CDT) = "" + selectedYear + @""
        //    UNION ALL
        //    SELECT 'Started' AS NAME, COUNT(*) AS TOTAL 
        //    FROM AUDIT_TASK WHERE TASK_STATUS = 2 AND ACT = 1 AND EXTRACT(YEAR FROM CDT) = "" + selectedYear + @""
        //    UNION ALL
        //    SELECT 'Middle stage' AS NAME, COUNT(*) AS TOTAL 
        //    FROM AUDIT_TASK WHERE TASK_STATUS = 3 AND ACT = 1 AND EXTRACT(YEAR FROM CDT) = "" + selectedYear + @""
        //    UNION ALL
        //    SELECT 'End stage' AS NAME, COUNT(*) AS TOTAL  
        //    FROM AUDIT_TASK WHERE TASK_STATUS = 4 AND ACT = 1 AND EXTRACT(YEAR FROM CDT) = "" + selectedYear + @""
        //    UNION ALL
        //    SELECT 'Completed' AS NAME, COUNT(*) AS TOTAL 
        //    FROM AUDIT_TASK WHERE TASK_STATUS = 5 AND ACT = 1 AND EXTRACT(YEAR FROM CDT) = "" + selectedYear + @""
        //";
        //    }
        //    else       // user-wise data filter
        //    {
        //        sql = @"
        //    SELECT 'Not Started' AS NAME, COUNT(*) AS TOTAL 
        //    FROM AUDIT_TASK WHERE TASK_STATUS = 1 AND ACT = 1 AND CDU = :UserId AND EXTRACT(YEAR FROM CDT) = "" + selectedYear + @""
        //    UNION ALL
        //    SELECT 'Started' AS NAME, COUNT(*) AS TOTAL 
        //    FROM AUDIT_TASK WHERE TASK_STATUS = 2 AND ACT = 1 AND CDU = :UserId AND EXTRACT(YEAR FROM CDT) = "" + selectedYear + @""
        //    UNION ALL
        //    SELECT 'Middle stage' AS NAME, COUNT(*) AS TOTAL 
        //    FROM AUDIT_TASK WHERE TASK_STATUS = 3 AND ACT = 1 AND CDU = :UserId AND EXTRACT(YEAR FROM CDT) = "" + selectedYear + @""
        //    UNION ALL
        //    SELECT 'End stage' AS NAME, COUNT(*) AS TOTAL 
        //    FROM AUDIT_TASK WHERE TASK_STATUS = 4 AND ACT = 1 AND CDU = :UserId AND EXTRACT(YEAR FROM CDT) = "" + selectedYear + @""
        //    UNION ALL
        //    SELECT 'Completed' AS NAME, COUNT(*) AS TOTAL 
        //    FROM AUDIT_TASK WHERE TASK_STATUS = 5 AND ACT = 1 AND CDU = :UserId AND EXTRACT(YEAR FROM CDT) = "" + selectedYear + @""
        //";
        //    }

        //    // execute with parameter if needed
        //    var data = db.Database.SqlQuery<TaskStatusDTO>(sql,
        //        new OracleParameter("UserId", U)
        //    ).ToList();

        //    // convert to Google chart format
        //    foreach (var item in data)
        //    {
        //        chartData.Add(new object[] { item.NAME, (double)item.TOTAL });
        //    }

        //    return Json(chartData, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult AUDIT_TASK_STATUS_DATA(int? year)
        {
            int selectedYear = year ?? DateTime.Now.Year;
            ViewBag.SelectedYear = selectedYear;

            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            List<object> chartData = new List<object>();
            chartData.Add(new object[] { "Name", "Data" });

            string sql = "";

            if (T == "10")   // admin
            {
                sql = @"
                SELECT 'Not Started' AS NAME, COUNT(*) AS TOTAL 
                FROM AUDIT_TASK 
                WHERE TASK_STATUS = 1 AND ACT = 1 AND EXTRACT(YEAR FROM CDT) = :year

                UNION ALL
                SELECT 'Started', COUNT(*) 
                FROM AUDIT_TASK 
                WHERE TASK_STATUS = 2 AND ACT = 1 AND EXTRACT(YEAR FROM CDT) = :year

                UNION ALL
                SELECT 'Middle stage', COUNT(*) 
                FROM AUDIT_TASK 
                WHERE TASK_STATUS = 3 AND ACT = 1 AND EXTRACT(YEAR FROM CDT) = :year

                UNION ALL
                SELECT 'End stage', COUNT(*)  
                FROM AUDIT_TASK 
                WHERE TASK_STATUS = 4 AND ACT = 1 AND EXTRACT(YEAR FROM CDT) = :year

                UNION ALL
                SELECT 'Completed', COUNT(*) 
                FROM AUDIT_TASK 
                WHERE TASK_STATUS = 5 AND ACT = 1 AND EXTRACT(YEAR FROM CDT) = :year
                ";

                var data = db.Database.SqlQuery<TaskStatusDTO>(
                    sql,
                    new OracleParameter("year", selectedYear)
                ).ToList();

                foreach (var item in data)
                {
                    chartData.Add(new object[] { item.NAME, Convert.ToDouble(item.TOTAL) });
                }
            }
            else   // user-wise
            {
                sql = @"
                SELECT 'Not Started' AS NAME, COUNT(*) AS TOTAL 
                FROM AUDIT_TASK 
                WHERE TASK_STATUS = 1 AND ACT = 1 AND CDU = :UserId AND EXTRACT(YEAR FROM CDT) = :year

                UNION ALL
                SELECT 'Started', COUNT(*) 
                FROM AUDIT_TASK 
                WHERE TASK_STATUS = 2 AND ACT = 1 AND CDU = :UserId AND EXTRACT(YEAR FROM CDT) = :year

                UNION ALL
                SELECT 'Middle stage', COUNT(*) 
                FROM AUDIT_TASK 
                WHERE TASK_STATUS = 3 AND ACT = 1 AND CDU = :UserId AND EXTRACT(YEAR FROM CDT) = :year

                UNION ALL
                SELECT 'End stage', COUNT(*)  
                FROM AUDIT_TASK 
                WHERE TASK_STATUS = 4 AND ACT = 1 AND CDU = :UserId AND EXTRACT(YEAR FROM CDT) = :year

                UNION ALL
                SELECT 'Completed', COUNT(*) 
                FROM AUDIT_TASK 
                WHERE TASK_STATUS = 5 AND ACT = 1 AND CDU = :UserId AND EXTRACT(YEAR FROM CDT) = :year
                ";

                var data = db.Database.SqlQuery<TaskStatusDTO>(
                    sql,
                    new OracleParameter("UserId", U),
                    new OracleParameter("year", selectedYear)
                ).ToList();

                foreach (var item in data)
                {
                    chartData.Add(new object[] { item.NAME, Convert.ToDouble(item.TOTAL) });
                }
            }

            return Json(chartData, JsonRequestBehavior.AllowGet);
        }


        public class TaskStatusDTO
        {
            public string NAME { get; set; }
            public int TOTAL { get; set; }
        }

        //public JsonResult AUDIT_REPORT_STATUS()
        //{
        //    var X = db.A_SUMMARY_VIEW.Where(x => x.SV_TABLE == "AUDIT_REPORT_STATUS").FirstOrDefault();
        //    List<object> chartData = new List<object>();
        //    chartData.Add(new object[] { "Name", "Data" });
        //    chartData.Add(new object[] { X.DATA_AN, (double)X.DATA_A });
        //    chartData.Add(new object[] { X.DATA_BN, (double)X.DATA_B });
        //    chartData.Add(new object[] { X.DATA_CN, (double)X.DATA_C });
        //    return Json(chartData, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult AUDIT_REPORT_STATUS()
        //{
        //    string U = Session["UserId"].ToString();
        //    string T = Session["UserRole"].ToString();

        //    List<object> chartData = new List<object>();
        //    chartData.Add(new object[] { "Name", "Data" });

        //    string sql;

        //    if (T == "10")   // Admin / Full Access
        //    {
        //        sql = @"
        //    SELECT 'Completed With Report' AS NAME,
        //           COUNT(*) AS TOTAL
        //    FROM AUDIT_TASK
        //    WHERE REPORT_CREATE_STATUS = 1 
        //      AND REPORT_STATUS_ID = 1 
        //      AND ACT = 1

        //    UNION ALL

        //    SELECT 'Completed Without Report' AS NAME,
        //           COUNT(*) AS TOTAL
        //    FROM AUDIT_TASK
        //    WHERE REPORT_CREATE_STATUS = 1 
        //      AND REPORT_STATUS_ID = 2 
        //      AND ACT = 1

        //    UNION ALL

        //    SELECT 'Pre-Audit With Report' AS NAME,
        //           COUNT(*) AS TOTAL
        //    FROM AUDIT_TASK
        //    WHERE REPORT_CREATE_STATUS = 1 
        //      AND REPORT_STATUS_ID = 3 
        //      AND ACT = 1

        //    UNION ALL

        //    SELECT 'Pre-Audit Without Report' AS NAME,
        //           COUNT(*) AS TOTAL
        //    FROM AUDIT_TASK
        //    WHERE REPORT_CREATE_STATUS = 1
        //      AND REPORT_STATUS_ID = 4 
        //      AND ACT = 1

        //    UNION ALL

        //    SELECT 'Task Created But Report Pending' AS NAME,
        //           COUNT(*) AS TOTAL
        //    FROM AUDIT_TASK
        //    WHERE REPORT_CREATE_STATUS = 0 
        //      AND (REPORT_STATUS_ID IN (1,2,3,4) OR REPORT_STATUS_ID IS NULL)
        //      AND ACT = 1
        //";
        //    }
        //    else   // User-wise filter
        //    {
        //        sql = @"
        //    SELECT 'Completed With Report' AS NAME,
        //           COUNT(*) AS TOTAL
        //    FROM AUDIT_TASK
        //    WHERE REPORT_CREATE_STATUS = 1 
        //      AND REPORT_STATUS_ID = 1 
        //      AND ACT = 1
        //      AND CDU = :UserId

        //    UNION ALL

        //    SELECT 'Completed Without Report' AS NAME,
        //           COUNT(*) AS TOTAL
        //    FROM AUDIT_TASK
        //    WHERE REPORT_CREATE_STATUS = 1 
        //      AND REPORT_STATUS_ID = 2 
        //      AND ACT = 1
        //      AND CDU = :UserId

        //    UNION ALL

        //    SELECT 'Pre-Audit With Report' AS NAME,
        //           COUNT(*) AS TOTAL
        //    FROM AUDIT_TASK
        //    WHERE REPORT_CREATE_STATUS = 1 
        //      AND REPORT_STATUS_ID = 3 
        //      AND ACT = 1
        //      AND CDU = :UserId

        //    UNION ALL

        //    SELECT 'Pre-Audit Without Report' AS NAME,
        //           COUNT(*) AS TOTAL
        //    FROM AUDIT_TASK
        //    WHERE REPORT_CREATE_STATUS = 1
        //      AND REPORT_STATUS_ID = 4 
        //      AND ACT = 1
        //      AND CDU = :UserId

        //    UNION ALL

        //    SELECT 'Task Created But Report Pending' AS NAME,
        //           COUNT(*) AS TOTAL
        //    FROM AUDIT_TASK
        //    WHERE REPORT_CREATE_STATUS = 0 
        //      AND (REPORT_STATUS_ID IN (1,2,3,4) OR REPORT_STATUS_ID IS NULL)
        //      AND ACT = 1
        //      AND CDU = :UserId
        //";
        //    }

        //    // Execute safely with Oracle parameter
        //    var data = db.Database.SqlQuery<ReportStatusDTO>(
        //        sql, new OracleParameter("UserId", U)
        //    ).ToList();

        //    // Convert to Google chart format
        //    foreach (var item in data)
        //    {
        //        chartData.Add(new object[] { item.NAME, (double)item.TOTAL });
        //    }

        //    return Json(chartData, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult AUDIT_REPORT_STATUS(int? year)
        {
            int selectedYear = year ?? DateTime.Now.Year;
            ViewBag.SelectedYear = selectedYear;

            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            List<object> chartData = new List<object>();
            chartData.Add(new object[] { "Name", "Data" });

            string sql;
            List<OracleParameter> parameters = new List<OracleParameter>();

            if (T == "10")   // Admin
            {
                sql = @"
        SELECT 'Completed With Report' AS NAME, COUNT(*) AS TOTAL
        FROM AUDIT_TASK
        WHERE REPORT_CREATE_STATUS = 1 
          AND REPORT_STATUS_ID = 1 
          AND ACT = 1 
          AND EXTRACT(YEAR FROM CDT) = :year

        UNION ALL

        SELECT 'Completed Without Report', COUNT(*) 
        FROM AUDIT_TASK
        WHERE REPORT_CREATE_STATUS = 1 
          AND REPORT_STATUS_ID = 2 
          AND ACT = 1 
          AND EXTRACT(YEAR FROM CDT) = :year

        UNION ALL

        SELECT 'Pre-Audit With Report', COUNT(*) 
        FROM AUDIT_TASK
        WHERE REPORT_CREATE_STATUS = 1 
          AND REPORT_STATUS_ID = 3 
          AND ACT = 1 
          AND EXTRACT(YEAR FROM CDT) = :year

        UNION ALL

        SELECT 'Pre-Audit Without Report', COUNT(*) 
        FROM AUDIT_TASK
        WHERE REPORT_CREATE_STATUS = 1
          AND REPORT_STATUS_ID = 4 
          AND ACT = 1 
          AND EXTRACT(YEAR FROM CDT) = :year

        UNION ALL

        SELECT 'Task Created But Report Pending', COUNT(*) 
        FROM AUDIT_TASK
        WHERE REPORT_CREATE_STATUS = 0 
          AND (REPORT_STATUS_ID IN (1,2,3,4) OR REPORT_STATUS_ID IS NULL)
          AND ACT = 1 
          AND EXTRACT(YEAR FROM CDT) = :year
        ";

                parameters.Add(new OracleParameter("year", selectedYear));
            }
            else   // User-wise
            {
                sql = @"
        SELECT 'Completed With Report' AS NAME, COUNT(*) AS TOTAL
        FROM AUDIT_TASK
        WHERE REPORT_CREATE_STATUS = 1 
          AND REPORT_STATUS_ID = 1 
          AND ACT = 1
          AND CDU = :UserId 
          AND EXTRACT(YEAR FROM CDT) = :year

        UNION ALL

        SELECT 'Completed Without Report', COUNT(*) 
        FROM AUDIT_TASK
        WHERE REPORT_CREATE_STATUS = 1 
          AND REPORT_STATUS_ID = 2 
          AND ACT = 1
          AND CDU = :UserId 
          AND EXTRACT(YEAR FROM CDT) = :year

        UNION ALL

        SELECT 'Pre-Audit With Report', COUNT(*) 
        FROM AUDIT_TASK
        WHERE REPORT_CREATE_STATUS = 1 
          AND REPORT_STATUS_ID = 3 
          AND ACT = 1
          AND CDU = :UserId 
          AND EXTRACT(YEAR FROM CDT) = :year

        UNION ALL

        SELECT 'Pre-Audit Without Report', COUNT(*) 
        FROM AUDIT_TASK
        WHERE REPORT_CREATE_STATUS = 1
          AND REPORT_STATUS_ID = 4 
          AND ACT = 1
          AND CDU = :UserId 
          AND EXTRACT(YEAR FROM CDT) = :year

        UNION ALL

        SELECT 'Task Created But Report Pending', COUNT(*) 
        FROM AUDIT_TASK
        WHERE REPORT_CREATE_STATUS = 0 
          AND (REPORT_STATUS_ID IN (1,2,3,4) OR REPORT_STATUS_ID IS NULL)
          AND ACT = 1
          AND CDU = :UserId 
          AND EXTRACT(YEAR FROM CDT) = :year
        ";

                parameters.Add(new OracleParameter("UserId", U));
                parameters.Add(new OracleParameter("year", selectedYear));
            }

            var data = db.Database.SqlQuery<ReportStatusDTO>(
                sql,
                parameters.ToArray()
            ).ToList();

            foreach (var item in data)
            {
                chartData.Add(new object[] { item.NAME, Convert.ToDouble(item.TOTAL) });
            }

            return Json(chartData, JsonRequestBehavior.AllowGet);
        }

        public class ReportStatusDTO
        {
            public string NAME { get; set; }
            public int TOTAL { get; set; }
        }


        //for Total Report of Current vs Last Month
        public JsonResult CURRENT_LAST_MONTH_REPORT_SUM()
        {
            var X = db.A_SUMMARY_VIEW.Where(x => x.SV_TABLE == "CURRENT_LAST_MONTH_REPORT_SUM").FirstOrDefault();
            List<object> chartData = new List<object>();
            chartData.Add(new object[] { "Particular", "Current Month", "Last Month" });
            chartData.Add(new object[] { X.DATA_AN, (double)X.DATA_A, (double)X.DATA_E });
            chartData.Add(new object[] { X.DATA_BN, (double)X.DATA_B, (double)X.DATA_F });
            chartData.Add(new object[] { X.DATA_CN, (double)X.DATA_C, (double)X.DATA_G });
            chartData.Add(new object[] { X.DATA_DN, (double)X.DATA_D, (double)X.DATA_H });
            return Json(chartData, JsonRequestBehavior.AllowGet);
        }
        //for Company Group Wise Total Report
        public JsonResult COMPAY_GROUP_WISE_REPORT_SUM()
        {
            var X = db.A_SUMMARY_VIEW.Where(x => x.SV_TABLE == "COMPAY_GROUP_WISE_REPORT_SUM").FirstOrDefault();
            List<object> chartData = new List<object>();
            chartData.Add(new object[] { "Particular", "Value" });
            chartData.Add(new object[] { X.DATA_AN, (double)X.DATA_A });
            chartData.Add(new object[] { X.DATA_BN, (double)X.DATA_B });
            chartData.Add(new object[] { X.DATA_CN, (double)X.DATA_C });
            chartData.Add(new object[] { X.DATA_DN, (double)X.DATA_D });
            return Json(chartData, JsonRequestBehavior.AllowGet);
        }
        //for KSA - Assset
        public JsonResult PRO_SUM_FOUR()
        {
            var X = db.A_SUMMARY_VIEW.Where(x => x.SV_TABLE == "PRO_SUM_FOUR").FirstOrDefault();
            List<object> chartData = new List<object>();
            chartData.Add(new object[] { "Name", "Data" });
            chartData.Add(new object[] { X.DATA_AN, (double)X.DATA_A });
            chartData.Add(new object[] { X.DATA_BN, (double)X.DATA_B });
            chartData.Add(new object[] { X.DATA_CN, (double)X.DATA_C });
            return Json(chartData, JsonRequestBehavior.AllowGet);
        }
        //for KSA - Staff
        public JsonResult PRO_SUM_FIVE()
        {
            var X = db.A_SUMMARY_VIEW.Where(x => x.SV_TABLE == "PRO_SUM_FIVE").FirstOrDefault();
            List<object> chartData = new List<object>();
            chartData.Add(new object[] { "Name", "Data" });
            chartData.Add(new object[] { X.DATA_AN, (double)X.DATA_A });
            chartData.Add(new object[] { X.DATA_BN, (double)X.DATA_B });
            chartData.Add(new object[] { X.DATA_CN, (double)X.DATA_C });
            return Json(chartData, JsonRequestBehavior.AllowGet);
        }
        //for KSA - Police Case
        public JsonResult PRO_SUM_SIX()
        {
            var X = db.A_SUMMARY_VIEW.Where(x => x.SV_TABLE == "PRO_SUM_SIX").FirstOrDefault();
            List<object> chartData = new List<object>();
            chartData.Add(new object[] { "Name", "Data" });
            chartData.Add(new object[] { X.DATA_AN, (double)X.DATA_A });
            chartData.Add(new object[] { X.DATA_BN, (double)X.DATA_B });
            return Json(chartData, JsonRequestBehavior.AllowGet);
        }
        //for KSA - Police Case
        //public JsonResult AUDIT_REPORT_SENT_SUM()
        //{
        //    var X = db.A_SUMMARY_VIEW.Where(x => x.SV_TABLE == "AUDIT_REPORT_SENT_SUM").FirstOrDefault();
        //    List<object> chartData = new List<object>();
        //    chartData.Add(new object[] { "Name", "Data" });
        //    chartData.Add(new object[] { X.DATA_AN, (double)X.DATA_A });
        //    chartData.Add(new object[] { X.DATA_BN, (double)X.DATA_B });
        //    return Json(chartData, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult AUDIT_REPORT_SENT_SUM()
        //{
        //    string U = Session["UserId"].ToString();
        //    string T = Session["UserRole"].ToString();

        //    List<object> chartData = new List<object>();
        //    chartData.Add(new object[] { "Name", "Data" });

        //    string sql;

        //    if (T == "10")  // Admin / Full Access
        //    {
        //        sql = @"
        //    SELECT 'Total Report Sent' AS NAME,
        //           COUNT(*) AS TOTAL
        //    FROM AUDIT_REPORT
        //    WHERE ACT = 1
        //      AND RPT_SENT_DATE IS NOT NULL

        //    UNION ALL

        //    SELECT 'Work In Process' AS NAME,
        //           COUNT(*) AS TOTAL
        //    FROM AUDIT_REPORT
        //    WHERE ACT = 1
        //      AND RPT_SENT_DATE IS NULL
        //  ";
        //    }
        //    else  // User-wise data
        //    {
        //        sql = @"
        //    SELECT 'Total Report Sent' AS NAME,
        //           COUNT(*) AS TOTAL
        //    FROM AUDIT_REPORT
        //    WHERE ACT = 1
        //      AND RPT_SENT_DATE IS NOT NULL
        //      AND CDU = :UserId

        //    UNION ALL

        //    SELECT 'Work In Process' AS NAME,
        //           COUNT(*) AS TOTAL
        //    FROM AUDIT_REPORT
        //    WHERE ACT = 1
        //      AND RPT_SENT_DATE IS NULL
        //      AND CDU = :UserId
        //  ";
        //    }

        //    var data = db.Database.SqlQuery<ReportSentDTO>(
        //        sql, new OracleParameter("UserId", U)
        //    ).ToList();

        //    foreach (var item in data)
        //    {
        //        chartData.Add(new object[] { item.NAME, (double)item.TOTAL });
        //    }

        //    return Json(chartData, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult AUDIT_REPORT_SENT_SUM(int? year)
        {
            int selectedYear = year ?? DateTime.Now.Year;
            ViewBag.SelectedYear = selectedYear;

            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            List<object> chartData = new List<object>();
            chartData.Add(new object[] { "Name", "Data" });

            string sql;
            List<OracleParameter> parameters = new List<OracleParameter>();

            if (T == "10")  // Admin
            {
                sql = @"
        SELECT 'Total Report Sent' AS NAME, COUNT(*) AS TOTAL
        FROM AUDIT_REPORT
        WHERE ACT = 1
          AND RPT_SENT_DATE IS NOT NULL
          AND EXTRACT(YEAR FROM CDT) = :year

        UNION ALL

        SELECT 'Work In Process', COUNT(*) 
        FROM AUDIT_REPORT
        WHERE ACT = 1
          AND RPT_SENT_DATE IS NULL
          AND EXTRACT(YEAR FROM CDT) = :year
        ";

                parameters.Add(new OracleParameter("year", selectedYear));
            }
            else  // User-wise
            {
                sql = @"
        SELECT 'Total Report Sent' AS NAME, COUNT(*) AS TOTAL
        FROM AUDIT_REPORT
        WHERE ACT = 1
          AND RPT_SENT_DATE IS NOT NULL
          AND CDU = :UserId
          AND EXTRACT(YEAR FROM CDT) = :year

        UNION ALL

        SELECT 'Work In Process', COUNT(*) 
        FROM AUDIT_REPORT
        WHERE ACT = 1
          AND RPT_SENT_DATE IS NULL
          AND CDU = :UserId
          AND EXTRACT(YEAR FROM CDT) = :year
        ";

                parameters.Add(new OracleParameter("UserId", U));
                parameters.Add(new OracleParameter("year", selectedYear));
            }

            var data = db.Database.SqlQuery<ReportSentDTO>(
                sql,
                parameters.ToArray()
            ).ToList();

            foreach (var item in data)
            {
                chartData.Add(new object[] { item.NAME, Convert.ToDouble(item.TOTAL) });
            }

            return Json(chartData, JsonRequestBehavior.AllowGet);
        }


        public class ReportSentDTO
        {
            public string NAME { get; set; }
            public int TOTAL { get; set; }
        }

        //for KSA - Inspection Job And Bill Current vs Last Month
        public JsonResult PRO_SUM_SEVEN()
        {
            var X = db.A_SUMMARY_VIEW.Where(x => x.SV_TABLE == "PRO_SUM_SEVEN").FirstOrDefault();
            List<object> chartData = new List<object>();
            chartData.Add(new object[] { "Particular", "Current Month", "Last Month" });
            chartData.Add(new object[] { X.DATA_AN, (double)X.DATA_A, (double)X.DATA_E });
            chartData.Add(new object[] { X.DATA_BN, (double)X.DATA_B, (double)X.DATA_F });
            chartData.Add(new object[] { X.DATA_CN, (double)X.DATA_C, (double)X.DATA_G });
            chartData.Add(new object[] { X.DATA_DN, (double)X.DATA_D, (double)X.DATA_H });
            return Json(chartData, JsonRequestBehavior.AllowGet);
        }
        //for KSA - Inspection Job And Bill Last 6 Month
        public JsonResult PRO_SUM_EIGHT()
        {
            var X = db.A_SUMMARY_VIEW.Where(x => x.SV_TABLE == "PRO_SUM_EIGHT").FirstOrDefault();
            List<object> chartData = new List<object>();
            chartData.Add(new object[] { "Particular", "Value" });
            chartData.Add(new object[] { X.DATA_AN, (double)X.DATA_A });
            chartData.Add(new object[] { X.DATA_BN, (double)X.DATA_B });
            chartData.Add(new object[] { X.DATA_CN, (double)X.DATA_C });
            chartData.Add(new object[] { X.DATA_DN, (double)X.DATA_D });
            return Json(chartData, JsonRequestBehavior.AllowGet);
        }
        public List<TopTenItem> TopTenItem(string SQL)
        {
            List<TopTenItem> data = db.Database.SqlQuery<TopTenItem>(SQL).ToList();
            return data;
        }
        public List<LocationCompanyWiseLicenseCount> LocationCompanyWiseLicenseCountData(string SQL)
        {
            List<LocationCompanyWiseLicenseCount> data = db.Database.SqlQuery<LocationCompanyWiseLicenseCount>(SQL).ToList();
            return data;
        }
        public List<LicenseExpireInformation> LicenseExpireData(string SQL)
        {
            List<LicenseExpireInformation> data = db.Database.SqlQuery<LicenseExpireInformation>(SQL).ToList();
            return data;
        }
        public List<AuditorWiseAuditReportCount> AuditorWiseAuditReportCountData(string SQL)
        {
            List<AuditorWiseAuditReportCount> data = db.Database.SqlQuery<AuditorWiseAuditReportCount>(SQL).ToList();
            return data;
        }

        public List<ReportSummary> ReportSummaryData(string SQL)
        {
            List<ReportSummary> data = db.Database.SqlQuery<ReportSummary>(SQL).ToList();
            return data;
        }

        public List<CategoryCGWiseTotalReport> GetTop5Category(string SQL)
        {
            List<CategoryCGWiseTotalReport> data = db.Database.SqlQuery<CategoryCGWiseTotalReport>(SQL).ToList();
            return data;
        }

        public List<CategoryWiseWorkScopeTotalReport> GetTopWorkScope5Category(string SQL)
        {
            List<CategoryWiseWorkScopeTotalReport> data = db.Database.SqlQuery<CategoryWiseWorkScopeTotalReport>(SQL).ToList();
            return data;
        }
        
        public List<BUCGWiseTotalReport> GetTop5BU(string SQL)
        {
            List<BUCGWiseTotalReport> data = db.Database.SqlQuery<BUCGWiseTotalReport>(SQL).ToList();
            return data;
        }
        
        public List<BUCGWiseTotalWorkScopeReport> GetTop5WorkScopeBU(string SQL)
        {
            List<BUCGWiseTotalWorkScopeReport> data = db.Database.SqlQuery<BUCGWiseTotalWorkScopeReport>(SQL).ToList();
            return data;
        }

        public List<SegmentConpamyGroupWiseAuditReportCount> SegmentConpamyGroupWiseAuditReportCountData(string SQL)
        {
            List<SegmentConpamyGroupWiseAuditReportCount> data = db.Database.SqlQuery<SegmentConpamyGroupWiseAuditReportCount>(SQL).ToList();
            return data;
        }
        
        public List<CompanyNameCompanyGroupWiseAuditReportCount> CompanyNameCompanyGroupWiseAuditReportCountData(string SQL)
        {
            List<CompanyNameCompanyGroupWiseAuditReportCount> data = db.Database.SqlQuery<CompanyNameCompanyGroupWiseAuditReportCount>(SQL).ToList();
            return data;
        }
        public List<CompanyGroupWiseAuditReportCount> CompanyGroupWiseAuditReportCountData(string SQL)
        {
            List<CompanyGroupWiseAuditReportCount> data = db.Database.SqlQuery<CompanyGroupWiseAuditReportCount>(SQL).ToList();
            return data;
        }
        public List<AuditorWiseAuditFollowUpReportCount> AuditorWiseAuditFollowUpReportCountData(string SQL)
        {
            List<AuditorWiseAuditFollowUpReportCount> data = db.Database.SqlQuery<AuditorWiseAuditFollowUpReportCount>(SQL).ToList();
            return data;
        }
        
        public List<BUWiseAuditFollowUpReportStatusCount> BUWiseAuditFollowUpReportStatusCountData(string SQL)
        {
            List<BUWiseAuditFollowUpReportStatusCount> data = db.Database.SqlQuery<BUWiseAuditFollowUpReportStatusCount>(SQL).ToList();
            return data;
        }
        public List<TopAsset> TopTenAsset(string SQL)
        {
            List<TopAsset> data = db.Database.SqlQuery<TopAsset>(SQL).ToList();
            return data;
        }
        public List<TopStaff> TopTenStaff(string SQL)
        {
            List<TopStaff> data = db.Database.SqlQuery<TopStaff>(SQL).ToList();
            return data;
        }

        public List<IqumaExpired> iqumaExpired()
        {
            List<IqumaExpired> IE = (from x in db.A_SUMMARY_VIEW
                                     where x.ACT == true
                                     && x.SV_USER == "KSA"
                                     && x.SV_ID == "IQUMA"
                                     select new IqumaExpired
                                     {
                                         User_Name = x.DATA_AN,
                                         Iquma_No = x.SV_TABLE,
                                         Expire_Date = x.DATA_BN
                                     }).ToList();
            return IE;
        }

        public ActionResult UPDATE_KSA_ADMIN_DASHBOARD(string id)
        {
            string U = Session["UserId"].ToString();
            bool Execute = false;
            string SQL = "X";
            id.ToUpper();
            var PARAM_A = new OracleParameter("USERID", OracleDbType.Varchar2, U, ParameterDirection.Input);
            if (id == "FOUR")
            {
                SQL = "BEGIN PRO_SUM_FOUR(:USERID); END;";
                Execute = true;
            }
            else if (id == "FIVE")
            {
                SQL = "BEGIN PRO_SUM_FIVE(:USERID); END;";
                Execute = true;
            }
            else if (id == "SIX")
            {
                SQL = "BEGIN PRO_SUM_SIX(:USERID); END;";
                Execute = true;
            }
            else if (id == "SEVEN")
            {
                SQL = "BEGIN PRO_SUM_SEVEN(:USERID); END;";
                Execute = true;
            }
            else if (id == "EIGHT")
            {
                SQL = "BEGIN PRO_SUM_EIGHT(:USERID); END;";
                Execute = true;
            }
            else
            {
                Execute = false;
            }
            if (Execute)
            {
                var result = db.Database.ExecuteSqlCommand(SQL, PARAM_A);
                if (result == -1)
                {
                    TempData["msg"] = "swal('Success..','Data updated','success');";
                }
                else
                {
                    TempData["msg"] = "swal('Error..','Data update failed','error');";
                }
            }
            else
            {
                TempData["msg"] = "swal('error..','invalid request','error');";
            }
            return RedirectToAction("KSA_ADMIN_DASHBOARD");
        }

        public ActionResult UPDATE_AUDIT_DASHBOARD(string id)
        {
            string U = Session["UserId"].ToString();
            bool Execute = false;
            string SQL = "X";
            id.ToUpper();
            var PARAM_A = new OracleParameter("USERID", OracleDbType.Varchar2, U, ParameterDirection.Input);
            if (id == "FOUR")
            {
                SQL = "BEGIN PRO_ADT_TASK_STATUS; END;";
                Execute = true;
            }
            else if (id == "FIVE")
            {
                SQL = "BEGIN PRO_ADT_REPORT_STATUS; END;";
                Execute = true;
            }
            else if (id == "SIX")
            {
                SQL = "BEGIN PRO_ADT_REPORT_SENT_STATUS; END;";
                Execute = true;
            }
            else if (id == "SEVEN")
            {
                SQL = "BEGIN PRO_ADT_CURRENT_LAST_MON_RPT(:USERID); END;";
                Execute = true;
            }
            else if (id == "EIGHT")
            {
                SQL = "BEGIN PRO_ADT_COM_GROUP_WISE_RPT(:USERID); END;";
                Execute = true;
            }
            else
            {
                Execute = false;
            }
            if (Execute)
            {
                var result = db.Database.ExecuteSqlCommand(SQL, PARAM_A);
                if (result == -1)
                {
                    TempData["msg"] = "swal('Success..','Data updated','success');";
                }
                else
                {
                    TempData["msg"] = "swal('Error..','Data update failed','error');";
                }
            }
            else
            {
                TempData["msg"] = "swal('error..','invalid request','error');";
            }
            return RedirectToAction("AUDIT_DASHBOARD");
        }

        //Update Area wise Asset
        public ActionResult Pro_UpSert_AreaWAsset()
        {
            string U = Session["UserId"].ToString();
            string ABA_SQL = @"SELECT COUNT (A.ASET_BODY)CNT,A.ASET_BODY, V.BODY_NAME, U.AU_AREA, R.AREA_NAME
                            FROM A_ASET_DATA A 
                            JOIN A_ASET_USER U ON U.AU_ASET = A.ASET_ID AND U.ACT = '1'
                            JOIN A_AREA R ON U.AU_AREA = R.AREA_ID
                            JOIN A_BODY V ON A.ASET_BODY = V.BODY_ID
                            WHERE A.ACT = '1' AND A.ASET_GROUP = 'GR0001'
                            group by A.ASET_BODY, V.BODY_NAME, U.AU_AREA, R.AREA_NAME
                            order by U.AU_AREA";
            List<Asset_Body_Area> ABA = db.Database.SqlQuery<Asset_Body_Area>(ABA_SQL).ToList();

            int Inserted = 0;
            int Updated = 0;

            var _oldData = db.A_SUMMARY_VIEW.Where(x => x.SV_USER == "KSA" && x.SV_ID == "AWC").ToList();
            _oldData.ForEach(a => { a.DATA_A = Inserted; });
            db.SaveChanges(); //Remove Old Data

            foreach (var item in ABA)
            {
                //Check exist or not
                string SV_TABLE = item.AU_AREA + item.ASET_BODY;
                if (_oldData.Where(s => s.SV_TABLE == SV_TABLE).Any())
                {
                    //Update
                    A_SUMMARY_VIEW SV = db.A_SUMMARY_VIEW.Find(SV_TABLE);
                    SV.DATA_A = item.CNT;
                    SV.UDU = U;
                    SV.UDT = DateTime.Now;
                    db.Entry(SV).State = EntityState.Modified;
                    db.SaveChanges();
                    Updated = Updated + 1;
                }
                else
                {
                    //Insert
                    A_SUMMARY_VIEW SV = new A_SUMMARY_VIEW();
                    SV.SV_ID = "AWC";
                    SV.SV_TABLE = SV_TABLE;
                    SV.SV_NOTE = "Area Wise Asset Sum -Auto";
                    SV.SV_USER = "KSA";
                    SV.DATA_A = item.CNT;
                    SV.DATA_AN = item.ASET_BODY;
                    SV.DATA_BN = item.BODY_NAME;
                    SV.DATA_CN = item.AU_AREA;
                    SV.DATA_DN = item.AREA_NAME;
                    SV.CDT = DateTime.Now;
                    SV.CDU = U;
                    SV.ACT = true;
                    db.A_SUMMARY_VIEW.Add(SV);
                    db.SaveChanges();
                    Inserted = Inserted + 1;
                }
            }
            string msg = string.Format("swal('Success..','{0} new inserted and {1} updated','info');", Inserted, Updated);
            TempData["msg"] = msg;
            return RedirectToAction("KSA_ADMIN_DASHBOARD");
        }
        //area wise asset car cost
        public ActionResult Pro_Update_Cost()
        {
            string U = Session["UserId"].ToString();
            string ABA_SQL = @"SELECT F.JOBH_AREA,F.AREA_NAME,F.INSD,F.JOBD,
                             NVL(L.JOBH_AREAA,'0') JOBH_AREA_1,NVL(L.INSDA,0)INSD_1,NVL(L.JOBDA,0) JOBD_1
                             FROM (
                            SELECT  
                            H.JOBH_AREA,A.AREA_NAME,
                            SUM(D.JOBD_IQTY*D.JOBD_IRATE) INSD,
                            SUM(D.JOBD_JQTY*D.JOBD_JRATE) JOBD
                            FROM A_JOBD D
                            JOIN A_JOBH H ON D.JOBH_ID=H.JOBH_ID
                            JOIN A_AREA A ON H.JOBH_AREA=A.AREA_ID
                            WHERE H.JOBH_DATE BETWEEN 
                            --CURRENT MONTH FIRST DATE
                            (SELECT TRUNC(SYSDATE) - (TO_NUMBER(TO_CHAR(SYSDATE,'DD')) - 1) AS FIRST_DAY FROM DUAL)
                            AND
                            --CURRENT MONTH LAST DATE
                            (SELECT ADD_MONTHS(TRUNC(SYSDATE) - (TO_NUMBER(TO_CHAR(SYSDATE,'DD')) - 1), 1) -1 AS LAST_DAY FROM DUAL)
                            GROUP BY H.JOBH_AREA,A.AREA_NAME) F
                            LEFT OUTER JOIN (
                            SELECT  
                            H.JOBH_AREA JOBH_AREAA,
                            SUM(D.JOBD_IQTY*D.JOBD_IRATE) INSDA,
                            SUM(D.JOBD_JQTY*D.JOBD_JRATE) JOBDA
                            FROM A_JOBD D
                            JOIN A_JOBH H ON D.JOBH_ID=H.JOBH_ID
                            WHERE H.JOBH_DATE BETWEEN 
                            --LAST MONTH FIRST DATE
                            (SELECT TO_DATE(LAST_DAY(ADD_MONTHS(SYSDATE,-2))+1,'DD-MM-YY') AS FIRST_DAY FROM DUAL)
                            AND
                            --LAST MONTH LAST DATE
                            (SELECT TO_DATE(LAST_DAY(ADD_MONTHS(SYSDATE,-1)),'DD-MM-YY') AS LAST_DAY FROM DUAL)
                            GROUP BY H.JOBH_AREA
                            ) L ON F.JOBH_AREA=L.JOBH_AREAA";
            List<Area_Wise_Cost> ABA = db.Database.SqlQuery<Area_Wise_Cost>(ABA_SQL).ToList();

            int Inserted = 0;
            int Updated = 0;
            var _oldData = db.A_SUMMARY_VIEW.Where(x => x.SV_USER == "KSA" && x.SV_ID == "AWJ").ToList();
            _oldData.ForEach(a =>
            {
                a.DATA_A = a.DATA_B = a.DATA_C = a.DATA_D = Inserted;
            });
            db.SaveChanges(); //Remove Old Data
            foreach (var item in ABA)
            {
                //Check exist or not
                string SV_TABLE = item.JOBH_AREA + "JOB";
                if (_oldData.Where(s => s.SV_TABLE == SV_TABLE).Any())
                {
                    //Update
                    A_SUMMARY_VIEW SV = db.A_SUMMARY_VIEW.Find(SV_TABLE);
                    SV.DATA_A = item.INSD;
                    SV.DATA_B = item.JOBD;
                    SV.DATA_C = item.INSD_1;
                    SV.DATA_D = item.JOBD_1;
                    SV.UDU = U;
                    SV.UDT = DateTime.Now;
                    db.Entry(SV).State = EntityState.Modified;
                    db.SaveChanges();
                    Updated = Updated + 1;
                }
                else
                {
                    //Insert
                    A_SUMMARY_VIEW SV = new A_SUMMARY_VIEW();
                    SV.SV_ID = "AWJ";
                    SV.SV_TABLE = SV_TABLE;
                    SV.SV_NOTE = "Area Wise Cost Sum -Auto";
                    SV.SV_USER = "KSA";
                    SV.DATA_A = item.INSD;
                    SV.DATA_AN = item.AREA_NAME;
                    SV.DATA_B = item.JOBD;
                    SV.DATA_C = item.INSD_1;
                    SV.DATA_D = item.JOBD_1;
                    SV.CDT = DateTime.Now;
                    SV.CDU = U;
                    SV.ACT = true;
                    db.A_SUMMARY_VIEW.Add(SV);
                    db.SaveChanges();
                    Inserted = Inserted + 1;
                }
            }
            string msg = string.Format("swal('Success..','{0} new inserted and {1} updated','info');", Inserted, Updated);
            TempData["msg"] = msg;
            return RedirectToAction("KSA_ADMIN_DASHBOARD");
        }
        //staff iquma expire list
        public ActionResult Pro_Iquma_Expire_List()
        {
            string U = Session["UserId"].ToString();
            var ExpList = db.A_OWNR.Where(x => x.ACT == true && x.OWNR_TYPE == "Staff" && x.REMIND > 0).ToList();
            int Inserted = 0;
            int Updated = 0;
            var _oldData = db.A_SUMMARY_VIEW.Where(x => x.SV_USER == "KSA" && x.SV_ID == "IQUMA").ToList();
            db.A_SUMMARY_VIEW.RemoveRange(_oldData);
            db.SaveChanges(); //Remove Old Data
            Updated = _oldData.Count();
            foreach (var item in ExpList)
            {
                //Only Insert
                A_SUMMARY_VIEW SV = new A_SUMMARY_VIEW();
                SV.SV_ID = "IQUMA";
                SV.SV_TABLE = item.OWNR_NAME + "x" + Inserted;  //Iquma No
                SV.SV_NOTE = "Iquma Expire Notification";
                SV.SV_USER = "KSA";
                string x = item.OWNR_USER; //name of staff
                if (x.Length > 30)
                {
                    x = x.Substring(0, 30);
                }
                SV.DATA_AN = x;
                SV.DATA_BN = item.OWNR_NDATE.ToString(); //Expire date
                SV.CDU = U;
                SV.UDU = U;
                SV.CDT = DateTime.Now;
                SV.UDT = DateTime.Now;
                SV.ACT = true;
                db.A_SUMMARY_VIEW.Add(SV);
                db.SaveChanges();
                Inserted = Inserted + 1;
            }
            string msg = string.Format("swal('Success..','{0} new inserted and {1} updated','info');", Inserted, Updated);
            TempData["msg"] = msg;
            return RedirectToAction("KSA_ADMIN_DASHBOARD");
        }


        #endregion

    }

}