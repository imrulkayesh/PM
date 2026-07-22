using System;
using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class A_INSP_JOB_IMG
    {
        [Key]
        public string IMG_ID { get; set; }
        public string IMG_NAME { get; set; }
        public string IMG_PATH { get; set; }
        public string IMG_REF { get; set; }
        public Nullable<System.DateTime> IMG_CDT { get; set; }
    }
}