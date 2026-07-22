using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class A_INSP_TYPE
    {

        public A_INSP_TYPE()
        {
            this.A_INSPH = new HashSet<A_INSPH>();
            this.A_JOBH = new HashSet<A_JOBH>();
        }

        [Key]
        public string ITYPE_ID { get; set; }
        public string ITYPE_NAME { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }


        public virtual ICollection<A_INSPH> A_INSPH { get; set; }
        public virtual ICollection<A_JOBH> A_JOBH { get; set; }
    }
}