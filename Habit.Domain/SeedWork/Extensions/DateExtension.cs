using System;

namespace Application.Extensions
{
    public static class DateExtension
    {
        public static string ToShortDate(this DateTime date) {
            return date.ToString("yyyy-MM-dd");
        }
    }
}