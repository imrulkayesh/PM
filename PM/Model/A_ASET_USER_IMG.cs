using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Model
{
    public class A_ASET_USER_IMG
    {
        [Key]
        public string IMG_ID { get; set; }
        public string IMG_NAME { get; set; }
        public string IMG_PATH { get; set; }
        public string IMG_AU { get; set; }

        [ForeignKey("IMG_AU")]
        public virtual A_ASET_USER A_ASET_USER { get; set; }
    }
}