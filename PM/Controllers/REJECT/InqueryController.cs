using PM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PM.Controllers
{
    public class InqueryController : Controller
    {
        private pmdbEntities db = new pmdbEntities();
        public ActionResult Index()
        {
            var Comp = GetCompany();
            var Beun = GetBeun();
            var Dept = GetDept();
            var Bran = GetBrand();

            ShowCompanyBEUNdept myModel = new ShowCompanyBEUNdept();
            myModel.Companiess = Comp;
            myModel.BusinessUnits = Beun;
            myModel.Departments = Dept;
            myModel.Brands = Bran;

            return View(myModel);
        }
        
        public List<P_COMP> GetCompany()
        {
            var Comp = db.P_COMP.ToList();
            return Comp;
        }
        public List<P_BEUN> GetBeun()
        {
            var BEUN = db.P_BEUN.ToList();
            return BEUN;
        }
        public List<P_DEPT> GetDept()
        {
            var Dept = db.P_DEPT.ToList();
            return Dept;
        }
        public List<P_BRAND> GetBrand()
        {
            var Brand = db.P_BRAND.ToList();
            return Brand;
        }
    }
}
