using System;
using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class VW_RPT_INSP_SUM
    {
        [Key]
        public string INSPH_ASET { get; set; }
        public Nullable<System.DateTime> INSPH_DATE { get; set; }
        public string INSPH_ID { get; set; }
        public string INSPH_OWNR { get; set; }
        public string INSPH_AREA { get; set; }
        public string INSPH_DEPARTMENT { get; set; }
        public Nullable<decimal> INSP_AMT { get; set; }
    }
}