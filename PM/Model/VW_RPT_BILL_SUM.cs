using System;
using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class VW_RPT_BILL_SUM
    {
        [Key]
        public string BILH_ID { get; set; }
        public Nullable<System.DateTime> BILH_DATE { get; set; }
        public string JOBH_ID { get; set; }
        public Nullable<decimal> BILL_AMT { get; set; }
    }
}