using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PM.Model
{
    public class C_DEPARTMENT
    {
        [Key]
        public string DEPARTMENT_ID { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string SHORT_NAME { get; set; }
        public bool ACT { get; set; }

    }
}