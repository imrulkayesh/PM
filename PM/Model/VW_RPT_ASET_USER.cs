using System;
using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class VW_RPT_ASET_USER
    {
        [Key]
        public string TAG { get; set; }
        public string CODE_NAME { get; set; }
        public string ASET_DEPT { get; set; }
        public string ASET_AREA { get; set; }
        public string USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public string WORKING_DEPT { get; set; }
        public string WORKING_AREA { get; set; }
        public Nullable<System.DateTime> START_DATE { get; set; }
        public string START_NOTE { get; set; }
        public Nullable<System.DateTime> END_DATE { get; set; }
        public string END_NOTE { get; set; }
        public string STAUTS { get; set; }
        public string ASET_GROUP { get; set; }
        public string UG_USER { get; set; }
    }
}