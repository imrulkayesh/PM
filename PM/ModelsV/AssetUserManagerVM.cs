using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PM.ModelsV
{
    public class AssetUserManagerVM
    {
        [DisplayName("Transfer Id")]
        public string AU_ID { get; set; }

        [DisplayName("Asset Id")]
        public string AU_ASET { get; set; }

        [DisplayName("User")]
        public string AU_USER_U { get; set; }

        [DisplayName("User Id")]
        public string AU_USER { get; set; }

        [DisplayName("Working Dept.")]
        public string AU_DEPT { get; set; }

        [DisplayName("Working Area")]
        public string AU_AREA { get; set; }

        [DisplayName("Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> AU_SRDATE { get; set; }

        [DisplayName("Requested By")]
        public string AU_SRUSER { get; set; }


        [DisplayName("App Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> AU_SADATE { get; set; }

        [DisplayName("App By")]
        public string AU_SAUSER { get; set; }

        [DisplayName("Start Note")]
        public string AU_SNOTE { get; set; }


        [DisplayName("End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> AU_ERDATE { get; set; }

        [DisplayName("Requested By")]
        public string AU_ERUSER { get; set; }


        [DisplayName("App Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> AU_EADATE { get; set; }

        [DisplayName("App By")]
        public string AU_EAUSER { get; set; }

        [DisplayName("App Note")]
        public string AU_ENOTE { get; set; }

        [DisplayName("Tansfer Type")]
        public string AU_TRNSTYPE { get; set; }
    }
    public class StartNew
    {
        public string AU_ID { get; set; }

        [DisplayName("Asset Id")]
        [Required(ErrorMessage = "Asset is reaquired")]
        public string F_ASET_ID { get; set; }
        //public string AU_ASET { get; set; } 

        [DisplayName("User Id")]
        [Required(ErrorMessage = "User is reaquired")]
        public string U_ASET_USER { get; set; }
        //public string AU_USER { get; set; }

        [DisplayName("Department")]
        [Required(ErrorMessage = "Department is reaquired")]
        public string T_ASET_DEPARTMENT { get; set; }
        // public string AU_DEPT { get; set; }

        [DisplayName("Area Id")]
        [Required(ErrorMessage = "Area is reaquired")]
        public string T_ASET_AREA { get; set; }
        //public string AU_AREA { get; set; }

        [DisplayName("Start Date")]
        public Nullable<System.DateTime> AU_SRDATE { get; set; }

        [DisplayName("App User")]
        public string AU_SRUSER { get; set; }

        [DisplayName("Note")]
        [Required(ErrorMessage = "Comment is reaquired")]
        public string ASET_NOTE { get; set; }
        //public string AU_SNOTE { get; set; }

        [DisplayName("Mileage")]
        [Required(ErrorMessage = "Mileage is reaquired")]
        public string AU_SMILEAGE { get; set; }

        [DisplayName("Transfer Type")]
        [Required(ErrorMessage = "Transfer Type is reaquired")]
        public string AU_TRNSTYPE { get; set; }

    }
    public class StartApp
    {
        public string AU_ID { get; set; }
        public Nullable<System.DateTime> AU_SADATE { get; set; }
        public string AU_SIAPPR { get; set; }
        public string AU_SNOTE { get; set; }
    }
    public class EndNew
    {
        [Required(ErrorMessage = "Id is reaquired")]
        public string AU_ID { get; set; }

        [Required(ErrorMessage = "Date is reaquired")]
        public Nullable<System.DateTime> AU_ERDATE { get; set; }
        public string AU_ERUSER { get; set; }

        [DisplayName("Reason")]
        [Required(ErrorMessage = "Reason is reaquired")]
        public string AU_ENOTE { get; set; }

        [DisplayName("Mileage")]
        [Required(ErrorMessage = "Mileage is reaquired")]
        public string AU_EMAILAGE { get; set; }
    }
    public class EndApp
    {
        public string AU_ID { get; set; }
        public Nullable<System.DateTime> AU_EADATE { get; set; }
        public string AU_EIAPPR { get; set; }
        public string AU_ENOTE { get; set; }
    }

    public class HandOverFormPrint
    {

        public string MILEAGE { get; set; }
        public string H_USER { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<DateTime> HO_DATE { get; set; }
        public string NOTES { get; set; }


        public string O_COMP { get; set; }
        public string O_BUSINESS { get; set; }
        public string O_DEPARTMENT { get; set; }
        public string O_AREA { get; set; }


        public string ASET_TAG { get; set; }
        public string ASET_NAME { get; set; }
        public string A_COMP { get; set; }
        public string A_BUSINESS { get; set; }
        public string A_DEPARTMENT { get; set; }
        public string A_AREA { get; set; }

        public string T_STAFF_ID { get; set; }
        public string T_STAFF_NAME { get; set; }
        public string T_IQAMA_NO { get; set; }
        public string T_COMP { get; set; }
        public string T_BUSINESS { get; set; }
        public string T_DEPARTMENT { get; set; }
        public string T_AREA { get; set; }



        public List<HOFPItems> HOFP_ITEMS { get; set; }
    }

    public class HOFPItems
    {
        public int SL_NO { get; set; }
        public string ITEM_ID { get; set; }
        public string ITEM_NAME { get; set; }
        public Nullable<byte> ITEM_QTY { get; set; }
        public string ITEM_NOTE { get; set; }
    }



    public class FridgeOutletExcel
    {
        [Required(ErrorMessage = "Fridge Code is reaquired")]
        public string FRIDGE_CODE { get; set; }

        [Required(ErrorMessage = "Outlet Code is reaquired")]
        public string OUTLET_CODE { get; set; }

        [Required(ErrorMessage = "Transfer Date is reaquired")]
        public DateTime TRANSFER_DATE { get; set; }

        [Required(ErrorMessage = "Transfer Note is reaquired")]
        public string NOTE { get; set; }

        [Required(ErrorMessage = "Transfer Type is reaquired")]
        public string TRANSFER_TYPE { get; set; }
    }


    public class AssetUserAssignQuick
    {
        [Required]
        public string AU_ASET { get; set; }
        [Required]
        public string AU_USER { get; set; }
        public DateTime AU_SRDATE { get; set; }
        [Required]
        public string AU_ENOTE { get; set; }
    }
    public class AssetUserAssignQuick_Selection
    {
        public string ownr_id { get; set; }
        public string ownr_dept { get; set; }
        public string ownr_area { get; set; }
    }
}