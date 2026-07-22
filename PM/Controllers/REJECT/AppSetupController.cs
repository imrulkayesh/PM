using PM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PM.Controllers
{
    public class AppSetupController : Controller
    {
        private pmdbEntities db = new pmdbEntities();
        // GET: AppSetup
        public ActionResult Index()
        {
            return View();
        }
        #region Document_Type
        public ActionResult DocumentsType()
        {
            var DocType = db.P_DOC_TYPE.ToList();
            return View(DocType);
        }
        #endregion

    }
}