using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Model
{
    public class A_REMINDER_IMG
    {
        [Key]
        public string IMG_ID { get; set; }
        public string IMG_NAME { get; set; }
        public string IMG_PATH { get; set; }
        public string IMG_REMINDER { get; set; }

        [ForeignKey("IMG_REMINDER")]
        public virtual A_REMINDER A_REMINDER { get; set; }
    }
}