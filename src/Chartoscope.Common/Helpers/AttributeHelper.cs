using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public static class AttributeHelper
    {       
        public static T GetTypeAttribute<T>(Type type)
        {
            T typeAttribute = default(T);

            object[] customAttributes = type.GetCustomAttributes(typeof(T), false);
            if (customAttributes.Length > 0)
            {
                typeAttribute = ((T)type.GetCustomAttributes(typeof(T), false)[0]);
            }

            return typeAttribute;
        }

        public static TAttribute GetAttribute<TAttribute>(this Enum value)
            where TAttribute : Attribute
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            return type.GetField(name)
                .GetCustomAttributes(false)
                .OfType<TAttribute>()
                .SingleOrDefault();
        }
    }
}
