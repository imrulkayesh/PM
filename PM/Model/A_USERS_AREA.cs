using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Model
{
    public class A_USERS_AREA
    {
        [Key]
        [Column(Order = 0)]
        public string UA_AREA { get; set; }
        [Key]
        [Column(Order = 1)]
        public string UA_USER { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }


        [ForeignKey("UA_AREA")]
        public virtual A_AREA A_AREA { get; set; }
        [ForeignKey("UA_USER")]
        public virtual A_USERS A_USERS { get; set; }
    }
}