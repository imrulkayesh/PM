using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PM.ModelsV
{
    public class AssetManagerVM
    {

        [DisplayName("Id")]
        public string ASET_ID { get; set; }
        [DisplayName("Tag")]
        public string ASET_TAG { get; set; }
        [DisplayName("Name")]
        public string ASET_CODE_NAME { get; set; }
        [DisplayName("Area")]
        public string ASET_AREA { get; set; }
        [DisplayName("Body")]
        public string ASET_BODY { get; set; }
        [DisplayName("Brand")]
        public string ASET_BRAND { get; set; }
        [DisplayName("Fuel")]
        public string ASET_FUEL { get; set; }
        [DisplayName("Color")]
        public string ASET_COLOR { get; set; }
        [DisplayName("Group")]
        public string ASET_GROUP { get; set; }
        [DisplayName("Model")]
        public string ASET_MODEL { get; set; }
        [DisplayName("PO/Engine")]
        public string ASET_PO_ENGINE { get; set; }
        [DisplayName("GRN/Chasis")]
        public string ASET_GRN_CHASIS { get; set; }
        [DisplayName("Capacity")]
        public string ASET_CAPACITY { get; set; }
        [DisplayName("Price")]
        public Nullable<int> ASET_PRICE { get; set; }
        [DisplayName("Reg Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> ASET_REG_DATE { get; set; }
        [DisplayName("Height")]
        public string ASET_HEIGHT { get; set; }
        [DisplayName("Weight")]
        public string ASET_WEIGHT { get; set; }

        [DisplayName("MFG Year")]
        public Nullable<short> ASET_MFG_YEAR { get; set; }
        [DisplayName("Image")]
        public string ASET_IMAGE { get; set; }
        [DisplayName("Note")]
        public string ASET_NOTE { get; set; }
        [DisplayName("Booked")]
        public Nullable<bool> ASET_USED { get; set; }
        [DisplayName("Status")]
        public Nullable<bool> ACT { get; set; }

        [DisplayName("Department")]
        public string DEPARTMENT_V { get; set; }

        [DisplayName("Business")]
        public string BUSINESS_V { get; set; }

        [DisplayName("Company")]
        public string COMPANY_V { get; set; }
    }

    public class CarVM
    {

        [DisplayName("Id")]
        public string ASET_ID { get; set; }
        [DisplayName("Tag")]
        public string ASET_TAG { get; set; }
        [DisplayName("Name")]
        public string ASET_CODE_NAME { get; set; }
        [DisplayName("Area")]
        public string ASET_AREA { get; set; }
        [DisplayName("Body")]
        public string ASET_BODY { get; set; }
        [DisplayName("Brand")]
        public string ASET_BRAND { get; set; }
        [DisplayName("Fuel")]
        public string ASET_FUEL { get; set; }
        [DisplayName("Color")]
        public string ASET_COLOR { get; set; }
        [DisplayName("Group")]
        public string ASET_GROUP { get; set; }
        [DisplayName("Model/Inside")]
        public string ASET_MODEL { get; set; }
        [DisplayName("PO/Engine/IMEI/Space")]
        public string ASET_PO_ENGINE { get; set; }
        [DisplayName("GRN/Chasis/Sqm")]
        public string ASET_GRN_CHASIS { get; set; }
        [DisplayName("Capacity/Rent Sqm")]
        public string ASET_CAPACITY { get; set; }
        [DisplayName("Price/Rent")]
        public Nullable<int> ASET_PRICE { get; set; }
        [DisplayName("Reg Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> ASET_REG_DATE { get; set; }
        [DisplayName("Height")]
        public string ASET_HEIGHT { get; set; }
        [DisplayName("Weight")]
        public string ASET_WEIGHT { get; set; }

        [DisplayName("MFG Year")]
        public Nullable<short> ASET_MFG_YEAR { get; set; }
        [DisplayName("Image")]
        public string ASET_IMAGE { get; set; }
        [DisplayName("Note")]
        public string ASET_NOTE { get; set; }
        [DisplayName("Booked")]
        public Nullable<bool> ASET_USED { get; set; }
        [DisplayName("Status")]
        public Nullable<bool> ACT { get; set; }

        [DisplayName("Department")]
        public string DEPARTMENT_V { get; set; }

        [DisplayName("Business/Sponsor")]
        public string BUSINESS_V { get; set; }

        [DisplayName("Company/Owner")]
        public string COMPANY_V { get; set; }
    }

    public class AssetManager_ID
    {
        public string AsetID { get; set; }
    }

    public class AssetPicture
    {
        public string ASET_IMAGE { get; set; }
    }
    public class NewPRANAsset
    {
        [DisplayName("Tag")]
        [Required(ErrorMessage = "Tag name is reaquired")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 50")]
        public string ASET_TAG { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "Asset name is reaquired")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 50")]
        public string ASET_CODE_NAME { get; set; }

        [DisplayName("Department")]
        [Required(ErrorMessage = "Department is reaquired")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 30")]
        public string ASET_DEPARTMENT { get; set; }

        [DisplayName("Area")]
        [Required(ErrorMessage = "Area is reaquired")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 30")]
        public string ASET_AREA { get; set; }

        [DisplayName("Body")]
        [Required(ErrorMessage = "Body is reaquired")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 30")]
        public string ASET_BODY { get; set; }

        [DisplayName("Brand")]
        [Required(ErrorMessage = "Brand is reaquired")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 30")]
        public string ASET_BRAND { get; set; }

        [DisplayName("Fuel")]
        [Required(ErrorMessage = "Fuel is reaquired")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 30")]
        public string ASET_FUEL { get; set; }

        [DisplayName("Color")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 30")]
        [Required(ErrorMessage = "Color is reaquired")]
        public string ASET_COLOR { get; set; }

        [DisplayName("Group")]
        [Required(ErrorMessage = "Group is reaquired")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 30")]
        public string ASET_GROUP { get; set; }

        [DisplayName("Model")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Minimum 2 Maximum 50")]
        public string ASET_MODEL { get; set; }

        [DisplayName("PO Number")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Minimum 2 Maximum 50")]
        public string ASET_PO_ENGINE { get; set; }

        [DisplayName("GRN Number")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Minimum 2 Maximum 50")]
        public string ASET_GRN_CHASIS { get; set; }

        [DisplayName("Capacity")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 10")]
        public string ASET_CAPACITY { get; set; }

        [DisplayName("Price")]
        [Required(ErrorMessage = "Price is reaquired")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Number")]
        [Range(1, 9999999999, ErrorMessage = "Minimum 1 Maximum 9999999999")]
        public Nullable<int> ASET_PRICE { get; set; }

        [DisplayName("Reg Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        //[Range(typeof(DateTime), "01/01/1999", "01/01/2099",ErrorMessage = "Date {0} must be between {1} and {2}")]
        public Nullable<System.DateTime> ASET_REG_DATE { get; set; }

        [DisplayName("MFG Year")]
        [Range(1999, 9999, ErrorMessage = "Minimum 1999 Maximum 9999")]
        public Nullable<short> ASET_MFG_YEAR { get; set; }

        [DisplayName("Note")]
        [StringLength(80, MinimumLength = 5, ErrorMessage = "Minimum 5 Maximum 80")]
        public string ASET_NOTE { get; set; }

    }

    public class NewVehicleData
    {
        [DisplayName("ID")]
        [Required(ErrorMessage = "ID is required")]
        public int ID { get; set; }

        [DisplayName("NAME")]
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 50")]
        public string NAME { get; set; }

        [DisplayName("MODEL")]
        [Required(ErrorMessage = "MODEL name is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 50")]
        public string MODEL { get; set; }
    }

    public class NewCar
    {
        [DisplayName("Tag")]
        [Required(ErrorMessage = "Tag name is reaquired")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 50")]
        public string ASET_TAG { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "Asset name is reaquired")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 50")]
        public string ASET_CODE_NAME { get; set; }

        [DisplayName("Department")]
        [Required(ErrorMessage = "Department is reaquired")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 30")]
        public string ASET_DEPARTMENT { get; set; }

        [DisplayName("Area")]
        [Required(ErrorMessage = "Area is reaquired")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 30")]
        public string ASET_AREA { get; set; }

        [DisplayName("Body")]
        [Required(ErrorMessage = "Body is reaquired")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 30")]
        public string ASET_BODY { get; set; }

        [DisplayName("Brand")]
        [Required(ErrorMessage = "Brand is reaquired")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 30")]
        public string ASET_BRAND { get; set; }

        [DisplayName("Fuel")]
        [Required(ErrorMessage = "Fuel is reaquired")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 30")]
        public string ASET_FUEL { get; set; }

        [DisplayName("Color")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 30")]
        [Required(ErrorMessage = "Color is reaquired")]
        public string ASET_COLOR { get; set; }

        [DisplayName("Group")]
        [Required(ErrorMessage = "Group is reaquired")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 30")]
        public string ASET_GROUP { get; set; }

        [DisplayName("Model")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Minimum 2 Maximum 50")]
        public string ASET_MODEL { get; set; }

        [DisplayName("Engine Number")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Minimum 2 Maximum 50")]
        public string ASET_PO_ENGINE { get; set; }

        [DisplayName("Chasis Number")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Minimum 2 Maximum 50")]
        public string ASET_GRN_CHASIS { get; set; }

        [DisplayName("Capacity")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 10")]
        public string ASET_CAPACITY { get; set; }

        [DisplayName("Price")]
        [Required(ErrorMessage = "Price is reaquired")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Number")]
        [Range(1, 9999999999, ErrorMessage = "Minimum 1 Maximum 9999999999")]
        public Nullable<int> ASET_PRICE { get; set; }

        [DisplayName("Reg Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        //[Range(typeof(DateTime), "01/01/1999", "01/01/2099",ErrorMessage = "Date {0} must be between {1} and {2}")]
        public Nullable<System.DateTime> ASET_REG_DATE { get; set; }

        [DisplayName("MFG Year")]
        [Range(1999, 9999, ErrorMessage = "Minimum 1999 Maximum 9999")]
        public Nullable<short> ASET_MFG_YEAR { get; set; }

        [DisplayName("Note")]
        [StringLength(80, MinimumLength = 5, ErrorMessage = "Minimum 5 Maximum 80")]
        public string ASET_NOTE { get; set; }

    }

    public class CarEdit
    {
        [DisplayName("Id")]
        public string ASET_ID { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "Asset name is reaquired")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 50")]
        public string ASET_CODE_NAME { get; set; }

        [DisplayName("Business")]
        public string ASET_BUSINESS { get; set; }

        [DisplayName("Department")]
        [Required(ErrorMessage = "Department is reaquired")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 30")]
        public string ASET_DEPARTMENT { get; set; }

        [DisplayName("Area")]
        [Required(ErrorMessage = "Area is reaquired")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 30")]
        public string ASET_AREA { get; set; }

        [DisplayName("Note")]
        [StringLength(80, MinimumLength = 5, ErrorMessage = "Minimum 5 Maximum 80")]
        public string ASET_NOTE { get; set; }

        [DisplayName("Asset HEIGHT")]
        public string ASET_HEIGHT { get; set; }
    }



    public class AccesoreisApprove
    {
        [DisplayName("Item")]
        [Required(ErrorMessage = "Item is reaquired")]
        public string ACCE_IDA { get; set; }

        [DisplayName("Asset")]
        [Required(ErrorMessage = "Asset is reaquired")]
        public string ASET_IDA { get; set; }
    }

    public class AccesoreisDelete
    {
        [DisplayName("Item")]
        [Required(ErrorMessage = "Item is reaquired")]
        public string ACCE_IDD { get; set; }

        [DisplayName("Asset")]
        [Required(ErrorMessage = "Asset is reaquired")]
        public string ASET_IDD { get; set; }
    }


    public class NewDismiss
    {

        [Required(ErrorMessage = "Asset is required")]
        public string ASET_ID { get; set; }

        [Required(ErrorMessage = "Area is required")]
        public string RECEIVE_AREA { get; set; }

        [Required(ErrorMessage = "Application Type is required")]
        public string APPLY_TYPE { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime APPLY_DATE { get; set; }

        [Required(ErrorMessage = "Reason is required")]
        [StringLength(150, MinimumLength = 5, ErrorMessage = "Max 150 char and Min 5 char")]
        public string APPLY_REASON { get; set; }

        [Required(ErrorMessage = "Claim Type is required")]
        public string IsIC { get; set; }

    }
    public class DismissInsurance
    {
        [Required(ErrorMessage = "Status is required")]
        public bool Success { get; set; }
        [Required(ErrorMessage = "Id is required")]
        public string INSU_ID { get; set; }
        [Required(ErrorMessage = "Comp is required")]
        public string INSU_COMP { get; set; }
        [Required(ErrorMessage = "Line is required")]
        public int INSU_LINE { get; set; }

    }
    public class UpdateDismiss
    {
        [Required(ErrorMessage = "Asset is required")]
        public string ASET_ID { get; set; }
        [Required(ErrorMessage = "Value is required")]
        public int ASET_VALUE { get; set; }
        [Required(ErrorMessage = "Type is required")]
        public string RECEIVE_TYPE { get; set; }
        [Required(ErrorMessage = "Detail is required")]
        public string RECEIVE_DETAIL { get; set; }
        [Required(ErrorMessage = "Date is required")]
        public System.DateTime RECEIVE_DATE { get; set; }
    }
    public class DismissList
    {
        [DisplayName("Asset Id")]
        public string ASET_ID { get; set; }

        [DisplayName("Asset Name")]
        public string ASET_CODE_NAME { get; set; }

        [DisplayName("Service Month")]
        public Nullable<short> SERVICE_MONTH { get; set; }

        [DisplayName("Asset Value")]
        public Nullable<int> ASET_VALUE { get; set; }

        [DisplayName("Asset User")]
        public string ASET_USER { get; set; }

        [DisplayName("Application Type")]
        public string APPLY_TYPE { get; set; }

        [DisplayName("Application Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> APPLY_DATE { get; set; }

        [DisplayName("Application Reason")]
        public string APPLY_REASON { get; set; }

        [DisplayName("Receive Type")]
        public string RECEIVE_TYPE { get; set; }

        [DisplayName("Receive Detail")]
        public string RECEIVE_DETAIL { get; set; }

        [DisplayName("Receive Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> RECEIVE_DATE { get; set; }

        [DisplayName("Aset Area")]
        public string ASET_AREA { get; set; }

        [DisplayName("Insurance Id")]
        public string INSURANCE_ID { get; set; }

        [DisplayName("Insurance Line")]
        public Nullable<int> INSURANCE_LINE { get; set; }

        [DisplayName("Insurance Value")]
        public Nullable<int> INSU_ASET_VALUE { get; set; }

        [DisplayName("Insurance Policy")]
        public string INSU_POLICY { get; set; }

        [DisplayName("Insurance Company")]
        public string INSURANCE_COMP { get; set; }

        [DisplayName("Apply User")]
        public string APPLY_USER { get; set; }
        [DisplayName("Approve User")]
        public string APPROVE_USER { get; set; }

        [DisplayName("Approve Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> APPROVE_DATE { get; set; }

        [DisplayName("Status")]
        public Nullable<bool> DISMISS_STATUS { get; set; }

    }

    public class FridgeExcel
    {

        [Required(ErrorMessage = "Asset name is reaquired")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 50")]
        public string ASET_CODE_NAME { get; set; }

        [Required(ErrorMessage = "Department is reaquired")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 30")]
        public string ASET_DEPARTMENT { get; set; }

        [Required(ErrorMessage = "Area is reaquired")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 30")]
        public string ASET_AREA { get; set; }

        [Required(ErrorMessage = "Brand is reaquired")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 30")]
        public string ASET_BRAND { get; set; }

        [Required(ErrorMessage = "Group is reaquired")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 30")]
        public string ASET_GROUP { get; set; }

        [Required(ErrorMessage = "Model is reaquired")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Minimum 2 Maximum 50")]
        public string ASET_MODEL { get; set; }

        [Required(ErrorMessage = "PO is reaquired")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Minimum 2 Maximum 50")]
        public string ASET_PO_ENGINE { get; set; }

        [Required(ErrorMessage = "GRN is reaquired")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Minimum 2 Maximum 50")]
        public string ASET_GRN_CHASIS { get; set; }

        [Required(ErrorMessage = "Capacity is reaquired")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 10")]
        public string ASET_CAPACITY { get; set; }

        [Required(ErrorMessage = "Price is reaquired")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Number")]
        [Range(1, 9999999999, ErrorMessage = "Minimum 1 Maximum 9999999999")]
        public Nullable<int> ASET_PRICE { get; set; }

        [Required(ErrorMessage = "Reg Date is reaquired")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> ASET_REG_DATE { get; set; }

        [Required(ErrorMessage = "Year is reaquired")]
        [Range(1999, 9999, ErrorMessage = "Minimum 1999 Maximum 9999")]
        public Nullable<short> ASET_MFG_YEAR { get; set; }

        //[Required(ErrorMessage = "Note is reaquired")]
        [StringLength(80, MinimumLength = 5, ErrorMessage = "Minimum 5 Maximum 80")]
        public string ASET_NOTE { get; set; }

    }

    public class NewMobilePhone
    {

        [DisplayName("Tag")]
        [Required(ErrorMessage = "Tag name is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 50")]
        public string ASET_TAG { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "Asset name is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 50")]
        public string ASET_CODE_NAME { get; set; }

        [DisplayName("Department")]
        [Required(ErrorMessage = "Department is required")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 30")]
        public string ASET_DEPARTMENT { get; set; }

        [DisplayName("Area")]
        [Required(ErrorMessage = "Area is required")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 30")]
        public string ASET_AREA { get; set; }

        [DisplayName("Body")]
        [Required(ErrorMessage = "Body is required")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 30")]
        public string ASET_BODY { get; set; }

        [DisplayName("Brand")]
        [Required(ErrorMessage = "Brand is raquired")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 30")]
        public string ASET_BRAND { get; set; }

        [DisplayName("Color")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 30")]
        [Required(ErrorMessage = "Color is required")]
        public string ASET_COLOR { get; set; }

        [DisplayName("Group")]
        [Required(ErrorMessage = "Group is required")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 30")]
        public string ASET_GROUP { get; set; }

        [DisplayName("Model")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Minimum 2 Maximum 50")]
        public string ASET_MODEL { get; set; }

        [DisplayName("IMEI 1 Number")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Minimum 2 Maximum 50")]
        [Required(ErrorMessage = "IEMI number is required")]
        public string ASET_PO_ENGINE { get; set; }

        [DisplayName("IMEI 2 Number")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Minimum 2 Maximum 50")]
        public string ASET_GRN_CHASIS { get; set; }


        [DisplayName("Price")]
        [Required(ErrorMessage = "Price is required")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Number")]
        [Range(1, 9999999999, ErrorMessage = "Minimum 1 Maximum 9999999999")]
        public Nullable<int> ASET_PRICE { get; set; }

        [DisplayName("Reg Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        //[Range(typeof(DateTime), "01/01/1999", "01/01/2099",ErrorMessage = "Date {0} must be between {1} and {2}")]
        public Nullable<System.DateTime> ASET_REG_DATE { get; set; }

        [DisplayName("Note")]
        [StringLength(80, MinimumLength = 5, ErrorMessage = "Minimum 5 Maximum 80")]
        public string ASET_NOTE { get; set; }
    }

    public class NewDepot
    {
        [DisplayName("Tag")]
        [Required(ErrorMessage = "Tag name is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 50")]
        public string ASET_TAG { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "Asset name is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 50")]
        public string ASET_CODE_NAME { get; set; }

        [DisplayName("Department")]
        [Required(ErrorMessage = "Department is required")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 30")]
        public string ASET_DEPARTMENT { get; set; }

        [DisplayName("Area")]
        [Required(ErrorMessage = "Area is required")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 30")]
        public string ASET_AREA { get; set; }

        [DisplayName("Group")]
        [Required(ErrorMessage = "Group is required")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Minimum 1 Maximum 30")]
        public string ASET_GROUP { get; set; }

        [DisplayName("Inside Square Meters")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Minimum 2 Maximum 50")]
        public string ASET_MODEL { get; set; }

        [DisplayName("Depo Space")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Minimum 2 Maximum 50")]
        [Required(ErrorMessage = "Depo Space is required")]
        public string ASET_PO_ENGINE { get; set; }

        [DisplayName("Total Square Meters")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Minimum 2 Maximum 50")]
        public string ASET_GRN_CHASIS { get; set; }

        [DisplayName("Per Square Meters Rent")]
        public string ASET_CAPACITY { get; set; }

        [DisplayName("Price")]
        [Required(ErrorMessage = "Price is required")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Number")]
        [Range(1, 9999999999, ErrorMessage = "Minimum 1 Maximum 9999999999")]
        public Nullable<int> ASET_PRICE { get; set; }

        [DisplayName("Reg Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        //[Range(typeof(DateTime), "01/01/1999", "01/01/2099",ErrorMessage = "Date {0} must be between {1} and {2}")]
        public Nullable<System.DateTime> ASET_REG_DATE { get; set; }

        [DisplayName("Note")]
        [StringLength(80, MinimumLength = 5, ErrorMessage = "Minimum 5 Maximum 80")]
        public string ASET_NOTE { get; set; }
    }

}