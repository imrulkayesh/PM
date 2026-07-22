using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Model
{
    public class AUDIT_TASK_IMG
    {
        [Key]
        public string TASK_IMG_ID { get; set; }
        public string TASK_ID { get; set; }
        public string IMG_NAME { get; set; }
        public string IMG_PATH { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }

        [ForeignKey("TASK_ID")]
        public virtual AUDIT_TASK AUDIT_TASK { get; set; }
    }
}