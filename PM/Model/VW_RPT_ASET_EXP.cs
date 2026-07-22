using System;
using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class VW_RPT_ASET_EXP
    {
        [Key]
        public string JOBH_ID { get; set; }
        public byte JOBD_LINE { get; set; }
        public string JOBD_ITEM { get; set; }
        public string ITEM_NAME { get; set; }
        public Nullable<byte> JOBD_IQTY { get; set; }
        public Nullable<int> JOBD_IRATE { get; set; }
        public Nullable<decimal> JOBD_IAMNT { get; set; }
        public Nullable<byte> JOBD_JQTY { get; set; }
        public Nullable<int> JOBD_JRATE { get; set; }
        public Nullable<decimal> JOBD_JAMNT { get; set; }
        public string JOBD_NOTE { get; set; }
        public string INSPH_ID { get; set; }
        public string JOBH_ASET { get; set; }
        public string ASET_CODE_NAME { get; set; }
        public string JOBH_OWNR { get; set; }
        public string OWNR_USER { get; set; }
        public string JOBH_DEPARTMENT { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string BUSINESS_ID { get; set; }
        public string BUSINESS_NAME { get; set; }
        public string JOBH_AREA { get; set; }
        public string AREA_NAME { get; set; }
        public Nullable<System.DateTime> JOBH_DATE { get; set; }
        public string JOBH_MILEAGE { get; set; }
        public string JOBH_VENDOR { get; set; }
        public string JOBH_NOTE { get; set; }
        public string JOBH_TYPE { get; set; }
        public string ITYPE_NAME { get; set; }
        public string INSPH_IUSER { get; set; }
        public string CDU { get; set; }

    }
}