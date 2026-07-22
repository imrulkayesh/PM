using System;
using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class VW_BNDL_INSP
    {
        [Key]
        public string INSPH_ASET { get; set; }
        public string ASET_CODE_NAME { get; set; }
        public string INSPH_OWNR { get; set; }
        public string OWNR_USER { get; set; }
        public string INSPH_DEPARTMENT { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string DEPARTMENT_BUSINESS { get; set; }
        public string INSPH_AREA { get; set; }
        public string AREA_NAME { get; set; }
        public string INSPH_IUSER { get; set; }
        public Nullable<System.DateTime> INSPH_DATE { get; set; }
        public string MILEAGE { get; set; }
        public string INSPH_TYPE { get; set; }
        public string INSPH_NOTE { get; set; }
        public string INSPH_ID { get; set; }
        public byte INSPD_LINE { get; set; }
        public string INSPD_ITEM { get; set; }
        public string ITEM_NAME { get; set; }
        public Nullable<byte> INSPD_IQTY { get; set; }
        public Nullable<int> INSPD_IRATE { get; set; }
        public string INSPD_NOTE { get; set; }
        public string INSPH_APP { get; set; }
        public string INSPH_JOB { get; set; }
    }
}
