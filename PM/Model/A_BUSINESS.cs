using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Model
{
    public class A_BUSINESS
    {
        public A_BUSINESS()
        {
            this.A_AREA = new HashSet<A_AREA>();
            this.A_EMAIL = new HashSet<A_EMAIL>();
            this.A_DEPARTMENT = new HashSet<A_DEPARTMENT>();
            this.A_THANA = new HashSet<A_THANA>();
            this.A_USERS_BUS = new HashSet<A_USERS_BUS>();
        }

        [Key]
        public string BUSINESS_ID { get; set; }
        public string BUSINESS_COMPANY { get; set; }
        public string BUSINESS_NAME { get; set; }
        public string BUSINESS_DESC { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }


        public virtual ICollection<A_AREA> A_AREA { get; set; }

        public virtual ICollection<A_EMAIL> A_EMAIL { get; set; }
        [ForeignKey("BUSINESS_COMPANY")]
        public virtual A_COMPANY A_COMPANY { get; set; }

        public virtual ICollection<A_DEPARTMENT> A_DEPARTMENT { get; set; }

        public virtual ICollection<A_THANA> A_THANA { get; set; }
        public virtual ICollection<A_USERS_BUS> A_USERS_BUS { get; set; }
    }
}