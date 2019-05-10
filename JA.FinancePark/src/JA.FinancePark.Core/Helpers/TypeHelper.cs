using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JA.FinancePark.Core.Helpers
{
    public static class TypeHelper
    {
        public static string GetStringRepresentation(object instance)
        {
            Type type = instance.GetType();

            List<PropertyInfo> props = type.GetProperties().OrderBy(o => o.Name).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (PropertyInfo prop in props)
            {
                if (prop.CanRead && (prop.PropertyType.IsPrimitive ||
                    prop.PropertyType == typeof(string) ||
                    prop.PropertyType == typeof(int) ||
                    prop.PropertyType == typeof(long) ||
                    prop.PropertyType == typeof(bool) ||
                    prop.PropertyType == typeof(bool?)))
                {
                    object val = prop.GetValue(instance);
                    if (val != null)
                    {
                        sb.Append($"{prop.Name}:{val} | ");
                    }
                }
            }

            string result = sb.ToString();

            return result;
        }
    }
}
