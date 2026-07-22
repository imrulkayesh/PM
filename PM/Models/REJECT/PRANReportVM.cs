using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PM.Models
{
    public class PRANReportVM // vm for XL export
    {
        public string MyGroup { get; set; }
        public string MyBU { get; set; }
        public int AssetType { get; set; }
        public string ASET_BRND { get; set; }
    }
    public class PRANAssetList //Get data assign to this class for XL export
    {
        public string AssetCode { get; set; }
        public string AssetName { get; set; }
        public string AssetModel { get; set; }
        public string PONO { get; set; }
        public string GRNNO { get; set; }
        public Nullable<System.DateTime> RegDate { get; set; }
        public Nullable<short> MfgYear { get; set; }
        public string  Group { get; set; }
        public string Dept { get; set; }
        public string Area { get; set; }
        public string  Brand { get; set; }
        public string Status { get; set; }
    }
    public class PRAN_ASSET_BY_USER_DOWNLOAD
    {
        public string ASET_NAME { get; set; }
        public string ASET_MODL { get; set; }
        public Nullable<short> MFG_YEAR { get; set; }
        public string BOOKING_STATUS { get; set; }
        public string ASGR_NAME { get; set; }
        public string DEPT_NAME { get; set; }
        public string AREA_NAME { get; set; }
        public string BRND_NAME { get; set; }
        public string ASET_STATUS { get; set; }
        public string OWNR_USER { get; set; }
        public string OWNR_NAME { get; set; }
        public string OWNR_CONTACT { get; set; }
        public Nullable<System.DateTime> OWNR_TAKE { get; set; }
        public Nullable<System.DateTime> OWNR_RELASE { get; set; }
        public string USER_STATUS { get; set; }
        public string OWNR_NOTE { get; set; }
    }

}