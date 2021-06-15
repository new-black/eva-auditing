using Newtonsoft.Json.Linq;
using NSubstitute.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace EVA.Auditing.Helpers
{
    public static class TypeExtensions
    {
        public static bool IsType<TBase>(this Type t)
        {
            return typeof(TBase).IsAssignableFrom(t);
        }

        public static bool IsType(this Type t, Type t2)
        {
            return t2.IsAssignableFrom(t);
        }

        public static bool IsAssemblyEVA(this Assembly a)
        {
            return a.FullName.StartsWith("EVA") || a.GetCustomAttribute<EVAPluginAssemblyAttribute>() != null;
        }

        public static bool IsAnonymousType(this Type t)
        {
            return Attribute.IsDefined(t, typeof(CompilerGeneratedAttribute), false);
        }

        public static bool IsNullableType(this Type t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static bool IsMaybeType(this Type t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Maybe<>);
        }

        public static bool IsEnumerable(this Type t)
        {
            return typeof(IEnumerable).IsAssignableFrom(t) && t != typeof(string) && !IsType<JToken>(t);
        }

        public static bool IsDictionary(this Type t)
        {
            return t.IsType<IDictionary>() || (t.IsGenericType && t.GetGenericTypeDefinition().IsType(typeof(IDictionary<,>)));
        }

        public static Type GetUnderlyingType(this Type t)
        {
            if (t.IsNullableType()) return Nullable.GetUnderlyingType(t);
            return t;
        }

        public static Type? GetEnumerableContentType(this Type t)
        {
            if (t.IsArray)
            {
                return t.GetElementType();
            }

            var getEnumeratorMethod = t.GetMethod("GetEnumerator", Type.EmptyTypes);
            getEnumeratorMethod ??= (from i in t.GetInterfaces()
                                     from m in i.GetMethods()
                                     where m.Name == "GetEnumerator"
                                     orderby m.ReturnType.IsGenericType descending
                                     select m).FirstOrDefault();


            if (getEnumeratorMethod == null) return null;

            if (getEnumeratorMethod.ReturnType.IsGenericType)
            {
                var args = getEnumeratorMethod.ReturnType.GetGenericArguments();

                if (t.IsDictionary() && args.Length == 2)
                {
                    return typeof(KeyValuePair<,>).MakeGenericType(args[0], args[1]);
                }

                return args.First();
            }

            return typeof(object);
        }

        public static bool TryGetCustomAttribute<T>(this Type t, out T customAttribute) where T : Attribute
        {
            return (customAttribute = t.GetCustomAttribute<T>()) != null;
        }
    }
}
