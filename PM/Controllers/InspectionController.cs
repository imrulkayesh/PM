using Newtonsoft.Json;
using PM.BLL;
using PM.Model;
using PM.ModelsV;
using PM.MyAuth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace PM.Controllers
{
    [Authorize]
    [CustomHttpReffer]
    public class InspectionController : Controller
    {
        private PMEntities db = new PMEntities();
        private GE ge = new GE();
        private ReturnPermission RP = new ReturnPermission();
        private InstantEmailSender IES = new InstantEmailSender();
        private JsonReaderPMController PMC = new JsonReaderPMController();
        // GET: Inspection

        public List<InspectionView> InspectionData(string Status, string Approval)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            #region Old
            //List<string> ASET = RP.MyAssetListString(UserId: U, UserType: T);
            //var InspectionData = (from i in db.A_INSPH
            //                      join a in db.A_ASET_DATA on i.INSPH_ASET equals a.ASET_ID
            //                      join d in db.A_DEPARTMENT on i.INSPH_DEPARTMENT equals d.DEPARTMENT_ID
            //                      join o in db.A_OWNR on i.INSPH_OWNR equals o.OWNR_ID
            //                      join r in db.A_AREA on i.INSPH_AREA equals r.AREA_ID
            //                      where i.ACT == Status
            //                      && ASET.Contains(a.ASET_ID)
            //                      && i.INSPH_APP == Approval
            //                      && i.A_INSP_TYPE.ITYPE_ID != "B"
            //                      select new InspectionView
            //                      {
            //                          INSPH_ID = i.INSPH_ID,
            //                          INSPH_ASET_ID = a.ASET_ID,
            //                          INSPH_ASET = a.ASET_CODE_NAME,
            //                          INSPH_OWNR = o.OWNR_USER,
            //                          INSPH_DEPARTMENT = d.DEPARTMENT_NAME,
            //                          INSPH_AREA = r.AREA_NAME,
            //                          INSPH_IUSER = i.INSPH_IUSER,
            //                          INSPH_DATE = i.INSPH_DATE ?? DateTime.Now,
            //                          MILEAGE = i.MILEAGE,
            //                          INSPH_TYPE = i.A_INSP_TYPE.ITYPE_NAME
            //                      }
            //    ).ToList(); 
            #endregion
            string sql_1 = @"SELECT 
                            I.INSPH_ID,ASS.ASET_ID INSPH_ASET_ID,ASS.ASET_CODE_NAME INSPH_ASET,O.OWNR_USER INSPH_OWNR,
                            DPT.DEPARTMENT_NAME INSPH_DEPARTMENT,R.AREA_NAME INSPH_AREA,I.INSPH_IUSER,NVL(I.INSPH_DATE,SYSDATE) INSPH_DATE,
                            I.MILEAGE,TY.ITYPE_NAME INSPH_TYPE
                            FROM PM.A_INSPH I 
                            INNER JOIN PM.A_ASET_DATA ASS ON I.INSPH_ASET = ASS.ASET_ID
                            INNER JOIN PM.A_DEPARTMENT DPT ON I.INSPH_DEPARTMENT = DPT.DEPARTMENT_ID
                            INNER JOIN PM.A_OWNR O ON I.INSPH_OWNR = O.OWNR_ID
                            INNER JOIN PM.A_AREA R ON I.INSPH_AREA = R.AREA_ID
                            INNER JOIN PM.A_INSP_TYPE TY ON I.INSPH_TYPE = TY.ITYPE_ID
                            INNER JOIN pm.A_ASET_DATA T6 ON T6.ASET_ID=I.INSPH_ASET AND T6.ACT=1
                            INNER JOIN PM.A_USERS_GROUP T5 ON T6.ASET_GROUP=T5.UG_GROUP AND T5.UG_USER='" + U + @"' AND T5.ACT=1
                            INNER JOIN PM.A_DEPARTMENT T7 ON T6.ASET_DEPARTMENT=T7.DEPARTMENT_ID AND T7.ACT=1
                            INNER JOIN PM.A_BUSINESS T8 ON T7.DEPARTMENT_BUSINESS=T8.BUSINESS_ID AND T8.ACT=1
                            INNER JOIN PM.A_USERS_BUS T9 ON T8.BUSINESS_ID =T9.UB_BUSINESS AND T9.UB_USER=T5.UG_USER AND T9.ACT=1 
                            INNER JOIN (SELECT DISTINCT(AREA_ID) FROM (SELECT T.AREA_ID
                            FROM PM.A_AREA T
                            INNER JOIN PM.A_THANA T1 ON T.AREA_THANA=T1.THANA_ID AND T1.ACT=1
                            INNER JOIN PM.A_DISTRICT T2 ON T1.THANA_DISTRICT=T2.DISTRICT_ID AND T2.ACT=1
                            INNER JOIN PM.A_USERS_COUNTRY T3 ON T2.DISTRICT_COUNTRY=T3.COUNTRY_ID AND T3.ACT=1 AND T3.USER_TEXT='" + U + @"'
                            WHERE T.ACT=1
                            UNION ALL
                            SELECT T.UA_AREA
                            FROM PM.A_USERS_AREA T WHERE T.ACT=1 AND T.UA_USER='" + U + @"'
                            )) T10 ON T6.ASET_AREA=T10.AREA_ID
                            WHERE I.ACT = '" + Status + @"' 
                            AND I.INSPH_APP = '" + Approval + @"'
                            AND TY.ITYPE_ID <> 'B'";
            var InspectionData = db.Database.SqlQuery<InspectionView>(sql: sql_1).ToList();
            return InspectionData;
        }
        [KAuth(401)]
        public ActionResult Inspections()
        {
            var ID = InspectionData("0", "N");
            return View(ID);
        }
        [KAuth(403)]
        public ActionResult InsApproval()
        {
            var ID = InspectionData("1", "N");
            return View(ID);
        }

        public ActionResult ViewInspection(string id, string Post)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Detail_Data = from d in db.A_INSPD
                              where d.INSPH_ID == id
                              group new { d } by d.INSPH_ID into g
                              select new
                              {
                                  INSPH = g.Key,
                                  IAmount = g.Sum(x => x.d.INSPD_IRATE * x.d.INSPD_IQTY)
                              };

            ViewBag.INSPH = Detail_Data.FirstOrDefault().INSPH;
            ViewBag.IAmount = Detail_Data.FirstOrDefault().IAmount;

            var VI = (from d in db.A_INSPD
                      join i in db.A_ITEM_LIST on d.INSPD_ITEM equals i.ITEM_ID
                      where d.INSPH_ID == id
                      select new InspectionD
                      {
                          INSPH_ID = d.INSPH_ID,
                          INSPD_LINE = d.INSPD_LINE,
                          INSPD_ITEM = d.INSPD_ITEM,
                          INSPD_ITEM_NAME = i.ITEM_NAME,
                          INSPD_IQTY = d.INSPD_IQTY ?? 0,
                          INSPD_IRATE = d.INSPD_IRATE ?? 0,
                          INSPD_NOTE = d.INSPD_NOTE
                      }).ToList();
            return PartialView(VI);
        }
        [KAuth(403)]
        public ActionResult InspectionAppDetail(string id, string AsetId, string aset)
        {
            if (id == null || AsetId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Detail_Data = from d in db.A_INSPD
                              where d.INSPH_ID == id
                              group new { d } by d.INSPH_ID into g
                              select new
                              {
                                  INSPH = g.Key,
                                  IAmount = g.Sum(x => x.d.INSPD_IRATE * x.d.INSPD_IQTY)
                              };

            ViewBag.INSPH = Detail_Data.FirstOrDefault().INSPH;
            ViewBag.IAmount = Detail_Data.FirstOrDefault().IAmount;
            ViewBag.ASET_ID = aset;

            List<InspectionDx> Old_data_Dup = (from h in db.A_INSPH
                                               join d in db.A_INSPD on h.INSPH_ID equals d.INSPH_ID
                                               where h.ACT == true
                                               && h.INSPH_ASET == AsetId //take date by aset
                                               && h.INSPH_ID != id //ignore this self
                                               select new InspectionDx
                                               {
                                                   INSPH_ID = h.INSPH_ID,
                                                   INSPH_ASET = h.INSPH_ASET,
                                                   INSPD_ITEM = d.INSPD_ITEM,
                                                   INSPD_IQTY = d.INSPD_IQTY,
                                                   INSPH_DATE = h.INSPH_DATE,
                                                   MILEAGE = h.MILEAGE
                                               }).OrderByDescending(s => s.INSPH_DATE).Distinct().ToList();

            List<InspectionDx> Old_data = new List<InspectionDx>();
            foreach (var item in Old_data_Dup)
            {
                bool isExist = Old_data.Where(e => e.INSPD_ITEM == item.INSPD_ITEM).Any();
                if (!isExist)
                {
                    Old_data.Add(item);
                }
            }

            List<InspectionD> VI = (from d in db.A_INSPD
                                    join i in db.A_ITEM_LIST on d.INSPD_ITEM equals i.ITEM_ID
                                    where d.INSPH_ID == id
                                    select new InspectionD
                                    {
                                        INSPH_ID = d.INSPH_ID,
                                        INSPD_LINE = d.INSPD_LINE,
                                        INSPD_ITEM = d.INSPD_ITEM,
                                        INSPD_ITEM_NAME = i.ITEM_NAME,
                                        INSPD_IQTY = d.INSPD_IQTY ?? 0,
                                        INSPD_IRATE = d.INSPD_IRATE ?? 0,
                                        INSPD_NOTE = d.INSPD_NOTE
                                    }).ToList();


            List<InspectionD_With_Old> New_Data = new List<InspectionD_With_Old>();
            //Code pattern is Ok
            //New_Data =(from category in VI
            //               join prod in Old_data on category.INSPD_ITEM equals prod.INSPD_ITEM into prodGroup
            //               from item in prodGroup.DefaultIfEmpty(new InspectionDx { INSPH_DATE = null, MILEAGE = string.Empty })
            //               select new InspectionD_With_Old
            //               {
            //                   INSPH_ID = category.INSPH_ID,
            //                   INSPD_LINE = category.INSPD_LINE,
            //                   INSPD_ITEM = category.INSPD_ITEM,
            //                   INSPD_ITEM_NAME = category.INSPD_ITEM_NAME,
            //                   INSPD_IQTY = category.INSPD_IQTY,
            //                   INSPD_IRATE = category.INSPD_IRATE,
            //                   INSPD_NOTE = category.INSPD_NOTE,
            //                   INSPH_DATE = item.INSPH_DATE,
            //                   MILEAGE = item.MILEAGE
            //               }).ToList();
            New_Data = (from n in VI
                        from o in Old_data.Where(x => x.INSPD_ITEM == n.INSPD_ITEM).DefaultIfEmpty()
                        select new InspectionD_With_Old
                        {
                            INSPH_ID = n.INSPH_ID,
                            INSPD_LINE = n.INSPD_LINE,
                            INSPD_ITEM = n.INSPD_ITEM,
                            INSPD_ITEM_NAME = n.INSPD_ITEM_NAME,
                            INSPD_IQTY = n.INSPD_IQTY,
                            INSPD_IRATE = n.INSPD_IRATE,
                            INSPD_NOTE = n.INSPD_NOTE,
                            INSPH_DATE = o?.INSPH_DATE ?? null,
                            MILEAGE = o?.MILEAGE ?? string.Empty
                        }).ToList();
            return View(New_Data);
        }

        //Need change db sourcse
        [KAuth(401)]
        public ActionResult NewInspection()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            List<string> ASET = RP.MyAssetListString(UserId: U, UserType: T);
            ViewBag.INSPH_ASET = new SelectList(RP.AsetByListId(AssetId: ASET), "ASET_ID", "ASET_NAME_TAG");
            ViewBag.INSPH_TYPE = new SelectList(db.A_INSP_TYPE.Where(r => r.ACT == true), "ITYPE_ID", "ITYPE_NAME");
            return View();
        }
        [KAuth(401)]
        [HttpPost]
        public ActionResult NewInspection(newInspection NI)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            DateTime IDATE = Convert.ToDateTime(NI.INSPH_DATE);
            var IsExistToday = db.A_INSPH.Where(a => a.INSPH_ASET == NI.INSPH_ASET && a.INSPH_DATE == NI.INSPH_DATE).ToList();
            if (IsExistToday.Count() > 0)
            {
                return Json(new { inspid = "Inspection already created today: " + IsExistToday.First().INSPH_ID }, JsonRequestBehavior.AllowGet);
            }

            var IsPendingForJobCard = db.A_INSPH.Where(a => a.INSPH_ASET == NI.INSPH_ASET && a.INSPH_JOB == "N").ToList();
            if (IsPendingForJobCard.Count() > 2)
            {
                return Json(new { jobid = IsPendingForJobCard.Count() + " Inspection(s) pending for Jobcard, please clear first" }, JsonRequestBehavior.AllowGet);
            }

            if (ModelState.IsValid)
            {
                string GetSeq = NI.INSPH_ASET;//ge.GetSeq(15).ToString();

                string id = ge.FifteenDigitId("INSP", GetSeq, dt: IDATE);
                A_INSPH H = new A_INSPH();
                H.INSPH_ID = id;
                H.INSPH_ASET = NI.INSPH_ASET;
                H.INSPH_OWNR = NI.A_USERF;
                H.INSPH_DEPARTMENT = NI.A_DEPTF;
                H.INSPH_AREA = NI.A_AREAF;
                H.INSPH_IUSER = NI.INSPH_IUSER;
                H.INSPH_DATE = IDATE;
                H.MILEAGE = NI.MILEAGE;
                H.INSPH_JOB = "N";
                H.INSPH_TYPE = NI.INSPH_TYPE;
                H.INSPH_NOTE = NI.INSPH_NOTE;
                H.INSPH_APP = "N";
                H.CDT = DateTime.Now;
                H.ACT = false;
                H.CDU = U;
                H.VAR = 1;
                int CounterId = 1;
                foreach (var item in NI.InspectionD)
                {
                    if (item.INSPD_IQTY == 0 || item.INSPD_IRATE == 0 || item.INSPD_IQTY == null || item.INSPD_IRATE == null)
                    {
                        List<string> ASETx = RP.MyAssetListString(UserId: U, UserType: T);
                        ViewBag.INSPH_ASET = new SelectList(RP.AsetByListId(AssetId: ASETx), "ASET_ID", "ASET_NAME_TAG");
                        ViewBag.INSPH_TYPE = new SelectList(db.A_INSP_TYPE.Where(r => r.ACT == true), "ITYPE_ID", "ITYPE_NAME");
                        return Json(new { success = false, inspid = "Inspection not saved " }, JsonRequestBehavior.AllowGet);
                    }
                    A_INSPD D = new A_INSPD();
                    D.INSPH_ID = id;
                    D.INSPD_LINE = Convert.ToByte(CounterId);
                    D.INSPD_ITEM = item.INSPD_ITEM;
                    D.INSPD_IQTY = item.INSPD_IQTY;
                    D.INSPD_IRATE = item?.INSPD_IRATE ?? 0;
                    D.INSPD_NOTE = item.INSPD_NOTE;
                    D.INSPD_STATE = "N";
                    CounterId = CounterId + 1;
                    db.A_INSPD.Add(D);
                }
                db.A_INSPH.Add(H);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                                       ve.PropertyName,
                                       eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                                       ve.ErrorMessage);
                        }
                    }
                    throw;
                }
                return Json(new { success = true, inspid = "Inspection: " + id }, JsonRequestBehavior.AllowGet);
            }
            List<string> ASET = RP.MyAssetListString(UserId: U, UserType: T);
            ViewBag.INSPH_ASET = new SelectList(RP.AsetByListId(AssetId: ASET), "ASET_ID", "ASET_NAME_TAG");
            //List<SelectListItem> JT = new List<SelectListItem>();
            //JT.Add(new SelectListItem { Text = "General", Value = "G", Selected = true });
            //JT.Add(new SelectListItem { Text = "Accident", Value = "A" });
            //JT.Add(new SelectListItem { Text = "Scheduled", Value = "S" });
            //ViewBag.INSPH_TYPE = JT;
            ViewBag.INSPH_TYPE = new SelectList(db.A_INSP_TYPE.Where(r => r.ACT == true), "ITYPE_ID", "ITYPE_NAME");
            return Json(new { success = false, inspid = "Inspection not saved " }, JsonRequestBehavior.AllowGet);
        }
        [KAuth(401)]
        public ActionResult EditInspection(string id)
        {
            var Editable = db.A_INSPH.Where(i => i.INSPH_ID == id && i.INSPH_JOB == "N" && i.ACT == false).ToList();
            if (Editable.Count == 0)
            {
                TempData["msg"] = string.Format("swal('Ohh..','Unable to modify Inspection: {0}' ,'info');", id);
                return RedirectToAction("Inspections");
            }
            A_INSPH I = db.A_INSPH.Where(i => i.INSPH_ID == id && i.INSPH_JOB == "N" && i.ACT == false).First();
            newInspection E = new newInspection();
            E.INSPH_ID = I.INSPH_ID;
            E.INSPH_ASET = I.INSPH_ASET;
            E.INSPH_DATE = I.INSPH_DATE;
            E.INSPH_IUSER = I.INSPH_IUSER;
            E.A_DEPTF = I.INSPH_DEPARTMENT;
            E.A_AREAF = I.INSPH_AREA;
            E.A_USERF = I.INSPH_OWNR;
            E.MILEAGE = I.MILEAGE;
            E.INSPH_TYPE = I.INSPH_TYPE;
            E.INSPH_NOTE = I.INSPH_NOTE;
            List<InspectionD> ED = new List<InspectionD>();
            List<string> Exist = new List<string>();
            foreach (var item in I.A_INSPD)
            {
                InspectionD D = new InspectionD();
                D.INSPD_ITEM = item.INSPD_ITEM;
                D.INSPD_ITEM_NAME = db.A_ITEM_LIST.Find(item.INSPD_ITEM).ITEM_NAME;
                D.INSPD_IQTY = item.INSPD_IQTY;
                D.INSPD_IRATE = item.INSPD_IRATE;
                D.INSPD_NOTE = item.INSPD_NOTE;
                ED.Add(D);
                Exist.Add(item.INSPD_ITEM);
            }
            E.InspectionD = ED;

            var Detail_Data = from d in db.A_INSPD
                              where d.INSPH_ID == id
                              group new { d } by d.INSPH_ID into g
                              select new
                              {
                                  INSPH = g.Key,
                                  IAmount = g.Sum(x => x.d.INSPD_IRATE * x.d.INSPD_IQTY)
                              };

            ViewBag.IAmount = Detail_Data.FirstOrDefault().IAmount;

            string Group = db.A_ASET_DATA.Where(a => a.ASET_ID == I.INSPH_ASET).First().ASET_GROUP;
            ViewBag.INSPD_ITEM = new SelectList(RP.InspectionItemNot(ig: Group, Exist: Exist), "ITEM_ID", "ITEM_NAME");
            ViewBag.INSPH_ASET = new SelectList(RP.AsetById(I.INSPH_ASET), "ASET_ID", "ASET_NAME_TAG");
            List<SelectListItem> JT = new List<SelectListItem>();
            JT.Add(new SelectListItem { Text = "General", Value = "G" });
            JT.Add(new SelectListItem { Text = "Accident", Value = "A" });
            JT.Add(new SelectListItem { Text = "Scheduled", Value = "S" });
            ViewBag.INSPH_TYPE = JT;
            return View(E);
        }
        [KAuth(401)]
        [HttpPost]
        public ActionResult UpdateInspection(newInspection OI)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (ModelState.IsValid)
            {
                A_INSPH I = db.A_INSPH.Where(i => i.INSPH_ID == OI.INSPH_ID && i.INSPH_JOB == "N" && i.ACT == false).First();
                I.INSPH_IUSER = OI.INSPH_IUSER;
                I.INSPH_DATE = OI.INSPH_DATE;
                I.MILEAGE = OI.MILEAGE;
                I.INSPH_TYPE = OI.INSPH_TYPE;
                I.INSPH_NOTE = OI.INSPH_NOTE;
                I.UDT = DateTime.Now;
                I.UDU = U;
                I.VAR = I.VAR + 1;
                db.Entry(I).State = EntityState.Modified;
                List<A_INSPD> DelD = db.A_INSPD.Where(d => d.INSPH_ID == OI.INSPH_ID).ToList();
                db.A_INSPD.RemoveRange(DelD);
                int CounterId = 1;
                foreach (var item in OI.InspectionD)
                {
                    if (item.INSPD_IQTY == 0 || item.INSPD_IRATE == 0 || item.INSPD_IQTY == null || item.INSPD_IRATE == null)
                    {
                        return Json(new { success = false, inspid = "Unable to modify Inspection: " + OI.INSPH_ID }, JsonRequestBehavior.AllowGet);
                    }
                    A_INSPD D = new A_INSPD();
                    D.INSPH_ID = OI.INSPH_ID;
                    D.INSPD_LINE = Convert.ToByte(CounterId);
                    D.INSPD_ITEM = item.INSPD_ITEM;
                    D.INSPD_IQTY = item.INSPD_IQTY;
                    D.INSPD_IRATE = item?.INSPD_IRATE ?? 0;
                    D.INSPD_NOTE = item.INSPD_NOTE;
                    D.INSPD_STATE = "N";
                    CounterId = CounterId + 1;
                    db.A_INSPD.Add(D);
                }
                db.SaveChanges();
                return Json(new { success = true, inspid = "Inspection modified: " + OI.INSPH_ID }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, inspid = "Unable to modify Inspection: " + OI.INSPH_ID }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDeptUser(string AsetId)
        {
            string Wd, Wa, Wu, Sec, Ad, Aa, Au, Pri;
            var fromAssetData = db.A_ASET_DATA.Where(a => a.ACT == true && a.ASET_ID == AsetId).ToList();
            if (fromAssetData.Count == 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            Ad = Wd = fromAssetData.First().ASET_DEPARTMENT;
            Aa = Wa = fromAssetData.First().ASET_AREA;
            Au = Wu = "0";
            Pri = Sec = RP.InspectionAndJobData(Ad, Aa, Au);

            var fromHandOver = db.A_ASET_USER.Where(a => a.AU_ASET == AsetId && (a.AU_ERUSER == null || a.AU_ERUSER == "")).ToList();
            if (fromHandOver.Count > 0)
            {
                Wd = fromHandOver.FirstOrDefault().AU_DEPT;
                Wa = fromHandOver.FirstOrDefault().AU_AREA;
                Wu = fromHandOver.FirstOrDefault().AU_USER;
                Sec = RP.InspectionAndJobData(Wd, Wa, Wu);
            }
            return Json(new { Department = Wd, Area = Wa, User = Wu, prim = Pri, seco = Sec }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getLastRate(string asetId, string itemId)
        {
            decimal LastRate = 0;
            string Infor = "No record found";
            if (asetId == "" || itemId == "")
            {
                return Json(new { LastRate = LastRate, Infor = Infor }, JsonRequestBehavior.AllowGet);
            }
            var LastInspection = db.A_JOBH.Where(a => a.JOBH_ASET == asetId).OrderByDescending(a => a.JOBH_DATE).ToList();
            if (LastInspection.Count > 0)
            {
                foreach (var item in LastInspection)
                {
                    string id = item.JOBH_ID;
                    string iMileage = item.JOBH_MILEAGE;
                    string iDate = item.JOBH_DATE.ToString();
                    var IsFoundItem = db.A_JOBD.Where(d => d.JOBH_ID == id && d.JOBD_ITEM == itemId).ToList();
                    if (IsFoundItem.Count > 0)
                    {
                        LastRate = IsFoundItem.First().JOBD_JRATE ?? 0;
                        byte iQty = IsFoundItem.First().JOBD_JQTY ?? 0;
                        string iNote = IsFoundItem.First().JOBD_NOTE ?? "";
                        Infor = string.Format("Mileage:{0} -Date:{1} -Item Id:{2} -Qty:{3} -Rate:{4} -Description:{5}", iMileage, iDate, itemId, iQty, LastRate, iNote);
                        return Json(new { LastRate = LastRate, Infor = Infor }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new { LastRate = LastRate, Infor = Infor }, JsonRequestBehavior.AllowGet);
        }
        [KAuth(401)]
        [HttpPost, ActionName("DelIns")]
        public ActionResult DeleteInspectionHeadAndDetails(string id)
        {
            if (id == null)
            {
                TempData["msg"] = "swal('Opps..','Inspection not found','warning');";
                return RedirectToAction("Inspections");
            }
            List<A_INSPH> I = db.A_INSPH.Where(i => i.INSPH_ID == id && i.ACT == false).ToList();
            if (I.Count > 0)
            {
                db.A_INSPH.RemoveRange(I); //delete head
                List<A_INSPD> D = db.A_INSPD.Where(d => d.INSPH_ID == id).ToList();
                if (D.Count > 0)
                {
                    db.A_INSPD.RemoveRange(D); //delete detail
                }
                List<A_INSP_JOB_IMG> IJI = db.A_INSP_JOB_IMG.Where(d => d.IMG_REF == id).ToList();
                if (IJI.Count > 0)
                {
                    db.A_INSP_JOB_IMG.RemoveRange(IJI); //delete attach file
                }
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Inspection deleted','success');";
                return RedirectToAction("Inspections");
            }
            TempData["msg"] = "swal('Opps..','Inspection delete failed','warning');";
            return RedirectToAction("Inspections");
        }
        [KAuth(401)]
        [HttpPost, ActionName("SubmiInsp")]
        public ActionResult SubmitInspection(string id)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (id == null)
            {
                TempData["msg"] = "swal('Opps..','Inspection not found','warning');";
                return RedirectToAction("Inspections");
            }
            A_INSPH I = db.A_INSPH.Find(id);
            if (I == null)
            {
                return HttpNotFound();
            }
            if (I.ACT == true)
            {
                TempData["msg"] = string.Format("swal('Ohh..',' Already Submitted Inspection: {0}' ,'info');", id);
                return RedirectToAction("Inspections");
            }
            else
            {
                I.ACT = true;
                I.UDT = DateTime.Now;
                I.UDU = U;
                db.Entry(I).State = EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = string.Format("swal('Success..','Inspection Submitted without eMail: {0}' ,'success');", id);
                //Need Send Mail=============================================================
                bool SendEmail = IES.Send_Email_KSA_Inspection_Approval(Area: I.INSPH_AREA, inspid: id, Request_Type: "INS");
                if (SendEmail)
                {
                    TempData["msg"] = string.Format("swal('Success..','Inspection Submitted with eMail: {0}' ,'success');", id);
                }
                return RedirectToAction("Inspections");
            }
        }
        [KAuth(403)]
        [HttpPost]
        [ActionName("AppIns")]
        public ActionResult InspectionApprove(string id)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            A_INSPH I = db.A_INSPH.Find(id);
            if (I == null)
            {
                return HttpNotFound();
            }
            if (I.ACT == false)
            {
                TempData["msg"] = string.Format("swal('Ohh..','Inspection Not Submitted : {0}' ,'info');", id);
                return RedirectToAction("InsApproval");
            }
            else if (I.INSPH_APP == "Y")
            {
                TempData["msg"] = string.Format("swal('Ohh..','Inspection Already Approved : {0}' ,'info');", id);
                return RedirectToAction("InsApproval");
            }
            else
            {
                I.INSPH_APBY = U;
                I.INSPH_APP = "Y";
                I.INSPH_APDATE = DateTime.Now;
                I.UDT = DateTime.Now;
                I.UDU = U;
                I.VAR = I.VAR + 1;
                db.Entry(I).State = EntityState.Modified;

                //List<string> Ins_Items = new List<string>();
                //List<A_INSPD> Ins_Details = db.A_INSPD.Where(a => a.INSPH_ID == id && a.INSPD_STATE == "N").ToList();
                //foreach (var item in Ins_Details)
                //{
                //    Ins_Items.Add(item.INSPD_ITEM);
                //}
                //List<A_ITEM_LIST> I_ITEM = db.A_ITEM_LIST
                //    .Where(i => Ins_Items.Contains(i.ITEM_ID) && i.ITEM_NOTIF_SET != "0")
                //    .ToList();
                //if (I_ITEM.Count > 0)
                //{

                //}

                db.SaveChanges();
                TempData["msg"] = string.Format("swal('Success..','Approved Inspection: {0}' ,'success');", id);
                return RedirectToAction("InsApproval");
            }
        }
        [KAuth(403)]
        [HttpPost]
        [ActionName("RejIns")]
        public ActionResult InspectionReject(string id)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            A_INSPH I = db.A_INSPH.Find(id);
            if (I == null)
            {
                return HttpNotFound();
            }
            if (I.ACT == false)
            {
                TempData["msg"] = string.Format("swal('Ohh..','Inspection Not Submitted : {0}' ,'info');", id);
                return RedirectToAction("InsApproval");
            }
            else if (I.INSPH_APP == "Y")
            {
                TempData["msg"] = string.Format("swal('Ohh..','Inspection Already Approved : {0}' ,'info');", id);
                return RedirectToAction("InsApproval");
            }
            else
            {
                I.ACT = false;
                I.INSPH_APBY = U;
                I.INSPH_APP = "N";
                I.INSPH_APDATE = DateTime.Now;
                I.UDT = DateTime.Now;
                I.UDU = U;
                I.VAR = I.VAR + 1;
                db.Entry(I).State = EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = string.Format("swal('Success..','Rejected Inspection: {0}' ,'success');", id);
                return RedirectToAction("InsApproval");
            }
        }

        //public ActionResult InsertAcce()
        //{               
        //    List<A_ASET_DATA> AD = db.A_ASET_DATA.Where(i => i.ASET_GROUP == "GR0001").ToList();
        //    List<A_ACCE_ITEMS> AI = db.A_ACCE_ITEMS.Where(i => i.ACCE_GROUP == "GR0001").ToList();
        //    int Item = AI.Count(), V = AD.Count(), row = 0;

        //    foreach (var v in AD)
        //    {
        //        foreach (var i in AI)
        //        {
        //            A_ASET_ACCE AC = new A_ASET_ACCE();
        //            AC.ASET_ID = v.ASET_ID;
        //            AC.ACCE_ID = i.ACCE_ID;
        //            AC.AAI_DESC = "BE";
        //            AC.AAI_QUANTITY = 1;
        //            AC.AAI_RATE = 1;
        //            AC.ACT = true;
        //            AC.CDT = DateTime.Now;
        //            AC.UDT = DateTime.Now;
        //            AC.VAR = 1;
        //            db.A_ASET_ACCE.Add(AC);
        //            db.SaveChanges();
        //            row += row;
        //        }
        //    }
        //    string RS = string.Format("Total Item: {0} Vehicle: {1} Total: {2}",Item,V,row);
        //    return null;
        //}

        public List<JobCardView> JobCarView(string Status, string Approval)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            #region OLD
            //List<string> ASET = RP.MyAssetListString(UserId: U, UserType: T);

            //  var JC = (from j in db.A_JOBH
            //            join a in db.A_ASET_DATA on j.JOBH_ASET equals a.ASET_ID
            //            join d in db.A_DEPARTMENT on j.JOBH_DEPARTMENT equals d.DEPARTMENT_ID
            //            join o in db.A_OWNR on j.JOBH_OWNR equals o.OWNR_ID
            //            join r in db.A_AREA on j.JOBH_AREA equals r.AREA_ID
            //            where j.ACT == Status
            //            && ASET.Contains(j.JOBH_ASET)
            //            && j.JOBH_APP == Approval
            //            && j.A_INSP_TYPE.ITYPE_ID != "B"
            //            select new JobCardView
            //            {
            //                JOBH_ID = j.JOBH_ID,
            //                JOBH_ASET_ID = a.ASET_ID,
            //                JOBH_ASET = a.ASET_CODE_NAME,
            //                JOBH_OWNR = o.OWNR_USER,
            //                JOBH_DEPARTMENT = d.DEPARTMENT_NAME,
            //                JOBH_AREA = r.AREA_NAME,
            //                JOBH_DATE = j.JOBH_DATE ?? DateTime.Now,
            //                JOBH_MILEAGE = j.JOBH_MILEAGE,
            //                JOBH_VENDOR = j.JOBH_VENDOR,
            //                JOBH_NOTE = j.JOBH_NOTE,
            //                JOBH_IPAY = j.JOBH_IPAY ?? 0,
            //                JOBH_UPAY = j.JOBH_UPAY ?? 0,
            //                JOBH_CPAY = j.JOBH_CPAY ?? 0,
            //                JOBH_TYPE = j.A_INSP_TYPE.ITYPE_NAME
            //            }
            //).ToList(); 
            #endregion


            string sql_1 = @"SELECT 
                            J.JOBH_ID,ASS.ASET_ID JOBH_ASET_ID,ASS.ASET_CODE_NAME JOBH_ASET,OWNER.OWNR_USER JOBH_OWNR,
                            DPT.DEPARTMENT_NAME JOBH_DEPARTMENT,AREA.AREA_NAME JOBH_AREA,NVL(J.JOBH_DATE,SYSDATE) JOBH_DATE,
                            J.JOBH_MILEAGE,J.JOBH_VENDOR,J.JOBH_NOTE,NVL(J.JOBH_IPAY,0) JOBH_IPAY,NVL(J.JOBH_UPAY,0) JOBH_UPAY,
                            NVL(J.JOBH_CPAY,0) JOBH_CPAY,TYP.ITYPE_NAME JOBH_TYPE
                            FROM 
                            PM.A_JOBH J 
                            INNER JOIN PM.A_ASET_DATA ASS ON J.JOBH_ASET = ASS.ASET_ID AND ASS.ACT = 1
                            INNER JOIN PM.A_DEPARTMENT DPT ON J.JOBH_DEPARTMENT = DPT.DEPARTMENT_ID AND DPT.ACT = 1
                            INNER JOIN PM.A_OWNR OWNER ON J.JOBH_OWNR = OWNER.OWNR_ID AND OWNER.ACT = 1
                            INNER JOIN PM.A_AREA AREA ON J.JOBH_AREA = AREA.AREA_ID AND AREA.ACT = 1
                            INNER JOIN PM.A_INSP_TYPE TYP ON J.JOBH_TYPE = TYP.ITYPE_ID AND TYP.ACT = 1
                            INNER JOIN PM.A_ASET_DATA T6 ON T6.ASET_ID=J.JOBH_ASET AND T6.ACT=1
                            INNER JOIN PM.A_USERS_GROUP T5 ON T6.ASET_GROUP=T5.UG_GROUP AND T5.UG_USER='" + U + @"' AND T5.ACT=1
                            INNER JOIN PM.A_DEPARTMENT T7 ON T6.ASET_DEPARTMENT=T7.DEPARTMENT_ID AND T7.ACT=1
                            INNER JOIN PM.A_BUSINESS T8 ON T7.DEPARTMENT_BUSINESS=T8.BUSINESS_ID AND T8.ACT=1
                            INNER JOIN PM.A_USERS_BUS T9 ON T8.BUSINESS_ID =T9.UB_BUSINESS AND T9.UB_USER=T5.UG_USER AND T9.ACT=1 
                            INNER JOIN (SELECT DISTINCT(AREA_ID) FROM (SELECT T.AREA_ID
                            FROM PM.A_AREA T
                            INNER JOIN PM.A_THANA T1 ON T.AREA_THANA=T1.THANA_ID AND T1.ACT=1
                            INNER JOIN PM.A_DISTRICT T2 ON T1.THANA_DISTRICT=T2.DISTRICT_ID AND T2.ACT=1
                            INNER JOIN PM.A_USERS_COUNTRY T3 ON T2.DISTRICT_COUNTRY=T3.COUNTRY_ID AND T3.ACT=1 AND T3.USER_TEXT='" + U + @"'
                            WHERE T.ACT=1
                            UNION ALL
                            SELECT T.UA_AREA
                            FROM PM.A_USERS_AREA T WHERE T.ACT=1 AND T.UA_USER='" + U + @"'
                            )) T10 ON T6.ASET_AREA=T10.AREA_ID
                            WHERE J.ACT = '" + Status + @"' 
                            AND j.JOBH_APP = '" + Approval + @"'
                            AND TYP.ITYPE_ID <> 'B'";
            var JC = db.Database.SqlQuery<JobCardView>(sql: sql_1).ToList();
            return JC;
        }
        [KAuth(402)]
        public ActionResult JobCard()
        {
            var JC = JobCarView("0", "N");
            return View(JC);
        }
        [KAuth(404)]
        public ActionResult JobApproval()
        {
            var JC = JobCarView("1", "N");
            return View(JC);
        }

        public ActionResult ViewJobCard(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Detail_Data = from d in db.A_JOBD
                              where d.JOBH_ID == id
                              group new { d } by d.JOBH_ID into g
                              select new
                              {
                                  JOBH = g.Key,
                                  IAmount = g.Sum(x => x.d.JOBD_IRATE * x.d.JOBD_IQTY),
                                  JAmount = g.Sum(x => x.d.JOBD_JRATE * x.d.JOBD_JQTY),
                                  IJDifferent = g.Sum(x => x.d.JOBD_JRATE * x.d.JOBD_JQTY) - g.Sum(x => x.d.JOBD_IRATE * x.d.JOBD_IQTY)
                              };

            ViewBag.JOBH = Detail_Data.FirstOrDefault().JOBH;
            ViewBag.IAmount = Detail_Data.FirstOrDefault().IAmount;
            ViewBag.JAmount = Detail_Data.FirstOrDefault().JAmount;
            ViewBag.IJDifferent = Detail_Data.FirstOrDefault().IJDifferent;

            var VI = (from d in db.A_JOBD
                      join i in db.A_ITEM_LIST on d.JOBD_ITEM equals i.ITEM_ID
                      where d.JOBH_ID == id
                      select new JobCardDetails
                      {
                          JOBH_ID = d.JOBH_ID,
                          JOBD_LINE = d.JOBD_LINE,
                          JOBD_ITEM = d.JOBD_ITEM,
                          JOBD_ITEM_NAME = i.ITEM_NAME,
                          JOBD_IQTY = d.JOBD_IQTY ?? 0,
                          JOBD_IRATE = d.JOBD_IRATE ?? 0,
                          JOBD_JQTY = d.JOBD_JQTY ?? 0,
                          JOBD_JRATE = d.JOBD_JRATE ?? 0,
                          JOBD_NOTE = d.JOBD_NOTE
                      }).ToList();
            return PartialView(VI);
        }
        [KAuth(404)]
        public ActionResult JobCardAppDetail(string id, string AsetId, string aset)
        {
            if (id == null || AsetId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Detail_Data = from d in db.A_JOBD
                              where d.JOBH_ID == id
                              group new { d } by d.JOBH_ID into g
                              select new
                              {
                                  JOBH = g.Key,
                                  IAmount = g.Sum(x => x.d.JOBD_IRATE * x.d.JOBD_IQTY),
                                  JAmount = g.Sum(x => x.d.JOBD_JRATE * x.d.JOBD_JQTY),
                                  IJDifferent = g.Sum(x => x.d.JOBD_JRATE * x.d.JOBD_JQTY) - g.Sum(x => x.d.JOBD_IRATE * x.d.JOBD_IQTY)
                              };

            ViewBag.JOBH = Detail_Data.FirstOrDefault().JOBH;
            ViewBag.IAmount = Detail_Data.FirstOrDefault().IAmount;
            ViewBag.JAmount = Detail_Data.FirstOrDefault().JAmount;
            ViewBag.IJDifferent = Detail_Data.FirstOrDefault().IJDifferent;
            ViewBag.ASET_ID = aset;

            List<InspectionDx> Old_data_Dup = (from h in db.A_JOBH
                                               join d in db.A_JOBD on h.JOBH_ID equals d.JOBH_ID
                                               where h.ACT == true
                                               && h.JOBH_ASET == AsetId //take date by aset
                                               && h.JOBH_ID != id //ignore this self
                                               select new InspectionDx
                                               {
                                                   INSPH_ID = h.JOBH_ID,
                                                   INSPH_ASET = h.JOBH_ASET,
                                                   INSPD_ITEM = d.JOBD_ITEM,
                                                   INSPD_IQTY = d.JOBD_IQTY,
                                                   INSPH_DATE = h.JOBH_DATE,
                                                   MILEAGE = h.JOBH_MILEAGE
                                               }).OrderByDescending(s => s.INSPH_DATE).ToList();

            List<InspectionDx> Old_data = new List<InspectionDx>();
            foreach (var item in Old_data_Dup)
            {
                bool isExist = Old_data.Where(e => e.INSPD_ITEM == item.INSPD_ITEM).Any();
                if (!isExist)
                {
                    Old_data.Add(item);
                }
            }


            List<JobCardDetails> VI = (from d in db.A_JOBD
                                       join i in db.A_ITEM_LIST on d.JOBD_ITEM equals i.ITEM_ID
                                       where d.JOBH_ID == id
                                       select new JobCardDetails
                                       {
                                           JOBH_ID = d.JOBH_ID,
                                           JOBD_LINE = d.JOBD_LINE,
                                           JOBD_ITEM = d.JOBD_ITEM,
                                           JOBD_ITEM_NAME = i.ITEM_NAME,
                                           JOBD_IQTY = d.JOBD_IQTY ?? 0,
                                           JOBD_IRATE = d.JOBD_IRATE ?? 0,
                                           JOBD_JQTY = d.JOBD_JQTY ?? 0,
                                           JOBD_JRATE = d.JOBD_JRATE ?? 0,
                                           JOBD_NOTE = d.JOBD_NOTE
                                       }).ToList();

            List<JobCardDetails_With_Old> New_Data = new List<JobCardDetails_With_Old>();
            New_Data = (from n in VI
                        from o in Old_data.Where(x => x.INSPD_ITEM == n.JOBD_ITEM).DefaultIfEmpty()
                        select new JobCardDetails_With_Old
                        {
                            JOBH_ID = n.JOBH_ID,
                            JOBD_LINE = n.JOBD_LINE,
                            JOBD_ITEM = n.JOBD_ITEM,
                            JOBD_ITEM_NAME = n.JOBD_ITEM_NAME,
                            JOBD_IQTY = n.JOBD_IQTY ?? 0,
                            JOBD_IRATE = n.JOBD_IRATE ?? 0,
                            JOBD_JQTY = n.JOBD_JQTY ?? 0,
                            JOBD_JRATE = n.JOBD_JRATE ?? 0,
                            JOBD_NOTE = n?.JOBD_NOTE ?? string.Empty,
                            JOBH_DATE = o?.INSPH_DATE ?? null,
                            JOBH_MILEAGE = o?.MILEAGE ?? string.Empty
                        }).ToList();
            return View(New_Data);
        }

        [KAuth(402)]
        [HttpGet]
        public ActionResult NewJobCard()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            List<string> ASET = RP.MyAssetListString(UserId: U, UserType: T);
            ViewBag.JOBH_ASET = new SelectList(RP.AsetByListId(AssetId: ASET), "ASET_ID", "ASET_NAME_TAG");
            return View();
        }
        [KAuth(402)]
        [HttpPost]
        public ActionResult NewJobCard(JobCard JC)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            DateTime IDATE = Convert.ToDateTime(JC.JOBH_DATE);
            var IsExistToday = db.A_JOBH.Where(a => a.JOBH_ASET == JC.JOBH_ASET && a.JOBH_DATE == JC.JOBH_DATE).ToList();
            if (IsExistToday.Count() > 0)
            {
                return Json(new { jobid = "One Job Card already created today: " + IsExistToday.First().JOBH_ID }, JsonRequestBehavior.AllowGet);
            }

            var IsPendingForBill = db.A_JOBH.Where(a => a.JOBH_ASET == JC.JOBH_ASET && a.JOBH_BILL == "N").ToList();
            if (IsPendingForBill.Count() > 1)
            {
                return Json(new { jobid = IsPendingForBill.Count + " Jobcard(s) pending for Bill, please clear first" }, JsonRequestBehavior.AllowGet);
            }

            if (ModelState.IsValid)
            {
                A_INSPH IH = db.A_INSPH.Find(JC.INSPH_ID);
                if (IH == null)
                {
                    return null;
                }
                IH.INSPH_JOB = "Y";
                IH.UDU = U;
                IH.UDT = DateTime.Now;
                IH.VAR = IH.VAR + 1;
                db.Entry(IH).State = EntityState.Modified;


                decimal TotalAmount = 0; //get amount from Inspection Details.                                
                if (JC.JOBH_UPAY > 0 || JC.JOBH_UPAY != null)
                {
                    TotalAmount = (TotalAmount - JC.JOBH_UPAY) ?? 0;
                }

                string Insu_Id = "";
                if (JC.JOBH_IPAY > 0 || JC.JOBH_IPAY != null)
                {
                    TotalAmount = (TotalAmount - JC.JOBH_IPAY) ?? 0;
                    Insu_Id = INSURANCE_NO(IH.INSPH_ASET);
                    if (Insu_Id == "" || Insu_Id == null || string.IsNullOrEmpty(Insu_Id))
                    {
                        return Json(new { jobid = "No Policy Found, Please Apply Policy First" }, JsonRequestBehavior.AllowGet);
                    }
                }

                string GetSeq = JC.JOBH_ASET;
                string id = ge.FifteenDigitId("JOBC", GetSeq, dt: IDATE);
                A_JOBH JH = new A_JOBH();
                JH.JOBH_ID = id;
                JH.INSPH_ID = JC.INSPH_ID; //User Ins Id
                JH.JOBH_ASET = IH.INSPH_ASET;
                JH.JOBH_OWNR = IH.INSPH_OWNR;
                JH.JOBH_DEPARTMENT = IH.INSPH_DEPARTMENT;
                JH.JOBH_AREA = IH.INSPH_AREA;
                JH.JOBH_DATE = JC.JOBH_DATE;//User Date
                JH.JOBH_MILEAGE = IH.MILEAGE;
                JH.JOBH_VENDOR = JC.JOBH_VENDOR;//User Vendor
                JH.JOBH_NOTE = JC.JOBH_NOTE;//User Note
                JH.JOBH_TYPE = IH.INSPH_TYPE;
                JH.JOBH_CNAME = Insu_Id;
                JH.JOBH_BILL = "N";
                JH.JOBH_APP = "N";
                JH.ACT = false;
                JH.CDT = DateTime.Now;
                JH.CDU = U;
                JH.VAR = 1;
                byte counter = 1;
                foreach (var item in JC.JobCardDetails)
                {
                    A_JOBD JD = new A_JOBD();
                    JD.JOBH_ID = id;
                    JD.JOBD_LINE = counter;
                    JD.JOBD_ITEM = item.JOBD_ITEM;
                    JD.JOBD_IQTY = item.JOBD_IQTY;
                    JD.JOBD_JQTY = item.JOBD_JQTY;
                    JD.JOBD_IRATE = item?.JOBD_IRATE ?? 0;
                    JD.JOBD_JRATE = item?.JOBD_JRATE ?? 0;
                    JD.JOBD_NOTE = item.JOBD_NOTE;
                    JD.JOBD_STATE = "J";
                    db.A_JOBD.Add(JD);
                    counter += 1;
                    TotalAmount += item.JOBD_JQTY * item.JOBD_JRATE ?? 1;
                }
                if (TotalAmount < 0)
                {
                    return Json(new { jobid = "Amount can not less then zero" }, JsonRequestBehavior.AllowGet);
                }
                JH.JOBH_IPAY = JC.JOBH_IPAY ?? 0;
                JH.JOBH_UPAY = JC.JOBH_UPAY ?? 0;
                JH.JOBH_CPAY = TotalAmount;
                JH.JOBH_DISC = 0;
                db.A_JOBH.Add(JH);
                db.SaveChanges();
                return Json(new { success = true, jobid = "Job Card: " + id }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { jobid = "Job not created" }, JsonRequestBehavior.AllowGet);
        }
        [KAuth(402)]
        private string INSURANCE_NO(string id)
        {
            string XX = "";
            var INSU_NO = db.A_INSUH.Where(i => i.INSU_ASET == id && i.ACT == true).FirstOrDefault();
            if (INSU_NO == null)
            {
                return XX;
            }
            else
            {
                return XX = INSU_NO.ToString();
            }
        }

        [HttpGet]
        public ActionResult InspectionItems(string id)
        {
            var Ins_Detail = from d in db.A_INSPD
                             join i in db.A_ITEM_LIST on d.INSPD_ITEM equals i.ITEM_ID
                             where d.INSPH_ID == id
                             select new JobCardDetails
                             {
                                 JOBD_ITEM = d.INSPD_ITEM,
                                 JOBD_ITEM_NAME = i.ITEM_NAME,
                                 JOBD_IQTY = d.INSPD_IQTY,
                                 JOBD_IRATE = d.INSPD_IRATE,
                                 JOBD_JQTY = d.INSPD_IQTY, // JQTY same as IQTY
                                 JOBD_JRATE = d.INSPD_IRATE, // JOBD_JRATE same as INSPD_IRATE
                                 JOBD_NOTE = d.INSPD_NOTE
                             };
            return PartialView(Ins_Detail);
        }
        [KAuth(402)]
        [HttpGet]
        public ActionResult EditJobCard(string id)
        {
            var Editable = db.A_JOBH.Where(i => i.JOBH_ID == id && i.JOBH_APP == "N" && i.JOBH_BILL == "N" && i.ACT == false).ToList();
            if (Editable.Count == 0)
            {
                TempData["msg"] = string.Format("swal('Ohh..','Unable to modify Job Card: {0}' ,'info');", id);
                return RedirectToAction("JobCard");
            }
            A_JOBH I = db.A_JOBH.Where(i => i.JOBH_ID == id && i.JOBH_APP == "N" && i.JOBH_BILL == "N" && i.ACT == false).First();
            EditJobCard E = new EditJobCard();
            E.JOBH_ID = id;
            E.INSPH_ID = I.INSPH_ID;
            E.JOBH_ASET = I.JOBH_ASET;
            E.JOBH_VENDOR = I.JOBH_VENDOR;
            E.JOBH_NOTE = I.JOBH_NOTE;
            E.JOBH_IPAY = I.JOBH_IPAY;
            E.JOBH_UPAY = I.JOBH_UPAY;
            List<JobCardDetails> ED = new List<JobCardDetails>();
            List<string> Exist = new List<string>();
            foreach (var item in I.A_JOBD)
            {
                JobCardDetails D = new JobCardDetails();
                D.JOBD_ITEM = item.JOBD_ITEM;
                D.JOBD_ITEM_NAME = db.A_ITEM_LIST.Find(item.JOBD_ITEM).ITEM_NAME;
                D.JOBD_IQTY = item.JOBD_IQTY;
                D.JOBD_JQTY = item.JOBD_JQTY;
                D.JOBD_IRATE = item.JOBD_IRATE;
                D.JOBD_JRATE = item.JOBD_JRATE;
                D.JOBD_NOTE = item.JOBD_NOTE;
                ED.Add(D);
                Exist.Add(item.JOBD_ITEM);
            }
            E.JobCardDetails = ED;
            string Group = db.A_ASET_DATA.Where(a => a.ASET_ID == I.JOBH_ASET).First().ASET_GROUP;
            ViewBag.JOBH_ASET = new SelectList(RP.AsetById(I.JOBH_ASET), "ASET_ID", "ASET_NAME_TAG");
            return View(E);
        }
        [KAuth(402)]
        [HttpPost]
        public ActionResult EditJobCard(EditJobCard JC)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (ModelState.IsValid)
            {
                A_JOBH JH = db.A_JOBH.Where(j => j.JOBH_APP == "N" && j.ACT == false && j.JOBH_ID == JC.JOBH_ID).FirstOrDefault();
                if (JH != null)
                {
                    decimal TotalAmount = 0; //get amount from Inspection Details.                                
                    if (JC.JOBH_UPAY > 0 || JC.JOBH_UPAY != null)
                    {
                        TotalAmount = TotalAmount - JC.JOBH_UPAY ?? 0;
                    }
                    if (JC.JOBH_IPAY > 0 || JC.JOBH_IPAY != null)
                    {
                        TotalAmount = TotalAmount - JC.JOBH_IPAY ?? 0;
                    }
                    JH.JOBH_VENDOR = JC.JOBH_VENDOR;
                    JH.JOBH_NOTE = JC.JOBH_NOTE;
                    JH.JOBH_UPAY = JC.JOBH_UPAY;
                    JH.JOBH_IPAY = JC.JOBH_IPAY;
                    JH.UDU = U;
                    JH.UDT = DateTime.Now;
                    JH.VAR = JH.VAR + 1;
                    foreach (var item in JC.JobCardDetails)
                    {
                        A_JOBD JD = db.A_JOBD.Where(i => i.JOBD_ITEM == item.JOBD_ITEM && i.JOBH_ID == JC.JOBH_ID).FirstOrDefault();
                        JD.JOBD_JQTY = item.JOBD_JQTY;
                        JD.JOBD_JRATE = item?.JOBD_JRATE ?? 0;
                        JD.JOBD_NOTE = item.JOBD_NOTE;
                        db.Entry(JD).State = EntityState.Modified;
                        TotalAmount += item.JOBD_JQTY * item?.JOBD_JRATE ?? 0;
                    }
                    if (TotalAmount < 0)
                    {
                        return Json(new { success = false, jobid = "Amount can not less then zero" }, JsonRequestBehavior.AllowGet);
                    }
                    db.SaveChanges();
                    return Json(new { success = true, jobid = "Job Card Edited: " + JC.JOBH_ID }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false, jobid = "Job Card Not Valid: " + JC.JOBH_ID }, JsonRequestBehavior.AllowGet);
        }
        [KAuth(402)]
        [HttpPost, ActionName("SubmiJob")]
        public ActionResult SubmitJob(string id)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (id == null)
            {
                TempData["msg"] = "swal('Opps..','Job not found','warning');";
                return RedirectToAction("JobCard");
            }
            A_JOBH I = db.A_JOBH.Find(id);
            if (I == null)
            {
                return HttpNotFound();
            }
            if (I.ACT == true)
            {
                TempData["msg"] = string.Format("swal('Ohh..',' Already Submitted Job: {0}' ,'info');", id);
                return RedirectToAction("JobCard");
            }
            else
            {
                I.UDU = U;
                I.ACT = true;
                I.UDT = DateTime.Now;
                db.Entry(I).State = EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = string.Format("swal('Success..','Job Submitted without eMail: {0}' ,'success');", id);
                //Need Send Mail=============================================================
                bool SendEmail = IES.Send_Email_KSA_Inspection_Approval(Area: I.JOBH_AREA, inspid: id, Request_Type: "JOB");
                if (SendEmail)
                {
                    TempData["msg"] = string.Format("swal('Success..','Job Submitted with eMail: {0}' ,'success');", id);
                }
                return RedirectToAction("JobCard");
            }
        }
        [KAuth(404)]
        [HttpPost]
        [ActionName("AppJob")]
        public ActionResult JobCardApprove(string id)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            A_JOBH I = db.A_JOBH.Find(id);
            if (I == null)
            {
                return HttpNotFound();
            }
            if (I.ACT == false)
            {
                TempData["msg"] = string.Format("swal('Ohh..','Job Card Not Submitted : {0}' ,'info');", id);
                return RedirectToAction("JobApproval");
            }
            else if (I.JOBH_APP == "Y")
            {
                TempData["msg"] = string.Format("swal('Ohh..','Job Card Already Approved : {0}' ,'info');", id);
                return RedirectToAction("JobApproval");
            }
            else
            {
                I.JOBH_APP = "Y";
                I.JOBH_APBY = U;
                I.JOBH_APDATE = DateTime.Now;
                I.UDT = DateTime.Now;
                I.UDU = U;
                I.VAR = I.VAR + 1;
                db.Entry(I).State = EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = string.Format("swal('Success..','Approved Job Card: {0}' ,'success');", id);
                return RedirectToAction("JobApproval");
            }
        }
        [KAuth(404)]
        [HttpPost]
        [ActionName("RejJob")]
        public ActionResult JobCardReject(string id)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            A_JOBH I = db.A_JOBH.Find(id);
            if (I == null)
            {
                return HttpNotFound();
            }
            if (I.ACT == false)
            {
                TempData["msg"] = string.Format("swal('Ohh..','Job Card Not Submitted : {0}' ,'info');", id);
                return RedirectToAction("JobApproval");
            }
            else if (I.JOBH_APP == "Y")
            {
                TempData["msg"] = string.Format("swal('Ohh..','Job Card Already Approved : {0}' ,'info');", id);
                return RedirectToAction("JobApproval");
            }
            else
            {
                I.ACT = false;
                I.JOBH_APP = "N";
                I.JOBH_APBY = U;
                I.JOBH_APDATE = DateTime.Now;
                I.UDU = U;
                I.UDT = DateTime.Now;
                I.VAR = I.VAR + 1;
                db.Entry(I).State = EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = string.Format("swal('Success..','Rejected Job Card: {0}' ,'success');", id);
                return RedirectToAction("JobApproval");
            }
        }

        [HttpGet]
        public ActionResult AttachFile(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.TRN_ID = id;
            return PartialView();
        }

        [HttpPost]
        public ActionResult AttachFile(AttachFile AF)
        {
            if (ModelState.IsValid)
            {
                var Upload_Path = ge.GetImgPath(103);//get file path
                if (AF.file.ContentLength <= 0 || AF.file.ContentLength > 2100000)
                {
                    TempData["msg"] = string.Format("swal('Opps..','{0} File not uploaded, File is too larger, Max file size 2MB','warning');", AF.file.FileName);
                    return Redirect(Request.UrlReferrer.ToString());
                }
                string fname;
                // Checking for Internet Explorer      
                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    string[] testfiles = AF.file.FileName.Split(new char[] { '\\' });
                    fname = testfiles[testfiles.Length - 1];
                }
                else
                {
                    fname = AF.file.FileName;
                }
                var Exits = db.A_INSP_JOB_IMG.Where(f => f.IMG_REF == AF.TRN_ID).ToList();
                if (Exits.Count > 0)
                {
                    TempData["msg"] = string.Format("swal('Ohh..','File Already uploaed' ,'info');");
                    return Redirect(Request.UrlReferrer.ToString());
                }
                A_INSP_JOB_IMG IJI = new A_INSP_JOB_IMG();
                IJI.IMG_ID = Guid.NewGuid().ToString();
                IJI.IMG_NAME = fname;
                IJI.IMG_PATH = Upload_Path;//get file path
                IJI.IMG_REF = AF.TRN_ID;
                IJI.IMG_CDT = System.DateTime.Now;
                db.A_INSP_JOB_IMG.Add(IJI);
                var path = System.IO.Path.Combine(Server.MapPath(IJI.IMG_PATH), IJI.IMG_ID);
                AF.file.SaveAs(path); //upload file to server
                db.SaveChanges();
                TempData["msg"] = string.Format("swal('Ohh..','File Uploaded' ,'success');");
                return Redirect(Request.UrlReferrer.ToString());
            }
            TempData["msg"] = string.Format("swal('Ohh..','File upload failed' ,'warning');");
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult DownloadAttachFile(string id)
        {
            var dbDATA = db.A_INSP_JOB_IMG.Where(a => a.IMG_REF == id);

            if (dbDATA.ToList().Count == 0)
            {
                TempData["msg"] = "swal('Opps..','File not found','warning');";
                return Redirect(Request.UrlReferrer.ToString());
            }
            var Upload_Path = dbDATA.FirstOrDefault().IMG_PATH;
            var GetFileGUIDname = dbDATA.FirstOrDefault().IMG_ID;
            var GetFileOrgName = dbDATA.FirstOrDefault().IMG_NAME;
            var CombinePath = System.IO.Path.Combine(Server.MapPath(Upload_Path), GetFileGUIDname);
            try
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(CombinePath);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, GetFileOrgName);
            }
            catch
            {
                TempData["msg"] = "swal('Ohh..','Error in File','error');";
                return Redirect(Request.UrlReferrer.ToString());
            }
        }

        #region Bulk_Inspection_JobCard_Bill
        [KAuth(401)]
        public ActionResult NewBundleInspection()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            string GROUP_ID = "GR0001";
            List<string> ASET = RP.MyAssetListString(UserId: U, UserType: T);
            ViewBag.INSPH_ASET = new SelectList(RP.AsetByListId(AssetId: ASET), "ASET_ID", "ASET_NAME_TAG");
            ViewBag.INSPD_ITEM = new SelectList(RP.BundleInspectionItem(ig: GROUP_ID), "ITEM_ID", "ITEM_NAME");
            return View();
        }
        [KAuth(401)]
        [HttpPost]
        public ActionResult SaveBundleInspection(NewBundleInspection NI)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            string GROUP_ID = "GR0001";
            Guid g = Guid.NewGuid();
            //DateTime IDATE = DateTime.Now.Date;
            DateTime IDATE = Convert.ToDateTime(NI.INSPH_DATE);
            foreach (var item in NI.NewBundleInspectionD)
            {
                var IsExistToday = db.A_INSPH.Where(a => a.INSPH_ASET == item.INSPH_ASET && a.INSPH_DATE == IDATE).ToList();
                if (IsExistToday.Count > 0)
                {
                    return Json(new { inspid = "Inspection already created today: " + IsExistToday.First().INSPH_ID + ", Code:" + IsExistToday.First().INSPH_ASET }, JsonRequestBehavior.AllowGet);
                }
            }

            if (ModelState.IsValid)
            {
                int CounterId = 0;
                foreach (var item in NI.NewBundleInspectionD)
                {
                    string GetSeq = item.INSPH_ASET;//ge.GetSeq(15).ToString();
                    string id = ge.FifteenDigitId("INSP", GetSeq, dt: IDATE);
                    JsonResult JR = PMC.CaseAndPenaltyUserData(item.INSPH_ASET, pdt: IDATE);
                    string json = JsonConvert.SerializeObject(JR.Data);
                    JRdata dto = JsonConvert.DeserializeObject<JRdata>(json);

                    A_INSPH H = new A_INSPH();
                    H.INSPH_ID = id;
                    H.INSPH_ASET = item.INSPH_ASET;
                    H.INSPH_OWNR = dto.USER_NAME;// NI.A_USERF;
                    H.INSPH_DEPARTMENT = dto.DEPT_NAME;// NI.A_DEPTF;
                    H.INSPH_AREA = dto.AREA_NAME;// NI.A_AREAF;
                    H.INSPH_IUSER = NI.INSPH_IUSER;
                    H.INSPH_DATE = IDATE;
                    //H.INSPH_DATE = NI.INSPH_DATE;
                    H.MILEAGE = item.MILEAGE;
                    H.INSPH_JOB = "N";
                    H.INSPH_TYPE = "B";
                    H.INSPH_NOTE = item.INSPH_NOTE;
                    H.INSPH_APP = "N";
                    H.CDT = DateTime.Now;
                    H.ACT = true;
                    H.CDU = U;
                    H.VAR = 1;
                    //  
                    A_INSPD D = new A_INSPD();
                    D.INSPH_ID = id;
                    D.INSPD_LINE = 1;// Convert.ToByte(CounterId);
                    D.INSPD_ITEM = NI.INSPD_ITEM;
                    D.INSPD_IQTY = 1;
                    D.INSPD_IRATE = item.INSPD_IRATE;
                    D.INSPD_NOTE = g.ToString();// "Bundle id";
                    D.INSPD_STATE = "B";
                    if (item.MILEAGE == "" || item.INSPD_IRATE == 0 || item.MILEAGE == null || item.INSPD_IRATE == null)
                    {
                        List<string> ASETx = RP.MyAssetListString(UserId: U, UserType: T);
                        ViewBag.INSPH_ASET = new SelectList(RP.AsetByListId(AssetId: ASETx), "ASET_ID", "ASET_NAME_TAG");
                        ViewBag.INSPD_ITEM = new SelectList(RP.BundleInspectionItem(ig: GROUP_ID), "ITEM_ID", "ITEM_NAME");
                        return Json(new { success = false, inspid = "Inspection not saved " }, JsonRequestBehavior.AllowGet);
                    }
                    db.A_INSPH.Add(H);
                    db.A_INSPD.Add(D);
                    CounterId = CounterId + 1;
                }
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                                       ve.PropertyName,
                                       eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                                       ve.ErrorMessage);
                        }
                    }
                    throw;
                }
                return Json(new { success = true, inspid = CounterId + " Inspections Created" }, JsonRequestBehavior.AllowGet);
            }
            List<string> ASET = RP.MyAssetListString(UserId: U, UserType: T);
            ViewBag.INSPH_ASET = new SelectList(RP.AsetByListId(AssetId: ASET), "ASET_ID", "ASET_NAME_TAG");
            ViewBag.INSPD_ITEM = new SelectList(RP.BundleInspectionItem(ig: GROUP_ID), "ITEM_ID", "ITEM_NAME");
            return Json(new { success = false, inspid = "Inspection not saved " }, JsonRequestBehavior.AllowGet);
        }
        //[KAuth(401)]
        //[HttpPost]
        //public ActionResult SaveBundleInspection(NewBundleInspection NI)
        //{
        //    if (Session["UserId"] == null || Session["UserRole"] == null)
        //    {
        //        return Json(new { success = false, inspid = "Session expired. Please log in again." }, JsonRequestBehavior.AllowGet);
        //    }

        //    string U = Session["UserId"].ToString();
        //    string T = Session["UserRole"].ToString();
        //    string GROUP_ID = "GR0001";
        //    Guid g = Guid.NewGuid();

        //    DateTime IDATE;
        //    if (!DateTime.TryParse(NI.INSPH_DATE?.ToString(), out IDATE))
        //    {
        //        return Json(new { success = false, inspid = "Invalid date format." }, JsonRequestBehavior.AllowGet);
        //    }

        //    // Check duplicates before insert
        //    foreach (var item in NI.NewBundleInspectionD)
        //    {
        //        var exists = db.A_INSPH.Any(a => a.INSPH_ASET == item.INSPH_ASET && a.INSPH_DATE == IDATE);
        //        if (exists)
        //        {
        //            return Json(new
        //            {
        //                success = false,
        //                inspid = $"Inspection already created today for Code: {item.INSPH_ASET}"
        //            }, JsonRequestBehavior.AllowGet);
        //        }
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        return Json(new { success = false, inspid = "Invalid input." }, JsonRequestBehavior.AllowGet);
        //    }

        //    int successCount = 0;
        //    foreach (var item in NI.NewBundleInspectionD)
        //    {
        //        // Validate each item safely
        //        if (string.IsNullOrWhiteSpace(item.MILEAGE) || item.INSPD_IRATE == null || item.INSPD_IRATE == 0)
        //        {
        //            continue; // skip this invalid item, continue with others
        //        }

        //        try
        //        {
        //            string id = ge.FifteenDigitId("INSP", item.INSPH_ASET, IDATE);
        //            JsonResult JR = PMC.CaseAndPenaltyUserData(item.INSPH_ASET, pdt: IDATE);
        //            string json = JsonConvert.SerializeObject(JR.Data);
        //            JRdata dto = JsonConvert.DeserializeObject<JRdata>(json);

        //            var H = new A_INSPH
        //            {
        //                INSPH_ID = id,
        //                INSPH_ASET = item.INSPH_ASET,
        //                INSPH_OWNR = dto.USER_NAME,
        //                INSPH_DEPARTMENT = dto.DEPT_NAME,
        //                INSPH_AREA = dto.AREA_NAME,
        //                INSPH_IUSER = NI.INSPH_IUSER,
        //                INSPH_DATE = IDATE,
        //                MILEAGE = item.MILEAGE,
        //                INSPH_JOB = "N",
        //                INSPH_TYPE = "B",
        //                INSPH_NOTE = item.INSPH_NOTE,
        //                INSPH_APP = "N",
        //                CDT = DateTime.Now,
        //                ACT = true,
        //                CDU = U,
        //                VAR = 1
        //            };

        //            var D = new A_INSPD
        //            {
        //                INSPH_ID = id,
        //                INSPD_LINE = 1,
        //                INSPD_ITEM = NI.INSPD_ITEM,
        //                INSPD_IQTY = 1,
        //                INSPD_IRATE = item.INSPD_IRATE,
        //                INSPD_NOTE = g.ToString(),
        //                INSPD_STATE = "B"
        //            };

        //            db.A_INSPH.Add(H);
        //            db.A_INSPD.Add(D);
        //            successCount++;
        //        }
        //        catch (Exception ex)
        //        {
        //            // Optionally log error for this item
        //            continue;
        //        }
        //    }

        //    try
        //    {
        //        if (successCount > 0)
        //        {
        //            db.SaveChanges();
        //            return Json(new { success = true, inspid = $"{successCount} inspections created." }, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json(new { success = false, inspid = "No valid inspections to save." }, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        var errors = e.EntityValidationErrors
        //            .SelectMany(x => x.ValidationErrors)
        //            .Select(x => $"{x.PropertyName}: {x.ErrorMessage}")
        //            .ToList();

        //        return Json(new { success = false, inspid = "Validation error", errors }, JsonRequestBehavior.AllowGet);
        //    }
        //}
        public List<BundleInspectionApp> BIA(string APP, string JOB)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            List<string> ASET = RP.MyAssetListString(UserId: U, UserType: T);

            List<BundleInspectionApp> BI = db.VW_BNDL_INSP
         .Where(c => c.INSPH_APP == APP && c.INSPH_JOB == JOB && ASET.Contains(c.INSPH_ASET))
         .GroupBy(x => x.INSPD_NOTE)
         .Select(y => new BundleInspectionApp
         {
             BUNDLE_ID = y.Key,
             AMOUNT = y.Sum(x => x.INSPD_IQTY * x.INSPD_IRATE),
             TOTAL_ASET = y.Count()
         }
         ).ToList();
            return BI;
        }

        [KAuth(406)]
        public ActionResult BundleInspectionApp()
        {
            List<BundleInspectionApp> BI = BIA(APP: "N", JOB: "N");
            return View(BI);
        }
        [KAuth(406)]
        [HttpPost]
        public ActionResult BundleInspectionApp(string id)
        {
            string U = Session["UserId"].ToString();
            if (id != null || id == "")
            {
                List<string> INSP_ID = db.VW_BNDL_INSP.Where(s => s.INSPD_NOTE == id && s.INSPH_APP == "N" && s.INSPH_JOB == "N").Select(x => x.INSPH_ID).ToList();
                int Counter = 0;
                foreach (var item in INSP_ID)
                {
                    A_INSPH I = db.A_INSPH.Find(item);
                    I.INSPH_APBY = U;
                    I.INSPH_APP = "Y";
                    I.INSPH_APDATE = DateTime.Now;
                    I.UDT = DateTime.Now;
                    I.UDU = U;
                    I.VAR = I.VAR + 1;
                    db.Entry(I).State = EntityState.Modified;
                    Counter = Counter + 1;
                }
                db.SaveChanges();
                TempData["msg"] = string.Format("swal('Success..','{0} Inspections Approved' ,'success');", Counter);
                return RedirectToAction("BundleInspectionApp");
            }
            TempData["msg"] = string.Format("swal('Ohh..','Inspections not approved' ,'error');");
            return RedirectToAction("BundleInspectionApp");
        }

        //Only Authorize User
        public ActionResult BundleInspectionAppView(string id)
        {
            var BI = (from r in db.VW_BNDL_INSP.Where(x => x.INSPD_NOTE == id)
                      select new BundleInspectionAppD
                      {
                          INSPD_NOTE = r.INSPD_NOTE,
                          INSPH_ID = r.INSPH_ID,
                          ASET_CODE_NAME = r.ASET_CODE_NAME,
                          OWNR_USER = r.OWNR_USER,
                          ITEM_NAME = r.ITEM_NAME,
                          INSPD_IQTY = r.INSPD_IQTY,
                          INSPD_IRATE = r.INSPD_IRATE,
                          INSPH_IUSER = r.INSPH_IUSER,
                          INSPH_DATE = r.INSPH_DATE,
                          INSPH_NOTE = r.INSPH_NOTE,
                          DEPARTMENT_NAME = r.DEPARTMENT_NAME,
                          AREA_NAME = r.AREA_NAME,
                          MILEAGE = r.MILEAGE
                      }).ToList();
            return View(BI);
        }

        [KAuth(406)] //can delete only approver
        public ActionResult BundleInspectionDel(string id)
        {
            if (id == null)
            {
                TempData["msg"] = "swal('Opps..','Inspection not found','warning');";
                return RedirectToAction("Inspections");
            }
            List<string> I = db.VW_BNDL_INSP.Where(s => s.INSPD_NOTE == id && s.INSPH_APP == "N" && s.INSPH_JOB == "N").Select(x => x.INSPH_ID).ToList();
            if (I.Count > 0)
            {
                foreach (var item in I)
                {
                    A_INSPH X = db.A_INSPH.Find(item);
                    db.A_INSPH.Remove(X);
                    List<A_INSPD> D = db.A_INSPD.Where(d => d.INSPH_ID == item).ToList();
                    if (D.Count > 0)
                    {
                        db.A_INSPD.RemoveRange(D); //delete detail
                    }
                    List<A_INSP_JOB_IMG> IJI = db.A_INSP_JOB_IMG.Where(d => d.IMG_REF == item).ToList();
                    if (IJI.Count > 0)
                    {
                        db.A_INSP_JOB_IMG.RemoveRange(IJI); //delete attach file
                    }
                }
                db.SaveChanges();

                // db.A_INSPH.RemoveRange(I); //delete head
                TempData["msg"] = string.Format("swal('Success..','{0} Inspections deleted','success');", I.Count());
                return RedirectToAction("BundleInspectionApp");
            }
            TempData["msg"] = "swal('Opps..','Inspection delete failed','warning');";
            return RedirectToAction("BundleInspectionApp");
        }


        [KAuth(402)] //Same as Job card creator
        public ActionResult BundleJobcard()
        {
            List<BundleInspectionApp> BI = BIA(APP: "Y", JOB: "N");
            return View(BI);
        }
        [KAuth(402)] //Same as Job card creator
        [HttpPost]
        public ActionResult BundleJobcard(BundleJobcard BJC)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            if (ModelState.IsValid)
            {
                DateTime IDATE = DateTime.Now.Date;

                List<BundleJobcardAuto> INSPH_ASET = (from x in db.VW_BNDL_INSP.Where(s => s.INSPD_NOTE == BJC.BUNDLE_ID && s.INSPH_APP == "Y" && s.INSPH_JOB == "N")
                                                      select new BundleJobcardAuto
                                                      {
                                                          INSP_ID = x.INSPH_ID,
                                                          ASET_ID = x.INSPH_ASET
                                                      }
                                                      ).ToList();
                //?? Inspection take Inspection Id and Job Card take Asset Id
                foreach (var item in INSPH_ASET)
                {
                    var IsExistToday = db.A_JOBH.Where(a => a.JOBH_ASET == item.ASET_ID && a.JOBH_DATE == IDATE).ToList();
                    if (IsExistToday.Count > 0)
                    {
                        TempData["msg"] = string.Format("swal('Ohh..','{0} Job Card already created today' ,'info');", IsExistToday.First().JOBH_ID);
                        return RedirectToAction("BundleJobcard");
                    }
                }
                //?? after valid date again take Inspection Id and prepare for Job Card
                foreach (var Insp in INSPH_ASET)
                {
                    A_INSPH IH = db.A_INSPH.Find(Insp.INSP_ID);
                    if (IH == null)
                    {
                        return null;
                    }
                    IH.INSPH_JOB = "Y";
                    IH.UDU = U;
                    IH.UDT = DateTime.Now;
                    IH.VAR = IH.VAR + 1;
                    db.Entry(IH).State = EntityState.Modified;


                    decimal TotalAmount = 0; //get amount from Inspection Details.                                

                    string Insu_Id = "";

                    string GetSeq = Insp.ASET_ID;
                    string id = ge.FifteenDigitId("JOBC", GetSeq, dt: IDATE);
                    A_JOBH JH = new A_JOBH();
                    JH.JOBH_ID = id;
                    JH.INSPH_ID = Insp.INSP_ID; //User Ins Id
                    JH.JOBH_ASET = IH.INSPH_ASET;
                    JH.JOBH_OWNR = IH.INSPH_OWNR;
                    JH.JOBH_DEPARTMENT = IH.INSPH_DEPARTMENT;
                    JH.JOBH_AREA = IH.INSPH_AREA;
                    //JH.JOBH_DATE = IDATE;//User Date
                    JH.JOBH_DATE = IH.INSPH_DATE;
                    JH.JOBH_MILEAGE = IH.MILEAGE;
                    JH.JOBH_VENDOR = BJC.VENDOR_NAME;//User Vendor
                    JH.JOBH_NOTE = BJC.VENDOR_NOTE;//User Note
                    JH.JOBH_TYPE = IH.INSPH_TYPE;
                    JH.JOBH_CNAME = Insu_Id;
                    JH.JOBH_BILL = "N";
                    JH.JOBH_APP = "N";
                    JH.ACT = true;
                    JH.CDT = DateTime.Now;
                    JH.CDU = U;
                    JH.VAR = 1;
                    foreach (var item in IH.A_INSPD) //take data from inspection details
                    {
                        A_JOBD JD = new A_JOBD();
                        JD.JOBH_ID = id;
                        JD.JOBD_LINE = item.INSPD_LINE;
                        JD.JOBD_ITEM = item.INSPD_ITEM;
                        JD.JOBD_IQTY = item.INSPD_IQTY;
                        JD.JOBD_JQTY = item.INSPD_IQTY;
                        JD.JOBD_IRATE = item.INSPD_IRATE;
                        JD.JOBD_JRATE = item.INSPD_IRATE;
                        JD.JOBD_NOTE = BJC.BUNDLE_ID;
                        JD.JOBD_STATE = "B";
                        db.A_JOBD.Add(JD);
                        TotalAmount += item.INSPD_IQTY * item.INSPD_IRATE ?? 1;
                    }
                    if (TotalAmount < 0)
                    {
                        TempData["msg"] = string.Format("swal('Ohh..','Amount can not less then zero' ,'info');");
                        return RedirectToAction("BundleJobcard");
                    }
                    JH.JOBH_IPAY = 0;
                    JH.JOBH_UPAY = 0;
                    JH.JOBH_CPAY = TotalAmount;
                    JH.JOBH_DISC = 0;
                    db.A_JOBH.Add(JH);
                }
                db.SaveChanges();
                TempData["msg"] = string.Format("swal('Success..','{0}  Job Card created' ,'success');", INSPH_ASET.Count());
                return RedirectToAction("BundleJobcard");
            }
            TempData["msg"] = string.Format("swal('Ohh..','Job not created' ,'error');");
            return RedirectToAction("BundleJobcard");
        }

        public List<BundleInspectionApp> BJA(string APP, string BILL, string User, string Type)
        {
            List<string> ASET = RP.MyAssetListString(UserId: User, UserType: Type);

            List<BundleInspectionApp> BI = db.VW_BNDL_JOBC
            .Where(c => c.JOBH_APP == APP && c.JOBH_BILL == BILL && ASET.Contains(c.JOBH_ASET))
            .GroupBy(x => x.JOBD_NOTE)
            .Select(y => new BundleInspectionApp
            {
                BUNDLE_ID = y.Key,
                AMOUNT = y.Sum(x => x.JOBD_IQTY * x.JOBD_IRATE),
                TOTAL_ASET = y.Count()
            }
            ).ToList();
            return BI;
        }

        [KAuth(407)]
        public ActionResult BundleJobCardApp()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            List<BundleInspectionApp> BJ = BJA(APP: "N", BILL: "N", User: U, Type: T);
            return View(BJ);
        }
        [KAuth(407)]
        [HttpPost]
        public ActionResult BundleJobCardApp(string id)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<string> JOB_CARD = db.VW_BNDL_JOBC.Where(x => x.JOBD_NOTE == id).Select(y => y.JOBH_ID).ToList();
            foreach (var item in JOB_CARD)
            {
                A_JOBH I = db.A_JOBH.Find(item);
                if (I == null)
                {
                    return HttpNotFound();
                }
                if (I.ACT == false)
                {
                    TempData["msg"] = string.Format("swal('Ohh..','Job Card Not Submitted : {0}' ,'info');", item);
                    return RedirectToAction("BundleJobCardApp");
                }
                else
                {
                    I.JOBH_APP = "Y";
                    I.JOBH_APBY = U;
                    I.JOBH_APDATE = DateTime.Now;
                    I.UDT = DateTime.Now;
                    I.UDU = U;
                    I.VAR = I.VAR + 1;
                    db.Entry(I).State = EntityState.Modified;

                }
            }
            db.SaveChanges();
            TempData["msg"] = string.Format("swal('Success..','Approved Job Card: {0}' ,'success');", JOB_CARD.Count());
            return RedirectToAction("BundleJobCardApp");
        }

        //Only Authorize User
        public ActionResult BundleJobCardAppView(string id)
        {
            var BI = (from r in db.VW_BNDL_JOBC.Where(x => x.JOBD_NOTE == id)
                      select new BundleJobcardAppD
                      {
                          JOBD_NOTE = r.JOBD_NOTE,
                          JOBH_ID = r.JOBH_ID,
                          ASET_CODE_NAME = r.ASET_CODE_NAME,
                          OWNR_USER = r.OWNR_USER,
                          ITEM_NAME = r.ITEM_NAME,
                          JOBD_IQTY = r.JOBD_IQTY,
                          JOBD_IRATE = r.JOBD_IRATE,
                          JOBH_VENDOR = r.JOBH_VENDOR,
                          JOBH_DATE = r.JOBH_DATE,
                          JOBH_NOTE = r.JOBH_NOTE,
                          DEPARTMENT_NAME = r.DEPARTMENT_NAME,
                          AREA_NAME = r.AREA_NAME,
                          JOBH_MILEAGE = r.JOBH_MILEAGE
                      }).ToList();
            return View(BI);
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

}
