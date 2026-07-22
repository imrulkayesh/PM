using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class A_GROUP
    {
        public A_GROUP()
        {
            this.A_ACCE_ITEMS = new HashSet<A_ACCE_ITEMS>();
            this.A_ASET_DATA = new HashSet<A_ASET_DATA>();
            this.A_EMAIL = new HashSet<A_EMAIL>();
            this.A_ITEM_LIST = new HashSet<A_ITEM_LIST>();
            this.A_USERS_GROUP = new HashSet<A_USERS_GROUP>();
        }

        [Key]
        public string GROUP_ID { get; set; }
        public string GROUP_NAME { get; set; }
        public string GROUP_DESC { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }


        public virtual ICollection<A_ACCE_ITEMS> A_ACCE_ITEMS { get; set; }
        public virtual ICollection<A_ASET_DATA> A_ASET_DATA { get; set; }
        public virtual ICollection<A_EMAIL> A_EMAIL { get; set; }
        public virtual ICollection<A_ITEM_LIST> A_ITEM_LIST { get; set; }
        public virtual ICollection<A_USERS_GROUP> A_USERS_GROUP { get; set; }
    }
}