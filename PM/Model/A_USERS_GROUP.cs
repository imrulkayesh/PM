using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Model
{
    public class A_USERS_GROUP
    {
        [Key]
        [Column(Order = 0)]
        public string UG_GROUP { get; set; }
        [Key]
        [Column(Order = 1)]
        public string UG_USER { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }

        [ForeignKey("UG_GROUP")]
        public virtual A_GROUP A_GROUP { get; set; }
        [ForeignKey("UG_USER")]
        public virtual A_USERS A_USERS { get; set; }
    }
}