using PM.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PM.Controllers
{
    public class ReportPRANController : Controller
    {
        pmdbEntities db = new pmdbEntities();
        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {
                var getId = Session["UserId"].ToString();

                var MyGroupList = db.P_USERS_ASGR.Where(b => b.USER_TEXT == getId && b.UAP_ACTV == true).Select(s => s.ASGR_TEXT).ToList();
                ViewBag.MyGroup = new SelectList(db.P_ASGR.Where(g => MyGroupList.Contains(g.ASGR_TEXT)), "ASGR_TEXT", "ASGR_NAME");
                var MyBeunList = db.P_USERS_BEUN.Where(b => b.USER_TEXT == getId && b.UBP_ACTV == true).Select(s => s.BEUN_TEXT).ToList();
                ViewBag.MyBU = new SelectList(db.P_BEUN.Where(b => MyBeunList.Contains(b.BEUN_TEXT)), "BEUN_TEXT", "BEUN_NAME");
                List<SelectListItem> AssetType = new List<SelectListItem>();
                AssetType.Add(new SelectListItem() { Text = "Free Asset", Value = "0" });
                AssetType.Add(new SelectListItem() { Text = "Booked Asset", Value = "1" });
                AssetType.Add(new SelectListItem() { Text = "Closed", Value = "2" });
                AssetType.Add(new SelectListItem() { Text = "All", Value = "3" });
                this.ViewBag.AssetType = new SelectList(AssetType, "Value", "Text");
                //   var MyDept = db.P_DEPT.Where(d => MyBeunList.Contains(d.DEPT_BEUN)).Select(s => s.DEPT_TEXT).ToList();
                //    var MyAssetList = db.P_ASSET.Where(a => MyGroupList.Contains(a.ASET_ASGR) && MyDept.Contains(a.ASET_DEPT)).Where(a => a.ISBOOKED == true).Select(s => s.ASET_OID);
                ViewBag.ASET_BRND = new SelectList(db.P_BRAND, "BRND_TEXT", "BRND_NAME");

                return View();
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Time Expired, please login!');window.location.href = '/Home/Login';</script>");
            }
        }
        public ActionResult ViewPRANReport(PRANReportVM rptvm)
        {
            if (ModelState.IsValid)
            {
                IQueryable<P_ASSET> AllAssetList = db.P_ASSET;

                if (rptvm.AssetType == 0) //free
                {
                    AllAssetList = AllAssetList.Where(a => a.ISBOOKED == false);
                }
                if (rptvm.AssetType == 1) //booked
                {
                    AllAssetList = AllAssetList.Where(a => a.ISBOOKED == true);
                }
                if (rptvm.AssetType == 2) //Closed
                {
                    AllAssetList = AllAssetList.Where(a => a.ASET_ACTV == false);
                }
                var All_Dept_in_BU = db.P_DEPT.Where(d => d.DEPT_BEUN == rptvm.MyBU).Select(d => d.DEPT_TEXT).ToList(); //BU by Dept
                var AllAsset = from a in AllAssetList.Where(a => a.ASET_ASGR == rptvm.MyGroup && All_Dept_in_BU.Contains(a.ASET_DEPT))
                                   //from a in db.P_ASSET.Where(a=>a.ASET_ASGR== rptvm.MyGroup)
                               from g in db.P_ASGR.Where(g => g.ASGR_TEXT == a.ASET_ASGR)
                               from d in db.P_DEPT.Where(d => d.DEPT_TEXT == a.ASET_DEPT)
                               from r in db.P_AREA.Where(r => r.AREA_TEXT == a.ASET_AREA)
                               from b in db.P_BRAND.Where(b => b.BRND_TEXT == a.ASET_BRND)
                               select new PRANAssetList
                               {
                                   AssetCode = a.ASET_TEXT,
                                   AssetName = a.ASET_NAME,
                                   AssetModel = a.ASET_MODL,
                                   PONO = a.ENGN_PONO,
                                   GRNNO = a.CHSS_GRN,
                                   RegDate = a.REG_DATE,
                                   MfgYear = a.MFG_YEAR,
                                   Group = g.ASGR_NAME,
                                   Dept = d.DEPT_NAME,
                                   Area = r.AREA_NAME,
                                   Brand = b.BRND_NAME,
                                   Status = a.ASET_ACTV.ToString()
                               };
                var returnReport = AllAsset.ToList();

                OfficeOpenXml.ExcelPackage excel = new OfficeOpenXml.ExcelPackage();
                string dateformat = "dd/MM/yyyy";
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells[1, 1].LoadFromCollection(returnReport, true);
                workSheet.Cells["A1"].Value = "Asset Code";
                workSheet.Cells["B1"].Value = "Asset Name";
                workSheet.Cells["C1"].Value = "Model";
                workSheet.Cells["D1"].Value = "PO Number";
                workSheet.Cells["E1"].Value = "GRN Number";
                workSheet.Cells["F1"].Value = "Reg Date";
                workSheet.Cells["G1"].Value = "MFG Year";
                workSheet.Cells["H1"].Value = "Group Name";
                workSheet.Cells["I1"].Value = "Dept Name";
                workSheet.Cells["J1"].Value = "Area Name";
                workSheet.Cells["K1"].Value = "Brand Name";
                workSheet.Cells["L1"].Value = "Status";
                workSheet.Column(6).Style.Numberformat.Format = dateformat;
                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=AssetList.xlsx");
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult PRANAssetByUser(PRANReportVM rptvm)
        {
            if (ModelState.IsValid)
            {
                IQueryable<VN_RPT_PRAN_ASSET> AllAssetList = db.VN_RPT_PRAN_ASSET;
                IQueryable<P_ASSET> AllAssetList2 = db.P_ASSET;

                if (rptvm.AssetType == 0) //free
                {
                    AllAssetList = AllAssetList.Where(a => a.ISBOOKED == false);
                }
                if (rptvm.AssetType == 1) //booked
                {
                    AllAssetList = AllAssetList.Where(a => a.ISBOOKED == true);
                }
                if (rptvm.AssetType == 2) //Closed
                {
                    AllAssetList = AllAssetList.Where(a => a.ASET_ACTV == false);
                }
                var All_Dept_in_BU = db.P_DEPT.Where(d => d.DEPT_BEUN == rptvm.MyBU).Select(d => d.DEPT_TEXT).ToList(); //BU by Dept
                var All_Asset_Status = from r in AllAssetList.Where(a => a.ASET_ASGR == rptvm.MyGroup && All_Dept_in_BU.Contains(a.ASET_DEPT))
                                       select new PRAN_ASSET_BY_USER_DOWNLOAD
                                       {
                                           ASET_NAME = r.ASET_NAME,
                                           ASET_MODL = r.ASET_MODL,
                                           MFG_YEAR = r.MFG_YEAR,
                                           BOOKING_STATUS = r.BOOKING_STATUS,
                                           ASGR_NAME = r.ASGR_NAME,
                                           DEPT_NAME = r.DEPT_NAME,
                                           AREA_NAME = r.AREA_NAME,
                                           BRND_NAME = r.BRND_NAME,
                                           ASET_STATUS = r.ASET_STATUS,
                                           OWNR_USER = r.OWNR_USER,
                                           OWNR_NAME = r.OWNR_NAME,
                                           OWNR_CONTACT = r.OWNR_CONTACT,
                                           OWNR_TAKE = r.OWNR_TAKE,
                                           OWNR_RELASE = r.OWNR_RELASE,
                                           USER_STATUS = r.USER_STATUS,
                                           OWNR_NOTE=r.OWNR_NOTE

                                       };
                var take_report = All_Asset_Status.ToList();

                OfficeOpenXml.ExcelPackage excel = new OfficeOpenXml.ExcelPackage();
                string dateformat = "dd/MM/yyyy";
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells[1, 1].LoadFromCollection(take_report, true);
                workSheet.Cells["A1"].Value = "Asset Name";
                workSheet.Cells["B1"].Value = "Model";
                workSheet.Cells["C1"].Value = "Year";
                workSheet.Cells["D1"].Value = "Booking";
                workSheet.Cells["E1"].Value = "Item Group";
                workSheet.Cells["F1"].Value = "Department";
                workSheet.Cells["G1"].Value = "Area";
                workSheet.Cells["H1"].Value = "Brand";
                workSheet.Cells["I1"].Value = "Asset Status";
                workSheet.Cells["J1"].Value = "Outlet";
                workSheet.Cells["K1"].Value = "Proprietor";
                workSheet.Cells["L1"].Value = "Contact";
                workSheet.Cells["M1"].Value = "RDAT";
                workSheet.Cells["N1"].Value = "TDAT";
                workSheet.Cells["O1"].Value = "Status";
                workSheet.Cells["P1"].Value = "Note";
                workSheet.Column(13).Style.Numberformat.Format = dateformat;
                workSheet.Column(14).Style.Numberformat.Format = dateformat;
                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=AssetListByUser.xlsx");
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            return RedirectToAction("Index");
        }
    }
}