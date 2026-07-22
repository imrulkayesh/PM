using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Model
{
    public class A_PLEA
    {
        [Key]
        public string PLEA_ID { get; set; }
        public string PLEA_DFND { get; set; }
        public string PLEA_PAID_BY { get; set; }
        public Nullable<System.DateTime> PLEA_DATE { get; set; }
        public string PLEA_REF { get; set; }
        public string PLEA_REASON_ID { get; set; }
        public string PLEA_DESCRIPTION { get; set; }
        public Nullable<int> PLEA_AMNT { get; set; }
        public string PLEA_LOCATION { get; set; }
        public string PLEA_OWNR { get; set; }
        public string PLEA_MILEAGE { get; set; }
        public string PLEA_ASET { get; set; }
        public string PLEA_DEPT { get; set; }
        public string PLEA_AREA { get; set; }
        public string PLEA_STATUS { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }
        public string PLEA_APP { get; set; }
        public Nullable<System.DateTime> PLEA_APP_DATE { get; set; }


        [ForeignKey("PLEA_AREA")]
        public virtual A_AREA A_AREA { get; set; }
        [ForeignKey("PLEA_ASET")]
        public virtual A_ASET_DATA A_ASET_DATA { get; set; }
        [ForeignKey("PLEA_DEPT")]
        public virtual A_DEPARTMENT A_DEPARTMENT { get; set; }
        [ForeignKey("PLEA_OWNR")]
        public virtual A_OWNR A_OWNR { get; set; }
        [ForeignKey("PLEA_REASON_ID")]
        public virtual A_PLEA_REASON A_PLEA_REASON { get; set; }

    }
}