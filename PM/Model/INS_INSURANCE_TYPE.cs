using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class INS_INSURANCE_TYPE
    {
        [Key]
        public string INSURANCE_TYPE_ID { get; set; }
        public string INSURANCE_TYPE_NAME { get; set; }
        public string REMARKS { get; set; }
        public bool ACT { get; set; }

    }
}