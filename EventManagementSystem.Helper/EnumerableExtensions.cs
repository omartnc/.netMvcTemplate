using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.Helper
{
    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            if (source == null)
                return true;
            return !source.Any();
        }

        public static IEnumerable<TSource> DistinctEquals<TSource, TProperty>(this IEnumerable<TSource> list, Func<TSource, TProperty> comparisonProperty)
        {
            return DistinctEquals(list, (s, t) => comparisonProperty(s).Equals(comparisonProperty(t)));
        }

        public static IEnumerable<TSource> DistinctEquals<TSource>(this IEnumerable<TSource> list, Func<TSource, TSource, bool> equality)
        {
            if (list == null || equality == null)
                return null;

            var result = new List<TSource>();

            foreach (TSource item in list)
            {
                bool exists = result.Any(s => equality.Invoke(s, item));

                if (!exists)
                    result.Add(item);
            }

            return result;
        }

        public static string JoinString<T>(this IEnumerable<T> list, Func<T, object> selector)
        {
            return JoinString<T>(list, selector, String.Empty);
        }

        public static string JoinString<T>(this IEnumerable<T> list, Func<T, object> selector, string separator)
        {
            if (list == null || selector == null)
                return String.Empty;

            var builder = new StringBuilder();

            foreach (T item in list)
            {
                builder.AppendFormat("{0}{1}", selector(item).ToString(), separator);
            }

            if (builder.Length > 0 && String.IsNullOrEmpty(separator) == false)
                builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }

        public static int FindIndex<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (items == null) throw new ArgumentNullException("items");
            if (predicate == null) throw new ArgumentNullException("predicate");

            int retVal = 0;
            foreach (var item in items)
            {
                if (predicate(item)) return retVal;
                retVal++;
            }
            return -1;
        }

        public static T GetNext<T>(this IEnumerable<T> list, T current)
        {
            return list.SkipWhile(x => !x.Equals(current)).Skip(1).FirstOrDefault();
        }

        public static T GetNext<T>(this IEnumerable<T> list, T current, Func<T, T, bool> comparison)
        {
            return list.SkipWhile(x => !comparison(x, current)).Skip(1).FirstOrDefault();
        }

        public static T GetPrevious<T>(this IEnumerable<T> list, T current)
        {
            return list.TakeWhile(x => !x.Equals(current)).LastOrDefault();
        }

        public static T GetPrevious<T>(this IEnumerable<T> list, T current, Func<T, T, bool> comparison)
        {
            return list.TakeWhile(x => !comparison(x, current)).LastOrDefault();
        }

    }
}
