using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PM.Model
{
    public class AUDIT_FOLLOWUP_TYPE
    {

        [Key]
        public string FOLLOWUP_TYPE_ID { get; set; }
        public string FOLLOWUP_NAME { get; set; }
        public int FOLLOWUP_DAYS { get; set; }
        public bool ACT { get; set; }
    }
}