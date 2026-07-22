using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Model
{
    public class A_OWNR
    {
        public A_OWNR()
        {
            this.A_ASET_APP = new HashSet<A_ASET_APP>();
            this.A_ASET_USER = new HashSet<A_ASET_USER>();
            this.A_OWNR_BILL = new HashSet<A_OWNR_BILL>();
            this.A_PLEA = new HashSet<A_PLEA>();
        }
        [Key]
        public string OWNR_ID { get; set; }
        public string OWNR_DEPT { get; set; }
        public string OWNR_AREA { get; set; }
        public string OWNR_TEXT { get; set; }
        public string OWNR_USER { get; set; }
        public string OWNR_NAME { get; set; }
        public string OWNR_DESG { get; set; }
        public string OWNR_CONT { get; set; }
        public string OWNR_EMAIL { get; set; }
        public string OWNR_ADDR { get; set; }
        public string OWNR_PIC { get; set; }
        public string OWNR_NOTE { get; set; }
        public string OWNR_TYPE { get; set; }
        public string OWNR_RESIGN { get; set; }
        public Nullable<System.DateTime> OWNR_NDATE { get; set; }
        public Nullable<System.DateTime> OWNR_RDATE { get; set; }
        public Nullable<bool> OWNR_POST { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }
        public Nullable<int> REMIND { get; set; }
        public string OWNR_PASSPORT_NUMBER { get; set; }
        public Nullable<System.DateTime> OWNR_PASSPORT_EDATE { get; set; }
        public string OWNR_HEALTH_CARD { get; set; }
        public Nullable<System.DateTime> OWNR_HEALTH_CARD_EDATE { get; set; }
        public string OWNR_STYPE { get; set; }

        [ForeignKey("OWNR_AREA")]
        public virtual A_AREA A_AREA { get; set; }

        public virtual ICollection<A_ASET_APP> A_ASET_APP { get; set; }

        public virtual ICollection<A_ASET_USER> A_ASET_USER { get; set; }
        [ForeignKey("OWNR_DEPT")]
        public virtual A_DEPARTMENT A_DEPARTMENT { get; set; }

        public virtual ICollection<A_OWNR_BILL> A_OWNR_BILL { get; set; }

        public virtual ICollection<A_PLEA> A_PLEA { get; set; }
        //[ForeignKey("OWNR_STYPE")]
        //public virtual A_STAFF_TYPE A_STAFF_TYPE { get; set; }
    }
}