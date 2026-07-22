using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Model
{
    public class A_BILL
    {
        [Key]
        public string BILH_ID { get; set; }
        public Nullable<System.DateTime> BILH_DATE { get; set; }
        public string BILH_NOTE { get; set; }
        public string BILH_REF { get; set; }
        public string JOBH_ID { get; set; }
        public string BILH_RECEIVER { get; set; }
        public string BILH_PAID { get; set; }
        public Nullable<decimal> BILH_IPAY { get; set; }
        public Nullable<decimal> BILH_UPAY { get; set; }
        public Nullable<decimal> BILH_CPAY { get; set; }
        public Nullable<decimal> BILH_DISC { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }


        [ForeignKey("JOBH_ID")]
        public virtual A_JOBH A_JOBH { get; set; }
    }
}