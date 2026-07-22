using System;

namespace PM.Model
{
    public class VW_RPT_JOBD_S1
    {
        public string JOBH_ID { get; set; }
        public byte JOBD_LINE { get; set; }
        public string JOBD_ITEM { get; set; }
        public string ITEM_NAME { get; set; }
        public Nullable<byte> JOBD_IQTY { get; set; }
        public Nullable<int> JOBD_IRATE { get; set; }
        public Nullable<decimal> JOBD_IAMNT { get; set; }
        public Nullable<byte> JOBD_JQTY { get; set; }
        public Nullable<int> JOBD_JRATE { get; set; }
        public Nullable<decimal> JOBD_JAMNT { get; set; }
        public string JOBD_NOTE { get; set; }
    }
}