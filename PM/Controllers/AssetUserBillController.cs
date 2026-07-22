using PM.BLL;
using PM.Model;
using PM.ModelsV;
using PM.MyAuth;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace PM.Controllers
{
    [CustomHttpReffer]
    public class AssetUserBillController : Controller
    {
        private PMEntities db = new PMEntities();
        private ReturnPermission RP = new ReturnPermission();
        private GE Sequence = new GE();
        private PMExcelDataLoader EDL = new PMExcelDataLoader();
        [KAuth(501)]
        public ActionResult Index()
        {
            return View();
        }
        [KAuth(501)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(cls_SaveAssetUserBill a_OWNR_BILL)
        {
            string U = Session["UserId"].ToString();
            A_OWNR_BILL nb = new A_OWNR_BILL();
            if (ModelState.IsValid)
            {
                string Sql = string.Format(@"select h.au_aset, h.au_user , u.OWNR_USER from a_aset_user h
                        join a_ownr u on h.au_user=u.ownr_id
                        join a_aset_data d on h.au_aset=d.aset_id and d.act=1
                        where h.au_eruser is null
                        and u.ownr_text='{0}'", a_OWNR_BILL.BILL_OWNR);
                List<cls_user_aset> _cls_user_aset = db.Database.SqlQuery<cls_user_aset>(sql: Sql).ToList();
                if (_cls_user_aset.Count > 0)
                {
                    nb.BILL_ID = Sequence.GetSeq(16).ToString(); //16 is the table seq id
                    nb.BILL_OWNR = _cls_user_aset.FirstOrDefault().au_user;
                    nb.BILL_ASET = _cls_user_aset.FirstOrDefault().au_aset;
                    nb.BILL_DATE = a_OWNR_BILL.BILL_DATE;
                    nb.BILL_AMNT = a_OWNR_BILL.BILL_AMNT;
                    nb.BILL_OWNR_NAME = _cls_user_aset.FirstOrDefault().OWNR_USER;
                    nb.BILL_NOTE = a_OWNR_BILL.BILL_NOTE;
                    nb.ACT = true;
                    nb.CDT = DateTime.Now;
                    nb.CDU = U;
                    nb.VAR = 1;
                    db.A_OWNR_BILL.Add(nb);
                    db.SaveChanges();
                    TempData["msg"] = "swal('Success..','Bill saved succefully','success');";
                    return RedirectToAction("Index");
                }
            }
            nb.BILL_OWNR = a_OWNR_BILL.BILL_OWNR;
            nb.BILL_AMNT = a_OWNR_BILL.BILL_AMNT;
            nb.BILL_DATE = a_OWNR_BILL.BILL_DATE;
            nb.BILL_NOTE = a_OWNR_BILL.BILL_NOTE;
            TempData["msg"] = "swal('Ohh..','Bill saved failed','error');";
            return View(nb);
        }
        public JsonResult GetDeptUser(string StaffId)
        {
            string Sql = string.Format(@"select o.ownr_text staff_id,o.ownr_user staff_name,u.au_srdate receive_date,
                d.aset_code_name aset_name,d.aset_reg_date,e.department_name,f.area_name
                 from a_aset_user u
                join a_ownr o on u.au_user=o.ownr_id
                join a_aset_data d on u.au_aset=d.aset_id
                join a_department e on u.au_dept=e.department_id
                join a_area f on u.au_area=f.area_id
                where u.au_eruser is null and
                o.ownr_text='{0}' and d.act=1", StaffId);
            var UserData = db.Database.SqlQuery<cls_view_aset_aset_data>(sql: Sql).ToList();
            string data = string.Empty;
            if (UserData.Count > 0)
            {
                cls_view_aset_aset_data vd = UserData.FirstOrDefault();
                DateTime aset_reg_date = vd.aset_reg_date ?? DateTime.Now;
                DateTime receive_date = vd.receive_date ?? DateTime.Now;
                data = vd.staff_id + "-" + vd.staff_name + ", Aset: " + vd.aset_name + ", Reg. Date: " + aset_reg_date.ToString("dd/MMM/yyyy") + ", Received Date: " + receive_date.ToString("dd/MMM/yyyy") + ", Area: " + vd.area_name + ", Department: " + vd.department_name;
            }

            return Json(new { seco = data }, JsonRequestBehavior.AllowGet);

            //string Wd = "", Wa = "", Wu = "", Sec = "";
            //var fromStaffData = db.A_OWNR.Where(a => a.ACT == true && a.OWNR_TEXT == StaffId).ToList();
            //if (fromStaffData.Count == 0)
            //{
            //    return Json(false, JsonRequestBehavior.AllowGet);
            //}
            //var StaffDbid = fromStaffData.FirstOrDefault().OWNR_ID;
            //var fromHandOver = db.A_ASET_USER.Where(a => a.AU_USER == StaffDbid && (a.AU_ERUSER == null || a.AU_ERUSER == "")).ToList();
            //if (fromHandOver.Count > 0)
            //{
            //    Wd = fromHandOver.FirstOrDefault().AU_DEPT;
            //    Wa = fromHandOver.FirstOrDefault().AU_AREA;
            //    Wu = fromHandOver.FirstOrDefault().AU_USER;
            //    Sec = RP.InspectionAndJobData(Wd, Wa, Wu);
            //}
            //return Json(new { Department = Wd, Area = Wa, User = Wu, seco = Sec }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewOldData(string _staffId)
        {
            if (string.IsNullOrWhiteSpace(_staffId))
            {
                TempData["msg"] = "swal('Opps..','please enter staff id','info');";
                return RedirectToAction("Index");
            }
            //string Sql = string.Format(@"select op.ownr_id,op.pmnt_date,op.pmnt_amnt,op.ownr_name,
            //    op.pmnt_note,op.vhcl_id,ov.vhcl_name,ov.vhcl_rgdt,ou.ownr_rdat,ou.ownr_tdat
            //    from pm_pmnt op
            //    join pm_vhcl ov on op.vhcl_id=ov.vhcl_id 
            //    join pm_ownr ou on ov.vhcl_id=ou.vhcl_id and ou.ownr_id=op.ownr_id
            //    where op.ownr_id='{0}'
            //    union all
            //    select s.ownr_text,  b.bill_date, b.bill_amnt,s.ownr_user,
            //    b.bill_note,d.aset_tag,d.aset_code_name,d.aset_reg_date,e.au_srdate,e.au_erdate
            //    from a_ownr_bill b
            //    join a_ownr s on b.bill_ownr=s.ownr_id
            //    join a_aset_data d on b.bill_aset=d.aset_id
            //    join a_aset_user e on d.aset_id=e.au_aset and b.bill_ownr=e.au_user
            //    where s.ownr_text='{1}'", _staffId, _staffId);
            string Sql = string.Format(@"                
                select s.ownr_text,  b.bill_date, b.bill_amnt,s.ownr_user,
                b.bill_note,d.aset_tag,d.aset_code_name,d.aset_reg_date,e.au_srdate,e.au_erdate
                from a_ownr_bill b
                join a_ownr s on b.bill_ownr=s.ownr_id
                join a_aset_data d on b.bill_aset=d.aset_id
                join a_aset_user e on d.aset_id=e.au_aset and b.bill_ownr=e.au_user
                where s.ownr_text='{1}'
                union all
                select op.ownr_id, op.pmnt_date, op.pmnt_amnt, op.ownr_name, op.pmnt_note,
                op.vhcl_id, vh.vhcl_name, vh.vhcl_rgdt, to_date('') , to_date('')
                from pm.pm_pmnt op left outer join pm.pm_vhcl vh on op.vhcl_id = vh.vhcl_id
                where op.ownr_id = '{0}'", _staffId, _staffId);
            var _data = db.Database.SqlQuery<cls_ViewOldData>(Sql).ToList();
            return PartialView(_data);
        }
        public JsonResult ViewOldDataJson(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                TempData["msg"] = "swal('Opps..','please enter staff id','info');";
                return Json(new { data = "" });
            }

            //string Sql = string.Format(@"select op.ownr_id,op.pmnt_date,op.pmnt_amnt,op.ownr_name,
            //    op.pmnt_note,op.vhcl_id,ov.vhcl_name,ov.vhcl_rgdt--,ou.ownr_rdat,ou.ownr_tdat
            //    from pm_pmnt op
            //    join pm_vhcl ov on op.vhcl_id=ov.vhcl_id 
            //    --join pm_ownr ou on ov.vhcl_id=ou.vhcl_id and ou.ownr_id=op.ownr_id
            //    where op.ownr_id='{0}'
            //    union all
            //    select s.ownr_text,  b.bill_date, b.bill_amnt,s.ownr_user,
            //    b.bill_note,d.aset_tag,d.aset_code_name,d.aset_reg_date--,e.au_srdate,e.au_erdate
            //    from a_ownr_bill b
            //    join a_ownr s on b.bill_ownr=s.ownr_id
            //    join a_aset_data d on b.bill_aset=d.aset_id
            //    --join a_aset_user e on d.aset_id=e.au_aset and b.bill_ownr=e.au_user
            //    where s.ownr_text='{1}'", id, id);
            string Sql = string.Format(@"select op.ownr_id,op.pmnt_date,op.pmnt_amnt,op.ownr_name,
                op.pmnt_note,op.vhcl_id,ov.vhcl_name,ov.vhcl_rgdt
                from pm_pmnt op
                join pm_vhcl ov on op.vhcl_id=ov.vhcl_id 
                where op.ownr_id='{0}'
                union all
                select s.ownr_text,  b.bill_date, b.bill_amnt,s.ownr_user,
                b.bill_note,d.aset_tag,d.aset_code_name,d.aset_reg_date
                from a_ownr_bill b
                join a_ownr s on b.bill_ownr=s.ownr_id
                join a_aset_data d on b.bill_aset=d.aset_id
                where s.ownr_text='{1}'", id, id);
            var _data = db.Database.SqlQuery<cls_ViewOldData>(Sql).ToList();
            return Json(new { data = _data }, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// Imran Khan - 24-04-2021
        /// </summary>
        #region Asset_User_Excel
        [KAuth(502)]
        [HttpGet]
        public ActionResult UserBillExcel()
        {
            var model = new ExcelInputVM();
            return View(model);
        }


        [HttpPost]
        public ActionResult UserBillExcel(ExcelInputVM model)
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
            DataTable dt = EDL.MCExcelData(model.MyExcelFile.InputStream, false);
            string strContent = EDL.ConvertDataTableToHTMLTable(dt);
            model.MSExcelTable = strContent;
            TempData["msg"] = "swal('Success..','Excel fill loaded for checking','success');";
            return View(model);
        }

        public ActionResult SaveUserBillExcel(A_OWNR_BILL_VM[] ExlData)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            string result = "Asset data is not complete!";
            if (ModelState.IsValid)
            {
                List<A_OWNR_BILL_VM> final_obj = new List<A_OWNR_BILL_VM>();
                foreach (var item in ExlData)
                {
                    var _aset = db.A_ASET_DATA.Where(x => x.ASET_CODE_NAME == item.BILL_ASET && x.ACT == true);
                    var _ownr = db.A_OWNR.Where(x => x.OWNR_TEXT == item.BILL_OWNR && x.OWNR_RESIGN == "N" && x.ACT == true);
                    if (_aset != null && _ownr != null)
                    {
                        A_OWNR_BILL_VM _ob = new A_OWNR_BILL_VM();
                        _ob.ASET_ID = _aset.FirstOrDefault().ASET_ID;
                        _ob.BILL_DATE = item.BILL_DATE;
                        _ob.BILL_AMNT = item.BILL_AMNT;
                        _ob.BILL_NOTE = item.BILL_NOTE;
                        _ob.OWNR_ID = _ownr.FirstOrDefault().OWNR_ID;
                        _ob.OWNR_NAME = _ownr.FirstOrDefault().OWNR_USER;
                        final_obj.Add(_ob);
                    }
                }
                if (ExlData.Count() == final_obj.Count)
                {
                    int Count = 0;
                    foreach (var item in final_obj)
                    {
                        A_OWNR_BILL ad = new A_OWNR_BILL();
                        ad.BILL_OWNR = item.OWNR_ID;
                        ad.BILL_OWNR_NAME = item.OWNR_NAME;
                        ad.BILL_ASET = item.ASET_ID;
                        ad.BILL_DATE = item.BILL_DATE;
                        ad.BILL_AMNT = item.BILL_AMNT;
                        ad.BILL_NOTE = item.BILL_NOTE;
                        ad.CDT = DateTime.Now;
                        ad.CDU = U;
                        ad.VAR = 1;
                        ad.BILL_ID = Sequence.GetSeq(16).ToString();
                        db.A_OWNR_BILL.Add(ad);
                        Count = Count + 1;
                    }
                    db.SaveChanges();
                    result = string.Format("{0} :New Bill Saved", Count);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
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
    public class cls_ViewOldData
    {
        public string ownr_id { get; set; }
        public Nullable<DateTime> pmnt_date { get; set; }
        public Nullable<Int64> pmnt_amnt { get; set; }
        public string ownr_name { get; set; }
        public string pmnt_note { get; set; }
        public string vhcl_id { get; set; }
        public string vhcl_name { get; set; }
        public Nullable<DateTime> vhcl_rgdt { get; set; }
        public Nullable<DateTime> ownr_rdat { get; set; }
        public Nullable<DateTime> ownr_tdat { get; set; }
    }
    public class cls_SaveAssetUserBill
    {
        [Required]
        public string BILL_OWNR { get; set; }
        public DateTime BILL_DATE { get; set; }
        public int BILL_AMNT { get; set; }
        public string BILL_NOTE { get; set; }
        [Required]
        public string userDetail { get; set; }


    }
    public class cls_user_aset
    {
        public string au_aset { get; set; }
        public string au_user { get; set; }
        public string OWNR_USER { get; set; }
    }
    public class cls_view_aset_aset_data
    {
        public string staff_id { get; set; }
        public string staff_name { get; set; }
        public Nullable<DateTime> receive_date { get; set; }
        public string aset_name { get; set; }
        public Nullable<DateTime> aset_reg_date { get; set; }
        public string department_name { get; set; }
        public string area_name { get; set; }
    }
}
