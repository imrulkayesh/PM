using System;
using System.Collections.Generic;

namespace PM.ModelsV
{
    public class Inventory
    {
    }
    public class NewConsumption
    {
        public string TRNH_REF { get; set; }
        public string TRNH_FMWH { get; set; }
        public string TRNH_NOTE { get; set; }
        public Nullable<System.DateTime> TRNH_DATE { get; set; }
        public List<NewConsumptionD> newConsumptionD { get; set; }
    }
    public class NewConsumptionD
    {
        public string TRND_ITEM { get; set; }
        public short TRND_QNTY { get; set; }
        public string TRND_NOTE { get; set; }
    }
}