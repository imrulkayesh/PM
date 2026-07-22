using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PM.BLL
{
    public class MyAuthorization : ActionFilterAttribute
    {
        int Role; //from controller or action method
        public MyAuthorization(int RoleNo)
        {
            Role = RoleNo;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string redirectUrl = string.Format("?returnUrl={0}", filterContext.HttpContext.Request.Url.PathAndQuery);
            int x = Convert.ToInt32(HttpContext.Current.Session["UserRole"].ToString());

            if (HttpContext.Current.Session["UserRole"] == null)
            {
                filterContext.Result = new RedirectResult("~/Home/Login" + redirectUrl, true);
                return;
            }
            else if (HttpContext.Current.Request.ServerVariables["HTTP_REFERER"] == null)
            {
                //filterContext.HttpContext.Response.Redirect("~/Home/Login" + redirectUrl, true);
                string actionName = filterContext.RouteData.Values["action"].ToString();
                string controllerName = filterContext.RouteData.Values["controller"].ToString();

                if (!IsValidHttpRefferer(actionName, controllerName))
                {
                    base.OnActionExecuting(filterContext);
                }
                else
                {
                    filterContext.Result = new RedirectResult("~/Home/Login" + redirectUrl, true);
                    return;
                }
            }
            else if (Convert.ToInt32(HttpContext.Current.Session["UserRole"].ToString()) < Role)
            {               
                string actionName = filterContext.RouteData.Values["action"].ToString();
                string controllerName = filterContext.RouteData.Values["controller"].ToString();
                filterContext.Result = new RedirectResult("~/Home/Forbidden_403",true ) ;
                return;
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }
        }

        private bool IsValidHttpRefferer(string actionName, string controllerName)
        {
            var LoadedList = (List<PM.ModelsV.UserMenuVM>)HttpContext.Current.Session["UserMenuMaster"];

            if (LoadedList.Where(a => a.MENUC_ACTION == actionName && a.MENUC_CONTROLLER == controllerName).Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}