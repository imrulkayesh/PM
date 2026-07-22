using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using PM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PM.Controllers
{
    public class InsuranceController : Controller
    {
        private pmdbEntities db = new pmdbEntities();
        // GET: Insurance
        public ActionResult Index()
        {
            return View();
        }
        //new insurance entry
        public ActionResult NewInsurance()
        {
            if (Session["UserId"] != null)
            {
                var getId = Session["UserId"].ToString();

                var Groups = db.P_USERS_ASGR.Where(b => b.USER_TEXT == getId && b.UAP_ACTV == true).Select(s => s.ASGR_TEXT).ToList();
                var Business = db.P_USERS_BEUN.Where(b => b.USER_TEXT == getId && b.UBP_ACTV == true).Select(s => s.BEUN_TEXT).ToList();
                ViewBag.INSC_GU = new SelectList(db.P_ASGR.Where(g => Groups.Contains(g.ASGR_TEXT)), "ASGR_TEXT", "ASGR_NAME");
                ViewBag.INSC_BU = new SelectList(db.P_BEUN.Where(d => Business.Contains(d.BEUN_TEXT)), "BEUN_TEXT", "BEUN_NAME");
                return View();
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Time Expired, please login!');window.location.href = '/Home/Login';</script>");
            }
        }
        //Get Asset for new Insurance entry
        public ActionResult GetAssets(string Group, string Business)
        {
            if (!string.IsNullOrEmpty(Group) && !string.IsNullOrEmpty(Business))
            {
                var MyDept = db.P_DEPT.Where(d => d.DEPT_BEUN == Business).Select(s => s.DEPT_TEXT).ToList();
                var returnMyAsset = from a in db.P_ASSET
                                    where a.ASET_ASGR == Group && MyDept.Contains(a.ASET_DEPT) && a.ASET_ACTV == true && a.ASET_CLSE == "N"
                                    select new { a.ASET_OID, a.ASET_NAME };
                return Json(returnMyAsset, JsonRequestBehavior.AllowGet);
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        //Save new Insurance entry
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewInsurance(P_INSC p_INSC)
        {
            if (ModelState.IsValid)
            {
                var getCurrentUser = db.P_OWNR.Where(o => o.OWNR_ASET == p_INSC.INSC_ASET && o.OWNR_ACTV == true).Select(s => s.OWNR_OID).FirstOrDefault();
                if (getCurrentUser == null)
                {
                    getCurrentUser = "0";
                }
                var GetSeq = db.P_SEQ.Where(a => a.SEQ_TEXT.Equals(40)).FirstOrDefault().SEQ_NUMBER;
                int OID = ((Convert.ToInt32(GetSeq) + 1));
                p_INSC.INSC_TEXT = OID.ToString();
                p_INSC.INSC_USER = getCurrentUser;
                p_INSC.INSC_EXPH = "10001";
                p_INSC.INSC_ASET = p_INSC.INSC_ASET;
                p_INSC.INSC_PLCY = p_INSC.INSC_PLCY;
                p_INSC.INSC_RDAT = p_INSC.INSC_RDAT;
                p_INSC.INSC_PREM = p_INSC.INSC_PREM;
                decimal premium = p_INSC.INSC_PREM;
                decimal amount = Math.Round( premium + (premium * Convert.ToDecimal(0.15) ))- Math.Round(premium * Convert.ToDecimal(0.8));
                p_INSC.INSC_AMNT = amount;
                p_INSC.INSC_NOTE = p_INSC.INSC_NOTE;
                p_INSC.INSC_ACTV = true;
                p_INSC.INSC_EDAT = System.DateTime.Now;
                db.P_INSC.Add(p_INSC);
                db.SaveChanges();
                //Update Last Number
                P_SEQ p_SEQ = db.P_SEQ.Find(40);
                p_SEQ.SEQ_NUMBER = OID;
                db.Entry(p_SEQ).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Model Error! Pls rectify");
            return View(p_INSC);
        }

        public ActionResult XL()
        {
            if (Session["UserId"] != null)
            {
                var model = new ExcelUpload();
                return View(model);
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Time Expired, please login!');window.location.href = '/Home/Login';</script>");
            }
        }
        [HttpPost]
        public ActionResult XL(ExcelUpload model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Errrrrr");
                return View(model);
            }
            DataTable dt = GetDataTableFromSpreadsheet(model.MyExcelFile.InputStream, false);
            string strContent = "<p>Excel data loaded for upload, please check and verify!</p>" + ConvertDataTableToHTMLTable(dt);
            model.MSExcelTable = strContent;
            return View(model);
        }
        public static DataTable GetDataTableFromSpreadsheet(Stream MyExcelStream, bool ReadOnly)
        {
            DataTable dt = new DataTable();
            using (SpreadsheetDocument sDoc = SpreadsheetDocument.Open(MyExcelStream, ReadOnly))
            {
                WorkbookPart workbookPart = sDoc.WorkbookPart;
                IEnumerable<Sheet> sheets = sDoc.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                string relationshipId = sheets.First().Id.Value;
                WorksheetPart worksheetPart = (WorksheetPart)sDoc.WorkbookPart.GetPartById(relationshipId);
                Worksheet workSheet = worksheetPart.Worksheet;
                SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                IEnumerable<Row> rows = sheetData.Descendants<Row>();

                foreach (Cell cell in rows.ElementAt(0))
                {
                    dt.Columns.Add(GetCellValue(sDoc, cell));
                }

                foreach (Row row in rows) //this will also include your header row...
                {
                    DataRow tempRow = dt.NewRow();

                    for (int i = 0; i < row.Descendants<Cell>().Count(); i++)
                    {
                        tempRow[i] = GetCellValue(sDoc, row.Descendants<Cell>().ElementAt(i));
                        if (i == 3) //Date data type excel column no D1
                        {
                            double x = Convert.ToDouble(GetCellValue(sDoc, row.Descendants<Cell>().ElementAt(i)));
                            tempRow[i] = DateTime.FromOADate(x);
                        }
                    }

                    dt.Rows.Add(tempRow);
                }
            }
            dt.Rows.RemoveAt(0);
            return dt;
        }
        public static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            string value = cell.CellValue.InnerXml;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
            }
            else
            {
                return value;
            }
        }
        public static string ConvertDataTableToHTMLTable(DataTable dt)
        {
            string ret = "";
            ret = "<table class=" + (char)34 + "table-bordered table-striped" + (char)34 + ", id=" + (char)34 + "table1" + (char)34 + ">";

            ret += "<thead> <tr><th>#</th>";
            foreach (DataColumn col in dt.Columns)
            {
                ret += "<th class=" + (char)34 + "tdColumnHeader" + (char)34 + ">" + col.ColumnName + "</th>";
            }
            ret += "</tr> </thead> <tbody>";
            foreach (DataRow row in dt.Rows)
            {
                ret += "<tr><td><a data-itemId='0' href='#' class='deleteItem'>Remove</a></td>";
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ret += "<td class=" + (char)34 + "tdCellData" + (char)34 + ">" + row[i].ToString() + "</td>";
                }
                ret += "</tr>";
            }
            ret += "</tbody></table>";
            return ret;
        }
        //PRAN Assset Excel HTTP POST
        public ActionResult ExcelSave(InsuranceExpenses[] htmlDataToModel)
        {
            //Execute Once before Save
            db.Database.ExecuteSqlCommand("BEGIN PRO_INSC_XL_JUNK_CLEAR(); end;");
            string result = "Error! Data format is not Complete!";
            if (htmlDataToModel != null)
            {
                foreach (var item in htmlDataToModel)
                {
                    if (item.INSC_RDAT == null)
                    {
                        result = "Error";
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    P_INSC_XL e = new P_INSC_XL();
                    e.INSC_ASET = item.INSC_ASET;
                    e.INSC_PLCY = item.INSC_PLCY;
                    e.INSC_PREM = item.INSC_PREM;
                    e.INSC_RDAT = Convert.ToDateTime(item.INSC_RDAT);
                    e.INSC_NOTE = item.INSC_NOTE;
                    e.INSC_USER = Session["UserId"].ToString();
                    db.P_INSC_XL.Add(e);
                }
                db.SaveChanges();
                result = "Success! Data Upload Is Complete!";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewInsurance()
        {
            if (Session["UserId"] != null)
            {
                var getId = Session["UserId"].ToString();

                var MyGroupList = db.P_USERS_ASGR.Where(b => b.USER_TEXT == getId && b.UAP_ACTV == true).Select(s => s.ASGR_TEXT).ToList();
                var MyBeunList = db.P_USERS_BEUN.Where(b => b.USER_TEXT == getId && b.UBP_ACTV == true).Select(s => s.BEUN_TEXT).ToList();
                var MyDept = db.P_DEPT.Where(d => MyBeunList.Contains(d.DEPT_BEUN)).Select(s => s.DEPT_TEXT).ToList();
                var MyAssetList = db.P_ASSET.Where(a => MyGroupList.Contains(a.ASET_ASGR) && MyDept.Contains(a.ASET_DEPT))
                    .Select(a=>a.ASET_OID).ToList();
                var MyInsurance = db.P_INSC.Where(i => MyAssetList.Contains(i.INSC_ASET)).ToList();
                return View(MyInsurance);
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Time Expired, please login!');window.location.href = '/Home/Login';</script>");
            }
        }
        public ActionResult InsuranceHistory(string AsetId)
        {
            if (AsetId != null)
            {
                var History = db.P_INSC.Where(a => a.INSC_ASET == AsetId).ToList();
                return PartialView("_InsuranceHistory", History);
            }
            return View();
        }
    }
}