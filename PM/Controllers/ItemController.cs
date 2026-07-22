using PM.BLL;
using PM.Model;
using PM.ModelsV;
using PM.MyAuth;
using System;
using System.Linq;
using System.Web.Mvc;

namespace PM.Controllers
{
    [Authorize]
    [CustomHttpReffer]
    public class ItemController : Controller
    {
        private PMEntities db = new PMEntities();
        ReturnPermission RP = new ReturnPermission();
        GE Sequence = new GE();

        [KAuth(2005)]
        public ActionResult Index()
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            string sql_1 = @"SELECT a.ITEM_ID,
                               a.ITEM_NAME,
                               a.ITEM_CODE,
                               a.LICENSE_NAME_ID,
                               b.LICENSE_NAME,
                               a.ITEM_BRAND,
                               a.ITEM_FLAVOUR
                          FROM A_ITEM_MASTER  a
                               LEFT OUTER JOIN LIC_LICENSE_NAME b ON a.LICENSE_NAME_ID = b.LICENSE_NAME_ID
                         WHERE a.ACT = 1";
            var ItemData = db.Database.SqlQuery<ItemVM>(sql: sql_1);
            ViewBag.Message = "Item List";
            var R = ItemData.Take(500).ToList();
            return View(R);
        }

        [KAuth(2005)]
        [HttpGet]
        public ActionResult NewItemMaster()
        {
            ViewBag.LICENSE_NAME_ID = new SelectList(RP.LicenseTypeList(), "LICENSE_NAME_ID", "LICENSE_NAME");
            return View();
        }

        [KAuth(2005)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewItemMaster(ItemVM IV)
        {
            string U = Session["UserId"].ToString();
            string T = Session["UserRole"].ToString();

            if (ModelState.IsValid)
            {
                A_ITEM_MASTER IM = new A_ITEM_MASTER();
                IM.ITEM_NAME = IV.ITEM_NAME;
                IM.ITEM_CODE = IV.ITEM_CODE;
                IM.ITEM_BRAND = IV.ITEM_BRAND;
                IM.ITEM_FLAVOUR = IV.ITEM_FLAVOUR;
                IM.LICENSE_NAME_ID = IV.LICENSE_NAME_ID;
                IM.ACT = true;
                IM.CDT = DateTime.Now;
                IM.CDU = U;
                IM.ITEM_ID = Sequence.GetSeq(28).ToString();
                db.A_ITEM_MASTER.Add(IM);
                db.SaveChanges();
                TempData["msg"] = "swal('Success..','Saved succefully','success');";
                return RedirectToAction("NewItemMaster", new { ITEM = IM.ITEM_ID });
            }
            TempData["msg"] = "swal('Ohh..','Saved failed','error');";
            return View();
        }
    }
}