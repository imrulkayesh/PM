using PM.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PM.Controllers
{
    public class ReportBIKEController : Controller
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
        public ActionResult PRANAssetByUser(ReportBikeVM rptvm)
        {
            if (ModelState.IsValid)
            {

                var take_report = db.P_ASSET.Where(a => a.ASET_ASGR == "10000");//.ToList();

                var AllBike = from a in take_report
                              select new ReportBikeVM
                              {
                                  AsetCode=a.ASET_OID,
                                  AsetNumber=a.ASET_TEXT,
                                  AsetName=a.ASET_NAME,
                                  AsetModel=a.ASET_MODL,
                                  AsetType=a.ASET_TYPE,
                                  AsetEngine=a.ENGN_PONO,
                                  AsetChasis=a.CHSS_GRN,
                                  AsetPrice=a.ASET_PRIC,
                                  RegDate=a.REG_DATE,
                                  FuelType=a.FUEL_TYPE,
                                  AsetColor=a.ASET_COLR,
                                  MGFyear=a.MFG_YEAR,
                                  AsetNote=a.ASET_NAME,
                                  Booking=a.ISBOOKED,
                                  ASET_ASGR=a.P_ASGR.ASGR_NAME,
                                  ASET_DEPT=a.P_DEPT.DEPT_NAME,
                                  ASET_AREA=a.P_AREA.AREA_NAME,
                                  ASET_BRND=a.P_BRAND.BRND_NAME,
                                  ASET_ACTV=a.ASET_ACTV
                              };
                var returnBike = AllBike.ToList();


                OfficeOpenXml.ExcelPackage excel = new OfficeOpenXml.ExcelPackage();
                string dateformat = "dd/MM/yyyy";
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells[1, 1].LoadFromCollection(returnBike, true);
                workSheet.Column(9).Style.Numberformat.Format = dateformat;
                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=BikeList.xlsx");
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