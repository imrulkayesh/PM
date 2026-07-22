using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class LIC_LICENSE_TYPE
    {
        [Key]
        public string LICENSE_TYPE_ID { get; set; }
        public string LICENSE_TYPE_NAME { get; set; }
        public string REMARKS { get; set; }
        public bool ACT { get; set; }
    }
}