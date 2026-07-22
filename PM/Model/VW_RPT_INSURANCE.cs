using System;
using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class VW_RPT_INSURANCE
    {
        [Key]
        public string INSU_ASET { get; set; }
        public string ASET_CODE_NAME { get; set; }
        public string ASET_AREA { get; set; }
        public string AREA_NAME { get; set; }
        public string DEPARTMENT_ID { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string BUSINESS_ID { get; set; }
        public string BUSINESS_NAME { get; set; }
        public string INSU_ID { get; set; }
        public int INSU_LINE { get; set; }
        public Nullable<int> INSU_ASET_VALUE { get; set; }
        public Nullable<int> INSU_PREMIUM { get; set; }
        public Nullable<int> INSU_VAT_AMOUNT { get; set; }
        public Nullable<System.DateTime> INSU_DATE { get; set; }
        public string INSU_PAID { get; set; }
        public Nullable<bool> DACT { get; set; }
        public string INSU_POLICY { get; set; }
        public string INSU_COMP { get; set; }
        public string INSU_COMP_ADDRESS { get; set; }
        public string INSU_NOTE { get; set; }
        public Nullable<System.DateTime> INSU_REG_END { get; set; }
        public Nullable<bool> HACT { get; set; }
    }
}