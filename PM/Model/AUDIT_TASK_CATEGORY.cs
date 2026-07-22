using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PM.Model
{
    public class AUDIT_TASK_CATEGORY
    {
        [Key]
        public string TASK_CATEGORY_ID { get; set; }
        public string TASK_CATEGORY_NAME { get; set; }
        public string TASK_CATEGORY_TYPE_ID { get; set; }
        public bool ACT { get; set; }
    }
}