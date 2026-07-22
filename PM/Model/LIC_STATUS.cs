using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class LIC_STATUS
    {
        [Key]
        public string LICENSE_STATUS_ID { get; set; }
        public string LICENSE_STATUS_NAME { get; set; }
        public string REMARKS { get; set; }
        public bool ACT { get; set; }
    }
}