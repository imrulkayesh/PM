using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PM.Models
{
    public partial class TransferAcceItems
    {
        public string ACCE_ASAC { get; set; }
        public Nullable<byte> ACCE_QTY { get; set; }
        public Nullable<int> ACCE_PRICE { get; set; }
        public string ACCE_NOTE { get; set; }

    }
}