using PM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PM.Controllers
{
    public class Inquery2Controller : Controller
    {
        private pmdbEntities db = new pmdbEntities();
        // GET: Inquery2
        public ActionResult Index()
        {
            var AllBUnDept = db.P_DEPT.ToList();
            return View(AllBUnDept);
        }
    }
}