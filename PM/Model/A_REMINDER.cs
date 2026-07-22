using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Model
{
    public class A_REMINDER
    {
        public A_REMINDER()
        {
            this.A_REMINDER_IMG = new HashSet<A_REMINDER_IMG>();
        }
        [Key]
        public string REMIND_ID { get; set; }
        public string ASET_ID { get; set; }
        public string ITEM_ID { get; set; }
        public string C_STATE { get; set; }
        public string N_STATE { get; set; }
        public Nullable<System.DateTime> C_DATE { get; set; }
        public Nullable<System.DateTime> N_DATE { get; set; }
        public string NOTE { get; set; }
        public string REMIND { get; set; }
        public string COMPLETED { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }

        [ForeignKey("ASET_ID")]
        public virtual A_ASET_DATA A_ASET_DATA { get; set; }
        [ForeignKey("ITEM_ID")]
        public virtual A_ITEM_LIST A_ITEM_LIST { get; set; }
        public virtual ICollection<A_REMINDER_IMG> A_REMINDER_IMG { get; set; }
    }
}