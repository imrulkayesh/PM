using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Model
{
    public class A_ASET_USER
    {
        public A_ASET_USER()
        {
            this.A_ASET_USR_ACE = new HashSet<A_ASET_USR_ACE>();
            this.A_ASET_USER_IMG = new HashSet<A_ASET_USER_IMG>();
        }

        [Key]
        public string AU_ID { get; set; }
        public string AU_ASET { get; set; }
        public string AU_USER { get; set; }
        public string AU_DEPT { get; set; }
        public string AU_AREA { get; set; }
        public Nullable<System.DateTime> AU_SRDATE { get; set; }
        public string AU_SRUSER { get; set; }
        public Nullable<System.DateTime> AU_SADATE { get; set; }
        public string AU_SAUSER { get; set; }
        public string AU_SNOTE { get; set; }
        public Nullable<System.DateTime> AU_ERDATE { get; set; }
        public string AU_ERUSER { get; set; }
        public Nullable<System.DateTime> AU_EADATE { get; set; }
        public string AU_EAUSER { get; set; }
        public string AU_ENOTE { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }
        public string AU_EMAILAGE { get; set; }
        public string AU_SMAILAGE { get; set; }
        public string AU_TRNSTYPE { get; set; }

        [ForeignKey("AU_AREA")]
        public virtual A_AREA A_AREA { get; set; }
        [ForeignKey("AU_ASET")]
        public virtual A_ASET_DATA A_ASET_DATA { get; set; }
        [ForeignKey("AU_DEPT")]
        public virtual A_DEPARTMENT A_DEPARTMENT { get; set; }
        [ForeignKey("AU_USER")]
        public virtual A_OWNR A_OWNR { get; set; }
        public virtual ICollection<A_ASET_USR_ACE> A_ASET_USR_ACE { get; set; }
        public virtual ICollection<A_ASET_USER_IMG> A_ASET_USER_IMG { get; set; }
    }
}