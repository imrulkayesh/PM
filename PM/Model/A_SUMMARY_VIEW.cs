using System;
using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class A_SUMMARY_VIEW
    {
        public string SV_ID { get; set; }
        [Key]
        public string SV_TABLE { get; set; }
        public string SV_NOTE { get; set; }
        public string SV_USER { get; set; }
        public Nullable<int> DATA_A { get; set; }
        public string DATA_AN { get; set; }
        public Nullable<int> DATA_B { get; set; }
        public string DATA_BN { get; set; }
        public Nullable<int> DATA_C { get; set; }
        public string DATA_CN { get; set; }
        public Nullable<int> DATA_D { get; set; }
        public string DATA_DN { get; set; }
        public Nullable<int> DATA_E { get; set; }
        public string DATA_EN { get; set; }
        public Nullable<int> DATA_F { get; set; }
        public string DATA_FN { get; set; }
        public Nullable<int> DATA_G { get; set; }
        public string DATA_GN { get; set; }
        public Nullable<int> DATA_H { get; set; }
        public string DATA_HN { get; set; }
        public Nullable<int> DATA_I { get; set; }
        public string DATA_IN { get; set; }
        public Nullable<int> DATA_J { get; set; }
        public string DATA_JN { get; set; }
        public Nullable<bool> ACT { get; set; }
        public Nullable<System.DateTime> CDT { get; set; }
        public Nullable<System.DateTime> UDT { get; set; }
        public string CDU { get; set; }
        public string UDU { get; set; }
        public Nullable<int> VAR { get; set; }
    }
}