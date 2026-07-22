using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Model
{
    public class A_INSPH
    {
        public A_INSPH()
        {
            this.A_INSPD = new HashSet<A_INSPD>();
            this.A_JOBH = new HashSet<A_JOBH>();
        }


        [Key]
        public string INSPH_ID { get; set; }
        public string INSPH_ASET { get; set; }
        public string INSPH_OWNR { get; set; }
        public string INSPH_DEPARTMENT { get; set; }
        public string INSPH_AREA { get; set; }
        public string INSPH_IUSER { get; set; }
        public Nullable<System.DateTime> INSPH_DATE { get; set; }
        public string MILEAGE { get; set; }
        public string INSPH_TYPE { get; set; }
        public string INSPH_NOTE { get; set; }
        public string INSPH_JOB { get; set; }
        public string INSPH_APP { get; set; }
        public Nullable<System.DateTime> INSPH_APDATE { get; set; }
        public string INSPH_APBY { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }

        [ForeignKey("INSPH_ASET")]
        public virtual A_ASET_DATA A_ASET_DATA { get; set; }
        [ForeignKey("INSPH_TYPE")]
        public virtual A_INSP_TYPE A_INSP_TYPE { get; set; }
        //[ForeignKey("INSPH_DEPARTMENT")]
        //public virtual A_DEPARTMENT A_DEPARTMENT { get; set; }
        public virtual ICollection<A_INSPD> A_INSPD { get; set; }
        public virtual ICollection<A_JOBH> A_JOBH { get; set; }
    }
}