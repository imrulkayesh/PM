using System;

namespace PM.Model
{
    public class VW_NOTIF_MAIL_METER
    {
        public string REMIND_ID { get; set; }
        public string ASET_ID { get; set; }
        public string ASET_TAG { get; set; }
        public string ASET_CODE_NAME { get; set; }
        public string ASET_DEPARTMENT { get; set; }
        public string DEPARTMENT_BUSINESS { get; set; }
        public string ASET_GROUP { get; set; }
        public string ITEM_ID { get; set; }
        public string ITEM_NAME { get; set; }
        public string ITEM_NOTIF_SET { get; set; }
        public string NOTIF_TYPE { get; set; }
        public string NOTIF_REMIND { get; set; }
        public string NOTIF_FIRST { get; set; }
        public string NOTIF_FINAL { get; set; }
        public Nullable<System.DateTime> C_DATE { get; set; }
        public Nullable<System.DateTime> N_DATE { get; set; }
        public string REMIND { get; set; }
        public string NOTE { get; set; }
        public string COMPLETED { get; set; }
        public Nullable<decimal> ELAPSED { get; set; }
        public string REMINDER_NOTE { get; set; }
        public string NOTIF_MESSAGES { get; set; }
        public string MAIL_SEND { get; set; }
        public string MAIL_SUBJECT { get; set; }
        public string MAIL_BODY { get; set; }
        public string UPDT { get; set; }
    }
}