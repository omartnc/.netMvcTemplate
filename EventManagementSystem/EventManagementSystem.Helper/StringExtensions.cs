using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EventManagementSystem.Helper
{
    public static class StringExtensions
    {
        public static string MakeSingleDot(this string value, int length)
        {
            if (string.IsNullOrEmpty(value)) return "";
            if (value.Length > length)
                value = value.Substring(0, length) + ".";
            return value;
        }

        public static string MakeThreeDot(this string value, int length)
        {
            if (string.IsNullOrEmpty(value)) return "";
            if (value.Length > length)
                value = value.Substring(0, length) + "...";
            return value;
        }

        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static string ToUrlFriendly(this string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            string[] olds = { " ", "Ğ", "ğ", "Ü", "ü", "Ş", "ş", "İ", "ı", "Ö", "ö", "Ç", "ç", "&", "/", ".", "&" };
            string[] news = { "-", "G", "g", "U", "u", "S", "s", "I", "i", "O", "o", "C", "c", "-", "-", "-", "-" };

            for (int i = 0; i < olds.Length; i++)
            {
                value = value.Replace(olds[i], news[i]);
            }

            while (value.Contains("--"))
                value = value.Replace("--", "-");

            value = value.TrimStart('-').TrimEnd('-');

            value = new Regex("[^a-zA-Z0-9-\\.]").Replace(value, "");

            return value;
        }

        public static string ToEnglishLetter(this string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            string[] olds = { "Ğ", "ğ", "Ü", "ü", "Ş", "ş", "İ", "ı", "Ö", "ö", "Ç", "ç"};
            string[] news = { "G", "g", "U", "u", "S", "s", "I", "i", "O", "o", "C", "c" };

            for (int i = 0; i < olds.Length; i++)
            {
                value = value.Replace(olds[i], news[i]);
            }

            return value;
        }

        public static string ToUrlFriendlyLower(this string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            return ToUrlFriendly(value.ToLower());
        }

        public static string ReplaceNewLine(this string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            return value.Replace(Environment.NewLine, "<br/>");
        }

        public static string ClearHtml(this string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            return Regex.Replace(value, @"<(.|\n)*?>", string.Empty);
        }
    }
}
