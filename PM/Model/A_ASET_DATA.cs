using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Model
{
    public class A_ASET_DATA
    {
        public A_ASET_DATA()
        {
            this.A_ASET_ACCE = new HashSet<A_ASET_ACCE>();
            this.A_ASET_USER = new HashSet<A_ASET_USER>();
            this.A_OWNR_BILL = new HashSet<A_OWNR_BILL>();
            this.A_INSPH = new HashSet<A_INSPH>();
            this.A_INSUH = new HashSet<A_INSUH>();
            this.A_PLEA = new HashSet<A_PLEA>();
            this.A_REMINDER = new HashSet<A_REMINDER>();
        }

        [Key]
        public string ASET_ID { get; set; }
        public string ASET_TAG { get; set; }
        public string ASET_CODE_NAME { get; set; }
        public string ASET_DEPARTMENT { get; set; }
        public string ASET_AREA { get; set; }
        public string ASET_BODY { get; set; }
        public string ASET_BRAND { get; set; }
        public string ASET_FUEL { get; set; }
        public string ASET_COLOR { get; set; }
        public string ASET_GROUP { get; set; }
        public string ASET_MODEL { get; set; }
        public string ASET_PO_ENGINE { get; set; }
        public string ASET_GRN_CHASIS { get; set; }
        public string ASET_CAPACITY { get; set; }
        public Nullable<int> ASET_PRICE { get; set; }
        public Nullable<System.DateTime> ASET_REG_DATE { get; set; }
        public string ASET_HEIGHT { get; set; }
        public string ASET_WEIGHT { get; set; }
        public Nullable<short> ASET_MFG_YEAR { get; set; }
        public string ASET_IMAGE { get; set; }
        public string ASET_NOTE { get; set; }
        public Nullable<bool> ASET_USED { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }


        [ForeignKey("ASET_AREA")]
        public virtual A_AREA A_AREA { get; set; }
        public virtual ICollection<A_ASET_ACCE> A_ASET_ACCE { get; set; }
        [ForeignKey("ASET_ID")]
        public virtual A_ASET_APP A_ASET_APP { get; set; }
        public virtual ICollection<A_ASET_USER> A_ASET_USER { get; set; }
        public virtual ICollection<A_OWNR_BILL> A_OWNR_BILL { get; set; }
        public virtual ICollection<A_INSPH> A_INSPH { get; set; }
        public virtual ICollection<A_INSUH> A_INSUH { get; set; }
        [ForeignKey("ASET_BODY")]
        public virtual A_BODY A_BODY { get; set; }
        [ForeignKey("ASET_BRAND")]
        public virtual A_BRAND A_BRAND { get; set; }
        [ForeignKey("ASET_COLOR")]
        public virtual A_COLOR A_COLOR { get; set; }
        [ForeignKey("ASET_DEPARTMENT")]
        public virtual A_DEPARTMENT A_DEPARTMENT { get; set; }
        [ForeignKey("ASET_FUEL")]
        public virtual A_FUEL A_FUEL { get; set; }
        [ForeignKey("ASET_GROUP")]
        public virtual A_GROUP A_GROUP { get; set; }
        public virtual ICollection<A_PLEA> A_PLEA { get; set; }
        public virtual ICollection<A_REMINDER> A_REMINDER { get; set; }
    }
}