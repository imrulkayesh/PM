using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace PM.Model
{
    public class AUDIT_FOLLOWUP_REQUIREMENT
    {
        [Key]
        public string FOLLOWUP_REQUIREMENT_ID { get; set; }
        public string FOLLOWUP_REQUIREMENT_NAME { get; set; }
        public bool ACT { get; set; }
    }
}