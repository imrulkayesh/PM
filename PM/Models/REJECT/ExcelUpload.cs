using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PM.Models
{
    public class ExcelUpload
    {
        [Required]
        public HttpPostedFileBase MyExcelFile { get; set; }
        public string MSExcelTable { get; set; }
    }
    public class PRANAssetExcel
    {
        public string ASET_TEXT { get; set; }
        public string ASET_NAME { get; set; }
        public string ASET_MODL { get; set; }
        public string ASET_TYPE { get; set; }
        public string ENGN_PONO { get; set; }
        public string CHSS_GRN { get; set; }
        public string ASET_CPCT { get; set; }
        public Nullable<int> ASET_PRIC { get; set; }
        public Nullable<System.DateTime> REG_DATE { get; set; }
        // public Nullable<int> REG_DATE { get; set; }
        public string ASET_IMAG { get; set; }
        public string FUEL_TYPE { get; set; }
        public string BODY_TYPE { get; set; }
        public string ASET_HIGT { get; set; }
        public string ASET_WIGT { get; set; }
        public string ASET_COLR { get; set; }
        public Nullable<short> MFG_YEAR { get; set; }
        public string ASET_NOTE { get; set; }
        public Nullable<bool> ISBOOKED { get; set; }
        public string ASET_ASGR { get; set; }
        public string ASET_DEPT { get; set; }
        public string ASET_AREA { get; set; }
        public string ASET_BRND { get; set; }
        public Nullable<bool> ASET_ACTV { get; set; }
        public virtual P_ASGR P_ASGR { get; set; }
        public virtual P_AREA P_AREA { get; set; }
        public virtual P_BRAND P_BRAND { get; set; }
        public virtual P_DEPT P_DEPT { get; set; }

    }
    public class BikeExpenses
    {
        public string ASET_OID { get; set; }
        public string EXPH_TEXT { get; set; }
        public long EXPD_AMNT { get; set; }
        public System.DateTime EXPD_DATE { get; set; }
        public string EXPD_NOTE { get; set; }
    }
    public class InsuranceExpenses{
        public string INSC_ASET { get; set; }
        public string INSC_PLCY { get; set; }
        public System.DateTime INSC_RDAT { get; set; }
        public decimal INSC_PREM { get; set; }
        public string INSC_NOTE { get; set; }
    }

}