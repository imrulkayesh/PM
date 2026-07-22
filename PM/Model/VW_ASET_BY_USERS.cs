using System;

namespace PM.Model
{
    public class VW_ASET_BY_USERS
    {
        public string ASET_ID { get; set; }
        public string ASET_TAG { get; set; }
        public string ASET_CODE_NAME { get; set; }
        public string ASET_DEPARTMENT { get; set; }
        public string ASET_AREA { get; set; }
        public string ASET_GROUP { get; set; }
        public Nullable<bool> ASET_USED { get; set; }
        public Nullable<bool> ACT { get; set; }
        public string UG_USER { get; set; }
    }
}