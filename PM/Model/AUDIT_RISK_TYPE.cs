using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PM.Model
{
    public class AUDIT_RISK_TYPE
    {
        [Key]
        public string RISK_TYPE_ID { get; set; }
        public string RISK_TYPE_NAME { get; set; }
        public bool ACT { get; set; }
    }
}