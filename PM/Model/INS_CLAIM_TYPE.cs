using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class INS_CLAIM_TYPE
    {
        [Key]
        public string CLAIM_TYPE_ID { get; set; }
        public string CLAIM_NAME { get; set; }
        public string REMARK { get; set; }
        public bool ACT { get; set; }
    }
}