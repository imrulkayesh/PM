using PM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PM.MyAuth
{
    public class PMRoleProvider : RoleProvider
    {
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            PMEntities db = new PMEntities();
            var ROLE_NAME = from ROLE in db.A_USERS_ROLES
                            join r in db.A_ROLES on ROLE.ROLE_TEXT equals r.ROLE_TEXT
                            join u in db.A_USERS on ROLE.USER_TEXT equals u.USER_TEXT
                            where ROLE.ACT == true
                            && r.ACT == true
                            && u.ACT == true
                            && u.USER_TEXT == username
                            select r.ROLE_NAME;
            //var RoleName = from r in db.A_ROLES
            //               join u in db.A_USERS on r.ROLE_TEXT equals u.USER_ROLE
            //               where u.USER_TEXT == username
            //               select r.ROLE_NAME;
            //  string data = ROLE_NAME.ToList().FirstOrDefault();
            //string[] result = { data };
            //  string data = 
            string[] result = ROLE_NAME.ToArray();
            return result;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }

    public class CustomHttpReffer : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string redirectUrl = string.Format("?returnUrl={0}", filterContext.HttpContext.Request.Url.PathAndQuery);
            if (HttpContext.Current.Request.ServerVariables["HTTP_REFERER"] == null)
            {
                string actionName = filterContext.RouteData.Values["action"].ToString();
                string controllerName = filterContext.RouteData.Values["controller"].ToString();

                if (IsValidHttpRefferer(actionName, controllerName))
                {
                    base.OnActionExecuting(filterContext);
                }
                else
                {
                    filterContext.HttpContext.Response.Redirect("~/Home/Forbidden_403", true);
                    //filterContext.Result = new RedirectResult("~/Home/Login" + redirectUrl, true);
                    return;
                }
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }
        }

        private bool IsValidHttpRefferer(string actionName, string controllerName)
        {
            var LoadedList = (List<PM.ModelsV.UserMenuVM>)HttpContext.Current.Session["UserMenuMaster"];
            if (LoadedList != null)
            {
                if (LoadedList.Where(a => a.MENUC_ACTION == actionName && a.MENUC_CONTROLLER == controllerName).Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }

}