global using System;
global using System.Collections.Generic;
global using System.Linq;

using Dyson.CodeAnalysis.Binding.Types;

namespace Dyson
{
    // key k = ini["KeyName"]
    // key.Value = "NewValue"
    // key.Name = "NewName"
    // key a = new key("KeyName")
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
            else if (type == typeof(Key))
                return "key";
            else
                return "";
        }
    }
}