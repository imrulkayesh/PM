using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PM.ModelsV
{
    public class ApplicationUsers
    {

    }
    public class UserRoleList
    {
        public string USER_NAME { get; set; }
        public int ROLE_ID { get; set; }
        public string ROLE_NAME { get; set; }
        public Nullable<bool> STATUS { get; set; }
    }
    public class UserGroupList
    {
        public string USER_NAME { get; set; }
        public string GROUP_ID { get; set; }
        public string GROUP_NAME { get; set; }
        public Nullable<bool> STATUS { get; set; }
    }
    public class UserBusinessList
    {
        public string USER_NAME { get; set; }
        public string BUSINESS_ID { get; set; }
        public string BUSINESS_NAME { get; set; }
        public Nullable<int> STATUS { get; set; }
    }

    public class UserMenuList
    {
        public string USER_NAME { get; set; }
        public string MENUP_NAME { get; set; }
        public int MENUC_ID { get; set; }
        public string MENUC_NAME { get; set; }
        public string MENUC_ROLE { get; set; }
        public Nullable<bool> STATUS { get; set; }

    }

    public class UserAreaList
    {
        public string USER_NAME { get; set; }
        public string AREA_ID { get; set; }
        public string AREA_NAME { get; set; }
        public Nullable<int> STATUS { get; set; }
    }

    public class SetUserMenu
    {
        public int PMNU_TEXT { get; set; }
        public string PMNU_NAME { get; set; }
        public int CMNU_TEXT { get; set; }
        public string CMNU_NAME { get; set; }
        public string USER_TEXT { get; set; }
        public Nullable<bool> MENU_STATUS { get; set; }

    }

    public class UserEmailList
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EMAIL_ID { get; set; }
        [Required]
        public string EMAIL_DISPLAY { get; set; }
        [Required]
        public string KEEP_IN { get; set; }
        [Required]
        public string MAIL_TYPE { get; set; }
        [Required]
        public string MAIL_GROUP { get; set; }

        public string MAIL_BUSINESS { get; set; }
        public string AREA_ID { get; set; }
        public int ACT { get; set; }

        public bool ACT_B { get; set; }

        public virtual List<UserEmailList> User_Email_List { get; set; }
    }
}