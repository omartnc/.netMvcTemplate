using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using EventManagementSystem.Web.App_Start;
using Newtonsoft.Json;

namespace EventManagementSystem.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };


            log4net.Config.DOMConfigurator.Configure();

        }
        protected void FormsAuthentication_OnAuthenticate(object sender, FormsAuthenticationEventArgs e)
        {
            if (!FormsAuthentication.CookiesSupported) return;
            if (Request.Cookies[FormsAuthentication.FormsCookieName] == null) return;

            try
            {
                var formsAuthenticationTicket = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value);
                if (formsAuthenticationTicket == null) return;
                var userId = formsAuthenticationTicket.Name;
                e.User = new GenericPrincipal(new GenericIdentity(userId, "Forms"), new[] { "" });
            }
            catch (Exception)
            {

            }
        }
    }
}
