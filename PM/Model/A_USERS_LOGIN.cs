using System;
using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class A_USERS_LOGIN
    {
        [Key]
        public string USER_TEXT { get; set; }
        public string USER_IP { get; set; }
        public string USER_PC { get; set; }
        public string PC_USER { get; set; }
        public string PC_TYPE { get; set; }
        public string BROWSER_NAME { get; set; }
        public string BROWSER_VERSION { get; set; }
        public string SESSION_ID { get; set; }
        public Nullable<bool> SESSION_ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
    }
}