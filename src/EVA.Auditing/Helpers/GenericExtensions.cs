using System;
using System.Threading.Tasks;

namespace EVA.Auditing.Helpers
{
    public static class GenericExtensions
    {
        public static T With<T>(this T t, params Action<T>[] actions) where T : class
        {
            if (t == null) return null;
            foreach (var a in actions) a(t);
            return t;
        }

        public static T With<T>(this T t, Action<T> action) where T : class
        {
            if (t == null) return null;
            action(t);
            return t;
        }

        public static T With<T>(this T t, bool condition, params Action<T>[] actions) where T : class
        {
            return condition ? t.With(actions) : t;
        }

        public static T With<T>(this T t, bool condition, Action<T> action) where T : class
        {
            return condition ? t.With(action) : t;
        }

        public static TU Then<T, TU>(this T t, Func<T, TU> selector) where TU : class
        {
            if (t == null) return null;
            return selector(t);
        }

        public static async Task<TU> Then<T, TU>(this Task<T> task, Func<T, TU> selector)
        {
            return selector(await task);
        }

        public static async ValueTask<TU> Then<T, TU>(this ValueTask<T> task, Func<T, TU> selector)
        {
            return selector(await task);
        }

        public static Task<T> AsTask<T>(this T t) => Task.FromResult(t);
        public static ValueTask AsValueTask(this Task t) => new ValueTask(t);
        public static ValueTask<T> AsValueTask<T>(this Task<T> t) => new ValueTask<T>(t);
        public static ValueTask<T> AsValueTask<T>(this T t) => new ValueTask<T>(t);
    }
}
