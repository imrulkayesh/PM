using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PM.ModelsV
{
    public partial class OutletVM
    {
        public string OWNR_ID { get; set; }

        [Required(ErrorMessage = "Department is reaquired")]
        public string OWNR_DEPT { get; set; }

        [Required(ErrorMessage = "Area is reaquired")]
        public string OWNR_AREA { get; set; }

        [DisplayName("Outlet Id")]
        //[Required(ErrorMessage = "Outlet Id is reaquired")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Minimum 3 Maximum 50")]
        public string OWNR_TEXT { get; set; }

        [DisplayName("Outlet Name")]
        [Required(ErrorMessage = "Outlet Name is reaquired")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Minimum 2 Maximum 50")]
        public string OWNR_USER { get; set; }

        [DisplayName("Proprietor")]
        [Required(ErrorMessage = "Proprietor Name is reaquired")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Minimum 2 Maximum 30")]
        public string OWNR_NAME { get; set; }

        [DisplayName("Proprietor NID")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Minimum 6 Maximum 30")]
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
        public Nullable<System.DateTime> OWNR_NDATE { get; set; }

        [DisplayName("Status")]
        public Nullable<bool> OWNR_POST { get; set; }
        public string A_AREA { get; set; }
        public string A_DEPARTMENT { get; set; }
    }
    public partial class AssetOwnerVM2
    {
        public string OWNR_PIC { get; set; }
        public string OWNR_RESIGN { get; set; }
        public Nullable<System.DateTime> OWNR_RDATE { get; set; }
        public Nullable<bool> ACT { get; set; }
    }

    public class OutletExcel
    {
        [Required(ErrorMessage = "Outlet Code is reaquired")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Minimum 2 Maximum 50")]
        public string OWNR_TEXT { get; set; }

        [Required(ErrorMessage = "Outlet Name is reaquired")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Minimum 2 Maximum 50")]
        public string OWNR_USER { get; set; }

        [Required(ErrorMessage = "Proprietor Name is reaquired")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Minimum 2 Maximum 30")]
        public string OWNR_NAME { get; set; }


        [Required(ErrorMessage = "Area is reaquired")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 20")]
        public string OWNR_CONT { get; set; }


        [Required(ErrorMessage = "Address is reaquired")]
        [StringLength(120, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 120")]
        public string OWNR_ADDR { get; set; }

        [Required(ErrorMessage = "Note is reaquired")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 100")]
        public string OWNR_NOTE { get; set; }

        [Required(ErrorMessage = "Department is reaquired")]
        public string OWNR_DEPT { get; set; }

        [Required(ErrorMessage = "Area is reaquired")]
        public string OWNR_AREA { get; set; }
    }
}



