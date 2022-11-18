using System;
using System.Net;

namespace IocLists
{
    public static class Extensions
    {
        public static string UrlEncode(this string input) => WebUtility.UrlEncode(input);

        public static DateTime ToDate(this double milliseconds)
            => DateTime.UnixEpoch.AddMilliseconds(milliseconds);

        public static double ToUnix(this DateTime date)
            => (date - DateTime.UnixEpoch).TotalMilliseconds;
    }
}