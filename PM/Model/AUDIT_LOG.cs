using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PM.Model
{
    public class AUDIT_LOG
    {
        [Key]
        public string LOG_ID { get; set; }
        public string USER_ID { get; set; }
        public int CMNU_TEXT { get; set; }
        public  Nullable<System.DateTime> CDT { get; set; }
    }
}