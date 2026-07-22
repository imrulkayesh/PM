using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class A_USERS
    {
        public A_USERS()
        {
            this.A_USERS_MENUC = new HashSet<A_USERS_MENUC>();
            this.A_USERS_AREA = new HashSet<A_USERS_AREA>();
            this.A_USERS_BUS = new HashSet<A_USERS_BUS>();
            this.A_USERS_GROUP = new HashSet<A_USERS_GROUP>();
            this.A_USERS_ROLES = new HashSet<A_USERS_ROLES>();
        }
        [Key]
        public string USER_TEXT { get; set; }
        public string USER_NAME { get; set; }
        public string USER_PASS { get; set; }
        public string USER_MAIL { get; set; }
        public string USER_MOBL { get; set; }
        public string USER_IMAG { get; set; }
        public int USER_ROLE { get; set; }
        public string USER_LOCATION { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }


        public virtual ICollection<A_USERS_MENUC> A_USERS_MENUC { get; set; }
        public virtual ICollection<A_USERS_AREA> A_USERS_AREA { get; set; }
        public virtual ICollection<A_USERS_BUS> A_USERS_BUS { get; set; }
        public virtual ICollection<A_USERS_GROUP> A_USERS_GROUP { get; set; }
        public virtual ICollection<A_USERS_ROLES> A_USERS_ROLES { get; set; }
    }
}