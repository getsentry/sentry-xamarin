using Xamarin.Forms;

namespace Sentry.Xamarin.Forms.Extensions
{
    internal static class PageExtensions
    {
        internal static Page GetPreviousPage(this Page page)
            => page?.Parent is NavigationPage navigationPage ? 
                navigationPage.CurrentPage : page?.Parent is Page previousPage ? 
                    previousPage : null;
    }
}
