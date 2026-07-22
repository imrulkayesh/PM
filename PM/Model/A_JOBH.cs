using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Model
{
    public class A_JOBH
    {

        public A_JOBH()
        {
            this.A_BILL = new HashSet<A_BILL>();
            this.A_JOBD = new HashSet<A_JOBD>();
        }
        [Key]
        public string JOBH_ID { get; set; }
        public string INSPH_ID { get; set; }
        public string JOBH_ASET { get; set; }
        public string JOBH_OWNR { get; set; }
        public string JOBH_DEPARTMENT { get; set; }
        public string JOBH_AREA { get; set; }
        public Nullable<System.DateTime> JOBH_DATE { get; set; }
        public string JOBH_MILEAGE { get; set; }
        public string JOBH_VENDOR { get; set; }
        public string JOBH_NOTE { get; set; }
        public string JOBH_TYPE { get; set; }
        public Nullable<decimal> JOBH_IPAY { get; set; }
        public Nullable<decimal> JOBH_UPAY { get; set; }
        public Nullable<decimal> JOBH_CPAY { get; set; }
        public Nullable<decimal> JOBH_DISC { get; set; }
        public string JOBH_CNAME { get; set; }
        public string JOBH_BILL { get; set; }
        public string JOBH_APP { get; set; }
        public Nullable<System.DateTime> JOBH_APDATE { get; set; }
        public string JOBH_APBY { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }

        public virtual ICollection<A_BILL> A_BILL { get; set; }
        [ForeignKey("JOBH_AREA")]
        public virtual A_INSP_TYPE A_INSP_TYPE { get; set; }
        [ForeignKey("INSPH_ID")]
        public virtual A_INSPH A_INSPH { get; set; }

        public virtual ICollection<A_JOBD> A_JOBD { get; set; }
    }
}