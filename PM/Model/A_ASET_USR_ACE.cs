using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Model
{
    public class A_ASET_USR_ACE
    {
        [Key]
        [Column(Order = 0)]
        public string AUA_AU { get; set; }
        [Key]
        [Column(Order = 1)]
        public byte AUA_LINE { get; set; }
        public string AUA_ACCE_ID { get; set; }
        public string AUA_DESC { get; set; }
        public Nullable<byte> AUA_QTY { get; set; }
        public Nullable<int> AUA_RATE { get; set; }
        public string AUA_TYPE { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }

        [ForeignKey("AUA_ACCE_ID")]
        public virtual A_ACCE_ITEMS A_ACCE_ITEMS { get; set; }

        [ForeignKey("AUA_AU")]
        public virtual A_ASET_USER A_ASET_USER { get; set; }

    }
}