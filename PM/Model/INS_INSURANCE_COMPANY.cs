using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class INS_INSURANCE_COMPANY
    {
        [Key]
        public string INSURANCE_COMPANY_ID { get; set; }
        public string INS_COMPANY_NAME { get; set; }
        public string ADDRESS { get; set; }
        public string MOBILE { get; set; }
        public string EMAIL { get; set; }
        public bool ACT { get; set; }
    }
}