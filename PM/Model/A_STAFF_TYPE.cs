using System;
using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class A_STAFF_TYPE
    {
        [Key]
        public string STAFF_TYPE_ID { get; set; }
        public string STAFF_TYPE_NAME { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }
    }
}