using System;
using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class VW_ASSETUSERMANAGERVM
    {
        [Key]
        public string AU_ID { get; set; }
        public string ASET_CODE_NAME { get; set; }
        public string OWNR_TEXT { get; set; }
        public string OWNR_USER { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string AREA_NAME { get; set; }
        public Nullable<System.DateTime> AU_SRDATE { get; set; }
        public string T_USER { get; set; }
        public Nullable<System.DateTime> AU_SADATE { get; set; }
        public string T_APPROVER { get; set; }
        public string AU_SNOTE { get; set; }
        public Nullable<System.DateTime> AU_ERDATE { get; set; }
        public string R_USER { get; set; }
        public Nullable<System.DateTime> AU_EADATE { get; set; }
        public string R_APPROVER { get; set; }
        public string AU_ENOTE { get; set; }
        public string UG_USER { get; set; }
        public string AU_TRNSTYPE { get; set; }
        public string BRAND_NAME { get; set; }
        public string BODY_NAME { get; set; }
        public string ASET_MODEL { get; set; }
    }
}