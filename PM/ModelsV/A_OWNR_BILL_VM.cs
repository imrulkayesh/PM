namespace PM.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class A_OWNR_BILL_VM
    {
        //public string BILL_ID { get; set; }
        public string BILL_OWNR { get; set; }
        public string BILL_ASET { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime BILL_DATE { get; set; }
        public long BILL_AMNT { get; set; }
        public string BILL_OWNR_NAME { get; set; }
        public string BILL_NOTE { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }
        public string ASET_ID { get; set; }
        public string OWNR_ID { get; set; }
        public string OWNR_NAME { get; set; }

        public virtual A_ASET_DATA A_ASET_DATA { get; set; }
        public virtual A_OWNR A_OWNR { get; set; }
    }
}
