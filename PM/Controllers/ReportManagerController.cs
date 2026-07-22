using PM.BLL;
using PM.Model;
using PM.ModelsV;
using PM.MyAuth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace PM.Controllers
{
    [Authorize]
    [CustomHttpReffer]
    public class ReportManagerController : Controller
    {
        private PMEntities db = new PMEntities();
        ReturnPermission RP = new ReturnPermission();
        public ActionResult PranBike()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            ViewBag.ASET_BUSINESS = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
            ViewBag.ASET_GROUP = new SelectList(RP.MyGroup(UserId: U, UserType: T), "GROUP_ID", "GROUP_NAME");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PranBikeView(PranBike P)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (ModelState.IsValid)
            {
                IQueryable<A_ASET_DATA> AD = db.A_ASET_DATA;
                if (!string.IsNullOrEmpty(P.ASET_DEPARTMENT))
                {
                    List<string> BU = RP.MyDeptListString(U, T);
                    AD.Where(a => BU.Contains(a.ASET_DEPARTMENT));
                }

            }
            return View();
        }

        [KAuth(902)]
        public ActionResult Report_Pending_JobCard_ForBill()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            List<string> ASET = RP.MyAssetListString(UserId: U, UserType: T);
            var JC = (from j in db.A_JOBH
                      join a in db.A_ASET_DATA on j.JOBH_ASET equals a.ASET_ID
                      join d in db.A_DEPARTMENT on j.JOBH_DEPARTMENT equals d.DEPARTMENT_ID
                      join o in db.A_OWNR on j.JOBH_OWNR equals o.OWNR_ID
                      join r in db.A_AREA on j.JOBH_AREA equals r.AREA_ID
                      where j.ACT == true
                      && ASET.Contains(j.JOBH_ASET)
                      && j.JOBH_BILL == "N"
                      // && j.JOBH_APP == "Y"
                      select new Report_Pending_JobCard_ForBill
                      {
                          JOBH_ID = j.JOBH_ID,
                          JOBH_ASET = a.ASET_CODE_NAME,
                          JOBH_OWNR = o.OWNR_USER,
                          JOBH_DEPARTMENT = d.DEPARTMENT_NAME,
                          JOBH_AREA = r.AREA_NAME,
                          JOBH_DATE = j.JOBH_DATE ?? DateTime.Now,
                          JOBH_MILEAGE = j.JOBH_MILEAGE,
                          JOBH_VENDOR = j.JOBH_VENDOR,
                          JOBH_NOTE = j.JOBH_NOTE,
                          JOBH_IPAY = j.JOBH_IPAY ?? 0,
                          JOBH_UPAY = j.JOBH_UPAY ?? 0,
                          JOBH_CPAY = j.JOBH_CPAY ?? 0,
                          JOBH_APPR = j.JOBH_APP
                      }
          ).ToList();
            return View(JC);
        }

        [KAuth(903)]
        public ActionResult Report_Pending_Inspection_ForJobCard()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            List<string> ASET = RP.MyAssetListString(UserId: U, UserType: T);
            var InspectionData = (from i in db.A_INSPH
                                  join a in db.A_ASET_DATA on i.INSPH_ASET equals a.ASET_ID
                                  join d in db.A_DEPARTMENT on i.INSPH_DEPARTMENT equals d.DEPARTMENT_ID
                                  join o in db.A_OWNR on i.INSPH_OWNR equals o.OWNR_ID
                                  join r in db.A_AREA on i.INSPH_AREA equals r.AREA_ID
                                  where i.ACT == true
                                  && ASET.Contains(a.ASET_ID)
                                  && i.INSPH_JOB == "N"
                                  select new Report_Pending_Inspection_ForJobCard
                                  {
                                      INSPH_ID = i.INSPH_ID,
                                      INSPH_ASET = a.ASET_CODE_NAME,
                                      INSPH_OWNR = o.OWNR_USER,
                                      INSPH_DEPARTMENT = d.DEPARTMENT_NAME,
                                      INSPH_AREA = r.AREA_NAME,
                                      INSPH_IUSER = i.INSPH_IUSER,
                                      INSPH_DATE = i.INSPH_DATE ?? DateTime.Now,
                                      MILEAGE = i.MILEAGE,
                                      INSPH_APPR = i.INSPH_APP
                                  }
                ).ToList();
            return View(InspectionData);
        }

        [KAuth(904)]
        public string Report_Asset_Transfer()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            //    List<string> MyGroup = RP.MyGroupString(UserId:U,UserType:T);
            var DT = DateTime.Now;
            string fileName = string.Format("Asset_Transfer_List_{0}{1}{2}.xlsx", DT.Year, DT.Month.ToString().PadLeft(2, '0'), DT.Day.ToString().PadLeft(2, '0'));
            List<Report_VW_RPT_ASET_USER> TL = (from r in db.VW_RPT_ASET_USER
                                                .Where(s => s.UG_USER == U)
                                                    // .Where(g=> MyGroup.Contains(g.ASET_GROUP))
                                                select new Report_VW_RPT_ASET_USER
                                                {
                                                    TAG = r.TAG,
                                                    CODE_NAME = r.CODE_NAME,
                                                    ASET_DEPT = r.ASET_DEPT,
                                                    ASET_AREA = r.ASET_AREA,
                                                    USER_ID = r.USER_ID,
                                                    USER_NAME = r.USER_NAME,
                                                    WORKING_DEPT = r.WORKING_DEPT,
                                                    WORKING_AREA = r.WORKING_AREA,
                                                    START_DATE = r.START_DATE,
                                                    START_NOTE = r.START_NOTE,
                                                    END_DATE = r.END_DATE,
                                                    END_NOTE = r.END_NOTE,
                                                    STAUTS = r.STAUTS
                                                }).ToList();

            OfficeOpenXml.ExcelPackage excel = new OfficeOpenXml.ExcelPackage();
            string dateformat = "dd/MM/yyyy";
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            workSheet.Cells[1, 1].LoadFromCollection(TL, true);

            var allCells = workSheet.Cells[1, 1, workSheet.Dimension.Start.Row, workSheet.Dimension.End.Column];
            var cellFont = allCells.Style.Font;
            cellFont.Size = 12;
            cellFont.Bold = true;
            cellFont.Color.SetColor(System.Drawing.Color.Blue);
            workSheet.Cells["A1"].Value = "TAG";
            workSheet.Cells["B1"].Value = "Asset Name";
            workSheet.Cells["C1"].Value = "Asset Deparment";
            workSheet.Cells["D1"].Value = "Asset Area";
            workSheet.Cells["E1"].Value = "User Id";
            workSheet.Cells["F1"].Value = "User Name";
            workSheet.Cells["G1"].Value = "Working Department";
            workSheet.Cells["H1"].Value = "Working Area";
            workSheet.Cells["I1"].Value = "Start Date";
            workSheet.Cells["J1"].Value = "Start Note";
            workSheet.Cells["K1"].Value = "End Date";
            workSheet.Cells["L1"].Value = "End Note";
            workSheet.Cells["M1"].Value = "Status";
            workSheet.Column(9).Style.Numberformat.Format = dateformat;
            workSheet.Column(11).Style.Numberformat.Format = dateformat;
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=" + fileName);
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
            return ("Downloaded");
        }


        [KAuth(905)]
        public ActionResult Report_Paid_Bills()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            List<string> ASET = RP.MyAssetListString(UserId: U, UserType: T);
            ViewBag.ASET_ID = new SelectList(db.A_ASET_DATA.Where(x => ASET.Contains(x.ASET_ID)), "ASET_ID", "ASET_CODE_NAME");
            ViewBag.AREA_ID = new SelectList(RP.MyAreaList(UserId: U, UserType: T), "AREA_ID", "AREA_NAME");
            ViewBag.BUSINESS_ID = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
            List<string> TID = new List<string>();
            TID.Add("Y");
            TID.Add("N");
            ViewBag.TYPE_ID = new SelectList(TID);

            return View();
        }
        [KAuth(905)]
        [HttpPost]
        public ActionResult Report_Paid_Bills(Report_Paid_Bills RPB)
        {
            if (ModelState.IsValid)
            {
                var DT = DateTime.Now;
                string fileName = string.Format("Bill_Paid_{0}{1}{2}.xlsx", DT.Year, DT.Month.ToString().PadLeft(2, '0'), DT.Day.ToString().PadLeft(2, '0'));
                double DayRange = (RPB.T_BILH_DATE - RPB.F_BILH_DATE).TotalDays;
                if (DayRange >= 92)
                {
                    TempData["msg"] = string.Format("swal('Ohh..','You select {0} days, You can not select date range more than 3 months.','info');", DayRange);
                    return Redirect("Report_inspXjobcXbill");
                }

                var TempStore = db.VW_RPT_BILL.Where(x => x.BILH_DATE >= RPB.F_BILH_DATE && x.BILH_DATE <= RPB.T_BILH_DATE).ToList();

                if (RPB.AREA_ID != null)
                {
                    TempStore = TempStore.Where(x => x.JOBH_AREA == RPB.AREA_ID).ToList();
                }
                if (RPB.BUSINESS_ID != null)
                {
                    TempStore = TempStore.Where(x => x.DEPARTMENT_BUSINESS == RPB.BUSINESS_ID).ToList();
                }

                List<Report_Paid_Bills_Download> DATA = (from x in TempStore
                                                         select new Report_Paid_Bills_Download
                                                         {
                                                             BILH_ID = x.BILH_ID,
                                                             BILH_DATE = x.BILH_DATE,
                                                             BILH_NOTE = x.BILH_NOTE,
                                                             BILH_REF = x.BILH_REF,
                                                             BILH_RECEIVER = x.BILH_RECEIVER,
                                                             BILH_IPAY = x.BILH_IPAY,
                                                             BILH_UPAY = x.BILH_UPAY,
                                                             BILH_CPAY = x.BILH_CPAY,
                                                             ASET_CODE_NAME = x.ASET_CODE_NAME,
                                                             OWNR_USER = x.OWNR_USER,
                                                             DEPARTMENT_NAME = x.DEPARTMENT_NAME,
                                                             BUSINESS_NAME = x.BUSINESS_NAME,
                                                             AREA_NAME = x.AREA_NAME,
                                                             JOBH_TYPE = x.ITYPE_NAME
                                                         }
                            ).ToList();

                OfficeOpenXml.ExcelPackage excel = new OfficeOpenXml.ExcelPackage();
                string dateformat = "dd/MM/yyyy";
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells[1, 1].LoadFromCollection(DATA, true);

                var allCells = workSheet.Cells[1, 1, workSheet.Dimension.Start.Row, workSheet.Dimension.End.Column];
                var cellFont = allCells.Style.Font;
                cellFont.Size = 12;
                cellFont.Bold = true;
                cellFont.Color.SetColor(System.Drawing.Color.Blue);
                workSheet.Cells["A1"].Value = "Bill No";
                workSheet.Cells["B1"].Value = "Bill Date";
                workSheet.Cells["C1"].Value = "Note";
                workSheet.Cells["D1"].Value = "References";
                workSheet.Cells["E1"].Value = "Receiver";
                workSheet.Cells["F1"].Value = "Insurance";
                workSheet.Cells["G1"].Value = "User";
                workSheet.Cells["H1"].Value = "Company";
                workSheet.Cells["I1"].Value = "Aset Name";
                workSheet.Cells["J1"].Value = "User Name";
                workSheet.Cells["K1"].Value = "Department";
                workSheet.Cells["L1"].Value = "Business";
                workSheet.Cells["M1"].Value = "Area";
                workSheet.Cells["N1"].Value = "Bill Type";
                workSheet.Column(2).Style.Numberformat.Format = dateformat;
                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=" + fileName);
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            return View("Report_Paid_Bills");
        }


        [KAuth(906)]
        public ActionResult Report_Asset_JobCard_Expense()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            List<string> ASET = RP.MyAssetListString(UserId: U, UserType: T);

            ViewBag.ASET_ID = new SelectList(db.A_ASET_DATA.Where(x => ASET.Contains(x.ASET_ID)), "ASET_ID", "ASET_CODE_NAME");
            ViewBag.AREA_ID = new SelectList(RP.MyAreaList(UserId: U, UserType: T), "AREA_ID", "AREA_NAME");
            ViewBag.BUSINESS_ID = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
            ViewBag.ITEM_ID = new SelectList(RP.InspectionItem(ig: "GR0001"), "ITEM_ID", "ITEM_NAME");
            ViewBag.JOBC_TYPE = new SelectList(db.A_INSP_TYPE, "ITYPE_ID", "ITYPE_NAME");

            ViewBag.ITEM_IDx = new SelectList(RP.InspectionItem(ig: "GR0001"), "ITEM_ID", "ITEM_NAME");
            return View();
        }
        public bool Write_Report_Excel(List<Report_JobCard_Month> DATA, string fileName)
        {
            OfficeOpenXml.ExcelPackage excel = new OfficeOpenXml.ExcelPackage();
            string dateformat = "dd/MM/yyyy";
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            workSheet.Cells[1, 1].LoadFromCollection(DATA, true);

            var allCells = workSheet.Cells[1, 1, workSheet.Dimension.Start.Row, workSheet.Dimension.End.Column];
            var cellFont = allCells.Style.Font;
            cellFont.Size = 12;
            cellFont.Bold = true;
            cellFont.Color.SetColor(System.Drawing.Color.Blue);
            workSheet.Cells["A1"].Value = "Job Card";
            workSheet.Cells["B1"].Value = "Job Date";
            workSheet.Cells["C1"].Value = "Line";
            workSheet.Cells["D1"].Value = "Item";
            workSheet.Cells["E1"].Value = "Item Name";
            workSheet.Cells["F1"].Value = "Qty";
            workSheet.Cells["G1"].Value = "Rate";
            workSheet.Cells["H1"].Value = "Amount";
            workSheet.Cells["I1"].Value = "Item Note";
            workSheet.Cells["J1"].Value = "Asset";
            workSheet.Cells["K1"].Value = "User";
            workSheet.Cells["L1"].Value = "Department";
            workSheet.Cells["M1"].Value = "Business";
            workSheet.Cells["N1"].Value = "Area";
            workSheet.Cells["O1"].Value = "Mileage";
            workSheet.Cells["P1"].Value = "Vendor";
            workSheet.Cells["Q1"].Value = "Job Note";
            workSheet.Cells["R1"].Value = "Job Type";
            workSheet.Cells["S1"].Value = "Inspection No";
            workSheet.Cells["T1"].Value = "INSPH CREATE USER";
            workSheet.Cells["U1"].Value = "INSPH USER ID";
            workSheet.Column(2).Style.Numberformat.Format = dateformat;
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=" + fileName);
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
            return true;
        }

        [KAuth(906)]
        [HttpPost]
        public ActionResult Month_Wise_Report(Month_Wise_Report MWR)
        {
            if (ModelState.IsValid)
            {
                var DT = DateTime.Now;
                string fileName = string.Format("Month_Wise_Jobcard_{0}{1}{2}.xlsx", DT.Year, DT.Month.ToString().PadLeft(2, '0'), DT.Day.ToString().PadLeft(2, '0'));
                double DayRange = (MWR.T_BILH_DATE - MWR.F_BILH_DATE).TotalDays;
                if (DayRange >= 183)
                {
                    TempData["msg"] = string.Format("swal('Ohh..','You select {0} days, You can not select date range more than 6 months.','info');", DayRange);
                    return Redirect("Report_Asset_JobCard_Expense");
                }
                // Break 6 Month
                List<Report_JobCard_Month> DATA = (from x in db.VW_RPT_ASET_EXP.Where(x => x.JOBH_DATE >= MWR.F_BILH_DATE && x.JOBH_DATE <= MWR.T_BILH_DATE)
                                                   select new Report_JobCard_Month
                                                   {
                                                       JOBH_ID = x.JOBH_ID,
                                                       JOBH_DATE = x.JOBH_DATE,
                                                       JOBD_LINE = x.JOBD_LINE,
                                                       JOBD_ITEM = x.JOBD_ITEM,
                                                       ITEM_NAME = x.ITEM_NAME,
                                                       JOBD_JQTY = x.JOBD_JQTY,
                                                       JOBD_JRATE = x.JOBD_JRATE,
                                                       JOBD_JAMNT = x.JOBD_JAMNT,
                                                       JOBD_NOTE = x.JOBD_NOTE,
                                                       ASET_CODE_NAME = x.ASET_CODE_NAME,
                                                       OWNR_USER = x.OWNR_USER,
                                                       DEPARTMENT_NAME = x.DEPARTMENT_NAME,
                                                       BUSINESS_NAME = x.BUSINESS_NAME,
                                                       AREA_NAME = x.AREA_NAME,
                                                       JOBH_MILEAGE = x.JOBH_MILEAGE,
                                                       JOBH_VENDOR = x.JOBH_VENDOR,
                                                       JOBH_NOTE = x.JOBH_NOTE,
                                                       ITYPE_NAME = x.ITYPE_NAME,
                                                       INSPH_ID = x.INSPH_ID
                                                   }
                            ).ToList();
                //Write Excel Data
                Write_Report_Excel(DATA: DATA, fileName: fileName);
            }
            return View("Report_Asset_JobCard_Expense");
        }


        [KAuth(906)]
        [HttpPost]
        public ActionResult Monthly_Area_Wise_Report(Monthly_Area_Wise_Report MAWR)
        {
            if (ModelState.IsValid)
            {
                var DT = DateTime.Now;
                string fileName = string.Format("Monthly_Area_Wise_Jobcard_{0}{1}{2}.xlsx", DT.Year, DT.Month.ToString().PadLeft(2, '0'), DT.Day.ToString().PadLeft(2, '0'));
                double DayRange = (MAWR.T_BILH_DATE - MAWR.F_BILH_DATE).TotalDays;
                if (DayRange >= 183)
                {
                    TempData["msg"] = string.Format("swal('Ohh..','You select {0} days, You can not select date range more than 6 months.','info');", DayRange);
                    return Redirect("Report_Asset_JobCard_Expense");
                }
                // Break 6 Month
                List<Report_JobCard_Month> DATA = (from x in db.VW_RPT_ASET_EXP.Where(x => x.JOBH_DATE >= MAWR.F_BILH_DATE && x.JOBH_DATE <= MAWR.T_BILH_DATE && x.JOBH_AREA == MAWR.AREA_ID)
                                                   select new Report_JobCard_Month
                                                   {
                                                       JOBH_ID = x.JOBH_ID,
                                                       JOBH_DATE = x.JOBH_DATE,
                                                       JOBD_LINE = x.JOBD_LINE,
                                                       JOBD_ITEM = x.JOBD_ITEM,
                                                       ITEM_NAME = x.ITEM_NAME,
                                                       JOBD_JQTY = x.JOBD_JQTY,
                                                       JOBD_JRATE = x.JOBD_JRATE,
                                                       JOBD_JAMNT = x.JOBD_JAMNT,
                                                       JOBD_NOTE = x.JOBD_NOTE,
                                                       ASET_CODE_NAME = x.ASET_CODE_NAME,
                                                       OWNR_USER = x.OWNR_USER,
                                                       DEPARTMENT_NAME = x.DEPARTMENT_NAME,
                                                       BUSINESS_NAME = x.BUSINESS_NAME,
                                                       AREA_NAME = x.AREA_NAME,
                                                       JOBH_MILEAGE = x.JOBH_MILEAGE,
                                                       JOBH_VENDOR = x.JOBH_VENDOR,
                                                       JOBH_NOTE = x.JOBH_NOTE,
                                                       ITYPE_NAME = x.ITYPE_NAME,
                                                       INSPH_ID = x.INSPH_ID,
                                                       INSPH_IUSER = x.INSPH_IUSER,
                                                       CDU = x.CDU
                                                   }
                            ).ToList();
                //Write Excel Data
                Write_Report_Excel(DATA: DATA, fileName: fileName);
            }
            return View("Report_Asset_JobCard_Expense");
        }


        [KAuth(906)]
        [HttpPost]
        public ActionResult Asset_Wise_Report(Asset_Wise_Report AWR)
        {
            if (ModelState.IsValid)
            {
                var DT = DateTime.Now;
                string fileName = string.Format("Asset_Wise_Jobcard_{0}{1}{2}.xlsx", DT.Year, DT.Month.ToString().PadLeft(2, '0'), DT.Day.ToString().PadLeft(2, '0'));
                double DayRange = (AWR.T_BILH_DATE - AWR.F_BILH_DATE).TotalDays;
                if (DayRange >= 183)
                {
                    TempData["msg"] = string.Format("swal('Ohh..','You select {0} days, You can not select date range more than 6 months.','info');", DayRange);
                    return Redirect("Report_Asset_JobCard_Expense");
                }
                // Break 6 Month
                List<Report_JobCard_Month> DATA = (from x in db.VW_RPT_ASET_EXP.Where(x => x.JOBH_DATE >= AWR.F_BILH_DATE && x.JOBH_DATE <= AWR.T_BILH_DATE && x.JOBH_ASET == AWR.ASET_ID)
                                                   select new Report_JobCard_Month
                                                   {
                                                       JOBH_ID = x.JOBH_ID,
                                                       JOBH_DATE = x.JOBH_DATE,
                                                       JOBD_LINE = x.JOBD_LINE,
                                                       JOBD_ITEM = x.JOBD_ITEM,
                                                       ITEM_NAME = x.ITEM_NAME,
                                                       JOBD_JQTY = x.JOBD_JQTY,
                                                       JOBD_JRATE = x.JOBD_JRATE,
                                                       JOBD_JAMNT = x.JOBD_JAMNT,
                                                       JOBD_NOTE = x.JOBD_NOTE,
                                                       ASET_CODE_NAME = x.ASET_CODE_NAME,
                                                       OWNR_USER = x.OWNR_USER,
                                                       DEPARTMENT_NAME = x.DEPARTMENT_NAME,
                                                       BUSINESS_NAME = x.BUSINESS_NAME,
                                                       AREA_NAME = x.AREA_NAME,
                                                       JOBH_MILEAGE = x.JOBH_MILEAGE,
                                                       JOBH_VENDOR = x.JOBH_VENDOR,
                                                       JOBH_NOTE = x.JOBH_NOTE,
                                                       ITYPE_NAME = x.ITYPE_NAME,
                                                       INSPH_ID = x.INSPH_ID
                                                   }
                            ).ToList();
                //Write Excel Data
                Write_Report_Excel(DATA: DATA, fileName: fileName);
            }
            return View("Report_Asset_JobCard_Expense");
        }
        [KAuth(906)]
        [HttpPost]
        public ActionResult Zone_Wise_Report(Zone_Wise_Report ZWR)
        {
            if (ModelState.IsValid)
            {
                var DT = DateTime.Now;
                string fileName = string.Format("Zone_Wise_Jobcard_{0}{1}{2}.xlsx", DT.Year, DT.Month.ToString().PadLeft(2, '0'), DT.Day.ToString().PadLeft(2, '0'));
                double DayRange = (ZWR.T_BILH_DATE - ZWR.F_BILH_DATE).TotalDays;
                if (DayRange >= 183)
                {
                    TempData["msg"] = string.Format("swal('Ohh..','You select {0} days, You can not select date range more than 6 months.','info');", DayRange);
                    return Redirect("Report_Asset_JobCard_Expense");
                }
                // Break 6 Month
                List<Report_JobCard_Month> DATA = (from x in db.VW_RPT_ASET_EXP.Where(x => x.JOBH_DATE >= ZWR.F_BILH_DATE && x.JOBH_DATE <= ZWR.T_BILH_DATE && x.JOBH_AREA == ZWR.AREA_ID)
                                                   select new Report_JobCard_Month
                                                   {
                                                       JOBH_ID = x.JOBH_ID,
                                                       JOBH_DATE = x.JOBH_DATE,
                                                       JOBD_LINE = x.JOBD_LINE,
                                                       JOBD_ITEM = x.JOBD_ITEM,
                                                       ITEM_NAME = x.ITEM_NAME,
                                                       JOBD_JQTY = x.JOBD_JQTY,
                                                       JOBD_JRATE = x.JOBD_JRATE,
                                                       JOBD_JAMNT = x.JOBD_JAMNT,
                                                       JOBD_NOTE = x.JOBD_NOTE,
                                                       ASET_CODE_NAME = x.ASET_CODE_NAME,
                                                       OWNR_USER = x.OWNR_USER,
                                                       DEPARTMENT_NAME = x.DEPARTMENT_NAME,
                                                       BUSINESS_NAME = x.BUSINESS_NAME,
                                                       AREA_NAME = x.AREA_NAME,
                                                       JOBH_MILEAGE = x.JOBH_MILEAGE,
                                                       JOBH_VENDOR = x.JOBH_VENDOR,
                                                       JOBH_NOTE = x.JOBH_NOTE,
                                                       ITYPE_NAME = x.ITYPE_NAME,
                                                       INSPH_ID = x.INSPH_ID,
                                                       INSPH_IUSER = x.INSPH_IUSER,
                                                       CDU = x.CDU
                                                   }
                            ).ToList();
                //Write Excel Data
                Write_Report_Excel(DATA: DATA, fileName: fileName);
            }
            return View("Report_Asset_JobCard_Expense");
        }
        [KAuth(906)]
        [HttpPost]
        public ActionResult Business_Wise_Report(Business_Wise_Report BWR)
        {
            if (ModelState.IsValid)
            {
                var DT = DateTime.Now;
                string fileName = string.Format("Business_Wise_Jobcard_{0}{1}{2}.xlsx", DT.Year, DT.Month.ToString().PadLeft(2, '0'), DT.Day.ToString().PadLeft(2, '0'));
                double DayRange = (BWR.T_BILH_DATE - BWR.F_BILH_DATE).TotalDays;
                if (DayRange >= 183)
                {
                    TempData["msg"] = string.Format("swal('Ohh..','You select {0} days, You can not select date range more than 6 months.','info');", DayRange);
                    return Redirect("Report_Asset_JobCard_Expense");
                }
                // Break 6 Month
                List<Report_JobCard_Month> DATA = (from x in db.VW_RPT_ASET_EXP.Where(x => x.JOBH_DATE >= BWR.F_BILH_DATE && x.JOBH_DATE <= BWR.T_BILH_DATE && x.BUSINESS_ID == BWR.BUSINESS_ID)
                                                   select new Report_JobCard_Month
                                                   {
                                                       JOBH_ID = x.JOBH_ID,
                                                       JOBH_DATE = x.JOBH_DATE,
                                                       JOBD_LINE = x.JOBD_LINE,
                                                       JOBD_ITEM = x.JOBD_ITEM,
                                                       ITEM_NAME = x.ITEM_NAME,
                                                       JOBD_JQTY = x.JOBD_JQTY,
                                                       JOBD_JRATE = x.JOBD_JRATE,
                                                       JOBD_JAMNT = x.JOBD_JAMNT,
                                                       JOBD_NOTE = x.JOBD_NOTE,
                                                       ASET_CODE_NAME = x.ASET_CODE_NAME,
                                                       OWNR_USER = x.OWNR_USER,
                                                       DEPARTMENT_NAME = x.DEPARTMENT_NAME,
                                                       BUSINESS_NAME = x.BUSINESS_NAME,
                                                       AREA_NAME = x.AREA_NAME,
                                                       JOBH_MILEAGE = x.JOBH_MILEAGE,
                                                       JOBH_VENDOR = x.JOBH_VENDOR,
                                                       JOBH_NOTE = x.JOBH_NOTE,
                                                       ITYPE_NAME = x.ITYPE_NAME,
                                                       INSPH_ID = x.INSPH_ID,
                                                       INSPH_IUSER = x.INSPH_IUSER,
                                                       CDU = x.CDU
                                                   }
                            ).ToList();
                //Write Excel Data
                Write_Report_Excel(DATA: DATA, fileName: fileName);
            }
            return View("Report_Asset_JobCard_Expense");
        }
        [KAuth(906)]
        [HttpPost]
        public ActionResult Item_Wise_Report(Item_Wise_Report IWR)
        {
            if (ModelState.IsValid)
            {
                var DT = DateTime.Now;
                string fileName = string.Format("Item_Wise_Jobcard_{0}{1}{2}.xlsx", DT.Year, DT.Month.ToString().PadLeft(2, '0'), DT.Day.ToString().PadLeft(2, '0'));
                double DayRange = (IWR.T_BILH_DATEx - IWR.F_BILH_DATEx).TotalDays;
                if (DayRange >= 183)
                {
                    TempData["msg"] = string.Format("swal('Ohh..','You select {0} days, You can not select date range more than 6 months.','info');", DayRange);
                    return Redirect("Report_Asset_JobCard_Expense");
                }
                List<string> F_ITEM = new List<string>();
                foreach (var item in IWR.ITEM_IDx)
                {
                    F_ITEM.Add(item.JOB_ITEM);
                }
                // Break 6 Month
                List<Report_JobCard_Month> DATA = (from x in db.VW_RPT_ASET_EXP.Where(x => x.JOBH_DATE >= IWR.F_BILH_DATEx && x.JOBH_DATE <= IWR.T_BILH_DATEx && F_ITEM.Contains(x.JOBD_ITEM))
                                                   select new Report_JobCard_Month
                                                   {
                                                       JOBH_ID = x.JOBH_ID,
                                                       JOBH_DATE = x.JOBH_DATE,
                                                       JOBD_LINE = x.JOBD_LINE,
                                                       JOBD_ITEM = x.JOBD_ITEM,
                                                       ITEM_NAME = x.ITEM_NAME,
                                                       JOBD_JQTY = x.JOBD_JQTY,
                                                       JOBD_JRATE = x.JOBD_JRATE,
                                                       JOBD_JAMNT = x.JOBD_JAMNT,
                                                       JOBD_NOTE = x.JOBD_NOTE,
                                                       ASET_CODE_NAME = x.ASET_CODE_NAME,
                                                       OWNR_USER = x.OWNR_USER,
                                                       DEPARTMENT_NAME = x.DEPARTMENT_NAME,
                                                       BUSINESS_NAME = x.BUSINESS_NAME,
                                                       AREA_NAME = x.AREA_NAME,
                                                       JOBH_MILEAGE = x.JOBH_MILEAGE,
                                                       JOBH_VENDOR = x.JOBH_VENDOR,
                                                       JOBH_NOTE = x.JOBH_NOTE,
                                                       ITYPE_NAME = x.ITYPE_NAME,
                                                       INSPH_ID = x.INSPH_ID
                                                   }
                            ).ToList();
                //Write Excel Data
                return Json(Write_Report_Excel(DATA: DATA, fileName: fileName), JsonRequestBehavior.AllowGet);
                //  return Json(new { success= Write_Report_Excel(DATA: DATA, fileName: fileName), messag = fileName }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, messag = "Report generate failed" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Report_Asset_JobCard_Expense(JobCardReports JCR)
        {
            if (ModelState.IsValid)
            {
                var DT = DateTime.Now;
                string fileName = string.Format("Asset_JobCard_Expense_{0}{1}{2}.xlsx", DT.Year, DT.Month.ToString().PadLeft(2, '0'), DT.Day.ToString().PadLeft(2, '0'));
                double DayRange = (JCR.T_BILH_DATE - JCR.F_BILH_DATE).TotalDays;
                if (DayRange >= 92)
                {
                    TempData["msg"] = string.Format("swal('Ohh..','You select {0} days, You can not select date range more than 3 months.','info');", DayRange);
                    return Redirect("Report_Asset_JobCard_Expense");
                }
                var TempStore = db.VW_RPT_ASET_EXP.Where(x => x.JOBH_DATE >= JCR.F_BILH_DATE && x.JOBH_DATE <= JCR.T_BILH_DATE).ToList();
                if (JCR.ASET_ID != null)
                {
                    TempStore = TempStore.Where(a => a.JOBH_ASET == JCR.ASET_ID).ToList();
                }
                if (JCR.AREA_ID != null)
                {
                    TempStore = TempStore.Where(a => a.JOBH_AREA == JCR.AREA_ID).ToList();
                }
                if (JCR.BUSINESS_ID != null)
                {
                    TempStore = TempStore.Where(a => a.BUSINESS_ID == JCR.BUSINESS_ID).ToList();
                }
                if (JCR.ITEM_ID != null)
                {
                    TempStore = TempStore.Where(a => a.JOBD_ITEM == JCR.ITEM_ID).ToList();
                }
                if (JCR.JOBC_TYPE != null)
                {
                    TempStore = TempStore.Where(a => a.JOBH_TYPE == JCR.JOBC_TYPE).ToList();
                }
                List<Report_JobCard_Month> DATA = (from x in TempStore
                                                   select new Report_JobCard_Month
                                                   {
                                                       JOBH_ID = x.JOBH_ID,
                                                       JOBH_DATE = x.JOBH_DATE,
                                                       JOBD_LINE = x.JOBD_LINE,
                                                       JOBD_ITEM = x.JOBD_ITEM,
                                                       ITEM_NAME = x.ITEM_NAME,
                                                       JOBD_JQTY = x.JOBD_JQTY,
                                                       JOBD_JRATE = x.JOBD_JRATE,
                                                       JOBD_JAMNT = x.JOBD_JAMNT,
                                                       JOBD_NOTE = x.JOBD_NOTE,
                                                       ASET_CODE_NAME = x.ASET_CODE_NAME,
                                                       OWNR_USER = x.OWNR_USER,
                                                       DEPARTMENT_NAME = x.DEPARTMENT_NAME,
                                                       BUSINESS_NAME = x.BUSINESS_NAME,
                                                       AREA_NAME = x.AREA_NAME,
                                                       JOBH_MILEAGE = x.JOBH_MILEAGE,
                                                       JOBH_VENDOR = x.JOBH_VENDOR,
                                                       JOBH_NOTE = x.JOBH_NOTE,
                                                       ITYPE_NAME = x.ITYPE_NAME,
                                                       INSPH_ID = x.INSPH_ID
                                                   }
                            ).ToList();
                //Write Excel Data
                Write_Report_Excel(DATA: DATA, fileName: fileName);
            }
            return View("Report_Asset_JobCard_Expense");
        }
        [KAuth(907)]
        public ActionResult Report_inspXjobcXbill()
        {
            return View();
        }
        [KAuth(907)]
        [HttpPost]
        public ActionResult Report_inspXjobcXbill(Month_Wise_Report MWR)
        {
            if (ModelState.IsValid)
            {
                var DT = DateTime.Now;
                string fileName = string.Format("inspXjobcXbill_{0}{1}{2}.xlsx", DT.Year, DT.Month.ToString().PadLeft(2, '0'), DT.Day.ToString().PadLeft(2, '0'));
                double DayRange = (MWR.T_BILH_DATE - MWR.F_BILH_DATE).TotalDays;
                if (DayRange >= 92)
                {
                    TempData["msg"] = string.Format("swal('Ohh..','You select {0} days, You can not select date range more than 3 months.','info');", DayRange);
                    return Redirect("Report_inspXjobcXbill");
                }
                var VS = (from I in db.VW_RPT_INSP_SUM.Where(x => x.INSPH_DATE >= MWR.F_BILH_DATE && x.INSPH_DATE <= MWR.T_BILH_DATE)
                          join a in db.A_ASET_DATA on I.INSPH_ASET equals a.ASET_ID
                          join o in db.A_OWNR on I.INSPH_OWNR equals o.OWNR_ID
                          join r in db.A_AREA on I.INSPH_AREA equals r.AREA_ID
                          join d in db.A_DEPARTMENT on I.INSPH_DEPARTMENT equals d.DEPARTMENT_ID
                          from J in db.VW_RPT_JOB_SUM.Where(j => j.INSPH_ID == I.INSPH_ID).DefaultIfEmpty()
                          from B in db.VW_RPT_BILL_SUM.Where(b => b.JOBH_ID == J.JOBH_ID).DefaultIfEmpty()
                          select new inspXjobcXbill
                          {
                              INSPH_ASET = a.ASET_CODE_NAME,
                              INSPH_ID = I.INSPH_ID,
                              INSPH_DATE = I.INSPH_DATE,
                              INSP_AMT = I.INSP_AMT,
                              JOBH_ID = J.JOBH_ID,
                              JOBH_DATE = J.JOBH_DATE,
                              JOBH_AMT = J.JOBH_AMT,
                              BILH_ID = B.BILH_ID,
                              BILH_DATE = B.BILH_DATE,
                              BILL_AMT = B.BILL_AMT,
                              INSPH_OWNR = o.OWNR_USER,
                              OWNR_NAME = o.OWNR_NAME,
                              INSPH_AREA = r.AREA_NAME,
                              INSPH_DEPARTMENT = d.DEPARTMENT_NAME,
                              INSPH_BUSINESS = d.A_BUSINESS.BUSINESS_NAME
                          }).ToList();

                OfficeOpenXml.ExcelPackage excel = new OfficeOpenXml.ExcelPackage();
                string dateformat = "dd/MM/yyyy";
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells[1, 1].LoadFromCollection(VS, true);

                var allCells = workSheet.Cells[1, 1, workSheet.Dimension.Start.Row, workSheet.Dimension.End.Column];
                var cellFont = allCells.Style.Font;
                cellFont.Size = 12;
                cellFont.Bold = true;
                cellFont.Color.SetColor(System.Drawing.Color.Blue);
                workSheet.Cells["A1"].Value = "Asset Name";
                workSheet.Cells["B1"].Value = "Insp Id";
                workSheet.Cells["C1"].Value = "Insp Date";
                workSheet.Cells["D1"].Value = "Insp Amount";
                workSheet.Cells["E1"].Value = "Job Id";
                workSheet.Cells["F1"].Value = "Job Date";
                workSheet.Cells["G1"].Value = "Job Amount";
                workSheet.Cells["H1"].Value = "Bill Id";
                workSheet.Cells["I1"].Value = "Bill Date";
                workSheet.Cells["J1"].Value = "Bill Amount";
                workSheet.Cells["K1"].Value = "User";
                workSheet.Cells["L1"].Value = "User Id";
                workSheet.Cells["M1"].Value = "Area";
                workSheet.Cells["N1"].Value = "Department";
                workSheet.Cells["O1"].Value = "Business";
                workSheet.Column(3).Style.Numberformat.Format = dateformat;
                workSheet.Column(6).Style.Numberformat.Format = dateformat;
                workSheet.Column(9).Style.Numberformat.Format = dateformat;
                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=" + fileName);
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            TempData["msg"] = "swal('Success..','Report Downloaded','success');";
            return View();
        }

        [KAuth(913)]
        public ActionResult Report_inspXjobcXbill_Area_Wise()
        {

            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            List<string> ASET = RP.MyAssetListString(UserId: U, UserType: T);
            ViewBag.ASET_ID = new SelectList(db.A_ASET_DATA.Where(x => ASET.Contains(x.ASET_ID)), "ASET_ID", "ASET_CODE_NAME");
            ViewBag.AREA_ID = new SelectList(RP.MyAreaList(UserId: U, UserType: T), "AREA_ID", "AREA_NAME");
            ViewBag.BUSINESS_ID = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
            return View();
        }

        [KAuth(913)]
        [HttpPost]
        public ActionResult Report_inspXjobcXbill_Area_Wise(Month_Wise_Report MWR)
        {
            if (ModelState.IsValid)
            {
                var DT = DateTime.Now;
                string fileName = string.Format("inspXjobcXbill_{0}{1}{2}.xlsx", DT.Year, DT.Month.ToString().PadLeft(2, '0'), DT.Day.ToString().PadLeft(2, '0'));
                double DayRange = (MWR.T_BILH_DATE - MWR.F_BILH_DATE).TotalDays;
                if (DayRange >= 92)
                {
                    TempData["msg"] = string.Format("swal('Ohh..','You select {0} days, You can not select date range more than 3 months.','info');", DayRange);
                    return Redirect("Report_inspXjobcXbill");
                }
                var VS = (from I in db.VW_RPT_INSP_SUM.Where(x => x.INSPH_DATE >= MWR.F_BILH_DATE && x.INSPH_DATE <= MWR.T_BILH_DATE && x.INSPH_AREA == MWR.AREA_ID)
                          join a in db.A_ASET_DATA on I.INSPH_ASET equals a.ASET_ID
                          join o in db.A_OWNR on I.INSPH_OWNR equals o.OWNR_ID
                          join r in db.A_AREA on I.INSPH_AREA equals r.AREA_ID
                          join d in db.A_DEPARTMENT on I.INSPH_DEPARTMENT equals d.DEPARTMENT_ID
                          from J in db.VW_RPT_JOB_SUM.Where(j => j.INSPH_ID == I.INSPH_ID).DefaultIfEmpty()
                          from B in db.VW_RPT_BILL_SUM.Where(b => b.JOBH_ID == J.JOBH_ID).DefaultIfEmpty()
                          select new inspXjobcXbill
                          {
                              INSPH_ASET = a.ASET_CODE_NAME,
                              INSPH_ID = I.INSPH_ID,
                              INSPH_DATE = I.INSPH_DATE,
                              INSP_AMT = I.INSP_AMT,
                              JOBH_ID = J.JOBH_ID,
                              JOBH_DATE = J.JOBH_DATE,
                              JOBH_AMT = J.JOBH_AMT,
                              BILH_ID = B.BILH_ID,
                              BILH_DATE = B.BILH_DATE,
                              BILL_AMT = B.BILL_AMT,
                              INSPH_OWNR = o.OWNR_USER,
                              OWNR_NAME = o.OWNR_NAME,
                              INSPH_AREA = r.AREA_NAME,
                              INSPH_DEPARTMENT = d.DEPARTMENT_NAME,
                              INSPH_BUSINESS = d.A_BUSINESS.BUSINESS_NAME
                          }).ToList();

                OfficeOpenXml.ExcelPackage excel = new OfficeOpenXml.ExcelPackage();
                string dateformat = "dd/MM/yyyy";
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells[1, 1].LoadFromCollection(VS, true);

                var allCells = workSheet.Cells[1, 1, workSheet.Dimension.Start.Row, workSheet.Dimension.End.Column];
                var cellFont = allCells.Style.Font;
                cellFont.Size = 12;
                cellFont.Bold = true;
                cellFont.Color.SetColor(System.Drawing.Color.Blue);
                workSheet.Cells["A1"].Value = "Asset Name";
                workSheet.Cells["B1"].Value = "Insp Id";
                workSheet.Cells["C1"].Value = "Insp Date";
                workSheet.Cells["D1"].Value = "Insp Amount";
                workSheet.Cells["E1"].Value = "Job Id";
                workSheet.Cells["F1"].Value = "Job Date";
                workSheet.Cells["G1"].Value = "Job Amount";
                workSheet.Cells["H1"].Value = "Bill Id";
                workSheet.Cells["I1"].Value = "Bill Date";
                workSheet.Cells["J1"].Value = "Bill Amount";
                workSheet.Cells["K1"].Value = "User";
                workSheet.Cells["L1"].Value = "User Id";
                workSheet.Cells["M1"].Value = "Area";
                workSheet.Cells["N1"].Value = "Department";
                workSheet.Cells["O1"].Value = "Business";
                workSheet.Column(3).Style.Numberformat.Format = dateformat;
                workSheet.Column(6).Style.Numberformat.Format = dateformat;
                workSheet.Column(9).Style.Numberformat.Format = dateformat;
                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=" + fileName);
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            TempData["msg"] = "swal('Success..','Report Downloaded','success');";
            return View();
        }

        [KAuth(908)]
        public ActionResult Report_Insurance_Data()
        {
            string U = Session["UserId"].ToString();
            ViewBag.BUSINESS_ID = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
            return View();
        }
        [KAuth(908)]
        [HttpPost]
        public ActionResult Report_Insurance_Data(Business_Wise_Report BWR)
        {
            if (ModelState.IsValid)
            {
                var DT = DateTime.Now;
                string fileName = string.Format("Business_Wise_Insurance_{0}{1}{2}.xlsx", DT.Year, DT.Month.ToString().PadLeft(2, '0'), DT.Day.ToString().PadLeft(2, '0'));
                double DayRange = (BWR.T_BILH_DATE - BWR.F_BILH_DATE).TotalDays;
                if (DayRange >= 366)
                {
                    TempData["msg"] = string.Format("swal('Ohh..','You select {0} days, You can not select date range more than 1 year.','info');", DayRange);
                    return Redirect("Report_inspXjobcXbill");
                }
                List<Report_Insurance_Data> DATA = (from x in db.VW_RPT_INSURANCE.Where(x => x.INSU_DATE >= BWR.F_BILH_DATE && x.INSU_DATE <= BWR.T_BILH_DATE && x.BUSINESS_ID == BWR.BUSINESS_ID)
                                                    select new Report_Insurance_Data
                                                    {
                                                        ASET_CODE_NAME = x.ASET_CODE_NAME,
                                                        AREA_NAME = x.AREA_NAME,
                                                        DEPARTMENT_NAME = x.DEPARTMENT_NAME,
                                                        BUSINESS_NAME = x.BUSINESS_NAME,
                                                        INSU_ID = x.INSU_ID,
                                                        INSU_LINE = x.INSU_LINE,
                                                        INSU_ASET_VALUE = x.INSU_ASET_VALUE,
                                                        INSU_PREMIUM = x.INSU_PREMIUM,
                                                        INSU_VAT_AMOUNT = x.INSU_VAT_AMOUNT,
                                                        INSU_DATE = x.INSU_DATE,
                                                        INSU_PAID = x.INSU_PAID,
                                                        DACT = x.DACT,
                                                        INSU_POLICY = x.INSU_POLICY,
                                                        INSU_COMP = x.INSU_COMP,
                                                        INSU_COMP_ADDRESS = x.INSU_COMP_ADDRESS,
                                                        INSU_NOTE = x.INSU_NOTE,
                                                        INSU_REG_END = x.INSU_REG_END,
                                                        HACT = x.HACT
                                                    }
                            ).ToList();
                OfficeOpenXml.ExcelPackage excel = new OfficeOpenXml.ExcelPackage();
                string dateformat = "dd/MM/yyyy";
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells[1, 1].LoadFromCollection(DATA, true);

                var allCells = workSheet.Cells[1, 1, workSheet.Dimension.Start.Row, workSheet.Dimension.End.Column];
                var cellFont = allCells.Style.Font;
                cellFont.Size = 12;
                cellFont.Bold = true;
                cellFont.Color.SetColor(System.Drawing.Color.Blue);
                workSheet.Cells["A1"].Value = "Asset Name";
                workSheet.Cells["B1"].Value = "Area";
                workSheet.Cells["C1"].Value = "Department";
                workSheet.Cells["D1"].Value = "Business";
                workSheet.Cells["E1"].Value = "Insurance Id";
                workSheet.Cells["F1"].Value = "Insurance Line";
                workSheet.Cells["G1"].Value = "Asset Value";
                workSheet.Cells["H1"].Value = "Premium Value";
                workSheet.Cells["I1"].Value = "Vat";
                workSheet.Cells["J1"].Value = "Payment Date";
                workSheet.Cells["K1"].Value = "Paid";
                workSheet.Cells["L1"].Value = "Insurance Status";
                workSheet.Cells["M1"].Value = "Policy";
                workSheet.Cells["N1"].Value = "Company";
                workSheet.Cells["O1"].Value = "Address";
                workSheet.Cells["P1"].Value = "Note";
                workSheet.Cells["Q1"].Value = "Expire";
                workSheet.Cells["R1"].Value = "Company Status";
                workSheet.Column(10).Style.Numberformat.Format = dateformat;
                workSheet.Column(17).Style.Numberformat.Format = dateformat;
                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=" + fileName);
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            return View("Report_Asset_JobCard_Expense");
        }

        [KAuth(909)]
        public ActionResult Report_Police_Case()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            List<string> ASET = RP.MyAssetListString(UserId: U, UserType: T);
            ViewBag.ASET_ID = new SelectList(db.A_ASET_DATA.Where(x => ASET.Contains(x.ASET_ID)), "ASET_ID", "ASET_CODE_NAME");
            ViewBag.AREA_ID = new SelectList(RP.MyAreaList(UserId: U, UserType: T), "AREA_ID", "AREA_NAME");
            ViewBag.BUSINESS_ID = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
            List<string> TID = new List<string>();
            TID.Add("Y");
            TID.Add("N");
            ViewBag.TYPE_ID = new SelectList(TID);
            return View();
        }
        [KAuth(909)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Report_Police_Case(Report_Police_Case RPC)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (ModelState.IsValid)
            {

                var DT = DateTime.Now;
                string fileName = string.Format("Police_Case_{0}{1}{2}.xlsx", DT.Year, DT.Month.ToString().PadLeft(2, '0'), DT.Day.ToString().PadLeft(2, '0'));
                double DayRange = (RPC.T_BILH_DATE - RPC.F_BILH_DATE).TotalDays;
                if (DayRange >= 183)
                {
                    TempData["msg"] = string.Format("swal('Ohh..','You select {0} days, You can not select date range more than 6 months.','info');", DayRange);
                    return Redirect("Report_Asset_JobCard_Expense");
                }
                var TempStore = db.VW_RPT_POLICE_CASE.Where(x => x.PLEA_DATE >= RPC.F_BILH_DATE && x.PLEA_DATE <= RPC.T_BILH_DATE).ToList();
                if (RPC.ASET_ID != null)
                {
                    TempStore = TempStore.Where(a => a.PLEA_ASET == RPC.ASET_ID).ToList();
                }
                if (RPC.AREA_ID != null)
                {
                    TempStore = TempStore.Where(a => a.PLEA_AREA == RPC.AREA_ID).ToList();
                }
                if (RPC.BUSINESS_ID != null)
                {
                    TempStore = TempStore.Where(a => a.BUSINESS_ID == RPC.BUSINESS_ID).ToList();
                }
                if (RPC.TYPE_ID != null)
                {
                    TempStore = TempStore.Where(a => a.PLEA_STATUS == RPC.TYPE_ID).ToList();
                }
                List<Report_Police_Case_View> DATA = (from x in TempStore
                                                      select new Report_Police_Case_View
                                                      {
                                                          PLEA_ID = x.PLEA_ID,
                                                          PLEA_REF = x.PLEA_REF,
                                                          PLEA_DESCRIPTION = x.PLEA_DESCRIPTION,
                                                          PLEA_LOCATION = x.PLEA_LOCATION,
                                                          Case_On = x.Case_On,
                                                          Paid_By = x.Paid_By,
                                                          REASON_NAME = x.REASON_NAME,
                                                          OWNR_TEXT = x.OWNR_TEXT,
                                                          OWNR_USER = x.OWNR_USER,
                                                          ASET_CODE_NAME = x.ASET_CODE_NAME,
                                                          DEPARTMENT_NAME = x.DEPARTMENT_NAME,
                                                          BUSINESS_NAME = x.BUSINESS_NAME,
                                                          AREA_NAME = x.AREA_NAME,
                                                          PLEA_DATE = x.PLEA_DATE,
                                                          PLEA_STATUS = x.PLEA_STATUS,
                                                          Status = x.Status,
                                                          PLEA_APP_DATE = x.PLEA_APP_DATE,
                                                          Approval = x.Approval,
                                                          PLEA_AMNT = x.PLEA_AMNT
                                                      }
                            ).ToList();
                //Write Excel Data
                Write_Plea_Report_Excel(DATA: DATA, fileName: fileName);
            }
            List<string> ASET = RP.MyAssetListString(UserId: U, UserType: T);
            ViewBag.ASET_ID = new SelectList(db.A_ASET_DATA.Where(x => ASET.Contains(x.ASET_ID)), "ASET_ID", "ASET_CODE_NAME");
            ViewBag.AREA_ID = new SelectList(RP.MyAreaList(UserId: U, UserType: T), "AREA_ID", "AREA_NAME");
            ViewBag.BUSINESS_ID = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
            List<string> TID = new List<string>();
            TID.Add("Y");
            TID.Add("N");
            ViewBag.TYPE_ID = new SelectList(TID);
            return View();
        }
        public bool Write_Plea_Report_Excel(List<Report_Police_Case_View> DATA, string fileName)
        {
            OfficeOpenXml.ExcelPackage excel = new OfficeOpenXml.ExcelPackage();
            string dateformat = "dd/MM/yyyy";
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            workSheet.Cells[1, 1].LoadFromCollection(DATA, true);

            var allCells = workSheet.Cells[1, 1, workSheet.Dimension.Start.Row, workSheet.Dimension.End.Column];
            var cellFont = allCells.Style.Font;
            cellFont.Size = 12;
            cellFont.Bold = true;
            cellFont.Color.SetColor(System.Drawing.Color.Blue);
            workSheet.Cells["A1"].Value = "Case Id";
            workSheet.Cells["B1"].Value = "Violence No";
            workSheet.Cells["C1"].Value = "Description";
            workSheet.Cells["D1"].Value = "Location";
            workSheet.Cells["E1"].Value = "Case On";
            workSheet.Cells["F1"].Value = "Paid By";
            workSheet.Cells["G1"].Value = "Reason";
            workSheet.Cells["H1"].Value = "User Id";
            workSheet.Cells["I1"].Value = "User Name";
            workSheet.Cells["J1"].Value = "Asset";
            workSheet.Cells["K1"].Value = "Department";
            workSheet.Cells["L1"].Value = "Business";
            workSheet.Cells["M1"].Value = "Area";
            workSheet.Cells["N1"].Value = "Case Date";
            workSheet.Cells["O1"].Value = "Payement";
            workSheet.Cells["P1"].Value = "Status";
            workSheet.Cells["Q1"].Value = "App Date";
            workSheet.Cells["R1"].Value = "App Status";
            workSheet.Cells["S1"].Value = "Amount";
            workSheet.Column(14).Style.Numberformat.Format = dateformat;
            workSheet.Column(17).Style.Numberformat.Format = dateformat;
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=" + fileName);
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
            return true;
        }


        [KAuth(910)]
        public ActionResult Report_AssetList_KSA()
        {
            GenerateCommonViewBag();
            return View();
        }
        [KAuth(910)]
        [HttpPost]
        public ActionResult Report_AssetList_KSA(string ASET_GROUP, string ASET_ID, string AREA_ID, string BUSINESS_ID, string ASET_BODY, string ASET_BRAND, string ASET_FUEL, string ASET_COLOR, string ASET_USED, string ASET_CLOSED)
        {

            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            //List<string> MyAssetList = RP.MyAssetListString(UserId: U, UserType: T);
            List<string> MyDepartments = RP.MyDeptListString(UserId: U, UserType: T);

            List<string> aset_group = new List<string>();
            if (string.IsNullOrEmpty(ASET_GROUP))
            {
                aset_group = RP.MyGroupString(UserId: U, UserType: T);
            }
            else
            {
                aset_group.Add(ASET_GROUP);
            }
            string fileName = "Report_AssetList_KSA" + DateTime.Now.ToString() + ".xlsx";
            var TempStore = db.A_ASET_DATA.Where(x => aset_group.Contains(x.ASET_GROUP) && x.ASET_ID != "0").ToList();

            if (!string.IsNullOrEmpty(ASET_ID))
            {
                TempStore = TempStore.Where(a => a.ASET_ID == ASET_ID).ToList();
            }
            if (!string.IsNullOrEmpty(AREA_ID))
            {
                TempStore = TempStore.Where(a => a.ASET_AREA == AREA_ID).ToList();
            }
            if (!string.IsNullOrEmpty(BUSINESS_ID))
            {
                TempStore = TempStore.Where(a => a.A_DEPARTMENT.DEPARTMENT_BUSINESS == BUSINESS_ID).ToList();
            }
            else
            {
                TempStore = TempStore.Where(a => MyDepartments.Contains(a.ASET_DEPARTMENT)).ToList();
            }
            if (!string.IsNullOrEmpty(ASET_BODY))
            {
                TempStore = TempStore.Where(a => a.ASET_BODY == ASET_BODY).ToList();
            }
            if (!string.IsNullOrEmpty(ASET_BRAND))
            {
                TempStore = TempStore.Where(a => a.ASET_BRAND == ASET_BRAND).ToList();
            }
            if (!string.IsNullOrEmpty(ASET_FUEL))
            {
                TempStore = TempStore.Where(a => a.ASET_FUEL == ASET_FUEL).ToList();
            }
            if (!string.IsNullOrEmpty(ASET_COLOR))
            {
                TempStore = TempStore.Where(a => a.ASET_COLOR == ASET_COLOR).ToList();
            }
            if (!string.IsNullOrEmpty(ASET_USED))
            {
                bool rslt = true;
                if (ASET_USED == "0")
                {
                    rslt = false;
                }
                TempStore = TempStore.Where(a => a.ASET_USED == rslt).ToList();
            }
            if (!string.IsNullOrEmpty(ASET_CLOSED))
            {
                bool rslt = true;
                if (ASET_CLOSED == "0")
                {
                    rslt = false;
                }
                TempStore = TempStore.Where(a => a.ACT == rslt).ToList();
            }

            List<CarVM> DATA = (from a in TempStore
                                select new CarVM
                                {
                                    ASET_ID = a.ASET_ID,
                                    ASET_TAG = a.ASET_TAG,
                                    ASET_CODE_NAME = a.ASET_CODE_NAME,
                                    ASET_AREA = a.A_AREA.AREA_NAME,//area
                                    ASET_BODY = a.A_BODY.BODY_NAME,//body
                                    ASET_BRAND = a.A_BRAND.BRAND_NAME,//body
                                    ASET_FUEL = a.A_FUEL.FUEL_NAME,//fuel
                                    ASET_COLOR = a.A_COLOR.COLOR_NAME,//color
                                    ASET_GROUP = a.A_GROUP.GROUP_NAME,//group
                                    ASET_MODEL = a.ASET_MODEL,
                                    ASET_PO_ENGINE = a.ASET_PO_ENGINE,
                                    ASET_GRN_CHASIS = a.ASET_GRN_CHASIS,
                                    ASET_CAPACITY = a.ASET_CAPACITY,
                                    ASET_PRICE = a.ASET_PRICE,
                                    ASET_REG_DATE = a.ASET_REG_DATE,
                                    ASET_HEIGHT = a.ASET_HEIGHT,
                                    ASET_WEIGHT = a.ASET_WEIGHT,
                                    ASET_MFG_YEAR = a.ASET_MFG_YEAR,
                                    ASET_IMAGE = a.ASET_IMAGE,
                                    ASET_NOTE = a.ASET_NOTE,
                                    ASET_USED = a.ASET_USED,
                                    ACT = a.ACT,
                                    DEPARTMENT_V = a.A_DEPARTMENT.DEPARTMENT_NAME,//depeartment
                                    BUSINESS_V = a.A_DEPARTMENT.A_BUSINESS.BUSINESS_NAME,
                                    COMPANY_V = a.A_DEPARTMENT.A_BUSINESS.A_COMPANY.COMPANY_NAME
                                }
               ).ToList();

            //Write Excel Data

            OfficeOpenXml.ExcelPackage excel = new OfficeOpenXml.ExcelPackage();
            string dateformat = "dd/MM/yyyy";
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            workSheet.Cells[1, 1].LoadFromCollection(DATA, true);

            var allCells = workSheet.Cells[1, 1, workSheet.Dimension.Start.Row, workSheet.Dimension.End.Column];
            var cellFont = allCells.Style.Font;
            cellFont.Size = 12;
            cellFont.Bold = true;
            cellFont.Color.SetColor(System.Drawing.Color.Blue);
            workSheet.Cells["A1"].Value = "Id";
            workSheet.Cells["B1"].Value = "Tag";
            workSheet.Cells["C1"].Value = "Name";
            workSheet.Cells["D1"].Value = "Area";
            workSheet.Cells["E1"].Value = "Body";
            workSheet.Cells["F1"].Value = "Brand";
            workSheet.Cells["G1"].Value = "Fuel";
            workSheet.Cells["H1"].Value = "Color";
            workSheet.Cells["I1"].Value = "Group";
            workSheet.Cells["J1"].Value = "Model";
            workSheet.Cells["K1"].Value = "Engine";
            workSheet.Cells["L1"].Value = "Chasis";
            workSheet.Cells["M1"].Value = "Capacity";
            workSheet.Cells["N1"].Value = "Price";
            workSheet.Cells["O1"].Value = "Reg Date";
            workSheet.Cells["P1"].Value = "Height";
            workSheet.Cells["Q1"].Value = "Weight";
            workSheet.Cells["R1"].Value = "MFG Year";
            workSheet.Cells["S1"].Value = "Image";
            workSheet.Cells["T1"].Value = "Note";
            workSheet.Cells["U1"].Value = "In Use";
            workSheet.Cells["V1"].Value = "Status";
            workSheet.Cells["W1"].Value = "Department";
            workSheet.Cells["X1"].Value = "Business/Sponsor";
            workSheet.Cells["Y1"].Value = "Company/Owner";
            workSheet.Column(15).Style.Numberformat.Format = dateformat;
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=" + fileName);
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }

            return View("Report_AssetList_KSA");
        }

        [KAuth(914)]
        public ActionResult Report_Insurance_List()
        {
            GenerateInsuranceViewBag();
            return View();
        }

        [KAuth(914)]
        [HttpPost]
        public ActionResult Report_Insurance_List(string COMPANY_ID, string INSURANCE_TYPE_ID, string INSURANCE_COMPANY_ID, string CLAIM_STATUS, string KICK_BACK_STATUS, DateTime? FROM_DATE, DateTime? TO_DATE)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            if (FROM_DATE != null && TO_DATE != null)
            {
                string fileName = "Report_Insurance_List" + DateTime.Now.ToString() + ".xlsx";
                var TempStore = db.INS_INSURANCE_MST.Where(x => x.ACT == true).ToList();

                if (!string.IsNullOrEmpty(COMPANY_ID))
                {
                    TempStore = TempStore.Where(a => a.COMPANY_ID == COMPANY_ID && a.INSURANE_DATE >= FROM_DATE && a.INSURANE_DATE <= TO_DATE).ToList();
                }
                if (!string.IsNullOrEmpty(INSURANCE_TYPE_ID))
                {
                    TempStore = TempStore.Where(a => a.INSURANCE_TYPE_ID == INSURANCE_TYPE_ID && a.INSURANE_DATE >= FROM_DATE && a.INSURANE_DATE <= TO_DATE).ToList();
                }
                if (!string.IsNullOrEmpty(INSURANCE_COMPANY_ID))
                {
                    TempStore = TempStore.Where(a => a.INSURANCE_COMPANY_ID == INSURANCE_COMPANY_ID && a.INSURANE_DATE >= FROM_DATE && a.INSURANE_DATE <= TO_DATE).ToList();
                }
                if (!string.IsNullOrEmpty(KICK_BACK_STATUS))
                {
                    TempStore = TempStore.Where(a => a.KICK_BACK_STATUS == KICK_BACK_STATUS && a.INSURANE_DATE >= FROM_DATE && a.INSURANE_DATE <= TO_DATE).ToList();
                }

                if (!string.IsNullOrEmpty(CLAIM_STATUS))
                {
                    bool rslt = true;
                    if (CLAIM_STATUS == "0")
                    {
                        rslt = false;
                    }
                    TempStore = TempStore.Where(a => a.CLAIM_STATUS == rslt && a.INSURANE_DATE >= FROM_DATE && a.INSURANE_DATE <= TO_DATE).ToList();
                }
                if (string.IsNullOrEmpty(COMPANY_ID) && string.IsNullOrEmpty(INSURANCE_TYPE_ID) && string.IsNullOrEmpty(INSURANCE_COMPANY_ID) && string.IsNullOrEmpty(KICK_BACK_STATUS) && string.IsNullOrEmpty(CLAIM_STATUS))
                {
                    TempStore = TempStore.Where(a => a.INSURANE_DATE >= FROM_DATE && a.INSURANE_DATE <= TO_DATE).ToList();
                }

                List<InsuranceReportVM> DATA = (from a in TempStore
                                                select new InsuranceReportVM
                                                {
                                                    INSURANCE_MST_ID = a.INSURANCE_MST_ID,
                                                    INSURANCE_NUMBER = a.INSURANCE_NUMBER,
                                                    INSURANE_DATE = a.INSURANE_DATE.HasValue ? (DateTime)a.INSURANE_DATE : default(DateTime),
                                                    INSURANCE_EXPAIR_DATE = a.INSURANCE_EXPAIR_DATE.HasValue ? (DateTime)a.INSURANCE_EXPAIR_DATE : default(DateTime),
                                                    INSURANCE_AMOUNT = a.INSURANCE_AMOUNT,
                                                    NET_PREMIUM = a.NET_PREMIUM,
                                                    VAT = a.VAT,
                                                    STAMP = a.STAMP,
                                                    DESCRIPTION = a.DESCRIPTION,
                                                    KICK_BACK_AMOUNT = a.KICK_BACK_AMOUNT,
                                                    KICK_BACK_PERCENTAGE = a.KICK_BACK_PERCENTAGE,
                                                    BENIFICIARY_DETAILS = a.BENIFICIARY_DETAILS,
                                                    INSURED_ITEM = a.INSURED_ITEM,
                                                    OCCUPATION = a.OCCUPATION,

                                                    // Check if INS_INSURANCE_COMPANY is not null before accessing INS_COMPANY_NAME
                                                    INSURANCE_COMPANY_ID = a.INS_INSURANCE_COMPANY != null ? a.INS_INSURANCE_COMPANY.INS_COMPANY_NAME : null,

                                                    // Check if INS_INSURANCE_TYPE is not null before accessing INSURANCE_TYPE_NAME
                                                    INSURANCE_TYPE_ID = a.INS_INSURANCE_TYPE != null ? a.INS_INSURANCE_TYPE.INSURANCE_TYPE_NAME : null,

                                                    // Check if INS_COMPANY is not null before accessing COMPANY_NAME
                                                    COMPANY_ID = a.INS_COMPANY != null ? a.INS_COMPANY.COMPANY_NAME : null,

                                                    // Check if INS_KICK_BACK_STATUS is not null before accessing KICK_BACK_STATUS_NAME
                                                    KICK_BACK_STATUS = a.INS_KICK_BACK_STATUS != null ? a.INS_KICK_BACK_STATUS.KICK_BACK_STATUS_NAME : null,

                                                    CLAIM_STATUS = a.CLAIM_STATUS,
                                                    REQUEST_BY = a.REQUEST_BY,
                                                    TOTAL_PREMIUM = (a.NET_PREMIUM + a.VAT + a.STAMP),
                                                    ACTUAL_PAID_AMOUNT = (a.NET_PREMIUM + a.VAT + a.STAMP) - a.KICK_BACK_AMOUNT
                                                }).ToList();

                //Write Excel Data

                OfficeOpenXml.ExcelPackage excel = new OfficeOpenXml.ExcelPackage();
                string dateformat = "dd/MM/yyyy";
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells[1, 1].LoadFromCollection(DATA, true);

                var allCells = workSheet.Cells[1, 1, workSheet.Dimension.Start.Row, workSheet.Dimension.End.Column];
                var cellFont = allCells.Style.Font;
                cellFont.Size = 12;
                cellFont.Bold = true;
                cellFont.Color.SetColor(System.Drawing.Color.Blue);
                workSheet.Cells["A1"].Value = "Id";
                workSheet.Cells["B1"].Value = "Benificiary Name";
                workSheet.Cells["C1"].Value = "Type of Insurance";
                workSheet.Cells["D1"].Value = "Insurance Company";

                workSheet.Cells["E1"].Value = "Insurance Number";
                workSheet.Cells["F1"].Value = "Registration Date";
                workSheet.Cells["G1"].Value = "Expire Date";
                workSheet.Cells["H1"].Value = "Insurred Amount";
                workSheet.Cells["I1"].Value = "Net Premium";
                workSheet.Cells["J1"].Value = "VAT";
                workSheet.Cells["K1"].Value = "STAMP";
                workSheet.Cells["L1"].Value = "Description";
                workSheet.Cells["M1"].Value = "Discount Amount";
                workSheet.Cells["N1"].Value = "Discount (%)";
                workSheet.Cells["O1"].Value = "Benificiary Details";
                workSheet.Cells["P1"].Value = "Insured Item";
                workSheet.Cells["Q1"].Value = "Occupation";
                workSheet.Cells["R1"].Value = "Discount Status";
                workSheet.Cells["S1"].Value = "Claim Status";
                workSheet.Cells["T1"].Value = "Request BY";
                workSheet.Cells["U1"].Value = "Total Premium";
                workSheet.Cells["V1"].Value = "Actual Paid Amount";


                workSheet.Column(6).Style.Numberformat.Format = dateformat;
                workSheet.Column(7).Style.Numberformat.Format = dateformat;
                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=" + fileName);
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }

                return View("Report_Insurance_List");
            }
            else
            {
                string fileName = "Report_Insurance_List" + DateTime.Now.ToString() + ".xlsx";
                var TempStore = db.INS_INSURANCE_MST.Where(x => x.ACT == true).ToList();

                if (!string.IsNullOrEmpty(COMPANY_ID))
                {
                    TempStore = TempStore.Where(a => a.COMPANY_ID == COMPANY_ID).ToList();
                }
                if (!string.IsNullOrEmpty(INSURANCE_TYPE_ID))
                {
                    TempStore = TempStore.Where(a => a.INSURANCE_TYPE_ID == INSURANCE_TYPE_ID).ToList();
                }
                if (!string.IsNullOrEmpty(INSURANCE_COMPANY_ID))
                {
                    TempStore = TempStore.Where(a => a.INSURANCE_COMPANY_ID == INSURANCE_COMPANY_ID).ToList();
                }
                if (!string.IsNullOrEmpty(KICK_BACK_STATUS))
                {
                    TempStore = TempStore.Where(a => a.KICK_BACK_STATUS == KICK_BACK_STATUS).ToList();
                }

                if (!string.IsNullOrEmpty(CLAIM_STATUS))
                {
                    bool rslt = true;
                    if (CLAIM_STATUS == "0")
                    {
                        rslt = false;
                    }
                    TempStore = TempStore.Where(a => a.CLAIM_STATUS == rslt).ToList();
                }


                List<InsuranceReportVM> DATA = (from a in TempStore
                                                select new InsuranceReportVM
                                                {
                                                    INSURANCE_MST_ID = a.INSURANCE_MST_ID,
                                                    INSURANCE_NUMBER = a.INSURANCE_NUMBER,
                                                    INSURANE_DATE = a.INSURANE_DATE.HasValue ? (DateTime)a.INSURANE_DATE : default(DateTime),
                                                    INSURANCE_EXPAIR_DATE = a.INSURANCE_EXPAIR_DATE.HasValue ? (DateTime)a.INSURANCE_EXPAIR_DATE : default(DateTime),
                                                    INSURANCE_AMOUNT = a.INSURANCE_AMOUNT,
                                                    NET_PREMIUM = a.NET_PREMIUM,
                                                    VAT = a.VAT,
                                                    STAMP = a.STAMP,
                                                    DESCRIPTION = a.DESCRIPTION,
                                                    KICK_BACK_AMOUNT = a.KICK_BACK_AMOUNT,
                                                    KICK_BACK_PERCENTAGE = a.KICK_BACK_PERCENTAGE,
                                                    BENIFICIARY_DETAILS = a.BENIFICIARY_DETAILS,
                                                    INSURED_ITEM = a.INSURED_ITEM,
                                                    OCCUPATION = a.OCCUPATION,

                                                    // Check if INS_INSURANCE_COMPANY is not null before accessing INS_COMPANY_NAME
                                                    INSURANCE_COMPANY_ID = a.INS_INSURANCE_COMPANY != null ? a.INS_INSURANCE_COMPANY.INS_COMPANY_NAME : null,

                                                    // Check if INS_INSURANCE_TYPE is not null before accessing INSURANCE_TYPE_NAME
                                                    INSURANCE_TYPE_ID = a.INS_INSURANCE_TYPE != null ? a.INS_INSURANCE_TYPE.INSURANCE_TYPE_NAME : null,

                                                    // Check if INS_COMPANY is not null before accessing COMPANY_NAME
                                                    COMPANY_ID = a.INS_COMPANY != null ? a.INS_COMPANY.COMPANY_NAME : null,

                                                    // Check if INS_KICK_BACK_STATUS is not null before accessing KICK_BACK_STATUS_NAME
                                                    KICK_BACK_STATUS = a.INS_KICK_BACK_STATUS != null ? a.INS_KICK_BACK_STATUS.KICK_BACK_STATUS_NAME : null,

                                                    CLAIM_STATUS = a.CLAIM_STATUS,
                                                    REQUEST_BY = a.REQUEST_BY,
                                                    TOTAL_PREMIUM = (a.NET_PREMIUM + a.VAT + a.STAMP),
                                                    ACTUAL_PAID_AMOUNT = (a.NET_PREMIUM + a.VAT + a.STAMP) - a.KICK_BACK_AMOUNT
                                                }).ToList();

                //Write Excel Data

                OfficeOpenXml.ExcelPackage excel = new OfficeOpenXml.ExcelPackage();
                string dateformat = "dd/MM/yyyy";
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells[1, 1].LoadFromCollection(DATA, true);

                var allCells = workSheet.Cells[1, 1, workSheet.Dimension.Start.Row, workSheet.Dimension.End.Column];
                var cellFont = allCells.Style.Font;
                cellFont.Size = 12;
                cellFont.Bold = true;
                cellFont.Color.SetColor(System.Drawing.Color.Blue);
                workSheet.Cells["A1"].Value = "Id";
                workSheet.Cells["B1"].Value = "Benificiary Name";
                workSheet.Cells["C1"].Value = "Type of Insurance";
                workSheet.Cells["D1"].Value = "Insurance Company";

                workSheet.Cells["E1"].Value = "Insurance Number";
                workSheet.Cells["F1"].Value = "Registration Date";
                workSheet.Cells["G1"].Value = "Expire Date";
                workSheet.Cells["H1"].Value = "Insurred Amount";
                workSheet.Cells["I1"].Value = "Net Premium";
                workSheet.Cells["J1"].Value = "VAT";
                workSheet.Cells["K1"].Value = "STAMP";
                workSheet.Cells["L1"].Value = "Description";
                workSheet.Cells["M1"].Value = "Discount Amount";
                workSheet.Cells["N1"].Value = "Discount (%)";
                workSheet.Cells["O1"].Value = "Benificiary Details";
                workSheet.Cells["P1"].Value = "Insured Item";
                workSheet.Cells["Q1"].Value = "Occupation";
                workSheet.Cells["R1"].Value = "Discount Status";
                workSheet.Cells["S1"].Value = "Claim Status";
                workSheet.Cells["T1"].Value = "Request BY";
                workSheet.Cells["U1"].Value = "Total Premium";
                workSheet.Cells["V1"].Value = "Actual Paid Amount";

                workSheet.Column(6).Style.Numberformat.Format = dateformat;
                workSheet.Column(7).Style.Numberformat.Format = dateformat;
                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=" + fileName);
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }

                return View("Report_Insurance_List");
            }
        }

        [KAuth(109)]
        [HttpPost]
        public ActionResult Report_Closed_Bike()
        {
            string U = Session["UserId"].ToString();
            string Sql = string.Format(@"select a.aset_code_name, a.aset_capacity, a.aset_reg_date,d1.department_name aset_department,b1.business_name aset_business,
            o.ownr_text,o.ownr_user,d2.department_name ownr_department,b2.business_name ownr_business, d.apply_date,
            d.apply_reason, d.apply_type, d.service_month
            from pm.a_aset_app d
            join pm.a_aset_data a on d.aset_id = a.aset_id
            and a.act = '0'
            join a_department d1 on a.aset_department=d1.department_id
            join a_business b1 on d1.department_business=b1.business_id
            join pm.a_ownr o on d.ownr_id=o.ownr_id
            join a_department d2 on o.ownr_dept=d2.department_id
            join a_business b2 on d2.department_business=b2.business_id
            join pm.vw_user_asset_id_string p on d.aset_id=p.aset_id
            where p.ug_user='{0}'", U);
            var DT = DateTime.Now;
            string fileName = string.Format("Report_Closed_Bike_{0}{1}{2}.xlsx", DT.Year, DT.Month.ToString().PadLeft(2, '0'), DT.Day.ToString().PadLeft(2, '0'));

            var DATA = db.Database.SqlQuery<Report_Closed_Bike>(sql: Sql).ToList();

            OfficeOpenXml.ExcelPackage excel = new OfficeOpenXml.ExcelPackage();
            string dateformat = "dd/MM/yyyy";
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            workSheet.Cells[1, 1].LoadFromCollection(DATA, true);

            var allCells = workSheet.Cells[1, 1, workSheet.Dimension.Start.Row, workSheet.Dimension.End.Column];
            var cellFont = allCells.Style.Font;
            cellFont.Size = 12;
            cellFont.Bold = true;
            cellFont.Color.SetColor(System.Drawing.Color.Blue);
            workSheet.Cells["A1"].Value = "Asset Name";
            workSheet.Cells["B1"].Value = "Capacity";
            workSheet.Cells["C1"].Value = "Reg. Date";
            workSheet.Cells["D1"].Value = "Asset Dept";
            workSheet.Cells["E1"].Value = "Asset Business";
            workSheet.Cells["F1"].Value = "Owner Id";
            workSheet.Cells["G1"].Value = "Owner Name";
            workSheet.Cells["H1"].Value = "Owner Dept";
            workSheet.Cells["I1"].Value = "Owner Business";
            workSheet.Cells["J1"].Value = "Apply Date";
            workSheet.Cells["K1"].Value = "Note";
            workSheet.Cells["L1"].Value = "Closing Type";
            workSheet.Cells["M1"].Value = "Service Month";
            workSheet.Column(3).Style.Numberformat.Format = dateformat;
            workSheet.Column(10).Style.Numberformat.Format = dateformat;
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=" + fileName);
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
            return View();
        }


        [KAuth(911)]
        [HttpGet]
        public ActionResult Report_StaffList_KSA()
        {
            GenerateCommonViewBag();
            return View();
        }
        [KAuth(911)]
        [HttpPost]
        public ActionResult Report_StaffList_KSA(string AREA_ID, string BUSINESS_ID, string ASET_USED, string ASET_CLOSED, string REMIND)
        {
            string fileName = "Report_StaffList_KSA" + DateTime.Now.ToString() + ".xlsx";
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            var Areas = RP.MyAreaList(UserId: U, UserType: T).ToList();
            List<string> AREA = new List<string>();
            foreach (var item in Areas)
            {
                AREA.Add(item.AREA_ID);
                AREA.Add(item.AREA_NAME);
            }
            var TempStore = db.A_OWNR.Where(x => x.OWNR_TYPE == "Staff" && AREA.Contains(x.OWNR_AREA)).ToList();

            if (!string.IsNullOrEmpty(AREA_ID))
            {
                TempStore = TempStore.Where(a => a.OWNR_AREA == AREA_ID).ToList();
            }
            if (!string.IsNullOrEmpty(BUSINESS_ID))
            {
                TempStore = TempStore.Where(a => a.A_DEPARTMENT.DEPARTMENT_BUSINESS == BUSINESS_ID).ToList();
            }
            if (!string.IsNullOrEmpty(ASET_USED)) //use a car
            {
                bool rslt = true;
                if (ASET_USED == "0")
                {
                    rslt = false;
                }
                TempStore = TempStore.Where(a => a.OWNR_POST == rslt).ToList();
            }
            if (!string.IsNullOrEmpty(ASET_CLOSED))
            {
                bool rslt = true;
                if (ASET_CLOSED == "0")
                {
                    rslt = false;
                }
                TempStore = TempStore.Where(a => a.ACT == rslt).ToList();
            }
            if (!string.IsNullOrEmpty(REMIND))
            {
                if (ASET_CLOSED == "0")
                {
                    TempStore = TempStore.Where(a => a.REMIND == 0).ToList();
                }
                else
                {
                    TempStore = TempStore.Where(a => a.REMIND >= 1).ToList();
                }
            }

            List<Report_StaffList_KSA> DATA = (from a in TempStore
                                               select new Report_StaffList_KSA
                                               {
                                                   OWNR_TEXT = a.OWNR_TEXT,
                                                   OWNR_USER = a.OWNR_USER,
                                                   OWNR_NAME = a.OWNR_NAME,
                                                   OWNR_DESG = a.OWNR_DESG,
                                                   OWNR_CONT = a.OWNR_CONT,
                                                   OWNR_EMAIL = a.OWNR_EMAIL,
                                                   OWNER_AREA = a.A_AREA.AREA_NAME,
                                                   OWNER_COMPANY = a.A_DEPARTMENT.A_BUSINESS.A_COMPANY.COMPANY_NAME,
                                                   OWNR_ADDR = a.OWNR_ADDR,
                                                   OWNR_NOTE = a.OWNR_NOTE,
                                                   OWNR_TYPE = a.OWNR_TYPE,
                                                   OWNR_NDATE = a.OWNR_NDATE,
                                                   OWNR_RDATE = a.OWNR_RDATE,
                                                   OWNR_POST = a.OWNR_POST.ToString(),
                                                   ACT = a.ACT.ToString(),
                                                   REMIND = a.REMIND.ToString()
                                               }).ToList();
            foreach (var item in DATA)
            {
                if (item.REMIND == "0")
                {
                    item.REMIND = "Running";
                }
                else if (item.REMIND == "1")
                {
                    item.REMIND = "First Reminder";
                }
                else
                {
                    item.REMIND = "Final Reminder";
                }

                if (item.OWNR_POST == "True")
                {
                    item.OWNR_POST = "Used";
                }
                else
                {
                    item.OWNR_POST = "Non Used";
                }

                if (item.ACT == "True")
                {
                    item.ACT = "Active";
                }
                else
                {
                    item.ACT = "Closed";
                }
            }


            //Write Excel Data

            OfficeOpenXml.ExcelPackage excel = new OfficeOpenXml.ExcelPackage();
            string dateformat = "dd/MM/yyyy";
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            workSheet.Cells[1, 1].LoadFromCollection(DATA, true);

            var allCells = workSheet.Cells[1, 1, workSheet.Dimension.Start.Row, workSheet.Dimension.End.Column];
            var cellFont = allCells.Style.Font;
            cellFont.Size = 12;
            cellFont.Bold = true;
            cellFont.Color.SetColor(System.Drawing.Color.Blue);
            workSheet.Cells["A1"].Value = "Staff Id";
            workSheet.Cells["B1"].Value = "Staff Name";
            workSheet.Cells["C1"].Value = "Staff Iquama";
            workSheet.Cells["D1"].Value = "Designation";
            workSheet.Cells["E1"].Value = "Mobile";
            workSheet.Cells["F1"].Value = "Email";
            workSheet.Cells["G1"].Value = "Area";
            workSheet.Cells["H1"].Value = "Company";
            workSheet.Cells["I1"].Value = "Address";
            workSheet.Cells["J1"].Value = "Note";
            workSheet.Cells["K1"].Value = "Type";
            workSheet.Cells["L1"].Value = "Iquama Expire Date";
            workSheet.Cells["M1"].Value = "Resign Date";
            workSheet.Cells["N1"].Value = "Use Status";
            workSheet.Cells["O1"].Value = "Status";
            workSheet.Cells["P1"].Value = "Iquma Expire";
            workSheet.Column(12).Style.Numberformat.Format = dateformat;
            workSheet.Column(13).Style.Numberformat.Format = dateformat;
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=" + fileName);
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }

            return View("Report_StaffList_KSA");

        }



        [KAuth(912)]
        [HttpGet]
        public ActionResult Report_InsuranceBD_By_Date()
        {
            return View();
        }

        [KAuth(912)]
        [HttpPost]
        public ActionResult Report_InsuranceBD_By_Date(Report_Paid_Bills rpb)
        {
            string U = Session["UserId"].ToString();
            DateTime DT = DateTime.Now;
            string fileName = string.Format("Report_InsuranceBD_By_Date_{0}{1}{2}.xlsx", DT.Year, DT.Month.ToString().PadLeft(2, '0'), DT.Day.ToString().PadLeft(2, '0'));
            //     string Sql = string.Format(@"select i.insu_id, a.aset_code_name, i.insu_policy, a.aset_capacity,
            //i.insu_premium, i.insu_comp, i.insu_comp_address, i.insu_note,
            //i.insu_reg_start, i.insu_reg_end
            //   from a_insuh i join vw_user_asset_id_string p on i.insu_aset = p.aset_id
            //        join a_aset_data a on i.insu_aset = a.aset_id
            //  where p.ug_user = '{0}' and i.insu_reg_end between to_date('{1}','dd/mm/YYYY HH:MI:SS AM') and to_date('{2}','dd/mm/YYYY HH:MI:SS AM')", U,rpb.F_BILH_DATE, rpb.T_BILH_DATE);
            var DATA = (from i in db.A_INSUH
                        join a in db.VW_USER_ASSET_ID_STRING on i.INSU_ASET equals a.ASET_ID
                        join s in db.A_ASET_DATA on a.ASET_ID equals s.ASET_ID
                        where i.INSU_REG_END >= rpb.F_BILH_DATE && i.INSU_REG_END <= rpb.T_BILH_DATE && a.UG_USER == U
                        select new InsuranceVMBDRpt
                        {
                            INSU_ID = i.INSU_ID,
                            ASET_CODE_NAME = s.ASET_CODE_NAME,
                            INSU_POLICY = i.INSU_POLICY,
                            ASET_CAPACITY = s.ASET_CAPACITY,
                            INSU_PREMIUM = i.INSU_PREMIUM ?? 0,
                            INSU_COMP = i.INSU_COMP,
                            INSU_COMP_ADDRESS = i.INSU_COMP_ADDRESS,
                            INSU_NOTE = i.INSU_NOTE,
                            INSU_REG_START = i.INSU_REG_START ?? DateTime.Now,
                            INSU_REG_END = i.INSU_REG_END ?? DateTime.Now
                        }).ToList().OrderBy(x => x.ASET_CODE_NAME);


            //  List<InsuranceVMBDRpt> DATA = db.Database.SqlQuery<InsuranceVMBDRpt>(sql: Sql).ToList();

            OfficeOpenXml.ExcelPackage excel = new OfficeOpenXml.ExcelPackage();
            string dateformat = "dd/MM/yyyy";
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            workSheet.Cells[1, 1].LoadFromCollection(DATA, true);

            var allCells = workSheet.Cells[1, 1, workSheet.Dimension.Start.Row, workSheet.Dimension.End.Column];
            var cellFont = allCells.Style.Font;
            cellFont.Size = 12;
            cellFont.Bold = true;
            cellFont.Color.SetColor(System.Drawing.Color.Blue);
            workSheet.Cells["A1"].Value = "Insurance Id";
            workSheet.Cells["B1"].Value = "Asset Name";
            workSheet.Cells["C1"].Value = "Policy Number";
            workSheet.Cells["D1"].Value = "Type";
            workSheet.Cells["E1"].Value = "Premium";
            workSheet.Cells["F1"].Value = "Insurance Company";
            workSheet.Cells["G1"].Value = "Company Address";
            workSheet.Cells["H1"].Value = "Note";
            workSheet.Cells["I1"].Value = "Insurance Date";
            workSheet.Cells["J1"].Value = "Insurance Expire Date";
            workSheet.Column(9).Style.Numberformat.Format = dateformat;
            workSheet.Column(10).Style.Numberformat.Format = dateformat;
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=" + fileName);
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
            return View();
        }


        [KAuth(912)]
        [HttpPost]
        public ActionResult Report_Insurance_By_Company_Detail(Report_Paid_Bills rpb)
        {
            string U = Session["UserId"].ToString();
            //string Sql = string.Format(@"select b.business_name,d.department_name, s.aset_code_name, s.aset_capacity, i.insu_policy,
            //        i.insu_premium, i.insu_comp, i.insu_reg_start, i.insu_reg_end
            //        from a_insuh i join vw_user_asset_id_string a
            //        on i.insu_aset = a.aset_id and a.ug_user = '{0}'
            //        join a_aset_data s on a.aset_id = s.aset_id
            //        join a_department d on s.aset_department = d.department_id
            //        join a_business b on d.department_business = b.business_id
            //        where i.insu_reg_end between to_date('{1}','dd/mm/YYYY HH:MI:SS AM') and to_date('{2}','dd/mm/YYYY HH:MI:SS AM')", U, rpb.F_BILH_DATE, rpb.T_BILH_DATE);


            var SqlLink = (from i in db.A_INSUH
                           join a in db.VW_USER_ASSET_ID_STRING on i.INSU_ASET equals a.ASET_ID
                           join s in db.A_ASET_DATA on a.ASET_ID equals s.ASET_ID
                           join d in db.A_DEPARTMENT on s.ASET_DEPARTMENT equals d.DEPARTMENT_ID
                           join b in db.A_BUSINESS on d.DEPARTMENT_BUSINESS equals b.BUSINESS_ID
                           where i.INSU_REG_END >= rpb.F_BILH_DATE && i.INSU_REG_END <= rpb.T_BILH_DATE && a.UG_USER == U
                           select new Report_Insurance_By_Company_Detail
                           {
                               BUSINESS_NAME = b.BUSINESS_DESC,
                               DEPARTMENT_NAME = d.DEPARTMENT_NAME,
                               ASET_CODE_NAME = s.ASET_CODE_NAME,
                               ASET_CAPACITY = s.ASET_CAPACITY,
                               INSU_POLICY = i.INSU_POLICY,
                               INSU_PREMIUM = i.INSU_PREMIUM ?? 0,
                               INSU_COMP = i.INSU_COMP,
                               INSU_REG_START = i.INSU_REG_START ?? DateTime.Now,
                               INSU_REG_END = i.INSU_REG_END ?? DateTime.Now
                           }).ToList().OrderBy(x => x.BUSINESS_NAME);
            //       List<Report_Insurance_By_Company_Detail> d = db.Database.SqlQuery<Report_Insurance_By_Company_Detail>(Sql).ToList();
            return View(SqlLink);
        }


        [KAuth(912)]
        [HttpPost]
        public ActionResult Report_Insurance_By_Company_Summary(Report_Paid_Bills rpb)
        {
            string U = Session["UserId"].ToString();
            //string Sql = string.Format(@"select b.business_name, sum(i.insu_premium)insu_premium
            //        from a_insuh i join vw_user_asset_id_string a
            //        on i.insu_aset = a.aset_id and a.ug_user = '{0}'
            //        join a_aset_data s on a.aset_id = s.aset_id
            //        join a_department d on s.aset_department = d.department_id
            //        join a_business b on d.department_business = b.business_id
            //        where i.insu_reg_end between to_date('{1}','dd/mm/YYYY HH:MI:SS AM') and to_date('{2}','dd/mm/YYYY HH:MI:SS AM')
            //        group by b.business_name", U, rpb.F_BILH_DATE, rpb.T_BILH_DATE);

            var SqlLink = (from i in db.A_INSUH
                           join a in db.VW_USER_ASSET_ID_STRING on i.INSU_ASET equals a.ASET_ID
                           join s in db.A_ASET_DATA on a.ASET_ID equals s.ASET_ID
                           join d in db.A_DEPARTMENT on s.ASET_DEPARTMENT equals d.DEPARTMENT_ID
                           join b in db.A_BUSINESS on d.DEPARTMENT_BUSINESS equals b.BUSINESS_ID
                           where i.INSU_REG_END >= rpb.F_BILH_DATE && i.INSU_REG_END <= rpb.T_BILH_DATE && a.UG_USER == U
                           group i by b.BUSINESS_DESC into g
                           select new Report_Insurance_By_Company_Summary
                           {
                               BUSINESS_NAME = g.Key,
                               INSU_PREMIUM = g.Sum(i => i.INSU_PREMIUM) ?? 0
                           }).ToList().OrderBy(x => x.BUSINESS_NAME);

            //   List<Report_Insurance_By_Company_Summary> rpt = db.Database.SqlQuery<Report_Insurance_By_Company_Summary>(Sql).ToList();
            return View(SqlLink);
        }


        public void GenerateCommonViewBag()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            List<string> ASET = RP.MyAssetListString(UserId: U, UserType: T);

            ViewBag.ASET_ID = new SelectList(db.A_ASET_DATA.Where(x => ASET.Contains(x.ASET_ID)), "ASET_ID", "ASET_CODE_NAME");
            ViewBag.AREA_ID = new SelectList(RP.MyAreaList(UserId: U, UserType: T), "AREA_ID", "AREA_NAME");
            ViewBag.BUSINESS_ID = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
            ViewBag.ASET_BODY = new SelectList(db.A_BODY, "BODY_ID", "BODY_NAME");
            ViewBag.ASET_BRAND = new SelectList(db.A_BRAND, "BRAND_ID", "BRAND_NAME");
            ViewBag.ASET_FUEL = new SelectList(db.A_FUEL, "FUEL_ID", "FUEL_NAME");
            ViewBag.ASET_COLOR = new SelectList(db.A_COLOR, "COLOR_ID", "COLOR_NAME");
            List<SelectListItem> AA = new List<SelectListItem>();
            AA.Add(new SelectListItem { Text = "Used", Value = "1" });
            AA.Add(new SelectListItem { Text = "Free", Value = "0" });
            ViewBag.ASET_USED = AA;
            List<SelectListItem> BB = new List<SelectListItem>();
            BB.Add(new SelectListItem { Text = "Active", Value = "1" });
            BB.Add(new SelectListItem { Text = "Closed", Value = "0" });
            ViewBag.ASET_CLOSED = BB;
            List<SelectListItem> CC = new List<SelectListItem>();
            CC.Add(new SelectListItem { Text = "Running", Value = "1" });
            CC.Add(new SelectListItem { Text = "Expire", Value = "0" });
            ViewBag.REMIND = CC; //Iquma
            ViewBag.ASET_GROUP = new SelectList(RP.MyGroup(UserId: U, UserType: T), "GROUP_ID", "GROUP_NAME");
        }
        public void GenerateInsuranceViewBag()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            ViewBag.INSURANCE_TYPE_ID = new SelectList(RP.InsuranceTypeList(), "INSURANCE_TYPE_ID", "INSURANCE_TYPE_NAME");
            ViewBag.INSURANCE_COMPANY_ID = new SelectList(RP.InsuranceCompanyList(), "INSURANCE_COMPANY_ID", "INS_COMPANY_NAME");
            ViewBag.INS_COMPANY_ID = new SelectList(RP.CompanyList(), "INS_COMPANY_ID", "COMPANY_NAME");
            ViewBag.CLAIM_TYPE_LIST = new SelectList(RP.ClaimTypeList(), "CLAIM_TYPE_ID", "CLAIM_NAME");
            ViewBag.CLAIM_STATUS_LIST = new SelectList(RP.ClaimStatusList(), "CLAIM_STATUS_ID", "CLAIM_STATUS_NAME");
            ViewBag.KICK_BACK_STATUS = new SelectList(RP.KickBackStatusList(), "KICK_BACK_STATUS_ID", "KICK_BACK_STATUS_NAME");

            List<SelectListItem> AA = new List<SelectListItem>();
            AA.Add(new SelectListItem { Text = "claimed", Value = "1" });
            AA.Add(new SelectListItem { Text = "Not Claim", Value = "0" });
            ViewBag.CLAIM_STATUS = AA;
        }


    }
}