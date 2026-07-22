using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PM.BLL;
using PM.Model;
using PM.ModelsV;
using PM.MyAuth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;


namespace PM.Controllers
{
    [Authorize]
    [CustomHttpReffer]
    public class AssetUserController : Controller
    {
        private PMEntities db = new PMEntities();
        ReturnPermission RP = new ReturnPermission();
        GE Sequence = new GE();
        private PMExcelDataLoader EDL = new PMExcelDataLoader();

        [KAuth(201)]
        public ActionResult Outlet(string Outlet, string Operation)
        {

            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            List<string> Department = RP.MyDeptListString(UserId: U, UserType: T);

            var UserList = from a in db.A_OWNR
                           where a.ACT == true && a.OWNR_RESIGN == "N"
                           && a.OWNR_TYPE == "Retailer"
                           && Department.Contains(a.OWNR_DEPT)
                           select new OutletVM
                           {
                               OWNR_TEXT = a.OWNR_TEXT,
                               OWNR_USER = a.OWNR_USER,
                               OWNR_NAME = a.OWNR_NAME,
                               A_DEPARTMENT = a.A_DEPARTMENT.DEPARTMENT_NAME,//depeartment
                               A_AREA = a.A_AREA.AREA_NAME,//area
                               OWNR_DESG = a.OWNR_DESG,
                               OWNR_CONT = a.OWNR_CONT,
                               OWNR_EMAIL = a.OWNR_EMAIL,
                               OWNR_ADDR = a.OWNR_ADDR,
                               OWNR_NOTE = a.OWNR_NOTE,
                               OWNR_TYPE = a.OWNR_TYPE,
                               OWNR_NDATE = a.OWNR_NDATE,
                               OWNR_POST = a.OWNR_POST
                           };

            if (!string.IsNullOrEmpty(Outlet))
            {
                var R = UserList.Where(a => Outlet.Contains(a.OWNR_TEXT)
                     || Outlet.Contains(a.OWNR_USER)
                     || Outlet.Contains(a.OWNR_NAME))
                    .ToList();
                if (!string.IsNullOrEmpty(Operation))
                {
                    ViewBag.Message = "Search Result";
                }
                return View(R);
            }
            else
            {
                ViewBag.Message = "100 Outlet List";
                var R = UserList.Take(100).ToList();
                return View(R);
            }
        }
        [KAuth(201)]
        public ActionResult NewOutlet()
        {
            //string U = "78278", 
            string T = "10";
            string U = Session["UserId"].ToString();

            List<string> C = RP.MyCountryListString(UserId: U, UserType: T);

            ViewBag.OWNR_AREA = new SelectList(RP.AreaList(C), "AREA_ID", "AREA_NAME");
            ViewBag.OWNR_BUSINESS = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
            return View();
        }
        [KAuth(201)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewOutlet(OutletVM AO)
        {
            //string U = "78278", 
            string T = "10";
            string U = Session["UserId"].ToString();
            if (ModelState.IsValid)
            {
                A_OWNR O = new A_OWNR();
                O.OWNR_DEPT = AO.OWNR_DEPT;
                O.OWNR_AREA = AO.OWNR_AREA;
                O.OWNR_TEXT = AO.OWNR_TEXT ?? AO.OWNR_USER;
                O.OWNR_USER = AO.OWNR_USER;
                O.OWNR_NAME = AO.OWNR_NAME;
                O.OWNR_DESG = AO.OWNR_DESG;
                O.OWNR_CONT = AO.OWNR_CONT;
                O.OWNR_EMAIL = AO.OWNR_EMAIL;
                O.OWNR_ADDR = AO.OWNR_ADDR;
                O.OWNR_NOTE = AO.OWNR_NOTE;
                O.OWNR_TYPE = "Retailer";
                O.OWNR_RESIGN = "N";
                O.OWNR_NDATE = DateTime.Now.Date;
                O.OWNR_POST = false;
                O.ACT = true;
                O.CDT = DateTime.Now;
                O.CDU = U;
                O.VAR = 1;
                //Update Last Number
                //  A_TBL_SEQ seq = db.A_TBL_SEQ.Find(11);
                //seq.TABLE_SEQ = seq.TABLE_SEQ + 1;
                //db.Entry(seq).State = EntityState.Modified;
                //Save Aset Data
                O.OWNR_ID = Sequence.GetSeq(11).ToString();// seq.TABLE_SEQ.ToString() ?? "0";
                db.A_OWNR.Add(O);
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','New outlet opened','success');";
                return RedirectToAction("Outlet");
            }
            List<string> C = RP.MyCountryListString(UserId: U, UserType: T);

            ViewBag.OWNR_AREA = new SelectList(RP.AreaList(C), "AREA_ID", "AREA_NAME", AO.OWNR_AREA);
            ViewBag.OWNR_BUSINESS = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
            //ViewBag.OWNR_DEPT = new SelectList(db.A_DEPARTMENT, "DEPARTMENT_ID", "DEPARTMENT_BUSINESS", AO.OWNR_DEPT);
            TempData["msg"] = "swal('Ohh..','Outlet opened failed','error');";
            return View(AO);
        }


        public List<StaffVM> StaffList()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            List<string> Department = RP.MyDeptListString(UserId: U, UserType: T);
            var UserList = (from a in db.A_OWNR
                            where a.ACT == true && a.OWNR_RESIGN == "N"
                            && a.OWNR_TYPE == "Staff"
                              && Department.Contains(a.OWNR_DEPT)
                            select new StaffVM
                            {
                                OWNR_ID = a.OWNR_ID,
                                OWNR_TEXT = a.OWNR_TEXT,
                                OWNR_USER = a.OWNR_USER,
                                OWNR_NAME = a.OWNR_NAME,
                                A_DEPARTMENT = a.A_DEPARTMENT.DEPARTMENT_NAME,//depeartment
                                A_AREA = a.A_AREA.AREA_NAME,//area
                                OWNR_DESG = a.OWNR_DESG,
                                OWNR_CONT = a.OWNR_CONT,
                                OWNR_EMAIL = a.OWNR_EMAIL,
                                OWNR_ADDR = a.OWNR_ADDR,
                                OWNR_NOTE = a.OWNR_NOTE,
                                OWNR_TYPE = a.OWNR_TYPE,
                                OWNR_NDATE = a.OWNR_NDATE,
                                OWNR_STYPE = a.OWNR_STYPE,
                                //A_STAFF_TYPE = a.A_STAFF_TYPE.STAFF_TYPE_NAME,
                                OWNR_PASSPORT_NUMBER = a.OWNR_PASSPORT_NUMBER,
                                OWNR_PASSPORT_EDATE = a.OWNR_PASSPORT_EDATE,
                                OWNR_POST = a.OWNR_POST,
                                DEPARTMENT_V = a.A_DEPARTMENT.DEPARTMENT_NAME,
                                AREA_V = a.A_AREA.AREA_NAME,
                                BUSINESS_V = a.A_DEPARTMENT.A_BUSINESS.BUSINESS_NAME,
                                COMPANY_V = a.A_DEPARTMENT.A_BUSINESS.A_COMPANY.COMPANY_NAME
                            }).ToList();
            return UserList;
        }

        public List<StaffVM> HealthCardStaffList()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            List<string> Department = RP.MyDeptListString(UserId: U, UserType: T);
            var UserList = (from a in db.A_OWNR
                            where a.ACT == true && a.OWNR_RESIGN == "N"
                            && a.OWNR_TYPE == "Staff" && a.OWNR_STYPE == "2"
                              && Department.Contains(a.OWNR_DEPT)
                            select new StaffVM
                            {
                                OWNR_ID = a.OWNR_ID,
                                OWNR_TEXT = a.OWNR_TEXT,
                                OWNR_USER = a.OWNR_USER,
                                OWNR_NAME = a.OWNR_NAME,
                                A_DEPARTMENT = a.A_DEPARTMENT.DEPARTMENT_NAME,//depeartment
                                A_AREA = a.A_AREA.AREA_NAME,//area
                                OWNR_DESG = a.OWNR_DESG,
                                OWNR_CONT = a.OWNR_CONT,
                                OWNR_EMAIL = a.OWNR_EMAIL,
                                OWNR_ADDR = a.OWNR_ADDR,
                                OWNR_NOTE = a.OWNR_NOTE,
                                OWNR_TYPE = a.OWNR_TYPE,
                                OWNR_NDATE = a.OWNR_NDATE,
                                OWNR_PASSPORT_NUMBER = a.OWNR_PASSPORT_NUMBER,
                                OWNR_PASSPORT_EDATE = a.OWNR_PASSPORT_EDATE,
                                OWNR_HEALTH_CARD = a.OWNR_HEALTH_CARD,
                                OWNR_HEALTH_CARD_EDATE = a.OWNR_HEALTH_CARD_EDATE,
                                OWNR_POST = a.OWNR_POST,
                                DEPARTMENT_V = a.A_DEPARTMENT.DEPARTMENT_NAME,
                                AREA_V = a.A_AREA.AREA_NAME,
                                BUSINESS_V = a.A_DEPARTMENT.A_BUSINESS.BUSINESS_NAME,
                                COMPANY_V = a.A_DEPARTMENT.A_BUSINESS.A_COMPANY.COMPANY_NAME
                            }).ToList();
            return UserList;
        }

        [KAuth(202)]
        public ActionResult Staff(string StaffId, string Operation)
        {
            List<StaffVM> UserList = StaffList();
            if (!string.IsNullOrEmpty(StaffId))
            {
                var R = UserList.Where(a => StaffId.Contains(a.OWNR_TEXT)
                     || StaffId.Contains(a.OWNR_USER)
                     || StaffId.Contains(a.OWNR_NAME))
                    .ToList();
                if (!string.IsNullOrEmpty(Operation))
                {
                    ViewBag.Message = "Search Result";
                }
                return View(R);
            }
            else
            {
                ViewBag.Message = "100 Staff List";
                var R = UserList.Take(100).ToList();
                return View(R);
            }
        }

        #region old API
        //public ActionResult staffChecker(string staff)

        //{
        //    if (staff == "")
        //    {
        //        //TempData["msg"] = "swal('Opps..','Inspection not found','warning');";
        //        return RedirectToAction("Staff");
        //    }
        //    else
        //    {
        //        try
        //        {

        //            var res_dict = new Dictionary<string, string>();
        //            string apiUrl = "http://hris.prangroup.com:8686/api/hrisapi.svc/Staff/" + staff;
        //            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
        //            request.Method = "GET";
        //            request.ContentType = "application/json";
        //            request.ContentLength = 0;
        //            WebResponse webResponse = request.GetResponse();
        //            string response;
        //            using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
        //            using (StreamReader responseReader = new StreamReader(webStream))
        //            {
        //                response = responseReader.ReadToEnd();
        //            }
        //            var ParseOrderDate = JObject.Parse(response);
        //            var jsonStringOrderData = ParseOrderDate["StaffResult"];

        //            JavaScriptSerializer serializer = new JavaScriptSerializer();
        //            serializer.MaxJsonLength = Int32.MaxValue;
        //            List<ApiUserInfo> UserList =
        //                (serializer).Deserialize<List<ApiUserInfo>>(jsonStringOrderData.ToString());
        //            //HRIS Update Data

        //            if (UserList == null)
        //            {
        //                TempData["msg"] = "User Not Found in HRIS";
        //                return Json(new { STATUS = 3 }, JsonRequestBehavior.AllowGet);
        //            }
        //            if (UserList.Count == 0)
        //            {
        //                TempData["msg"] = "User Not Found in HRIS";
        //                return Json(new { STATUS = 3 }, JsonRequestBehavior.AllowGet);
        //            }
        //            else
        //            {
        //                ApiUserInfo user = UserList.FirstOrDefault();

        //                return Json(new { STATUS = 1, OWNR_USER = user.NAME, OWNR_DESG = user.DESIGNATION, OWNR_CONT = user.CONTACTNO, OWNR_EMAIL = user.EMAIL, OWNR_ADDR = user.LOCATIONNAME }, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //        catch (Exception)
        //        {

        //            TempData["msg"] = "User Not Found in HRIS";
        //            return Json(new { STATUS = 2 }, JsonRequestBehavior.AllowGet);
        //        }
        //    }

        //    //return Json(null, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        public ActionResult staffChecker(string staff)
        {
            string baseUrl = "http://hris.prangroup.com:8696/StaffInformation/getStaffPersonalInfo?staffId=";

            string url = baseUrl + staff;

            using (WebClient client = new WebClient())
            {
                try
                {

                    byte[] responseBytes = client.UploadData(url, "POST", new byte[0]);


                    string response = Encoding.UTF8.GetString(responseBytes);

                    var responseData = JsonConvert.DeserializeObject<StaffResponse>(response);

                    if (responseData.Status == "warning")
                    {
                        TempData["msg"] = "User Not Found in HRIS";
                        return Json(new { STATUS = 3 }, JsonRequestBehavior.AllowGet);
                    }

                    if (responseData != null)
                    {
                        var responseContent = responseData;

                            return Json(new
                            {
                                STATUS = 1,
                                OWNR_USER = responseContent.StaffPersonalDetails[0].EmployeeName,
                                OWNR_DESG = responseContent.StaffPersonalDetails[0].Designation,
                                OWNR_CONT = responseContent.StaffPersonalDetails[0].ContactNo,
                                OWNR_EMAIL = responseContent.StaffPersonalDetails[0].Email,
                                OWNR_ADDR = responseContent.StaffPersonalDetails[0].LocationName
                            },
                                            JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        TempData["msg"] = "User Not Found in HRIS";
                        return Json(new { STATUS = 3 }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (WebException ex)
                {
                    return Json(new { success = false, error = "An error occurred: " + ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public class StaffResponse
        {
            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }

            [JsonProperty("staff_personal_details")]
            public List<StaffPersonalDetail> StaffPersonalDetails { get; set; }
        }

        public class StaffPersonalDetail
        {
            [JsonProperty("staff_id")]
            public string StaffId { get; set; }

            [JsonProperty("employee_name")]
            public string EmployeeName { get; set; }

            [JsonProperty("tin")]
            public string Tin { get; set; }

            [JsonProperty("religion_id")]
            public string ReligionId { get; set; }

            [JsonProperty("gender_id")]
            public string GenderId { get; set; }

            [JsonProperty("nationality_id")]
            public string NationalityId { get; set; }

            [JsonProperty("marital_status")]
            public string MaritalStatus { get; set; }

            [JsonProperty("nid")]
            public string NID { get; set; }

            [JsonProperty("birth_certificate")]
            public string BirthCertificate { get; set; }

            [JsonProperty("blood_group")]
            public string BloodGroup { get; set; }

            [JsonProperty("date_of_birth")]
            public string DateOfBirth { get; set; }

            [JsonProperty("passportid")]
            public string PassportId { get; set; }

            [JsonProperty("spouse_name")]
            public string SpouseName { get; set; }

            [JsonProperty("spouse_mobile")]
            public string SpouseMobile { get; set; }

            [JsonProperty("spouseprof_id")]
            public string SpouseProfId { get; set; }

            [JsonProperty("contactno")]
            public string ContactNo { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("company")]
            public string Company { get; set; }

            [JsonProperty("groupname")]
            public string GroupName { get; set; }

            [JsonProperty("email")]
            public string Email { get; set; }

            [JsonProperty("department")]
            public string Department { get; set; }

            [JsonProperty("locationname")]
            public string LocationName { get; set; }

            [JsonProperty("designation")]
            public string Designation { get; set; }
        }

        public class ApiUserInfo
        {
            public string ID { get; set; }
            public string NAME { get; set; }
            public string CONTACTNO { get; set; }
            public string EMAIL { get; set; }
            public string GROUPNAME { get; set; }
            public string COMPANY { get; set; }
            public string DEPARTMENT { get; set; }
            public string LOCATIONNAME { get; set; }
            public string DESIGNATION { get; set; }
            public string STATUS { get; set; }
            public string GENDER { get; set; }
        }


        [KAuth(202)]
        public ActionResult NewStaff()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            List<string> C = RP.MyCountryListString(UserId: U, UserType: T);

            ViewBag.OWNR_AREA = new SelectList(RP.AreaList(C), "AREA_ID", "AREA_NAME");

            ViewBag.OWNR_STYPE = new SelectList(RP.StaffTypeList(), "STAFF_TYPE_ID", "STAFF_TYPE_NAME");

            ViewBag.OWNR_BUSINESS = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
            return View();
        }
        [KAuth(202)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewStaff(StaffVM AS)
        {
            //string U = "78278", 
            string T = "10";
            string U = Session["UserId"].ToString();
            if (ModelState.IsValid)
            {
                var IsVal = CheckOwnrTextExist(AS.OWNR_TEXT.ToUpper());
                if ((bool)IsVal.Data == true)
                {
                    A_OWNR O = new A_OWNR();
                    O.OWNR_DEPT = AS.OWNR_DEPT;
                    O.OWNR_AREA = AS.OWNR_AREA;
                    O.OWNR_TEXT = AS.OWNR_TEXT.ToUpper();
                    O.OWNR_USER = AS.OWNR_USER;
                    O.OWNR_NAME = AS.OWNR_NAME;
                    O.OWNR_DESG = AS.OWNR_DESG;
                    O.OWNR_CONT = AS.OWNR_CONT;
                    O.OWNR_EMAIL = AS.OWNR_EMAIL;
                    O.OWNR_ADDR = AS.OWNR_ADDR;
                    O.OWNR_NOTE = AS.OWNR_NOTE;
                    O.OWNR_TYPE = "Staff";
                    O.OWNR_RESIGN = "N";
                    O.OWNR_NDATE = AS.OWNR_NDATE;
                    O.OWNR_PASSPORT_NUMBER = AS.OWNR_PASSPORT_NUMBER;
                    O.OWNR_PASSPORT_EDATE = AS.OWNR_PASSPORT_EDATE;
                    O.OWNR_STYPE = AS.OWNR_STYPE;
                    O.REMIND = 0;
                    O.OWNR_POST = false;
                    O.ACT = true;
                    O.CDT = DateTime.Now;
                    O.CDU = U;
                    O.VAR = 1;
                    O.OWNR_ID = Sequence.GetSeq(11).ToString();
                    db.A_OWNR.Add(O);
                    db.SaveChanges();
                    TempData["msg"] = "swal('Success..','New staff opened','success');";
                    return RedirectToAction("Staff");

                }
            }
            List<string> C = RP.MyCountryListString(UserId: U, UserType: T);

            ViewBag.OWNR_AREA = new SelectList(RP.AreaList(C), "AREA_ID", "AREA_NAME", AS.OWNR_AREA);
            ViewBag.OWNR_BUSINESS = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
            //ViewBag.OWNR_DEPT = new SelectList(db.A_DEPARTMENT, "DEPARTMENT_ID", "DEPARTMENT_BUSINESS", AO.OWNR_DEPT);
            TempData["msg"] = "swal('Ohh..','Staff opened failed','error');";
            return View(AS);
        }
        [KAuth(202)]
        [HttpGet]
        public ActionResult StaffEdit(string id)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            List<string> C = RP.MyCountryListString(UserId: U, UserType: T);

            if (id == null)
            {
                TempData["msg"] = "swal('Opss..','Try with valid user','info');";
                return RedirectToAction("Staff");
            }
            A_OWNR ao = db.A_OWNR.Find(id);
            if (ao == null)
            {
                TempData["msg"] = "swal('Ohh..','No user found','warning');";
                return RedirectToAction("Staff");
            }
            StaffEdit SE = new StaffEdit();
            SE.OWNR_ID = ao.OWNR_ID;
            SE.OWNR_DESG = ao.OWNR_DESG;
            SE.OWNR_CONT = ao.OWNR_CONT;
            SE.OWNR_EMAIL = ao.OWNR_EMAIL;
            SE.OWNR_ADDR = ao.OWNR_ADDR;
            SE.OWNR_NOTE = ao.OWNR_NOTE;
            ViewBag.OWNR_BUSINESS = new SelectList(RP.MyBusiness(UserId: U), "BUSINESS_ID", "BUSINESS_NAME");
            ViewBag.OWNR_DEPT = new SelectList(RP.DepartmentById(ao.OWNR_DEPT), "DEPARTMENT_ID", "DEPARTMENT_NAME");
            ViewBag.OWNR_AREA = new SelectList(RP.AreaList(C), "AREA_ID", "AREA_NAME", ao.OWNR_AREA);
            ViewBag.OWNR_STYPE = new SelectList(RP.StaffTypeList(), "STAFF_TYPE_ID", "STAFF_TYPE_NAME", ao.OWNR_STYPE);
            return PartialView("_StaffEdit", SE);
        }
        [KAuth(202)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StaffEdit(StaffEdit SE)
        {
            if (ModelState.IsValid)
            {
                string U = Session["UserId"].ToString();
                A_OWNR ao = db.A_OWNR.Find(SE.OWNR_ID);
                ao.OWNR_DESG = SE.OWNR_DESG;
                ao.OWNR_CONT = SE.OWNR_CONT;
                ao.OWNR_EMAIL = SE.OWNR_EMAIL;
                ao.OWNR_ADDR = SE.OWNR_ADDR;
                ao.OWNR_NOTE = SE.OWNR_NOTE;
                ao.OWNR_DEPT = SE.OWNR_DEPT;
                ao.OWNR_AREA = SE.OWNR_AREA;
                ao.OWNR_STYPE = SE.OWNR_STYPE;
                ao.UDT = DateTime.Now;
                ao.UDU = U;
                ao.VAR = ao.VAR + 1;
                db.Entry(ao).State = EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Staff data updated','success');";
                return RedirectToAction("Staff", new { StaffId = SE.OWNR_ID });
            }
            TempData["msg"] = "swal('Ohh..','Staff data not updated','warning');";
            return RedirectToAction("Staff", new { StaffId = SE.OWNR_ID });
        }


        [AllowAnonymous]
        public JsonResult CheckOwnrTextExist(string OWNR_TEXT)
        {
            string Sql = string.Format(@"select ownr_text from a_ownr where ownr_text='{0}'", OWNR_TEXT);
            var isExist = db.Database.SqlQuery<string>(Sql).ToList();
            if (isExist.Count > 0)
            {
                return Json("User Already Exists!", JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }



        [KAuth(204)]
        public ActionResult StaffIqumaList(string StaffId, string Operation)
        {
            List<StaffVM> UserList = StaffList();
            if (!string.IsNullOrEmpty(StaffId))
            {
                var R = UserList.Where(a => StaffId.Contains(a.OWNR_TEXT)
                     || StaffId.Contains(a.OWNR_USER)
                     || StaffId.Contains(a.OWNR_NAME))
                    .ToList();
                if (!string.IsNullOrEmpty(Operation))
                {
                    ViewBag.Message = "Search Result";
                }
                return View(R);
            }
            else
            {
                ViewBag.Message = "Staff List";
                var R = UserList.ToList();
                return View(R);
            }
        }
        [KAuth(204)]
        [HttpGet]
        public ActionResult StaffIquma(string id)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (id == null)
            {
                TempData["msg"] = "swal('Opss..','Try with valid user','info');";
                return RedirectToAction("StaffIquma");
            }
            A_OWNR ao = db.A_OWNR.Find(id);
            if (ao == null)
            {
                TempData["msg"] = "swal('Ohh..','No user found','warning');";
                return RedirectToAction("StaffIquma");
            }
            StaffIquma SI = new StaffIquma();
            SI.OWNR_ID = ao.OWNR_ID;
            SI.OWNR_NAME = ao.OWNR_NAME;
            SI.OWNR_NDATE = ao.OWNR_NDATE;
            return PartialView("_StaffIquma", SI);
        }
        [KAuth(204)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StaffIquma(StaffIquma SI)
        {
            if (ModelState.IsValid)
            {
                string U = Session["UserId"].ToString();
                A_OWNR ao = db.A_OWNR.Find(SI.OWNR_ID);
                ao.OWNR_NAME = SI.OWNR_NAME;
                ao.OWNR_NDATE = SI.OWNR_NDATE;
                ao.REMIND = 0;
                ao.UDT = DateTime.Now;
                ao.UDU = U;
                ao.VAR = ao.VAR + 1;
                db.Entry(ao).State = EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Staff Iquma updated','success');";
                return RedirectToAction("StaffIquma", new { StaffId = SI.OWNR_ID });
            }
            TempData["msg"] = "swal('Ohh..','Staff Iquma not updated','warning');";
            return RedirectToAction("StaffIquma", new { StaffId = SI.OWNR_ID });
        }

        //Passport Expire Update

        [KAuth(204)]
        public ActionResult StaffPassportList(string StaffId, string Operation)
        {
            List<StaffVM> UserList = StaffList();
            if (!string.IsNullOrEmpty(StaffId))
            {
                var R = UserList.Where(a => StaffId.Contains(a.OWNR_TEXT)
                     || StaffId.Contains(a.OWNR_USER)
                     || StaffId.Contains(a.OWNR_NAME))
                    .ToList();
                if (!string.IsNullOrEmpty(Operation))
                {
                    ViewBag.Message = "Search Result";
                }
                return View(R);
            }
            else
            {
                ViewBag.Message = "Staff List";
                var R = UserList.ToList();
                return View(R);
            }
        }

        [KAuth(204)]
        [HttpGet]
        public ActionResult StaffPassport(string id)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (id == null)
            {
                TempData["msg"] = "swal('Opss..','Try with valid user','info');";
                return RedirectToAction("StaffPassport");
            }
            A_OWNR ao = db.A_OWNR.Find(id);
            if (ao == null)
            {
                TempData["msg"] = "swal('Ohh..','No user found','warning');";
                return RedirectToAction("StaffPassport");
            }
            StaffPassport SI = new StaffPassport();
            SI.OWNR_ID = ao.OWNR_ID;
            SI.OWNR_PASSPORT_NUMBER = ao.OWNR_PASSPORT_NUMBER;
            SI.OWNR_PASSPORT_EDATE = ao.OWNR_PASSPORT_EDATE;
            return PartialView("_StaffPassport", SI);
        }

        [KAuth(204)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StaffPassport(StaffPassport SI)
        {
            if (ModelState.IsValid)
            {
                string U = Session["UserId"].ToString();
                A_OWNR ao = db.A_OWNR.Find(SI.OWNR_ID);
                ao.OWNR_PASSPORT_NUMBER = SI.OWNR_PASSPORT_NUMBER;
                ao.OWNR_PASSPORT_EDATE = SI.OWNR_PASSPORT_EDATE;
                //ao.REMIND = 0;
                ao.UDT = DateTime.Now;
                ao.UDU = U;
                ao.VAR = ao.VAR + 1;
                db.Entry(ao).State = EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Staff Passport updated','success');";
                return RedirectToAction("StaffPassportList");
            }
            TempData["msg"] = "swal('Ohh..','Staff Passport not updated','warning');";
            return RedirectToAction("StaffPassport", new { StaffId = SI.OWNR_ID });
        }

        //Passport Expire Update End

        //Health Card Expire Update

        [KAuth(204)]
        public ActionResult StaffHealthCardList(string StaffId, string Operation)
        {
            List<StaffVM> UserList = HealthCardStaffList();
            if (!string.IsNullOrEmpty(StaffId))
            {
                var R = UserList.Where(a => StaffId.Contains(a.OWNR_TEXT)
                     || StaffId.Contains(a.OWNR_USER)
                     || StaffId.Contains(a.OWNR_NAME))
                    .ToList();
                if (!string.IsNullOrEmpty(Operation))
                {
                    ViewBag.Message = "Search Result";
                }
                return View(R);
            }
            else
            {
                ViewBag.Message = "Staff List";
                var R = UserList.ToList();
                return View(R);
            }
        }

        [KAuth(204)]
        [HttpGet]
        public ActionResult StaffHealthCard(string id)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (id == null)
            {
                TempData["msg"] = "swal('Opss..','Try with valid user','info');";
                return RedirectToAction("StaffHealthCard");
            }
            A_OWNR ao = db.A_OWNR.Find(id);
            if (ao == null)
            {
                TempData["msg"] = "swal('Ohh..','No user found','warning');";
                return RedirectToAction("StaffHealthCard");
            }
            StaffHealthCard SI = new StaffHealthCard();
            SI.OWNR_ID = ao.OWNR_ID;
            SI.OWNR_HEALTH_CARD = ao.OWNR_HEALTH_CARD;
            SI.OWNR_HEALTH_CARD_EDATE = ao.OWNR_HEALTH_CARD_EDATE;
            return PartialView("_StaffHealthCard", SI);
        }

        [KAuth(204)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StaffHealthCard(StaffHealthCard SI)
        {
            if (ModelState.IsValid)
            {
                string U = Session["UserId"].ToString();
                A_OWNR ao = db.A_OWNR.Find(SI.OWNR_ID);
                ao.OWNR_HEALTH_CARD = SI.OWNR_HEALTH_CARD;
                ao.OWNR_HEALTH_CARD_EDATE = SI.OWNR_HEALTH_CARD_EDATE;
                //ao.REMIND = 0;
                ao.UDT = DateTime.Now;
                ao.UDU = U;
                ao.VAR = ao.VAR + 1;
                db.Entry(ao).State = EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Staff Health Card updated','success');";
                return RedirectToAction("StaffHealthCardList");
            }
            TempData["msg"] = "swal('Ohh..','Staff Health Card not updated','warning');";
            return RedirectToAction("StaffHealthCard", new { StaffId = SI.OWNR_ID });
        }

        //Health Card Expire Update End

        [KAuth(203)]
        public ActionResult OutletExcel()
        {
            var model = new ExcelInputVM();
            return View(model);
        }
        [KAuth(203)]
        [HttpPost]
        public ActionResult OutletExcel(ExcelInputVM model)
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
            DataTable dt = EDL.OutletExcelData(model.MyExcelFile.InputStream, false);
            string strContent = EDL.ConvertDataTableToHTMLTable(dt);
            model.MSExcelTable = strContent;
            TempData["msg"] = "swal('Success..','Excel fill loaded for checking','success');";
            return View(model);
        }
        [KAuth(203)]
        public ActionResult SaveOutletExcel(OutletExcel[] ExlData)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            string result = "Outlet data is not complete!";
            bool IsValidExcelData = true; //start condition as failed
            if (ModelState.IsValid)
            {
                //Check Excel Data Validation
                List<string> MyDepartments = RP.MyDeptListString(UserId: U, UserType: T);
                List<string> MyArea = RP.MyAreaListString(UserId: U, UserType: T);

                foreach (var item in ExlData)
                {
                    if (IsValidExcelData)
                    {
                        #region Check_Data_Validation
                        //   bool IsValidName = Sequence.IsExist_OUTLET_NAME(item.OWNR_USER.Trim()); //23-09-2020
                        bool IsValidName = Sequence.IsExist_OUTLET_NAME(item.OWNR_TEXT.Trim());
                        if (IsValidName)
                        {
                            result = string.Format("{0} :Name is already in used, try another Name", item.OWNR_USER);
                            IsValidExcelData = false;
                        }
                        if (IsValidExcelData)
                        {
                            bool IsValidDepartment = MyDepartments.Contains(item.OWNR_DEPT);
                            if (!IsValidDepartment)
                            {
                                result = string.Format("{0} :Department Code {1} is Invalid, try with Valid Department", item.OWNR_USER, item.OWNR_DEPT);
                                IsValidExcelData = false;
                            }
                        }
                        if (IsValidExcelData)
                        {
                            bool IsValidArea = MyArea.Contains(item.OWNR_AREA);
                            if (!IsValidArea)
                            {
                                result = string.Format("{0} :Area Code {1} is Invalid, try with Valid Area", item.OWNR_USER, item.OWNR_AREA);
                                IsValidExcelData = false;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (IsValidExcelData)
                {
                    #region Start_Insertion ///If Valid start Insert to database
                    int Count = 0;
                    foreach (var item in ExlData)
                    {
                        A_OWNR O = new A_OWNR();
                        O.OWNR_DEPT = item.OWNR_DEPT;
                        O.OWNR_AREA = item.OWNR_AREA;
                        O.OWNR_TEXT = item.OWNR_TEXT.Trim();
                        O.OWNR_USER = item.OWNR_USER.Trim();
                        O.OWNR_NAME = item.OWNR_NAME.Trim();
                        O.OWNR_DESG = "";
                        O.OWNR_CONT = item.OWNR_CONT;
                        O.OWNR_EMAIL = "";
                        O.OWNR_ADDR = item.OWNR_ADDR;
                        O.OWNR_NOTE = item.OWNR_NOTE;
                        O.OWNR_TYPE = "Retailer";
                        O.OWNR_RESIGN = "N";
                        O.OWNR_NDATE = DateTime.Now.Date;
                        O.OWNR_POST = false;
                        O.ACT = true;
                        O.CDT = DateTime.Now;
                        O.CDU = U;
                        O.VAR = 1;
                        //Save Aset Data
                        O.OWNR_ID = Sequence.GetSeq(11).ToString();
                        db.A_OWNR.Add(O);
                        Count = Count + 1;
                    }
                    db.SaveChanges();
                    result = string.Format("{0} :New Outlet Saved", Count);
                    return Json(result, JsonRequestBehavior.AllowGet);
                    #endregion
                }
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                          .Where(y => y.Count > 0)
                          .ToList();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
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
