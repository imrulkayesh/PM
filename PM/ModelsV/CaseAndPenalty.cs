using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PM.ModelsV
{
    public class CaseAndPenalty
    {
        [DisplayName("Asset Id")]
        [Required(ErrorMessage = "Asset is reaquired")]
        public string PLEA_ASET { get; set; }

        [DisplayName("Plea On")]
        [Required(ErrorMessage = "Case on is reaquired")]
        public string PLEA_DFND { get; set; }

        [DisplayName("Paid By")]
        [Required(ErrorMessage = "Paid By is reaquired")]
        public string PLEA_PAID_BY { get; set; }

        [DisplayName("Occurance Date")]
        [Required(ErrorMessage = "Date is reaquired")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public System.DateTime PLEA_DATE { get; set; }

        [DisplayName("Violence Number")]
        [Required(ErrorMessage = "Violence number is reaquired")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Max 50 char and Min 3 char")]
        [Remote("DuplicatePleaRef", "JsonReaderPM", ErrorMessage = "Violence number already exist")]
        public string PLEA_REF { get; set; }

        [DisplayName("Note")]
        [Required(ErrorMessage = "Description is reaquired")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Max 100 char and Min 5 char")]
        public string PLEA_DESC { get; set; }

        [DisplayName("Amount")]
        [Required(ErrorMessage = "Amount is reaquired")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid Amount")]
        public Nullable<int> PLEA_AMNT { get; set; }
        [DisplayName("Occurance Location")]
        [Required(ErrorMessage = "Location is reaquired")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Max 50 char and Min 3 char")]
        public string PLEA_LOCATION { get; set; }

        [DisplayName("Mileage")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid Number")]
        public string PLEA_MILEAGE { get; set; }

        [DisplayName("Reason Id")]
        [Required(ErrorMessage = "Reason is reaquired")]
        public string PLEA_REASON_ID { get; set; }

    }
    public class JRdata
    {
        public bool success { get; set; }
        public string USER_NAME { get; set; }
        public string AREA_NAME { get; set; }
        public string DEPT_NAME { get; set; }
    }


    public class CasePenlatyView
    {
        [DisplayName("Case Id")]
        public string PLEA_ID { get; set; }
        [DisplayName("Case Base")]
        public string PLEA_DFND { get; set; }
        [DisplayName("Pay")]
        public string PLEA_PAID_BY { get; set; }
        [DisplayName("Occure Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<DateTime> PLEA_DATE { get; set; }
        [DisplayName("Violence Number")]
        public string PLEA_REF { get; set; }
        [DisplayName("Note")]
        public string PLEA_DESC { get; set; }
        [DisplayName("Amount")]
        public Nullable<int> PLEA_AMNT { get; set; }
        [DisplayName("Location")]
        public string PLEA_LOCATION { get; set; }
        [DisplayName("User")]
        public string PLEA_OWNR { get; set; }
        [DisplayName("Mileage")]
        public string PLEA_MILEAGE { get; set; }
        [DisplayName("Asset")]
        public string PLEA_ASET { get; set; }
        [DisplayName("Department")]
        public string PLEA_DEPT { get; set; }
        [DisplayName("Area")]
        public string PLEA_AREA { get; set; }
        [DisplayName("Status")]
        public string PLEA_STATUS { get; set; }
        [DisplayName("Reason")]
        public string PLEA_REASON { get; set; }
        [DisplayName("Approved by")]
        public string PLEA_APP { get; set; }

        [DisplayName("Approve Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<DateTime> PLEA_APP_DATE { get; set; }
    }
    public class CasePleaUpdate
    {
        [DisplayName("Case Id")]
        [Required(ErrorMessage = "Case is reaquired")]
        public string PLEA_ID { get; set; }

        [DisplayName("Note")]
        [Required(ErrorMessage = "Note is reaquired")]
        public string PLEA_DESCRIPTION { get; set; }
    }
    public class CasePleaApproval
    {
        [DisplayName("Case Id")]
        [Required(ErrorMessage = "Case is reaquired")]
        public string PLEA_ID { get; set; }
    }
}
