using PM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PM.Controllers
{
    public class PRANReportOldController : Controller
    {
        private pmdbEntities db = new pmdbEntities();
        // GET: PRANReportOld
        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Time Expired, please login!');window.location.href = '/Home/Login';</script>");
            }

        }
        //render report
        public List<VW_RPT_VENDINGMACHINE> getReports(string Comp, string DepotId)
        {
            var Reports = db.VW_RPT_VENDINGMACHINE.Where(r => r.CMP == Comp && r.DEPOT_ID == DepotId).ToList();
            //var Reports = from r in db.VW_RPT_VENDINGMACHINE
            //              where r.CMP == Comp && r.DEPOT_ID == DepotId
            //              select new rptPRANassetOldVM
            //              {
            //                  r
            //              };
                          
            return Reports;
        }
        public void CSVwriter()
        {
            List<VW_RPT_VENDINGMACHINE> dt = getReports("PRAN", "48503"); //Ice - box Retailer
            //your datatableList<Person> persons; // populated earlier
            using (StreamWriter wr = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\VendingMachine.csv"))
            {
                wr.WriteLine("Dept", "Depo", "Company", "ID", "Name", "Model", "Closed", "Owner", "Mobile", "Zone", "Group", "RDAT", "TDAT", "Status", "RGDT");
                foreach (VW_RPT_VENDINGMACHINE item in dt)
                {
                    wr.WriteLine(item.TEXT + "," + item.DEPOT_NAME + "," + item.CMP + "," + item.VHCL_ID + "," + item.VHCL_NAME + "," + item.VHCL_MODL + "," + item.VHCL_CLSE + "," + item.OWNR_NAME + "," + item.OWNER_MOB + "," + item.OWNR_ZONE + "," + item.OWNR_GROUP + "," + item.OWNR_RDAT + "," + item.OWNR_TDAT + "," + item.OWNR_RSGN + "," + item.OWNR_RGDT);
                }
            }
        }
        //perform Expost xlsx
        public void AssetOld(string deptId,string RptName)
        {
            var returnReport = getReports("PRAN", deptId);
            OfficeOpenXml.ExcelPackage excel = new OfficeOpenXml.ExcelPackage();
            string dateformat = "dd/MM/yyyy";
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            workSheet.Cells[1, 1].LoadFromCollection(returnReport, true);
            workSheet.Cells["A1"].Value = "WH";
            workSheet.Cells["B1"].Value = "WH Name";
            workSheet.Cells["C1"].Value = "Model";
            workSheet.Cells["D1"].Value = "Company";
            workSheet.Cells["E1"].Value = "Asset ID";
            workSheet.Cells["F1"].Value = "Asset Name";
            workSheet.Cells["G1"].Value = "Model";
            workSheet.Cells["H1"].Value = "Close";
            workSheet.Cells["I1"].Value = "Owner ID";
            workSheet.Cells["J1"].Value = "Owner Name";
            workSheet.Cells["K1"].Value = "Owner Mobile";
            workSheet.Cells["L1"].Value = "Zone";
            workSheet.Cells["M1"].Value = "Group";
            workSheet.Cells["N1"].Value = "H";
            workSheet.Cells["O1"].Value = "R";
            workSheet.Cells["P1"].Value = "U";
            workSheet.Cells["Q1"].Value = "Reg Date";
            workSheet.Cells["R1"].Value = "Delivery Date";
            workSheet.Cells["S1"].Value = "Status";
            workSheet.Cells["T1"].Value = "Status Date";
            workSheet.Column(17).Style.Numberformat.Format = dateformat;
            workSheet.Column(18).Style.Numberformat.Format = dateformat;
            workSheet.Column(20).Style.Numberformat.Format = dateformat;
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=file.xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }

    }
}