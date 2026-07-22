using System;
using System.ComponentModel.DataAnnotations;

namespace PM.Model
{
    public class VW_PMENU_CMENU
    {
        [Key]
        public int PMNU_TEXT { get; set; }
        public string PMNU_NAME { get; set; }
        public int CMNU_TEXT { get; set; }
        public string CMNU_NAME { get; set; }
        public Nullable<int> CMNU_ROLE { get; set; }
        public string CMNU_NOTE { get; set; }
    }
}