using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace PM.Model
{
    public class AUDIT_AUDITOR
    {
        [Key]
        public string AUDITOR_ID { get; set; }
        public string AUDITOR_NAME { get; set; }
        public string AUDITOR_STAFF_ID { get; set; }
        public string AUDITOR_MOBILE { get; set; }
        public string AUDITOR_EMAIL { get; set; }
        public bool ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }

    }
}