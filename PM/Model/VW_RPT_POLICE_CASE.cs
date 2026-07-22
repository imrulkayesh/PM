using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Model
{
    public class VW_RPT_POLICE_CASE
    {
        [Key]
        public string PLEA_ID { get; set; }
        public string PLEA_REF { get; set; }
        public string PLEA_DESCRIPTION { get; set; }
        public string PLEA_LOCATION { get; set; }
        [Column("Case On")]
        public string Case_On { get; set; }
        [Column("Paid By")]
        public string Paid_By { get; set; }
        public string PLEA_REASON_ID { get; set; }
        public string REASON_NAME { get; set; }
        public string PLEA_OWNR { get; set; }
        public string OWNR_TEXT { get; set; }
        public string OWNR_USER { get; set; }
        public string PLEA_ASET { get; set; }
        public string ASET_TAG { get; set; }
        public string ASET_CODE_NAME { get; set; }
        public string PLEA_DEPT { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string BUSINESS_ID { get; set; }
        public string BUSINESS_NAME { get; set; }
        public string PLEA_AREA { get; set; }
        public string AREA_NAME { get; set; }
        public Nullable<System.DateTime> PLEA_DATE { get; set; }
        public string PLEA_STATUS { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> PLEA_APP_DATE { get; set; }
        public string Approval { get; set; }
        public Nullable<int> PLEA_AMNT { get; set; }
    }
}