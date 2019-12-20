using System;
using System.Web.Mvc;
using EventManagementSystem.Service;

namespace EventManagementSystem.Web.Helpers
{
    [Authorize]
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        public string RootController { get; set; }
        public string Action { get; set; }


        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
                    return;

                var routingValues = filterContext.RouteData.Values;
                var currentController = string.IsNullOrEmpty(RootController) ?  ((string)routingValues["controller"] ?? string.Empty) : RootController ;
                //var currentAction = (string)routingValues["action"] ?? string.Empty;

                var contextUser = filterContext.RequestContext.HttpContext.User.Identity.Name;

                if (!int.TryParse(contextUser, out var userId))
                {
                    var userService = new UserService();
                    var user = userService.GetByUsername(contextUser);
                    userId = user.Id;
                }

                var userAuthorizationService = new UserRoleService();
                var moduleService = new ModuleService();

                var isAllowed = false;
                var url = filterContext.HttpContext.Request.RawUrl;

                var actions = Action.Split(',');
                foreach (var action in actions)
                {
                    var module = moduleService.Get($"{currentController}.{action}");
                    isAllowed = isAllowed || userAuthorizationService.GetUserRole(module.Id, userId);
                }
                
                if (isAllowed)
                    base.OnAuthorization(filterContext);
                else
                    filterContext.Result = new RedirectResult("/kullanici/oturum-ac?auth=0&ReturnUrl=" + url);
            }
            catch
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
    }

    public static class AuthorizeUserCheck
    {
        public static bool HasAccess(string controllerName, string actionName)
        {
            var userService = new UserService();
            var user = userService.Get();

            var userAuthorizationService = new UserRoleService();
            var moduleService = new ModuleService();

            /*
            var module = moduleService.Get($"{controllerName}.{actionName}");

            if (module == null)
                return false;
                */
            int moduleId = 0;

            var actions = actionName.Split(',');
            foreach (var action in actions)
            {
                var module = moduleService.Get($"{controllerName}.{action}");

                if (module != null)
                {
                    moduleId = module.Id;
                    break;
                }
            }

            if (moduleId == 0)
                return false;


            if (userAuthorizationService.GetUserRole(moduleId, user.Id))
                return true;

            return false;
        }

        public static bool HasAccess(string controllerName, string actionName, string customName)
        {
            var userService = new UserService();
            var user = userService.Get();

            var userAuthorizationService = new UserRoleService();
            var moduleService = new ModuleService();

            int moduleId = 0;

            var actions = actionName.Split(',');
            foreach (var action in actions)
            {
                var module = moduleService.Get($"{controllerName}.{action}.{customName}");

                if (module != null)
                {
                    moduleId = module.Id;
                    break;
                }
            }

            if (moduleId == 0)
                return false;

            if (userAuthorizationService.GetUserRole(moduleId, user.Id))
                return true;

            return false;
        }
    }
}