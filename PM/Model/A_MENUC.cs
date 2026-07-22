using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Model
{
    public class A_MENUC
    {

        public A_MENUC()
        {
            this.A_USERS_MENUC = new HashSet<A_USERS_MENUC>();
        }
        [Key]
        public int CMNU_TEXT { get; set; }
        public string CMNU_NAME { get; set; }
        public string CMNU_ICON { get; set; }
        public string CMNU_CONT { get; set; }
        public string CMNU_ACTN { get; set; }
        public Nullable<int> CMNU_ROLE { get; set; }
        public int CMNU_PMNU { get; set; }
        public string CMNU_NOTE { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }

        public virtual ICollection<A_USERS_MENUC> A_USERS_MENUC { get; set; }
        [ForeignKey("CMNU_PMNU")]
        public virtual A_MENUP A_MENUP { get; set; }
    }
}