using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PM.Model
{
    public class C_COMPANY_GROUP
    {
        [Key]
        public string COMPANY_GROUP_ID { get; set; }
        public string COMPANY_GROUP_NAME { get; set; }
        public bool ACT { get; set; }
    }
}