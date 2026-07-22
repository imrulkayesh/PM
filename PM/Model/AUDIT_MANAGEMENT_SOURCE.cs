using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PM.Model
{
    public class AUDIT_MANAGEMENT_SOURCE
    {
        [Key]
        public string MANAGEMENT_SOURCE_ID { get; set; }
        public string MANAGEMENT_SOURCE_NAME { get; set; }
        public bool ACT { get; set; }
    }
}