using System;
using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class LIC_ISSUING_AUTHORITY
    {
        [Key]
        public string ISSUING_AUTHORITY_ID { get; set; }
        public string AUTHORITY_NAME { get; set; }
        public string ADDRESS { get; set; }
        public string MOBILE { get; set; }
        public string EMAIL { get; set; }
        public bool ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
    }
}