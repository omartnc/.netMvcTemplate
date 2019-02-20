using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EventManagementSystem.Helper
{
    public static class HttpContextHelper
    {
        public static string HttpContextUsername
        {
            get
            {
                if (HttpContext.Current == null) return null;
                if (!HttpContext.Current.Request.IsAuthenticated) return null;
                try
                {
                    return HttpContext.Current.User.Identity.Name;
                }
                catch
                {
                }
                return null;
            }
        }
    }
}
