using MVC_CRUD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVC_CRUD.Filters
{
    public class CustomAuthorizationFilter : AuthorizeAttribute
    {
        private readonly string[] allowedRoles;
        public CustomAuthorizationFilter(params string[] roles)
        {
            this.allowedRoles = roles;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            var userName = Convert.ToString(httpContext.Session["UserName"]);
            if (!string.IsNullOrEmpty(userName))
            {
                using (var db = new DBModel())
                {
                    var userRole = (from u in db.users
                                    join r in db.roles on u.RoleId equals r.Id
                                    where u.UserName == userName
                                    select new
                                    {
                                        r.Name
                                    }).FirstOrDefault();
                    foreach (var role in allowedRoles)
                    {
                        if (role == userRole.Name) return true;
                    }
                }
            }
            return authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    { "controller", "Home" },
                    { "action", "Unauthorized" }
                });
        }
    }
}