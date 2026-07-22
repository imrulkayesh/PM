using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class LIC_LOCATION
    {
        [Key]
        public string LIC_LOCATION_ID { get; set; }
        public string LOCATION_NAME { get; set; }
        public string SHORT_NAME { get; set; }
        public string ADDRESS { get; set; }
        public bool ACT { get; set; }
    }
}