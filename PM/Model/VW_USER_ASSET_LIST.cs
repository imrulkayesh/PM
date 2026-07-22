using System;
using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class VW_USER_ASSET_LIST
    {

        [Key]
        public string ASET_ID { get; set; }
        public string ASET_TAG { get; set; }
        public string ASET_CODE_NAME { get; set; }
        public string AREA_NAME { get; set; }
        public string BODY_NAME { get; set; }
        public string BRAND_NAME { get; set; }
        public string FUEL_NAME { get; set; }
        public string COLOR_NAME { get; set; }
        public string GROUP_NAME { get; set; }
        public string ASET_MODEL { get; set; }
        public string ASET_PO_ENGINE { get; set; }
        public string ASET_GRN_CHASIS { get; set; }
        public string ASET_CAPACITY { get; set; }
        public Nullable<int> ASET_PRICE { get; set; }
        public Nullable<System.DateTime> ASET_REG_DATE { get; set; }
        public string ASET_HEIGHT { get; set; }
        public string ASET_WEIGHT { get; set; }
        public Nullable<short> ASET_MFG_YEAR { get; set; }
        public string ASET_IMAGE { get; set; }
        public string ASET_NOTE { get; set; }
        public Nullable<bool> ASET_USED { get; set; }
        public Nullable<bool> ACT { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string BUSINESS_NAME { get; set; }
        public string COMPANY_NAME { get; set; }
        public string UG_USER { get; set; }
    }
}