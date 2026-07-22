using PM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PM.Controllers
{
    public class TransferDataController : Controller
    {
        private pmdbEntities db = new pmdbEntities();
        // GET: TransferData
        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {
                var getId = Session["UserId"].ToString();

                var MyGroupList = db.P_USERS_ASGR.Where(b => b.USER_TEXT == getId && b.UAP_ACTV == true).Select(s => s.ASGR_TEXT).ToList();
                var MyBeunList = db.P_USERS_BEUN.Where(b => b.USER_TEXT == getId && b.UBP_ACTV == true).Select(s => s.BEUN_TEXT).ToList();
                var MyDept = db.P_DEPT.Where(d => MyBeunList.Contains(d.DEPT_BEUN)).Select(s => s.DEPT_TEXT).ToList();
                var MyAssetList = db.P_ASSET.Where(a => MyGroupList.Contains(a.ASET_ASGR) && MyDept.Contains(a.ASET_DEPT)).Where(e => e.ASET_ACTV == true).ToList();
                return View(MyAssetList);
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Time Expired, please login!');window.location.href = '/Home/Login';</script>");
            }
        }
        public ActionResult History(string id)
        {
            string SQL = @"SELECT T.TRNS_TEXT,T.TRNS_FUSER,T.TRNS_TUSER,T.TRNS_CUSER,T.TRNS_AUSER,
                            T.TRNS_ASET,(A.ASET_TEXT ||'-' || A.ASET_NAME) AS ASSET_NAME,
                            T.TRNS_FDEPT,D.DEPT_NAME,
                            T.TRNS_TDEPT,DX.DEPT_NAME AS DEPT_NAME1,
                            T.TRNS_FDEPO,Z.AREA_NAME,
                            T.TRNS_TDEPO,ZX.AREA_NAME AS AREA_NAME1,
                            T.TRNS_DATE,T.TRNS_NOTE,
                            DECODE(T.TRNS_POST,1,'Approve','Not Approve')AS APPROVE,
                            DECODE(T.TRNS_ACTV,1,'Submitted','Not Submitted')AS SUBMITTED,
                            DECODE(T.TRNS_EXPR,1,'Last User','Old User')AS LASTUSER
                            FROM P_ASSET_TRNS T
                            INNER JOIN P_ASSET A ON T.TRNS_ASET=A.ASET_OID
                            INNER JOIN P_DEPT D ON T.TRNS_FDEPT=D.DEPT_TEXT
                            INNER JOIN P_DEPT DX ON T.TRNS_TDEPT=DX.DEPT_TEXT
                            INNER JOIN P_AREA Z ON T.TRNS_FDEPO = Z.AREA_TEXT
                            INNER JOIN P_AREA ZX ON T.TRNS_TDEPO = ZX.AREA_TEXT
                            WHERE T.TRNS_ASET='"+id+@"'
                            ORDER BY T.TRNS_DATE";
            var QueryString = db.Database.SqlQuery<RptAssetHistoryKSA>(SQL).ToList<RptAssetHistoryKSA>();
            return PartialView(QueryString);
        }

        public ActionResult CommonReport()
        {
            ViewBag.CompanyA = new SelectList(db.P_COMP.Where(c => c.COMP_ACTV == true), "COMP_TEXT", "COMP_NAME");
            ViewBag.DepoA =new SelectList(db.P_AREA.Where(b => b.AREA_ACTV == true && b.P_THNA.P_DIST.DIST_CUNT.Contains("KSA")), "AREA_TEXT", "AREA_NAME");
            return View();
        }
        public ActionResult getByCompany(string CompanyA)
        {
            string param2="A";
            List<RptAssetDataKSA> data = GetData(CompanyA, param2);
            return View(data);
        }
        public ActionResult getByDepot(string DepoA)
        {
            string param2 = "D";
            List<RptAssetDataKSA> data = GetData(DepoA, param2);
            return View("getByCompany", data);
        }
        public List<RptAssetDataKSA> GetData(string Param1, string Param2)
        {
            var asetdata = from a in db.P_ASSET
                           join d in  db.P_DEPT on a.ASET_DEPT equals d.DEPT_TEXT
                           join c in db.P_BEUN on d.DEPT_BEUN equals c.BEUN_TEXT
                           join o in db.P_COMP on c.BEUN_COMP equals o.COMP_TEXT
                           join z in db.P_AREA on a.ASET_AREA equals z.AREA_TEXT
                           select new RptAssetDataKSA
                           {
                               aset_id = a.ASET_TEXT,
                               aset_name=a.ASET_NAME,
                               aset_price=a.ASET_PRIC ?? 0,
                               aset_date=a.REG_DATE ,
                                aset_year =a.MFG_YEAR ?? 2000,
                               aset_dept=d.DEPT_NAME,
                               aset_owner_id =o.COMP_TEXT,
                               aset_owner_name=o.COMP_NAME,
                               aset_depo_id=z.AREA_TEXT,
                               aset_depo_name=z.AREA_NAME,
                               aset_status=a.ASET_ACTV ?? false,
                               aset_status_type=a.ASET_CLSE
                           };
            var listData = asetdata.ToList();
            if (Param2 == "A")
            {
                listData = asetdata.Where(a => a.aset_owner_id == Param1).ToList();
            }
            if (Param2 == "D")
            {
                listData = asetdata.Where(a => a.aset_depo_id == Param1).ToList();
            }
            
            return listData;
        }
    }
}