using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PM.Model
{
    public class AUDIT_TASK_STATUS
    {
        [Key]
        public string TASK_STATUS_ID { get; set; }
        public string TASK_STATUS_NAME { get; set; }
        public bool ACT { get; set; }
    }
}