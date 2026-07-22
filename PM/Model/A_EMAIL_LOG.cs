using System;
using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class A_EMAIL_LOG
    {
        [Key]
        public string LOG_ID { get; set; }
        public string ASET_ID { get; set; }
        public string REMIND_ID { get; set; }
        public Nullable<int> TO_EMAIL { get; set; }
        public Nullable<int> CC_EMAIL { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
    }
}