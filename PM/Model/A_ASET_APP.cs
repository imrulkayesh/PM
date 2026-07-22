using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Model
{
    public class A_ASET_APP
    {
        [Key, ForeignKey("A_ASET_DATA")]
        public string ASET_ID { get; set; }
        public string OWNR_ID { get; set; }
        public Nullable<int> ASET_VALUE { get; set; }
        public Nullable<System.DateTime> APPLY_DATE { get; set; }
        public string APPLY_REASON { get; set; }
        public string APPLY_TYPE { get; set; }
        public Nullable<short> SERVICE_MONTH { get; set; }
        public string RECEIVE_TYPE { get; set; }
        public string RECEIVE_DETAIL { get; set; }
        public Nullable<System.DateTime> RECEIVE_DATE { get; set; }
        public string RECEIVE_AREA { get; set; }
        public string INSURACE_COMP { get; set; }
        public string INSURACE_ID { get; set; }
        public Nullable<int> INSURACE_LINE { get; set; }
        public string REQU_USER { get; set; }
        public string APPR_USER { get; set; }
        public Nullable<System.DateTime> APPR_DATE { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }


        //[ForeignKey("UB_BUSINESS")]
        public virtual A_ASET_DATA A_ASET_DATA { get; set; }
        [ForeignKey("INSURACE_ID")]
        public virtual A_INSUH A_INSUH { get; set; }
        [ForeignKey("OWNR_ID")]
        public virtual A_OWNR A_OWNR { get; set; }

    }
}