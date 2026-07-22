using System;
using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class VW_DATE_NOTIF
    {
        [Key]
        public string REMIND_ID { get; set; }
        public string ASET_ID { get; set; }
        public string ASET_TAG { get; set; }
        public string ITEM_ID { get; set; }
        public string ITEM_NAME { get; set; }
        public Nullable<System.DateTime> C_DATE { get; set; }
        public Nullable<System.DateTime> N_DATE { get; set; }
        public Nullable<decimal> ELAPSED { get; set; }
        public string NOTE { get; set; }
        public string REMIND { get; set; }
        public string NOTIF_FIRST { get; set; }
        public string NOTIF_FINAL { get; set; }
        public string REMINDER_NOTE { get; set; }
        public string NOTIF_TYPE { get; set; }
        public string COMPLETED { get; set; }
    }
}