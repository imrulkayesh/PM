using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Oracle.ManagedDataAccess.Client;
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
    public class ExpensesController : Controller
    {
        private pmdbEntities db = new pmdbEntities();
        // GET: Expenses
        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {
                var getId = Session["UserId"].ToString();

                var MyGroupList = db.P_USERS_ASGR.Where(b => b.USER_TEXT == getId && b.UAP_ACTV == true).Select(s => s.ASGR_TEXT).ToList();
                var MyBeunList = db.P_USERS_BEUN.Where(b => b.USER_TEXT == getId && b.UBP_ACTV == true).Select(s => s.BEUN_TEXT).ToList();
                var MyDept = db.P_DEPT.Where(d => MyBeunList.Contains(d.DEPT_BEUN)).Select(s => s.DEPT_TEXT).ToList();
                var MyAssetList = db.VN_EXPD_XL.Where(a => MyGroupList.Contains(a.ASET_ASGR) && MyDept.Contains(a.ASET_DEPT)).Take(10).ToList();
                return View(MyAssetList);
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Time Expired, please login!');window.location.href = '/Home/Login';</script>");
            }
        }
        public ActionResult getSuggession(string term)
        {
            if (Session["UserId"] != null)
            {
                var getId = Session["UserId"].ToString();

                var MyGroupList = db.P_USERS_ASGR.Where(b => b.USER_TEXT == getId && b.UAP_ACTV == true).Select(s => s.ASGR_TEXT).ToList();
                var MyBeunList = db.P_USERS_BEUN.Where(b => b.USER_TEXT == getId && b.UBP_ACTV == true).Select(s => s.BEUN_TEXT).ToList();
                var MyDept = db.P_DEPT.Where(d => MyBeunList.Contains(d.DEPT_BEUN)).Select(s => s.DEPT_TEXT).ToList();
                var MyAssetList = db.P_ASSET.Where(a => MyGroupList.Contains(a.ASET_ASGR) && MyDept.Contains(a.ASET_DEPT)).Select(s => s.ASET_NAME).ToList();
                var filteredItems = MyAssetList.Where(
                  item => item.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
                  );
                return Json(filteredItems, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Time Expired, please login!');window.location.href = '/Home/Login';</script>");
            }
        }
        public ActionResult SingleExpensesView(string AssetName)
        {
            var AssetList= db.VN_EXPD_ALL.Where(a => a.ASET_NAME == AssetName).ToList();
            return View(AssetList);
        }
        //new expenses entry
        public ActionResult NewExpenses()
        {
            if (Session["UserId"] != null)
            {
                var getId = Session["UserId"].ToString();

                var Groups = db.P_USERS_ASGR.Where(b => b.USER_TEXT == getId && b.UAP_ACTV == true).Select(s => s.ASGR_TEXT).ToList();
                var Business = db.P_USERS_BEUN.Where(b => b.USER_TEXT == getId && b.UBP_ACTV == true).Select(s => s.BEUN_TEXT).ToList();
                ViewBag.EXPD_GU= new SelectList(db.P_ASGR.Where(g=> Groups.Contains(g.ASGR_TEXT)), "ASGR_TEXT", "ASGR_NAME");
                ViewBag.EXPD_BU = new SelectList(db.P_BEUN.Where(d=> Business.Contains(d.BEUN_TEXT)), "BEUN_TEXT", "BEUN_NAME");
                return View();
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Time Expired, please login!');window.location.href = '/Home/Login';</script>");
            }
        }
        //Get Asset for new expenses entry
        public ActionResult GetAssets(string Group,string Business)
        {
            if (!string.IsNullOrEmpty(Group) && !string.IsNullOrEmpty(Business))
            {
                var MyDept = db.P_DEPT.Where(d => d.DEPT_BEUN == Business).Select(s => s.DEPT_TEXT).ToList();
                var returnMyAsset = from a in db.P_ASSET
                                    where a.ASET_ASGR == Group && MyDept.Contains(a.ASET_DEPT) && a.ASET_ACTV==true && a.ASET_CLSE=="N"
                                    select new { a.ASET_OID, a.ASET_NAME };
                return Json(returnMyAsset, JsonRequestBehavior.AllowGet);
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        //Save new expenses entry
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewExpenses(P_EXPD p_EXPD)
        {
            if (ModelState.IsValid)
            {
                var getCurrentUser = db.P_OWNR.Where(o => o.OWNR_ASET == p_EXPD.EXPD_ASET && o.OWNR_ACTV==true).Select(s => s.OWNR_OID).FirstOrDefault();
                if(getCurrentUser == null)
                {
                    getCurrentUser = "0";
                }
                var GetSeq = db.P_SEQ.Where(a => a.SEQ_TEXT.Equals(30)).FirstOrDefault().SEQ_NUMBER;
                int OID = ((Convert.ToInt32(GetSeq) + 1));
                p_EXPD.EXPD_TEXT = OID.ToString();
                p_EXPD.EXPD_OWNR = getCurrentUser;
                p_EXPD.EXPD_EXPH = "10000";
                p_EXPD.EXPD_ASET = p_EXPD.EXPD_ASET;
                p_EXPD.EXPD_AMNT = p_EXPD.EXPD_AMNT;
                p_EXPD.EXPD_DATE = p_EXPD.EXPD_DATE;
                p_EXPD.EXPD_NOTE = p_EXPD.EXPD_NOTE;
                p_EXPD.EXPD_AUTH = true;
                p_EXPD.CUSR= Session["UserId"].ToString();
                p_EXPD.CDT = System.DateTime.Now;
                db.P_EXPD.Add(p_EXPD);
                db.SaveChanges();
                //Update Last Number
                P_SEQ p_SEQ = db.P_SEQ.Find(30);
                p_SEQ.SEQ_NUMBER = OID;
                db.Entry(p_SEQ).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Model Error! Pls rectify");
            return View(p_EXPD);
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
        public ActionResult ExcelSave(BikeExpenses[] htmlDataToModel)
        {
            string result = "Error! Data format is not Complete!";
            if (htmlDataToModel != null)
            {
                foreach (var item in htmlDataToModel)
                {
                    if (item.EXPD_DATE == null)
                    {
                        result = "Error";
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    P_EXPD_XL e = new P_EXPD_XL();
                    e.ASET_OID = item.ASET_OID;
                    e.EXPH_TEXT = item.EXPH_TEXT;
                    e.EXPD_AMNT = item.EXPD_AMNT;
                    e.EXPD_DATE = Convert.ToDateTime(item.EXPD_DATE);
                    e.EXPD_NOTE = item.EXPD_NOTE;
                    e.EXPD_USER = Session["UserId"].ToString();
                    db.P_EXPD_XL.Add(e);
                }
                db.SaveChanges();
                result = "Success! Data Upload Is Complete!";
            }
            //Execute Once before Save
            db.Database.ExecuteSqlCommand("BEGIN PRO_EXPD_XL_JUNK_CLEAR(); end;");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //Temp to Detail
        public ActionResult SendToDetail(string id)
        {
            //Asset Marked as Damages
            var param1 = new OracleParameter("ASSETOID", OracleDbType.Int32, id, ParameterDirection.Input);
            db.Database.ExecuteSqlCommand("BEGIN PRO_EXPD_XL(:ASSETOID); end;", param1);
            return RedirectToAction("Index");
        }

        public ActionResult AuthorizePost()
        {
            var Exp = db.VN_EXPD_XL_APP.Where(e=>e.EXPD_AUTH==false).ToList();
            return PartialView ("_AuthorizePost",Exp);
        }
        public ActionResult ApprovePOST(string id, string Exp)
        {
            P_EXPD p_EXPD = db.P_EXPD.Where(e => e.EXPD_ASET == id && e.EXPD_EXPH==Exp && e.EXPD_AUTH == false).SingleOrDefault();
            p_EXPD.EXPD_AUTH =true;
            db.Entry(p_EXPD).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult ExpensesHistory(string AsetId)
        {
            if (AsetId != null)
            {
                var History = db.P_EXPD.Where(a => a.EXPD_ASET == AsetId).ToList();
                return PartialView("_ExpensesHistory", History);
            }
            return View();
        }
    }

}