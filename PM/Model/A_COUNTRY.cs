using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class A_COUNTRY
    {
        public A_COUNTRY()
        {
            this.A_COMPANY = new HashSet<A_COMPANY>();
            this.A_DISTRICT = new HashSet<A_DISTRICT>();
        }

        [Key]
        public string COUNTRY_ID { get; set; }
        public string COUNTRY_NAME { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }



        public virtual ICollection<A_COMPANY> A_COMPANY { get; set; }
        public virtual ICollection<A_DISTRICT> A_DISTRICT { get; set; }
    }
}