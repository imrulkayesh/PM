using PM.BLL;
using PM.Model;
using PM.MyAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PM.Controllers
{
    [Authorize]
    [CustomHttpReffer]
    public class JsonReaderPMController : Controller
    {
        private PMEntities db = new PMEntities();
        ReturnPermission RP = new ReturnPermission();
        public ActionResult getDept(string bus_id)
        {
            var returnDept = new SelectList(RP.DepartmentList(BusId: bus_id), "DEPARTMENT_ID", "DEPARTMENT_NAME");
            return Json(returnDept, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCompany(string group_id)
        {
            var returnCompany = new SelectList(RP.GCompanyList(GrpId: group_id), "COMPANY_ID", "COMPANY_NAME");
            return Json(returnCompany, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getItem(string License_Name_id)
        {
            var returnItem = new SelectList(RP.ItemList(LicNameId: License_Name_id), "ITEM_ID", "ITEM_NAME");
            return Json(returnItem, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getTaskCategory(string category_type_id)
        {
            var returnTaskCategory = new SelectList(RP.TaskCategoryList(CaTypeId: category_type_id), "TASK_CATEGORY_ID", "TASK_CATEGORY_NAME");
            return Json(returnTaskCategory, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getSavingName(string saving_type_id)
        {
            var returnSavingName = new SelectList(RP.SavingNameList(SavingTypeId: saving_type_id), "AUDIT_SAVING_NAME_ID", "AUDIT_SAVING_NAME");
            return Json(returnSavingName, JsonRequestBehavior.AllowGet);
        }
        public ActionResult VehilceListByDept(string DeptId)
        {
            var returnVehicle = new SelectList(RP.VehilceListByDept(DeptId: DeptId), "ASET_ID", "ASET_NAME_TAG");
            return Json(returnVehicle, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AccesoriesByAsset(string AsetId)
        {
            var returnAcce = new SelectList(RP.AccesoriesByAsset(AsetId: AsetId), "ACCE_ID", "ACCE_NAME");
            return Json(returnAcce, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AccAssociateByAsset(string AsetId)
        {
            var returnAcce = new SelectList(RP.AcceAssociatedByAsset(AsetId: AsetId), "ACCE_ID", "ACCE_NAME");
            return Json(returnAcce, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getFreeAsset(string dept, string group)
        {
            var retunFreeAset = new SelectList(RP.FreeAsetByDeptGroup(DeptId: dept, GroupId: group), "ASET_ID", "ASET_CODE_NAME");
            return Json(retunFreeAset, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getActiveAsset(string dept, string group)
        {
            var retunActiveAset = new SelectList(RP.ActiveAsetByDeptGroup(DeptId: dept, GroupId: group), "ASET_ID", "ASET_CODE_NAME");
            return Json(retunActiveAset, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getUserList(string dept, string area)
        {
            var retunUser = new SelectList(RP.OwnerList(DeptId: dept, AreaId: area), "OWNR_ID", "OWNR_USER");
            return Json(retunUser, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ItemNotifyBase(string itemId)
        {
            string Base = db.A_ITEM_LIST.Where(i => i.ITEM_ID == itemId).FirstOrDefault().ITEM_BASE;
            return Json(Base, JsonRequestBehavior.AllowGet);
        }
        public ActionResult IsNotifyActive(string ast, string itm)
        {
            bool IsExist = db.A_REMINDER.Where(a => a.ASET_ID == ast && a.ITEM_ID == itm && a.COMPLETED == "N").Any();
            if (IsExist)
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReminderItem(string gid)
        {
            var Item = new SelectList(RP.ReminderItem(ig: gid), "ITEM_ID", "ITEM_NAME");
            return Json(Item, JsonRequestBehavior.AllowGet);
        }
        public ActionResult InspectionItem(string aid)
        {
            var Item = new SelectList(RP.InspectionItem(ig: aid), "ITEM_ID", "ITEM_NAME");
            return Json(Item, JsonRequestBehavior.AllowGet);
        }
        public JsonResult InspectionHead(string id)
        {
            var insp = new SelectList(RP.MyInspectionForJob(id: id));
            return Json(insp, JsonRequestBehavior.AllowGet);
        }
        public JsonResult InspectionItemNotIn(string aid, string iid)
        {
            var Aset = db.A_ASET_DATA.Find(aid);
            string Group_Id = "";
            List<string> Existitems = new List<string>();
            if (Aset != null)
            {
                Group_Id = Aset.ASET_GROUP;
            }
            var ItemList = db.A_INSPD.Where(i => i.INSPH_ID == iid).ToList();
            if (ItemList.Count > 0)
            {
                foreach (var item in ItemList)
                {
                    Existitems.Add(item.INSPD_ITEM);
                }
            }
            var IITEM = new SelectList(RP.InspectionItemNot(ig: Group_Id, Exist: Existitems), "ITEM_ID", "ITEM_NAME");
            return Json(IITEM, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JobHead(string id)
        {
            var jobs = new SelectList(RP.MyJobForBill(id: id));
            return Json(jobs, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CaseAndPenaltyUserData(string id, DateTime pdt)
        {
            bool succes = false;
            string DEPT = "";
            string AREA = "";
            string USER = "";
            A_ASET_USER AU = db.A_ASET_USER.Where(u => u.AU_ASET == id && (pdt > u.AU_SRDATE && pdt < u.AU_ERDATE)).FirstOrDefault();
            if (AU == null)
            {
                A_ASET_USER AUL = db.A_ASET_USER.Where(u => u.AU_ASET == id && u.ACT == true).FirstOrDefault();
                if (AUL == null)
                {
                    return Json(new { success = succes }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    DEPT = AUL.AU_DEPT;
                    AREA = AUL.AU_AREA;
                    USER = AUL.AU_USER;
                }
            }
            else
            {
                DEPT = AU.AU_DEPT;
                AREA = AU.AU_AREA;
                USER = AU.AU_USER;
            }
            succes = true;
            return Json(new { success = succes, USER_NAME = USER, AREA_NAME = AREA, DEPT_NAME = DEPT }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInsuranceData(string id, DateTime DTE)
        {
            bool Succes = false;
            string INSU_ID = "";
            string INSU_COMP = "";
            int INSU_LINE = 0;
            A_INSUH H = db.A_INSUH.Where(h => h.ACT == true
                                        && h.INSU_ASET == id
                                        && (h.INSU_REG_START <= DTE && h.INSU_REG_END >= DTE)
                                        ).FirstOrDefault();
            if (H != null)
            {
                A_INSUD D = db.A_INSUD.OrderByDescending(d => d.INSU_LINE).Where(d => d.INSU_ID == H.INSU_ID).FirstOrDefault();
                INSU_ID = H.INSU_ID;
                INSU_COMP = H.INSU_COMP;
                INSU_LINE = D.INSU_LINE;
                Succes = true;
            }
            return Json(new { Success = Succes, INSU_ID = INSU_ID, INSU_COMP = INSU_COMP, INSU_LINE = INSU_LINE }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DuplicatePleaRef(string PLEA_REF)
        {
            bool isExist = db.A_PLEA.Where(x => x.PLEA_REF.ToLower() == PLEA_REF.ToLower()).Any();
            return Json(!isExist, JsonRequestBehavior.AllowGet);
        }
    }
}