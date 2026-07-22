using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Model
{
    public class A_USERS_BUS
    {
        [Key]
        [Column(Order = 0)]
        public string UB_BUSINESS { get; set; }
        [Key]
        [Column(Order = 1)]
        public string UB_USER { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }

        [ForeignKey("UB_BUSINESS")]
        public virtual A_BUSINESS A_BUSINESS { get; set; }
        [ForeignKey("UB_USER")]
        public virtual A_USERS A_USERS { get; set; }
    }
}