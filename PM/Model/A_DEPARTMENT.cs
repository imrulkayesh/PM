using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Model
{
    public class A_DEPARTMENT
    {

        public A_DEPARTMENT()
        {
            this.A_ASET_DATA = new HashSet<A_ASET_DATA>();
            this.A_ASET_USER = new HashSet<A_ASET_USER>();
            this.A_OWNR = new HashSet<A_OWNR>();
            this.A_PLEA = new HashSet<A_PLEA>();
        }

        [Key]
        public string DEPARTMENT_ID { get; set; }
        public string DEPARTMENT_BUSINESS { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string DEPARTMENT_ADDR { get; set; }
        public string DEPARTMENT_DESC { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }




        public virtual ICollection<A_ASET_DATA> A_ASET_DATA { get; set; }

        public virtual ICollection<A_ASET_USER> A_ASET_USER { get; set; }
        [ForeignKey("DEPARTMENT_BUSINESS")]
        public virtual A_BUSINESS A_BUSINESS { get; set; }

        public virtual ICollection<A_OWNR> A_OWNR { get; set; }

        public virtual ICollection<A_PLEA> A_PLEA { get; set; }
    }
}