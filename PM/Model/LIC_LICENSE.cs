using System;
using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class LIC_LICENSE
    {
        [Key]
        public string LICENSE_ID { get; set; }
        public string COMPANY_ID { get; set; }
        public string LIC_LOCATION_ID { get; set; }
        public string ITEM_ID { get; set; }
        public string LICENSE_NAME_ID { get; set; }
        public string LICENSE_NUMBER { get; set; }
        public string ISSUING_AUTHORITY_ID { get; set; }
        public Nullable<System.DateTime> ISSUE_DATE { get; set; }
        public Nullable<System.DateTime> EXPIRE_DATE { get; set; }
        public string LICENSE_HOLDER_MOBILE { get; set; }
        public string LICENSE_HOLDER_EMAIL { get; set; }
        public int LICENSE_FEE { get; set; }
        public string REMARK { get; set; }
        public string LICENSE_STATUS_ID { get; set; }
        public DateTime APPLICATION_DATE { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public int ENTERTAINMENT_FEE { get; set; }
    }
}