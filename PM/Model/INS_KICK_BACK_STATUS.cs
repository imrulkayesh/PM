using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class INS_KICK_BACK_STATUS
    {
        [Key]
        public string KICK_BACK_STATUS_ID { get; set; }
        public string KICK_BACK_STATUS_NAME { get; set; }
        public string REMARK { get; set; }
        public bool ACT { get; set; }
    }
}