using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Model
{
    public class A_COMPANY
    {
        public A_COMPANY()
        {
            this.A_BUSINESS = new HashSet<A_BUSINESS>();
        }

        [Key]
        public string COMPANY_ID { get; set; }
        public string COMPANY_COUNTRY { get; set; }
        public string COMPANY_NAME { get; set; }
        public string COMPANY_DESC { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }


        public virtual ICollection<A_BUSINESS> A_BUSINESS { get; set; }
        [ForeignKey("COMPANY_COUNTRY")]
        public virtual A_COUNTRY A_COUNTRY { get; set; }
    }
}