using System;

namespace PM.Model
{
    public class VW_IQAMA_NOTIF_V1
    {
        public string OWNR_ID { get; set; }
        public string OWNR_TEXT { get; set; }
        public string OWNR_USER { get; set; }
        public string OWNR_NAME { get; set; }
        public Nullable<System.DateTime> AAJKEY { get; set; }
        public Nullable<System.DateTime> OWNR_NDATE { get; set; }
        public Nullable<decimal> ELAPSED { get; set; }
        public Nullable<int> REMIND { get; set; }
        public string BUSINESS_ID { get; set; }
        public string OWNR_AREA { get; set; }
        public string GROUP_NAME { get; set; }
        public string REMINDER_NOTE { get; set; }
        public string NOTIF_MESSAGES { get; set; }
        public string MAIL_SEND { get; set; }
        public string UPDT { get; set; }
    }
}