using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PM.ModelsV
{
    public class TarminationVM
    {
        [DisplayName("Aset Id")]
        [Required(ErrorMessage = "Aset is reaquired")]

        public string ASET_ID { get; set; }

        [DisplayName("User Id")]
        public string OWNR_ID { get; set; }


        [DisplayName("Current Price")]
        [Required(ErrorMessage = "Price is reaquired")]
        public Nullable<int> ASET_PRICE { get; set; }

        [DisplayName("Service Duration")]
        public Nullable<short> SERVICE_MONTH { get; set; }

        [DisplayName("Comment")]
        [Required(ErrorMessage = "First Comment is reaquired")]
        public string FIRST_COMMENT { get; set; }

        [DisplayName("First User")]
        public string FIRST_USER { get; set; }

        [DisplayName("First Date")]
        public Nullable<System.DateTime> FIRST_DATE { get; set; }

        [DisplayName("Tarm. Type")]
        [Required(ErrorMessage = "Type is reaquired")]
        public string APP_TYPE { get; set; }

        //public virtual A_OWNR A_OWNR { get; set; }
    }
    public class TarminationApprovalVM
    {
        [DisplayName("Aset Id")]
        [Required(ErrorMessage = "Aset is reaquired")]
        public string ASET_ID { get; set; }

        [DisplayName("Comment")]
        [Required(ErrorMessage = "Final Comment is reaquired")]
        public string FINAL_COMMENT { get; set; }

        [DisplayName("Final User")]
        public string FINAL_USER { get; set; }

        [DisplayName("Final Date")]
        public Nullable<System.DateTime> FINAL_DATE { get; set; }
    }
    public partial class TarminationViewVM
    {
        [DisplayName("Aset Id")]
        public string ASET_ID { get; set; }

        [DisplayName("User Id")]
        public string OWNR_ID { get; set; }

        [DisplayName("Current Price")]
        public Nullable<int> ASET_PRICE { get; set; }

        [DisplayName("Service Duration")]
        public Nullable<short> SERVICE_MONTH { get; set; }

        [DisplayName("Comment")]
        public string FIRST_COMMENT { get; set; }

        [DisplayName("First User")]
        public string FIRST_USER { get; set; }

        [DisplayName("First Date")]
        public Nullable<System.DateTime> FIRST_DATE { get; set; }

        [DisplayName("Final Comment")]
        public string FINAL_COMMENT { get; set; }

        [DisplayName("Final User")]
        public string FINAL_USER { get; set; }

        [DisplayName("Final Date")]
        public Nullable<System.DateTime> FINAL_DATE { get; set; }

        [DisplayName("Tarm. Type")]
        public string APP_TYPE { get; set; }
    }
}