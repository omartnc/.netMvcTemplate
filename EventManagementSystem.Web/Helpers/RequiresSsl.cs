using System.Web.Mvc;

namespace EventManagementSystem.Web.Helpers
{
    public class RequiresSsl : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var req = filterContext.HttpContext.Request;
            var res = filterContext.HttpContext.Response;
            
            if (!req.IsSecureConnection && !req.IsLocal)
            {
                var url = req.Url.ToString().ToLower().Replace("http:", "https:");
                //res.Redirect(url);
            }
            base.OnActionExecuting(filterContext);
        }
    }
}