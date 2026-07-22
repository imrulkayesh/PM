using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace PM.ModelsV
{
    public class InsuranceManagerVM
    {
        public string INSURANCE_TYPE_ID { get; set; }
        public string INSURANCE_TYPE_NAME { get; set; }
        public string REMARKS { get; set; }
        public bool ACT { get; set; }
    }
    public class InsuranceCompanyVM
    {
        public string INSURANCE_COMPANY_ID { get; set; }
        public string INS_COMPANY_NAME { get; set; }
        public string ADDRESS { get; set; }
        public string MOBILE { get; set; }
        public string EMAIL { get; set; }
        public bool ACT { get; set; }
    }
    public class ClaimTypeVM
    {
        public string CLAIM_TYPE_ID { get; set; }
        public string CLAIM_NAME { get; set; }
        public string REMARK { get; set; }
        public bool ACT { get; set; }
    }
    public class ClaimStatusVM
    {
        public string CLAIM_STATUS_ID { get; set; }
        public string CLAIM_STATUS_NAME { get; set; }
        public string REMARK { get; set; }
        public int ACT { get; set; }
    }
    public class InsuranceMasterVM
    {
        public string INSURANCE_MST_ID { get; set; }
        [DisplayName("Company Name")]
        public string COMPANY_ID { get; set; }
        [DisplayName("Types of Policy")]

        public string BENIFICIARY_DETAILS { get; set; }
        public string INSURED_ITEM { get; set; }
        public string OCCUPATION { get; set; }
        [DisplayName("Insurance Type")]
        public string INSURANCE_TYPE_ID { get; set; }
        [DisplayName("Insurance Company")]
        public string INSURANCE_COMPANY_ID { get; set; }
        [DisplayName("Policy Number")]
        public string INSURANCE_NUMBER { get; set; }
        [DisplayName("Registration Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? INSURANE_DATE { get; set; }

        [DisplayName("Expire Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? INSURANCE_EXPAIR_DATE { get; set; }
        //[Required(ErrorMessage = "Insurance Amount is required.")]
        //[RegularExpression(@"^\d+$", ErrorMessage = "Insurance Amount must be a valid number.")]
        //[Range(1, 99999999999, ErrorMessage = "Insurance Amount must be a valid number between 1 and 99,999,999,999.")]
        [DisplayName("Insurred Amount")]
        public long? INSURANCE_AMOUNT { get; set; }
        [DisplayName("Net Premium")]
        public int? NET_PREMIUM { get; set; }
        [DisplayName("VAT")]
        public int? VAT { get; set; }
        [DisplayName("Stamp")]
        public int? STAMP { get; set; }
        [DisplayName("Total Premium")]
        public int? TOTAL_PREMIUM { get; set; }
        [DisplayName("Description")]
        public string DESCRIPTION { get; set; }
        [DisplayName("Discount Amount")]
        public int? KICK_BACK_AMOUNT { get; set; }
        [DisplayName("Discount (%)")]
        public decimal? KICK_BACK_PERCENTAGE { get; set; }
        [DisplayName("Actual Paid")]
        public int? ACTUAL_PAID_AMOUNT { get; set; }
        [DisplayName("Status")]
        public string KICK_BACK_STATUS { get; set; }
        [DisplayName("Request By")]
        public string REQUEST_BY { get; set; }
        [DisplayName("Insurance Status")]
        public Nullable<bool> INSURANCE_STATUS { get; set; }

        public HttpPostedFileBase FileX { get; set; }

        public string COMPANY_NAME { get; set; }
        public string INSURANCE_TYPE_NAME { get; set; }
        public string INS_COMPANY_NAME { get; set; }
        [DisplayName("Discount From")]
        public string KICKBACK_FROM { get; set; }
        public Nullable<bool> CLAIM_STATUS { get; set; }
        public Nullable<bool> ACT { get; set; }
    }

    public class InsuranceUpdateVM
    {
        public string INSURANCE_MST_ID { get; set; }
        [DisplayName("Company Name")]
        public string COMPANY_ID { get; set; }
        [DisplayName("Types of Policy")]

        public string BENIFICIARY_DETAILS { get; set; }
        public string INSURED_ITEM { get; set; }
        public string OCCUPATION { get; set; }
        [DisplayName("Insurance Type")]
        public string INSURANCE_TYPE_ID { get; set; }
        [DisplayName("Insurance Company")]
        public string INSURANCE_COMPANY_ID { get; set; }
        [DisplayName("Policy Number")]
        public string INSURANCE_NUMBER { get; set; }

        [DisplayName("Issue Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? INSURANE_DATE { get; set; }

        [DisplayName("Expire Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? INSURANCE_EXPAIR_DATE { get; set; }

        //[DisplayName("Registration Date")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        //public DateTime INSURANE_DATE { get; set; }

        //[DisplayName("Expire Date")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        //public DateTime INSURANCE_EXPAIR_DATE { get; set; }

        [DisplayName("Insurred Amount")]
        public long INSURANCE_AMOUNT { get; set; }
        [DisplayName("Net Premium")]
        public int NET_PREMIUM { get; set; }
        [DisplayName("VAT")]
        public int VAT { get; set; }
        [DisplayName("Stamp")]
        public int STAMP { get; set; }

        [DisplayName("Description")]
        public string DESCRIPTION { get; set; }

        [DisplayName("Request By")]
        public string REQUEST_BY { get; set; }

        public HttpPostedFileBase FileX { get; set; }

        public string COMPANY_NAME { get; set; }
        public string INSURANCE_TYPE_NAME { get; set; }
        public string INS_COMPANY_NAME { get; set; }

    }

    public class InsuranceReportVM
    {
        [DisplayName("ID")]
        public string INSURANCE_MST_ID { get; set; }
        [DisplayName("Company Name")]
        public string COMPANY_ID { get; set; }
        [DisplayName("Insurance Type")]
        public string INSURANCE_TYPE_ID { get; set; }
        [DisplayName("Insurance Company")]
        public string INSURANCE_COMPANY_ID { get; set; }
        [DisplayName("Policy Number")]
        public string INSURANCE_NUMBER { get; set; }
        [DisplayName("Registration Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime INSURANE_DATE { get; set; }
        [DisplayName("Expire Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime INSURANCE_EXPAIR_DATE { get; set; }
        [DisplayName("Insurred Amount")]
        public long INSURANCE_AMOUNT { get; set; }
        [DisplayName("Net Premium")]
        public int NET_PREMIUM { get; set; }
        [DisplayName("VAT")]
        public int VAT { get; set; }
        [DisplayName("Stamp")]
        public int STAMP { get; set; }
        [DisplayName("Description")]
        public string DESCRIPTION { get; set; }
        [DisplayName("Discount Amount")]
        public Nullable<decimal> KICK_BACK_AMOUNT { get; set; }
        [DisplayName("Discount (%)")]
        public Nullable<decimal> KICK_BACK_PERCENTAGE { get; set; }
        public string BENIFICIARY_DETAILS { get; set; }
        public string INSURED_ITEM { get; set; }
        public string OCCUPATION { get; set; }
        public string KICK_BACK_STATUS { get; set; }
        public Nullable<bool> CLAIM_STATUS { get; set; }
        public string REQUEST_BY { get; set; }

        public decimal TOTAL_PREMIUM { get; set; }
        public Nullable<decimal> ACTUAL_PAID_AMOUNT { get; set; }
        


        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? FROM_DATE { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? TO_DATE { get; set; }

    }

    public class InsuranceDetailVM
    {
        public string INSURANCE_DTLS_ID { get; set; }
        public string ITEM_ID { get; set; }
        public string INSURANCE_MST_ID { get; set; }
        public int INSURANCE_PREMIUM { get; set; }
        public int VAT { get; set; }
        public int STAMP { get; set; }
        public int TOTAL_PREMIUM { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PREMIUM_DATE { get; set; }
        public string REMARK { get; set; }
    }
    public class KickBackVM
    {
        public string INSURANCE_MST_ID { get; set; }
        public string INSURANCE_NUMBER { get; set; }
        public Nullable<decimal> KICK_BACK_PERCENTAGE { get; set; }
        public string KICK_BACK_STATUS { get; set; }
        public string KICKBACK_FROM { get; set; }
        public int VAR { get; set; }
        public int ACT { get; set; }
    }
    public class DeleteVM
    {
        public string INSURANCE_MST_ID { get; set; }
        public string INSURANCE_NUMBER { get; set; }
        public int ACT { get; set; }
    }
    public class ClaimVM
    {
        public string CLAIM_ID { get; set; }
        public string INSURANCE_MST_ID { get; set; }
        [DisplayName("Policy Number")]
        public string INSURANCE_NUMBER { get; set; }
        [DisplayName("Claim Types")]
        public string CLAIM_TYPE_ID { get; set; }
        [DisplayName("Claim Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CLAIM_DATE { get; set; }
        [DisplayName("Location")]
        public string CLAIM_LOCATION { get; set; }
        [DisplayName("Loss Items")]
        public string LOSS_ITEM { get; set; }
        [DisplayName("Claim Amount")]
        public int CLAIM_AMOUNT { get; set; }
        [DisplayName("Status")]
        public string CLAIM_STATUS_ID { get; set; }
        [DisplayName("Remerks")]
        public string REMARK { get; set; }
        [DisplayName("Insured Amount")]
        public long INSURANCE_AMOUNT { get; set; }
        [DisplayName("Ins Amt for Item")]
        public long INS_AMT_LOSS_ITEM { get; set; }

        public string CLAIM_TYPE_LIST { get; set; }
        public string CLAIM_STATUS_LIST { get; set; }
        [DisplayName("Claim Name")]
        public string CLAIM_NAME { get; set; }
        [DisplayName("Company Name")]
        public string COMPANY_NAME { get; set; }
        [DisplayName("Claim Status")]
        public string CLAIM_STATUS_NAME { get; set; }
        [DisplayName("Insurance Type")]
        public string INSURANCE_TYPE_NAME { get; set; }
        [DisplayName("Insurance Company")]
        public string INS_COMPANY_NAME { get; set; }

        public string CLAIM_STATUS { get; set; }
        [DisplayName("Receive Amount")]
        public int CLAIM_RECEIVE_AMOUNT { get; set; }
    }
    public class ClaimReceive
    {
        public string CLAIM_RECEIVE_ID { get; set; }
        public string INSURANCE_MST_ID { get; set; }
        public string CLAIM_ID { get; set; }
        public int RECEIVE_AMOUNT { get; set; }
        public int RECEIVE_PERCENTAGE { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime RECEIVE_DATE { get; set; }
        public string CLAIM_STATUS_ID { get; set; }

        public string RECEIVEREMARK { get; set; }
        public int CLAIM_AMOUNT { get; set; }
        public string CLAIM_STATUS_LIST { get; set; }
        //public int CLAIM_RECEIVE_AMOUNT { get; set; }
    }

}