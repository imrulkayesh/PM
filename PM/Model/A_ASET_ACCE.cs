using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Model
{
    public class A_ASET_ACCE
    {
        [Key]
        [Column(Order = 0)]
        public string ASET_ID { get; set; }
        [Key]
        [Column(Order = 1)]
        public string ACCE_ID { get; set; }
        public string AAI_DESC { get; set; }
        public Nullable<byte> AAI_QUANTITY { get; set; }
        public Nullable<int> AAI_RATE { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }

        [ForeignKey("ACCE_ID")]
        public virtual A_ACCE_ITEMS A_ACCE_ITEMS { get; set; }
        [ForeignKey("ASET_ID")]
        public virtual A_ASET_DATA A_ASET_DATA { get; set; }
    }
}