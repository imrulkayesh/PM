using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Model
{
    public class A_DISTRICT
    {
        public A_DISTRICT()
        {
            this.A_THANA = new HashSet<A_THANA>();
        }

        [Key]
        public string DISTRICT_ID { get; set; }
        public string DISTRICT_COUNTRY { get; set; }
        public string DISTRICT_NAME { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }


        [ForeignKey("DISTRICT_COUNTRY")]
        public virtual A_COUNTRY A_COUNTRY { get; set; }

        public virtual ICollection<A_THANA> A_THANA { get; set; }
    }
}