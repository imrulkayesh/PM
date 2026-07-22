using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Model
{
    public class A_USERS_MENUC
    {
        [Key]
        [Column(Order = 0)]
        public int CMNU_TEXT { get; set; }
        [Key]
        [Column(Order = 1)]
        public string USER_TEXT { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }

        [ForeignKey("CMNU_TEXT")]
        public virtual A_MENUC A_MENUC { get; set; }
        [ForeignKey("USER_TEXT")]
        public virtual A_USERS A_USERS { get; set; }
    }
}