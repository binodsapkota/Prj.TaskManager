using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Web;


namespace Prj.TaskManager.Filters
{
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        string _roles;
        public CustomAuthorizeAttribute(string roles = "")
        {
            _roles = roles;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            bool isAuthenticated = user.Identity?.IsAuthenticated ?? false;//go to identity library and check for the user authentication
            if (!isAuthenticated)
            {
                context.Result = new RedirectToActionResult("Login", "Account", "");
                return;
            }
            bool isPermisionDenied = true;
            if (!string.IsNullOrEmpty(_roles) && !user.IsInRole(_roles))
            {
                context.Result = new RedirectToActionResult("NoPermission", "Account", "");
            }

        }


    }
}
