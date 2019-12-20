using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.Helper
{
    public  static class md5Helper
    {
        public static string GetMD5_2(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            foreach (var t in targetData)
            {
                byte2String += t.ToString("x2");
            }
            return byte2String;
        }
    }
}
