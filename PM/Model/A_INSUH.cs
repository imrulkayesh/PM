using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Model
{
    public class A_INSUH
    {
        public A_INSUH()
        {
            this.A_ASET_APP = new HashSet<A_ASET_APP>();
            this.A_INSUD = new HashSet<A_INSUD>();
        }

        [Key]
        public string INSU_ID { get; set; }
        public string INSU_ASET { get; set; }
        public string INSU_POLICY { get; set; }
        public Nullable<int> INSU_ASET_VALUE { get; set; }
        public Nullable<int> INSU_PREMIUM { get; set; }
        public string INSU_COMP { get; set; }
        public string INSU_COMP_ADDRESS { get; set; }
        public string INSU_NOTE { get; set; }
        public Nullable<System.DateTime> INSU_REG_START { get; set; }
        public Nullable<System.DateTime> INSU_REG_END { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }
        public Nullable<bool> REMIND { get; set; }



        public virtual ICollection<A_ASET_APP> A_ASET_APP { get; set; }
        [ForeignKey("INSU_ASET")]
        public virtual A_ASET_DATA A_ASET_DATA { get; set; }

        public virtual ICollection<A_INSUD> A_INSUD { get; set; }
    }
}