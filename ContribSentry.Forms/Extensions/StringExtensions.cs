namespace ContribSentry.Forms.Extensions
{
    internal static class StringExtensions
    {
        internal static string FilterUnknown(this string @string)
            => @string.Replace("unknown", null);
    }
}
