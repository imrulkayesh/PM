using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace PM.ModelsV
{
    public class Inspection
    {
    }
    public class newInspection
    {
        public string INSPH_ID { get; set; }

        [Required(ErrorMessage = "Asset is required")]
        public string INSPH_ASET { get; set; }

        [Required(ErrorMessage = "Inspector is required")]
        public string INSPH_IUSER { get; set; }

        [Required(ErrorMessage = "Mileage is required")]
        public string MILEAGE { get; set; }

        [Required(ErrorMessage = "Date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> INSPH_DATE { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public string INSPH_TYPE { get; set; }

        [StringLength(maximumLength: 80, ErrorMessage = "{0} maximum length is {1} and minimum {2}", MinimumLength = 2)]
        public string INSPH_NOTE { get; set; }

        [Required(ErrorMessage = "Department is required")]
        public string A_DEPTF { get; set; }

        [Required(ErrorMessage = "Area is required")]
        public string A_AREAF { get; set; }

        [Required(ErrorMessage = "Asset User is required")]
        public string A_USERF { get; set; }

        [Required(ErrorMessage = "Items are required")]
        public List<InspectionD> InspectionD { get; set; }

    }

    public class InspectionD
    {
        public string INSPH_ID { get; set; }

        [DisplayName("Line")]
        public byte INSPD_LINE { get; set; }

        [DisplayName("Code")]
        [Required(ErrorMessage = "Item is required")]
        public string INSPD_ITEM { get; set; }

        [DisplayName("Name")]
        public string INSPD_ITEM_NAME { get; set; }

        [DisplayName("Qty")]
        [Required(ErrorMessage = "Qty is required")]
        public Nullable<byte> INSPD_IQTY { get; set; }

        [DisplayName("Rate")]
        [Required(ErrorMessage = "Rate is required")]
        public Nullable<decimal> INSPD_IRATE { get; set; }

        [DisplayName("Note")]
        public string INSPD_NOTE { get; set; }
    }
    public class InspectionDx
    {
        public string INSPH_ID { get; set; }
        public string INSPH_ASET { get; set; }
        public string INSPD_ITEM { get; set; }
        public Nullable<byte> INSPD_IQTY { get; set; }
        public Nullable<DateTime> INSPH_DATE { get; set; }
        public string MILEAGE { get; set; }
    }
    public class InspectionD_With_Old
    {
        public string INSPH_ID { get; set; }

        [DisplayName("Line")]
        public byte INSPD_LINE { get; set; }

        [DisplayName("Code")]
        [Required(ErrorMessage = "Item is required")]
        public string INSPD_ITEM { get; set; }

        [DisplayName("Name")]
        public string INSPD_ITEM_NAME { get; set; }

        [DisplayName("Qty")]
        [Required(ErrorMessage = "Qty is required")]
        public Nullable<byte> INSPD_IQTY { get; set; }

        [DisplayName("Rate")]
        [Required(ErrorMessage = "Rate is required")]
        public Nullable<decimal> INSPD_IRATE { get; set; }

        [DisplayName("Note")]
        public string INSPD_NOTE { get; set; }
        [DisplayName("Last Inspection date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dddd, dd MMMM yyyy}")]
        public Nullable<DateTime> INSPH_DATE { get; set; }
        [DisplayName("Last Inspection mileage")]
        public string MILEAGE { get; set; }
    }

    public class InspectionView
    {
        [DisplayName("Inspection")]
        public string INSPH_ID { get; set; }

        [DisplayName("Asset Id")]
        public string INSPH_ASET_ID { get; set; }

        [DisplayName("Asset")]
        public string INSPH_ASET { get; set; }

        [DisplayName("User")]
        public string INSPH_OWNR { get; set; }

        [DisplayName("Department")]
        public string INSPH_DEPARTMENT { get; set; }

        [DisplayName("Area")]
        public string INSPH_AREA { get; set; }

        [DisplayName("Inspection User")]
        public string INSPH_IUSER { get; set; }

        [DisplayName("Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime INSPH_DATE { get; set; }

        [DisplayName("Mileage")]
        public string MILEAGE { get; set; }

        [DisplayName("Type")]
        public string INSPH_TYPE { get; set; }
    }

    public class AttachFile
    {
        [Required(ErrorMessage = "TRN is required")]
        public string TRN_ID { get; set; }

        [Required(ErrorMessage = "File is required")]
        public HttpPostedFileBase file { get; set; }
    }


    public class NewBundleInspection
    {
        [Required(ErrorMessage = "Item is required")]
        public string INSPD_ITEM { get; set; }

        [Required(ErrorMessage = "Inspector is required")]
        public string INSPH_IUSER { get; set; }

        //[Required(ErrorMessage = "Date is required")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> INSPH_DATE { get; set; }

        [Required(ErrorMessage = "Items are required")]
        public List<NewBundleInspectionD> NewBundleInspectionD { get; set; }
    }
    public class NewBundleInspectionD
    {
        [Required(ErrorMessage = "Assets are required")]
        public string INSPH_ASET { get; set; }

        [Required(ErrorMessage = "Mileages are required")]
        public string MILEAGE { get; set; }

        [Required(ErrorMessage = "Rates are required")]
        public Nullable<int> INSPD_IRATE { get; set; }

        [Required(ErrorMessage = "Note is required")]
        public string INSPH_NOTE { get; set; }
    }

    public class BundleInspectionApp
    {
        [DisplayName("Bundle Id")]
        public string BUNDLE_ID { get; set; }

        [DisplayName("Bundle Amount")]
        public Nullable<int> AMOUNT { get; set; }

        [DisplayName("Total Asset")]
        public Nullable<int> TOTAL_ASET { get; set; }
    }
    public class BundleInspectionAppD
    {
        [DisplayName("Bundle Id")]
        public string INSPD_NOTE { get; set; }

        [DisplayName("Asset Name")]
        public string ASET_CODE_NAME { get; set; }

        [DisplayName("User Name")]
        public string OWNR_USER { get; set; }

        [DisplayName("Department")]
        public string DEPARTMENT_NAME { get; set; }

        [DisplayName("Area")]
        public string AREA_NAME { get; set; }

        [DisplayName("Inspection By")]
        public string INSPH_IUSER { get; set; }

        [DisplayName("Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> INSPH_DATE { get; set; }

        [DisplayName("Mileage")]
        public string MILEAGE { get; set; }

        [DisplayName("Note")]
        public string INSPH_NOTE { get; set; }

        [DisplayName("Inspection")]
        public string INSPH_ID { get; set; }

        [DisplayName("Item")]
        public string ITEM_NAME { get; set; }

        [DisplayName("Qty")]
        public Nullable<byte> INSPD_IQTY { get; set; }

        [DisplayName("Rate")]
        public Nullable<int> INSPD_IRATE { get; set; }

    }

    public class BundleJobcard
    {
        [Required(ErrorMessage = "Bundle id is required")]
        public string BUNDLE_ID { get; set; }

        [Required(ErrorMessage = "Vendor name is required")]
        public string VENDOR_NAME { get; set; }

        [Required(ErrorMessage = "Vendor note is required")]
        public string VENDOR_NOTE { get; set; }
    }
    public class BundleJobcardAuto
    {
        [Required(ErrorMessage = "Asset id is required")]
        public string ASET_ID { get; set; }

        [Required(ErrorMessage = "Inspection number is required")]
        public string INSP_ID { get; set; }
        [Required(ErrorMessage = "Date is required")]
        public Nullable<System.DateTime> JOB_DATE { get; set; }
    }
    public class BundleJobcardAppD
    {
        [DisplayName("Bundle Id")]
        public string JOBD_NOTE { get; set; }

        [DisplayName("Asset Name")]
        public string ASET_CODE_NAME { get; set; }

        [DisplayName("User Name")]
        public string OWNR_USER { get; set; }

        [DisplayName("Department")]
        public string DEPARTMENT_NAME { get; set; }

        [DisplayName("Area")]
        public string AREA_NAME { get; set; }

        [DisplayName("Vendor")]
        public string JOBH_VENDOR { get; set; }

        [DisplayName("Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> JOBH_DATE { get; set; }

        [DisplayName("Mileage")]
        public string JOBH_MILEAGE { get; set; }

        [DisplayName("Note")]
        public string JOBH_NOTE { get; set; }

        [DisplayName("Job Card")]
        public string JOBH_ID { get; set; }

        [DisplayName("Item")]
        public string ITEM_NAME { get; set; }

        [DisplayName("Qty")]
        public Nullable<byte> JOBD_IQTY { get; set; }

        [DisplayName("Rate")]
        public Nullable<int> JOBD_IRATE { get; set; }

    }


}