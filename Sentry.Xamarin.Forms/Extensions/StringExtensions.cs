namespace Sentry.Xamarin.Forms.Extensions
{
    internal static class StringExtensions
    {
        internal static string FilterUnknownOrEmpty(this string @string)
            => string.IsNullOrEmpty(@string) || @string == "unknown" ? null : @string;
    }
}
