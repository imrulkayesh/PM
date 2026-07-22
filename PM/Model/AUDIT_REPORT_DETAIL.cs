using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PM.Model
{
    public class AUDIT_REPORT_DETAIL
    {
        [Key]
        public string AUDIT_RPT_DETAIL_ID { get; set; }
        public string AUDIT_RPT_ID { get; set; }
        public string RPT_ACTION_ID { get; set; }
        public string VALUE { get; set; }
        public string REMARK { get; set; }
        public string DEMOTION { get; set; }
        public string DEVELOPMENT { get; set; }
        public string JOB_SACK_OUT { get; set; }
        public string TRANSFER { get; set; }
        public string WARNING_LETTER { get; set; }
        public string PENALTY { get; set; }

        public string DIRECT_SAVING_TYPE_ID { get; set; }
        public Nullable<decimal> DIRECT_SAVING_AMOUNT { get; set; }
        public string INDIRECT_SAVING_TYPE_ID { get; set; }
        public Nullable<decimal> INDIRECT_SAVING_AMOUNT { get; set; }

        public string TASK_CATEGORY_TYPE_ID { get; set; }
        public string TASK_CATEGORY_ID { get; set; }
        public string AUDIT_SAVING_TYPE_ID { get; set; }
        public string AUDIT_SAVING_NAME_ID { get; set; }
        public Nullable<decimal> SAVING_AMOUNT { get; set; }


        public int LINE { get; set; }
        public Nullable<bool> ACT { get; set; }

        public Nullable<decimal> FST_FU_SAVING_AMOUNT { get; set; }
        public Nullable<decimal> FST_FU_VALUE { get; set; }
        public DateTime? FST_FU_DATE { get; set; }
        public string FST_FU_STATUS { get; set; }

        public Nullable<decimal> SEC_FU_SAVING_AMOUNT { get; set; }
        public Nullable<decimal> SEC_FU_VALUE { get; set; }
        public DateTime? SEC_FU_DATE { get; set; }
        public string SEC_FU_STATUS { get; set; }

        public Nullable<decimal> FIN_FU_SAVING_AMOUNT { get; set; }
        public Nullable<decimal> FIN_FU_VALUE { get; set; }
        public DateTime? FIN_FU_DATE { get; set; }
        public string FIN_FU_STATUS { get; set; }


    }
}