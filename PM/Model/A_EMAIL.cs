using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Model
{
    public class A_EMAIL
    {
        [Key]
        public string EMAIL_ID { get; set; }
        public string EMAIL_DISPLAY { get; set; }
        public string KEEP_IN { get; set; }
        public string MAIL_GROUP { get; set; }
        public string MAIL_BUSINESS { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }
        public string MAIL_TYPE { get; set; }
        public string AREA_ID { get; set; }


        [ForeignKey("MAIL_BUSINESS")]
        public virtual A_BUSINESS A_BUSINESS { get; set; }
        [ForeignKey("MAIL_GROUP")]
        public virtual A_GROUP A_GROUP { get; set; }
    }
}