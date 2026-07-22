using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PM.Models;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Web;

namespace PM.Controllers
{
    public class AssetEntryController : Controller
    {
        private pmdbEntities db = new pmdbEntities();

        // GET: AssetEntry
        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {
                var getId = Session["UserId"].ToString();

                var MyGroupList = db.P_USERS_ASGR.Where(b => b.USER_TEXT == getId && b.UAP_ACTV == true).Select(s => s.ASGR_TEXT).ToList();
                var MyBeunList = db.P_USERS_BEUN.Where(b => b.USER_TEXT == getId && b.UBP_ACTV == true).Select(s => s.BEUN_TEXT).ToList();
                var MyDept = db.P_DEPT.Where(d => MyBeunList.Contains(d.DEPT_BEUN)).Select(s => s.DEPT_TEXT).ToList();
                var MyAssetList = db.P_ASSET.Where(a => MyGroupList.Contains(a.ASET_ASGR) && MyDept.Contains(a.ASET_DEPT)).Where(e => e.ASET_ACTV == true && e.ISBOOKED == false).Take(10).ToList();
                return View(MyAssetList);
                //return View(db.P_ASSET.Take(5).ToList());
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
        public ActionResult SingleAssetView(string AssetName)
        {
            if (Session["UserId"] != null)
            {
                var getId = Session["UserId"].ToString();

                var MyGroupList = db.P_USERS_ASGR.Where(b => b.USER_TEXT == getId && b.UAP_ACTV == true).Select(s => s.ASGR_TEXT).ToList();
                var MyBeunList = db.P_USERS_BEUN.Where(b => b.USER_TEXT == getId && b.UBP_ACTV == true).Select(s => s.BEUN_TEXT).ToList();
                var MyDept = db.P_DEPT.Where(d => MyBeunList.Contains(d.DEPT_BEUN)).Select(s => s.DEPT_TEXT).ToList();
                var MyAssetList = db.P_ASSET.Where(a => MyGroupList.Contains(a.ASET_ASGR) && MyDept.Contains(a.ASET_DEPT)).Where(a => a.ASET_NAME == AssetName && a.ASET_ACTV != null).ToList();
                return PartialView("_SingleAssetView", MyAssetList);
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Time Expired, please login!');window.location.href = '/Home/Login';</script>");
            }
        }

        // GET: AssetEntry/Create
        public ActionResult NewAsset()
        {
            if (Session["UserId"] != null)
            {
                var getId = Session["UserId"].ToString();

                var MyGroupList = db.P_USERS_ASGR.Where(b => b.USER_TEXT == getId && b.UAP_ACTV == true).Select(s => s.ASGR_TEXT).ToList();
                ViewBag.ASET_ASGR = new SelectList(db.P_ASGR.Where(n => MyGroupList.Contains(n.ASGR_TEXT)), "ASGR_TEXT", "ASGR_NAME");
                ViewBag.ASET_BRND = new SelectList(db.P_BRAND.Where(b => b.BRND_ACTV == true), "BRND_TEXT", "BRND_NAME");
                ViewBag.ASET_AREA = new SelectList(db.P_AREA.Where(b => b.AREA_ACTV == true && b.P_THNA.P_DIST.DIST_CUNT.Contains("BD")), "AREA_TEXT", "AREA_NAME");
                var MyBEUN = db.P_USERS_BEUN.Where(b => b.USER_TEXT == getId && b.UBP_ACTV == true).Select(s => s.BEUN_TEXT).ToList();
                ViewBag.AllBEUN = new SelectList(db.P_BEUN.Where(u => MyBEUN.Contains(u.BEUN_TEXT)), "BEUN_TEXT", "BEUN_NAME");
                return View();
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Time Expired, please login!');window.location.href = '/Home/Login';</script>");
            }
        }

        public ActionResult Autocomplete(string term)
        {
            var Asset = db.P_ASSET.Select(a => a.ASET_NAME).ToList();
            var filteredItems = Asset.Where(
                item => item.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
                );
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getDept(string BeuId)
        {
            var returnDept = from q in db.P_DEPT
                             where q.DEPT_BEUN == BeuId
                             select new { q.DEPT_TEXT, q.DEPT_NAME };
            return Json(returnDept, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getAccItem(string AccItem)
        {
            var returnAccItem = from q in db.P_ASAC
                                where q.ASAC_ASGR == AccItem
                                select new { q.ASAC_TEXT, q.ASAC_NAME };
            return Json(returnAccItem, JsonRequestBehavior.AllowGet);
        }
        // POST: AssetEntry/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewAsset([Bind(Include = "ASET_OID,ASET_TEXT,ASET_NAME,ASET_MODL,ASET_TYPE,ENGN_PONO,CHSS_GRN,ASET_CPCT,ASET_PRIC,REG_DATE,ASET_IMAG,FUEL_TYPE,BODY_TYPE,ASET_HIGT,ASET_WIGT,ASET_COLR,MFG_YEAR,ASET_NOTE,ISBOOKED,ASET_ASGR,ASET_DEPT,ASET_AREA,ASET_BRND,ASET_ACTV,CUSR,EUSR,CDT,EDT,UDT,VER")] P_ASSET p_ASSET, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0 && file.ContentLength < 2100000) //Max length 2100000 bytes = 2 MB
                {
                    var Upload_Path = db.P_SEQ.Where(a => a.SEQ_TEXT == 500).FirstOrDefault().SEQ_NAME;//find from App Setup table
                    var File_Ext = System.IO.Path.GetFileNameWithoutExtension(file.ContentType);
                    if (File_Ext == "jpg" || File_Ext == "png" || File_Ext == "jpeg")
                    {
                        var newFileName = Guid.NewGuid().ToString() + "." + File_Ext;
                        //store the file inside ~/App_Data/uploads folder
                        var path = System.IO.Path.Combine(Server.MapPath(Upload_Path), newFileName);
                        file.SaveAs(path);
                        //Picture detail Save to database
                        p_ASSET.ASET_IMAG = Upload_Path + "/" + newFileName;
                    }
                }
                var GetSeq = db.P_SEQ.Where(a => a.SEQ_TEXT.Equals(20)).FirstOrDefault().SEQ_NUMBER;
                int OID = ((Convert.ToInt32(GetSeq) + 1));
                p_ASSET.ASET_OID = OID.ToString();
                p_ASSET.ISBOOKED = false;
                p_ASSET.ASET_ACTV = true;
                p_ASSET.ASET_CLSE ="N";
                p_ASSET.CDT = System.DateTime.Now;
                db.P_ASSET.Add(p_ASSET);
                db.SaveChanges();
                //Update Last Number
                P_SEQ p_SEQ = db.P_SEQ.Find(20);
                p_SEQ.SEQ_NUMBER = OID;
                db.Entry(p_SEQ).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Model Error! Pls rectify");
            return View(p_ASSET);
        }

        public ActionResult UploadExcel()
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
        public ActionResult UploadExcel(ExcelUpload model)
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
                        if (i == 8) //Date data type excel column no I1
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
        public ActionResult ExcelSave(PRANAssetExcel[] htmlDataToModel)
        {
            string result = "Error! Data format is not Complete!";
            var GetSeq = db.P_SEQ.Where(a => a.SEQ_TEXT.Equals(20)).FirstOrDefault().SEQ_NUMBER;
            int OID = (Convert.ToInt32(GetSeq) + 1);
            if (htmlDataToModel != null)
            {
                foreach (var item in htmlDataToModel)
                {
                    if (item.REG_DATE == null)
                    {
                        result = "Error";
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    P_ASSET A = new P_ASSET();
                    OID = OID + 1;
                    A.ASET_OID = OID.ToString();
                    A.ASET_TEXT = item.ASET_TEXT;
                    A.ASET_NAME = item.ASET_NAME;
                    A.ASET_MODL = item.ASET_MODL;
                    A.ASET_TYPE = item.ASET_TYPE;
                    A.ENGN_PONO = item.ENGN_PONO;
                    A.CHSS_GRN = item.CHSS_GRN;
                    A.ASET_CPCT = item.ASET_CPCT;
                    A.ASET_PRIC = item.ASET_PRIC;
                    A.REG_DATE = Convert.ToDateTime(item.REG_DATE);
                    A.MFG_YEAR = item.MFG_YEAR;
                    A.ASET_NOTE = item.ASET_NOTE;
                    A.ASET_ASGR = item.ASET_ASGR;
                    A.ASET_DEPT = item.ASET_DEPT;
                    A.ASET_AREA = item.ASET_AREA;
                    A.ASET_BRND = item.ASET_BRND;
                    A.ISBOOKED = false;
                    A.ASET_ACTV = true;
                    A.CDT = System.DateTime.Now;
                    db.P_ASSET.Add(A);
                    db.SaveChanges();
                }
                //Update Last Number
                P_SEQ p_SEQ = db.P_SEQ.Find(20);
                p_SEQ.SEQ_NUMBER = OID;
                db.Entry(p_SEQ).State = EntityState.Modified;
                db.SaveChanges();
                result = "Success! Data Upload Is Complete!";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AssetUpdate(string id)
        {
            if (Session["UserId"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                P_ASSET p_ASSET = db.P_ASSET.Find(id);
                if (p_ASSET == null)
                {
                    return HttpNotFound();
                }

                var getId = Session["UserId"].ToString();

                var MyGroupList = db.P_USERS_ASGR.Where(b => b.USER_TEXT == getId && b.UAP_ACTV == true).Select(s => s.ASGR_TEXT).ToList();
                ViewBag.ASET_ASGR = new SelectList(db.P_ASGR.Where(n => MyGroupList.Contains(n.ASGR_TEXT)), "ASGR_TEXT", "ASGR_NAME");
                ViewBag.ASET_BRND = new SelectList(db.P_BRAND.Where(b => b.BRND_ACTV == true), "BRND_TEXT", "BRND_NAME");
                ViewBag.ASET_AREA = new SelectList(db.P_AREA.Where(b => b.AREA_ACTV == true && b.P_THNA.P_DIST.DIST_CUNT.Contains("BD")), "AREA_TEXT", "AREA_NAME");
                ViewBag.ASET_DEPT = new SelectList(db.P_DEPT.Where(b => b.DEPT_ACTV == true && b.DEPT_TEXT == p_ASSET.ASET_DEPT), "DEPT_TEXT", "DEPT_NAME");//Spe
                var MyBEUN = db.P_USERS_BEUN.Where(b => b.USER_TEXT == getId && b.UBP_ACTV == true).Select(s => s.BEUN_TEXT).ToList();
                ViewBag.AllBEUN = new SelectList(db.P_BEUN.Where(u => MyBEUN.Contains(u.BEUN_TEXT)), "BEUN_TEXT", "BEUN_NAME");

                return View(p_ASSET);
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Time Expired, please login!');window.location.href = '/Home/Login';</script>");
            }
        }
        [HttpPost]
        public ActionResult AssetUpdate(P_ASSET p_ASSET, string AllBEUN)
        {
            if (ModelState.IsValid)
            {
                P_ASSET _ASSET = db.P_ASSET.Find(p_ASSET.ASET_OID);
                _ASSET.ASET_MODL = p_ASSET.ASET_MODL;
                _ASSET.ASET_TYPE = p_ASSET.ASET_TYPE;
                _ASSET.ENGN_PONO = p_ASSET.ENGN_PONO;
                _ASSET.CHSS_GRN = p_ASSET.CHSS_GRN;
                _ASSET.ASET_CPCT = p_ASSET.ASET_CPCT;
                _ASSET.ASET_PRIC = p_ASSET.ASET_PRIC;
                _ASSET.BODY_TYPE = p_ASSET.BODY_TYPE;
                _ASSET.ASET_COLR = p_ASSET.ASET_COLR;
                _ASSET.MFG_YEAR = p_ASSET.MFG_YEAR;
                _ASSET.ASET_NOTE = p_ASSET.ASET_NOTE;
                _ASSET.ASET_ASGR = p_ASSET.ASET_ASGR;
                _ASSET.ASET_DEPT = p_ASSET.ASET_DEPT;
                _ASSET.ASET_AREA = p_ASSET.ASET_AREA;
                _ASSET.ASET_BRND = p_ASSET.ASET_BRND;
                db.Entry(_ASSET).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.ASET_AREA = new SelectList(db.P_AREA, "AREA_TEXT", "AREA_NAME", p_ASSET.ASET_AREA);
            //ViewBag.ASET_ASGR = new SelectList(db.P_ASGR, "ASGR_TEXT", "ASGR_NAME", p_ASSET.ASET_ASGR);
            //ViewBag.ASET_BRND = new SelectList(db.P_BRAND, "BRND_TEXT", "BRND_NAME", p_ASSET.ASET_BRND);
            //ViewBag.ASET_DEPT = new SelectList(db.P_DEPT, "DEPT_TEXT", "DEPT_NAME", p_ASSET.ASET_DEPT);
            return RedirectToAction("Index");
        }

        public ActionResult DamageApplications()
        {
            var DamageApplications = db.P_ASSET_DAMAGE.ToList();
            return View(DamageApplications);
        }
        public ActionResult ApplyDamage(string id)
        {
            if (Session["UserId"] != null)
            {
                ViewBag.ASET_NAME = new SelectList(db.P_ASSET.Where(b => b.ASET_ACTV == true && b.ASET_OID == id), "ASET_OID", "ASET_NAME");
                return View();
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Time Expired, please login!');window.location.href = '/Home/Login';</script>");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApplyDamage(P_ASSET_DAMAGE p_ASSET_DAMAGE,string ASET_NAME)
        {
            if (Session["UserId"] != null)
            {
                var getId = Session["UserId"].ToString();

                if (ModelState.IsValid)
                {
                    if (ASET_NAME == null)
                    {
                        return View();
                    }
                    p_ASSET_DAMAGE.ASET_OID = ASET_NAME;
                    p_ASSET_DAMAGE.DMG_APP_USER = getId;
                    db.P_ASSET_DAMAGE.Add(p_ASSET_DAMAGE);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View();
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Time Expired, please login!');window.location.href = '/Home/Login';</script>");
            }
        }
        
        public ActionResult DamageApprove(string id)
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DamageApprove(P_ASSET_DAMAGE p_ASSET_DAMAGE, string ASET_NAME)
        {
            return View();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
