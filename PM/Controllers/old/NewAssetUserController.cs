using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PM.Controllers
{
    public class NewAssetUserController : Controller
    {
        // GET: NewAssetUser
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadAsset(string AssetId)

        {
            if (Session["UserId"] != null)
            {

                string Dept = Session["UserDept"].ToString();
                string Comp = Session["UserComp"].ToString();

                return View();
            }
            else
            {
                return RedirectToRoute("Logout","Home");

            }
        }
    }
}