using System;
using System.Linq;
using System.Reflection;

namespace EVA.Auditing.Helpers
{
    public static class TypeHelper
    {
        public static bool IsEnum(Type type)
        {
            return !type.IsNullableType()
              ? type.IsEnum
              : Nullable.GetUnderlyingType(type)?.IsEnum ?? false;
        }

        public static bool Is(Type type, params Type[] types)
        {
            var nullableType = Nullable.GetUnderlyingType(type);

            return types.Any(t => t.IsAssignableFrom(nullableType ?? type));
        }

        public static bool IsMaybeWrappingAnEnumerationType(Type type)
        {
            if (!type.IsMaybeType()) return false;
            return type.GetGenericArguments()[0].IsEnum;

        }

        public static bool IsDictionaryOfEnum(Type type, out (bool key, bool value) keyValueMapping)
        {
            keyValueMapping = (false, false);

            if (!type.IsDictionary())
            {
                return false;
            }

            var kv = type.GetEnumerableContentType();

            if (kv is null)
            {
                return false;
            }

            var genericArgs = kv.GetGenericArguments();


            keyValueMapping = (IsEnum(genericArgs[0]), IsEnum(genericArgs[1]));

            return keyValueMapping.key || keyValueMapping.value;
        }

        public static bool IsNullableReferenceType(MemberInfo propertyInfo)
        {

            return (propertyInfo.DeclaringType?.GetCustomAttributes().Any(a => a.GetType().Name == "NullableAttribute") ?? false) ||
              propertyInfo.GetCustomAttributes().Any(a => a.GetType().Name == "NullableAttribute");
        }

        public static string GetNameOfGenericType(Type t)
        {
            var idx = t.Name.IndexOf('`');
            return idx > 0 ? t.Name.Substring(0, idx) : null;
        }

        public static long GetEnumValueAsNumber(Type enumType, object value)
        {
            var underlyingType = Enum.GetUnderlyingType(enumType);
            if (underlyingType == typeof(byte))
            {
                return (byte)value;
            }

            return (int)value;
        }
    }
}
