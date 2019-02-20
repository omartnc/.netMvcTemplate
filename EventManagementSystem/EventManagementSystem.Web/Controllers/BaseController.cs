using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventManagementSystem.Entity.Model.Common;
using EventManagementSystem.Service;
using log4net;

namespace EventManagementSystem.Web.Controllers
{
    public class BaseController : Controller
    {
        public static readonly ILog Logger = LogManager.GetLogger(System.Environment.UserName);
        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                    viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                    ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
        public UserService UserService = new UserService();
        public ParameterService ParameterService = new ParameterService();
        public ModuleService ModuleService = new ModuleService();
        public RoleService RoleService = new RoleService();
        public RoleModuleService RoleModuleService = new RoleModuleService();
        public UserRoleService UserRoleService = new UserRoleService();
        public FileService FileService = new FileService();
        public User CurrentUser = null;

        public BaseController()
        {
            log4net.LogicalThreadContext.Properties["IsActive"] = true;
            log4net.LogicalThreadContext.Properties["IsDeleted"] = false;
            log4net.LogicalThreadContext.Properties["CreationDate"] = DateTime.Now;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            CurrentUser = UserService.Get();
            
        }
    }
}