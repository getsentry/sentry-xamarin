namespace Sentry.Xamarin.Forms.Extensions
{
    internal static partial class StringExtensions
    {
        internal static string FilterUnknown(this string @string)
            => @string.Replace("unknown", null);
    }
}
