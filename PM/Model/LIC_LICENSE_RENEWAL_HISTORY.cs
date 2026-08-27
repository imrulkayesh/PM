using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PM.Model
{
    public class LIC_LICENSE_RENEWAL_HISTORY
    {
        [Key]
        public string LIC_RENEWAL_ID { get; set; }

        public string LICENSE_ID { get; set; }

        public string COMPANY_ID { get; set; }

        public string LIC_LOCATION_ID { get; set; }

        public string LICENSE_NUMBER { get; set; }

        public string LICENSE_NAME_ID { get; set; }

        public DateTime? OLD_ISSUE_DATE { get; set; }

        public DateTime? OLD_EXPIRE_DATE { get; set; }

        public DateTime? NEW_ISSUE_DATE { get; set; }

        public DateTime? NEW_EXPIRE_DATE { get; set; }

        public DateTime? RENEWAL_DATE { get; set; }

        public decimal? LICENSE_FEE { get; set; }

        public decimal? ENTERTAINMENT_FEE { get; set; }

        public string LICENSE_STATUS_ID { get; set; }

        public string REMARK { get; set; }

        public string CDU { get; set; }

        public DateTime? CDT { get; set; }

        public decimal? ACT { get; set; }

        public virtual LIC_LICENSE LIC_LICENSE { get; set; }
    }
}