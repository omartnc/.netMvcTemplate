using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using EventManagementSystem.Service;

namespace EventManagementSystem.WebApi.Helpers
{
    [Authorize]
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        public string RootController { get; set; }
        public string Action { get; set; }


        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                if (actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>(true).Any() ||
                   actionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>(true).Any())
                    return;

                var routingValues = actionContext.Request.GetRouteData().Values;
                var currentController = string.IsNullOrEmpty(RootController) ? ((string)routingValues["controller"] ?? string.Empty) : RootController;

                var ClaimsIdentity = (ClaimsIdentity)actionContext.RequestContext.Principal.Identity;
                var contextUser = ClaimsIdentity.Claims.FirstOrDefault().Value;

                if (!int.TryParse(contextUser, out var userId))
                {
                    var userService = new UserService();
                    var user = userService.GetByUsername(contextUser);
                    userId = user.Id;
                }

                var ApiUserRoleService = new ApiUserRoleService();
                var moduleService = new ModuleService();

                var isAllowed = false;

                var actions = Action.Split(',');
                foreach (var action in actions)
                {
                    var module = moduleService.Get($"{currentController}.{action}");
                    isAllowed = isAllowed || ApiUserRoleService.GetUserRole(module.Id, userId);
                }

                if (isAllowed)
                    base.OnAuthorization(actionContext);
                else
                    base.HandleUnauthorizedRequest(actionContext);
            }
            catch (Exception e)
            {
                base.HandleUnauthorizedRequest(actionContext);
            }
        }

    }
}