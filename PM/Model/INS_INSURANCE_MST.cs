using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Model
{
    public class INS_INSURANCE_MST
    {
        [Key]
        public string INSURANCE_MST_ID { get; set; }
        public string COMPANY_ID { get; set; }
        public string BENIFICIARY_DETAILS { get; set; }
        public string INSURED_ITEM { get; set; }
        public string OCCUPATION { get; set; }
        public string INSURANCE_TYPE_ID { get; set; }
        public string INSURANCE_COMPANY_ID { get; set; }
        public string INSURANCE_NUMBER { get; set; }
        public Nullable<System.DateTime> INSURANE_DATE { get; set; }
        public Nullable<System.DateTime> INSURANCE_EXPAIR_DATE { get; set; }
        public long INSURANCE_AMOUNT { get; set; }
        public int NET_PREMIUM { get; set; }
        public int VAT { get; set; }
        public int STAMP { get; set; }
        public int TOTAL_PREMIUM { get; set; }
        public string DESCRIPTION { get; set; }
        public Nullable<decimal> KICK_BACK_AMOUNT { get; set; }
        public Nullable<decimal> KICK_BACK_PERCENTAGE { get; set; }
        public int ACTUAL_PAID_AMOUNT { get; set; }
        public string KICK_BACK_STATUS { get; set; }
        public string REQUEST_BY { get; set; }
        public Nullable<bool> INSURANCE_STATUS { get; set; }
        public string INSURANCE_IMAGE { get; set; }
        public string KICKBACK_FROM { get; set; }
        public Nullable<bool> CLAIM_STATUS { get; set; }

        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }

        [ForeignKey("INSURANCE_COMPANY_ID")]
        public virtual INS_INSURANCE_COMPANY INS_INSURANCE_COMPANY { get; set; }
        [ForeignKey("COMPANY_ID")]
        public virtual INS_COMPANY INS_COMPANY { get; set; }
        [ForeignKey("INSURANCE_TYPE_ID")]
        public virtual INS_INSURANCE_TYPE INS_INSURANCE_TYPE { get; set; }
        [ForeignKey("KICK_BACK_STATUS")]
        public virtual INS_KICK_BACK_STATUS INS_KICK_BACK_STATUS { get; set; }

    }
}