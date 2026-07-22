using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Model
{
    public class A_ITEM_LIST
    {



        public A_ITEM_LIST()
        {
            this.A_INSPD = new HashSet<A_INSPD>();
            this.A_JOBD = new HashSet<A_JOBD>();
            this.A_REMINDER = new HashSet<A_REMINDER>();
        }



        [Key]
        public string ITEM_ID { get; set; }
        public string ITEM_NAME { get; set; }
        public string ITEM_DESCRIPTION { get; set; }
        public string ITEM_NOTIF_SET { get; set; }
        public string ITEM_BASE { get; set; }
        public string ITEM_BILL { get; set; }
        public string REMINDER_ENTRY { get; set; }
        public string JOB_ENTRY { get; set; }
        public string ITEM_GROUP { get; set; }

        [ForeignKey("ITEM_GROUP")]
        public virtual A_GROUP A_GROUP { get; set; }

        public virtual ICollection<A_INSPD> A_INSPD { get; set; }

        public virtual ICollection<A_JOBD> A_JOBD { get; set; }

        public virtual ICollection<A_REMINDER> A_REMINDER { get; set; }
    }
}