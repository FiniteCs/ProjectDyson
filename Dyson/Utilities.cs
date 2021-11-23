global using System;
global using System.Collections.Generic;
global using System.Linq;

namespace Dyson
{
    public static class Extensions
    {
        internal static string S(this Type type)
        {
            if (type == typeof(string))
                return "string";
            else if (type == typeof(long))
                return "long";
            else if (type == typeof(bool))
                return "bool";
            else
                return "";
        }
    }
}