using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PM.Model
{
    public class AUDIT_RISK_MITIGATION
    {
        [Key]
        public string RISK_MITIGATION_ID { get; set; }
        public string RISK_MITIGATION_NAME { get; set; }
        public bool ACT { get; set; }
    }
}