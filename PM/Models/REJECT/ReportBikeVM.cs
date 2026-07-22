using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PM.Models
{
    public class ReportBikeVM
    {
        public string AsetCode { get; set; }
        public string AsetNumber { get; set; }
        public string AsetName { get; set; }
        public string AsetModel { get; set; }
        public string AsetType { get; set; }
        public string AsetEngine { get; set; }
        public string AsetChasis { get; set; }
        public Nullable<int> AsetPrice { get; set; }
        public Nullable<System.DateTime> RegDate { get; set; }
        public string FuelType { get; set; }
        public string AsetColor { get; set; }
        public Nullable<short> MGFyear { get; set; }
        public string AsetNote { get; set; }
        public Nullable<bool> Booking { get; set; }
        public string ASET_ASGR { get; set; }
        public string ASET_DEPT { get; set; }
        public string ASET_AREA { get; set; }
        public string ASET_BRND { get; set; }
        public Nullable<bool> ASET_ACTV { get; set; }
    }
}