using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Model
{
    public class LIC_LICENSE_IMG
    {
        [Key]
        public string LICENSE_IMG_ID { get; set; }
        public string LICENSE_ID { get; set; }
        public string IMG_NAME { get; set; }
        public string IMG_PATH { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }

        [ForeignKey("LICENSE_ID")]
        public virtual LIC_LICENSE LIC_LICENSE { get; set; }
    }
}