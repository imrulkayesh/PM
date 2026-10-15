using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PM.Model
{
    public class AUDIT_REPORT_ASSIST_BY
    {
        [Key, Column(Order = 0)]
        public string AUDIT_RPT_ID { get; set; }

        [Key, Column(Order = 1)]
        public string AUDITOR_STAFF_ID { get; set; }

        public int ACT { get; set; }
        public DateTime? CDT { get; set; }
        public string CDU { get; set; }
        public DateTime? UDT { get; set; }
        public string UDU { get; set; }
    }
}