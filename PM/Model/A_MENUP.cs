using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class A_MENUP
    {

        public A_MENUP()
        {
            this.A_MENUC = new HashSet<A_MENUC>();
        }

        [Key]
        public int PMNU_TEXT { get; set; }
        public string PMNU_NAME { get; set; }
        public string PMNU_ICON { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }


        public virtual ICollection<A_MENUC> A_MENUC { get; set; }
    }
}