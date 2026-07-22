using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class INS_COMPANY
    {
        [Key]
        public string INS_COMPANY_ID { get; set; }
        public string COMPANY_NAME { get; set; }
        public string COMPANY_SHORT_NAME { get; set; }
        public string ADDRESS { get; set; }
        public bool ACT { get; set; }
    }
}