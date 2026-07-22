using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace PM.ModelsV
{
    public partial class UserLoginVM
    {
        public string StaffId { get; set; }
        public string Passwords { get; set; }
    }

    public partial class ChangePassVM
    {
        [DisplayName("Password")]
        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        [Required(ErrorMessage = "Confirm Password is Required")]
        [Compare("Password", ErrorMessage = "The Password and Confirm Password do not match.")]
        public string Passwords { get; set; }
    }

    public class RecPass
    {
        [DisplayName("User")]
        [Required(ErrorMessage = "User is Required")]
        public string USER_CRD { get; set; }

        [DisplayName("Reset")]
        public bool UPDT_PASS { get; set; }
    }
    public partial class UserMenuVM
    {
        public int MENUP_TEXT { get; set; }
        public string MENUP_NAME { get; set; }
        public string MENUP_ICON { get; set; }
        public int MENUC_TEXT { get; set; }
        public string MENUC_NAME { get; set; }
        public string MENUC_ICON { get; set; }
        public string MENUC_CONTROLLER { get; set; }
        public string MENUC_ACTION { get; set; }
        //  public string MENUU_ID { get; set; }
        public string MENUU_USER { get; set; }
    }

    public class UserProfile
    {
        [DisplayName("User Id")]
        [Required(ErrorMessage = "Id is Required")]
        public string USER_ID { get; set; }

        [DisplayName("User Name")]
        public string USER_NAME { get; set; }

        [DisplayName("Email")]
        [Required(ErrorMessage = "Email is Required")]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, MinimumLength = 7, ErrorMessage = "Minimum 7 Maximum 50")]
        public string USER_EMAIL { get; set; }

        [DisplayName("Mobile")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Minimum 3 Maximum 15")]
        public string USER_MOBILE { get; set; }

        [DisplayName("Picture")]
        public HttpPostedFileBase ProfileImage { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<DateTime> USER_DATE { get; set; }


        [DisplayName("Password")]
        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        public string Passwords { get; set; }
    }
    public partial class ParentMenuVM
    {
        public int MENUP_ID { get; set; }
        public string MENUP_TEXT { get; set; }
        public string MENUP_NAME { get; set; }
        public string MENUP_ICON { get; set; }
        public Nullable<bool> MENUP_ACTV { get; set; }
    }
    public partial class ChildMenuVM
    {
        public int MENUC_ID { get; set; }
        public int MENUC_MENUP_ID { get; set; }
        public string MENUC_TEXT { get; set; }
        public string MENUC_NAME { get; set; }
        public string MENUC_ICON { get; set; }
        public string MENUC_CONTROLLER { get; set; }
        public string MENUC_ACTION { get; set; }
        public Nullable<bool> MENUC_ACTV { get; set; }
    }
    public partial class ViewUserPermissionMenu
    {
        public int PMNU_TEXT { get; set; }
        public string PMNU_NAME { get; set; }
        public int CMNU_TEXT { get; set; }
        public string CMNU_NAME { get; set; }
        public string USER_TEXT { get; set; }
        public string USER_MENUC { get; set; }
        public Nullable<bool> UMP_ACTV { get; set; }
        public string CMNU_NOTE { get; set; }

    }
}