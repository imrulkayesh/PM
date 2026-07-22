using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Model
{
    public class A_INSPD
    {
        [Key]
        [Column(Order = 0)]
        public string INSPH_ID { get; set; }
        [Key]
        [Column(Order = 1)]
        public byte INSPD_LINE { get; set; }
        public string INSPD_ITEM { get; set; }
        public Nullable<byte> INSPD_IQTY { get; set; }
        public Nullable<decimal> INSPD_IRATE { get; set; }
        public string INSPD_NOTE { get; set; }
        public string INSPD_STATE { get; set; }

        [ForeignKey("INSPH_ID")]
        public virtual A_INSPH A_INSPH { get; set; }
        [ForeignKey("INSPD_ITEM")]
        public virtual A_ITEM_LIST A_ITEM_LIST { get; set; }
    }
}