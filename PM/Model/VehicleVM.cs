using System;
using System.ComponentModel.DataAnnotations;

namespace PM.ModelsV
{
    public class VehicleVM
    {
        [Key]
        public int ID { get; set; }
        public string NAME { get; set; }
        public string MODEL { get; set; }
        public DateTime CREATEDATE { get; set; }

    }
}