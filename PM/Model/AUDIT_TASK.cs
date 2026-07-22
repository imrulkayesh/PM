using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PM.Model
{
    public class AUDIT_TASK
    {
        [Key]
        public string TASK_ID { get; set; }
        public string AUDITOR_ID { get; set; }
        public string TASK_DETAILS { get; set; }

        public Nullable<System.DateTime> TASK_ASSIGN_DATE { get; set; }

    
        public Nullable<System.DateTime> TASK_START_PLANED_DATE { get; set; }

  
        public Nullable<System.DateTime> TASK_DEADLINE { get; set; }

        public Nullable<System.DateTime> TASK_START_DATE { get; set; }

        public string TASK_STATUS { get; set; }

        public string REMARKS { get; set; }

        public string LOCATION_ID { get; set; }
        public string COMPANY_GROUP_ID { get; set; }
        public string COMPANY_ID { get; set; }
        public string DEPARTMENT_ID { get; set; }
        public string TASK_COMMUNICATION_BY_ID { get; set; }
        public string TASK_ASSIGN_BY_ID { get; set; }
        public string MANAGEMENT_SOURCE_ID { get; set; }
        public string TASK_PRIORITY_ID { get; set; }
        public string REPORT_STATUS_ID { get; set; }
        public Nullable<System.DateTime> MANAGEMENT_SOURCE_DATE { get; set; }
        public string TASK_TYPE_ID { get; set; }
        public string REPORT_CREATE_STATUS { get; set; }

        public Nullable<decimal> FINANCIAL { get; set; }
        public string NON_FINANCIAL { get; set; }

        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
    }
}