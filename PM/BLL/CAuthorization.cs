using PM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PM.BLL
{
    public class CAuthorization : ActionFilterAttribute
    {
        int _i; //from controller or action method
        public CAuthorization(int RoleNo)
        {
            _i = RoleNo;
        }


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string actionName = filterContext.ActionDescriptor.ActionName;
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;



            if (!CheckAccessRight(actionName, controllerName))
            {
                string redirectUrl = string.Format("?returnUrl={0}", filterContext.HttpContext.Request.Url.PathAndQuery);

                filterContext.HttpContext.Response.Redirect("~/Home/Login" + redirectUrl, true);
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }
        }


        private bool CheckAccessRight(string Action, string Controller)
        {
            //    if (HttpContext.Current.Session["UserId"] != null)
            //    {
            //        string userID = HttpContext.Current.Session["UserId"].ToString();

            //        using (var db = new PMEntities())
            //        {
            //            A_USERS objAss = null;
            //            if (HttpContext.Current.Session["AccountType"].ToString() == "lab")
            //            {
            //                //   objAss = db.A_USERS.SingleOrDefault(model => model.userid == userID);
            //            }
            //            else
            //            {
            //                // objAss = db.A_USERS.SingleOrDefault(model => model.employeeId == userID);
            //            }

            //            //String UserRole = objAss.A_ROLES;

            //            //  itemchildren objChild = db.itemchildrens.SingleOrDefault(model => model.parent == UserRole && model.child == Controller + " " + Action);
            //            string objChild = null;
            //            if (objChild != null)
            //            {
            //                return true;
            //            }
            //            else
            //            {
            //                return false;
            //            }


            //        }
            //    }
            //    else
            //    {
            //        return false;
            //    }
            return true;
        }



    }
}