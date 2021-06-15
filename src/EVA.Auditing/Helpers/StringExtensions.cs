using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EVA.Auditing.Helpers
{
    public static class StringExtensions
    {
        private static readonly Regex _camelCaseRegex = new("(^[a-z]+|[A-Z]+(?![a-z])|[A-Z][a-z]+)");
        private static readonly Regex _newLineRegex = new(@"(\r?\n)", RegexOptions.Singleline);
        private static readonly Regex _wordDelimiters = new(@"[\r\f\n\t\s\v\u2014\u2013_,.:;\+=]", RegexOptions.Compiled);
        private static readonly Regex _invalidChars = new(@"[!@#$%\^&*\(\)/\\\[\]\{\}'""\<\>\?]", RegexOptions.Compiled);
        private static readonly Regex _multipleHyphens = new(@"-{2,}", RegexOptions.Compiled);
        private static readonly Regex _base64Regex = new(@"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.Compiled);
        private static readonly CultureInfo _englishCulture = new("en-US");

        #region Checks

        [DebuggerStepThrough]
        public static bool IsNotNullAndNotEmpty(this string s)
        {
            return !string.IsNullOrEmpty(s);
        }

        [DebuggerStepThrough]
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        [DebuggerStepThrough]
        public static bool IsNotNullAndNotWhiteSpace(this string s)
        {
            return !string.IsNullOrWhiteSpace(s);
        }

        [DebuggerStepThrough]
        public static bool IsNullOrWhiteSpace(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        [DebuggerStepThrough]
        public static bool Contains(this IEnumerable<string> values, string value, StringComparison comparison)
        {
            return values.Any(s => s.Equals(value, comparison));
        }

        #endregion

        #region Modifications

        [DebuggerStepThrough]
        public static string DefaultIfNullOrEmpty(this string s, string defaultValue = null)
        {
            return s.IsNullOrEmpty() ? defaultValue : s;
        }

        [DebuggerStepThrough]
        public static string DefaultIfNullOrWhiteSpace(this string s, string defaultValue = null)
        {
            return s.IsNullOrWhiteSpace() ? defaultValue : s;
        }

        [DebuggerStepThrough]
        public static string EmptyIfNull(this string s)
        {
            return s ?? string.Empty;
        }

        [DebuggerStepThrough]
        public static string FormatWith(this string s, params object[] args)
        {
            return string.Format(s, args);
        }

        [DebuggerStepThrough]
        public static string? ClampLength(this string s, int maxLength)
        {
            if (s == null) return string.Empty;
            if (s.Length <= maxLength) return s;

            return s.Substring(0, maxLength);
        }

        [DebuggerStepThrough]
        public static string Ellipsize(this string s, int maxLength)
        {
            if (s == null) return string.Empty;
            if (s.Length <= maxLength) return s;

            return s.Substring(0, maxLength - 1) + "...";
        }

        [DebuggerStepThrough]
        public static string ReplaceNewLinesWith(this string s, string replacement)
        {
            return s.IsNullOrWhiteSpace() ? string.Empty : _newLineRegex.Replace(s, replacement);
        }

        [DebuggerStepThrough]
        public static IEnumerable<string> Wrap(this string s, int length)
        {
            for (int i = 0; i < s.Length; i += length)
            {
                yield return s.Substring(i, Math.Min(length, s.Length - i));
            }
        }

        [DebuggerStepThrough]
        public static IEnumerable<string> WordWrap(this string s, int length)
        {
            var sentence = new List<string>();

            foreach (var word in s.Split(' '))
            {
                if (sentence.Any() && (sentence.Sum(x => x.Length + 1) + word.Length) > length)
                {
                    yield return string.Join(" ", sentence).Trim();
                    sentence.Clear();
                }

                sentence.Add(word);
            }

            if (sentence.Any()) yield return string.Join(" ", sentence).Trim();
        }

        [DebuggerStepThrough]
        public static string ToCamelCase(this string s)
        {
            if (s.IsNullOrEmpty()) return string.Empty;
            if (s.Length <= 1) return s;

            return string.Concat(char.ToLowerInvariant(s[0]), s[1..]);
        }

        [DebuggerStepThrough]
        public static string FromCamelCase(this string s)
        {
            if (s.IsNullOrWhiteSpace()) return string.Empty;

            return _camelCaseRegex.Matches(s).Select(x => x.Value).JoinBy(" ").ToLower().UppercaseFirst();
        }

        [DebuggerStepThrough]
        public static string UppercaseFirst(this string source)
        {
            return string.IsNullOrEmpty(source) ? string.Empty : char.ToUpper(source[0]) + source[1..];
        }

        [DebuggerStepThrough]
        public static string JoinBy(this IEnumerable<string> values, string separator, bool removeEmptyEntries = false)
        {
            return string.Join(separator, removeEmptyEntries ? values.Where(x => x.IsNotNullAndNotEmpty()) : values);
        }

        [DebuggerStepThrough]
        public static string JoinBy<T>(this IEnumerable<T> values, string separator, bool removeNullEntries = false)
        {
            return string.Join(separator, removeNullEntries ? values.Where(x => x != null) : values);
        }

        [DebuggerStepThrough]
        public static string ThrowIfEmpty(this string s, string field, string message = null)
        {
            return s.IsNotNullAndNotWhiteSpace() ? s : throw new ArgumentNullException(field, message);
        }

        [DebuggerStepThrough]
        public static string NullIfEmpty(this string s)
        {
            return s.IsNullOrEmpty() ? null : s;
        }

        [DebuggerStepThrough]
        public static string UrlEncode(this string s)
        {
            return Uri.EscapeDataString(s);
        }

        [DebuggerStepThrough]
        public static string DoubleUrlEncode(this string s)
        {
            return s.UrlEncode().UrlEncode();
        }

        [DebuggerStepThrough]
        public static string Mask(this string s)
        {
            if (s.IsNullOrEmpty()) return "****";
            if (s.Length <= 4) return "****";

            return s[0..^4] + "****";
        }

        [DebuggerStepThrough]
        public static async Task<string> ReplaceAsync(this Regex regex, string input, Func<Match, Task<string>> evaluator)
        {
            if (evaluator == null) throw new ArgumentNullException(nameof(evaluator));

            var count = input.Length;
            var match = regex.Match(input, 0);

            if (!match.Success) return input;

            var sb = new StringBuilder(16);
            var startIndex = 0;

            do
            {
                if (match.Index != startIndex)
                {
                    sb.Append(input, startIndex, match.Index - startIndex);
                }

                startIndex = match.Index + match.Length;
                sb.Append(await evaluator(match));

                if (--count == 0) break;

                match = match.NextMatch();

            } while (match.Success);

            if (startIndex < input.Length)
            {
                sb.Append(input, startIndex, input.Length - startIndex);
            }

            return sb.ToString();
        }

        [DebuggerStepThrough]
        public static string ToLowerButMoreBetterOptimized(this string s)
        {
            if (s.IsNullOrEmpty()) return string.Empty;

            foreach (var c in s) if (!char.IsLower(c)) return s.ToLower();
            return s;
        }

        [DebuggerStepThrough]
        public static string ToUpperButMoreBetterOptimized(this string s)
        {
            if (s.IsNullOrEmpty()) return string.Empty;

            foreach (var c in s) if (!char.IsUpper(c)) return s.ToUpper();
            return s;
        }

        #endregion

        #region Operations

        [DebuggerStepThrough]
        public static int ConvertToInt(this string s, int defaultValue = 0)
        {
            return !int.TryParse(s, out var val) ? defaultValue : val;
        }

        [DebuggerStepThrough]
        public static int? TryConvertToInt(this string s)
        {
            if (s == null) return null;
            if (!int.TryParse(s, out var val)) return null;

            return val;
        }

        [DebuggerStepThrough]
        public static decimal? TryConvertToDecimal(this string s, NumberFormatInfo numInfo = null)
        {
            if (s == null) return null;
            if (numInfo == null) s = s.Replace(",", ".");
            if (numInfo == null) numInfo = _englishCulture.NumberFormat;

            return decimal.TryParse(s, NumberStyles.AllowDecimalPoint, numInfo, out var val) ? val : null;
        }

        [DebuggerStepThrough]
        public static long? TryConvertToLong(this string s)
        {
            if (s == null) return null;
            if (!long.TryParse(s, out var val)) return null;

            return val;
        }

        [DebuggerStepThrough]
        public static bool? TryConvertToBool(this string s)
        {
            if (s == null) return null;
            if (!bool.TryParse(s, out var val)) return s switch { "1" => true, "0" => false, _ => default };

            return val;
        }

        [DebuggerStepThrough]
        public static DateTime? TryConvertToDateTime(this string s)
        {
            return s.IsNotNullAndNotEmpty() && DateTime.TryParse(s, out var datetime) ? datetime : null;
        }

        [DebuggerStepThrough]
        public static byte[] ToBytes(this string hex)
        {
            return Enumerable.Range(0, hex.Length).Where(x => 0 == x % 2).Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
        }

        [DebuggerStepThrough]
        public static string FromBytes(byte[] bytes)
        {
            var length = bytes.Length * 2;
            var chars = length < 100 ? stackalloc char[length] : new char[length];

            static char GetHexValue(int i) => (char)((i < 10) ? i + '0' : i - 10 + 'A');

            int index = 0;
            for (var i = 0; i < length; i += 2)
            {
                var b = bytes[index++];

                chars[i] = GetHexValue(b / 16);
                chars[i + 1] = GetHexValue(b % 16);
            }

            return new string(chars);
        }

        [DebuggerStepThrough]
        public static string GenerateSlug(this string s, char whitespaceReplacement = '-', int maxLength = 45)
        {
            if (s.IsNullOrEmpty()) return "";

            var normalized = WebUtility.HtmlDecode(s).Normalize(NormalizationForm.FormD);
            var previousWasWs = false;
            var j = 0;
            var output = new char[s.Length];

            static bool IsBasicLetter(char c) => (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9');

            for (var i = 0; i < normalized.Length; i++)
            {
                var c = normalized[i];
                var category = char.GetUnicodeCategory(c);

                switch (category)
                {
                    case UnicodeCategory.LowercaseLetter:
                    case UnicodeCategory.DecimalDigitNumber:

                        if (IsBasicLetter(c))
                        {
                            output[j++] = c;
                        }

                        previousWasWs = false;
                        break;

                    case UnicodeCategory.UppercaseLetter:

                        if (IsBasicLetter(c))
                        {
                            output[j++] = char.ToLowerInvariant(c);
                        }

                        previousWasWs = false;
                        break;

                    case UnicodeCategory.SpaceSeparator:
                    case UnicodeCategory.Control:

                        if (!previousWasWs)
                        {
                            output[j++] = whitespaceReplacement;
                            previousWasWs = true;
                        }

                        break;

                    default:

                        if (c == whitespaceReplacement && !previousWasWs)
                        {
                            output[j++] = whitespaceReplacement;
                            previousWasWs = true;
                        }

                        break;
                }
            }

            return new string(output, 0, (previousWasWs ? j - 1 : j).NoMoreThan(maxLength));
        }

        [DebuggerStepThrough]
        public static string GenerateSlugUnicode(this string s, char whitespaceReplacement = '-', int maxLength = 128)
        {
            s = s.ToLowerInvariant();
            s = _wordDelimiters.Replace(s, whitespaceReplacement.ToString());
            s = _invalidChars.Replace(s, string.Empty);
            s = _multipleHyphens.Replace(s, whitespaceReplacement.ToString());

            return s.Trim(whitespaceReplacement).ClampLength(maxLength);
        }

        [DebuggerStepThrough]
        public static string Hash(this string s, int length = 4)
        {
            var hash = SHA256.Create().ComputeHash(Encoding.Unicode.GetBytes(s));
            var code = new StringBuilder();

            foreach (var b in hash.Take(length))
            {
                int x = b;
                while (x > 0)
                {
                    var remainder = x % 26;
                    x /= 26;
                    code.Insert(0, (char)((char)remainder + 'A'));
                }
            }

            return code.ToString();
        }



        [DebuggerStepThrough]
        public static string TryGetCurrencySymbol(this string s)
        {
            foreach (var culture in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                try
                {
                    var region = new RegionInfo(culture.LCID);

                    if (region.ISOCurrencySymbol == s)
                    {
                        return region.CurrencySymbol;
                    }
                }
                catch
                {
                    // ignored
                }
            }

            return s;
        }

        [DebuggerStepThrough]
        public static decimal LevenshteinPercentage(this string a, string b)
        {
            var len = (decimal)Math.Max(a.Length, b?.Length ?? 0);

            return (len - a.Levenshtein(b)) / len;
        }

        [DebuggerStepThrough]
        public static int Levenshtein(this string a, string b)
        {
            if (a.IsNullOrEmpty()) return b.IsNullOrEmpty() ? 0 : b.Length;
            if (b.IsNullOrEmpty()) return a.IsNullOrEmpty() ? 0 : a.Length;

            var d = new int[a.Length + 1, b.Length + 1];

            for (var i = 0; i <= d.GetUpperBound(0); i += 1) d[i, 0] = i;
            for (var i = 0; i <= d.GetUpperBound(1); i += 1) d[0, i] = i;

            for (var i = 1; i <= d.GetUpperBound(0); i += 1)
            {
                for (var j = 1; j <= d.GetUpperBound(1); j += 1)
                {
                    var cost = Convert.ToInt32(a[i - 1] != b[j - 1]);

                    var min1 = d[i - 1, j] + 1;
                    var min2 = d[i, j - 1] + 1;
                    var min3 = d[i - 1, j - 1] + cost;

                    d[i, j] = Math.Min(Math.Min(min1, min2), min3);
                }
            }

            return d[d.GetUpperBound(0), d.GetUpperBound(1)];
        }

        #endregion

        #region Base64

        [DebuggerStepThrough]
        public static string AsBase64(this string s, Encoding encoding = null)
        {
            return Convert.ToBase64String((encoding ?? Encoding.ASCII).GetBytes(s));
        }

        [DebuggerStepThrough]
        public static string AsUrlSafeBase64(this string s, Encoding encoding = null)
        {
            return s.AsBase64(encoding).AsUrlSafe();
        }

        [DebuggerStepThrough]
        public static string AsUrlSafe(this string s)
        {
            return s.Replace('+', '-').Replace('/', '_').Trim('=');
        }

        [DebuggerStepThrough]
        public static string FromBase64(this string s, Encoding encoding = null)
        {
            return (encoding ?? Encoding.ASCII).GetString(Convert.FromBase64String(s));
        }

        [DebuggerStepThrough]
        public static string FromUrlSafeBase64(this string s, Encoding encoding = null)
        {
            return s.FromUrlSafe().FromBase64(encoding);
        }

        [DebuggerStepThrough]
        public static string FromUrlSafe(this string s)
        {
            return s.Replace('_', '/').Replace('-', '+').PadRight(s.Length + (s.Length % 4 == 2 ? 2 : s.Length % 4 == 3 ? 1 : 0), '=');
        }

        [DebuggerStepThrough]
        public static bool IsBase64String(this string s)
        {
            if (s.IsNullOrEmpty()) return false;

            s = s.Trim();
            return (s.Length % 4 == 0) && _base64Regex.IsMatch(s);
        }

        #endregion

        #region Convertions

        [DebuggerStepThrough]
        public static T As<T>(this string value, Func<string, T> converter = null)
        {
            if (value == null) return default;
            if (converter != null) return converter(value);

            return (T)value.As(typeof(T));
        }

        [DebuggerStepThrough]
        public static object As(this string value, Type type)
        {
            type = type.GetUnderlyingType();

            if (value == null) return default;
            if (type.IsEnum) return Enum.Parse(type, value, true);
            if (type == typeof(TimeSpan)) return TimeSpan.Parse(value);
            if (type == typeof(int[])) return value.AsCommaSeparatedIntegerArray();
            if (type == typeof(string[])) return value.AsCommaSeparatedStringArray();
            if (type == typeof(List<string>)) return value.AsCommaSeparatedStringArray().ToList();
            if (type == typeof(List<int>)) return value.AsCommaSeparatedIntegerArray().ToList();
            if (type == typeof(Guid)) return Guid.Parse(value);
            if (type.IsAssignableFrom(typeof(Dictionary<string, object>))) return JObject.Parse(value).ToObject<Dictionary<string, object>>();

            return Convert.ChangeType(value, type);
        }

        [DebuggerStepThrough]
        public static int[] AsCommaSeparatedIntegerArray(this string value)
        {
            return value.Split(',', ';').Select(v => int.TryParse(v, out int val) ? val : (int?)null).Where(i => i.HasValue).Select(i => i.Value).ToArray();
        }

        [DebuggerStepThrough]
        public static string[] AsCommaSeparatedStringArray(this string value)
        {
            return value.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
        }

        [DebuggerStepThrough]
        public static string[] AsNewLineSeparatedStringArray(this string value)
        {
            return value.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
        }

        #endregion
    }
}
