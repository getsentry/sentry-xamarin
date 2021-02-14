using Sentry.Internals;
using Xamarin.Forms;

namespace Sentry.Xamarin.Forms.Extensions
{
    internal static class PageExtensions
    {
        internal static PageInfo GetPageType(this Page page)
        {
            var type = page.GetType();
            if (type?.Name is null)
            {
                return new PageInfo()
                {
                    Name = page.Title
                };
            }
            return new PageInfo()
            {
                Name = type.Name,
                BaseType = type.BaseType?.Name
            };
        }
    }
}