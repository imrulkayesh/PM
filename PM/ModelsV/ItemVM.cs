using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PM.ModelsV
{
    public class ItemVM
    {
        [Key]

        [DisplayName("Item Id")]
        public string ITEM_ID { get; set; }
        [DisplayName("Item Name")]
        public string ITEM_NAME { get; set; }
        [DisplayName("Item Code")]
        public string ITEM_CODE { get; set; }
        [DisplayName("Brand")]
        public string ITEM_BRAND { get; set; }
        [DisplayName("Flavour")]
        public string ITEM_FLAVOUR { get; set; }
        [DisplayName("License Name Id")]
        public string LICENSE_NAME_ID { get; set; }
        public bool ACT { get; set; }
        [DisplayName("License Name")]
        public string LICENSE_NAME { get; set; }
    }
}