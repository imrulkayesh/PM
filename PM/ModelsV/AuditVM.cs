using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PM.ModelsV
{
    public class AuditVM
    {

    }

    public class AuditorVM
    {
        public string AUDITOR_ID { get; set; }
        [DisplayName("Auditor Name")]
        public string AUDITOR_NAME { get; set; }
        [DisplayName("Staff ID")]
        public string AUDITOR_STAFF_ID { get; set; }
        [DisplayName("Mobile Number")]
        public string AUDITOR_MOBILE { get; set; }
        [DisplayName("Email")]
        public string AUDITOR_EMAIL { get; set; }
        public bool ACT { get; set; }
    }

    public class NewTaskVM
    {
        public string TASK_ID { get; set; }
        [DisplayName("Entry By")]
        public string CDU { get; set; }
        [DisplayName("Task Details")]
        public string TASK_DETAILS { get; set; }

        [DisplayName("Task Assign")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? TASK_ASSIGN_DATE { get; set; }
        
        [DisplayName("Mgt. Source Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? MANAGEMENT_SOURCE_DATE { get; set; }

        [DisplayName("Start Plan")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? TASK_START_PLANED_DATE { get; set; }

        [DisplayName("Task Dateline")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? TASK_DEADLINE { get; set; }

        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? TASK_START_DATE { get; set; }
        [DisplayName("Task Status")]
        public string TASK_STATUS { get; set; }
        [DisplayName("Remark")]
        public string REMARKS { get; set; }

        [DisplayName("Financial Value")]
        public Nullable<decimal> FINANCIAL { get; set; }
        [DisplayName("Non Financial")]
        public string NON_FINANCIAL { get; set; }

        [DisplayName("Location")]
        public string LOCATION_ID { get; set; }

        [DisplayName("Group")]
        public string COMPANY_GROUP_ID { get; set; }
        [DisplayName("Company")]
        public string COMPANY_ID { get; set; }
        [DisplayName("Department")]
        public string DEPARTMENT_ID { get; set; }
        //[DisplayName("Category Type")]
        //public string TASK_CATEGORY_TYPE_ID { get; set; }
        //[DisplayName("Category")]
        //public string TASK_CATEGORY_ID { get; set; }
        [DisplayName("Communication By")]
        public string TASK_COMMUNICATION_BY_ID { get; set; }
        [DisplayName("Assign By")]
        public string TASK_ASSIGN_BY_ID { get; set; }
        [DisplayName("Management Source")]
        public string MANAGEMENT_SOURCE_ID { get; set; }
        [DisplayName("Task Priority")]
        public string TASK_PRIORITY_ID { get; set; }
        [DisplayName("Task Type")]
        public string TASK_TYPE_ID { get; set; }

        [DisplayName("Report Status")]
        public string REPORT_STATUS_ID { get; set; }

        public HttpPostedFileBase FileX { get; set; }

        public bool ACT { get; set; }
        public string AUDITOR_NAME { get; set; }
        public string TASK_STATUS_NAME { get; set; }
        public string FINAL_RPT_STATUS_NAME { get; set; }

        public string LOCATION_NAME { get; set; }
        public string COMPANY_GROUP_NAME { get; set; }
        public string COMPANY_NAME { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string TASK_CATEGORY_TYPE_NAME { get; set; }
        public string TASK_CATEGORY_NAME { get; set; }
        public string TASK_ASSIGN_BY_NAME { get; set; }
        public string MANAGEMENT_SOURCE_NAME { get; set; }
        public string TASK_PRIORITY_NAME { get; set; }
        public string TASK_COMMUNICATION_BY_NAME { get; set; }
        public string TASK_TYPE_NAME { get; set; }
        public string REPORT_STATUS_NAME { get; set; }
        public string USER_NAME { get; set; }

        public string TASK_CATEGORY { get; set; }



    }

    public class UpdateTaskVM
    {
        public string TASK_ID { get; set; }
        [DisplayName("Entry By")]
        public string CDU { get; set; }
        [DisplayName("Task Details")]
        public string TASK_DETAILS { get; set; }

        [DisplayName("Task Assign")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? TASK_ASSIGN_DATE { get; set; }

        [DisplayName("Mgt Source Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? MANAGEMENT_SOURCE_DATE { get; set; }

        [DisplayName("Start Plan")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? TASK_START_PLANED_DATE { get; set; }

        [DisplayName("Task Dateline")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? TASK_DEADLINE { get; set; }

        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? TASK_START_DATE { get; set; }
        [DisplayName("Task Status")]
        public string TASK_STATUS { get; set; }

        [DisplayName("Remark")]
        public string REMARKS { get; set; }

        [DisplayName("Location")]
        public string LOCATION_ID { get; set; }

        [DisplayName("Group")]
        public string COMPANY_GROUP_ID { get; set; }
        [DisplayName("Company")]
        public string COMPANY_ID { get; set; }
        [DisplayName("Department")]
        public string DEPARTMENT_ID { get; set; }
       
        [DisplayName("Communication By")]
        public string TASK_COMMUNICATION_BY_ID { get; set; }
        [DisplayName("Assign By")]
        public string TASK_ASSIGN_BY_ID { get; set; }
        [DisplayName("management Source")]
        public string MANAGEMENT_SOURCE_ID { get; set; }
        [DisplayName("Task Priority")]
        public string TASK_PRIORITY_ID { get; set; }
        [DisplayName("Task Type")]
        public string TASK_TYPE_ID { get; set; }

        [DisplayName("Report Status")]
        public string REPORT_STATUS_ID { get; set; }

        public HttpPostedFileBase FileX { get; set; }

        public bool ACT { get; set; }
        public string USER_NAME { get; set; }
        public string TASK_STATUS_NAME { get; set; }
        public Nullable<decimal> FINANCIAL { get; set; }
        public string NON_FINANCIAL { get; set; }

        public string LOCATION_NAME { get; set; }
        public string COMPANY_GROUP_NAME { get; set; }
        public string COMPANY_NAME { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        //public string TASK_CATEGORY_TYPE_NAME { get; set; }
        //public string TASK_CATEGORY_NAME { get; set; }
        public string TASK_ASSIGN_BY_NAME { get; set; }
        public string MANAGEMENT_SOURCE_NAME { get; set; }
        public string TASK_PRIORITY_NAME { get; set; }
        public string TASK_COMMUNICATION_BY_NAME { get; set; }
        public string TASK_TYPE_NAME { get; set; }
        public string REPORT_STATUS_NAME { get; set; }


    }
    public class AuditReportVM
    {
        public string AUDIT_RPT_DETAIL_ID { get; set; }
        public string AUDIT_RPT_ID { get; set; }
        public string TASK_ID { get; set; }
        public string AUDIT_RPT_NAME { get; set; }
        [DisplayName("Report Sent Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RPT_SENT_DATE { get; set; }
        [DisplayName("Category Type")]
        public string TASK_CATEGORY_TYPE_ID { get; set; }
        [DisplayName("Category")]
        public string TASK_CATEGORY_ID { get; set; }
        public string AUDIT_SAVING_TYPE_ID { get; set; }
        public string AUDIT_SAVING_NAME_ID { get; set; }
        public Nullable<decimal> SAVING_AMOUNT { get; set; }

        public string DIRECT_SAVING_TYPE_ID { get; set; }
        public Nullable<decimal> DIRECT_SAVING_AMOUNT { get; set; }
        public string INDIRECT_SAVING_TYPE_ID { get; set; }
        public Nullable<decimal> INDIRECT_SAVING_AMOUNT { get; set; }

        public string DIRECT_AUDIT_SAVING_NAME { get; set; }
        public string INDIRECT_AUDIT_SAVING_NAME { get; set; }
        public string RISK_TYPE_ID { get; set; }


        //public string Assist_BY_ID { get; set; }

        public Nullable<decimal> WORK_SCOPE { get; set; }
        public string WORK_OPPORTUNITY { get; set; }
        public string ASSIST_BY { get; set; }
        public string APPROVE_SENT_STATUS { get; set; }

        public string RPT_ACTION_ID { get; set; }

        public string VALUE { get; set; }

        public string DEMOTION { get; set; }

        public string DEVELOPMENT { get; set; }

        public string JOB_SACK_OUT { get; set; }

        public string TRANSFER { get; set; }

        public string WARNING_LETTER { get; set; }

        public string PENALTY { get; set; }
        public string HOLD_PAYMENT { get; set; }
        public string SUPPLIER_BLACKLIST { get; set; }
        public string CORRUPTION_IDENTIFIED { get; set; }
        public string INEFFICIENCY_IDENTIFIED { get; set; }
        public string MANAGEMENT_ACTION_ID { get; set; }
        public string REMARK{ get; set; }

        public List<ReportActionDetails> ReportActionDetails { get; set; }


        public string REPORT_CREATE_STATUS { get; set; }
        public string TASK_DETAILS { get; set; }
        public string REPORT_STATUS_ID { get; set; }
        public DateTime? FIRST_FOLLOWUP_DATE { get; set; }

    }

    public class ReportActionDetails
    {
        public string AUDIT_RPT_DETAIL_ID { get; set; }

        public string AUDIT_RPT_ID { get; set; }

        [DisplayName("Action ID")]
        //[Required(ErrorMessage = "Action ID is required")]
        public string RPT_ACTION_ID { get; set; }

        [DisplayName("Number")]
        //public int VALUE { get; set; }
        public string VALUE { get; set; } 

        [DisplayName("Remark")]
        public string REMARK { get; set; }

        [DisplayName("Line")]
        public int LINE { get; set; }
        [DisplayName("Category Type")]
        public string TASK_CATEGORY_TYPE_ID { get; set; }
        [DisplayName("Category")]
        public string TASK_CATEGORY_ID { get; set; }
        [DisplayName("Saving Type")]
        public string AUDIT_SAVING_TYPE_ID { get; set; }
        [DisplayName("Saving Name")]
        public string AUDIT_SAVING_NAME_ID { get; set; }
        [DisplayName("Saving Amount")]
        public Nullable<decimal> SAVING_AMOUNT { get; set; }

        [DisplayName("Demotion")]
        public string DEMOTION { get; set; }
        [DisplayName("Development")]
        public string DEVELOPMENT { get; set; }
        [DisplayName("Job Sack Out")]
        public string JOB_SACK_OUT { get; set; }
        [DisplayName("Transfer")]
        public string TRANSFER { get; set; }
        [DisplayName("Warning Latter")]
        public string WARNING_LETTER { get; set; }
        [DisplayName("Penalty")]
        public string PENALTY { get; set; }

        public string DIRECT_SAVING_TYPE_ID { get; set; }
        public Nullable<decimal> DIRECT_SAVING_AMOUNT { get; set; }
        public string INDIRECT_SAVING_TYPE_ID { get; set; }
        public Nullable<decimal> INDIRECT_SAVING_AMOUNT { get; set; }

        public string DIRECT_AUDIT_SAVING_NAME { get; set; }
        public string INDIRECT_AUDIT_SAVING_NAME { get; set; }
        

        public string TASK_CATEGORY_TYPE_NAME { get; set; }
        public string TASK_CATEGORY_NAME { get; set; }
        public string AUDIT_SAVING_TYPE_NAME { get; set; }
        public string AUDIT_SAVING_NAME { get; set; }

        public string RPT_ACTION_NAME { get; set; }


    }

    public class AuditReportListVM
    {
        [DisplayName("Report SL NO")]
        public string AUDIT_RPT_ID { get; set; }
        [DisplayName("Task ID")]
        public string TASK_ID { get; set; }
        [DisplayName("Report Name")]
        public string AUDIT_RPT_NAME { get; set; }
        [DisplayName("Report Sent Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RPT_SENT_DATE { get; set; }

        [DisplayName("Location")]
        public string LOCATION_ID { get; set; }
        [DisplayName("Group")]
        public string COMPANY_GROUP_ID { get; set; }
        [DisplayName("Company")]
        public string COMPANY_ID { get; set; }
        [DisplayName("Department")]
        public string DEPARTMENT_ID { get; set; }
        [DisplayName("Prepared by")]
        public string CDU { get; set; }
        [DisplayName("Work Opportunity")]
        public string WORK_OPPORTUNITY { get; set; }
        [DisplayName("Work Scope")]
        public Nullable<decimal> WORK_SCOPE { get; set; }
        //[DisplayName("Action")]
        //public string RPT_ACTION_ID { get; set; }
        //[DisplayName("Number")]
        //public int VALUE { get; set; }

        public string LOCATION_NAME { get; set; }
        public string COMPANY_GROUP_NAME { get; set; }
        public string COMPANY_NAME { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        //public string ACTION_NAME { get; set; }
        
        public string REPORT_CREATE_STATUS { get; set; }
        public string USER_NAME { get; set; }

    }
    public class AuditReportAttachmentListVM
    {
        public string AUDIT_IMG_ID { get; set; }
        [DisplayName("Report SL NO")]
        public string AUDIT_RPT_ID { get; set; }
        [DisplayName("Task ID")]
        public string TASK_ID { get; set; }

        public string IMG_NAME { get; set; }
        public string IMG_PATH { get; set; }

        public string CDU { get; set; }
        public DateTime? CDT { get; set; }


    }
    public class AuditReportDetailsListVM
    {
        [DisplayName("Report SL NO")]
        public string AUDIT_RPT_ID { get; set; }
        [DisplayName("Task ID")]
        public string TASK_ID { get; set; }
        [DisplayName("Report Name")]
        public string AUDIT_RPT_NAME { get; set; }
        [DisplayName("Report Sent Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RPT_SENT_DATE { get; set; }

        [DisplayName("Location")]
        public string LOCATION_ID { get; set; }
        [DisplayName("Group")]
        public string COMPANY_GROUP_ID { get; set; }
        [DisplayName("Company")]
        public string COMPANY_ID { get; set; }
        [DisplayName("Department")]
        public string DEPARTMENT_ID { get; set; }
        [DisplayName("Prepared by")]
        public string CDU { get; set; }
        [DisplayName("Work Opportunity")]
        public string WORK_OPPORTUNITY { get; set; }
        [DisplayName("Work Scope")]
        public Nullable<decimal> WORK_SCOPE { get; set; }

        public string LOCATION_NAME { get; set; }
        public string COMPANY_GROUP_NAME { get; set; }
        public string COMPANY_NAME { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        //public string ACTION_NAME { get; set; }

        public string REPORT_CREATE_STATUS { get; set; }
        public string USER_NAME { get; set; }

        [DisplayName("Segment Type")]
        public string TASK_CATEGORY_TYPE_NAME { get; set; }
        [DisplayName("Segment Name")]
        public string TASK_CATEGORY_NAME { get; set; }
        [DisplayName("Saving Type")]
        public string AUDIT_SAVING_TYPE_NAME { get; set; }
        [DisplayName("Saving Name")]
        public string AUDIT_SAVING_NAME { get; set; }
        [DisplayName("Audit Action")]
        public string ACTION_NAME { get; set; }
        [DisplayName("Saving Amount")]
        public string SAVING_AMOUNT { get; set; }
        [DisplayName("Number")]
        public string VALUE { get; set; }

        [DisplayName("Direct Saving Type")]
        public string DIRECT_SAVING_NAME { get; set; }
        [DisplayName("Indirect Saving Type")]
        public string INDIRECT_SAVING_NAME { get; set; }

        [DisplayName("Direct Saving Amount")]
        public string DIRECT_SAVING_AMOUNT { get; set; }
        [DisplayName("Indirect Saving Amount")]
        public string INDIRECT_SAVING_AMOUNT { get; set; }

        [DisplayName("Demotion")]
        public string DEMOTION { get; set; }
        [DisplayName("Development")]
        public string DEVELOPMENT { get; set; }
        [DisplayName("Job Sack Out")]
        public string JOB_SACK_OUT { get; set; }
        [DisplayName("Transfer")]
        public string TRANSFER { get; set; }
        [DisplayName("Warning Letter")]
        public string WARNING_LETTER { get; set; }
        [DisplayName("Penalty")]
        public string PENALTY { get; set; }

        [DisplayName("Hold Payment")]
        public string HOLD_PAYMENT { get; set; }
        [DisplayName("Supplier Blacklist")]
        public string SUPPLIER_BLACKLIST { get; set; }


        [DisplayName("Remark")]
        public string REMARK { get; set; }

        [DisplayName("Management Action")]
        public string MANAGEMENT_ACTION_ID { get; set; }
        public string MANAGEMENT_ACTION_NAME { get; set; }
        //1st Fillowup

        [DisplayName("First Follow Up Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FIRST_FOLLOWUP_DATE { get; set; }

        [DisplayName("Follow Up Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FU1ST_FOLLOWUP_DATE { get; set; }

        [DisplayName("Total Findings")]
        public string FU1ST_TOTAL_FINDINGS { get; set; }
        [DisplayName("Action Findings")]
        public string FU1ST_ACTION_FINDINGS { get; set; }
        [DisplayName("No Action Findings")]
        public string FU1ST_NO_ACTION_FINDINGS { get; set; }
        [DisplayName("Action Percentage (%)")]
        public string FU1ST_ACTION_PERCENTAGE { get; set; }
        [DisplayName("Risk Type")]
        public string FU1ST_RISK_TYPE_NAME { get; set; }
        [DisplayName("Risk Mitigation")]
        public string FU1ST_RISK_MITIGATION_NAME { get; set; }
        [DisplayName("Risk Mitigation Marks")]
        public string FU1ST_RISK_MITIGATION_MARKS { get; set; }
        [DisplayName("Reply Person")]
        public string FU1ST_REPLY_PERSON { get; set; }
        [DisplayName("Responsible Dept Head")]
        public string FU1ST_RESPONSIBLE_DEPT_HEAD { get; set; }
        [DisplayName("Remarks")]
        public string FU1ST_REMARKS { get; set; }
        [DisplayName("Significant Gap")]
        public string FU1ST_SIGNIFICANT_GAP { get; set; }
        [DisplayName("Next Follow Up")]
        public string FU1ST_FU_REQUIREMENT_NAME { get; set; }

        //2nd Fillowup

        [DisplayName("Second Follow Up Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? SECOND_FOLLOWUP_DATE { get; set; }

        [DisplayName("Follow Up Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FU2ND_FOLLOWUP_DATE { get; set; }

        [DisplayName("Total Findings")]
        public string FU2ND_TOTAL_FINDINGS { get; set; }
        [DisplayName("Action Findings")]
        public string FU2ND_ACTION_FINDINGS { get; set; }
        [DisplayName("No Action Findings")]
        public string FU2ND_NO_ACTION_FINDINGS { get; set; }
        [DisplayName("Action Percentage (%)")]
        public string FU2ND_ACTION_PERCENTAGE { get; set; }
        [DisplayName("Risk Type")]
        public string FU2ND_RISK_TYPE_NAME { get; set; }
        [DisplayName("Risk Mitigation")]
        public string FU2ND_RISK_MITIGATION_NAME { get; set; }
        [DisplayName("Risk Mitigation Marks")]
        public string FU2ND_RISK_MITIGATION_MARKS { get; set; }
        [DisplayName("Reply Person")]
        public string FU2ND_REPLY_PERSON { get; set; }
        [DisplayName("Responsible Dept Head")]
        public string FU2ND_RESPONSIBLE_DEPT_HEAD { get; set; }
        [DisplayName("Remarks")]
        public string FU2ND_REMARKS { get; set; }
        [DisplayName("Significant Gap")]
        public string FU2ND_SIGNIFICANT_GAP { get; set; }
        [DisplayName("Next Follow Up")]
        public string FU2ND_FU_REQUIREMENT_NAME { get; set; }

        //3rd Fillowup

        [DisplayName("Third Follow Up Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? THIRD_FOLLOWUP_DATE { get; set; }

        [DisplayName("Follow Up Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FU3RD_FOLLOWUP_DATE { get; set; }

        [DisplayName("Total Findings")]
        public string FU3RD_TOTAL_FINDINGS { get; set; }
        [DisplayName("Action Findings")]
        public string FU3RD_ACTION_FINDINGS { get; set; }
        [DisplayName("No Action Findings")]
        public string FU3RD_NO_ACTION_FINDINGS { get; set; }
        [DisplayName("Action Percentage (%)")]
        public string FU3RD_ACTION_PERCENTAGE { get; set; }
        [DisplayName("Risk Type")]
        public string FU3RD_RISK_TYPE_NAME { get; set; }
        [DisplayName("Risk Mitigation")]
        public string FU3RD_RISK_MITIGATION_NAME { get; set; }
        [DisplayName("Risk Mitigation Marks")]
        public string FU3RD_RISK_MITIGATION_MARKS { get; set; }
        [DisplayName("Reply Person")]
        public string FU3RD_REPLY_PERSON { get; set; }
        [DisplayName("Responsible Dept Head")]
        public string FU3RD_RESPONSIBLE_DEPT_HEAD { get; set; }
        [DisplayName("Remarks")]
        public string FU3RD_REMARKS { get; set; }
        [DisplayName("Significant Gap")]
        public string FU3RD_SIGNIFICANT_GAP { get; set; }
        [DisplayName("Next Follow Up")]
        public string FU3RD_FU_REQUIREMENT_NAME { get; set; }

        public string REPORT_STATUS_NAME { get; set; }
        public string TASK_STATUS_NAME { get; set; }


    }

    public class AuditorWiseAuditReportDetailsListVM
    {
        [DisplayName("Report SL NO")]
        public string AUDIT_RPT_ID { get; set; }

        [DisplayName("Report Sent Date")]
        public DateTime? RPT_SENT_DATE { get; set; }

        [DisplayName("Task ID")]
        public string TASK_ID { get; set; }

        [DisplayName("Report Name")]
        public string AUDIT_RPT_NAME { get; set; }

        [DisplayName("Location")]
        public string LOCATION_NAME { get; set; }

        [DisplayName("Group")]
        public string COMPANY_GROUP_NAME { get; set; }

        [DisplayName("Company")]
        public string COMPANY_NAME { get; set; }

        [DisplayName("Department")]
        public string DEPARTMENT_NAME { get; set; }

        [DisplayName("Report Status")]
        public string REPORT_STATUS_NAME { get; set; }

        [DisplayName("Task Status")]
        public string TASK_STATUS_NAME { get; set; }

        [DisplayName("Work Scope")]
        public decimal? WORK_SCOPE { get; set; }

        [DisplayName("Work Opportunity")]
        public string WORK_OPPORTUNITY { get; set; }

        [DisplayName("Prepared By")]
        public string USER_NAME { get; set; }

        [DisplayName("Segment Type")]
        public string TASK_CATEGORY_TYPE_NAME { get; set; }

        [DisplayName("Segment Name")]
        public string TASK_CATEGORY_NAME { get; set; }

        [DisplayName("Direct Saving Type")]
        public string DIRECT_SAVING_NAME { get; set; }

        [DisplayName("Direct Saving Amount")]
        public decimal? DIRECT_SAVING_AMOUNT { get; set; }

        [DisplayName("Indirect Saving Type")]
        public string INDIRECT_SAVING_NAME { get; set; }

        [DisplayName("Indirect Saving Amount")]
        public decimal? INDIRECT_SAVING_AMOUNT { get; set; }

        [DisplayName("Demotion")]
        public string DEMOTION { get; set; }

        [DisplayName("Development")]
        public string DEVELOPMENT { get; set; }

        [DisplayName("Job Sack Out")]
        public string JOB_SACK_OUT { get; set; }

        [DisplayName("Transfer")]
        public string TRANSFER { get; set; }

        [DisplayName("Warning Letter")]
        public string WARNING_LETTER { get; set; }

        [DisplayName("Penalty")]
        public string PENALTY { get; set; }

        [DisplayName("Hold Payment")]
        public string HOLD_PAYMENT { get; set; }

        [DisplayName("Remark")]
        public string REMARK { get; set; }

        [DisplayName("Management Action")]
        public string MANAGEMENT_ACTION_NAME { get; set; }

        // ==========================
        // 1st Follow Up
        // ==========================

        public string FU1ST_FOLLOWUP_NUMBER { get; set; }

        [DisplayName("First Follow Up Date")]
        public DateTime? FIRST_FOLLOWUP_DATE { get; set; }

        [DisplayName("Follow Up Date")]
        public DateTime? FU1ST_FOLLOWUP_DATE { get; set; }

        [DisplayName("Total Findings")]
        public string FU1ST_TOTAL_FINDINGS { get; set; }

        [DisplayName("Action Findings")]
        public string FU1ST_ACTION_FINDINGS { get; set; }

        [DisplayName("No Action Findings")]
        public string FU1ST_NO_ACTION_FINDINGS { get; set; }

        [DisplayName("Action Percentage (%)")]
        public string FU1ST_ACTION_PERCENTAGE { get; set; }

        [DisplayName("Risk Type")]
        public string FU1ST_RISK_TYPE_NAME { get; set; }

        [DisplayName("Risk Mitigation")]
        public string FU1ST_RISK_MITIGATION_NAME { get; set; }

        [DisplayName("Risk Mitigation Marks")]
        public string FU1ST_RISK_MITIGATION_MARKS { get; set; }

        [DisplayName("Reply Person")]
        public string FU1ST_REPLY_PERSON { get; set; }

        [DisplayName("Responsible Dept Head")]
        public string FU1ST_RESPONSIBLE_DEPT_HEAD { get; set; }

        [DisplayName("Remarks")]
        public string FU1ST_REMARKS { get; set; }

        [DisplayName("Significant Gap")]
        public string FU1ST_SIGNIFICANT_GAP { get; set; }

        [DisplayName("Next Follow Up")]
        public string FU1ST_FU_REQUIREMENT_NAME { get; set; }

        // ==========================
        // 2nd Follow Up
        // ==========================

        public string FU2ND_FOLLOWUP_NUMBER { get; set; }

        [DisplayName("Second Follow Up Date")]
        public DateTime? SECOND_FOLLOWUP_DATE { get; set; }

        [DisplayName("Follow Up Date")]
        public DateTime? FU2ND_FOLLOWUP_DATE { get; set; }

        [DisplayName("Total Findings")]
        public string FU2ND_TOTAL_FINDINGS { get; set; }

        [DisplayName("Action Findings")]
        public string FU2ND_ACTION_FINDINGS { get; set; }

        [DisplayName("No Action Findings")]
        public string FU2ND_NO_ACTION_FINDINGS { get; set; }

        [DisplayName("Action Percentage (%)")]
        public string FU2ND_ACTION_PERCENTAGE { get; set; }

        [DisplayName("Risk Type")]
        public string FU2ND_RISK_TYPE_NAME { get; set; }

        [DisplayName("Risk Mitigation")]
        public string FU2ND_RISK_MITIGATION_NAME { get; set; }

        [DisplayName("Risk Mitigation Marks")]
        public string FU2ND_RISK_MITIGATION_MARKS { get; set; }

        [DisplayName("Reply Person")]
        public string FU2ND_REPLY_PERSON { get; set; }

        [DisplayName("Responsible Dept Head")]
        public string FU2ND_RESPONSIBLE_DEPT_HEAD { get; set; }

        [DisplayName("Remarks")]
        public string FU2ND_REMARKS { get; set; }

        [DisplayName("Significant Gap")]
        public string FU2ND_SIGNIFICANT_GAP { get; set; }

        [DisplayName("Next Follow Up")]
        public string FU2ND_FU_REQUIREMENT_NAME { get; set; }

        // ==========================
        // 3rd Follow Up
        // ==========================

        public string FU3RD_FOLLOWUP_NUMBER { get; set; }

        [DisplayName("Third Follow Up Date")]
        public DateTime? THIRD_FOLLOWUP_DATE { get; set; }

        [DisplayName("Follow Up Date")]
        public DateTime? FU3RD_FOLLOWUP_DATE { get; set; }

        [DisplayName("Total Findings")]
        public string FU3RD_TOTAL_FINDINGS { get; set; }

        [DisplayName("Action Findings")]
        public string FU3RD_ACTION_FINDINGS { get; set; }

        [DisplayName("No Action Findings")]
        public string FU3RD_NO_ACTION_FINDINGS { get; set; }

        [DisplayName("Action Percentage (%)")]
        public string FU3RD_ACTION_PERCENTAGE { get; set; }

        [DisplayName("Risk Type")]
        public string FU3RD_RISK_TYPE_NAME { get; set; }

        [DisplayName("Risk Mitigation")]
        public string FU3RD_RISK_MITIGATION_NAME { get; set; }

        [DisplayName("Risk Mitigation Marks")]
        public string FU3RD_RISK_MITIGATION_MARKS { get; set; }

        [DisplayName("Reply Person")]
        public string FU3RD_REPLY_PERSON { get; set; }

        [DisplayName("Responsible Dept Head")]
        public string FU3RD_RESPONSIBLE_DEPT_HEAD { get; set; }

        [DisplayName("Remarks")]
        public string FU3RD_REMARKS { get; set; }

        [DisplayName("Significant Gap")]
        public string FU3RD_SIGNIFICANT_GAP { get; set; }

        [DisplayName("Next Follow Up")]
        public string FU3RD_FU_REQUIREMENT_NAME { get; set; }
    }

    public class AuditWithoutReportDetailsListVM
    {
        [DisplayName("Report SL NO")]
        public string AUDIT_RPT_ID { get; set; }
        [DisplayName("Task ID")]
        public string TASK_ID { get; set; }
        [DisplayName("Report Name")]
        public string AUDIT_RPT_NAME { get; set; }
        [DisplayName("Report Sent Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RPT_SENT_DATE { get; set; }

        [DisplayName("Location")]
        public string LOCATION_ID { get; set; }
        [DisplayName("Group")]
        public string COMPANY_GROUP_ID { get; set; }
        [DisplayName("Company")]
        public string COMPANY_ID { get; set; }
        [DisplayName("Department")]
        public string DEPARTMENT_ID { get; set; }
        [DisplayName("Prepared by")]
        public string CDU { get; set; }
        [DisplayName("Work Opportunity")]
        public string WORK_OPPORTUNITY { get; set; }
        [DisplayName("Work Scope")]
        public Nullable<decimal> WORK_SCOPE { get; set; }

        public string LOCATION_NAME { get; set; }
        public string COMPANY_GROUP_NAME { get; set; }
        public string COMPANY_NAME { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        //public string ACTION_NAME { get; set; }

        public string REPORT_CREATE_STATUS { get; set; }
        public string USER_NAME { get; set; }

        [DisplayName("Segment Type")]
        public string TASK_CATEGORY_TYPE_NAME { get; set; }
        [DisplayName("Segment Name")]
        public string TASK_CATEGORY_NAME { get; set; }
        [DisplayName("Saving Type")]
        public string AUDIT_SAVING_TYPE_NAME { get; set; }
        [DisplayName("Saving Name")]
        public string AUDIT_SAVING_NAME { get; set; }
        [DisplayName("Audit Action")]
        public string ACTION_NAME { get; set; }
        [DisplayName("Saving Amount")]
        public string SAVING_AMOUNT { get; set; }
        [DisplayName("Number")]
        public string VALUE { get; set; }

        [DisplayName("Direct Saving Type")]
        public string DIRECT_SAVING_NAME { get; set; }
        [DisplayName("Indirect Saving Type")]
        public string INDIRECT_SAVING_NAME { get; set; }

        [DisplayName("Direct Saving Amount")]
        public string DIRECT_SAVING_AMOUNT { get; set; }
        [DisplayName("Indirect Saving Amount")]
        public string INDIRECT_SAVING_AMOUNT { get; set; }

        [DisplayName("Demotion")]
        public string DEMOTION { get; set; }
        [DisplayName("Development")]
        public string DEVELOPMENT { get; set; }
        [DisplayName("Job Sack Out")]
        public string JOB_SACK_OUT { get; set; }
        [DisplayName("Transfer")]
        public string TRANSFER { get; set; }
        [DisplayName("Warning Letter")]
        public string WARNING_LETTER { get; set; }
        [DisplayName("Penalty")]
        public string PENALTY { get; set; }

        [DisplayName("Hold Payment")]
        public string HOLD_PAYMENT { get; set; }
        [DisplayName("Supplier Blacklist")]
        public string SUPPLIER_BLACKLIST { get; set; }


        [DisplayName("Remark")]
        public string REMARK { get; set; }

        [DisplayName("Management Action")]
        public string MANAGEMENT_ACTION_ID { get; set; }
        public string MANAGEMENT_ACTION_NAME { get; set; }
        //1st Fillowup

        [DisplayName("First Follow Up Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FIRST_FOLLOWUP_DATE { get; set; }

        [DisplayName("Follow Up Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FU1ST_FOLLOWUP_DATE { get; set; }

        [DisplayName("Total Findings")]
        public string FU1ST_TOTAL_FINDINGS { get; set; }
        [DisplayName("Action Findings")]
        public string FU1ST_ACTION_FINDINGS { get; set; }
        [DisplayName("No Action Findings")]
        public string FU1ST_NO_ACTION_FINDINGS { get; set; }
        [DisplayName("Action Percentage (%)")]
        public string FU1ST_ACTION_PERCENTAGE { get; set; }
        [DisplayName("Risk Type")]
        public string FU1ST_RISK_TYPE_NAME { get; set; }
        [DisplayName("Risk Mitigation")]
        public string FU1ST_RISK_MITIGATION_NAME { get; set; }
        [DisplayName("Risk Mitigation Marks")]
        public string FU1ST_RISK_MITIGATION_MARKS { get; set; }
        [DisplayName("Reply Person")]
        public string FU1ST_REPLY_PERSON { get; set; }
        [DisplayName("Responsible Dept Head")]
        public string FU1ST_RESPONSIBLE_DEPT_HEAD { get; set; }
        [DisplayName("Remarks")]
        public string FU1ST_REMARKS { get; set; }
        [DisplayName("Significant Gap")]
        public string FU1ST_SIGNIFICANT_GAP { get; set; }
        [DisplayName("Next Follow Up")]
        public string FU1ST_FU_REQUIREMENT_NAME { get; set; }

        //2nd Fillowup

        [DisplayName("Second Follow Up Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? SECOND_FOLLOWUP_DATE { get; set; }

        [DisplayName("Follow Up Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FU2ND_FOLLOWUP_DATE { get; set; }

        [DisplayName("Total Findings")]
        public string FU2ND_TOTAL_FINDINGS { get; set; }
        [DisplayName("Action Findings")]
        public string FU2ND_ACTION_FINDINGS { get; set; }
        [DisplayName("No Action Findings")]
        public string FU2ND_NO_ACTION_FINDINGS { get; set; }
        [DisplayName("Action Percentage (%)")]
        public string FU2ND_ACTION_PERCENTAGE { get; set; }
        [DisplayName("Risk Type")]
        public string FU2ND_RISK_TYPE_NAME { get; set; }
        [DisplayName("Risk Mitigation")]
        public string FU2ND_RISK_MITIGATION_NAME { get; set; }
        [DisplayName("Risk Mitigation Marks")]
        public string FU2ND_RISK_MITIGATION_MARKS { get; set; }
        [DisplayName("Reply Person")]
        public string FU2ND_REPLY_PERSON { get; set; }
        [DisplayName("Responsible Dept Head")]
        public string FU2ND_RESPONSIBLE_DEPT_HEAD { get; set; }
        [DisplayName("Remarks")]
        public string FU2ND_REMARKS { get; set; }
        [DisplayName("Significant Gap")]
        public string FU2ND_SIGNIFICANT_GAP { get; set; }
        [DisplayName("Next Follow Up")]
        public string FU2ND_FU_REQUIREMENT_NAME { get; set; }

        //3rd Fillowup

        [DisplayName("Third Follow Up Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? THIRD_FOLLOWUP_DATE { get; set; }

        [DisplayName("Follow Up Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FU3RD_FOLLOWUP_DATE { get; set; }

        [DisplayName("Total Findings")]
        public string FU3RD_TOTAL_FINDINGS { get; set; }
        [DisplayName("Action Findings")]
        public string FU3RD_ACTION_FINDINGS { get; set; }
        [DisplayName("No Action Findings")]
        public string FU3RD_NO_ACTION_FINDINGS { get; set; }
        [DisplayName("Action Percentage (%)")]
        public string FU3RD_ACTION_PERCENTAGE { get; set; }
        [DisplayName("Risk Type")]
        public string FU3RD_RISK_TYPE_NAME { get; set; }
        [DisplayName("Risk Mitigation")]
        public string FU3RD_RISK_MITIGATION_NAME { get; set; }
        [DisplayName("Risk Mitigation Marks")]
        public string FU3RD_RISK_MITIGATION_MARKS { get; set; }
        [DisplayName("Reply Person")]
        public string FU3RD_REPLY_PERSON { get; set; }
        [DisplayName("Responsible Dept Head")]
        public string FU3RD_RESPONSIBLE_DEPT_HEAD { get; set; }
        [DisplayName("Remarks")]
        public string FU3RD_REMARKS { get; set; }
        [DisplayName("Significant Gap")]
        public string FU3RD_SIGNIFICANT_GAP { get; set; }
        [DisplayName("Next Follow Up")]
        public string FU3RD_FU_REQUIREMENT_NAME { get; set; }


    }
    public class AuditReportFirstFollowUpVM
    {
        [DisplayName("Report SL NO")]
        public string AUDIT_RPT_ID { get; set; }
        [DisplayName("Report Follow Up ID")]
        public string AUDIT_RPT_FOLLOWUP_ID { get; set; }
        [DisplayName("Follow Up Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FOLLOWUP_DATE { get; set; }

        [DisplayName("Total Findings")]
        public string TOTAL_FINDINGS { get; set; }
        [DisplayName("Action Findings")]
        public string ACTION_FINDINGS { get; set; }
        [DisplayName("No Action Findings")]
        public string NO_ACTION_FINDINGS { get; set; }
        [DisplayName("Action Percentage (%)")]
        public string ACTION_PERCENTAGE { get; set; }
        [DisplayName("Risk Type")]
        public string RISK_TYPE_NAME { get; set; }
        [DisplayName("Risk Mitigation")]
        public string RISK_MITIGATION_NAME { get; set; }
        [DisplayName("Risk Mitigation Marks")]
        public string RISK_MITIGATION_MARKS { get; set; }
        [DisplayName("Reply Person")]
        public string REPLY_PERSON { get; set; }
        [DisplayName("Responsible Dept Head")]
        public string RESPONSIBLE_DEPT_HEAD { get; set; }
        [DisplayName("Remarks")]
        public string REMARKS { get; set; }
        [DisplayName("Significant Gap")]
        public string SIGNIFICANT_GAP { get; set; }
        [DisplayName("Next Follow Up")]
        public string FOLLOWUP_REQUIREMENT_NAME { get; set; }
        [DisplayName("Create User")]
        public string CDU { get; set; }
        [DisplayName("Create Date")]
        public string CDT { get; set; }

    }
    public class AuditReportDetails
    {
        public string AUDIT_RPT_ID { get; set; }

        [DisplayName("Line")]
        public int LINE { get; set; }
        [DisplayName("Segment Type")]
        public string TASK_CATEGORY_TYPE_ID { get; set; }
        [DisplayName("Segment Name")]
        public string TASK_CATEGORY_ID { get; set; }
        [DisplayName("Saving Type")]
        public string AUDIT_SAVING_TYPE_ID { get; set; }
        [DisplayName("Saving Name")]
        public string AUDIT_SAVING_NAME_ID { get; set; }

        public string TASK_CATEGORY_TYPE_NAME { get; set; }
        public string TASK_CATEGORY_NAME { get; set; }
        public string AUDIT_SAVING_TYPE_NAME { get; set; }
        public string AUDIT_SAVING_NAME { get; set; }
        [DisplayName("Saving Amount")]
        public Nullable<decimal> SAVING_AMOUNT { get; set; }

        public string RPT_ACTION_NAME { get; set; }

        [DisplayName("Action Name")]
        //[Required(ErrorMessage = "Item is required")]
        public string RPT_ACTION_ID { get; set; }

        [DisplayName("Number")]
        public string VALUE { get; set; }

        [DisplayName("Remark")]
        [Required(ErrorMessage = "Qty is required")]
        public string REMARK { get; set; }
    }
    public class AuditFollowUpReportVM
    {
        public string AUDIT_RPT_ID { get; set; }
        public string AUDIT_RPT_FOLLOWUP_ID { get; set; }
        public int FOLLOWUP_NUMBER { get; set; }

        [DisplayName(" First Follow Up Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FOLLOWUP_DATE { get; set; }

        [DisplayName("Total Findings")]
        public int TOTAL_FINDINGS { get; set; }
        [DisplayName("Action Findings")]
        public int ACTION_FINDINGS { get; set; }
        [DisplayName("No Action Findings")]
        public int NO_ACTION_FINDINGS { get; set; }
        [DisplayName("Action Percentage")]
        public decimal ACTION_PERCENTAGE { get; set; }

        public string RISK_TYPE_ID { get; set; }
        [DisplayName("Risk Mitigation")]
        public string RISK_MITIGATION_ID { get; set; }

        [DisplayName("Risk Mitigation Marks")]
        public string RISK_MITIGATION_MARKS { get; set; }
        [DisplayName("Reply Person")]
        public string REPLY_PERSON { get; set; }
        [DisplayName("Responsible Department Head")]
        public string RESPONSIBLE_DEPT_HEAD { get; set; }
        [DisplayName("Remarks")]
        public string REMARKS { get; set; }
        [DisplayName("Significant Gap")]
        public string SIGNIFICANT_GAP { get; set; }
        [DisplayName("Follow Up Requirement")]
        public string FOLLOWUP_REQUIREMENT_ID { get; set; }
        public string FIRST_FOLLOWUP_STATUS { get; set; }
        public string SECOND_FOLLOWUP_STATUS { get; set; }
        public string THIRD_FOLLOWUP_STATUS { get; set; }

    }


}