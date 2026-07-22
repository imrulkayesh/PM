using System;

namespace PM.Model
{
    public class VW_INSURANCE_S1
    {
        public string INSU_ID { get; set; }
        public string ASET_NAME_CODE { get; set; }
        public string ASET_GROUP { get; set; }
        public string DEPARTMENT_BUSINESS { get; set; }
        public string ASET_AREA { get; set; }
        public string INSU_POLICY { get; set; }
        public Nullable<int> INSU_PREMIUM { get; set; }
        public string INSU_COMP { get; set; }
        public string INSU_COMP_ADDRESS { get; set; }
        public string INSU_NOTE { get; set; }
        public Nullable<System.DateTime> INSU_REG_START { get; set; }
        public Nullable<System.DateTime> INSU_REG_END { get; set; }
        public Nullable<bool> REMIND { get; set; }
        public Nullable<decimal> DAYS { get; set; }
    }
}