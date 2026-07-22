using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PM.ModelsV
{
    public class rptAssetManagerVM
    {
    }


    public class PranBike
    {
        public string ASET_BUSINESS { get; set; }
        public string ASET_DEPARTMENT { get; set; }
        public string ASET_GROUP { get; set; }
        public string ASET_ID { get; set; }
    }
    public class rptPRANBike
    {
        [DisplayName("Id")]
        public string ASET_ID { get; set; }

        [DisplayName("Tag")]
        public string ASET_TAG { get; set; }

        [DisplayName("Bike No")]
        public string ASET_CODE_NAME { get; set; }

        [DisplayName("Department")]
        public string ASET_DEPARTMENT { get; set; }

        [DisplayName("Area")]
        public string ASET_AREA { get; set; }

        [DisplayName("Brand")]
        public string ASET_BRAND { get; set; }

        [DisplayName("Group")]
        public string ASET_GROUP { get; set; }

        [DisplayName("Model")]
        public string ASET_MODEL { get; set; }

        [DisplayName("Engine")]
        public string ASET_PO_ENGINE { get; set; }

        [DisplayName("Chasis")]
        public string ASET_GRN_CHASIS { get; set; }

        [DisplayName("Price")]
        public Nullable<int> ASET_PRICE { get; set; }

        [DisplayName("Reg Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> ASET_REG_DATE { get; set; }

        [DisplayName("Year")]
        public Nullable<short> ASET_MFG_YEAR { get; set; }

        [DisplayName("Note")]
        public string ASET_NOTE { get; set; }

        [DisplayName("Status")]
        public Nullable<bool> ACT { get; set; }
    }

}