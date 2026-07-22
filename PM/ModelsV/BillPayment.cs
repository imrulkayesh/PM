using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PM.ModelsV
{
    public class BillPayment
    {
        [DisplayName("Bill Number")]
        public string BILH_ID { get; set; }

        [DisplayName("Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> BILH_DATE { get; set; }

        [DisplayName("Note")]
        public string BILH_NOTE { get; set; }

        [DisplayName("Reference")]
        public string BILH_REF { get; set; }

        [DisplayName("Job Number")]
        public string JOBH_ID { get; set; }

        [DisplayName("Receiver")]
        public string BILH_RECEIVER { get; set; }

        [DisplayName("Payment")]
        public string BILH_PAID { get; set; }

        [DisplayName("Insurance")]
        public Nullable<decimal> BILH_IPAY { get; set; }

        [DisplayName("User")]
        public Nullable<decimal> BILH_UPAY { get; set; }

        [DisplayName("Company")]
        public Nullable<decimal> BILH_CPAY { get; set; }
    }
    public class NewBillPayment
    {
        [Required(ErrorMessage = "Asset is required")]
        public string BILH_ASET { get; set; }
        [Required(ErrorMessage = "Job Card is required")]
        public string JOBH_ID { get; set; }
        [Required(ErrorMessage = "Date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> BILH_DATE { get; set; }
        public string BILH_NOTE { get; set; }
        public string BILH_REF { get; set; }
        [Required(ErrorMessage = "Receiver Name is required")]
        public string BILH_RECEIVER { get; set; }

    }

    public class BillPrint
    {
        public string BILH_ID { get; set; }
        public string ASET_CODE_NAME { get; set; }
        public string BILH_MILEAGE { get; set; }
        public string ASET_USER { get; set; }
        public string BILL_USER { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<DateTime> INSP_DATE { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<DateTime> JOB_DATE { get; set; }
        public string INSP_BY { get; set; }
        public string INSP_NOTE { get; set; }
        public string JOB_VENDOR { get; set; }
        public string JOB_NOTE { get; set; }
        public string INSPH_ID { get; set; }
        public string JOBH_ID { get; set; }
        public string INSP_APP { get; set; }
        public string JOBH_APP { get; set; }
        public Nullable<decimal> USER_PAY { get; set; }
        public Nullable<decimal> INSU_PAY { get; set; }
        public Nullable<decimal> BILH_CPAY { get; set; }
        public string REF_NO { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<DateTime> BILL_DATE { get; set; }
        public decimal iTotal { get; set; }
        public decimal jTotal { get; set; }
        public string PAID_BY { get; set; }
        public string BILLH_NOTE { get; set; }
        public List<BillPrintINS> INSP_ITEM { get; set; }
        public List<BillPrintJOB> JOBH_ITEM { get; set; }

    }
    public class BillPrintINS
    {
        public string iITEM_NAME { get; set; }
        public Nullable<byte> iQty { get; set; }
        public Nullable<decimal> iRate { get; set; }
        public Nullable<decimal> iAmount { get; set; }

    }
    public class BillPrintJOB
    {
        public string jITEM_NAME { get; set; }
        public Nullable<byte> jQty { get; set; }
        public Nullable<decimal> jRate { get; set; }
        public Nullable<decimal> jAmount { get; set; }
    }


    public class BundleBillAuto
    {
        [Required(ErrorMessage = "Bundle id is required")]
        public string BUNDLE_ID { get; set; }

        [Required(ErrorMessage = "Receiver Name is required")]
        public string RECEIVER_NAME { get; set; }

        [Required(ErrorMessage = "Bill Note is required")]
        public string BILL_NOTE { get; set; }
    }
}