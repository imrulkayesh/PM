using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PM.ModelsV
{
    public class ReportManagerVM
    {
    }
    public class Report_Pending_JobCard_ForBill
    {
        [DisplayName("Job Number")]
        public string JOBH_ID { get; set; }
        [DisplayName("Asset")]
        public string JOBH_ASET { get; set; }
        [DisplayName("User")]
        public string JOBH_OWNR { get; set; }
        [DisplayName("Department")]
        public string JOBH_DEPARTMENT { get; set; }
        [DisplayName("Area")]
        public string JOBH_AREA { get; set; }
        [DisplayName("Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime JOBH_DATE { get; set; }
        [DisplayName("MIleage")]
        public string JOBH_MILEAGE { get; set; }
        [DisplayName("Vendor")]
        public string JOBH_VENDOR { get; set; }
        [DisplayName("Note")]
        public string JOBH_NOTE { get; set; }
        [DisplayName("Insurance")]
        public decimal JOBH_IPAY { get; set; }
        [DisplayName("User")]
        public decimal JOBH_UPAY { get; set; }
        [DisplayName("Company")]
        public decimal JOBH_CPAY { get; set; }
        [DisplayName("I Total")]
        public decimal JOBH_IT { get; set; }
        [DisplayName("J Total")]
        public decimal JOBH_JT { get; set; }
        [DisplayName("Approval")]
        public string JOBH_APPR { get; set; }

    }
    public class Report_Pending_Inspection_ForJobCard
    {
        [DisplayName("Inspection")]
        public string INSPH_ID { get; set; }

        [DisplayName("Asset")]
        public string INSPH_ASET { get; set; }

        [DisplayName("User")]
        public string INSPH_OWNR { get; set; }

        [DisplayName("Department")]
        public string INSPH_DEPARTMENT { get; set; }

        [DisplayName("Area")]
        public string INSPH_AREA { get; set; }

        [DisplayName("Inspection User")]
        public string INSPH_IUSER { get; set; }

        [DisplayName("Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime INSPH_DATE { get; set; }

        [DisplayName("Mileage")]
        public string MILEAGE { get; set; }

        [DisplayName("Approval")]
        public string INSPH_APPR { get; set; }

    }
    public class Report_Paid_Bills
    {
        [Required(ErrorMessage = "From Date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime F_BILH_DATE { get; set; }

        [Required(ErrorMessage = "To Date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime T_BILH_DATE { get; set; }
        public string JOBH_ASET { get; set; }
        public string JOBH_OWNR { get; set; }
        public string DEPARTMENT_BUSINESS { get; set; }
        public string JOBH_AREA { get; set; }
        public string JOBH_TYPE { get; set; }
        public string AREA_ID { get; set; }
        public string BUSINESS_ID { get; set; }
    }
    public class Report_Paid_Bills_Download
    {

        public string BILH_ID { get; set; }
        public Nullable<System.DateTime> BILH_DATE { get; set; }
        public string BILH_NOTE { get; set; }
        public string BILH_REF { get; set; }
        public string BILH_RECEIVER { get; set; }
        public Nullable<decimal> BILH_IPAY { get; set; }
        public Nullable<decimal> BILH_UPAY { get; set; }
        public Nullable<decimal> BILH_CPAY { get; set; }
        public string ASET_CODE_NAME { get; set; }
        public string OWNR_USER { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string BUSINESS_NAME { get; set; }
        public string AREA_NAME { get; set; }
        public string JOBH_TYPE { get; set; }
    }
    public class Month_Wise_Report
    {
        [Required(ErrorMessage = "From Date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime F_BILH_DATE { get; set; }

        [Required(ErrorMessage = "To Date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime T_BILH_DATE { get; set; }
        //[Required(ErrorMessage = "Area is required")]
        public string AREA_ID { get; set; }
        public string BUSINESS_ID { get; set; }
    }

    public class Monthly_Area_Wise_Report
    {
        [Required(ErrorMessage = "From Date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime F_BILH_DATE { get; set; }

        [Required(ErrorMessage = "To Date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime T_BILH_DATE { get; set; }
        [Required(ErrorMessage = "Area is required")]
        public string AREA_ID { get; set; }
        public string BUSINESS_ID { get; set; }
    }

    public class Asset_Wise_Report
    {
        [Required(ErrorMessage = "From Date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime F_BILH_DATE { get; set; }

        [Required(ErrorMessage = "To Date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime T_BILH_DATE { get; set; }

        [Required(ErrorMessage = "Asset is required")]
        public string ASET_ID { get; set; }
    }
    public class Zone_Wise_Report
    {
        [Required(ErrorMessage = "From Date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime F_BILH_DATE { get; set; }

        [Required(ErrorMessage = "To Date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime T_BILH_DATE { get; set; }

        [Required(ErrorMessage = "Area is required")]
        public string AREA_ID { get; set; }
    }
    public class Business_Wise_Report
    {
        [Required(ErrorMessage = "From Date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime F_BILH_DATE { get; set; }

        [Required(ErrorMessage = "To Date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime T_BILH_DATE { get; set; }

        [Required(ErrorMessage = "Business is required")]
        public string BUSINESS_ID { get; set; }
    }
    public class Item_Wise_Report
    {
        [Required(ErrorMessage = "From Date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime F_BILH_DATEx { get; set; }

        [Required(ErrorMessage = "To Date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime T_BILH_DATEx { get; set; }

        [Required(ErrorMessage = "Items are required")]
        public List<Item_Wise_Report_X> ITEM_IDx { get; set; }
    }
    public class Item_Wise_Report_X
    {
        [Required(ErrorMessage = "Item is required")]
        public string JOB_ITEM { get; set; }
    }

    public class Report_Police_Case
    {
        [Required(ErrorMessage = "From Date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime F_BILH_DATE { get; set; }

        [Required(ErrorMessage = "To Date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime T_BILH_DATE { get; set; }
        public string ASET_ID { get; set; }
        public string AREA_ID { get; set; }
        public string BUSINESS_ID { get; set; }
        public string TYPE_ID { get; set; }
    }
    public class Report_Police_Case_View
    {
        public string PLEA_ID { get; set; }
        public string PLEA_REF { get; set; }
        public string PLEA_DESCRIPTION { get; set; }
        public string PLEA_LOCATION { get; set; }
        public string Case_On { get; set; }
        public string Paid_By { get; set; }
        public string REASON_NAME { get; set; }
        public string OWNR_TEXT { get; set; }
        public string OWNR_USER { get; set; }
        public string ASET_CODE_NAME { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string BUSINESS_NAME { get; set; }
        public string AREA_NAME { get; set; }
        public Nullable<System.DateTime> PLEA_DATE { get; set; }
        public string PLEA_STATUS { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> PLEA_APP_DATE { get; set; }
        public string Approval { get; set; }
        public Nullable<int> PLEA_AMNT { get; set; }
    }




    public class JobCardReports
    {
        [Required(ErrorMessage = "From Date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime F_BILH_DATE { get; set; }

        [Required(ErrorMessage = "To Date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime T_BILH_DATE { get; set; }
        public string ASET_ID { get; set; }
        public string AREA_ID { get; set; }
        public string BUSINESS_ID { get; set; }
        public string ITEM_ID { get; set; }
        public string JOBC_TYPE { get; set; }
    }


    public class SubTotal_Report_JobCard_Month
    {
        public string JOBH_ID { get; set; }
        public Nullable<decimal> JOBH_VALUE { get; set; }
        public Report_JobCard_Month JOBH_ITEMS { get; set; }
    }

    public class Report_JobCard_Month
    {
        public string JOBH_ID { get; set; }
        public Nullable<System.DateTime> JOBH_DATE { get; set; }
        public byte JOBD_LINE { get; set; }
        public string JOBD_ITEM { get; set; }
        public string ITEM_NAME { get; set; }
        public Nullable<byte> JOBD_JQTY { get; set; }
        public Nullable<int> JOBD_JRATE { get; set; }
        public Nullable<decimal> JOBD_JAMNT { get; set; }
        public string JOBD_NOTE { get; set; }
        public string ASET_CODE_NAME { get; set; }
        public string OWNR_USER { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string BUSINESS_NAME { get; set; }
        public string AREA_NAME { get; set; }
        public string JOBH_MILEAGE { get; set; }
        public string JOBH_VENDOR { get; set; }
        public string JOBH_NOTE { get; set; }
        public string ITYPE_NAME { get; set; }
        public string INSPH_ID { get; set; }
        public string INSPH_IUSER { get; set; }
        public string CDU { get; set; }
    }
    public class inspXjobcXbill
    {
        public string INSPH_ASET { get; set; }
        public string INSPH_ID { get; set; }
        public Nullable<System.DateTime> INSPH_DATE { get; set; }
        public Nullable<decimal> INSP_AMT { get; set; }
        public string JOBH_ID { get; set; }
        public Nullable<System.DateTime> JOBH_DATE { get; set; }
        public Nullable<decimal> JOBH_AMT { get; set; }
        public string BILH_ID { get; set; }
        public Nullable<System.DateTime> BILH_DATE { get; set; }
        public Nullable<decimal> BILL_AMT { get; set; }
        public string INSPH_OWNR { get; set; }
        public string OWNR_NAME { get; set; }
        public string INSPH_AREA { get; set; }
        public string INSPH_DEPARTMENT { get; set; }
        public string INSPH_BUSINESS { get; set; }
    }
    public class Report_Insurance_Data
    {
        public string ASET_CODE_NAME { get; set; }
        public string AREA_NAME { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string BUSINESS_NAME { get; set; }
        public string INSU_ID { get; set; }
        public int INSU_LINE { get; set; }
        public Nullable<int> INSU_ASET_VALUE { get; set; }
        public Nullable<int> INSU_PREMIUM { get; set; }
        public Nullable<int> INSU_VAT_AMOUNT { get; set; }
        public Nullable<System.DateTime> INSU_DATE { get; set; }
        public string INSU_PAID { get; set; }
        public Nullable<bool> DACT { get; set; }
        public string INSU_POLICY { get; set; }
        public string INSU_COMP { get; set; }
        public string INSU_COMP_ADDRESS { get; set; }
        public string INSU_NOTE { get; set; }
        public Nullable<System.DateTime> INSU_REG_END { get; set; }
        public Nullable<bool> HACT { get; set; }
    }


    public class Report_StaffList_KSA
    {
        public string OWNR_TEXT { get; set; }
        public string OWNR_USER { get; set; }
        public string OWNR_NAME { get; set; }
        public string OWNR_DESG { get; set; }
        public string OWNR_CONT { get; set; }
        public string OWNR_EMAIL { get; set; }
        public string OWNER_AREA { get; set; }
        public string OWNER_COMPANY { get; set; }
        public string OWNR_ADDR { get; set; }
        public string OWNR_NOTE { get; set; }
        public string OWNR_TYPE { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> OWNR_NDATE { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> OWNR_RDATE { get; set; }
        public string OWNR_POST { get; set; }
        public string ACT { get; set; }
        public string REMIND { get; set; }
    }


    public partial class Report_VW_RPT_ASET_USER
    {
        public string TAG { get; set; }
        public string CODE_NAME { get; set; }
        public string ASET_DEPT { get; set; }
        public string ASET_AREA { get; set; }
        public string USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public string WORKING_DEPT { get; set; }
        public string WORKING_AREA { get; set; }
        public Nullable<System.DateTime> START_DATE { get; set; }
        public string START_NOTE { get; set; }
        public Nullable<System.DateTime> END_DATE { get; set; }
        public string END_NOTE { get; set; }
        public string STAUTS { get; set; }
    }

    public class InsuranceVMBDRpt
    {
        public string INSU_ID { get; set; }
        [Required]
        [DisplayName("Asset")]
        public string ASET_CODE_NAME { get; set; }
        [Required]
        [DisplayName("Policy")]
        public string INSU_POLICY { get; set; }
        [Required]
        [DisplayName("Asset Capacity")]
        public string ASET_CAPACITY { get; set; }
        [Required]
        [DisplayName("Premium")]
        public int INSU_PREMIUM { get; set; }
        [Required]
        [DisplayName("Company")]
        public string INSU_COMP { get; set; }
        [DisplayName("Address")]
        public string INSU_COMP_ADDRESS { get; set; }
        [DisplayName("Note")]
        public string INSU_NOTE { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DisplayName("Reg Date")]
        public DateTime INSU_REG_START { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DisplayName("Expire Date")]
        public DateTime INSU_REG_END { get; set; }
    }
    public class Report_Insurance_By_Company_Detail
    {
        public string BUSINESS_NAME { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string ASET_CODE_NAME { get; set; }
        public string ASET_CAPACITY { get; set; }
        public string INSU_POLICY { get; set; }
        public int INSU_PREMIUM { get; set; }
        public string INSU_COMP { get; set; }
        public DateTime INSU_REG_START { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime INSU_REG_END { get; set; }
    }
    public class Report_Insurance_By_Company_Summary
    {
        public string BUSINESS_NAME { get; set; }
        public int INSU_PREMIUM { get; set; }
    }

    public class Report_Closed_Bike
    {
        public string aset_code_name { get; set; }
        public string aset_capacity { get; set; }
        public DateTime aset_reg_date { get; set; }
        public string aset_department { get; set; }
        public string aset_business { get; set; }
        public string ownr_text { get; set; }
        public string ownr_user { get; set; }
        public string ownr_department { get; set; }
        public string ownr_business { get; set; }
        public DateTime apply_date { get; set; }
        public string apply_reason { get; set; }
        public string apply_type { get; set; }
        public int service_month { get; set; }

    }

    public class LicenseExpireInfo
    {
        [DisplayName("License ID")]
        public string LICENSE_ID { get; set; }
        [DisplayName("License Number")]
        public string LICENSE_NUMBER { get; set; }
        [DisplayName("License Holder Email")]
        public string LICENSE_HOLDER_EMAIL { get; set; }

        [DisplayName("Issue Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ISSUE_DATE { get; set; }

        [DisplayName("Expire Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EXPIRE_DATE { get; set; }

        [DisplayName("Remaining")]
        public int REMAINING { get; set; }
    }
}