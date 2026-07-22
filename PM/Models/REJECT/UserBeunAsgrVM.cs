using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PM.Models
{
    public partial class UserBeunAsgrVM
    {
    }
    public partial class BEUNVM
    {
        public string COMP_NAME { get; set; }
        public string BEUN_USER { get; set; }
        public string BEUN_TEXT { get; set; }
        public string BEUN_NAME { get; set; }
        public string USER_TEXT { get; set; }
        public Nullable<bool> UBP_ACTV { get; set; }
    }

    public partial class ASGRVM
    {
        public string ASGR_USER { get; set; }
        public string ASGR_TEXT { get; set; }
        public string ASGR_NAME { get; set; }
        public string USER_TEXT { get; set; }
        public Nullable<bool> UAP_ACTV { get; set; }
    }
    
    public partial class ShowCompanyBEUNdept
    {
        public List<P_COMP> Companiess { get; set; }
        public List<P_BEUN> BusinessUnits { get; set; }
        public List<P_DEPT> Departments { get; set; }
        public List<P_BRAND> Brands { get; set; }
    }
}