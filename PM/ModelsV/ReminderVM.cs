using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace PM.ModelsV
{
    public class ReminderVM
    {
        [DisplayName("Reminder Id")]
        public string REMIND_ID { get; set; }

        [DisplayName("Asset Id")]
        public string ASET_ID { get; set; }

        [DisplayName("Asset Tag")]
        public string ASET_TAG { get; set; }

        [DisplayName("Item Id")]
        public string ITEM_ID { get; set; }

        [DisplayName("Item Name")]
        public string ITEM_NAME { get; set; }

        [DisplayName("Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> C_DATE { get; set; }

        [DisplayName("End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> N_DATE { get; set; }

        [DisplayName("Elapesd Day")]
        public Nullable<decimal> ELAPSED { get; set; }

        [DisplayName("Remind")]
        public string REMIND { get; set; }

        [DisplayName("First Remind")]
        public string NOTIF_FIRST { get; set; }

        [DisplayName("Final Remind")]
        public string NOTIF_FINAL { get; set; }

        [DisplayName("Messages")]
        public string REMINDER_NOTE { get; set; }

        [DisplayName("Remind Type")]
        public string NOTIF_TYPE { get; set; }

        [DisplayName("Note")]
        public string NOTE { get; set; }

        [DisplayName("Status")]
        public string COMPLETED { get; set; }


    }

    public class newReminderVM
    {
        [DisplayName("Asset Group")]
        [Required(ErrorMessage = "Asset Group is reaquired")]
        public string ASET_GROUP { get; set; }

        [DisplayName("Asset")]
        [Required(ErrorMessage = "Asset is reaquired")]
        public string ASET_ID { get; set; }

        [DisplayName("Item")]
        [Required(ErrorMessage = "Item is reaquired")]
        public string ITEM_ID { get; set; }

        [DisplayName("Current State")]
        public string C_STATE { get; set; }


        [DisplayName("Expire State")]
        public string N_STATE { get; set; }


        [DisplayName("Current Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> C_DATE { get; set; }


        [DisplayName("Expire Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> N_DATE { get; set; }

        [DisplayName("Note")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Minimum 2 Maximum 100")]
        public string NOTE { get; set; }

        [DisplayName("File")]
        //[Required(ErrorMessage = "Choose an file"),FileExtensions(Extensions = "jpg,png,pdf,docx", ErrorMessage = "Error in file")]
        public HttpPostedFileBase FileX { get; set; }
    }

    public class UpdateReminder
    {
        [DisplayName("Reminder Id")]
        [Required(ErrorMessage = "Id reaquired")]
        public string REMIND_ID { get; set; }

        [DisplayName("Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Required(ErrorMessage = "Start Date is reaquired")]
        public Nullable<System.DateTime> C_DATE { get; set; }

        [DisplayName("End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Required(ErrorMessage = "End Date is reaquired")]
        public Nullable<System.DateTime> N_DATE { get; set; }
    }
    public class DeleteReminder
    {
        [DisplayName("Reminder Id")]
        [Required(ErrorMessage = "Id reaquired")]
        public string REMIND_DEL { get; set; }
    }
    public class CompleteReminder
    {
        [DisplayName("Reminder Id")]
        [Required(ErrorMessage = "Id reaquired")]
        public string REMIND_COM { get; set; }

    }

}