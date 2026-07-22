using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PM.ModelsV
{
    public class SummaryView
    {


    }
    public class SummaryViewKSA
    {
        public List<TopTenItem> topTenItem { get; set; }
        public List<LocationCompanyWiseLicenseCount> locationCompanyWiseLicenseCount { get; set; }
        public List<AuditorWiseAuditReportCount> AuditorWiseAuditReportCount { get; set; }
        public List<SegmentConpamyGroupWiseAuditReportCount> SegmentConpamyGroupWiseAuditReportCount { get; set; }
        public List<CompanyNameCompanyGroupWiseAuditReportCount> CompanyNameCompanyGroupWiseAuditReportCount { get; set; }
        public List<CompanyGroupWiseAuditReportCount> CompanyGroupWiseAuditReportCount { get; set; }
        public List<AuditorWiseAuditFollowUpReportCount> AuditorWiseAuditFollowUpReportCount { get; set; }
        public List<BUWiseAuditFollowUpReportStatusCount> BUWiseAuditFollowUpReportStatusCount { get; set; }
        

        public List<ReportSummary> ReportSummary { get; set; }
        public List<CategoryCGWiseTotalReport> CategoryCGWiseTotalReport { get; set; }
        

        public List<CategoryWiseWorkScopeTotalReport> CategoryWiseWorkScopeTotalReport { get; set; }
        public List<BUCGWiseTotalReport> BUCGWiseTotalReport { get; set; }
        public List<BUCGWiseTotalWorkScopeReport> BUCGWiseTotalWorkScopeReport { get; set; }


        public string UserRole { get; set; }


        public List<LicenseExpireInformation> licenseExpireInformation { get; set; }
        
        public List<TopAsset> topAsset { get; set; }
        public List<TopStaff> topStaff { get; set; }
        public Update_Date updateDate { get; set; }
        public List<Asset_Body_Area> bodyArea { get; set; }
        public List<Area_Wise_Cost> costArea { get; set; }
        public List<IqumaExpired> expireList { get; set; }
    }
    public class TopTenItem
    {
        public string ITEM_ID { get; set; }
        public int CNT { get; set; }
        public string ITEM_NAME { get; set; }
    }
    public class LocationCompanyWiseLicenseCount
    {
        public string LOCATION_NAME { get; set; }
        public int Total { get; set; }
        public string COMPANY_NAME { get; set; }
        public string COMPANY_ID { get; set; }
        public string LIC_LOCATION_ID { get; set; }
    }
    public class AuditorWiseAuditReportCount
    {
        public string AUDITOR_STAFF_ID { get; set; }
        public string AUDITOR_NAME { get; set; }
        public int TOTAL_REPORT { get; set; }
        public string TOTAL_WITH_REPORT { get; set; }
        public int TOTAL_WITHOUT_REPORT { get; set; }
        public Nullable<decimal> TOTAL_WORK_SCOPE { get; set; }
        public Nullable<decimal> TOTAL_DIRECT_SAVING_AMOUNT { get; set; }
        public Nullable<decimal> TOTAL_INDIRECT_SAVING_AMOUNT { get; set; }
        public int TOTAL_DEMOTION { get; set; }
        public int TOTAL_DEVELOPMENT { get; set; }
        public int TOTAL_JOB_SACK_OUT { get; set; }
        public int TOTAL_TRANSFER { get; set; }
        public int TOTAL_WARNING_LETTER { get; set; }
        public int TOTAL_PENALTY { get; set; }
        public int? TOTAL_HOLD_PAYMENT { get; set; }
        public int? TOTAL_SUPPLIER_BLACKLIST { get; set; }
        public int? TOTAL_INEFFICIENCY_IDENTIFIED { get; set; }
        public int? TOTAL_CORRUPTION_IDENTIFIED { get; set; }
    }
    
    public class SegmentConpamyGroupWiseAuditReportCount
    {
        public string COMPANY_GROUP_NAME { get; set; }
        public string TASK_CATEGORY_NAME { get; set; }
        public int? TOTAL_REPORT { get; set; }
        public Nullable<decimal> TOTAL_WORK_SCOPE { get; set; }
        public Nullable<decimal> TOTAL_DIRECT_SAVING_AMOUNT { get; set; }
        public Nullable<decimal> TOTAL_INDIRECT_SAVING_AMOUNT { get; set; }
        public int? TOTAL_DEMOTION { get; set; }
        public int? TOTAL_DEVELOPMENT { get; set; }
        public int? TOTAL_JOB_SACK_OUT { get; set; }
        public int? TOTAL_TRANSFER { get; set; }
        public int? TOTAL_WARNING_LETTER { get; set; }
        public int? TOTAL_PENALTY { get; set; }
        public int? TOTAL_HOLD_PAYMENT { get; set; }
        public int? TOTAL_SUPPLIER_BLACKLIST { get; set; }
        public int? TOTAL_INEFFICIENCY_IDENTIFIED { get; set; }
        public int? TOTAL_CORRUPTION_IDENTIFIED { get; set; }
    }
    public class CompanyNameCompanyGroupWiseAuditReportCount
    {
        public string COMPANY_GROUP_NAME { get; set; }
        public string COMPANY_NAME { get; set; }
        public int? TOTAL_REPORT { get; set; }
        public Nullable<decimal> TOTAL_WORK_SCOPE { get; set; }
        public Nullable<decimal> TOTAL_DIRECT_SAVING_AMOUNT { get; set; }
        public Nullable<decimal> TOTAL_INDIRECT_SAVING_AMOUNT { get; set; }
        public int? TOTAL_DEMOTION { get; set; }
        public int? TOTAL_DEVELOPMENT { get; set; }
        public int? TOTAL_JOB_SACK_OUT { get; set; }
        public int? TOTAL_TRANSFER { get; set; }
        public int? TOTAL_WARNING_LETTER { get; set; }
        public int? TOTAL_PENALTY { get; set; }
        public int? TOTAL_HOLD_PAYMENT { get; set; }
        public int? TOTAL_SUPPLIER_BLACKLIST { get; set; }
        public int? TOTAL_INEFFICIENCY_IDENTIFIED { get; set; }
        public int? TOTAL_CORRUPTION_IDENTIFIED { get; set; }
    }
    
    public class CompanyGroupWiseAuditReportCount
    {
        public string COMPANY_GROUP_NAME { get; set; }
        public int? TOTAL_REPORT { get; set; }
        public int? TOTAL_COMPLETED_WITH_REPORT { get; set; }
        public int? TOTAL_COMPLETED_WITHOUT_REPORT { get; set; }
    }

    public class AuditorWiseAuditFollowUpReportCount
    {
        public string AUDITOR_NAME { get; set; }
        public string TOTAL_REPORT { get; set; }
        public string TOTAL_FOLLOWUP { get; set; }
        public string FIRST_FOLLOWUP { get; set; }
        public string SECOND_FOLLOWUP { get; set; }
        public string THIRD_FOLLOWUP { get; set; }
        public string TOTAL_FINDINGS { get; set; }
        public string ACTION_DONE { get; set; }
        public string NO_ACTION { get; set; }
        public string ACTION_PROGRESS { get; set; }
    }
    
    public class BUWiseAuditFollowUpReportStatusCount
    {
        public string COMPANY_NAME { get; set; }
        public string TOTAL_REPORT { get; set; }
        public string TOTAL_FOLLOWUP { get; set; }
        public string FIRST_FOLLOWUP { get; set; }
        public string SECOND_FOLLOWUP { get; set; }
        public string THIRD_FOLLOWUP { get; set; }
        public string TOTAL_FINDINGS { get; set; }
        public string ACTION_DONE { get; set; }
        public string NO_ACTION { get; set; }
        public string ACTION_PROGRESS { get; set; }
    }

    public class ReportSummary
    {
        public string COMPANY_GROUP_NAME { get; set; }
        public string TOTAL_NO_OF_REPORT { get; set; }
        public string LAST_MONTH_TOTAL_REPORT { get; set; }
        public string THIS_MONTH_TOTAL_REPORT { get; set; }
        public string TOTAL_WITH_REPORT { get; set; }
        public string LAST_MONTH_WITH_REPORT { get; set; }
        public string THIS_MONTH_WITH_REPORT { get; set; }
        public string TOTAL_WITHOUT_REPORT { get; set; }
        public string LAST_MONTH_WITHOUT_REPORT { get; set; }
        public string THIS_MONTH_WITHOUT_REPORT { get; set; }
        

    }

    public class CategoryCGWiseTotalReport
    {
        public string TASK_CATEGORY_NAME { get; set; }
        public int TOTAL_REPORT_CATEGORY { get; set; }
        public int PRAN { get; set; }
        public int RFL { get; set; }
        public int CTL { get; set; }
        public int GROUP_CS { get; set; }
        public int PDL { get; set; }

    }
    
    public class CategoryCGWiseDetails
    {
        public string TASK_CATEGORY_ID { get; set; }
        public string TASK_CATEGORY_NAME { get; set; }
        public int TOTAL_REPORT_CATEGORY { get; set; }
        public int PRAN { get; set; }
        public int RFL { get; set; }
        public int CTL { get; set; }
        public int PDL { get; set; }
        public int GROUP_CS { get; set; }
    }

    public class CategoryWiseWorkScopeTotalReport
    {
        public string TASK_CATEGORY_NAME { get; set; }
        public Nullable<decimal> TOTAL_WORK_SCOPE { get; set; }
        public Nullable<decimal> PRAN { get; set; }
        public Nullable<decimal> RFL { get; set; }
        public Nullable<decimal> CTL { get; set; }
        public Nullable<decimal> GROUP_CS { get; set; }
        public Nullable<decimal> PDL { get; set; }

    }
    
    public class BUCGWiseTotalReport
    {
        public string COMPANY_NAME { get; set; }
        public int TOTAL_REPORT_BU { get; set; }
        public int PRAN { get; set; }
        public int RFL { get; set; }
        public int CTL { get; set; }
        public int GROUP_CS { get; set; }
        public int PDL { get; set; }

    }
    public class BUCGWiseTotalWorkScopeReport
    {
        public string COMPANY_NAME { get; set; }
        public Nullable<decimal> TOTAL_WORK_SCOPE { get; set; }
        public Nullable<decimal> PRAN { get; set; }
        public Nullable<decimal> RFL { get; set; }
        public Nullable<decimal> CTL { get; set; }
        public Nullable<decimal> GROUP_CS { get; set; }
        public Nullable<decimal> PDL { get; set; }

    }


public class LicenseExpireInformation
    {
        public string LICENSE_ID { get; set; }
        public int REMAINING { get; set; }
        public string LICENSE_NUMBER { get; set; }
        public string ISSUE_DATE { get; set; }
        public string EXPIRE_DATE { get; set; }
        public string LOCATION_NAME { get; set; }
        public string LICENSE_HOLDER_MOBILE { get; set; }

    }
    public class LicenseDetailsInformation
    {
        [DisplayName("License ID")]
        public string LICENSE_ID { get; set; }
        [DisplayName("Company")]
        public string COMPANY_ID { get; set; }
        [DisplayName("Location")]
        public string LIC_LOCATION_ID { get; set; }
        [DisplayName("Item Name")]
        public string ITEM_ID { get; set; }
        [DisplayName("License Name")]
        public string LICENSE_NAME_ID { get; set; }
        [DisplayName("License Number")]
        public string LICENSE_NUMBER { get; set; }
        [DisplayName("Issuing Authority")]
        public string ISSUING_AUTHORITY_ID { get; set; }

        [DisplayName("Issue Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ISSUE_DATE { get; set; }

        [DisplayName("Expire Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EXPIRE_DATE { get; set; }

        [DisplayName("License Holder Mobile")]
        public string LICENSE_HOLDER_MOBILE { get; set; }
        [DisplayName("License Holder Email")]
        public string LICENSE_HOLDER_EMAIL { get; set; }
        [DisplayName("License Fee")]
        public int LICENSE_FEE { get; set; }
        [DisplayName("Remark")]
        public string REMARK { get; set; }
        [DisplayName("Status")]
        public string LICENSE_STATUS_ID { get; set; }
        [DisplayName("Application Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime APPLICATION_DATE { get; set; }
        public Nullable<bool> ACT { get; set; }
        //public HttpPostedFileBase FileX { get; set; }
        [DisplayName("Entertainment Fee")]
        public int ENTERTAINMENT_FEE { get; set; }
        [DisplayName("Create By")]
        public string CDU { get; set; }

        public string COMPANY_NAME { get; set; }
        public string LOCATION_NAME { get; set; }
        [DisplayName("License Name")]
        public string LICENSE_NAME { get; set; }
        [DisplayName("Authority Name")]
        public string AUTHORITY_NAME { get; set; }
        public string LICENSE_STATUS_NAME { get; set; }
        [DisplayName("Item Name")]
        public string ITEM_NAME { get; set; }

    }
    public class AuditReportDetailsInfo
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
    public class TopAsset
    {
        public string ASET_TAG { get; set; }
        public int CNT { get; set; }
        public string ASET_CODE_NAME { get; set; }

    }
    public class TopStaff
    {
        public string OWNR_TEXT { get; set; }
        public int CNT { get; set; }
        public string OWNR_USER { get; set; }
    }
    public class Update_Date
    {
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<DateTime> CAR_DATE { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<DateTime> STAFF_DATE { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<DateTime> POLICE_DATE { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<DateTime> CL_DATE { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<DateTime> CM_DATE { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<DateTime> AWA_DATE { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<DateTime> AWC_DATE { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<DateTime> IQUMA_DATE { get; set; }
        public Nullable<DateTime> COMPANY_GROUP_DATE { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]

        public Nullable<DateTime> CURRENT_LAST_MONTH_DATE { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<DateTime> REPORT_STATUS_DATE { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<DateTime> REPORT_SENT_STATUS_DATE { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<DateTime> TASK_STATUS_DATE { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
       
        public int TOTAL_TASK { get; set; }
        public int TOTAL_AUDIT_REPORT { get; set; }

        public int TOTAL_AUDITOR { get; set; }
        public decimal TOTAL_WORK_SCOPE { get; set; }
        public decimal TOTAL_DIRECT_SAVING_AMOUNT { get; set; }
        public decimal TOTAL_INDIRECT_SAVING_AMOUNT { get; set; }
        public decimal TOTAL_SAVING_AMOUNT { get; set; }
        public int TOTAL_DEMOTION { get; set; }
        public decimal TOTAL_DEVELOPMENT { get; set; }

        public int TOTAL_JOB_SACK_OUT { get; set; }
        public int TOTAL_TRANSFER { get; set; }
        public int TOTAL_WARNING_LETTER { get; set; }
        public decimal TOTAL_PENALTY { get; set; }

        public decimal TOTAL_HOLD_PAYMENT { get; set; }
        public int TOTAL_SUPPLIER_BLACKLIST { get; set; }
        public decimal TOTAL_INEFFICIENCY_IDENTIFIED { get; set; }
        public decimal TOTAL_CORRUPTION_IDENTIFIED { get; set; }
        public int TOTAL_REPORT_SENT { get; set; }
        public int TOTAL_AUDIT_WITH_REPORT { get; set; }
        public int TOTAL_AUDIT_WITHOUT_REPORT { get; set; }






        public int TOTAL_LICENSE { get; set; }
        public int TOTAL_LOCATION { get; set; }
        public int TOTAL_ENTRY_LOCATION { get; set; }
        public int TOTAL_COMPANY { get; set; }
        public int TOTAL_ENTRY_COMPANY { get; set; }
        
    }

    //public class License_Dashboard_View
    //{
    //    public int TOTAL_LICENSE { get; set; }
    //    public int TOTAL_LOCATION { get; set; }
    //    public int TOTAL_COMPANY { get; set; }

    //    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    //    public Nullable<DateTime> CAR_DATE { get; set; }
    //    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    //    public Nullable<DateTime> STAFF_DATE { get; set; }
    //}



    public class Asset_Body_Area
    {
        public Nullable<int> CNT { get; set; }
        public string ASET_BODY { get; set; }
        public string BODY_NAME { get; set; }
        public string AU_AREA { get; set; }
        public string AREA_NAME { get; set; }

    }
    public class Area_Wise_Cost
    {
        public string JOBH_AREA { get; set; }
        public string AREA_NAME { get; set; }
        public Nullable<int> INSD { get; set; }
        public Nullable<int> JOBD { get; set; }
        public string JOBH_AREA_1 { get; set; }
        public Nullable<int> INSD_1 { get; set; }
        public Nullable<int> JOBD_1 { get; set; }

    }
    public class IqumaExpired
    {
        public string User_Name { get; set; }
        public string Iquma_No { get; set; }
        public string Expire_Date { get; set; }

    }
}