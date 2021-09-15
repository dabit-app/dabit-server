using System;

namespace Domain.SeedWork.Extensions
{
    public static class DateExtension
    {
        public static string ToShortDate(this DateTime date) {
            return date.ToString("yyyy-MM-dd");
        }
    }
}