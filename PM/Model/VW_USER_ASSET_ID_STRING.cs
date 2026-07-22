using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class VW_USER_ASSET_ID_STRING
    {
        [Key]
        public string ASET_ID { get; set; }
        public string UG_USER { get; set; }
    }
}