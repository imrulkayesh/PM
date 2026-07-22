using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Model
{
    public class INS_INSURANCE_IMG
    {
        [Key]
        public string INSURANCE_IMG_ID { get; set; }
        public string IMG_NAME { get; set; }
        public string IMG_PATH { get; set; }
        public string INSURANCE_MST_ID { get; set; }
        public bool ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }

        [ForeignKey("INSURANCE_MST_ID")]
        public virtual INS_INSURANCE_MST INS_INSURANCE_MST { get; set; }
    }
}