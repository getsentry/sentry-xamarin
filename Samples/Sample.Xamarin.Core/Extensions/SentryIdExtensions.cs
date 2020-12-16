using Sentry.Protocol;
using System;
//Based on https://github.com/csharpvitamins/CSharpVitamins.ShortGuid

namespace Sample.Xamarin.Core.Extensions
{
    public static class SentryIdExtensions
    {
        public static string GetShortId(this SentryId id)
        {
            if (id.Equals(SentryId.Empty))
            {
                return null;
            }
            string encoded = Convert.ToBase64String(((Guid)id).ToByteArray());

            encoded = encoded
                .Replace("/", "_")
                .Replace("+", "-");

            return encoded.Substring(0, 16);

        }
    }
}
