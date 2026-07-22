using System;
using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class INS_CLAIM_RECEIVE
    {
        [Key]
        public string CLAIM_RECEIVE_ID { get; set; }
        public string CLAIM_ID { get; set; }
        public int RECEIVE_AMOUNT { get; set; }
        public Nullable<System.DateTime> RECEIVE_DATE { get; set; }
        public string RECEIVEREMARK { get; set; }

        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }

    }
}