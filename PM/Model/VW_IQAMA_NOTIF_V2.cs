using System;

namespace PM.Model
{
    public class VW_IQAMA_NOTIF_V2
    {
        public string OWNR_ID { get; set; }
        public string OWNR_TEXT { get; set; }
        public string OWNR_USER { get; set; }
        public string OWNR_NAME { get; set; }
        public string OWNR_NOTE { get; set; }
        public Nullable<System.DateTime> AAJ_DATE { get; set; }
        public Nullable<System.DateTime> EXP_DATE { get; set; }
        public Nullable<int> REMIND { get; set; }
        public string BUSINESS_ID { get; set; }
        public string GROUP_NAME { get; set; }
        public Nullable<decimal> ELAPSED { get; set; }
        public string MESSAGES { get; set; }
        public string MESSAGE_NOTE { get; set; }
        public string MAIL_SEND { get; set; }
        public string MAIL_SUBJECT { get; set; }
        public string MAIL_BODY { get; set; }
        public string UPDT { get; set; }
    }
}