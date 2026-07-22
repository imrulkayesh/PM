using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PM.Model
{
    public class AUDIT_REPORT_ACTION
    {
        [Key]
        public string REPORT_ACTION_ID { get; set; }
        public string ACTION_NAME { get; set; }
        public Nullable<bool> ACT { get; set; }
    }
}