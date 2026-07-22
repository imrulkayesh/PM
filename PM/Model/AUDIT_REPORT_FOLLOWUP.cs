using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PM.Model
{
    public class AUDIT_REPORT_FOLLOWUP
    {
        [Key]
        public string AUDIT_RPT_ID { get; set; }
        public string AUDIT_RPT_FOLLOWUP_ID { get; set; }
        public int FOLLOWUP_NUMBER { get; set; }
        public DateTime? FOLLOWUP_DATE { get; set; }
        public int TOTAL_FINDINGS { get; set; }
        public int ACTION_FINDINGS { get; set; }
        public int NO_ACTION_FINDINGS { get; set; }  
        public decimal ACTION_PERCENTAGE { get; set; }
        public string RISK_TYPE_ID { get; set; }
        public string RISK_MITIGATION_ID { get; set; }
        public string RISK_MITIGATION_MARKS { get; set; }
        public string REPLY_PERSON { get; set; }
        public string RESPONSIBLE_DEPT_HEAD { get; set; }
        public string REMARKS { get; set; }
        public string SIGNIFICANT_GAP { get; set; }
        public string FOLLOWUP_REQUIREMENT_ID { get; set; }

        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
    }
}