using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class A_NOTIF_SET
    {
        [Key]
        public string NOTIF_ID { get; set; }
        public string NOTIF_TYPE { get; set; }
        public string NOTIF_FOR { get; set; }
        public string NOTIF_REMIND { get; set; }
        public string NOTIF_FIRST { get; set; }
        public string NOTIF_FINAL { get; set; }
    }
}