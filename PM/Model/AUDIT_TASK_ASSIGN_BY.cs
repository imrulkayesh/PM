using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PM.Model
{
    public class AUDIT_TASK_ASSIGN_BY
    {
        [Key]
        public string TASK_ASSIGN_BY_ID { get; set; }
        public string TASK_ASSIGN_BY_NAME { get; set; }
        public bool ACT { get; set; }
    }
}