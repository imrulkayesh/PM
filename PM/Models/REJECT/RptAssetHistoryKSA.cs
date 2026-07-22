using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PM.Models
{
    public partial class RptAssetHistoryKSA
    {
        public string TRNS_TEXT { get; set; }
        public string TRNS_FUSER { get; set; }
        public string TRNS_TUSER { get; set; }
        public string TRNS_CUSER { get; set; }
        public string TRNS_AUSER { get; set; }
        public string TRNS_ASET { get; set; }
        public string ASSET_NAME { get; set; }
        public string TRNS_FDEPT { get; set; }
        public string DEPT_NAME { get; set; }
        public string TRNS_TDEPT { get; set; }
        public string DEPT_NAME1 { get; set; }
        public string TRNS_FDEPO { get; set; }
        public string AREA_NAME { get; set; }
        public string TRNS_TDEPO { get; set; }
        public string AREA_NAME1 { get; set; }
        public DateTime TRNS_DATE { get; set; }
        public string TRNS_NOTE { get; set; }
        public string APPROVE { get; set; }
        public string SUBMITTED { get; set; }
        public string LASTUSER { get; set; }

    }
    public partial class RptAssetDataKSA
    {
        public string aset_id { get; set; }
        public string aset_name { get; set; }
        public  int aset_price { get; set; }
        public Nullable<DateTime> aset_date { get; set; }
        public short  aset_year { get; set; }
        public string aset_dept { get; set; }
        public string aset_owner_id { get; set; }
        public string aset_owner_name { get; set; }
        public string aset_depo_id { get; set; }
        public string aset_depo_name { get; set; }
        public bool  aset_status { get; set; }
        public string  aset_status_type { get; set; }
    }
}