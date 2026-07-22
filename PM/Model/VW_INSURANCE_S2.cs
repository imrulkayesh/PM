using System;

namespace PM.Model
{
    public class VW_INSURANCE_S2
    {
        public string INSU_ID { get; set; }
        public string ASET_NAME_CODE { get; set; }
        public string INSURANCE { get; set; }
        public string COMPANY { get; set; }
        public string INSU_NOTE { get; set; }
        public string MESSAGES { get; set; }
        public string NEED_MAIL { get; set; }
        public Nullable<System.DateTime> INSU_REG_START { get; set; }
        public Nullable<System.DateTime> INSU_REG_END { get; set; }
        public Nullable<decimal> DAYS { get; set; }
        public string MAIL_SUBJECT { get; set; }
        public string MAIL_BODY { get; set; }
        public string UPDT { get; set; }
        public string ASET_GROUP { get; set; }
        public string DEPARTMENT_BUSINESS { get; set; }
    }
}