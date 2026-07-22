using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PM.MyAuth
{
    public class KAuth : ActionFilterAttribute
    {
        int _i; //from controller or action method
        public KAuth(int i)
        {
            _i = i;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string redirectUrl = string.Format("?returnUrl={0}", filterContext.HttpContext.Request.Url.PathAndQuery);
            if (HttpContext.Current.Session["UserMenuMaster"] == null)
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();
                filterContext.Result = new RedirectResult("~/Home/Login" + redirectUrl, true);
            }
            var MenuMaster = (List<PM.ModelsV.UserMenuVM>)HttpContext.Current.Session["UserMenuMaster"];
            bool IsPermitted = false;

            if (MenuMaster != null)
            {
                IsPermitted = MenuMaster.Any(p => p.MENUC_TEXT == _i);
            }
            if (IsPermitted)
            {
                base.OnActionExecuting(filterContext);
            }
            else
            {
                filterContext.HttpContext.Response.Redirect("~/Home/Forbidden_403", true);
                return;
            }
            //string redirectUrl = string.Format("?returnUrl={0}", filterContext.HttpContext.Request.Url.PathAndQuery);
            //if (HttpContext.Current.Request.ServerVariables["HTTP_REFERER"] == null)
            //{
            //    string actionName = filterContext.RouteData.Values["action"].ToString();
            //    string controllerName = filterContext.RouteData.Values["controller"].ToString();
            //    if (IsValidHttpRefferer(actionName, controllerName))
            //    {
            //        base.OnActionExecuting(filterContext);
            //    }
            //    else
            //    {
            //        filterContext.HttpContext.Response.Redirect("~/Home/Forbidden_403", true);
            //        return;
            //    }
            //}
            //else
            //{
            //    if (HttpContext.Current.Session["UserMenuMaster"] == null)
            //    {
            //        HttpContext.Current.Session.Clear();
            //        HttpContext.Current.Session.Abandon();
            //        //HttpContext.Current.Response.Redirect("~/Home/Logout", true);
            //        filterContext.HttpContext.Response.Redirect("~/Home/Forbidden_403", true);
            //    }
            //    var MenuMaster = (List<PM.ModelsV.UserMenuVM>)HttpContext.Current.Session["UserMenuMaster"];
            //    bool IsPermitted = MenuMaster.Where(p => p.MENUC_TEXT == _i).Any();
            //    if (IsPermitted)
            //    {
            //        base.OnActionExecuting(filterContext);
            //    }
            //    else
            //    {
            //        filterContext.HttpContext.Response.Redirect("~/Home/Forbidden_403", true);
            //        return;
            //    }
            //}
        }

        //private bool IsValidHttpRefferer(string actionName, string controllerName)
        //{
        //    var LoadedList = (List<PM.ModelsV.UserMenuVM>)HttpContext.Current.Session["UserMenuMaster"];
        //    if (LoadedList != null)
        //    {
        //        if (LoadedList.Where(a => a.MENUC_ACTION == actionName && a.MENUC_CONTROLLER == controllerName).Any())
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
    }
}