using System;
using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class A_USERS_COUNTRY
    {
        [Key]
        public string UC_ID { get; set; }
        public string COUNTRY_ID { get; set; }
        public string USER_TEXT { get; set; }
        public Nullable<bool> ACT { get; set; }
    }
}