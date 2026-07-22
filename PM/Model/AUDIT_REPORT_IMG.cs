using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class AUDIT_REPORT_IMG
    {
        [Key]
        public string AUDIT_IMG_ID { get; set; }
        public string AUDIT_REPORT_ID { get; set; }
        public string IMG_NAME { get; set; }
        public string IMG_PATH { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
    }
}