using System;
using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class LIC_LICENSE_DOCUMENT
    {
        [Key]
        public string LICENSE_DOCUMENT_ID { get; set; }
        public string LICENSE_NAME_ID { get; set; }
        public string DOCUMENT_NAME { get; set; }
        public int DAYS { get; set; }
        public string REMARKS { get; set; }
        public bool ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
    }
}