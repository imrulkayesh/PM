using System;
using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class VW_RPT_BILL
    {
        [Key]
        public string BILH_ID { get; set; }
        public Nullable<System.DateTime> BILH_DATE { get; set; }
        public string BILH_NOTE { get; set; }
        public string BILH_REF { get; set; }
        public string BILH_RECEIVER { get; set; }
        public Nullable<int> BILH_IPAY { get; set; }
        public Nullable<int> BILH_UPAY { get; set; }
        public Nullable<int> BILH_CPAY { get; set; }
        public Nullable<int> BILH_DISC { get; set; }
        public string JOBH_ASET { get; set; }
        public string ASET_CODE_NAME { get; set; }
        public string JOBH_OWNR { get; set; }
        public string OWNR_USER { get; set; }
        public string JOBH_DEPARTMENT { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string DEPARTMENT_BUSINESS { get; set; }
        public string BUSINESS_NAME { get; set; }
        public string JOBH_AREA { get; set; }
        public string AREA_NAME { get; set; }
        public string JOBH_TYPE { get; set; }
        public string ITYPE_NAME { get; set; }
    }
}