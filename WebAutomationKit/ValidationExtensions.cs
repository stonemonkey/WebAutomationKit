using System;
using System.Collections.Generic;
using System.Linq;

namespace WebAutomationKit
{
    public static class ValidationExtensions
    {
        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable != null && enumerable.Any();
        }

        public static T ValidateNotNull<T>(this T obj, string paramName) where T : class
        {
            return obj ?? throw new ArgumentNullException(paramName);
        }

        public static string ValidateNotNullOrWhitespace(this string str, string paramName)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentNullException(paramName);
            }
            return str;
        }
    }
}
