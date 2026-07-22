using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class INS_CLAIM_STATUS
    {
        [Key]
        public string CLAIM_STATUS_ID { get; set; }
        public string CLAIM_STATUS_NAME { get; set; }
        public string REMARK { get; set; }
        public bool ACT { get; set; }
    }
}