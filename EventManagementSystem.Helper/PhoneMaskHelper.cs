using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.Helper
{
    public static class PhoneMaskHelper
    {
        public static string FormatPhoneNumber(string MaskPhone)
        {
            MaskPhone = MaskPhone.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");

            if (MaskPhone.StartsWith("+9"))
                MaskPhone = MaskPhone.Substring(2, MaskPhone.Length - 2);
            else if (MaskPhone.StartsWith("009"))
                MaskPhone = MaskPhone.Substring(3, MaskPhone.Length - 3);

            int len = MaskPhone.Length;

            if (len < 11)
            {
                MaskPhone = MaskPhone.PadLeft(11, '0');
            }

            MaskPhone = MaskPhone.Substring(0, 4) + " " + MaskPhone.Substring(4, 3) + " " + MaskPhone.Substring(7, 2) + " " + MaskPhone.Substring(9, 2);

            return MaskPhone;
        }
    }
}
