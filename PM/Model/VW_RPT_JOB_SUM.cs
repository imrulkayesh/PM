using System;
using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class VW_RPT_JOB_SUM
    {
        [Key]
        public string JOBH_ID { get; set; }
        public string INSPH_ID { get; set; }
        public string JOBH_ASET { get; set; }
        public Nullable<System.DateTime> JOBH_DATE { get; set; }
        public Nullable<decimal> JOBH_AMT { get; set; }
    }
}