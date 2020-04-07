using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Visitor_Registration.Controllers;
using VisitorRegistation.Common;
using VisitorRegistration.Common;
using VisitorRegistration.Models;


namespace VisitorRegistration.Common
{
    public class HasCredentialAttribute : AuthorizeAttribute
    {
        public string RoleID { set; get; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var session = (EmployeeModel)HttpContext.Current.Session[CommonConstants.USER_SESSION];
            if (session == null)
            {
                return false;
            }

            List<string> privilegeLevels = this.GetCredentialByLoggedInUser(); // Call another method to get rights of the user from DB

            if (privilegeLevels.Contains(this.RoleID))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var session = (EmployeeModel)HttpContext.Current.Session[CommonConstants.USER_SESSION];
            if (session != null)
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Views/Shared/Error.cshtml"
                };
            }
            else
            {
                //base.HandleUnauthorizedRequest(filterContext);
                filterContext.Result = new RedirectToRouteResult(
                 new RouteValueDictionary(new { controller = "Login", action = "Index" }));
                return;
            }
        }
        private List<string> GetCredentialByLoggedInUser()
        {
            var credentials = (List<string>)HttpContext.Current.Session[CommonConstants.SESSION_CREDENTIALS];
            return credentials;
        }
    }

    public class DenyHasCredentialAttribute : AuthorizeAttribute
    {
        public string RoleID { set; get; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var session = (EmployeeModel)HttpContext.Current.Session[CommonConstants.USER_SESSION];
            if (session == null)
            {
                return false;
            }
            List<string> privilegeLevels = this.GetCredentialByLoggedInUser(); // Call another method to get rights of the user from DB
            if (privilegeLevels.Contains(this.RoleID))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Shared/Error.cshtml"
            };
        }
        private List<string> GetCredentialByLoggedInUser()
        {
            var credentials = (List<string>)HttpContext.Current.Session[CommonConstants.SESSION_CREDENTIALS];
            return credentials;
        }
    }
}