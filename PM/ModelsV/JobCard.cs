using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PM.ModelsV
{
    public class JobCard
    {
        [Required(ErrorMessage = "Inspection is required")]
        public string INSPH_ID { get; set; }
        [Required(ErrorMessage = "Aset is required")]
        public string JOBH_ASET { get; set; }
        [Required(ErrorMessage = "Date is required")]
        public Nullable<System.DateTime> JOBH_DATE { get; set; }
        public string JOBH_VENDOR { get; set; }
        public string JOBH_NOTE { get; set; }
        public Nullable<decimal> JOBH_IPAY { get; set; }
        public Nullable<decimal> JOBH_UPAY { get; set; }
        [Required(ErrorMessage = "Items are required")]
        public List<JobCardDetails> JobCardDetails { get; set; }
    }
    public class JobCardDetails
    {
        public string JOBH_ID { get; set; }

        [DisplayName("Line")]
        public byte JOBD_LINE { get; set; }

        [DisplayName("Code")]
        [Required(ErrorMessage = "Item is required")]
        public string JOBD_ITEM { get; set; }

        [DisplayName("Name")]
        public string JOBD_ITEM_NAME { get; set; }
        [DisplayName("IQty")]
        public Nullable<byte> JOBD_IQTY { get; set; }

        [DisplayName("JQty")]
        [Required(ErrorMessage = "Qty is required")]
        public Nullable<byte> JOBD_JQTY { get; set; }
        [DisplayName("IRate")]
        public Nullable<decimal> JOBD_IRATE { get; set; }

        [DisplayName("JRate")]
        [Required(ErrorMessage = "Rate is required")]
        public Nullable<decimal> JOBD_JRATE { get; set; }

        [DisplayName("Note")]
        [Required(ErrorMessage = "Note is required")]
        public string JOBD_NOTE { get; set; }
        public string JOBD_STATE { get; set; }
    }
    public class JobCardDetails_With_Old
    {
        public string JOBH_ID { get; set; }

        [DisplayName("Line")]
        public byte JOBD_LINE { get; set; }

        [DisplayName("Code")]
        [Required(ErrorMessage = "Item is required")]
        public string JOBD_ITEM { get; set; }

        [DisplayName("Name")]
        public string JOBD_ITEM_NAME { get; set; }
        [DisplayName("IQty")]
        public Nullable<byte> JOBD_IQTY { get; set; }

        [DisplayName("JQty")]
        [Required(ErrorMessage = "Qty is required")]
        public Nullable<byte> JOBD_JQTY { get; set; }
        [DisplayName("IRate")]
        public Nullable<decimal> JOBD_IRATE { get; set; }

        [DisplayName("JRate")]
        [Required(ErrorMessage = "Rate is required")]
        public Nullable<decimal> JOBD_JRATE { get; set; }

        [DisplayName("Note")]
        [Required(ErrorMessage = "Note is required")]
        public string JOBD_NOTE { get; set; }
        public string JOBD_STATE { get; set; }
        [DisplayName("Last Jobcard date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dddd, dd MMMM yyyy}")]
        public Nullable<System.DateTime> JOBH_DATE { get; set; }
        [DisplayName("Last Jobcard mileage")]
        public string JOBH_MILEAGE { get; set; }
    }


    public class EditJobCard
    {
        [Required(ErrorMessage = "Job is required")]
        public string JOBH_ID { get; set; }
        public string INSPH_ID { get; set; }
        [Required(ErrorMessage = "Aset is required")]
        public string JOBH_ASET { get; set; }
        public string JOBH_VENDOR { get; set; }
        public string JOBH_NOTE { get; set; }
        public Nullable<decimal> JOBH_IPAY { get; set; }
        public Nullable<decimal> JOBH_UPAY { get; set; }

        [Required(ErrorMessage = "Items are required")]
        public List<JobCardDetails> JobCardDetails { get; set; }
    }

    public class JobCardView
    {
        [DisplayName("Job Number")]
        public string JOBH_ID { get; set; }
        public string JOBH_ASET_ID { get; set; }
        [DisplayName("Asset")]
        public string JOBH_ASET { get; set; }
        [DisplayName("User")]
        public string JOBH_OWNR { get; set; }
        [DisplayName("Department")]
        public string JOBH_DEPARTMENT { get; set; }
        [DisplayName("Area")]
        public string JOBH_AREA { get; set; }
        [DisplayName("Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime JOBH_DATE { get; set; }
        [DisplayName("MIleage")]
        public string JOBH_MILEAGE { get; set; }
        [DisplayName("Vendor")]
        public string JOBH_VENDOR { get; set; }
        [DisplayName("Note")]
        public string JOBH_NOTE { get; set; }
        [DisplayName("Insurance")]
        public decimal JOBH_IPAY { get; set; }
        [DisplayName("User")]
        public decimal JOBH_UPAY { get; set; }
        [DisplayName("Company")]
        public decimal JOBH_CPAY { get; set; }
        [DisplayName("I Total")]
        public decimal JOBH_IT { get; set; }
        [DisplayName("J Total")]
        public decimal JOBH_JT { get; set; }

        [DisplayName("Job Type")]
        public string JOBH_TYPE { get; set; }
    }
}