namespace EventManagementSystem.Web.Helpers
{
    public class CookieHelper
    {
        public static void SetCookie(string key, string value, int dayExpires)
        {
            //HttpContext.Current.Session[key] = value;
        }
         
        public static void SetCookie(string key, string value)
        {
            //SetCookie(key, value, 360);
        }
        
        public static string GetCookie(string key)
        {
            //if (HttpContext.Current.Session[key] != null)
            //{
            //    return HttpContext.Current.Session[key].ToString();
            //}
            //else
                return null;
        }
        
        public static void RemoveCookie(string key)
        {
            //if (HttpContext.Current.Request.Cookies[key] != null)
            //{
            //    HttpContext.Current.Session[key] = null;
            //}
        }

        public static void RemoveAllCookies()
        {
            //foreach (string key in HttpContext.Current.Request.Cookies.AllKeys)
            //{
            //    HttpContext.Current.Session[key] = null;
            //}
        }
    }
}