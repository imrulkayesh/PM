using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PM.Model
{
    public class C_LOCATION
    {
        [Key]
        public string LOCATION_ID { get; set; }
        public string LOCATION_NAME { get; set; }
        public string SHORT_NAME { get; set; }
        public string LOCATION_ADDRESS { get; set; }
        public bool ACT { get; set; }
    }
}