using PM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PM.Controllers
{
    public class DistThanaAreaController : Controller
    {
        private pmdbEntities db = new pmdbEntities();
        // GET: DistThanaArea
        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {
                var AllEntry = db.P_AREA.ToList();
                return View(AllEntry);
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Time Expired, please login!');window.location.href = '/Home/Login';</script>");
            }
            
        }
    }
}