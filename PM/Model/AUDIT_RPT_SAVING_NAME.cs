using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PM.Model
{
    public class AUDIT_RPT_SAVING_NAME
    {
        [Key]
        public string AUDIT_SAVING_NAME_ID { get; set; }
        public string AUDIT_SAVING_TYPE_ID { get; set; }
        public string AUDIT_SAVING_NAME { get; set; }
        public bool ACT { get; set; }
    }
}