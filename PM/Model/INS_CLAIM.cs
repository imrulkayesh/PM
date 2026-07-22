using System;
using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class INS_CLAIM
    {
        [Key]
        public string CLAIM_ID { get; set; }
        public string INSURANCE_MST_ID { get; set; }
        public string CLAIM_TYPE_ID { get; set; }
        public Nullable<System.DateTime> CLAIM_DATE { get; set; }
        public string CLAIM_LOCATION { get; set; }
        public string LOSS_ITEM { get; set; }
        public long INS_AMT_LOSS_ITEM { get; set; }
        public int CLAIM_AMOUNT { get; set; }
        public string CLAIM_STATUS_ID { get; set; }
        public string REMARK { get; set; }
        public int CLAIM_RECEIVE_AMOUNT { get; set; }

        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }

    }
}