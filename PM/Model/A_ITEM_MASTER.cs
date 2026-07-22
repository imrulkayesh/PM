using System;
using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class A_ITEM_MASTER
    {
        [Key]
        public string ITEM_ID { get; set; }
        public string ITEM_NAME { get; set; }
        public string ITEM_CODE { get; set; }
        public string ITEM_BRAND { get; set; }
        public string ITEM_FLAVOUR { get; set; }
        public string LICENSE_NAME_ID { get; set; }
        public bool ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
    }
}