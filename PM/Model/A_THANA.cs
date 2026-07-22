using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Model
{
    public class A_THANA
    {
        public A_THANA()
        {
            this.A_AREA = new HashSet<A_AREA>();
        }

        [Key]
        public string THANA_ID { get; set; }
        public string THANA_DISTRICT { get; set; }
        public string THANA_BUSINESS { get; set; }
        public string THANA_NAME { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }

        public virtual ICollection<A_AREA> A_AREA { get; set; }
        [ForeignKey("THANA_BUSINESS")]
        public virtual A_BUSINESS A_BUSINESS { get; set; }
        [ForeignKey("THANA_DISTRICT")]
        public virtual A_DISTRICT A_DISTRICT { get; set; }
    }
}