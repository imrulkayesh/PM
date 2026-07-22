using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PM.ModelsV
{
    public class AssetAcceVM
    {
        [DisplayName("Asset")]
        [Required(ErrorMessage = "Asset is reaquired")]
        public string ASET_ID { get; set; }

        public string ASET_ID_V { get; set; }

        [DisplayName("Item")]
        [Required(ErrorMessage = "Item is reaquired")]
        public string ACCE_ID { get; set; }

        public string ACCE_ID_V { get; set; }

        [DisplayName("Note")]
        public string AAI_DESC { get; set; }

        [DisplayName("Qty")]
        [Required(ErrorMessage = "Qty is reaquired")]
        public Nullable<byte> AAI_QUANTITY { get; set; }

        [DisplayName("Rate")]
        [Required(ErrorMessage = "Rate is reaquired")]
        public Nullable<int> AAI_RATE { get; set; }

        [DisplayName("Amount")]
        public Nullable<int> Amount { get; set; }

        [DisplayName("Status")]
        public Nullable<bool> ACT { get; set; }
    }
    public class AssetAcceAdjVM
    {
        [DisplayName("Asset")]
        [Required(ErrorMessage = "Asset is reaquired")]
        public string ASET_ID { get; set; }

        [DisplayName("Item")]
        [Required(ErrorMessage = "Item is reaquired")]
        public string ACCE_ID { get; set; }

        [DisplayName("Qty")]
        [Required(ErrorMessage = "Qty is reaquired")]
        public Nullable<byte> AAI_QUANTITY { get; set; }

        [DisplayName("Rate")]
        [Required(ErrorMessage = "Rate is reaquired")]
        public Nullable<int> AAI_RATE { get; set; }

        [DisplayName("Note")]
        public string AUA_DESC { get; set; }

        [DisplayName("Type")]
        [Required(ErrorMessage = "Adjustment Type is reaquired")]
        public string AUA_TYPE { get; set; }
    }
}