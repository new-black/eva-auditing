using System;

namespace EVA.Auditing.Helpers
{
    public static class NumberExtensions
    {
        public static int AbsoluteValue(this int @this) => Math.Abs(@this);
        public static decimal AbsoluteValue(this decimal @this) => Math.Abs(@this);
        public static double AbsoluteValue(this double @this) => Math.Abs(@this);

        public static bool Between(this int @this, int x, int y) => @this >= x && @this <= y;
        public static bool Between(this int @this, decimal x, decimal y) => @this >= x && @this <= y;
        public static bool Between(this int @this, double x, double y) => @this >= x && @this <= y;

        public static bool Between(this decimal @this, int x, int y) => @this >= x && @this <= y;
        public static bool Between(this decimal @this, decimal x, decimal y) => @this >= x && @this <= y;

        public static bool Between(this double @this, int x, int y) => @this >= x && @this <= y;
        public static bool Between(this double @this, double x, double y) => @this >= x && @this <= y;

        public static decimal OneIfZero(this decimal @this) => @this == 0m ? 1m : @this;

        public static int NotNegative(this int @this) => AtLeast(@this, 0);
        public static decimal NotNegative(this decimal @this) => AtLeast(@this, 0);
        public static decimal NotPositive(this decimal @this) => NoMoreThan(@this, 0);

        /// <summary>
        /// Return the max value, but based on the absolute values
        /// 15 and 10 -> 10
        /// -15 and -10 -> -10
        /// -15 and 10 -> 10
        /// 15 and -10 -> -10
        /// </summary>
        public static decimal AbsoluteMin(this decimal @this, decimal value) => @this.AbsoluteValue() < value.AbsoluteValue() ? @this : value;

        public static double AtLeast(this double @this, double value) => Math.Max(value, @this);
        public static int AtLeast(this int @this, int value) => Math.Max(value, @this);
        public static int AtLeast(this int @this, int? value) => Math.Max(value ?? @this, @this);
        public static int AtLeast(this int? @this, int value) => Math.Max(@this ?? value, value);
        public static int? AtLeast(this int? @this, int? value)
        {
            if (@this == null && value == null) return null;
            return @this == null ? @this.AtLeast(value.Value) : @this.Value.AtLeast(value);
        }

        public static long AtLeast(this long @this, long value) => Math.Max(value, @this);
        public static long AtLeast(this long @this, long? value) => Math.Max(value ?? @this, @this);
        public static long AtLeast(this long? @this, long value) => Math.Max(@this ?? value, value);
        public static long? AtLeast(this long? @this, long? value)
        {
            if (@this == null && value == null) return null;
            return @this == null ? @this.AtLeast(value.Value) : @this.Value.AtLeast(value);
        }

        public static decimal AtLeast(this decimal @this, decimal value) => Math.Max(value, @this);
        public static decimal AtLeast(this decimal @this, decimal? value) => Math.Max(value ?? @this, @this);
        public static decimal AtLeast(this decimal? @this, decimal value) => Math.Max(@this ?? value, value);
        public static decimal? AtLeast(this decimal? @this, decimal? value)
        {
            if (@this == null && value == null) return null;
            return @this == null ? @this.AtLeast(value.Value) : @this.Value.AtLeast(value);
        }

        public static double NoMoreThan(this double @this, double value) => Math.Min(value, @this);
        public static int NoMoreThan(this int @this, int value) => Math.Min(value, @this);
        public static int NoMoreThan(this int @this, int? value) => Math.Min(value ?? @this, @this);
        public static int NoMoreThan(this int? @this, int value) => Math.Min(@this ?? value, value);
        public static int? NoMoreThan(this int? @this, int? value)
        {
            if (@this == null && value == null) return null;
            return @this == null ? @this.NoMoreThan(value.Value) : @this.Value.NoMoreThan(value);
        }

        public static long NoMoreThan(this long @this, long value) => Math.Min(value, @this);
        public static long NoMoreThan(this long @this, long? value) => Math.Min(value ?? @this, @this);
        public static long NoMoreThan(this long? @this, long value) => Math.Min(@this ?? value, value);
        public static long? NoMoreThan(this long? @this, long? value)
        {
            if (@this == null && value == null) return null;
            return @this == null ? @this.NoMoreThan(value.Value) : @this.Value.NoMoreThan(value);
        }

        public static decimal NoMoreThan(this decimal @this, decimal value) => Math.Min(value, @this);
        public static decimal NoMoreThan(this decimal @this, decimal? value) => Math.Min(value ?? @this, @this);
        public static decimal NoMoreThan(this decimal? @this, decimal value) => Math.Min(@this ?? value, value);
        public static decimal? NoMoreThan(this decimal? @this, decimal? value)
        {
            if (@this == null && value == null) return null;
            return @this == null ? @this.NoMoreThan(value.Value) : @this.Value.NoMoreThan(value);
        }

        public static decimal RoundOn(this decimal @this, int precision) => Math.Round(@this, precision);
        public static double RoundOn(this double @this, int precision) => Math.Round(@this, precision);

        /// <summary>
        /// Rounds the value up to the neerest whole precision
        /// 15.12 with precision of 2 will remain 15.12
        /// 15.12 with precision of 1 will become 15.2
        /// 15.18 with precision of 1 will become 15.2
        /// 15.121 with precision of 2 will become 15.13
        /// 15.128 with precision of 2 will become 15.13
        /// </summary>
        public static decimal RoundUp(this decimal @this, int precision)
        {
            var multiplier = (decimal)Math.Pow(10, Convert.ToDouble(precision));
            return Math.Ceiling(@this * multiplier) / multiplier;
        }

        /// <summary>
        /// Rounds the value up to the neerest whole precision
        /// 15.12 with precision of 2 will remain 15.12
        /// 15.12 with precision of 1 will become 15.2
        /// 15.18 with precision of 1 will become 15.2
        /// 15.121 with precision of 2 will become 15.13
        /// 15.128 with precision of 2 will become 15.13
        /// </summary>
        public static double RoundUp(this double @this, int precision)
        {
            var multiplier = Math.Pow(10, Convert.ToDouble(precision));
            return Math.Ceiling(@this * multiplier) / multiplier;
        }

        /// <summary>
        /// Trancates the decimal to the given precision
        /// 15.12 with precision of 2 will remain 15.12
        /// 15.12 with precision of 1 will become 15.1
        /// 15.18 with precision of 1 will become 15.1
        /// 15.121 with precision of 2 will become 15.12
        /// 15.128 with precision of 2 will become 15.12
        /// </summary>
        public static decimal TruncateOn(this decimal @d, byte precision)
        {
            decimal r = Math.Round(d, precision);

            return d switch
            {
                > 0 when r > d => r - new decimal(1, 0, 0, false, precision),
                < 0 when r < d => r + new decimal(1, 0, 0, false, precision),
                _ => r
            };
        }

        public static DateTime FromUnixTime(this long @this) => DateTimeOffset.FromUnixTimeSeconds(@this).UtcDateTime;
        public static DateTime FromUnixTimeMilliseconds(this long @this) => DateTimeOffset.FromUnixTimeMilliseconds(@this).UtcDateTime;

        public static bool IsDefault(this double @this) => Math.Abs(@this - default(double)) < double.Epsilon;
    }
}
