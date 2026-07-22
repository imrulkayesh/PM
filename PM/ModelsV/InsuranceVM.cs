using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PM.ModelsV
{
    public class InsuranceVM
    {
        public string INSU_ID { get; set; }
        [Required]
        [DisplayName("Asset")]
        public string INSU_ASET { get; set; }
        [Required]
        [DisplayName("Policy")]
        public string INSU_POLICY { get; set; }
        [Required]
        [DisplayName("Asset Value")]
        public int INSU_ASET_VALUE { get; set; }
        [Required]
        [DisplayName("Premium")]
        public int INSU_PREMIUM { get; set; }
        [Required]
        [DisplayName("Company")]
        public string INSU_COMP { get; set; }
        [DisplayName("Address")]
        public string INSU_COMP_ADDRESS { get; set; }
        [DisplayName("Note")]
        public string INSU_NOTE { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DisplayName("Reg Date")]
        public DateTime INSU_REG_START { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DisplayName("Expire Date")]
        public DateTime INSU_REG_END { get; set; }
        public int INSU_VAT_AMOUNT { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime INSU_DATE { get; set; }
    }
    public class UpdateInsuranceSave
    {
        [Required]
        public string INSU_ASET { get; set; }
        [Required]
        public string INSU_POLICY { get; set; }
        [Required]
        public int INSU_ASET_VALUE { get; set; }
        [Required]
        public int INSU_PREMIUM { get; set; }
        public string INSU_NOTE { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime INSU_REG_START { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime INSU_REG_END { get; set; }
        public int INSU_VAT_AMOUNT { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime INSU_DATE { get; set; }
    }
    public class InsuranceDetail
    {
        [Required]
        [DisplayName("Policy")]
        public string INSU_POLICY { get; set; }
        [DisplayName("Line")]
        public int INSU_LINE { get; set; }
        [DisplayName("Premium")]
        public int INSU_PREMIUM { get; set; }
        [DisplayName("Amount")]
        public int INSU_AMOUNT { get; set; }
        [DisplayName("Vat")]
        public int INSU_VAT_AMOUNT { get; set; }
        [DisplayName("Pay Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<DateTime> INSU_DATE { get; set; }
    }

    public class InsuranceVMBD
    {
        public string INSU_ID { get; set; }
        [Required]
        [DisplayName("Asset")]
        public string INSU_ASET { get; set; }
        [Required]
        [DisplayName("Policy")]
        public string INSU_POLICY { get; set; }
        [Required]
        [DisplayName("Asset Capacity")]
        public string ASET_CAPACITY { get; set; }
        [Required]
        [DisplayName("Premium")]
        public int INSU_PREMIUM { get; set; }
        [Required]
        [DisplayName("Company")]
        public string INSU_COMP { get; set; }
        [DisplayName("Address")]
        public string INSU_COMP_ADDRESS { get; set; }
        [DisplayName("Note")]
        public string INSU_NOTE { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DisplayName("Reg Date")]
        public DateTime INSU_REG_START { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DisplayName("Expire Date")]
        public DateTime INSU_REG_END { get; set; }
        public int INSU_VAT_AMOUNT { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime INSU_DATE { get; set; }
    }

}