using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;


namespace PM.ModelsV
{
    public class LicenseVM
    {
        [Key]
        [DisplayName("License Name Id")]
        public string LICENSE_NAME_ID { get; set; }
        [DisplayName("License Name")]
        public string LICENSE_NAME { get; set; }
        [DisplayName("License Type")]
        public string LICENSE_TYPE_NAME { get; set; }
        public string LICENSE_TYPE_ID { get; set; }
        [DisplayName("Application Days")]
        public int APPLICATION_DAYS { get; set; }
        [DisplayName("Notify Days")]
        public int NOTIFY_DAYS { get; set; }
        [DisplayName("Remarks")]
        public string REMARKS { get; set; }
        public bool ACT { get; set; }
    }
    public class LicenseDocumnetVM
    {
        public string LICENSE_DOCUMENT_ID { get; set; }
        public string LICENSE_NAME_ID { get; set; }
        public string DOCUMENT_NAME { get; set; }
        public int DAYS { get; set; }
        public string REMARKS { get; set; }
        public bool ACT { get; set; }
        public string LICENSE_NAME { get; set; }

    }
    public class LicenseUserLocationVM
    {
        [DisplayName("Location ID")]
        public string LIC_LOCATION_ID { get; set; }
        [DisplayName("User ID")]
        public string USER_TEXT { get; set; }
        [DisplayName("User Name")]
        public string USER_NAME { get; set; }
        [DisplayName("Location ID")]
        public string USER_LOCATION { get; set; }
        [DisplayName("Location Name")]
        public string LOCATION_NAME { get; set; }
       

    }
    public class IssuingAuthorityVM
    {
        public string ISSUING_AUTHORITY_ID { get; set; }
        public string AUTHORITY_NAME { get; set; }
        public string ADDRESS { get; set; }
        public string MOBILE { get; set; }
        public string EMAIL { get; set; }
        public bool ACT { get; set; }
    }
    public class LicenseCompanyVM
    {
        [DisplayName("Company ID")]
        public string COMPANY_ID { get; set; }
        [DisplayName("Company Name")]
        public string COMPANY_NAME { get; set; }
        [DisplayName("Short Name")]
        public string SHORT_NAME { get; set; }
        public bool ACT { get; set; }
    }
    public class LicenseLocationVM
    {
        [DisplayName("Location ID")]
        public string LIC_LOCATION_ID { get; set; }
        [DisplayName("Location Name")]
        public string LOCATION_NAME { get; set; }
        [DisplayName("Short Name")]
        public string SHORT_NAME { get; set; }
        [DisplayName("Address")]
        public string ADDRESS { get; set; }
        public bool ACT { get; set; }
    }
    public class LicenseStatusVM
    {
        public string LICENSE_STATUS_ID { get; set; }
        public string LICENSE_STATUS_NAME { get; set; }
        public string REMARK { get; set; }
        public bool ACT { get; set; }
    }
    public class LicenseMasterNM
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
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ISSUE_DATE { get; set; }

        [DisplayName("Expire Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
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
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime APPLICATION_DATE { get; set; }
        public Nullable<bool> ACT { get; set; }
        public HttpPostedFileBase FileX { get; set; }
        [DisplayName("Entertainment Fee")]
        public int ENTERTAINMENT_FEE { get; set; }
        [DisplayName("Create By")]
        public string CDU { get; set; }

        public string COMPANY_NAME { get; set; }
        public string LOCATION_NAME { get; set; }
        public string LICENSE_NAME { get; set; }
        public string AUTHORITY_NAME { get; set; }
        public string LICENSE_STATUS_NAME { get; set; }
        [DisplayName("Item Name")]
        public string ITEM_NAME { get; set; }

    }

    public class LicStatusVM
    {
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


        //[DisplayName("Issue Date")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        //public string ISSUE_DATE { get; set; } 

        //[DisplayName("Expire Date")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        //public string EXPIRE_DATE { get; set; } 

        [DisplayName("License Holder Mobile")]
        public string LICENSE_HOLDER_MOBILE { get; set; }
        [DisplayName("License Holder Email")]
        public string LICENSE_HOLDER_EMAIL { get; set; }
        [DisplayName("License Fee")]
        public int LICENSE_FEE { get; set; }
        [DisplayName("Entertainment Fee")]
        public int ENTERTAINMENT_FEE { get; set; }
        
        [DisplayName("Remark")]
        public string REMARK { get; set; }
        [DisplayName("Status")]
        public string LICENSE_STATUS_ID { get; set; }
        [DisplayName("Application Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime APPLICATION_DATE { get; set; }
        public Nullable<bool> ACT { get; set; }
        public HttpPostedFileBase FileX { get; set; }

        public string COMPANY_NAME { get; set; }
        public string LOCATION_NAME { get; set; }
        public string LICENSE_NAME { get; set; }
        public string AUTHORITY_NAME { get; set; }
        public string LICENSE_STATUS_NAME { get; set; }
    }
    public class LicenseRenewVM
    {
        public string LICENSE_ID { get; set; }
        public string COMPANY_ID { get; set; }
        public string LIC_LOCATION_ID { get; set; }
        public string LICENSE_NAME_ID { get; set; }
        public string LICENSE_NUMBER { get; set; }

        // Was DateTime? — now string, since it's never used in controller logic
        // (controller only reads license.ISSUE_DATE from the DB, not model.ISSUE_DATE)
        public string ISSUE_DATE { get; set; }

        public string OLD_EXPIRE_DATE { get; set; }

        // Was DateTime? — this is the one that actually needs parsing below
        public string NEW_EXPIRE_DATE { get; set; }

        public decimal? LICENSE_FEE { get; set; }
        public decimal? ENTERTAINMENT_FEE { get; set; }
        public string LICENSE_STATUS_ID { get; set; }
        public string REMARK { get; set; }
        public HttpPostedFileBase FileX { get; set; }
    }
}