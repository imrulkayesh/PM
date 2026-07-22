using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Model
{
    public class A_INSUD
    {
        [Key]
        [Column(Order = 0)]
        public string INSU_ID { get; set; }
        [Key]
        [Column(Order = 1)]
        public int INSU_LINE { get; set; }
        public Nullable<int> INSU_ASET_VALUE { get; set; }
        public Nullable<int> INSU_PREMIUM { get; set; }
        public Nullable<int> INSU_AMOUNT { get; set; }
        public Nullable<int> INSU_VAT_AMOUNT { get; set; }
        public Nullable<System.DateTime> INSU_DATE { get; set; }
        public string INSU_PAID { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }


        [ForeignKey("INSU_ID")]
        public virtual A_INSUH A_INSUH { get; set; }
    }
}