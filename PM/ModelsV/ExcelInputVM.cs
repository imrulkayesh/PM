using System.ComponentModel.DataAnnotations;
using System.Web;

namespace PM.ModelsV
{
    public class ExcelInputVM
    {
        [Required]
        public HttpPostedFileBase MyExcelFile { get; set; }
        public string MSExcelTable { get; set; }
    }

}