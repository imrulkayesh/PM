using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class A_BRAND
    {
        public A_BRAND()
        {
            this.A_ASET_DATA = new HashSet<A_ASET_DATA>();
        }

        [Key]
        public string BRAND_ID { get; set; }
        public string BRAND_NAME { get; set; }
        public string BRAND_ORIGIN { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }


        public virtual ICollection<A_ASET_DATA> A_ASET_DATA { get; set; }
    }
}