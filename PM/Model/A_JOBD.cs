using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Model
{
    public class A_JOBD
    {

        [Key]
        [Column(Order = 0)]
        public string JOBH_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        public byte JOBD_LINE { get; set; }
        public string JOBD_ITEM { get; set; }
        public Nullable<byte> JOBD_IQTY { get; set; }
        public Nullable<byte> JOBD_JQTY { get; set; }
        public Nullable<decimal> JOBD_IRATE { get; set; }
        public Nullable<decimal> JOBD_JRATE { get; set; }
        public string JOBD_NOTE { get; set; }
        public string JOBD_STATE { get; set; }


        [ForeignKey("JOBD_ITEM")]
        public virtual A_ITEM_LIST A_ITEM_LIST { get; set; }
        [ForeignKey("JOBH_ID")]
        public virtual A_JOBH A_JOBH { get; set; }
    }
}