using PM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PM.Controllers
{
    public class DotBoardController : Controller
    {
        private pmdbEntities db = new pmdbEntities();
        // GET: DotBoard
        public ActionResult Index()
        {
            var getAll = db.P_ASSET;
            ViewBag.TotalAssetQty = getAll.Count();
            ViewBag.TotalInService = getAll.Where(g => g.ASET_ACTV == true).Count();
            ViewBag.TotalLost= getAll.Where(g => g.ASET_ACTV == false).Count();

            ViewBag.TotalInUse = getAll.Where(g => g.ISBOOKED == true).Count();
            ViewBag.TotalNotInUse = getAll.Where(g => g.ISBOOKED == false).Count();
            return View();
        }
        public ActionResult Index2()
        {
            return View();
        }
    }
}