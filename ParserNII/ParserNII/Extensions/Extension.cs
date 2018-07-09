using System;
using System.Globalization;

namespace ParserNII.Extensions
{
    public static class Extension
    {
        public static TAttribute GetAttribute<T, TAttribute>(this T value)
            where TAttribute : Attribute
        {
            Type type = value.GetType();


            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(TAttribute), false);
            return (attributes.Length > 0) ? (TAttribute)attributes[0] : null;
        }
    }
}