using System;

namespace EventManagementSystem.Web.Helpers
{
    public static class GalleryHelper
    {
        public static readonly Random R = new Random();

        public static string GetRandomImage(int? width = null, int? height = null, string keyword = "")
        {
            return "http://loremflickr.com/" + (width ?? R.Next(600, 1200)) + "/" + (height ?? R.Next(600, 1200)) + "/" + keyword + "?random=" + R.Next(1,999);
        }
    }
}