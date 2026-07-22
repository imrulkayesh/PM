using PM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PM.Controllers
{
    public class CompWiseAssetReportController : Controller
    {
        // GET: CompWiseReport
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RPT_VENDINGMACHINE(string DepotId)
        {
            using (pmdbEntities db = new pmdbEntities())
            {
                var GetList = db.VW_RPT_VENDINGMACHINE.Where(d=>d.DEPOT_ID==DepotId).ToList();
                return View(GetList);
            }
            
        }
        public void Export_Data(string DepotId)
        {
            using (pmdbEntities db = new pmdbEntities())
            {
                var GetList = db.VW_RPT_VENDINGMACHINE.Where(d => d.DEPOT_ID == DepotId).ToList();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename="+ DepotId + ".xls");
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            WriteTsv(GetList, Response.Output);
            Response.End();
            }

        }
        public void WriteTsv<T>(IEnumerable<T> data, TextWriter output)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            foreach (PropertyDescriptor prop in props)
            {
                output.Write(prop.DisplayName); // header
                output.Write("\t");
            }
            output.WriteLine();
            foreach (T item in data)
            {
                foreach (PropertyDescriptor prop in props)
                {
                    output.Write(prop.Converter.ConvertToString(
                         prop.GetValue(item)));
                    output.Write("\t");
                }
                output.WriteLine();
            }
        }
    }
}