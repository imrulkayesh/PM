using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PM.Model
{
    public class AUDIT_REPORT
    {
        [Key]
        public string AUDIT_RPT_ID { get; set; }
        public string TASK_ID { get; set; }
        public string AUDIT_RPT_NAME { get; set; }
        public DateTime? RPT_SENT_DATE { get; set; }
        //public string TASK_CATEGORY_TYPE_ID { get; set; }
        //public string TASK_CATEGORY_ID { get; set; }
        //public Nullable<decimal> SAVING_AMOUNT { get; set; }
        public string CHECK_STATUS { get; set; }
        public string CHECK_BY { get; set; }
        public DateTime? CHECK_DATE { get; set; }
        public string APPROVE_STATUS { get; set; }
        public string APPROVE_BY { get; set; }
        public DateTime? APPROVE_DATE { get; set; }
        public Nullable<decimal> WORK_SCOPE { get; set; }
        public string WORK_OPPORTUNITY { get; set; }
        public string ASSIST_BY { get; set; }
        public string APPROVE_SENT_STATUS { get; set; }

        public string REPORT_STATUS_ID { get; set; }

        public string DIRECT_SAVING_TYPE_ID { get; set; }
        public Nullable<decimal> DIRECT_SAVING_AMOUNT { get; set; }
        public string INDIRECT_SAVING_TYPE_ID { get; set; }
        public Nullable<decimal> INDIRECT_SAVING_AMOUNT { get; set; }
        public string RISK_TYPE_ID { get; set; }
        public string FOLLOWUP_STATUS { get; set; }
        public DateTime? FIRST_FOLLOWUP_DATE { get; set; }
        public DateTime? SECOND_FOLLOWUP_DATE { get; set; }
        public DateTime? THIRD_FOLLOWUP_DATE { get; set; }

        public string FIRST_FOLLOWUP_STATUS { get; set; }
        public string SECOND_FOLLOWUP_STATUS { get; set; }
        public string THIRD_FOLLOWUP_STATUS { get; set; }
        public string MANAGEMENT_ACTION_ID { get; set; }

        public string TASK_CATEGORY_TYPE_ID { get; set; }
        public string TASK_CATEGORY_ID { get; set; }
        public string REMARK { get; set; }

        public string DEMOTION { get; set; }
        public string DEVELOPMENT { get; set; }
        public string JOB_SACK_OUT { get; set; }
        public string TRANSFER { get; set; }
        public string WARNING_LETTER { get; set; }
        public string PENALTY { get; set; }
        public string HOLD_PAYMENT { get; set; }
        public string SUPPLIER_BLACKLIST { get; set; }
        public string CORRUPTION_IDENTIFIED { get; set; }
        public string INEFFICIENCY_IDENTIFIED { get; set; }


        public virtual ICollection<AUDIT_REPORT_DETAIL> AUDIT_REPORT_DETAIL { get; set; }

        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
    }
}