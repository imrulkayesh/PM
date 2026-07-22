using PM.Model;
using PM.ModelsV;
using System;
using System.Web.Mvc;

namespace PM.Controllers
{
    public class InventoryController : Controller
    {
        private PMEntities db = new PMEntities();
        public ActionResult Inventory()
        {
            //   var Inv=db.a
            //initial commit
            return View();
        }

        public ActionResult NewConsumption()
        {
            ViewBag.TRNH_FMWH = new SelectList(db.A_AREA, "AREA_ID", "AREA_NAME");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewConsumption(NewConsumption NC)
        {
            if (ModelState.IsValid)
            {
                string Id = "123";
                //A_TRNH H = new A_TRNH();
                //H.TRNH_ID = Id;//Trn id
                //H.TRNH_REF = NC.TRNH_REF;
                //H.TRNH_TYPE = "CO";
                //H.TRNH_FMWH = NC.TRNH_FMWH;
                //H.TRNH_TOWH = NC.TRNH_FMWH;//Same as from WH
                //H.TRNH_NOTE = NC.TRNH_NOTE;
                //H.TRNH_DATE = NC.TRNH_DATE;
                //H.ACT = true;
                //H.CDT = DateTime.Now;
                //H.CDU = "App";
                //H.VAR = 1;
                //db.A_TRNH.Add(H); //Master Data

                decimal Amount = 0.0M;
                short Line = 1;
                foreach (var item in NC.newConsumptionD)
                {
                    //A_TRND D = new A_TRND();
                    //D.TRND_ID = Id;
                    //D.TRND_LINE = Line;
                    //D.TRND_ITEM = item.TRND_ITEM;
                    //D.TRND_QNTY = item.TRND_QNTY;
                    //D.TRND_RATE = 123; //get item rate
                    //D.TRND_NOTE = item.TRND_NOTE;
                    //D.ACT = true;
                    //D.CDT = DateTime.Now;
                    //D.CDU = "App";
                    //D.VAR = 1;
                    //db.A_TRND.Add(D); //Detail Data
                    //Line = (Int16)(Line + 1); //increment for all the items
                    //Amount = Amount + (D.TRND_QNTY * D.TRND_RATE);
                }

                A_JOBH J = db.A_JOBH.Find(NC.TRNH_REF); //find for update Job Card Value
                if (J == null)
                {
                    //Invalid Ref Id
                    return View();
                }
                else
                {
                    if (J.JOBH_CPAY < Amount)
                    {
                        //Negetive Value
                        return View();
                    }
                    else
                    {
                        J.JOBH_CPAY = Convert.ToInt32(Amount);
                    }
                }
                J.UDU = "App";
                J.UDT = DateTime.Now;
                J.VAR = J.VAR + 1;
                //db.Entry(J).State = EntityState.Modified; update Job Card Value


                db.SaveChanges();
            }
            return View();
        }
    }
}