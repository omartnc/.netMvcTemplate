using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EventManagementSystem.Service.Helper
{
    public static class UserRoleCookie
    {
        public static void CookieSave(List<int> permitted_access_modules)
        {
            HttpCookie Cookie = null;
            if (HttpContext.Current.Response.Cookies["UserRole"] != null)
                //Cookie varsa devam.
                Cookie = HttpContext.Current.Response.Cookies["UserRole"];
            else
                //Yoksa oluşturuyoruz.
                Cookie = new HttpCookie("UserRole");


            for (int i = 0; i < permitted_access_modules.Count; i++)
            {
                Cookie["permitted_access_module_" + i] = permitted_access_modules[i].ToString();
            }

            HttpContext.Current.Response.Cookies.Add(Cookie);
        }

        public static List<int> CookieGet()
        {
            try
            {
                if (HttpContext.Current.Request.Cookies["UserRole"] != null)
                {
                    List<int> returnList = new List<int>();
                    var CookieValues = HttpContext.Current.Request.Cookies["UserRole"].Values;
                    for (int i = 0; i < CookieValues.Count; i++)
                    {
                        returnList.Add(Convert.ToInt32(CookieValues[i]));
                    }

                    return returnList;
                }
                else
                    return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static void CookieRemove()
        {
            if (HttpContext.Current.Request.Cookies["UserRole"] != null)
            {
                HttpContext.Current.Response.Cookies["UserRole"].Expires = DateTime.Now.AddDays(-1);
            }
        }
    }
}
