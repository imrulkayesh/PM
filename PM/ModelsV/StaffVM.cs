using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PM.ModelsV
{
    public partial class StaffVM
    {
        public string OWNR_ID { get; set; }

        [Required(ErrorMessage = "Department is reaquired")]
        public string OWNR_DEPT { get; set; }

        [Required(ErrorMessage = "Area is reaquired")]
        public string OWNR_AREA { get; set; }

        [DisplayName("Staff Id")]
        [Required(ErrorMessage = "Outlet Id is reaquired")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Minimum 3 Maximum 50")]
        [Remote("CheckOwnrTextExist", "AssetUser", ErrorMessage = "User already exist")]
        public string OWNR_TEXT { get; set; }

        [DisplayName("Staff Name")]
        [Required(ErrorMessage = "Staff Name is reaquired")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Minimum 3 Maximum 50")]
        public string OWNR_USER { get; set; }

        [DisplayName("Staff NID/Iquama")]
        //[Required(ErrorMessage = "Staff NID Name is reaquired")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Minimum 3 Maximum 30")]
        public string OWNR_NAME { get; set; }

        [DisplayName("Designation")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 100")]
        public string OWNR_DESG { get; set; }

        [DisplayName("Mobile")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 20")]
        public string OWNR_CONT { get; set; }

        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Minimum 6 Maximum 30")]
        public string OWNR_EMAIL { get; set; }

        [DisplayName("Address")]
        [Required(ErrorMessage = "Address is reaquired")]
        [StringLength(120, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 120")]
        public string OWNR_ADDR { get; set; }

        [DisplayName("Note")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 100")]
        public string OWNR_NOTE { get; set; }

        [DisplayName("Type")]
        [StringLength(15, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 15")]
        public string OWNR_TYPE { get; set; }

        [DisplayName("Expire Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> OWNR_NDATE { get; set; }

        [DisplayName("Status")]
        public Nullable<bool> OWNR_POST { get; set; }
        public string A_AREA { get; set; }
        public string A_DEPARTMENT { get; set; }

        [DisplayName("Department")]
        public string DEPARTMENT_V { get; set; }
        [DisplayName("Area")]
        public string AREA_V { get; set; }

        [DisplayName("Sponsor/Business")]
        public string BUSINESS_V { get; set; }

        [DisplayName("Owner/Company")]
        public string COMPANY_V { get; set; }

        [DisplayName("Passport Number")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 20")]
        public string OWNR_PASSPORT_NUMBER { get; set; }

        [DisplayName("Passport Expire Date")]
        [DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]]
        public Nullable<System.DateTime> OWNR_PASSPORT_EDATE { get; set; }

        [DisplayName("Health Card Number")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 50")]
        public string OWNR_HEALTH_CARD { get; set; }

        [DisplayName("Health Card Expire Date")]
        [DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]]
        public Nullable<System.DateTime> OWNR_HEALTH_CARD_EDATE { get; set; }

        [DisplayName("Staff Type")]
        public string OWNR_STYPE { get; set; }

        //public string A_STAFF_TYPE { get; set; }

        //public string OWNR_STYPE_V { get; set; }
    }
    public partial class StaffVM2
    {
        public string OWNR_PIC { get; set; }
        public string OWNR_RESIGN { get; set; }
        public Nullable<System.DateTime> OWNR_RDATE { get; set; }
        public Nullable<bool> ACT { get; set; }
    }


    public class StaffEdit
    {
        [Required(ErrorMessage = "Staff Id is reaquired")]
        public string OWNR_ID { get; set; }

        [DisplayName("Designation")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 30")]
        public string OWNR_DESG { get; set; }

        [DisplayName("Mobile")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 20")]
        public string OWNR_CONT { get; set; }

        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Minimum 6 Maximum 30")]
        public string OWNR_EMAIL { get; set; }

        [DisplayName("Address")]
        [Required(ErrorMessage = "Address is reaquired")]
        [StringLength(120, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 120")]
        public string OWNR_ADDR { get; set; }

        [DisplayName("Note")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 100")]
        public string OWNR_NOTE { get; set; }

        [Required(ErrorMessage = "Department is reaquired")]
        public string OWNR_DEPT { get; set; }

        [Required(ErrorMessage = "Area is reaquired")]
        public string OWNR_AREA { get; set; }

        [DisplayName("Staff Type")]
        public string OWNR_STYPE { get; set; }

    }

    public class StaffIquma
    {
        [Required(ErrorMessage = "Staff Id is reaquired")]
        public string OWNR_ID { get; set; }

        [DisplayName("Staff NID")]
        [Required(ErrorMessage = "NID is reaquired")]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "Minimum 4 Maximum 30")]
        public string OWNR_NAME { get; set; }

        [DisplayName("Expire Date")]
        [Required(ErrorMessage = "Expire date is reaquired")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> OWNR_NDATE { get; set; }
    }

    public class StaffPassport
    {
        [Required(ErrorMessage = "Staff Id is reaquired")]
        public string OWNR_ID { get; set; }

        [DisplayName("Staff Passport")]
        [Required(ErrorMessage = "Passport is reaquired")]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "Minimum 4 Maximum 30")]
        public string OWNR_PASSPORT_NUMBER { get; set; }

        [DisplayName("Passport Expire Date")]
        [Required(ErrorMessage = "Passport Expire date is reaquired")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> OWNR_PASSPORT_EDATE { get; set; }
    }

    public class StaffHealthCard
    {
        [Required(ErrorMessage = "Staff Id is reaquired")]
        public string OWNR_ID { get; set; }

        [DisplayName("Staff Health Card")]
        [Required(ErrorMessage = "Health Card is reaquired")]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "Minimum 4 Maximum 30")]
        public string OWNR_HEALTH_CARD { get; set; }

        [DisplayName("Health Card Expire Date")]
        [Required(ErrorMessage = "Health Card Expire date is reaquired")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> OWNR_HEALTH_CARD_EDATE { get; set; }
    }
}