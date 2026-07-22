using Oracle.ManagedDataAccess.Client;
using PM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PM.Controllers
{
    public class MenuPermissionController : Controller
    {
        // GET: UserPermission
        public ActionResult Index()
        {
            return View();
        }
        // Get ? current user permission
        public ActionResult LoadPermission(string UserId)
        {
            string SQL = "";
            SQL = @"SELECT P.MENUU_ID,P.MENUU_MENUC_ID,C.MENUC_ID || '-' || C.MENUC_NAME AS MENU,
                        P.MENUU_USER,P.MENUU_USER || '-' || U.USER_NAME AS USERID,P.MENUU_ACTV
                        FROM PM.MENUU P 
                        INNER JOIN PM.P_USERS U ON P.MENUU_USER=U.USER_TEXT
                        INNER JOIN PM.MENUC C ON P.MENUU_MENUC_ID=C.MENUC_ID
                        WHERE U.USER_TEXT='" + UserId + "'  ORDER BY P.MENUU_ID";

            List<Dictionary<string, string>> dictioneryList = new List<Dictionary<string, string>>();
            DataTable dataTable = new DataTable();
            OracleCommand cmd = new OracleCommand();
            OracleDataAdapter adpt = new OracleDataAdapter();
            cmd.Connection = new OracleConnection(PM.ExtClass.DH.ConStringPMdB);
            cmd.CommandText = SQL;
            adpt.SelectCommand = cmd;
            dataTable.Reset();
            adpt.Fill(dataTable);
            dictioneryList.AddRange(PM.ExtClass.DH.GetTableRows(dataTable));
            return View(dictioneryList);
        }
        //Get ? No permiss menu list
        public ActionResult NonPermitMenuList(string UserId)
        {
            string SQL = "";
            SQL = @"SELECT C.MENUC_ID,C.MENUC_NAME
            FROM PM.MENUC C
            LEFT OUTER JOIN PM.MENUU U ON C.MENUC_ID = U.MENUU_MENUC_ID AND U.MENUU_USER='"+ UserId + @"'
            WHERE U.MENUU_ID IS NULL
            AND exists (SELECT * FROM PM.P_USERS P WHERE P.USER_TEXT='"+ UserId + "')";

            List<Dictionary<string, string>> dictioneryList = new List<Dictionary<string, string>>();
            DataTable dataTable = new DataTable();
            OracleCommand cmd = new OracleCommand();
            OracleDataAdapter adpt = new OracleDataAdapter();
            cmd.Connection = new OracleConnection(PM.ExtClass.DH.ConStringPMdB);
            cmd.CommandText = SQL;
            adpt.SelectCommand = cmd;
            dataTable.Reset();
            adpt.Fill(dataTable);
            dictioneryList.AddRange(PM.ExtClass.DH.GetTableRows(dataTable));
            ViewBag.UserString = UserId;
            return View(dictioneryList);
        }
        //Post ? Add Menu to User
        public ActionResult AddToUser(string UserId, string MenuId)
        {
            string ConcatId = MenuId+ UserId;
            int Menu = Convert.ToInt32(MenuId);
            // int User = Convert.ToInt32(UserId);
            using (pmdbEntities db = new pmdbEntities())
            {
                MENUU MU = new MENUU();
                MU.MENUU_ID = Convert.ToDecimal(ConcatId);
                MU.MENUU_MENUC_ID = Menu;
                MU.MENUU_USER = UserId;
                MU.MENUU_ACTV = true;
                db.MENUUs.Add(MU);
                db.SaveChanges();
                ViewBag.UserString = UserId;
                TempData["MSG"] = "OK! Save successfully.";
                TempData["UserString"] = UserId;
                return RedirectToAction("Index");
            }
        }
    }
}