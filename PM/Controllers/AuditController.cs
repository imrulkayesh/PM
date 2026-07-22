using Newtonsoft.Json;
using OfficeOpenXml;
using Oracle.ManagedDataAccess.Client;
using PM.BLL;
using PM.Model;
using PM.ModelsV;
using PM.MyAuth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace PM.Controllers
{
    [Authorize]
    [CustomHttpReffer]
    public class AuditController : Controller
    {
        private PMEntities db = new PMEntities();
        ReturnPermission RP = new ReturnPermission();
        GE Sequence = new GE();
        private JsonReaderPMController PMC = new JsonReaderPMController();

        [KAuth(5001)]
        public ActionResult Index()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            string sql_1 = @"SELECT a.AUDITOR_ID, a.AUDITOR_STAFF_ID, a.AUDITOR_NAME, a.AUDITOR_MOBILE, a.AUDITOR_EMAIL, a.ACT FROM AUDIT_AUDITOR a ";
            var AuditorData = db.Database.SqlQuery<AuditorVM>(sql: sql_1);
            ViewBag.Message = "Auditor List";
            var R = AuditorData.Take(500).ToList();
            return View(R);
        }

        [KAuth(5001)]
        [HttpGet]
        public ActionResult NewAuditor()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            return View();
        }

        [KAuth(5001)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewAuditor(AuditorVM ADTVM)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (ModelState.IsValid)
            {
                AUDIT_AUDITOR ADT = new AUDIT_AUDITOR();
                ADT.AUDITOR_NAME = ADTVM.AUDITOR_NAME;
                ADT.AUDITOR_STAFF_ID = ADTVM.AUDITOR_STAFF_ID;
                ADT.AUDITOR_MOBILE = ADTVM.AUDITOR_MOBILE;
                ADT.AUDITOR_EMAIL = ADTVM.AUDITOR_EMAIL;
                ADT.ACT = true;
                ADT.CDT = DateTime.Now;
                ADT.CDU = U;
                ADT.AUDITOR_ID = Sequence.GetSeq(29).ToString();
                db.AUDIT_AUDITOR.Add(ADT);
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Saved succefully','success');";
                return RedirectToAction("NewAuditor", new { AuditorID = ADT.AUDITOR_ID });
            }
            TempData["msg"] = "swal('Ohh..','Saved failed','error');";
            return View();
        }

        [KAuth(5001)]
        [HttpGet]
        public ActionResult EditAuditor(string id)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            var auditor = db.AUDIT_AUDITOR
                            .Where(x => x.AUDITOR_ID == id)
                            .FirstOrDefault();

            if (auditor == null)
            {
                return HttpNotFound();
            }

            AuditorVM vm = new AuditorVM()
            {
                AUDITOR_ID = auditor.AUDITOR_ID,
                AUDITOR_NAME = auditor.AUDITOR_NAME,
                AUDITOR_STAFF_ID = auditor.AUDITOR_STAFF_ID,
                AUDITOR_MOBILE = auditor.AUDITOR_MOBILE,
                AUDITOR_EMAIL = auditor.AUDITOR_EMAIL,
                ACT = auditor.ACT
            };

            return View(vm);
        }

        [KAuth(5001)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAuditor(AuditorVM vm)
        {
            string U = Session["UserId"].ToString();

            if (ModelState.IsValid)
            {
                var auditor = db.AUDIT_AUDITOR
                                .Where(x => x.AUDITOR_ID == vm.AUDITOR_ID)
                                .FirstOrDefault();

                if (auditor == null)
                {
                    TempData["msg"] = "swal('Error','Record not found','error');";
                    return RedirectToAction("Index");
                }

                auditor.AUDITOR_NAME = vm.AUDITOR_NAME;
                auditor.AUDITOR_STAFF_ID = vm.AUDITOR_STAFF_ID;
                auditor.AUDITOR_MOBILE = vm.AUDITOR_MOBILE;
                auditor.AUDITOR_EMAIL = vm.AUDITOR_EMAIL;
                auditor.ACT = vm.ACT;

                // Optional Audit Fields
                auditor.UDT = DateTime.Now;
                auditor.UDU = U;

                db.SaveChanges();

                TempData["msg"] = "swal('Success','Updated Successfully','success');";

                return RedirectToAction("Index");
            }

            TempData["msg"] = "swal('Error','Update Failed','error');";
            return View(vm);
        }

        [KAuth(5002)]
        [HttpGet]
        public ActionResult NewTask()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            ViewBag.LOCATION_ID = new SelectList(RP.C_LocationList(), "LOCATION_ID", "LOCATION_NAME");
            ViewBag.COMPANY_GROUP_ID = new SelectList(RP.CompanyGroupList(), "COMPANY_GROUP_ID", "COMPANY_GROUP_NAME");
            ViewBag.DEPARTMENT_ID = new SelectList(RP.C_DepartmentList(), "DEPARTMENT_ID", "DEPARTMENT_NAME");
            ViewBag.TASK_COMMUNICATION_BY_ID = new SelectList(RP.TaskCommunicationByList(), "TASK_COMMUNICATION_BY_ID", "TASK_COMMUNICATION_BY_NAME");
            ViewBag.TASK_ASSIGN_BY_ID = new SelectList(RP.TaskAssignByList(), "TASK_ASSIGN_BY_ID", "TASK_ASSIGN_BY_NAME");
            ViewBag.MANAGEMENT_SOURCE_ID = new SelectList(RP.ManagementSourceList(), "MANAGEMENT_SOURCE_ID", "MANAGEMENT_SOURCE_NAME");
            ViewBag.TASK_TYPE_ID = new SelectList(RP.TaskTypeList(), "TASK_TYPE_ID", "TASK_TYPE_NAME");
            ViewBag.TASK_PRIORITY_ID = new SelectList(RP.TaskPriorityList(), "TASK_PRIORITY_ID", "TASK_PRIORITY_NAME");
            ViewBag.REPORT_STATUS_ID = new SelectList(RP.ReportStatusList(), "REPORT_STATUS_ID", "REPORT_STATUS_NAME");

            ViewBag.AUDITOR_ID = new SelectList(RP.AuditorList(), "AUDITOR_ID", "AUDITOR_NAME");
            ViewBag.TASK_STATUS = new SelectList(RP.TaskStatusList(), "TASK_STATUS_ID", "TASK_STATUS_NAME");

            return View();
        }
       
        [KAuth(5002)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewTask(NewTaskVM TVM)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            if (TVM.FileX != null && TVM.FileX.ContentLength > 0)
            {

                int fileSize = TVM.FileX.ContentLength;
                if (fileSize <= 0 || fileSize > 21000000)
                {
                    TempData["msg"] = "swal('Opps..','File is too larger, Max file size 20MB','warning');";
                    ViewBag.LOCATION_ID = new SelectList(RP.C_LocationList(), "LOCATION_ID", "LOCATION_NAME");
                    ViewBag.COMPANY_GROUP_ID = new SelectList(RP.CompanyGroupList(), "COMPANY_GROUP_ID", "COMPANY_GROUP_NAME");
                    ViewBag.DEPARTMENT_ID = new SelectList(RP.C_DepartmentList(), "DEPARTMENT_ID", "DEPARTMENT_NAME");
                    ViewBag.TASK_COMMUNICATION_BY_ID = new SelectList(RP.TaskCommunicationByList(), "TASK_COMMUNICATION_BY_ID", "TASK_COMMUNICATION_BY_NAME");
                    ViewBag.TASK_ASSIGN_BY_ID = new SelectList(RP.TaskAssignByList(), "TASK_ASSIGN_BY_ID", "TASK_ASSIGN_BY_NAME");
                    ViewBag.MANAGEMENT_SOURCE_ID = new SelectList(RP.ManagementSourceList(), "MANAGEMENT_SOURCE_ID", "MANAGEMENT_SOURCE_NAME");
                    ViewBag.TASK_TYPE_ID = new SelectList(RP.TaskTypeList(), "TASK_TYPE_ID", "TASK_TYPE_NAME");
                    ViewBag.TASK_PRIORITY_ID = new SelectList(RP.TaskPriorityList(), "TASK_PRIORITY_ID", "TASK_PRIORITY_NAME");
                    ViewBag.REPORT_STATUS_ID = new SelectList(RP.ReportStatusList(), "REPORT_STATUS_ID", "REPORT_STATUS_NAME");
                    ViewBag.AUDITOR_ID = new SelectList(RP.AuditorList(), "AUDITOR_ID", "AUDITOR_NAME");
                    ViewBag.TASK_STATUS = new SelectList(RP.TaskStatusList(), "TASK_STATUS_ID", "TASK_STATUS_NAME");
                    return View();
                }
            }

            if (TVM.TASK_STATUS == "5" && (TVM.REPORT_STATUS_ID == "" || TVM.REPORT_STATUS_ID == null))
            {
                TempData["msg"] = "swal('Opps..','Please select Report Status','warning');";
                return View();
            }


            if (ModelState.IsValid)
            {
                AUDIT_TASK AT = new AUDIT_TASK();

                AT.LOCATION_ID = TVM.LOCATION_ID;
                AT.COMPANY_GROUP_ID = TVM.COMPANY_GROUP_ID;
                AT.COMPANY_ID = TVM.COMPANY_ID;
                AT.DEPARTMENT_ID = TVM.DEPARTMENT_ID;
                AT.TASK_COMMUNICATION_BY_ID = TVM.TASK_COMMUNICATION_BY_ID;
                AT.TASK_ASSIGN_BY_ID = TVM.TASK_ASSIGN_BY_ID;
                AT.MANAGEMENT_SOURCE_ID = TVM.MANAGEMENT_SOURCE_ID;
                AT.TASK_PRIORITY_ID = TVM.TASK_PRIORITY_ID;
                AT.TASK_DETAILS = TVM.TASK_DETAILS;
                AT.TASK_ASSIGN_DATE = TVM.TASK_ASSIGN_DATE;

                AT.TASK_START_PLANED_DATE = TVM.TASK_START_PLANED_DATE;
                AT.TASK_DEADLINE = TVM.TASK_DEADLINE;
                AT.TASK_START_DATE = TVM.TASK_START_DATE;
                AT.TASK_STATUS = TVM.TASK_STATUS;
                AT.REMARKS = TVM.REMARKS;
                AT.MANAGEMENT_SOURCE_DATE = TVM.MANAGEMENT_SOURCE_DATE;
                AT.TASK_TYPE_ID = TVM.TASK_TYPE_ID;
                AT.REPORT_STATUS_ID = TVM.REPORT_STATUS_ID;
                AT.REPORT_CREATE_STATUS = "0";
                AT.FINANCIAL = TVM?.FINANCIAL ?? 0;
                AT.NON_FINANCIAL = TVM.NON_FINANCIAL;

                AT.ACT = true;
                AT.CDT = DateTime.Now;
                AT.CDU = U;

                //Save Task Data
                AT.TASK_ID = Sequence.GetSeq(30).ToString();
                db.AUDIT_TASK.Add(AT);

                if (TVM.FileX != null && TVM.FileX.ContentLength > 0)
                {
                    AUDIT_TASK_IMG I = new AUDIT_TASK_IMG();
                    I.TASK_IMG_ID = Guid.NewGuid().ToString();
                    I.IMG_NAME = TVM.FileX.FileName;
                    I.ACT = true;
                    I.CDT = DateTime.Now;
                    I.IMG_PATH = Sequence.GetImgPath(108);
                    I.TASK_ID = AT.TASK_ID;
                    db.AUDIT_TASK_IMG.Add(I);

                    //upload file to server
                    var path = System.IO.Path.Combine(Server.MapPath(I.IMG_PATH), I.TASK_IMG_ID);
                    TVM.FileX.SaveAs(path);
                }


                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Saved succefully','success');";
                return RedirectToAction("NewTask", new { TaskId = AT.TASK_ID });
            }

            ViewBag.LOCATION_ID = new SelectList(RP.C_LocationList(), "LOCATION_ID", "LOCATION_NAME");
            ViewBag.COMPANY_GROUP_ID = new SelectList(RP.CompanyGroupList(), "COMPANY_GROUP_ID", "COMPANY_GROUP_NAME");
            ViewBag.DEPARTMENT_ID = new SelectList(RP.C_DepartmentList(), "DEPARTMENT_ID", "DEPARTMENT_NAME");
            ViewBag.TASK_COMMUNICATION_BY_ID = new SelectList(RP.TaskCommunicationByList(), "TASK_COMMUNICATION_BY_ID", "TASK_COMMUNICATION_BY_NAME");
            ViewBag.TASK_ASSIGN_BY_ID = new SelectList(RP.TaskAssignByList(), "TASK_ASSIGN_BY_ID", "TASK_ASSIGN_BY_NAME");
            ViewBag.MANAGEMENT_SOURCE_ID = new SelectList(RP.ManagementSourceList(), "MANAGEMENT_SOURCE_ID", "MANAGEMENT_SOURCE_NAME");
            ViewBag.TASK_TYPE_ID = new SelectList(RP.TaskTypeList(), "TASK_TYPE_ID", "TASK_TYPE_NAME");
            ViewBag.TASK_PRIORITY_ID = new SelectList(RP.TaskPriorityList(), "TASK_PRIORITY_ID", "TASK_PRIORITY_NAME");
            ViewBag.REPORT_STATUS_ID = new SelectList(RP.ReportStatusList(), "REPORT_STATUS_ID", "REPORT_STATUS_NAME");

            ViewBag.AUDITOR_ID = new SelectList(RP.AuditorList(), "AUDITOR_ID", "AUDITOR_NAME");
            ViewBag.TASK_STATUS = new SelectList(RP.TaskStatusList(), "TASK_STATUS_ID", "TASK_STATUS_NAME");
            return View();
        }

        [KAuth(5002)]
        [HttpGet]
        public ActionResult Update(string id)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (id == null)
            {
                TempData["msg"] = "swal('Opss..','Try with valid data','info');";
                return RedirectToAction("TaskList");
            }
            AUDIT_TASK ATU = db.AUDIT_TASK.Find(id);
            if (ATU == null)
            {
                TempData["msg"] = "swal('Ohh..','No Liceense found','warning');";
                return RedirectToAction("TaskList");
            }
            UpdateTaskVM ATUVM = new UpdateTaskVM();

            ATUVM.TASK_DETAILS = ATU.TASK_DETAILS;
            ATUVM.TASK_TYPE_ID = ATU.TASK_TYPE_ID;
            ATUVM.MANAGEMENT_SOURCE_ID = ATU.MANAGEMENT_SOURCE_ID;
            ATUVM.TASK_COMMUNICATION_BY_ID = ATU.TASK_COMMUNICATION_BY_ID;
            ATUVM.TASK_ASSIGN_BY_ID = ATU.TASK_ASSIGN_BY_ID;
            ATUVM.TASK_PRIORITY_ID = ATU.TASK_PRIORITY_ID;

            ATUVM.LOCATION_ID = ATU.LOCATION_ID;
            ATUVM.COMPANY_ID = ATU.COMPANY_ID;
            ATUVM.COMPANY_GROUP_ID = ATU.COMPANY_GROUP_ID;

            ATUVM.DEPARTMENT_ID = ATU.DEPARTMENT_ID;
            ATUVM.TASK_STATUS = ATU.TASK_STATUS;
            ATUVM.REPORT_STATUS_ID = ATU.REPORT_STATUS_ID;
            ATUVM.TASK_ASSIGN_DATE = ATU.TASK_ASSIGN_DATE;
            ATUVM.MANAGEMENT_SOURCE_DATE = ATU.MANAGEMENT_SOURCE_DATE;

            ATUVM.TASK_START_PLANED_DATE = ATU.TASK_START_PLANED_DATE;
            ATUVM.TASK_START_DATE = ATU.TASK_START_DATE;
            ATUVM.TASK_DEADLINE = ATU.TASK_DEADLINE;
            ATUVM.REMARKS = ATU.REMARKS;
            ATUVM.FINANCIAL = ATU.FINANCIAL;
            ATUVM.NON_FINANCIAL = ATU.NON_FINANCIAL;

            ATUVM.TASK_ID = ATU.TASK_ID;

            ViewBag.LOCATION_ID = new SelectList(RP.C_LocationList(), "LOCATION_ID", "LOCATION_NAME");
            ViewBag.DEPARTMENT_ID = new SelectList(RP.C_DepartmentList(), "DEPARTMENT_ID", "DEPARTMENT_NAME");
            ViewBag.COMPANY_GROUP_ID = new SelectList(RP.CompanyGroupList(), "COMPANY_GROUP_ID", "COMPANY_GROUP_NAME");
            ViewBag.COMPANY_ID = new SelectList(RP.C_CompanyList(), "COMPANY_ID", "COMPANY_NAME");
            ViewBag.TASK_COMMUNICATION_BY_ID = new SelectList(RP.TaskCommunicationByList(), "TASK_COMMUNICATION_BY_ID", "TASK_COMMUNICATION_BY_NAME");
            ViewBag.TASK_ASSIGN_BY_ID = new SelectList(RP.TaskAssignByList(), "TASK_ASSIGN_BY_ID", "TASK_ASSIGN_BY_NAME");
            ViewBag.MANAGEMENT_SOURCE_ID = new SelectList(RP.ManagementSourceList(), "MANAGEMENT_SOURCE_ID", "MANAGEMENT_SOURCE_NAME");
            ViewBag.TASK_TYPE_ID = new SelectList(RP.TaskTypeList(), "TASK_TYPE_ID", "TASK_TYPE_NAME");
            ViewBag.TASK_PRIORITY_ID = new SelectList(RP.TaskPriorityList(), "TASK_PRIORITY_ID", "TASK_PRIORITY_NAME");
            ViewBag.REPORT_STATUS_ID = new SelectList(RP.ReportStatusList(), "REPORT_STATUS_ID", "REPORT_STATUS_NAME");
            ViewBag.TASK_STATUS = new SelectList(RP.TaskStatusList(), "TASK_STATUS_ID", "TASK_STATUS_NAME");

            return PartialView("_Update", ATUVM);
        }

        [KAuth(5002)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(UpdateTaskVM UT)
        {
            if (ModelState.IsValid)
            {
                string U = Session["UserId"].ToString();
                if (UT.FileX != null && UT.FileX.ContentLength > 0)
                {

                    int fileSize = UT.FileX.ContentLength;
                    if (fileSize <= 0 || fileSize > 21000000)
                    {
                        TempData["msg"] = "swal('Opps..','File is too larger, Max file size 20MB','warning');";
                        ViewBag.LOCATION_ID = new SelectList(RP.C_LocationList(), "LOCATION_ID", "LOCATION_NAME");
                        ViewBag.DEPARTMENT_ID = new SelectList(RP.C_DepartmentList(), "DEPARTMENT_ID", "DEPARTMENT_NAME");
                        ViewBag.COMPANY_GROUP_ID = new SelectList(RP.CompanyGroupList(), "COMPANY_GROUP_ID", "COMPANY_GROUP_NAME");
                        ViewBag.COMPANY_ID = new SelectList(RP.C_CompanyList(), "COMPANY_ID", "COMPANY_NAME");
                        ViewBag.TASK_COMMUNICATION_BY_ID = new SelectList(RP.TaskCommunicationByList(), "TASK_COMMUNICATION_BY_ID", "TASK_COMMUNICATION_BY_NAME");
                        ViewBag.TASK_ASSIGN_BY_ID = new SelectList(RP.TaskAssignByList(), "TASK_ASSIGN_BY_ID", "TASK_ASSIGN_BY_NAME");
                        ViewBag.MANAGEMENT_SOURCE_ID = new SelectList(RP.ManagementSourceList(), "MANAGEMENT_SOURCE_ID", "MANAGEMENT_SOURCE_NAME");
                        ViewBag.TASK_TYPE_ID = new SelectList(RP.TaskTypeList(), "TASK_TYPE_ID", "TASK_TYPE_NAME");
                        ViewBag.TASK_PRIORITY_ID = new SelectList(RP.TaskPriorityList(), "TASK_PRIORITY_ID", "TASK_PRIORITY_NAME");
                        ViewBag.REPORT_STATUS_ID = new SelectList(RP.ReportStatusList(), "REPORT_STATUS_ID", "REPORT_STATUS_NAME");
                        ViewBag.TASK_STATUS = new SelectList(RP.TaskStatusList(), "TASK_STATUS_ID", "TASK_STATUS_NAME");
                        return View();
                    }
                }

                if (UT.TASK_STATUS == "5" && (string.IsNullOrEmpty(UT.REPORT_STATUS_ID)))
                {
                    TempData["msg"] = "swal('Opps..','Please Select Report Status Before Completed Task','warning');";
                    return RedirectToAction("TaskList", new { TaskId = UT.TASK_ID }); 
                }


                AUDIT_TASK AT = db.AUDIT_TASK.Find(UT.TASK_ID);

                AT.TASK_DETAILS = UT.TASK_DETAILS;
                AT.TASK_TYPE_ID = UT.TASK_TYPE_ID;
                AT.MANAGEMENT_SOURCE_ID = UT.MANAGEMENT_SOURCE_ID;
                AT.TASK_COMMUNICATION_BY_ID = UT.TASK_COMMUNICATION_BY_ID;
                AT.TASK_ASSIGN_BY_ID = UT.TASK_ASSIGN_BY_ID;
                AT.TASK_PRIORITY_ID = UT.TASK_PRIORITY_ID;

                AT.LOCATION_ID = UT.LOCATION_ID;
                AT.COMPANY_ID = UT.COMPANY_ID;
                AT.COMPANY_GROUP_ID = UT.COMPANY_GROUP_ID;
                AT.DEPARTMENT_ID = UT.DEPARTMENT_ID;
                AT.TASK_STATUS = UT.TASK_STATUS;
                AT.REPORT_STATUS_ID = UT.REPORT_STATUS_ID;
                AT.FINANCIAL = UT.FINANCIAL;
                AT.NON_FINANCIAL = UT.NON_FINANCIAL;

                if (UT.TASK_ASSIGN_DATE != null)
                {
                    AT.TASK_ASSIGN_DATE = UT.TASK_ASSIGN_DATE;
                }
                if (UT.MANAGEMENT_SOURCE_DATE != null)
                {
                    AT.MANAGEMENT_SOURCE_DATE = UT.MANAGEMENT_SOURCE_DATE;
                }
                if (UT.TASK_START_PLANED_DATE != null)
                {
                    AT.TASK_START_PLANED_DATE = UT.TASK_START_PLANED_DATE;
                }
                if (UT.TASK_START_DATE != null)
                {
                    AT.TASK_START_DATE = UT.TASK_START_DATE;
                }
                if (UT.TASK_DEADLINE != null)
                {
                    AT.TASK_DEADLINE = UT.TASK_DEADLINE;
                }

                AT.REMARKS = UT.REMARKS;


                if (UT.FileX != null && UT.FileX.ContentLength > 0)
                {
                    AUDIT_TASK_IMG I = new AUDIT_TASK_IMG();
                    I.TASK_IMG_ID = Guid.NewGuid().ToString();
                    I.IMG_NAME = UT.FileX.FileName;
                    I.ACT = true;
                    I.CDT = DateTime.Now;
                    I.IMG_PATH = Sequence.GetImgPath(108);
                    I.TASK_ID = UT.TASK_ID;
                    db.AUDIT_TASK_IMG.Add(I);

                    var path = System.IO.Path.Combine(Server.MapPath(I.IMG_PATH), I.TASK_IMG_ID);
                    UT.FileX.SaveAs(path);
                }

                AT.UDT = DateTime.Now;
                AT.UDU = U;
                db.Entry(AT).State = EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Task data updated','success');";
                return RedirectToAction("TaskList", new { TaskId = UT.TASK_ID });
            }
            TempData["msg"] = "swal('Ohh..','Data not updated','warning');";
            return RedirectToAction("TaskList", new { TaskId = UT.TASK_ID });
        }

        [KAuth(5003)]
        public ActionResult TaskList()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (T == "10")
            {
                string sql_1 = @"SELECT 
                                   l.LOCATION_NAME,
                                   g.COMPANY_GROUP_NAME,
                                   com.COMPANY_NAME,
                                   dep.DEPARTMENT_NAME,
                                   assign_by.TASK_ASSIGN_BY_NAME,
                                   management_source.MANAGEMENT_SOURCE_NAME,
                                   a.MANAGEMENT_SOURCE_DATE,
                                   a.TASK_DETAILS,
                                   a.TASK_ASSIGN_DATE,
                                   a.TASK_START_PLANED_DATE,
                                   b.USER_NAME,
                                   a.TASK_START_DATE,
                                   a.TASK_DEADLINE,
                                   communication.TASK_COMMUNICATION_BY_NAME,
                                   p.TASK_PRIORITY_NAME,
                                   a.TASK_STATUS,
                                   c.TASK_STATUS_NAME,
                                   t.TASK_TYPE_NAME,
                                   s.REPORT_STATUS_NAME,
                                   a.REMARKS, 
                                   a.TASK_ID,
                                   a.ACT,
                                   a.FINANCIAL,
                                   a.NON_FINANCIAL
                              FROM AUDIT_TASK a
                              LEFT JOIN C_LOCATION l ON a.LOCATION_ID = l.LOCATION_ID
                              LEFT JOIN C_COMPANY_GROUP g ON a.COMPANY_GROUP_ID = g.COMPANY_GROUP_ID
                              LEFT JOIN C_COMPANY com ON a.COMPANY_ID = com.COMPANY_ID
                              LEFT JOIN C_DEPARTMENT dep ON a.DEPARTMENT_ID = dep.DEPARTMENT_ID
                              LEFT JOIN A_USERS b ON a.CDU = b.USER_TEXT
                              LEFT JOIN AUDIT_TASK_ASSIGN_BY assign_by ON a.TASK_ASSIGN_BY_ID = assign_by.TASK_ASSIGN_BY_ID
                              LEFT JOIN AUDIT_MANAGEMENT_SOURCE management_source ON a.MANAGEMENT_SOURCE_ID = management_source.MANAGEMENT_SOURCE_ID
                              LEFT JOIN AUDIT_TASK_COMMUNICATION_BY communication ON a.TASK_COMMUNICATION_BY_ID = communication.TASK_COMMUNICATION_BY_ID
                              LEFT JOIN AUDIT_TASK_PRIORITY p ON a.TASK_PRIORITY_ID = p.TASK_PRIORITY_ID
                              LEFT JOIN AUDIT_TASK_STATUS c ON a.TASK_STATUS = c.TASK_STATUS_ID   
                              LEFT JOIN AUDIT_TASK_TYPE t ON a.TASK_TYPE_ID = t.TASK_TYPE_ID   
                              LEFT JOIN AUDIT_REPORT_STATUS s ON a.REPORT_STATUS_ID = s.REPORT_STATUS_ID                   
                              WHERE a.ACT = 1 AND a.REPORT_CREATE_STATUS=0 AND a.TASK_STATUS !=5";
                var TaskData = db.Database.SqlQuery<NewTaskVM>(sql: sql_1);
                ViewBag.Message = "Audit Task List";
                var R = TaskData.Take(500).ToList();
                return View(R);
            }
            else
            {
                string sql_1 = @"SELECT 
                                   l.LOCATION_NAME,
                                   g.COMPANY_GROUP_NAME,
                                   com.COMPANY_NAME,
                                   dep.DEPARTMENT_NAME,
                                   assign_by.TASK_ASSIGN_BY_NAME,
                                   management_source.MANAGEMENT_SOURCE_NAME,
                                   a.MANAGEMENT_SOURCE_DATE,
                                   a.TASK_DETAILS,
                                   a.TASK_ASSIGN_DATE,
                                   a.TASK_START_PLANED_DATE,
                                   b.USER_NAME,
                                   a.TASK_START_DATE,
                                   a.TASK_DEADLINE,
                                   communication.TASK_COMMUNICATION_BY_NAME,
                                   p.TASK_PRIORITY_NAME,
                                   c.TASK_STATUS_NAME,
                                   a.TASK_STATUS,
                                   t.TASK_TYPE_NAME,
                                   s.REPORT_STATUS_NAME,
                                   a.REMARKS, 
                                   a.TASK_ID,
                                   a.ACT,
                                   a.FINANCIAL,
                                   a.NON_FINANCIAL
                              FROM AUDIT_TASK a
                              LEFT JOIN C_LOCATION l ON a.LOCATION_ID = l.LOCATION_ID
                              LEFT JOIN C_COMPANY_GROUP g ON a.COMPANY_GROUP_ID = g.COMPANY_GROUP_ID
                              LEFT JOIN C_COMPANY com ON a.COMPANY_ID = com.COMPANY_ID
                              LEFT JOIN C_DEPARTMENT dep ON a.DEPARTMENT_ID = dep.DEPARTMENT_ID
                              LEFT JOIN A_USERS b ON a.CDU = b.USER_TEXT
                              LEFT JOIN AUDIT_TASK_ASSIGN_BY assign_by ON a.TASK_ASSIGN_BY_ID = assign_by.TASK_ASSIGN_BY_ID
                              LEFT JOIN AUDIT_MANAGEMENT_SOURCE management_source ON a.MANAGEMENT_SOURCE_ID = management_source.MANAGEMENT_SOURCE_ID
                              LEFT JOIN AUDIT_TASK_COMMUNICATION_BY communication ON a.TASK_COMMUNICATION_BY_ID = communication.TASK_COMMUNICATION_BY_ID
                              LEFT JOIN AUDIT_TASK_PRIORITY p ON a.TASK_PRIORITY_ID = p.TASK_PRIORITY_ID
                              LEFT JOIN AUDIT_TASK_STATUS c ON a.TASK_STATUS = c.TASK_STATUS_ID   
                              LEFT JOIN AUDIT_TASK_TYPE t ON a.TASK_TYPE_ID = t.TASK_TYPE_ID   
                              LEFT JOIN AUDIT_REPORT_STATUS s ON a.REPORT_STATUS_ID = s.REPORT_STATUS_ID                   
                              WHERE a.ACT = 1 AND a.REPORT_CREATE_STATUS=0 AND a.TASK_STATUS !=5 AND a.CDU= '" + U + @"'";
                var TaskData = db.Database.SqlQuery<NewTaskVM>(sql: sql_1);
                ViewBag.Message = "Audit Task List";
                var R = TaskData.Take(500).ToList();
                return View(R);
            }
            
        }

        public ActionResult TaskDetailList()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (T == "10")
            {
                string sql_1 = @"SELECT 
                                   l.LOCATION_NAME,
                                   g.COMPANY_GROUP_NAME,
                                   com.COMPANY_NAME,
                                   dep.DEPARTMENT_NAME,
                                   assign_by.TASK_ASSIGN_BY_NAME,
                                   management_source.MANAGEMENT_SOURCE_NAME,
                                   a.MANAGEMENT_SOURCE_DATE,
                                   a.TASK_DETAILS,
                                   a.TASK_ASSIGN_DATE,
                                   a.TASK_START_PLANED_DATE,
                                   b.USER_NAME,
                                   a.TASK_START_DATE,
                                   a.TASK_DEADLINE,
                                   communication.TASK_COMMUNICATION_BY_NAME,
                                   p.TASK_PRIORITY_NAME,
                                   a.TASK_STATUS,
                                   c.TASK_STATUS_NAME,
                                   t.TASK_TYPE_NAME,
                                   s.REPORT_STATUS_NAME,
                                   a.REMARKS, 
                                   a.TASK_ID,
                                   a.ACT,
                                   a.FINANCIAL,
                                   a.NON_FINANCIAL,
                                   CASE 
                                        WHEN a.REPORT_CREATE_STATUS = 0 AND a.TASK_STATUS IN (2, 3, 4, 5) THEN 'Work In Process'
                                        WHEN a.REPORT_CREATE_STATUS = 0 AND a.TASK_STATUS = 1 THEN 'Task Not Started'
                                        WHEN a.REPORT_CREATE_STATUS = 1 THEN 'Report Created'
                                        ELSE 'Other'
                                    END AS TASK_CATEGORY
                              FROM AUDIT_TASK a
                              LEFT JOIN C_LOCATION l ON a.LOCATION_ID = l.LOCATION_ID
                              LEFT JOIN C_COMPANY_GROUP g ON a.COMPANY_GROUP_ID = g.COMPANY_GROUP_ID
                              LEFT JOIN C_COMPANY com ON a.COMPANY_ID = com.COMPANY_ID
                              LEFT JOIN C_DEPARTMENT dep ON a.DEPARTMENT_ID = dep.DEPARTMENT_ID
                              LEFT JOIN A_USERS b ON a.CDU = b.USER_TEXT
                              LEFT JOIN AUDIT_TASK_ASSIGN_BY assign_by ON a.TASK_ASSIGN_BY_ID = assign_by.TASK_ASSIGN_BY_ID
                              LEFT JOIN AUDIT_MANAGEMENT_SOURCE management_source ON a.MANAGEMENT_SOURCE_ID = management_source.MANAGEMENT_SOURCE_ID
                              LEFT JOIN AUDIT_TASK_COMMUNICATION_BY communication ON a.TASK_COMMUNICATION_BY_ID = communication.TASK_COMMUNICATION_BY_ID
                              LEFT JOIN AUDIT_TASK_PRIORITY p ON a.TASK_PRIORITY_ID = p.TASK_PRIORITY_ID
                              LEFT JOIN AUDIT_TASK_STATUS c ON a.TASK_STATUS = c.TASK_STATUS_ID   
                              LEFT JOIN AUDIT_TASK_TYPE t ON a.TASK_TYPE_ID = t.TASK_TYPE_ID   
                              LEFT JOIN AUDIT_REPORT_STATUS s ON a.REPORT_STATUS_ID = s.REPORT_STATUS_ID                   
                              WHERE a.ACT = 1 
                              ORDER BY TASK_CATEGORY";
                var TaskData = db.Database.SqlQuery<NewTaskVM>(sql: sql_1);
                ViewBag.Message = "Audit Task List";
                var R = TaskData.Take(500).ToList();
                return View(R);
            }
            else
            {
                string sql_1 = @"SELECT 
                                   l.LOCATION_NAME,
                                   g.COMPANY_GROUP_NAME,
                                   com.COMPANY_NAME,
                                   dep.DEPARTMENT_NAME,
                                   assign_by.TASK_ASSIGN_BY_NAME,
                                   management_source.MANAGEMENT_SOURCE_NAME,
                                   a.MANAGEMENT_SOURCE_DATE,
                                   a.TASK_DETAILS,
                                   a.TASK_ASSIGN_DATE,
                                   a.TASK_START_PLANED_DATE,
                                   b.USER_NAME,
                                   a.TASK_START_DATE,
                                   a.TASK_DEADLINE,
                                   communication.TASK_COMMUNICATION_BY_NAME,
                                   p.TASK_PRIORITY_NAME,
                                   a.TASK_STATUS,
                                   c.TASK_STATUS_NAME,
                                   t.TASK_TYPE_NAME,
                                   s.REPORT_STATUS_NAME,
                                   a.REMARKS, 
                                   a.TASK_ID,
                                   a.ACT,
                                   a.FINANCIAL,
                                   a.NON_FINANCIAL,
                                   CASE 
                                        WHEN a.REPORT_CREATE_STATUS = 0 AND a.TASK_STATUS IN (2, 3, 4, 5) THEN 'Work In Process'
                                        WHEN a.REPORT_CREATE_STATUS = 0 AND a.TASK_STATUS = 1 THEN 'Task Not Started'
                                        WHEN a.REPORT_CREATE_STATUS = 1 THEN 'Report Created'
                                        ELSE 'Other'
                                    END AS TASK_CATEGORY
                              FROM AUDIT_TASK a
                              LEFT JOIN C_LOCATION l ON a.LOCATION_ID = l.LOCATION_ID
                              LEFT JOIN C_COMPANY_GROUP g ON a.COMPANY_GROUP_ID = g.COMPANY_GROUP_ID
                              LEFT JOIN C_COMPANY com ON a.COMPANY_ID = com.COMPANY_ID
                              LEFT JOIN C_DEPARTMENT dep ON a.DEPARTMENT_ID = dep.DEPARTMENT_ID
                              LEFT JOIN A_USERS b ON a.CDU = b.USER_TEXT
                              LEFT JOIN AUDIT_TASK_ASSIGN_BY assign_by ON a.TASK_ASSIGN_BY_ID = assign_by.TASK_ASSIGN_BY_ID
                              LEFT JOIN AUDIT_MANAGEMENT_SOURCE management_source ON a.MANAGEMENT_SOURCE_ID = management_source.MANAGEMENT_SOURCE_ID
                              LEFT JOIN AUDIT_TASK_COMMUNICATION_BY communication ON a.TASK_COMMUNICATION_BY_ID = communication.TASK_COMMUNICATION_BY_ID
                              LEFT JOIN AUDIT_TASK_PRIORITY p ON a.TASK_PRIORITY_ID = p.TASK_PRIORITY_ID
                              LEFT JOIN AUDIT_TASK_STATUS c ON a.TASK_STATUS = c.TASK_STATUS_ID   
                              LEFT JOIN AUDIT_TASK_TYPE t ON a.TASK_TYPE_ID = t.TASK_TYPE_ID   
                              LEFT JOIN AUDIT_REPORT_STATUS s ON a.REPORT_STATUS_ID = s.REPORT_STATUS_ID                   
                              WHERE a.ACT = 1 
                              AND a.CDU= '" + U + @"'
                              ORDER BY TASK_CATEGORY";
                var TaskData = db.Database.SqlQuery<NewTaskVM>(sql: sql_1);
                ViewBag.Message = "Audit All Task List";
                var R = TaskData.Take(500).ToList();
                return View(R);
            }

        }

        [KAuth(5202)]
        public ActionResult ReportList()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (T == "10")
            {
                string sql_1 = @"SELECT 
                                r.AUDIT_RPT_ID,
                                r.RPT_SENT_DATE,
                                r.TASK_ID,
                                r.AUDIT_RPT_NAME,
                                l.LOCATION_NAME,
                                g.COMPANY_GROUP_NAME,
                                com.COMPANY_NAME,
                                dep.DEPARTMENT_NAME,
                                r.WORK_SCOPE,
                                r.WORK_OPPORTUNITY,
                                u.USER_NAME
                                from AUDIT_REPORT r
                                LEFT JOIN AUDIT_TASK at ON r.TASK_ID = at.TASK_ID
                                LEFT JOIN A_USERS u ON r.CDU = u.USER_TEXT
                                INNER JOIN C_LOCATION l ON at.LOCATION_ID = l.LOCATION_ID
                                INNER JOIN C_COMPANY_GROUP g ON at.COMPANY_GROUP_ID = g.COMPANY_GROUP_ID
                                INNER JOIN C_COMPANY com ON at.COMPANY_ID = com.COMPANY_ID
                                INNER JOIN C_DEPARTMENT dep ON at.DEPARTMENT_ID = dep.DEPARTMENT_ID
                                WHERE r.ACT = 1 AND r.APPROVE_SENT_STATUS='N' AND at.REPORT_CREATE_STATUS=1 AND at.TASK_STATUS=5 AND at.REPORT_STATUS_ID in (1,3) ORDER BY TO_NUMBER(r.AUDIT_RPT_ID) ASC";
                var TaskData = db.Database.SqlQuery<AuditReportListVM>(sql: sql_1);
                ViewBag.Message = "Audit Report List";
                var R = TaskData.Take(500).ToList();
                return View(R);
            }
            else
            {
                string sql_1 = @"SELECT 
                                r.AUDIT_RPT_ID,
                                r.RPT_SENT_DATE,
                                r.TASK_ID,
                                r.AUDIT_RPT_NAME,
                                l.LOCATION_NAME,
                                g.COMPANY_GROUP_NAME,
                                com.COMPANY_NAME,
                                dep.DEPARTMENT_NAME,
                                r.WORK_SCOPE,
                                r.WORK_OPPORTUNITY,
                                u.USER_NAME
                                from AUDIT_REPORT r
                                LEFT JOIN AUDIT_TASK at ON r.TASK_ID = at.TASK_ID
                                LEFT JOIN A_USERS u ON r.CDU = u.USER_TEXT
                                INNER JOIN C_LOCATION l ON at.LOCATION_ID = l.LOCATION_ID
                                INNER JOIN C_COMPANY_GROUP g ON at.COMPANY_GROUP_ID = g.COMPANY_GROUP_ID
                                INNER JOIN C_COMPANY com ON at.COMPANY_ID = com.COMPANY_ID
                                INNER JOIN C_DEPARTMENT dep ON at.DEPARTMENT_ID = dep.DEPARTMENT_ID
                                WHERE r.ACT = 1 AND r.APPROVE_SENT_STATUS='N' AND at.REPORT_CREATE_STATUS=1 AND at.TASK_STATUS=5 AND at.REPORT_STATUS_ID in (1,3) AND r.CDU= '" + U + @"' ORDER BY TO_NUMBER(r.AUDIT_RPT_ID) ASC";
                var TaskData = db.Database.SqlQuery<AuditReportListVM>(sql: sql_1);
                ViewBag.Message = "Audit Report List";
                var R = TaskData.Take(500).ToList();
                return View(R);
            }
        }

        [KAuth(5204)]
        public ActionResult ReportDetailsList()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (T == "10")
            {
                string sql_1 = @"SELECT r.AUDIT_RPT_ID,
                                 r.RPT_SENT_DATE,
                                 r.TASK_ID,
                                 r.AUDIT_RPT_NAME,
                                 l.LOCATION_NAME,
                                 g.COMPANY_GROUP_NAME,
                                 com.COMPANY_NAME,
                                 dep.DEPARTMENT_NAME,
                                 r.WORK_SCOPE,
                                 r.WORK_OPPORTUNITY,
                                 u.USER_NAME,
                                 ct.TASK_CATEGORY_TYPE_NAME,
                                 c.TASK_CATEGORY_NAME,

                                 ds.AUDIT_SAVING_NAME AS DIRECT_SAVING_NAME,
                                 r.DIRECT_SAVING_AMOUNT,
                                 ids.AUDIT_SAVING_NAME AS INDIRECT_SAVING_NAME,
                                 r.INDIRECT_SAVING_AMOUNT,

                                 r.DEMOTION,
                                 r.DEVELOPMENT,
                                 r.JOB_SACK_OUT,
                                 r.TRANSFER,
                                 r.WARNING_LETTER,
                                 r.PENALTY,
                                 r.HOLD_PAYMENT,
                                 r.REMARK,
                                 m.MANAGEMENT_ACTION_NAME,

                                 arf.FU1ST_FOLLOWUP_NUMBER,
                                 r.FIRST_FOLLOWUP_DATE,
                                 arf.FU1ST_FOLLOWUP_DATE,
                                 arf.FU1ST_TOTAL_FINDINGS,
                                 arf.FU1ST_ACTION_FINDINGS,
                                 arf.FU1ST_NO_ACTION_FINDINGS,
                                 arf.FU1ST_ACTION_PERCENTAGE,
                                 arf.FU1ST_RISK_TYPE_NAME,
                                 arf.FU1ST_RISK_MITIGATION_NAME,
                                 arf.FU1ST_RISK_MITIGATION_MARKS,
                                 arf.FU1ST_REPLY_PERSON,
                                 arf.FU1ST_RESPONSIBLE_DEPT_HEAD,
                                 arf.FU1ST_REMARKS,
                                 arf.FU1ST_SIGNIFICANT_GAP,
                                 arf.FU1ST_FU_REQUIREMENT_NAME,

                                 arf.FU2ND_FOLLOWUP_NUMBER,
                                 r.SECOND_FOLLOWUP_DATE,
                                 arf.FU2ND_FOLLOWUP_DATE,
                                 arf.FU2ND_TOTAL_FINDINGS,
                                 arf.FU2ND_ACTION_FINDINGS,
                                 arf.FU2ND_NO_ACTION_FINDINGS,
                                 arf.FU2ND_ACTION_PERCENTAGE,
                                 arf.FU2ND_RISK_TYPE_NAME,
                                 arf.FU2ND_RISK_MITIGATION_NAME,
                                 arf.FU2ND_RISK_MITIGATION_MARKS,
                                 arf.FU2ND_REPLY_PERSON,
                                 arf.FU2ND_RESPONSIBLE_DEPT_HEAD,
                                 arf.FU2ND_REMARKS,
                                 arf.FU2ND_SIGNIFICANT_GAP,
                                 arf.FU2ND_FU_REQUIREMENT_NAME,

                                 arf.FU3RD_FOLLOWUP_NUMBER,
                                 r.THIRD_FOLLOWUP_DATE,
                                 arf.FU3RD_FOLLOWUP_DATE,
                                 arf.FU3RD_TOTAL_FINDINGS,
                                 arf.FU3RD_ACTION_FINDINGS,
                                 arf.FU3RD_NO_ACTION_FINDINGS,
                                 arf.FU3RD_ACTION_PERCENTAGE,
                                 arf.FU3RD_RISK_TYPE_NAME,
                                 arf.FU3RD_RISK_MITIGATION_NAME,
                                 arf.FU3RD_RISK_MITIGATION_MARKS,
                                 arf.FU3RD_REPLY_PERSON,
                                 arf.FU3RD_RESPONSIBLE_DEPT_HEAD,
                                 arf.FU3RD_REMARKS,
                                 arf.FU3RD_SIGNIFICANT_GAP,
                                 arf.FU3RD_FU_REQUIREMENT_NAME

                                FROM AUDIT_REPORT r
                                     LEFT JOIN AUDIT_TASK at ON r.TASK_ID = at.TASK_ID
                                     LEFT JOIN AUDIT_TASK_CATEGORY_TYPE ct ON ct.TASK_CATEGORY_TYPE_ID = r.TASK_CATEGORY_TYPE_ID
                                     LEFT JOIN AUDIT_TASK_CATEGORY c ON c.TASK_CATEGORY_ID = r.TASK_CATEGORY_ID
                                     LEFT JOIN AUDIT_RPT_SAVING_NAME ds ON ds.AUDIT_SAVING_NAME_ID = r.DIRECT_SAVING_TYPE_ID
                                     LEFT JOIN AUDIT_RPT_SAVING_NAME ids ON ids.AUDIT_SAVING_NAME_ID  = r.INDIRECT_SAVING_TYPE_ID 
                                     LEFT JOIN AUDIT_MANAGEMENT_ACTION m ON r.MANAGEMENT_ACTION_ID = m.MANAGEMENT_ACTION_ID
                                     LEFT JOIN A_USERS u ON r.CDU = u.USER_TEXT

                                     LEFT JOIN V_AUDIT_REPORT_FOLLOWUP arf ON arf.AUDIT_RPT_ID = r.AUDIT_RPT_ID


                                     INNER JOIN C_LOCATION l ON at.LOCATION_ID = l.LOCATION_ID
                                     INNER JOIN C_COMPANY_GROUP g ON at.COMPANY_GROUP_ID = g.COMPANY_GROUP_ID
                                     INNER JOIN C_COMPANY com ON at.COMPANY_ID = com.COMPANY_ID
                                     INNER JOIN C_DEPARTMENT dep ON at.DEPARTMENT_ID = dep.DEPARTMENT_ID
                                WHERE     r.ACT = 1
                                     AND r.APPROVE_SENT_STATUS = 'N'
                                     AND at.REPORT_CREATE_STATUS = 1
                                     AND at.TASK_STATUS = 5
                                     AND at.REPORT_STATUS_ID in (1,3)
                                ORDER BY TO_NUMBER (r.AUDIT_RPT_ID) ASC";
                var TaskData = db.Database.SqlQuery<AuditReportDetailsListVM>(sql: sql_1);
                ViewBag.Message = "Audit Report List";
                var R = TaskData.Take(500).ToList();
                return View(R);
            }
            else
            {
                string sql_1 = @"SELECT r.AUDIT_RPT_ID,
                                 r.RPT_SENT_DATE,
                                 r.TASK_ID,
                                 r.AUDIT_RPT_NAME,
                                 l.LOCATION_NAME,
                                 g.COMPANY_GROUP_NAME,
                                 com.COMPANY_NAME,
                                 dep.DEPARTMENT_NAME,
                                 r.WORK_SCOPE,
                                 r.WORK_OPPORTUNITY,
                                 u.USER_NAME,
                                 ct.TASK_CATEGORY_TYPE_NAME,
                                 c.TASK_CATEGORY_NAME,

                                 ds.AUDIT_SAVING_NAME AS DIRECT_SAVING_NAME,
                                 r.DIRECT_SAVING_AMOUNT,
                                 ids.AUDIT_SAVING_NAME AS INDIRECT_SAVING_NAME,
                                 r.INDIRECT_SAVING_AMOUNT,

                                 r.DEMOTION,
                                 r.DEVELOPMENT,
                                 r.JOB_SACK_OUT,
                                 r.TRANSFER,
                                 r.WARNING_LETTER,
                                 r.PENALTY,
                                 r.HOLD_PAYMENT,
                                 r.REMARK,
                                 m.MANAGEMENT_ACTION_NAME,

                                 arf.FU1ST_FOLLOWUP_NUMBER,
                                 r.FIRST_FOLLOWUP_DATE,
                                 arf.FU1ST_FOLLOWUP_DATE,
                                 arf.FU1ST_TOTAL_FINDINGS,
                                 arf.FU1ST_ACTION_FINDINGS,
                                 arf.FU1ST_NO_ACTION_FINDINGS,
                                 arf.FU1ST_ACTION_PERCENTAGE,
                                 arf.FU1ST_RISK_TYPE_NAME,
                                 arf.FU1ST_RISK_MITIGATION_NAME,
                                 arf.FU1ST_RISK_MITIGATION_MARKS,
                                 arf.FU1ST_REPLY_PERSON,
                                 arf.FU1ST_RESPONSIBLE_DEPT_HEAD,
                                 arf.FU1ST_REMARKS,
                                 arf.FU1ST_SIGNIFICANT_GAP,
                                 arf.FU1ST_FU_REQUIREMENT_NAME,

                                 arf.FU2ND_FOLLOWUP_NUMBER,
                                 r.SECOND_FOLLOWUP_DATE,
                                 arf.FU2ND_FOLLOWUP_DATE,
                                 arf.FU2ND_TOTAL_FINDINGS,
                                 arf.FU2ND_ACTION_FINDINGS,
                                 arf.FU2ND_NO_ACTION_FINDINGS,
                                 arf.FU2ND_ACTION_PERCENTAGE,
                                 arf.FU2ND_RISK_TYPE_NAME,
                                 arf.FU2ND_RISK_MITIGATION_NAME,
                                 arf.FU2ND_RISK_MITIGATION_MARKS,
                                 arf.FU2ND_REPLY_PERSON,
                                 arf.FU2ND_RESPONSIBLE_DEPT_HEAD,
                                 arf.FU2ND_REMARKS,
                                 arf.FU2ND_SIGNIFICANT_GAP,
                                 arf.FU2ND_FU_REQUIREMENT_NAME,

                                 arf.FU3RD_FOLLOWUP_NUMBER,
                                 r.THIRD_FOLLOWUP_DATE,
                                 arf.FU3RD_FOLLOWUP_DATE,
                                 arf.FU3RD_TOTAL_FINDINGS,
                                 arf.FU3RD_ACTION_FINDINGS,
                                 arf.FU3RD_NO_ACTION_FINDINGS,
                                 arf.FU3RD_ACTION_PERCENTAGE,
                                 arf.FU3RD_RISK_TYPE_NAME,
                                 arf.FU3RD_RISK_MITIGATION_NAME,
                                 arf.FU3RD_RISK_MITIGATION_MARKS,
                                 arf.FU3RD_REPLY_PERSON,
                                 arf.FU3RD_RESPONSIBLE_DEPT_HEAD,
                                 arf.FU3RD_REMARKS,
                                 arf.FU3RD_SIGNIFICANT_GAP,
                                 arf.FU3RD_FU_REQUIREMENT_NAME

                                FROM AUDIT_REPORT r
                                     LEFT JOIN AUDIT_TASK at ON r.TASK_ID = at.TASK_ID
                                     LEFT JOIN AUDIT_TASK_CATEGORY_TYPE ct ON ct.TASK_CATEGORY_TYPE_ID = r.TASK_CATEGORY_TYPE_ID
                                     LEFT JOIN AUDIT_TASK_CATEGORY c ON c.TASK_CATEGORY_ID = r.TASK_CATEGORY_ID
                                     LEFT JOIN AUDIT_RPT_SAVING_NAME ds ON ds.AUDIT_SAVING_NAME_ID = r.DIRECT_SAVING_TYPE_ID
                                     LEFT JOIN AUDIT_RPT_SAVING_NAME ids ON ids.AUDIT_SAVING_NAME_ID  = r.INDIRECT_SAVING_TYPE_ID 
                                     LEFT JOIN AUDIT_MANAGEMENT_ACTION m ON r.MANAGEMENT_ACTION_ID = m.MANAGEMENT_ACTION_ID
                                     LEFT JOIN A_USERS u ON r.CDU = u.USER_TEXT

                                     LEFT JOIN V_AUDIT_REPORT_FOLLOWUP arf ON arf.AUDIT_RPT_ID = r.AUDIT_RPT_ID


                                     INNER JOIN C_LOCATION l ON at.LOCATION_ID = l.LOCATION_ID
                                     INNER JOIN C_COMPANY_GROUP g ON at.COMPANY_GROUP_ID = g.COMPANY_GROUP_ID
                                     INNER JOIN C_COMPANY com ON at.COMPANY_ID = com.COMPANY_ID
                                     INNER JOIN C_DEPARTMENT dep ON at.DEPARTMENT_ID = dep.DEPARTMENT_ID
                               WHERE     r.ACT = 1
                                     AND r.APPROVE_SENT_STATUS = 'N'
                                     AND at.REPORT_CREATE_STATUS = 1
                                     AND at.TASK_STATUS = 5
                                     AND at.REPORT_STATUS_ID in (1,3)
                                     AND r.CDU= '" + U + @"' ORDER BY TO_NUMBER(r.AUDIT_RPT_ID) ASC";
                var TaskData = db.Database.SqlQuery<AuditReportDetailsListVM>(sql: sql_1);
                ViewBag.Message = "Audit Report List";
                var R = TaskData.Take(500).ToList();
                return View(R);
            }
        }
        [KAuth(5207)]
        public ActionResult WithoutReportDetailsList()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (T == "10")
            {
                string sql_1 = @"SELECT r.AUDIT_RPT_ID,
                                 r.RPT_SENT_DATE,
                                 r.TASK_ID,
                                 r.AUDIT_RPT_NAME,
                                 l.LOCATION_NAME,
                                 g.COMPANY_GROUP_NAME,
                                 com.COMPANY_NAME,
                                 dep.DEPARTMENT_NAME,
                                 r.WORK_SCOPE,
                                 r.WORK_OPPORTUNITY,
                                 u.USER_NAME,
                                 ct.TASK_CATEGORY_TYPE_NAME,
                                 c.TASK_CATEGORY_NAME,

                                 ds.AUDIT_SAVING_NAME AS DIRECT_SAVING_NAME,
                                 r.DIRECT_SAVING_AMOUNT,
                                 ids.AUDIT_SAVING_NAME AS INDIRECT_SAVING_NAME,
                                 r.INDIRECT_SAVING_AMOUNT,

                                 r.DEMOTION,
                                 r.DEVELOPMENT,
                                 r.JOB_SACK_OUT,
                                 r.TRANSFER,
                                 r.WARNING_LETTER,
                                 r.PENALTY,
                                 r.HOLD_PAYMENT,
                                 r.REMARK,
                                 m.MANAGEMENT_ACTION_NAME,

                                 arf.FU1ST_FOLLOWUP_NUMBER,
                                 r.FIRST_FOLLOWUP_DATE,
                                 arf.FU1ST_FOLLOWUP_DATE,
                                 arf.FU1ST_TOTAL_FINDINGS,
                                 arf.FU1ST_ACTION_FINDINGS,
                                 arf.FU1ST_NO_ACTION_FINDINGS,
                                 arf.FU1ST_ACTION_PERCENTAGE,
                                 arf.FU1ST_RISK_TYPE_NAME,
                                 arf.FU1ST_RISK_MITIGATION_NAME,
                                 arf.FU1ST_RISK_MITIGATION_MARKS,
                                 arf.FU1ST_REPLY_PERSON,
                                 arf.FU1ST_RESPONSIBLE_DEPT_HEAD,
                                 arf.FU1ST_REMARKS,
                                 arf.FU1ST_SIGNIFICANT_GAP,
                                 arf.FU1ST_FU_REQUIREMENT_NAME,

                                 arf.FU2ND_FOLLOWUP_NUMBER,
                                 r.SECOND_FOLLOWUP_DATE,
                                 arf.FU2ND_FOLLOWUP_DATE,
                                 arf.FU2ND_TOTAL_FINDINGS,
                                 arf.FU2ND_ACTION_FINDINGS,
                                 arf.FU2ND_NO_ACTION_FINDINGS,
                                 arf.FU2ND_ACTION_PERCENTAGE,
                                 arf.FU2ND_RISK_TYPE_NAME,
                                 arf.FU2ND_RISK_MITIGATION_NAME,
                                 arf.FU2ND_RISK_MITIGATION_MARKS,
                                 arf.FU2ND_REPLY_PERSON,
                                 arf.FU2ND_RESPONSIBLE_DEPT_HEAD,
                                 arf.FU2ND_REMARKS,
                                 arf.FU2ND_SIGNIFICANT_GAP,
                                 arf.FU2ND_FU_REQUIREMENT_NAME,

                                 arf.FU3RD_FOLLOWUP_NUMBER,
                                 r.THIRD_FOLLOWUP_DATE,
                                 arf.FU3RD_FOLLOWUP_DATE,
                                 arf.FU3RD_TOTAL_FINDINGS,
                                 arf.FU3RD_ACTION_FINDINGS,
                                 arf.FU3RD_NO_ACTION_FINDINGS,
                                 arf.FU3RD_ACTION_PERCENTAGE,
                                 arf.FU3RD_RISK_TYPE_NAME,
                                 arf.FU3RD_RISK_MITIGATION_NAME,
                                 arf.FU3RD_RISK_MITIGATION_MARKS,
                                 arf.FU3RD_REPLY_PERSON,
                                 arf.FU3RD_RESPONSIBLE_DEPT_HEAD,
                                 arf.FU3RD_REMARKS,
                                 arf.FU3RD_SIGNIFICANT_GAP,
                                 arf.FU3RD_FU_REQUIREMENT_NAME

                                FROM AUDIT_REPORT r
                                     LEFT JOIN AUDIT_TASK at ON r.TASK_ID = at.TASK_ID
                                     LEFT JOIN AUDIT_TASK_CATEGORY_TYPE ct ON ct.TASK_CATEGORY_TYPE_ID = r.TASK_CATEGORY_TYPE_ID
                                     LEFT JOIN AUDIT_TASK_CATEGORY c ON c.TASK_CATEGORY_ID = r.TASK_CATEGORY_ID
                                     LEFT JOIN AUDIT_RPT_SAVING_NAME ds ON ds.AUDIT_SAVING_NAME_ID = r.DIRECT_SAVING_TYPE_ID
                                     LEFT JOIN AUDIT_RPT_SAVING_NAME ids ON ids.AUDIT_SAVING_NAME_ID  = r.INDIRECT_SAVING_TYPE_ID 
                                     LEFT JOIN AUDIT_MANAGEMENT_ACTION m ON r.MANAGEMENT_ACTION_ID = m.MANAGEMENT_ACTION_ID
                                     LEFT JOIN A_USERS u ON r.CDU = u.USER_TEXT

                                     LEFT JOIN V_AUDIT_REPORT_FOLLOWUP arf ON arf.AUDIT_RPT_ID = r.AUDIT_RPT_ID


                                     INNER JOIN C_LOCATION l ON at.LOCATION_ID = l.LOCATION_ID
                                     INNER JOIN C_COMPANY_GROUP g ON at.COMPANY_GROUP_ID = g.COMPANY_GROUP_ID
                                     INNER JOIN C_COMPANY com ON at.COMPANY_ID = com.COMPANY_ID
                                     INNER JOIN C_DEPARTMENT dep ON at.DEPARTMENT_ID = dep.DEPARTMENT_ID
                                WHERE     r.ACT = 1
                                     AND r.APPROVE_SENT_STATUS = 'N'
                                     AND at.REPORT_CREATE_STATUS = 1
                                     AND at.TASK_STATUS = 5
                                     AND at.REPORT_STATUS_ID in (2,4)
                                ORDER BY TO_NUMBER (r.AUDIT_RPT_ID) ASC";
                var TaskData = db.Database.SqlQuery<AuditWithoutReportDetailsListVM>(sql: sql_1);
                ViewBag.Message = "Audit Without Report List";
                var R = TaskData.Take(500).ToList();
                return View(R);
            }
            else
            {
                string sql_1 = @"SELECT r.AUDIT_RPT_ID,
                                 r.RPT_SENT_DATE,
                                 r.TASK_ID,
                                 r.AUDIT_RPT_NAME,
                                 l.LOCATION_NAME,
                                 g.COMPANY_GROUP_NAME,
                                 com.COMPANY_NAME,
                                 dep.DEPARTMENT_NAME,
                                 r.WORK_SCOPE,
                                 r.WORK_OPPORTUNITY,
                                 u.USER_NAME,
                                 ct.TASK_CATEGORY_TYPE_NAME,
                                 c.TASK_CATEGORY_NAME,

                                 ds.AUDIT_SAVING_NAME AS DIRECT_SAVING_NAME,
                                 r.DIRECT_SAVING_AMOUNT,
                                 ids.AUDIT_SAVING_NAME AS INDIRECT_SAVING_NAME,
                                 r.INDIRECT_SAVING_AMOUNT,

                                 r.DEMOTION,
                                 r.DEVELOPMENT,
                                 r.JOB_SACK_OUT,
                                 r.TRANSFER,
                                 r.WARNING_LETTER,
                                 r.PENALTY,
                                 r.HOLD_PAYMENT,
                                 r.REMARK,
                                 m.MANAGEMENT_ACTION_NAME,

                                 arf.FU1ST_FOLLOWUP_NUMBER,
                                 r.FIRST_FOLLOWUP_DATE,
                                 arf.FU1ST_FOLLOWUP_DATE,
                                 arf.FU1ST_TOTAL_FINDINGS,
                                 arf.FU1ST_ACTION_FINDINGS,
                                 arf.FU1ST_NO_ACTION_FINDINGS,
                                 arf.FU1ST_ACTION_PERCENTAGE,
                                 arf.FU1ST_RISK_TYPE_NAME,
                                 arf.FU1ST_RISK_MITIGATION_NAME,
                                 arf.FU1ST_RISK_MITIGATION_MARKS,
                                 arf.FU1ST_REPLY_PERSON,
                                 arf.FU1ST_RESPONSIBLE_DEPT_HEAD,
                                 arf.FU1ST_REMARKS,
                                 arf.FU1ST_SIGNIFICANT_GAP,
                                 arf.FU1ST_FU_REQUIREMENT_NAME,

                                 arf.FU2ND_FOLLOWUP_NUMBER,
                                 r.SECOND_FOLLOWUP_DATE,
                                 arf.FU2ND_FOLLOWUP_DATE,
                                 arf.FU2ND_TOTAL_FINDINGS,
                                 arf.FU2ND_ACTION_FINDINGS,
                                 arf.FU2ND_NO_ACTION_FINDINGS,
                                 arf.FU2ND_ACTION_PERCENTAGE,
                                 arf.FU2ND_RISK_TYPE_NAME,
                                 arf.FU2ND_RISK_MITIGATION_NAME,
                                 arf.FU2ND_RISK_MITIGATION_MARKS,
                                 arf.FU2ND_REPLY_PERSON,
                                 arf.FU2ND_RESPONSIBLE_DEPT_HEAD,
                                 arf.FU2ND_REMARKS,
                                 arf.FU2ND_SIGNIFICANT_GAP,
                                 arf.FU2ND_FU_REQUIREMENT_NAME,

                                 arf.FU3RD_FOLLOWUP_NUMBER,
                                 r.THIRD_FOLLOWUP_DATE,
                                 arf.FU3RD_FOLLOWUP_DATE,
                                 arf.FU3RD_TOTAL_FINDINGS,
                                 arf.FU3RD_ACTION_FINDINGS,
                                 arf.FU3RD_NO_ACTION_FINDINGS,
                                 arf.FU3RD_ACTION_PERCENTAGE,
                                 arf.FU3RD_RISK_TYPE_NAME,
                                 arf.FU3RD_RISK_MITIGATION_NAME,
                                 arf.FU3RD_RISK_MITIGATION_MARKS,
                                 arf.FU3RD_REPLY_PERSON,
                                 arf.FU3RD_RESPONSIBLE_DEPT_HEAD,
                                 arf.FU3RD_REMARKS,
                                 arf.FU3RD_SIGNIFICANT_GAP,
                                 arf.FU3RD_FU_REQUIREMENT_NAME

                                FROM AUDIT_REPORT r
                                     LEFT JOIN AUDIT_TASK at ON r.TASK_ID = at.TASK_ID
                                     LEFT JOIN AUDIT_TASK_CATEGORY_TYPE ct ON ct.TASK_CATEGORY_TYPE_ID = r.TASK_CATEGORY_TYPE_ID
                                     LEFT JOIN AUDIT_TASK_CATEGORY c ON c.TASK_CATEGORY_ID = r.TASK_CATEGORY_ID
                                     LEFT JOIN AUDIT_RPT_SAVING_NAME ds ON ds.AUDIT_SAVING_NAME_ID = r.DIRECT_SAVING_TYPE_ID
                                     LEFT JOIN AUDIT_RPT_SAVING_NAME ids ON ids.AUDIT_SAVING_NAME_ID  = r.INDIRECT_SAVING_TYPE_ID 
                                     LEFT JOIN AUDIT_MANAGEMENT_ACTION m ON r.MANAGEMENT_ACTION_ID = m.MANAGEMENT_ACTION_ID
                                     LEFT JOIN A_USERS u ON r.CDU = u.USER_TEXT

                                     LEFT JOIN V_AUDIT_REPORT_FOLLOWUP arf ON arf.AUDIT_RPT_ID = r.AUDIT_RPT_ID


                                     INNER JOIN C_LOCATION l ON at.LOCATION_ID = l.LOCATION_ID
                                     INNER JOIN C_COMPANY_GROUP g ON at.COMPANY_GROUP_ID = g.COMPANY_GROUP_ID
                                     INNER JOIN C_COMPANY com ON at.COMPANY_ID = com.COMPANY_ID
                                     INNER JOIN C_DEPARTMENT dep ON at.DEPARTMENT_ID = dep.DEPARTMENT_ID
                               WHERE     r.ACT = 1
                                     AND r.APPROVE_SENT_STATUS = 'N'
                                     AND at.REPORT_CREATE_STATUS = 1
                                     AND at.TASK_STATUS = 5
                                     AND at.REPORT_STATUS_ID in (2,4)
                                     AND r.CDU= '" + U + @"' ORDER BY TO_NUMBER(r.AUDIT_RPT_ID) ASC";
                var TaskData = db.Database.SqlQuery<AuditWithoutReportDetailsListVM>(sql: sql_1);
                ViewBag.Message = "Audit Without Report List";
                var R = TaskData.Take(500).ToList();
                return View(R);
            }
        }
        public ActionResult DownloadAttachFile(string id)
        {
            var dbDATA = db.AUDIT_TASK_IMG
                .Where(a => a.TASK_ID == id)
                .OrderByDescending(a => a.CDT)
                .ToList();

            if (dbDATA == null || !dbDATA.Any())
            {
                TempData["msg"] = "swal('Oops..','File not found','warning');";
                return Redirect(Request.UrlReferrer.ToString());
            }

            var latestImage = dbDATA.FirstOrDefault();
            if (latestImage == null)
            {
                TempData["msg"] = "swal('Oops..','No latest image found','warning');";
                return Redirect(Request.UrlReferrer.ToString());
            }

            var Upload_Path = latestImage.IMG_PATH;
            var GetFileGUIDname = latestImage.TASK_IMG_ID;
            var GetFileOrgName = latestImage.IMG_NAME;
            var CombinePath = System.IO.Path.Combine(Server.MapPath(Upload_Path), GetFileGUIDname);

            try
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(CombinePath);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, GetFileOrgName);
            }
            catch (Exception ex)
            {
                TempData["msg"] = "swal('Ohh..','Error in File: " + ex.Message + "','error');";
                return Redirect(Request.UrlReferrer.ToString());
            }
        }

        [KAuth(5201)]
        [HttpGet]
        public ActionResult CreateNewReport()
        {
            string U = Session["UserId"].ToString();
            ViewBag.TASK_ID = new SelectList(RP.TaskList(UserId: U), "TASK_ID", "TASK_DETAILS");
            ViewBag.TASK_CATEGORY_TYPE_ID = new SelectList(RP.TaskCategoryTypeList(), "TASK_CATEGORY_TYPE_ID", "TASK_CATEGORY_TYPE_NAME");
            ViewBag.TASK_CATEGORY_ID = new SelectList(RP.TaskAllCategoryList(), "TASK_CATEGORY_ID", "TASK_CATEGORY_NAME");
            ViewBag.RPT_ACTION_ID = new SelectList(RP.AuditActionList(), "REPORT_ACTION_ID", "ACTION_NAME");
            ViewBag.AUDIT_SAVING_TYPE_ID = new SelectList(RP.RptSavingTypeList(), "AUDIT_SAVING_TYPE_ID", "AUDIT_SAVING_TYPE_NAME");
            ViewBag.AUDIT_SAVING_NAME_ID = new SelectList(RP.RptSavingNameList(), "AUDIT_SAVING_NAME_ID", "AUDIT_SAVING_NAME");
            ViewBag.Assist_BY_ID = new SelectList(RP.AuditorList(), "AUDITOR_ID", "AUDITOR_NAME");

            ViewBag.DIRECT_SAVING_TYPE_ID = new SelectList(RP.DirectSavingTypeList(), "AUDIT_SAVING_NAME_ID", "AUDIT_SAVING_NAME");
            ViewBag.INDIRECT_SAVING_TYPE_ID = new SelectList(RP.InDirectSavingTypeList(), "AUDIT_SAVING_NAME_ID", "AUDIT_SAVING_NAME");
            ViewBag.MANAGEMENT_ACTION_ID = new SelectList(RP.ManagementActionList(), "MANAGEMENT_ACTION_ID", "MANAGEMENT_ACTION_NAME");
            ViewBag.RISK_TYPE_ID = new SelectList(RP.RiskTypeList(), "RISK_TYPE_ID", "RISK_TYPE_NAME");



            return View();
        }

        [KAuth(5201)]
        [HttpPost]
        public ActionResult CreateNewReport(AuditReportVM UT)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            if (!ModelState.IsValid)
            {
                // Collect all model state errors
                var errors = ModelState.Where(ms => ms.Value.Errors.Count > 0)
                    .Select(ms => new
                    {
                        Property = ms.Key,
                        Errors = ms.Value.Errors.Select(e => e.ErrorMessage + (e.Exception != null ? $" (Exception: {e.Exception.Message})" : ""))
                    })
                    .ToList();

                // Log errors for debugging purposes
                foreach (var error in errors)
                {
                    foreach (var err in error.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"Property: {error.Property}, Error: {err}");
                    }
                }

                // Return the errors as part of the JSON response
                return Json(new
                {
                    success = false,
                    message = "Model validation failed. Please check the highlighted fields and try again.",
                    errors
                }, JsonRequestBehavior.AllowGet);
            }

            if (ModelState.IsValid)
            {
                try
                {

                    AUDIT_TASK AT = db.AUDIT_TASK.Find(UT.TASK_ID);

                    AT.REPORT_CREATE_STATUS = "1";
                    AT.UDT = DateTime.Now;
                    AT.UDU = U;
                    db.Entry(AT).State = EntityState.Modified;


                    AUDIT_REPORT AR = new AUDIT_REPORT
                    {
                        TASK_ID = UT.TASK_ID,
                        AUDIT_RPT_NAME = UT.AUDIT_RPT_NAME,
                        //RPT_SENT_DATE = UT.RPT_SENT_DATE,
                        ASSIST_BY = UT.ASSIST_BY,
                        WORK_OPPORTUNITY = UT.WORK_OPPORTUNITY,
                        WORK_SCOPE = UT?.WORK_SCOPE ?? 0,
                        CHECK_STATUS = "N",
                        APPROVE_STATUS = "N",
                        APPROVE_SENT_STATUS = "N",
                        //REPORT_STATUS_ID = "1",
                        ACT = true,
                        CDT = DateTime.Now,
                        CDU = U,
                        //AUDIT_RPT_ID = Sequence.GetSeq(31).ToString()
                        DIRECT_SAVING_TYPE_ID = UT.DIRECT_SAVING_TYPE_ID,
                        DIRECT_SAVING_AMOUNT = UT?.DIRECT_SAVING_AMOUNT ?? 0,
                        INDIRECT_SAVING_TYPE_ID = UT.INDIRECT_SAVING_TYPE_ID,
                        INDIRECT_SAVING_AMOUNT = UT?.INDIRECT_SAVING_AMOUNT ?? 0,

                        FIRST_FOLLOWUP_STATUS = "N",
                        SECOND_FOLLOWUP_STATUS = "N",
                        THIRD_FOLLOWUP_STATUS = "N",

                        MANAGEMENT_ACTION_ID = UT.MANAGEMENT_ACTION_ID,
                        TASK_CATEGORY_TYPE_ID = UT.TASK_CATEGORY_TYPE_ID,
                        TASK_CATEGORY_ID = UT.TASK_CATEGORY_ID,
                        RISK_TYPE_ID = UT.RISK_TYPE_ID,

                        DEMOTION = UT.DEMOTION,
                        DEVELOPMENT = UT.DEVELOPMENT,
                        JOB_SACK_OUT = UT.JOB_SACK_OUT,
                        TRANSFER = UT.TRANSFER,
                        WARNING_LETTER = UT.WARNING_LETTER,
                        PENALTY = UT.PENALTY,
                        HOLD_PAYMENT = UT.HOLD_PAYMENT,
                        SUPPLIER_BLACKLIST = UT.SUPPLIER_BLACKLIST,
                        CORRUPTION_IDENTIFIED = UT.CORRUPTION_IDENTIFIED,
                        INEFFICIENCY_IDENTIFIED = UT.INEFFICIENCY_IDENTIFIED,

                        REMARK = UT.REMARK,

                        AUDIT_RPT_ID = Sequence.GetSeqNumber(31).ToString()
                        
                    };

                    if (AT.REPORT_STATUS_ID == "1")
                    {
                        AR.REPORT_STATUS_ID = "1";
                    }else
                    {
                        AR.REPORT_STATUS_ID = "3";
                    }
                    

                    if (UT.RPT_SENT_DATE != null && UT.MANAGEMENT_ACTION_ID != "4" && UT.MANAGEMENT_ACTION_ID != null)
                    {
                        AR.RPT_SENT_DATE = UT.RPT_SENT_DATE;
                        //AR.FIRST_FOLLOWUP_DATE = UT.RPT_SENT_DATE.Value.AddDays(10);
                        //AR.FOLLOWUP_STATUS = "1";
                        var AUDIT_FOLLOWUP_TYPE_DATA = db.AUDIT_FOLLOWUP_TYPE.FirstOrDefault(c => c.FOLLOWUP_TYPE_ID == "1");
                        int FIRST_FOLLOWUP_DAYS = AUDIT_FOLLOWUP_TYPE_DATA.FOLLOWUP_DAYS;
                        AR.FIRST_FOLLOWUP_DATE = UT.RPT_SENT_DATE.Value.AddDays(FIRST_FOLLOWUP_DAYS);
                    }

                    if (UT.RPT_SENT_DATE != null && UT.MANAGEMENT_ACTION_ID == "4")
                    {
                        AR.RPT_SENT_DATE = UT.RPT_SENT_DATE;
                    }

                    db.AUDIT_REPORT.Add(AR);


                    db.SaveChanges();
                   
                        Sequence.GetSeqNew(31).ToString();
                        //Sequence.GetSeqNew(32).ToString();
                    


                    return Json(new { success = true, reportpid = "Audit Report Number: " + AR.AUDIT_RPT_ID }, JsonRequestBehavior.AllowGet);
                }
                catch (DbEntityValidationException e)
                {

                    foreach (var eve in e.EntityValidationErrors)
                    {
                        foreach (var ve in eve.ValidationErrors)
                        {
                            System.Diagnostics.Debug.WriteLine($"Validation error: Property: {ve.PropertyName}, Error: {ve.ErrorMessage}");
                        }
                    }
                    return Json(new { success = false, reportpid = "Validation errors occurred. Check logs for details." }, JsonRequestBehavior.AllowGet);
                }

                catch (DbUpdateException ex)
                {
                    // Get detailed inner exception information
                    var innerException = ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? ex.Message;

                    // Log the full exception for debugging
                    System.Diagnostics.Debug.WriteLine($"DbUpdateException: {innerException}");
                    System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");

                    return Json(new { success = false, reportpid = $"Database update error: {innerException}" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {

                    System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message}");
                    return Json(new { success = false, reportpid = $"Unexpected error: {ex.Message}" }, JsonRequestBehavior.AllowGet);
                }
            }


            return Json(new { success = false, reportpid = "Invalid input data. Audit Report not saved." }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewAuditReportDetails(string id, string Post)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Detail_Data = from d in db.AUDIT_REPORT_DETAIL
                              where d.AUDIT_RPT_ID == id
                              group new { d } by d.AUDIT_RPT_ID into g
                              select new
                              {
                                  AuditRptID = g.Key
                                  //IAmount = g.Sum(x => x.d.INSPD_IRATE * x.d.INSPD_IQTY)
                              };

            ViewBag.AuditRptID = Detail_Data.FirstOrDefault().AuditRptID;
            //ViewBag.IAmount = Detail_Data.FirstOrDefault().IAmount;

            var VI = (from d in db.AUDIT_REPORT_DETAIL
                      join i in db.AUDIT_REPORT_ACTION on d.RPT_ACTION_ID equals i.REPORT_ACTION_ID
                      join ct in db.AUDIT_TASK_CATEGORY_TYPE on d.TASK_CATEGORY_TYPE_ID equals ct.TASK_CATEGORY_TYPE_ID
                      join c in db.AUDIT_TASK_CATEGORY on d.TASK_CATEGORY_ID equals c.TASK_CATEGORY_ID
                      join st in db.AUDIT_SAVING_TYPE on d.AUDIT_SAVING_TYPE_ID equals st.AUDIT_SAVING_TYPE_ID
                      join s in db.AUDIT_RPT_SAVING_NAME on d.AUDIT_SAVING_NAME_ID equals s.AUDIT_SAVING_NAME_ID

                      where d.AUDIT_RPT_ID == id
                      select new AuditReportDetails
                      {
                          AUDIT_RPT_ID = d.AUDIT_RPT_ID,

                          LINE = d.LINE,
                          TASK_CATEGORY_TYPE_NAME = ct.TASK_CATEGORY_TYPE_NAME,
                          TASK_CATEGORY_NAME = c.TASK_CATEGORY_NAME,
                          AUDIT_SAVING_TYPE_NAME = st.AUDIT_SAVING_TYPE_NAME,
                          AUDIT_SAVING_NAME = s.AUDIT_SAVING_NAME,
                          RPT_ACTION_ID = d.RPT_ACTION_ID,
                          RPT_ACTION_NAME = i.ACTION_NAME,
                          REMARK = d.REMARK,
                          VALUE = d.VALUE,
                          SAVING_AMOUNT = d.SAVING_AMOUNT

                      }).ToList();
            return PartialView(VI);
        }

        [KAuth(5201)]
        public ActionResult EditAuditReport(string id)
        {
            string U = Session["UserId"].ToString();
            //ViewBag.TASK_ID = new SelectList(RP.TaskList(UserId: U), "TASK_ID", "TASK_DETAILS");
            
           

            var Editable = db.AUDIT_REPORT.Where(i => i.AUDIT_RPT_ID == id && i.APPROVE_SENT_STATUS == "N" && i.CHECK_STATUS == "N").ToList();
            if (Editable.Count == 0)
            {
                TempData["msg"] = string.Format("swal('Ohh..','Unable to modify Audit Report: {0}' ,'info');", id);
                return RedirectToAction("ReportList");
            }
            AUDIT_REPORT I = db.AUDIT_REPORT.Where(i => i.AUDIT_RPT_ID == id && i.APPROVE_SENT_STATUS == "N" && i.CHECK_STATUS == "N").First();
            AuditReportVM E = new AuditReportVM();
            E.AUDIT_RPT_ID = I.AUDIT_RPT_ID;
            E.AUDIT_RPT_NAME = I.AUDIT_RPT_NAME;
            E.TASK_ID = I.TASK_ID;
            //ViewBag.TASK_ID = new SelectList(RP.ReportTaskList(UserId: U), "TASK_ID", "TASK_DETAILS");
            //ViewBag.TASK_ID = new SelectList(RP.ReportTaskList(UserId: U), "TASK_ID", "TASK_DETAILS");
            //E.TASK_ID = db.AUDIT_TASK.Find(I.TASK_ID).TASK_DETAILS;
            E.RPT_SENT_DATE = I.RPT_SENT_DATE;
            E.WORK_OPPORTUNITY = I.WORK_OPPORTUNITY;
            E.WORK_SCOPE = I.WORK_SCOPE;
            E.ASSIST_BY = I.ASSIST_BY;

            E.DIRECT_AUDIT_SAVING_NAME = I.DIRECT_SAVING_TYPE_ID;
            E.DIRECT_SAVING_AMOUNT = I.DIRECT_SAVING_AMOUNT;
            E.INDIRECT_AUDIT_SAVING_NAME = I.INDIRECT_SAVING_TYPE_ID;
            E.INDIRECT_SAVING_AMOUNT = I.INDIRECT_SAVING_AMOUNT;

            E.MANAGEMENT_ACTION_ID = I.MANAGEMENT_ACTION_ID;
            E.RISK_TYPE_ID = I.RISK_TYPE_ID;

            E.TASK_CATEGORY_TYPE_ID = I.TASK_CATEGORY_TYPE_ID;
            //E.TASK_CATEGORY_TYPE_NAME = db.AUDIT_TASK_CATEGORY_TYPE.Find(I.TASK_CATEGORY_TYPE_ID).TASK_CATEGORY_TYPE_NAME;

            E.TASK_CATEGORY_ID = I.TASK_CATEGORY_ID;
            //E.TASK_CATEGORY_NAME = db.AUDIT_TASK_CATEGORY.Find(I.TASK_CATEGORY_ID).TASK_CATEGORY_NAME;

            E.DEMOTION = I.DEMOTION;
            E.DEVELOPMENT = I.DEVELOPMENT;
            E.JOB_SACK_OUT = I.JOB_SACK_OUT;
            E.TRANSFER = I.TRANSFER;
            E.WARNING_LETTER = I.WARNING_LETTER;
            E.PENALTY = I.PENALTY;
            E.HOLD_PAYMENT = I.HOLD_PAYMENT;
            E.SUPPLIER_BLACKLIST = I.SUPPLIER_BLACKLIST;
            E.CORRUPTION_IDENTIFIED = I.CORRUPTION_IDENTIFIED;
            E.INEFFICIENCY_IDENTIFIED = I.INEFFICIENCY_IDENTIFIED;

            E.REMARK = I.REMARK;

            ViewBag.TASK_CATEGORY_TYPE_ID = new SelectList(RP.TaskCategoryTypeList(), "TASK_CATEGORY_TYPE_ID", "TASK_CATEGORY_TYPE_NAME");
            ViewBag.TASK_CATEGORY_ID = new SelectList(RP.TaskAllCategoryList(), "TASK_CATEGORY_ID", "TASK_CATEGORY_NAME");
            ViewBag.RPT_ACTION_ID = new SelectList(RP.AuditActionList(), "REPORT_ACTION_ID", "ACTION_NAME");
            //ViewBag.AUDIT_SAVING_TYPE_ID = new SelectList(RP.RptSavingTypeList(), "AUDIT_SAVING_TYPE_ID", "AUDIT_SAVING_TYPE_NAME");
            //ViewBag.AUDIT_SAVING_NAME_ID = new SelectList(RP.RptSavingNameList(), "AUDIT_SAVING_NAME_ID", "AUDIT_SAVING_NAME");
            ViewBag.Assist_BY_ID = new SelectList(RP.AuditorList(), "AUDITOR_ID", "AUDITOR_NAME");
            ViewBag.DIRECT_AUDIT_SAVING_NAME = new SelectList(RP.DirectSavingNameList(), "AUDIT_SAVING_NAME_ID", "AUDIT_SAVING_NAME");
            ViewBag.INDIRECT_AUDIT_SAVING_NAME = new SelectList(RP.INDirectSavingNameList(), "AUDIT_SAVING_NAME_ID", "AUDIT_SAVING_NAME");
            ViewBag.MANAGEMENT_ACTION_ID = new SelectList(RP.ManagementActionList(), "MANAGEMENT_ACTION_ID", "MANAGEMENT_ACTION_NAME");
            ViewBag.RISK_TYPE_ID = new SelectList(RP.RiskTypeList(), "RISK_TYPE_ID", "RISK_TYPE_NAME");

            return View(E);
        }

        [KAuth(5201)]
        [HttpPost]
        public ActionResult UpdateAuditReport(AuditReportVM UAR)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { Field = x.Key, Errors = x.Value.Errors.Select(e => e.ErrorMessage).ToList() })
                    .ToList();

                return Json(new { success = false, message = "Validation failed", errors }, JsonRequestBehavior.AllowGet);
            }
            if (ModelState.IsValid)
            {
                AUDIT_REPORT I = db.AUDIT_REPORT.Where(i => i.AUDIT_RPT_ID == UAR.AUDIT_RPT_ID && i.CHECK_STATUS == "N" && i.APPROVE_SENT_STATUS == "N").First();
                I.AUDIT_RPT_NAME = UAR.AUDIT_RPT_NAME;
                if (UAR.RPT_SENT_DATE != null && UAR.MANAGEMENT_ACTION_ID != "4" && UAR.MANAGEMENT_ACTION_ID != null)
                {
                    I.RPT_SENT_DATE = UAR.RPT_SENT_DATE;

                    var AUDIT_FOLLOWUP_TYPE_DATA = db.AUDIT_FOLLOWUP_TYPE.FirstOrDefault(c => c.FOLLOWUP_TYPE_ID == "1");
                    int FIRST_FOLLOWUP_DAYS = AUDIT_FOLLOWUP_TYPE_DATA.FOLLOWUP_DAYS;
                    I.FIRST_FOLLOWUP_DATE = UAR.RPT_SENT_DATE.Value.AddDays(FIRST_FOLLOWUP_DAYS);
                    //I.FOLLOWUP_STATUS = "1";
                }

                if (UAR.RPT_SENT_DATE != null && UAR.MANAGEMENT_ACTION_ID == "4")
                {
                    I.RPT_SENT_DATE = UAR.RPT_SENT_DATE;

                }

                I.WORK_SCOPE = UAR.WORK_SCOPE;
                I.WORK_OPPORTUNITY = UAR.WORK_OPPORTUNITY;

                I.DIRECT_SAVING_TYPE_ID = UAR.DIRECT_SAVING_TYPE_ID;
                I.DIRECT_SAVING_AMOUNT = UAR.DIRECT_SAVING_AMOUNT;
                if (UAR.INDIRECT_SAVING_TYPE_ID != null)
                {
                    I.INDIRECT_SAVING_TYPE_ID = UAR.INDIRECT_SAVING_TYPE_ID;
                }
               
                I.INDIRECT_SAVING_AMOUNT = UAR.INDIRECT_SAVING_AMOUNT;

                I.ASSIST_BY = UAR.ASSIST_BY;

                if (UAR.TASK_CATEGORY_TYPE_ID != null)
                {
                    I.TASK_CATEGORY_TYPE_ID = UAR.TASK_CATEGORY_TYPE_ID;
                }

                if (UAR.TASK_CATEGORY_ID != null)
                {
                    I.TASK_CATEGORY_ID = UAR.TASK_CATEGORY_ID;
                }

                if (UAR.MANAGEMENT_ACTION_ID != null)
                {
                    I.MANAGEMENT_ACTION_ID = UAR.MANAGEMENT_ACTION_ID;
                }

                if (UAR.RISK_TYPE_ID != null)
                {
                    I.RISK_TYPE_ID = UAR.RISK_TYPE_ID;
                }

                I.DEMOTION = UAR.DEMOTION;
                I.DEVELOPMENT = UAR.DEVELOPMENT;
                I.JOB_SACK_OUT = UAR.JOB_SACK_OUT;
                I.TRANSFER = UAR.TRANSFER;
                I.WARNING_LETTER = UAR.WARNING_LETTER;
                I.PENALTY = UAR.PENALTY;
                I.HOLD_PAYMENT = UAR.HOLD_PAYMENT;
                I.SUPPLIER_BLACKLIST = UAR.SUPPLIER_BLACKLIST;
                I.CORRUPTION_IDENTIFIED = UAR.CORRUPTION_IDENTIFIED;
                I.INEFFICIENCY_IDENTIFIED = UAR.INEFFICIENCY_IDENTIFIED;

                I.REMARK = UAR.REMARK;

                I.UDT = DateTime.Now;
                I.UDU = U;
                db.Entry(I).State = EntityState.Modified;
     
                db.SaveChanges();
                return Json(new { success = true, reportid = "Audit Report modified: " + UAR.AUDIT_RPT_ID }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, reportid = "Unable to modify Audit Report: " + UAR.AUDIT_RPT_ID }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AttachFileAudit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.TRN_ID = id;
            return PartialView();
        }

        [HttpPost]
        public ActionResult AttachFileAudit(string TRN_ID, IEnumerable<HttpPostedFileBase> files)
        {
            if (files != null)
            {
                var Upload_Path = Sequence.GetImgPath(109);

                foreach (var file in files)
                {
                    if (file == null) continue;

                    if (file.ContentLength <= 0 || file.ContentLength > 21000000)
                    {
                        continue;
                    }

                    string fname;

                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split('\\');
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }

                    AUDIT_REPORT_IMG ARI = new AUDIT_REPORT_IMG();

                    ARI.AUDIT_IMG_ID = Guid.NewGuid().ToString();
                    ARI.IMG_NAME = fname;
                    ARI.IMG_PATH = Upload_Path;
                    ARI.AUDIT_REPORT_ID = TRN_ID;
                    ARI.CDT = System.DateTime.Now;

                    db.AUDIT_REPORT_IMG.Add(ARI);

                    var path = System.IO.Path.Combine(Server.MapPath(ARI.IMG_PATH), ARI.AUDIT_IMG_ID);

                    file.SaveAs(path);
                }

                db.SaveChanges();

                TempData["msg"] = "swal('Success','Files Uploaded Successfully','success');";
                return Redirect(Request.UrlReferrer.ToString());
            }

            TempData["msg"] = "swal('Error','File upload failed','warning');";
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpGet]
        public ActionResult ViewAuditAttachFile(string id)
        {
            var files = db.AUDIT_REPORT_IMG
                .Where(x => x.AUDIT_REPORT_ID == id)
                .OrderByDescending(x => x.CDT)
                .Select(x => new PM.ModelsV.AuditReportAttachmentListVM
                {
                    AUDIT_IMG_ID = x.AUDIT_IMG_ID,
                    IMG_NAME = x.IMG_NAME,
                    CDT = x.CDT
                }).ToList();

            return PartialView(files);
        }

        public ActionResult DownloadFile(string id)
        {
            var file = db.AUDIT_REPORT_IMG.FirstOrDefault(x => x.AUDIT_IMG_ID == id);

            if (file == null)
                return HttpNotFound();

            var path = System.IO.Path.Combine(Server.MapPath(file.IMG_PATH), file.AUDIT_IMG_ID);

            return File(path, "application/octet-stream", file.IMG_NAME);
        }




        //public ActionResult DownloadAuditReportAttachFile(string id)
        //{
        //    var dbDATA = db.AUDIT_REPORT_IMG
        //        .Where(a => a.AUDIT_REPORT_ID == id)
        //        .OrderByDescending(a => a.CDT)
        //        .ToList();

        //    if (dbDATA == null || !dbDATA.Any())
        //    {
        //        TempData["msg"] = "swal('Oops..','File not found','warning');";
        //        return Redirect(Request.UrlReferrer.ToString());
        //    }

        //    var latestImage = dbDATA.FirstOrDefault();
        //    if (latestImage == null)
        //    {
        //        TempData["msg"] = "swal('Oops..','No latest image found','warning');";
        //        return Redirect(Request.UrlReferrer.ToString());
        //    }

        //    var Upload_Path = latestImage.IMG_PATH;
        //    var GetFileGUIDname = latestImage.AUDIT_IMG_ID;
        //    var GetFileOrgName = latestImage.IMG_NAME;
        //    var CombinePath = System.IO.Path.Combine(Server.MapPath(Upload_Path), GetFileGUIDname);

        //    try
        //    {
        //        byte[] fileBytes = System.IO.File.ReadAllBytes(CombinePath);
        //        return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, GetFileOrgName);
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["msg"] = "swal('Ohh..','Error in File: " + ex.Message + "','error');";
        //        return Redirect(Request.UrlReferrer.ToString());
        //    }
        //}

        public ActionResult DownloadAuditReportAttachFile(string id)
        {
            var dbDATA = db.AUDIT_REPORT_IMG
                .Where(a => a.AUDIT_REPORT_ID == id)
                .OrderByDescending(a => a.CDT)
                .ToList();

            if (dbDATA == null || !dbDATA.Any())
            {
                TempData["msg"] = "swal('Oops..','No files found','warning');";
                return Redirect(Request.UrlReferrer.ToString());
            }

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var zipArchive = new System.IO.Compression.ZipArchive(memoryStream, System.IO.Compression.ZipArchiveMode.Create, true))
                    {
                        foreach (var image in dbDATA)
                        {
                            var filePath = Path.Combine(Server.MapPath(image.IMG_PATH), image.AUDIT_IMG_ID);

                            if (System.IO.File.Exists(filePath))
                            {
                                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                                // Ensure unique filename in zip by using AUDIT_IMG_ID as prefix
                                string uniqueFileNameInZip = $"{image.AUDIT_IMG_ID}_{image.IMG_NAME}";

                                var entry = zipArchive.CreateEntry(uniqueFileNameInZip);

                                using (var entryStream = entry.Open())
                                {
                                    entryStream.Write(fileBytes, 0, fileBytes.Length);
                                }
                            }
                        }
                    }

                    memoryStream.Position = 0;
                    return File(memoryStream.ToArray(), "application/zip", "AuditReportFiles.zip");
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "swal('Error','Unable to download files: " + ex.Message + "','error');";
                return Redirect(Request.UrlReferrer.ToString());
            }
        }



        [KAuth(5205)]
        [HttpGet]
        public ActionResult CreateRptForCompletedWithoutRpt()
        {
            string U = Session["UserId"].ToString();
            ViewBag.TASK_ID = new SelectList(RP.TaskListWithoutRpt(UserId: U), "TASK_ID", "TASK_DETAILS");
            ViewBag.TASK_CATEGORY_TYPE_ID = new SelectList(RP.TaskCategoryTypeList(), "TASK_CATEGORY_TYPE_ID", "TASK_CATEGORY_TYPE_NAME");
            ViewBag.TASK_CATEGORY_ID = new SelectList(RP.TaskAllCategoryList(), "TASK_CATEGORY_ID", "TASK_CATEGORY_NAME");
            ViewBag.RPT_ACTION_ID = new SelectList(RP.AuditActionList(), "REPORT_ACTION_ID", "ACTION_NAME");
            ViewBag.AUDIT_SAVING_TYPE_ID = new SelectList(RP.RptSavingTypeList(), "AUDIT_SAVING_TYPE_ID", "AUDIT_SAVING_TYPE_NAME");
            ViewBag.AUDIT_SAVING_NAME_ID = new SelectList(RP.RptSavingNameList(), "AUDIT_SAVING_NAME_ID", "AUDIT_SAVING_NAME");
            ViewBag.Assist_BY_ID = new SelectList(RP.AuditorList(), "AUDITOR_ID", "AUDITOR_NAME");

            ViewBag.DIRECT_SAVING_TYPE_ID = new SelectList(RP.DirectSavingTypeList(), "AUDIT_SAVING_NAME_ID", "AUDIT_SAVING_NAME");
            ViewBag.INDIRECT_SAVING_TYPE_ID = new SelectList(RP.InDirectSavingTypeList(), "AUDIT_SAVING_NAME_ID", "AUDIT_SAVING_NAME");
            ViewBag.MANAGEMENT_ACTION_ID = new SelectList(RP.ManagementActionList(), "MANAGEMENT_ACTION_ID", "MANAGEMENT_ACTION_NAME");


            return View();
        }

        [KAuth(5205)]
        [HttpPost]
        public ActionResult CreateRptForCompletedWithoutRpt(AuditReportVM UT)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            if (!ModelState.IsValid)
            {
                // Collect all model state errors
                var errors = ModelState.Where(ms => ms.Value.Errors.Count > 0)
                    .Select(ms => new
                    {
                        Property = ms.Key,
                        Errors = ms.Value.Errors.Select(e => e.ErrorMessage + (e.Exception != null ? $" (Exception: {e.Exception.Message})" : ""))
                    })
                    .ToList();

                // Log errors for debugging purposes
                foreach (var error in errors)
                {
                    foreach (var err in error.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"Property: {error.Property}, Error: {err}");
                    }
                }

                // Return the errors as part of the JSON response
                return Json(new
                {
                    success = false,
                    message = "Model validation failed. Please check the highlighted fields and try again.",
                    errors
                }, JsonRequestBehavior.AllowGet);
            }

            if (ModelState.IsValid)
            {
                try
                {

                    AUDIT_TASK AT = db.AUDIT_TASK.Find(UT.TASK_ID);

                    AT.REPORT_CREATE_STATUS = "1";
                    AT.UDT = DateTime.Now;
                    AT.UDU = U;
                    db.Entry(AT).State = EntityState.Modified;


                    AUDIT_REPORT AR = new AUDIT_REPORT
                    {
                        TASK_ID = UT.TASK_ID,
                        AUDIT_RPT_NAME = UT.AUDIT_RPT_NAME,
                        //RPT_SENT_DATE = UT.RPT_SENT_DATE,
                        ASSIST_BY = UT.ASSIST_BY,
                        WORK_OPPORTUNITY = UT.WORK_OPPORTUNITY,
                        WORK_SCOPE = UT?.WORK_SCOPE ?? 0,
                        CHECK_STATUS = "N",
                        APPROVE_STATUS = "N",
                        APPROVE_SENT_STATUS = "N",
                        //REPORT_STATUS_ID = "2",
                        ACT = true,
                        CDT = DateTime.Now,
                        CDU = U,
                        //AUDIT_RPT_ID = Sequence.GetSeq(31).ToString()
                        DIRECT_SAVING_TYPE_ID = UT.DIRECT_SAVING_TYPE_ID,
                        DIRECT_SAVING_AMOUNT = UT?.DIRECT_SAVING_AMOUNT ?? 0,
                        INDIRECT_SAVING_TYPE_ID = UT.INDIRECT_SAVING_TYPE_ID,
                        INDIRECT_SAVING_AMOUNT = UT?.INDIRECT_SAVING_AMOUNT ?? 0,

                        MANAGEMENT_ACTION_ID = UT.MANAGEMENT_ACTION_ID,
                        TASK_CATEGORY_TYPE_ID = UT.TASK_CATEGORY_TYPE_ID,
                        TASK_CATEGORY_ID = UT.TASK_CATEGORY_ID,

                        DEMOTION = UT.DEMOTION,
                        DEVELOPMENT = UT.DEVELOPMENT,
                        JOB_SACK_OUT = UT.JOB_SACK_OUT,
                        TRANSFER = UT.TRANSFER,
                        WARNING_LETTER = UT.WARNING_LETTER,
                        PENALTY = UT.PENALTY,
                        HOLD_PAYMENT = UT.HOLD_PAYMENT,
                        SUPPLIER_BLACKLIST = UT.SUPPLIER_BLACKLIST,

                        REMARK = UT.REMARK,

                        AUDIT_RPT_ID = Sequence.GetSeqNumber(35).ToString()

                    };

                    if (AT.REPORT_STATUS_ID == "2")
                    {
                        AR.REPORT_STATUS_ID = "2";
                    }
                    else
                    {
                        AR.REPORT_STATUS_ID = "4";
                    }

                    //int CounterId = 1;
                    //foreach (var item in UT.ReportActionDetails)
                    //{
                    //    AUDIT_REPORT_DETAIL D = new AUDIT_REPORT_DETAIL
                    //    {
                    //        //AUDIT_RPT_DETAIL_ID = Sequence.GetSeq(32).ToString(),
                    //        AUDIT_RPT_DETAIL_ID = Sequence.GetSeqNumber(32).ToString(),
                    //        AUDIT_RPT_ID = AR.AUDIT_RPT_ID,
                    //        TASK_CATEGORY_TYPE_ID = item.TASK_CATEGORY_TYPE_ID,
                    //        TASK_CATEGORY_ID = item.TASK_CATEGORY_ID,
                    //        AUDIT_SAVING_TYPE_ID = item.AUDIT_SAVING_TYPE_ID,
                    //        AUDIT_SAVING_NAME_ID = item.AUDIT_SAVING_NAME_ID,
                    //        RPT_ACTION_ID = item.RPT_ACTION_ID,
                    //        SAVING_AMOUNT = item?.SAVING_AMOUNT ?? 0,
                    //        REMARK = item.REMARK,
                    //        VALUE = item.VALUE,

                    //        DIRECT_SAVING_TYPE_ID = item.DIRECT_SAVING_TYPE_ID,
                    //        DIRECT_SAVING_AMOUNT = item?.DIRECT_SAVING_AMOUNT ?? 0,
                    //        INDIRECT_SAVING_TYPE_ID = item.INDIRECT_SAVING_TYPE_ID,
                    //        INDIRECT_SAVING_AMOUNT = item?.INDIRECT_SAVING_AMOUNT ?? 0,

                    //        DEMOTION = item?.DEMOTION,
                    //        DEVELOPMENT = item?.DEVELOPMENT,
                    //        JOB_SACK_OUT = item?.JOB_SACK_OUT,
                    //        TRANSFER = item?.TRANSFER,
                    //        WARNING_LETTER = item?.WARNING_LETTER,
                    //        PENALTY = item?.PENALTY,
                    //        LINE = Convert.ToByte(CounterId),
                    //        ACT = true
                    //    };
                    //    CounterId++;
                    //    db.AUDIT_REPORT_DETAIL.Add(D);
                    //}

                    db.AUDIT_REPORT.Add(AR);


                    db.SaveChanges();

                    Sequence.GetSeqNew(35).ToString();
                    //Sequence.GetSeqNew(32).ToString();



                    return Json(new { success = true, reportpid = "Audit Report Number: " + AR.AUDIT_RPT_ID }, JsonRequestBehavior.AllowGet);
                }
                catch (DbEntityValidationException e)
                {

                    foreach (var eve in e.EntityValidationErrors)
                    {
                        foreach (var ve in eve.ValidationErrors)
                        {
                            System.Diagnostics.Debug.WriteLine($"Validation error: Property: {ve.PropertyName}, Error: {ve.ErrorMessage}");
                        }
                    }
                    return Json(new { success = false, reportpid = "Validation errors occurred. Check logs for details." }, JsonRequestBehavior.AllowGet);
                }

                catch (DbUpdateException ex)
                {
                    // Get detailed inner exception information
                    var innerException = ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? ex.Message;

                    // Log the full exception for debugging
                    System.Diagnostics.Debug.WriteLine($"DbUpdateException: {innerException}");
                    System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");

                    return Json(new { success = false, reportpid = $"Database update error: {innerException}" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {

                    System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message}");
                    return Json(new { success = false, reportpid = $"Unexpected error: {ex.Message}" }, JsonRequestBehavior.AllowGet);
                }
            }


            return Json(new { success = false, reportpid = "Invalid input data. Audit Report not saved." }, JsonRequestBehavior.AllowGet);
        }

        [KAuth(5206)]
        public ActionResult AuditWithoutReportList()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (T == "10")
            {
                string sql_1 = @"SELECT 
                                r.AUDIT_RPT_ID,
                                r.RPT_SENT_DATE,
                                r.TASK_ID,
                                r.AUDIT_RPT_NAME,
                                l.LOCATION_NAME,
                                g.COMPANY_GROUP_NAME,
                                com.COMPANY_NAME,
                                dep.DEPARTMENT_NAME,
                                r.WORK_SCOPE,
                                r.WORK_OPPORTUNITY,
                                u.USER_NAME
                                from AUDIT_REPORT r
                                LEFT JOIN AUDIT_TASK at ON r.TASK_ID = at.TASK_ID
                                LEFT JOIN A_USERS u ON r.CDU = u.USER_TEXT
                                INNER JOIN C_LOCATION l ON at.LOCATION_ID = l.LOCATION_ID
                                INNER JOIN C_COMPANY_GROUP g ON at.COMPANY_GROUP_ID = g.COMPANY_GROUP_ID
                                INNER JOIN C_COMPANY com ON at.COMPANY_ID = com.COMPANY_ID
                                INNER JOIN C_DEPARTMENT dep ON at.DEPARTMENT_ID = dep.DEPARTMENT_ID
                                WHERE r.ACT = 1 AND r.APPROVE_SENT_STATUS='N' AND at.REPORT_CREATE_STATUS=1 AND at.TASK_STATUS=5 AND at.REPORT_STATUS_ID in (2,4) ORDER BY TO_NUMBER(r.AUDIT_RPT_ID) ASC";
                var TaskData = db.Database.SqlQuery<AuditReportListVM>(sql: sql_1);
                ViewBag.Message = "Without Audit Report List";
                var R = TaskData.Take(500).ToList();
                return View(R);
            }
            else
            {
                string sql_1 = @"SELECT 
                                r.AUDIT_RPT_ID,
                                r.RPT_SENT_DATE,
                                r.TASK_ID,
                                r.AUDIT_RPT_NAME,
                                l.LOCATION_NAME,
                                g.COMPANY_GROUP_NAME,
                                com.COMPANY_NAME,
                                dep.DEPARTMENT_NAME,
                                r.WORK_SCOPE,
                                r.WORK_OPPORTUNITY,
                                u.USER_NAME
                                from AUDIT_REPORT r
                                LEFT JOIN AUDIT_TASK at ON r.TASK_ID = at.TASK_ID
                                LEFT JOIN A_USERS u ON r.CDU = u.USER_TEXT
                                INNER JOIN C_LOCATION l ON at.LOCATION_ID = l.LOCATION_ID
                                INNER JOIN C_COMPANY_GROUP g ON at.COMPANY_GROUP_ID = g.COMPANY_GROUP_ID
                                INNER JOIN C_COMPANY com ON at.COMPANY_ID = com.COMPANY_ID
                                INNER JOIN C_DEPARTMENT dep ON at.DEPARTMENT_ID = dep.DEPARTMENT_ID
                                WHERE r.ACT = 1 AND r.APPROVE_SENT_STATUS='N' AND at.REPORT_CREATE_STATUS=1 AND at.TASK_STATUS=5 AND at.REPORT_STATUS_ID in (2,4) AND r.CDU= '" + U + @"' ORDER BY TO_NUMBER(r.AUDIT_RPT_ID) ASC";
                var TaskData = db.Database.SqlQuery<AuditReportListVM>(sql: sql_1);
                ViewBag.Message = "Without Audit Report List";
                var R = TaskData.Take(500).ToList();
                return View(R);
            }
        }

        [KAuth(5206)]
        public ActionResult EditWithoutAuditReport(string id)
        {
            string U = Session["UserId"].ToString();
            //ViewBag.TASK_ID = new SelectList(RP.TaskList(UserId: U), "TASK_ID", "TASK_DETAILS");



            var Editable = db.AUDIT_REPORT.Where(i => i.AUDIT_RPT_ID == id && i.APPROVE_SENT_STATUS == "N" && i.CHECK_STATUS == "N").ToList();
            if (Editable.Count == 0)
            {
                TempData["msg"] = string.Format("swal('Ohh..','Unable to modify Audit Report: {0}' ,'info');", id);
                return RedirectToAction("ReportList");
            }
            AUDIT_REPORT I = db.AUDIT_REPORT.Where(i => i.AUDIT_RPT_ID == id && i.APPROVE_SENT_STATUS == "N" && i.CHECK_STATUS == "N").First();
            AuditReportVM E = new AuditReportVM();
            E.AUDIT_RPT_ID = I.AUDIT_RPT_ID;
            E.AUDIT_RPT_NAME = I.AUDIT_RPT_NAME;
            E.TASK_ID = I.TASK_ID;
            E.RPT_SENT_DATE = I.RPT_SENT_DATE;
            E.WORK_OPPORTUNITY = I.WORK_OPPORTUNITY;
            E.WORK_SCOPE = I.WORK_SCOPE;
            E.ASSIST_BY = I.ASSIST_BY;

            E.DIRECT_AUDIT_SAVING_NAME = I.DIRECT_SAVING_TYPE_ID;
            E.DIRECT_SAVING_AMOUNT = I.DIRECT_SAVING_AMOUNT;
            E.INDIRECT_AUDIT_SAVING_NAME = I.INDIRECT_SAVING_TYPE_ID;
            E.INDIRECT_SAVING_AMOUNT = I.INDIRECT_SAVING_AMOUNT;

            E.MANAGEMENT_ACTION_ID = I.MANAGEMENT_ACTION_ID;

            E.TASK_CATEGORY_TYPE_ID = I.TASK_CATEGORY_TYPE_ID;
            //E.TASK_CATEGORY_TYPE_NAME = db.AUDIT_TASK_CATEGORY_TYPE.Find(I.TASK_CATEGORY_TYPE_ID).TASK_CATEGORY_TYPE_NAME;

            E.TASK_CATEGORY_ID = I.TASK_CATEGORY_ID;
            //E.TASK_CATEGORY_NAME = db.AUDIT_TASK_CATEGORY.Find(I.TASK_CATEGORY_ID).TASK_CATEGORY_NAME;

            E.DEMOTION = I.DEMOTION;
            E.DEVELOPMENT = I.DEVELOPMENT;
            E.JOB_SACK_OUT = I.JOB_SACK_OUT;
            E.TRANSFER = I.TRANSFER;
            E.WARNING_LETTER = I.WARNING_LETTER;
            E.PENALTY = I.PENALTY;
            E.HOLD_PAYMENT = I.HOLD_PAYMENT;
            E.SUPPLIER_BLACKLIST = I.SUPPLIER_BLACKLIST;

            E.REMARK = I.REMARK;

            //List<ReportActionDetails> ED = new List<ReportActionDetails>();
            //List<string> Exist = new List<string>();
            //foreach (var item in I.AUDIT_REPORT_DETAIL)
            //{
            //    ReportActionDetails D = new ReportActionDetails();

            //    D.TASK_CATEGORY_TYPE_ID = item.TASK_CATEGORY_TYPE_ID;
            //    D.TASK_CATEGORY_TYPE_NAME = db.AUDIT_TASK_CATEGORY_TYPE.Find(item.TASK_CATEGORY_TYPE_ID).TASK_CATEGORY_TYPE_NAME;

            //    D.TASK_CATEGORY_ID = item.TASK_CATEGORY_ID;
            //    D.TASK_CATEGORY_NAME = db.AUDIT_TASK_CATEGORY.Find(item.TASK_CATEGORY_ID).TASK_CATEGORY_NAME;

            //    D.DEMOTION = item.DEMOTION;
            //    D.DEVELOPMENT = item.DEVELOPMENT;
            //    D.JOB_SACK_OUT = item.JOB_SACK_OUT;
            //    D.TRANSFER = item.TRANSFER;
            //    D.WARNING_LETTER = item.WARNING_LETTER;
            //    D.PENALTY = item.PENALTY;

            //    D.REMARK = item.REMARK;
            //    ED.Add(D);
            //    Exist.Add(item.RPT_ACTION_ID);
            //}
            //E.ReportActionDetails = ED;

            ViewBag.TASK_CATEGORY_TYPE_ID = new SelectList(RP.TaskCategoryTypeList(), "TASK_CATEGORY_TYPE_ID", "TASK_CATEGORY_TYPE_NAME");
            ViewBag.TASK_CATEGORY_ID = new SelectList(RP.TaskAllCategoryList(), "TASK_CATEGORY_ID", "TASK_CATEGORY_NAME");
            ViewBag.RPT_ACTION_ID = new SelectList(RP.AuditActionList(), "REPORT_ACTION_ID", "ACTION_NAME");

            ViewBag.Assist_BY_ID = new SelectList(RP.AuditorList(), "AUDITOR_ID", "AUDITOR_NAME");
            ViewBag.DIRECT_AUDIT_SAVING_NAME = new SelectList(RP.DirectSavingNameList(), "AUDIT_SAVING_NAME_ID", "AUDIT_SAVING_NAME");
            ViewBag.INDIRECT_AUDIT_SAVING_NAME = new SelectList(RP.INDirectSavingNameList(), "AUDIT_SAVING_NAME_ID", "AUDIT_SAVING_NAME");
            ViewBag.MANAGEMENT_ACTION_ID = new SelectList(RP.ManagementActionList(), "MANAGEMENT_ACTION_ID", "MANAGEMENT_ACTION_NAME");

            return View(E);
        }

        [KAuth(5206)]
        [HttpPost]
        public ActionResult UpdateWithoutAuditReport(AuditReportVM UAR)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { Field = x.Key, Errors = x.Value.Errors.Select(e => e.ErrorMessage).ToList() })
                    .ToList();

                return Json(new { success = false, message = "Validation failed", errors }, JsonRequestBehavior.AllowGet);
            }
            if (ModelState.IsValid)
            {
                AUDIT_REPORT I = db.AUDIT_REPORT.Where(i => i.AUDIT_RPT_ID == UAR.AUDIT_RPT_ID && i.CHECK_STATUS == "N" && i.APPROVE_SENT_STATUS == "N").First();
                I.AUDIT_RPT_NAME = UAR.AUDIT_RPT_NAME;
 
                I.WORK_SCOPE = UAR.WORK_SCOPE;
                I.WORK_OPPORTUNITY = UAR.WORK_OPPORTUNITY;

                I.DIRECT_SAVING_TYPE_ID = UAR.DIRECT_SAVING_TYPE_ID;
                I.DIRECT_SAVING_AMOUNT = UAR.DIRECT_SAVING_AMOUNT;
                if (UAR.INDIRECT_SAVING_TYPE_ID != null)
                {
                    I.INDIRECT_SAVING_TYPE_ID = UAR.INDIRECT_SAVING_TYPE_ID;
                }

                I.INDIRECT_SAVING_AMOUNT = UAR.INDIRECT_SAVING_AMOUNT;

                I.ASSIST_BY = UAR.ASSIST_BY;

                if (UAR.TASK_CATEGORY_TYPE_ID != null)
                {
                    I.TASK_CATEGORY_TYPE_ID = UAR.TASK_CATEGORY_TYPE_ID;
                }

                if (UAR.TASK_CATEGORY_ID != null)
                {
                    I.TASK_CATEGORY_ID = UAR.TASK_CATEGORY_ID;
                }

                if (UAR.MANAGEMENT_ACTION_ID != null)
                {
                    I.MANAGEMENT_ACTION_ID = UAR.MANAGEMENT_ACTION_ID;
                }

                I.DEMOTION = UAR.DEMOTION;
                I.DEVELOPMENT = UAR.DEVELOPMENT;
                I.JOB_SACK_OUT = UAR.JOB_SACK_OUT;
                I.TRANSFER = UAR.TRANSFER;
                I.WARNING_LETTER = UAR.WARNING_LETTER;
                I.PENALTY = UAR.PENALTY;
                I.HOLD_PAYMENT = UAR.HOLD_PAYMENT;
                I.SUPPLIER_BLACKLIST = UAR.SUPPLIER_BLACKLIST;

                I.REMARK = UAR.REMARK;

                I.UDT = DateTime.Now;
                I.UDU = U;
                db.Entry(I).State = EntityState.Modified;
                //List<AUDIT_REPORT_DETAIL> DelD = db.AUDIT_REPORT_DETAIL.Where(d => d.AUDIT_RPT_ID == UAR.AUDIT_RPT_ID).ToList();
                //db.AUDIT_REPORT_DETAIL.RemoveRange(DelD);
                //int CounterId = 1;
                //foreach (var item in UAR.ReportActionDetails)
                //{
                //    AUDIT_REPORT_DETAIL D = new AUDIT_REPORT_DETAIL();

                //    D.AUDIT_RPT_DETAIL_ID = Sequence.GetSeq(32).ToString();
                //    D.AUDIT_RPT_ID = UAR.AUDIT_RPT_ID;
                //    D.TASK_CATEGORY_TYPE_ID = item.TASK_CATEGORY_TYPE_ID;
                //    D.TASK_CATEGORY_ID = item.TASK_CATEGORY_ID;
   

                //    D.DEMOTION = item.DEMOTION;
                //    D.DEVELOPMENT = item.DEVELOPMENT;
                //    D.JOB_SACK_OUT = item.JOB_SACK_OUT;
                //    D.TRANSFER = item.TRANSFER;
                //    D.WARNING_LETTER = item.WARNING_LETTER;
                //    D.PENALTY = item.PENALTY;

                //    D.REMARK = item.REMARK;
 


                //    D.LINE = Convert.ToByte(CounterId);
                //    D.ACT = true;

                //    CounterId = CounterId + 1;

                //    db.AUDIT_REPORT_DETAIL.Add(D);
                //}
                db.SaveChanges();
                return Json(new { success = true, reportid = "Audit Report modified: " + UAR.AUDIT_RPT_ID }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, reportid = "Unable to modify Audit Report: " + UAR.AUDIT_RPT_ID }, JsonRequestBehavior.AllowGet);
        }

        [KAuth(5301)]
        [HttpGet]
        public ActionResult CreateFirstFollowUpReport()
        {
            string U = Session["UserId"].ToString();
            ViewBag.AUDIT_RPT_ID = new SelectList(RP.SearchFirstFollowUpReportList(UserId: U), "AUDIT_RPT_ID", "AUDIT_RPT_NAME");
            ViewBag.RISK_MITIGATION_ID = new SelectList(RP.RiskMitigationList(), "RISK_MITIGATION_ID", "RISK_MITIGATION_NAME");
            ViewBag.RISK_TYPE_ID = new SelectList(RP.RiskTypeList(), "RISK_TYPE_ID", "RISK_TYPE_NAME");
            ViewBag.FOLLOWUP_REQUIREMENT_ID = new SelectList(RP.FollowUpRequirementList(), "FOLLOWUP_REQUIREMENT_ID", "FOLLOWUP_REQUIREMENT_NAME");

            ViewBag.FollowUpType = "FirstFollowUp";
            return View("CreateFirstFollowUpReport");
        }
 
        [KAuth(5301)]
        [HttpPost]
        public ActionResult CreateFirstFollowUpReport(AuditFollowUpReportVM AFRVM)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            if (ModelState.IsValid)
            {
                    AUDIT_REPORT AR = db.AUDIT_REPORT.Find(AFRVM.AUDIT_RPT_ID);

                

                if (AFRVM.FOLLOWUP_REQUIREMENT_ID == "1" && AFRVM.FOLLOWUP_DATE != null)
                    {
                    var AUDIT_FOLLOWUP_TYPE_DATA = db.AUDIT_FOLLOWUP_TYPE.FirstOrDefault(c => c.FOLLOWUP_TYPE_ID == "2");
                    int SECOND_FOLLOWUP_DAYS = AUDIT_FOLLOWUP_TYPE_DATA.FOLLOWUP_DAYS; 
                    AR.SECOND_FOLLOWUP_DATE = AFRVM.FOLLOWUP_DATE.Value.AddDays(SECOND_FOLLOWUP_DAYS);
                    }

                    if (AFRVM.FOLLOWUP_REQUIREMENT_ID == "2")
                    {
                        AR.SECOND_FOLLOWUP_STATUS = "NO NEED";
                        AR.THIRD_FOLLOWUP_STATUS = "NO NEED";
                    }
                    AR.FIRST_FOLLOWUP_STATUS = "Y";
                    AR.UDT = DateTime.Now;
                    AR.UDU = U;
                    db.Entry(AR).State = EntityState.Modified;

                AUDIT_REPORT_FOLLOWUP AFR = new AUDIT_REPORT_FOLLOWUP
                    {
                        AUDIT_RPT_ID = AFRVM.AUDIT_RPT_ID,
                        FOLLOWUP_NUMBER = 1,
                        FOLLOWUP_DATE = AFRVM.FOLLOWUP_DATE,
                        TOTAL_FINDINGS = AFRVM.TOTAL_FINDINGS,

                        ACTION_FINDINGS = AFRVM.ACTION_FINDINGS,
                        NO_ACTION_FINDINGS = (AFRVM.TOTAL_FINDINGS - AFRVM.ACTION_FINDINGS),
                        ACTION_PERCENTAGE = AFRVM.TOTAL_FINDINGS == 0 ? 0 : ((decimal)AFRVM.ACTION_FINDINGS / AFRVM.TOTAL_FINDINGS) * 100,
                        RISK_TYPE_ID = AFRVM.RISK_TYPE_ID,
                        RISK_MITIGATION_ID = AFRVM.RISK_MITIGATION_ID,
                        RISK_MITIGATION_MARKS = AFRVM.RISK_MITIGATION_MARKS,
                    

                        REPLY_PERSON = AFRVM.REPLY_PERSON,
                        RESPONSIBLE_DEPT_HEAD = AFRVM.RESPONSIBLE_DEPT_HEAD,
                        REMARKS = AFRVM.REMARKS,
                        SIGNIFICANT_GAP = AFRVM.SIGNIFICANT_GAP,
                        FOLLOWUP_REQUIREMENT_ID = AFRVM.FOLLOWUP_REQUIREMENT_ID,

                        ACT = true,
                        CDT = DateTime.Now,
                        CDU = U,

                        AUDIT_RPT_FOLLOWUP_ID = Sequence.GetSeq(36).ToString()

            };

                    db.AUDIT_REPORT_FOLLOWUP.Add(AFR);
                    db.SaveChanges();
                    return Json(new { success = true, reportpid = "Audit Report Follow Up Number: " + AFR.AUDIT_RPT_FOLLOWUP_ID }, JsonRequestBehavior.AllowGet);
  
            }

            return Json(new { success = false, reportpid = "Invalid input data. Audit Report 1st Follow Up not saved." }, JsonRequestBehavior.AllowGet);
        }

        [KAuth(5302)]
        public ActionResult FirstFollowUpReportList()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (T == "10")
            {
                string sql_1 = @"SELECT RF.AUDIT_RPT_FOLLOWUP_ID,
                                   RF.AUDIT_RPT_ID,
                                   RF.FOLLOWUP_DATE,
                                   RF.TOTAL_FINDINGS,
                                   RF.ACTION_FINDINGS,

                                   RF.NO_ACTION_FINDINGS,
                                   RF.ACTION_PERCENTAGE,
                                   RT.RISK_TYPE_NAME,
                                   RM.RISK_MITIGATION_NAME,
                                   RF.RISK_MITIGATION_MARKS,

                                   RF.REPLY_PERSON,
                                   RF.RESPONSIBLE_DEPT_HEAD,
                                   RF.REMARKS,
                                   RF.SIGNIFICANT_GAP,
                                   FUR.FOLLOWUP_REQUIREMENT_NAME,
                                   RF.CDU,
                                   RF.CDT
                              FROM AUDIT_REPORT_FOLLOWUP  RF
                                   LEFT JOIN AUDIT_RISK_TYPE RT ON RF.RISK_TYPE_ID = RT.RISK_TYPE_ID
                                   LEFT JOIN AUDIT_RISK_MITIGATION RM
                                       ON RF.RISK_MITIGATION_ID = RM.RISK_MITIGATION_ID
                                   LEFT JOIN AUDIT_FOLLOWUP_REQUIREMENT FUR
                                       ON RF.FOLLOWUP_REQUIREMENT_ID = FUR.FOLLOWUP_REQUIREMENT_ID
                             WHERE RF.ACT = 1 AND  RF.FOLLOWUP_NUMBER =1
                             ORDER BY TO_NUMBER(RF.AUDIT_RPT_ID) ASC";
                var TaskData = db.Database.SqlQuery<AuditReportFirstFollowUpVM>(sql: sql_1);
                ViewBag.Message = "Audit Report List";
                var R = TaskData.Take(500).ToList();
                return View(R);
            }
            else
            {
                string sql_1 = @"SELECT RF.AUDIT_RPT_FOLLOWUP_ID,
                                   RF.AUDIT_RPT_ID,
                                   RF.FOLLOWUP_DATE,
                                   RF.TOTAL_FINDINGS,
                                   RF.ACTION_FINDINGS,
                                   RF.NO_ACTION_FINDINGS,
                                   RF.ACTION_PERCENTAGE,
                                   RT.RISK_TYPE_NAME,
                                   RM.RISK_MITIGATION_NAME,
                                   RF.RISK_MITIGATION_MARKS,
                                   RF.REPLY_PERSON,
                                   RF.RESPONSIBLE_DEPT_HEAD,
                                   RF.REMARKS,
                                   RF.SIGNIFICANT_GAP,
                                   FUR.FOLLOWUP_REQUIREMENT_NAME,
                                   RF.CDU,
                                   RF.CDT
                              FROM AUDIT_REPORT_FOLLOWUP  RF
                                   LEFT JOIN AUDIT_RISK_TYPE RT ON RF.RISK_TYPE_ID = RT.RISK_TYPE_ID
                                   LEFT JOIN AUDIT_RISK_MITIGATION RM
                                       ON RF.RISK_MITIGATION_ID = RM.RISK_MITIGATION_ID
                                   LEFT JOIN AUDIT_FOLLOWUP_REQUIREMENT FUR
                                       ON RF.FOLLOWUP_REQUIREMENT_ID = FUR.FOLLOWUP_REQUIREMENT_ID
                               WHERE RF.ACT = 1 AND  RF.FOLLOWUP_NUMBER =1
                                AND RF.CDU= '" + U + @"' ORDER BY TO_NUMBER(RF.AUDIT_RPT_ID) ASC";
                var TaskData = db.Database.SqlQuery<AuditReportFirstFollowUpVM>(sql: sql_1);
                ViewBag.Message = "Audit Report List";
                var R = TaskData.Take(500).ToList();
                return View(R);
            }
        }

        [KAuth(5303)]
        [HttpGet]
        public ActionResult CreateSecondFollowUpReport()
        {
            string U = Session["UserId"].ToString();
            ViewBag.AUDIT_RPT_ID = new SelectList(RP.SearchSecondFollowUpReportList(UserId: U), "AUDIT_RPT_ID", "AUDIT_RPT_NAME");
            ViewBag.RISK_MITIGATION_ID = new SelectList(RP.RiskMitigationList(), "RISK_MITIGATION_ID", "RISK_MITIGATION_NAME");
            ViewBag.RISK_TYPE_ID = new SelectList(RP.RiskTypeList(), "RISK_TYPE_ID", "RISK_TYPE_NAME");
            ViewBag.FOLLOWUP_REQUIREMENT_ID = new SelectList(RP.FollowUpRequirementList(), "FOLLOWUP_REQUIREMENT_ID", "FOLLOWUP_REQUIREMENT_NAME");

            ViewBag.FollowUpType = "SecondFollowUp";
            return View("CreateFirstFollowUpReport");
        }

        [KAuth(5303)]
        [HttpPost]
        public ActionResult CreateSecondFollowUpReport(AuditFollowUpReportVM AFRVM)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            if (ModelState.IsValid)
            {
                AUDIT_REPORT AR = db.AUDIT_REPORT.Find(AFRVM.AUDIT_RPT_ID);

              

                if (AFRVM.FOLLOWUP_REQUIREMENT_ID == "1" && AFRVM.FOLLOWUP_DATE != null)
                {
                    var AUDIT_FOLLOWUP_TYPE_DATA = db.AUDIT_FOLLOWUP_TYPE.FirstOrDefault(c => c.FOLLOWUP_TYPE_ID == "3");
                    int THIRD_FOLLOWUP_DAYS = AUDIT_FOLLOWUP_TYPE_DATA.FOLLOWUP_DAYS;
                    AR.THIRD_FOLLOWUP_DATE = AFRVM.FOLLOWUP_DATE.Value.AddDays(THIRD_FOLLOWUP_DAYS);
                }

                if (AFRVM.FOLLOWUP_REQUIREMENT_ID == "2")
                {
                    AR.THIRD_FOLLOWUP_STATUS = "NO NEED";
                }
                AR.SECOND_FOLLOWUP_STATUS = "Y";
                AR.UDT = DateTime.Now;
                AR.UDU = U;
                db.Entry(AR).State = EntityState.Modified;

                AUDIT_REPORT_FOLLOWUP AFR = new AUDIT_REPORT_FOLLOWUP
                {
                    AUDIT_RPT_ID = AFRVM.AUDIT_RPT_ID,
                    FOLLOWUP_NUMBER = 2,
                    FOLLOWUP_DATE = AFRVM.FOLLOWUP_DATE,
                    TOTAL_FINDINGS = AFRVM.TOTAL_FINDINGS,

                    ACTION_FINDINGS = AFRVM.ACTION_FINDINGS,
                    NO_ACTION_FINDINGS = (AFRVM.TOTAL_FINDINGS - AFRVM.ACTION_FINDINGS),
                    ACTION_PERCENTAGE = AFRVM.TOTAL_FINDINGS == 0 ? 0 : ((decimal)AFRVM.ACTION_FINDINGS / AFRVM.TOTAL_FINDINGS) * 100,
                    RISK_TYPE_ID = AFRVM.RISK_TYPE_ID,
                    RISK_MITIGATION_ID = AFRVM.RISK_MITIGATION_ID,
                    RISK_MITIGATION_MARKS = AFRVM.RISK_MITIGATION_MARKS,


                    REPLY_PERSON = AFRVM.REPLY_PERSON,
                    RESPONSIBLE_DEPT_HEAD = AFRVM.RESPONSIBLE_DEPT_HEAD,
                    REMARKS = AFRVM.REMARKS,
                    SIGNIFICANT_GAP = AFRVM.SIGNIFICANT_GAP,
                    FOLLOWUP_REQUIREMENT_ID = AFRVM.FOLLOWUP_REQUIREMENT_ID,

                    ACT = true,
                    CDT = DateTime.Now,
                    CDU = U,

                    AUDIT_RPT_FOLLOWUP_ID = Sequence.GetSeq(36).ToString()

                };

                db.AUDIT_REPORT_FOLLOWUP.Add(AFR);
                db.SaveChanges();
                return Json(new { success = true, reportpid = "Audit Report Follow Up Number: " + AFR.AUDIT_RPT_FOLLOWUP_ID }, JsonRequestBehavior.AllowGet);

            }

            return Json(new { success = false, reportpid = "Invalid input data. Audit Report 2nd Follow Up not saved." }, JsonRequestBehavior.AllowGet);
        }

        [KAuth(5304)]
        public ActionResult SecondFollowUpReportList()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (T == "10")
            {
                string sql_1 = @"SELECT RF.AUDIT_RPT_FOLLOWUP_ID,
                                   RF.AUDIT_RPT_ID,
                                   RF.FOLLOWUP_DATE,
                                   RF.TOTAL_FINDINGS,
                                   RF.ACTION_FINDINGS,

                                   RF.NO_ACTION_FINDINGS,
                                   RF.ACTION_PERCENTAGE,
                                   RT.RISK_TYPE_NAME,
                                   RM.RISK_MITIGATION_NAME,
                                   RF.RISK_MITIGATION_MARKS,

                                   RF.REPLY_PERSON,
                                   RF.RESPONSIBLE_DEPT_HEAD,
                                   RF.REMARKS,
                                   RF.SIGNIFICANT_GAP,
                                   FUR.FOLLOWUP_REQUIREMENT_NAME,
                                   RF.CDU,
                                   RF.CDT
                              FROM AUDIT_REPORT_FOLLOWUP  RF
                                   LEFT JOIN AUDIT_RISK_TYPE RT ON RF.RISK_TYPE_ID = RT.RISK_TYPE_ID
                                   LEFT JOIN AUDIT_RISK_MITIGATION RM
                                       ON RF.RISK_MITIGATION_ID = RM.RISK_MITIGATION_ID
                                   LEFT JOIN AUDIT_FOLLOWUP_REQUIREMENT FUR
                                       ON RF.FOLLOWUP_REQUIREMENT_ID = FUR.FOLLOWUP_REQUIREMENT_ID
                             WHERE RF.ACT = 1 AND  RF.FOLLOWUP_NUMBER =2
                             ORDER BY TO_NUMBER(RF.AUDIT_RPT_ID) ASC";
                var TaskData = db.Database.SqlQuery<AuditReportFirstFollowUpVM>(sql: sql_1);
                ViewBag.Message = "Audit Report List";
                var R = TaskData.Take(500).ToList();
                return View(R);
            }
            else
            {
                string sql_1 = @"SELECT RF.AUDIT_RPT_FOLLOWUP_ID,
                                   RF.AUDIT_RPT_ID,
                                   RF.FOLLOWUP_DATE,
                                   RF.TOTAL_FINDINGS,
                                   RF.ACTION_FINDINGS,
                                   RF.NO_ACTION_FINDINGS,
                                   RF.ACTION_PERCENTAGE,
                                   RT.RISK_TYPE_NAME,
                                   RM.RISK_MITIGATION_NAME,
                                   RF.RISK_MITIGATION_MARKS,
                                   RF.REPLY_PERSON,
                                   RF.RESPONSIBLE_DEPT_HEAD,
                                   RF.REMARKS,
                                   RF.SIGNIFICANT_GAP,
                                   FUR.FOLLOWUP_REQUIREMENT_NAME,
                                   RF.CDU,
                                   RF.CDT
                              FROM AUDIT_REPORT_FOLLOWUP  RF
                                   LEFT JOIN AUDIT_RISK_TYPE RT ON RF.RISK_TYPE_ID = RT.RISK_TYPE_ID
                                   LEFT JOIN AUDIT_RISK_MITIGATION RM
                                       ON RF.RISK_MITIGATION_ID = RM.RISK_MITIGATION_ID
                                   LEFT JOIN AUDIT_FOLLOWUP_REQUIREMENT FUR
                                       ON RF.FOLLOWUP_REQUIREMENT_ID = FUR.FOLLOWUP_REQUIREMENT_ID
                               WHERE RF.ACT = 1 AND  RF.FOLLOWUP_NUMBER =2
                                AND RF.CDU= '" + U + @"' ORDER BY TO_NUMBER(RF.AUDIT_RPT_ID) ASC";
                var TaskData = db.Database.SqlQuery<AuditReportFirstFollowUpVM>(sql: sql_1);
                ViewBag.Message = "Audit Report List";
                var R = TaskData.Take(500).ToList();
                return View(R);
            }
        }

        [KAuth(5303)]
        [HttpGet]
        public ActionResult CreateThirdFollowUpReport()
        {
            string U = Session["UserId"].ToString();
            ViewBag.AUDIT_RPT_ID = new SelectList(RP.SearchThirdFollowUpReportList(UserId: U), "AUDIT_RPT_ID", "AUDIT_RPT_NAME");
            ViewBag.RISK_MITIGATION_ID = new SelectList(RP.RiskMitigationList(), "RISK_MITIGATION_ID", "RISK_MITIGATION_NAME");
            ViewBag.RISK_TYPE_ID = new SelectList(RP.RiskTypeList(), "RISK_TYPE_ID", "RISK_TYPE_NAME");
            ViewBag.FOLLOWUP_REQUIREMENT_ID = new SelectList(RP.FollowUpRequirementList(), "FOLLOWUP_REQUIREMENT_ID", "FOLLOWUP_REQUIREMENT_NAME");

            ViewBag.FollowUpType = "ThirdFollowUp";
            return View("CreateFirstFollowUpReport");
        }

        [KAuth(5303)]
        [HttpPost]
        public ActionResult CreateThirdFollowUpReport(AuditFollowUpReportVM AFRVM)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            if (ModelState.IsValid)
            {
                AUDIT_REPORT AR = db.AUDIT_REPORT.Find(AFRVM.AUDIT_RPT_ID);

                //if (AFRVM.FOLLOWUP_REQUIREMENT_ID == "1" && AFRVM.FOLLOWUP_DATE != null)
                //{
                //    AR.THIRD_FOLLOWUP_DATE = AFRVM.FOLLOWUP_DATE.Value.AddDays(10);
                //}

                //if (AFRVM.FOLLOWUP_REQUIREMENT_ID == "2")
                //{
                    AR.FOLLOWUP_STATUS = "Done";
                //}
                AR.THIRD_FOLLOWUP_STATUS = "Y";
                AR.UDT = DateTime.Now;
                AR.UDU = U;
                db.Entry(AR).State = EntityState.Modified;

                AUDIT_REPORT_FOLLOWUP AFR = new AUDIT_REPORT_FOLLOWUP
                {
                    AUDIT_RPT_ID = AFRVM.AUDIT_RPT_ID,
                    FOLLOWUP_NUMBER = 3,
                    FOLLOWUP_DATE = AFRVM.FOLLOWUP_DATE,
                    TOTAL_FINDINGS = AFRVM.TOTAL_FINDINGS,

                    ACTION_FINDINGS = AFRVM.ACTION_FINDINGS,
                    NO_ACTION_FINDINGS = (AFRVM.TOTAL_FINDINGS - AFRVM.ACTION_FINDINGS),
                    ACTION_PERCENTAGE = AFRVM.TOTAL_FINDINGS == 0 ? 0 : ((decimal)AFRVM.ACTION_FINDINGS / AFRVM.TOTAL_FINDINGS) * 100,
                    RISK_TYPE_ID = AFRVM.RISK_TYPE_ID,
                    RISK_MITIGATION_ID = AFRVM.RISK_MITIGATION_ID,
                    RISK_MITIGATION_MARKS = AFRVM.RISK_MITIGATION_MARKS,


                    REPLY_PERSON = AFRVM.REPLY_PERSON,
                    RESPONSIBLE_DEPT_HEAD = AFRVM.RESPONSIBLE_DEPT_HEAD,
                    REMARKS = AFRVM.REMARKS,
                    SIGNIFICANT_GAP = AFRVM.SIGNIFICANT_GAP,
                    FOLLOWUP_REQUIREMENT_ID = AFRVM.FOLLOWUP_REQUIREMENT_ID,

                    ACT = true,
                    CDT = DateTime.Now,
                    CDU = U,

                    AUDIT_RPT_FOLLOWUP_ID = Sequence.GetSeq(36).ToString()

                };

                db.AUDIT_REPORT_FOLLOWUP.Add(AFR);
                db.SaveChanges();
                return Json(new { success = true, reportpid = "Audit Report Follow Up Number: " + AFR.AUDIT_RPT_FOLLOWUP_ID }, JsonRequestBehavior.AllowGet);

            }

            return Json(new { success = false, reportpid = "Invalid input data. Audit Report 3rd Follow Up not saved." }, JsonRequestBehavior.AllowGet);
        }

        [KAuth(5304)]
        public ActionResult ThirdFollowUpReportList()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();
            if (T == "10")
            {
                string sql_1 = @"SELECT RF.AUDIT_RPT_FOLLOWUP_ID,
                                   RF.AUDIT_RPT_ID,
                                   RF.FOLLOWUP_DATE,
                                   RF.TOTAL_FINDINGS,
                                   RF.ACTION_FINDINGS,

                                   RF.NO_ACTION_FINDINGS,
                                   RF.ACTION_PERCENTAGE,
                                   RT.RISK_TYPE_NAME,
                                   RM.RISK_MITIGATION_NAME,
                                   RF.RISK_MITIGATION_MARKS,

                                   RF.REPLY_PERSON,
                                   RF.RESPONSIBLE_DEPT_HEAD,
                                   RF.REMARKS,
                                   RF.SIGNIFICANT_GAP,
                                   FUR.FOLLOWUP_REQUIREMENT_NAME,
                                   RF.CDU,
                                   RF.CDT
                              FROM AUDIT_REPORT_FOLLOWUP  RF
                                   LEFT JOIN AUDIT_RISK_TYPE RT ON RF.RISK_TYPE_ID = RT.RISK_TYPE_ID
                                   LEFT JOIN AUDIT_RISK_MITIGATION RM
                                       ON RF.RISK_MITIGATION_ID = RM.RISK_MITIGATION_ID
                                   LEFT JOIN AUDIT_FOLLOWUP_REQUIREMENT FUR
                                       ON RF.FOLLOWUP_REQUIREMENT_ID = FUR.FOLLOWUP_REQUIREMENT_ID
                             WHERE RF.ACT = 1 AND  RF.FOLLOWUP_NUMBER =3
                             ORDER BY TO_NUMBER(RF.AUDIT_RPT_ID) ASC";
                var TaskData = db.Database.SqlQuery<AuditReportFirstFollowUpVM>(sql: sql_1);
                ViewBag.Message = "Audit Report List";
                var R = TaskData.Take(500).ToList();
                return View(R);
            }
            else
            {
                string sql_1 = @"SELECT RF.AUDIT_RPT_FOLLOWUP_ID,
                                   RF.AUDIT_RPT_ID,
                                   RF.FOLLOWUP_DATE,
                                   RF.TOTAL_FINDINGS,
                                   RF.ACTION_FINDINGS,
                                   RF.NO_ACTION_FINDINGS,
                                   RF.ACTION_PERCENTAGE,
                                   RT.RISK_TYPE_NAME,
                                   RM.RISK_MITIGATION_NAME,
                                   RF.RISK_MITIGATION_MARKS,
                                   RF.REPLY_PERSON,
                                   RF.RESPONSIBLE_DEPT_HEAD,
                                   RF.REMARKS,
                                   RF.SIGNIFICANT_GAP,
                                   FUR.FOLLOWUP_REQUIREMENT_NAME,
                                   RF.CDU,
                                   RF.CDT
                              FROM AUDIT_REPORT_FOLLOWUP  RF
                                   LEFT JOIN AUDIT_RISK_TYPE RT ON RF.RISK_TYPE_ID = RT.RISK_TYPE_ID
                                   LEFT JOIN AUDIT_RISK_MITIGATION RM
                                       ON RF.RISK_MITIGATION_ID = RM.RISK_MITIGATION_ID
                                   LEFT JOIN AUDIT_FOLLOWUP_REQUIREMENT FUR
                                       ON RF.FOLLOWUP_REQUIREMENT_ID = FUR.FOLLOWUP_REQUIREMENT_ID
                               WHERE RF.ACT = 1 AND  RF.FOLLOWUP_NUMBER =3
                                AND RF.CDU= '" + U + @"' ORDER BY TO_NUMBER(RF.AUDIT_RPT_ID) ASC";
                var TaskData = db.Database.SqlQuery<AuditReportFirstFollowUpVM>(sql: sql_1);
                ViewBag.Message = "Audit Report List";
                var R = TaskData.Take(500).ToList();
                return View(R);
            }
        }

        [KAuth(5208)]
        public ActionResult AuditorWiseReportDetailList()
        {
            ViewBag.AUDITOR_STAFF_ID = new SelectList(RP.AuditorListNew(), "AUDITOR_STAFF_ID", "AUDITOR_NAME");
            ViewBag.REPORT_STATUS_ID = new SelectList(RP.ReportStatusList(), "REPORT_STATUS_ID", "REPORT_STATUS_NAME");
            ViewBag.TASK_STATUS = new SelectList(RP.TaskStatusList(), "TASK_STATUS_ID", "TASK_STATUS_NAME");
            return View();
        }

        [KAuth(5208)]
        [HttpPost]
        public ActionResult AuditorWiseReportDetailList(string AUDITOR_STAFF_ID, String REPORT_STATUS_ID, string TASK_STATUS_ID )
        {
            string fileName = "Auditor_Wise_Report_Detail_List_" +
                              DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

            string sql = @"
                    SELECT
                        r.AUDIT_RPT_ID,
                        r.RPT_SENT_DATE,
                        r.TASK_ID,
                        r.AUDIT_RPT_NAME,
                        l.LOCATION_NAME,
                        g.COMPANY_GROUP_NAME,
                        com.COMPANY_NAME,
                        dep.DEPARTMENT_NAME,

                        ARS.REPORT_STATUS_NAME,
                        ATS.TASK_STATUS_NAME,

                        r.WORK_SCOPE,
                        r.WORK_OPPORTUNITY,
                        u.USER_NAME,

                        ct.TASK_CATEGORY_TYPE_NAME,
                        c.TASK_CATEGORY_NAME,

                        ds.AUDIT_SAVING_NAME AS DIRECT_SAVING_NAME,
                        r.DIRECT_SAVING_AMOUNT,

                        ids.AUDIT_SAVING_NAME AS INDIRECT_SAVING_NAME,
                        r.INDIRECT_SAVING_AMOUNT,

                        r.DEMOTION,
                        r.DEVELOPMENT,
                        r.JOB_SACK_OUT,
                        r.TRANSFER,
                        r.WARNING_LETTER,
                        r.PENALTY,
                        r.HOLD_PAYMENT,
                        r.REMARK,

                        m.MANAGEMENT_ACTION_NAME,

                        arf.FU1ST_FOLLOWUP_NUMBER,
                        r.FIRST_FOLLOWUP_DATE,
                        arf.FU1ST_FOLLOWUP_DATE,
                        arf.FU1ST_TOTAL_FINDINGS,
                        arf.FU1ST_ACTION_FINDINGS,
                        arf.FU1ST_NO_ACTION_FINDINGS,
                        arf.FU1ST_ACTION_PERCENTAGE,
                        arf.FU1ST_RISK_TYPE_NAME,
                        arf.FU1ST_RISK_MITIGATION_NAME,
                        arf.FU1ST_RISK_MITIGATION_MARKS,
                        arf.FU1ST_REPLY_PERSON,
                        arf.FU1ST_RESPONSIBLE_DEPT_HEAD,
                        arf.FU1ST_REMARKS,
                        arf.FU1ST_SIGNIFICANT_GAP,
                        arf.FU1ST_FU_REQUIREMENT_NAME,

                        arf.FU2ND_FOLLOWUP_NUMBER,
                        r.SECOND_FOLLOWUP_DATE,
                        arf.FU2ND_FOLLOWUP_DATE,
                        arf.FU2ND_TOTAL_FINDINGS,
                        arf.FU2ND_ACTION_FINDINGS,
                        arf.FU2ND_NO_ACTION_FINDINGS,
                        arf.FU2ND_ACTION_PERCENTAGE,
                        arf.FU2ND_RISK_TYPE_NAME,
                        arf.FU2ND_RISK_MITIGATION_NAME,
                        arf.FU2ND_RISK_MITIGATION_MARKS,
                        arf.FU2ND_REPLY_PERSON,
                        arf.FU2ND_RESPONSIBLE_DEPT_HEAD,
                        arf.FU2ND_REMARKS,
                        arf.FU2ND_SIGNIFICANT_GAP,
                        arf.FU2ND_FU_REQUIREMENT_NAME,

                        arf.FU3RD_FOLLOWUP_NUMBER,
                        r.THIRD_FOLLOWUP_DATE,
                        arf.FU3RD_FOLLOWUP_DATE,
                        arf.FU3RD_TOTAL_FINDINGS,
                        arf.FU3RD_ACTION_FINDINGS,
                        arf.FU3RD_NO_ACTION_FINDINGS,
                        arf.FU3RD_ACTION_PERCENTAGE,
                        arf.FU3RD_RISK_TYPE_NAME,
                        arf.FU3RD_RISK_MITIGATION_NAME,
                        arf.FU3RD_RISK_MITIGATION_MARKS,
                        arf.FU3RD_REPLY_PERSON,
                        arf.FU3RD_RESPONSIBLE_DEPT_HEAD,
                        arf.FU3RD_REMARKS,
                        arf.FU3RD_SIGNIFICANT_GAP,
                        arf.FU3RD_FU_REQUIREMENT_NAME

                        FROM AUDIT_REPORT r

                            LEFT JOIN AUDIT_TASK_CATEGORY_TYPE ct
                                ON ct.TASK_CATEGORY_TYPE_ID = r.TASK_CATEGORY_TYPE_ID

                            LEFT JOIN AUDIT_TASK_CATEGORY c
                                ON c.TASK_CATEGORY_ID = r.TASK_CATEGORY_ID

                            LEFT JOIN AUDIT_RPT_SAVING_NAME ds
                                ON ds.AUDIT_SAVING_NAME_ID = r.DIRECT_SAVING_TYPE_ID

                            LEFT JOIN AUDIT_RPT_SAVING_NAME ids
                                ON ids.AUDIT_SAVING_NAME_ID = r.INDIRECT_SAVING_TYPE_ID

                            LEFT JOIN AUDIT_MANAGEMENT_ACTION m
                                ON m.MANAGEMENT_ACTION_ID = r.MANAGEMENT_ACTION_ID

                            LEFT JOIN A_USERS u
                                ON u.USER_TEXT = r.CDU

                            LEFT JOIN V_AUDIT_REPORT_FOLLOWUP arf
                                ON arf.AUDIT_RPT_ID = r.AUDIT_RPT_ID

                            INNER JOIN AUDIT_TASK at
                                ON at.TASK_ID = r.TASK_ID

                            INNER JOIN C_LOCATION l
                                ON l.LOCATION_ID = at.LOCATION_ID

                            INNER JOIN C_COMPANY_GROUP g
                                ON g.COMPANY_GROUP_ID = at.COMPANY_GROUP_ID

                            INNER JOIN C_COMPANY com
                                ON com.COMPANY_ID = at.COMPANY_ID

                            INNER JOIN C_DEPARTMENT dep
                                ON dep.DEPARTMENT_ID = at.DEPARTMENT_ID

                            LEFT JOIN  AUDIT_REPORT_STATUS   ARS ON ARS.REPORT_STATUS_ID = at.REPORT_STATUS_ID
                            LEFT JOIN  AUDIT_TASK_STATUS   ATS ON ATS.TASK_STATUS_ID = at.TASK_STATUS

                        WHERE r.ACT = 1
                          AND r.APPROVE_SENT_STATUS = 'N'
                          AND at.REPORT_CREATE_STATUS = 1
                        ";

            List<OracleParameter> parameters = new List<OracleParameter>();

            if (!string.IsNullOrEmpty(AUDITOR_STAFF_ID))
            {
                sql += " AND r.CDU = :AUDITOR_STAFF_ID ";

                parameters.Add(new OracleParameter("AUDITOR_STAFF_ID", AUDITOR_STAFF_ID));
            }

            if (!string.IsNullOrEmpty(REPORT_STATUS_ID))
            {
                sql += " AND at.REPORT_STATUS_ID = :REPORT_STATUS_ID ";

                parameters.Add(new OracleParameter("REPORT_STATUS_ID", REPORT_STATUS_ID));
            }

            if (!string.IsNullOrEmpty(TASK_STATUS_ID))
            {
                sql += " AND ats.TASK_STATUS_ID = :TASK_STATUS_ID ";

                parameters.Add(new OracleParameter("TASK_STATUS_ID", TASK_STATUS_ID));
            }

            //if (FROM_DATE.HasValue)
            //{
            //    sql += " AND TRUNC(r.RPT_SENT_DATE) >= :FROM_DATE ";
            //    parameters.Add(new OracleParameter("FROM_DATE", FROM_DATE.Value));
            //}

            //if (TO_DATE.HasValue)
            //{
            //    sql += " AND TRUNC(r.RPT_SENT_DATE) <= :TO_DATE ";
            //    parameters.Add(new OracleParameter("TO_DATE", TO_DATE.Value));
            //}

            sql += " ORDER BY TO_NUMBER(r.AUDIT_RPT_ID) ASC ";

            List<AuditorWiseAuditReportDetailsListVM> data =
                db.Database.SqlQuery<AuditorWiseAuditReportDetailsListVM>(
                    sql,
                    parameters.ToArray()
                ).ToList();

            using (ExcelPackage excel = new ExcelPackage())
            {
                ExcelWorksheet worksheet =
                    excel.Workbook.Worksheets.Add("Auditor Report");

                worksheet.Cells["A1"].LoadFromCollection(data, true);

                // Format all date columns
                int totalRows = worksheet.Dimension.End.Row;

                for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                {
                    string header = worksheet.Cells[1, col].Text.Trim();

                    if (header == "Report Sent Date" ||
                        header == "First Follow Up Date" ||
                        header == "Follow Up Date" ||
                        header == "Second Follow Up Date" ||
                        header == "Third Follow Up Date" ||
                        header == "FU3RD_FOLLOWUP_DATE")
                    {
                        worksheet.Cells[2, col, totalRows, col]
                                 .Style.Numberformat.Format = "dd/MM/yyyy";
                    }
                }

                worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column]
                         .Style.Font.Bold = true;

                worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column]
                         .Style.Font.Size = 12;

                worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column]
                         .Style.Font.Color.SetColor(System.Drawing.Color.Blue);

                worksheet.Cells.AutoFitColumns();

                using (MemoryStream stream = new MemoryStream())
                {
                    excel.SaveAs(stream);

                    return File(
                        stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName
                    );
                }
            }
        }
    }
}
