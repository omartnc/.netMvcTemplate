using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;

namespace EventManagementSystem.Web.Helpers
{
    public static class TemplateHelper
    {
        public static string RenderViewToString(string controlPath, ViewDataDictionary data = null)
        {
            var sb = new StringBuilder();

            if (controlPath != null && controlPath.EndsWith(".cshtml", StringComparison.InvariantCultureIgnoreCase))
            {
                data = data ?? new ViewDataDictionary(new Dictionary<string, object>());
                var httpBase = new HttpContextWrapper(HttpContext.Current);

                var controller = new FakeController();
                var route = new RouteData();
                route.Values.Add("controller", "something");
                var controllerContext = new ControllerContext(httpBase, route, controller);

                var actualPath = controlPath;
                if (actualPath.StartsWith("~/"))
                    actualPath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, actualPath.Substring(2).Replace("/", "\\"));

                if (!File.Exists(actualPath))
                    throw new ArgumentOutOfRangeException(nameof(controlPath), string.Format("controlPath: {0}", actualPath));

                var view = new RazorView(controllerContext, controlPath, null, false, null, null);
                using (var sw = new StringWriter(sb))
                {
                    var viewContext = new ViewContext(controllerContext, view, data, new TempDataDictionary(), sw);
                    view.Render(viewContext, sw);
                }
            }
            else
            {
                var vp = new ViewPage { ViewData = data };
                var control = vp.LoadControl(controlPath);
                vp.Controls.Add(control);
                using (var sw = new StringWriter(sb))
                using (var tw = new HtmlTextWriter(sw))
                    vp.RenderControl(tw);
            }

            return sb.ToString();
        }

        public static string RenderTemplate<T>(string templatePath, T model)
            => RenderViewToString(templatePath, new ViewDataDictionary<T>(model));
    }
    public class FakeController : Controller
    {
    }
}