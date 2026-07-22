using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class A_ROLES
    {
        public A_ROLES()
        {
            this.A_USERS_ROLES = new HashSet<A_USERS_ROLES>();
        }

        [Key]
        public int ROLE_TEXT { get; set; }
        public string ROLE_NAME { get; set; }
        public string ROLE_DESC { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }

        public virtual ICollection<A_USERS_ROLES> A_USERS_ROLES { get; set; }
    }
}