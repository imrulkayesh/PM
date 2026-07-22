using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Model
{
    public class A_ACCE_ITEMS
    {
        public A_ACCE_ITEMS()
        {
            this.A_ASET_ACCE = new HashSet<A_ASET_ACCE>();
            this.A_ASET_USR_ACE = new HashSet<A_ASET_USR_ACE>();
        }


        [Key]
        public string ACCE_ID { get; set; }
        public string ACCE_GROUP { get; set; }
        public string ACCE_NAME { get; set; }
        public string ACCE_DESC { get; set; }
        public Nullable<int> ACCE_RATE { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }

        [ForeignKey("ACCE_GROUP")]
        public virtual A_GROUP A_GROUP { get; set; }
        public virtual ICollection<A_ASET_ACCE> A_ASET_ACCE { get; set; }
        public virtual ICollection<A_ASET_USR_ACE> A_ASET_USR_ACE { get; set; }

    }
}