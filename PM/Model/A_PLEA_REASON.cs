using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class A_PLEA_REASON
    {
        public A_PLEA_REASON()
        {
            this.A_PLEA = new HashSet<A_PLEA>();
        }

        [Key]
        public string REASON_ID { get; set; }
        public string REASON_NAME { get; set; }
        public string REASON_DESC { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }


        public virtual ICollection<A_PLEA> A_PLEA { get; set; }
    }
}