using System;
using System.Linq;

namespace Habit.API.Extensions
{
    public static class StringUtils
    {
        public static string TrimEachLine(this string content) {
            return string.Join(
                Environment.NewLine,
                content
                    .Split(Environment.NewLine)
                    .Select(s => s.Trim())
            );
        }
    }
}