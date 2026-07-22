using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PM.Model
{
    public class AUDIT_RPT_FINAL_STATUS
    {
        [Key]
        public string FINAL_RPT_STATUS_ID { get; set; }
        public string FINAL_RPT_STATUS_NAME { get; set; }
        public bool ACT { get; set; }

    }
}