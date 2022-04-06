using Sentry.Xamarin.Forms.Extensions;
using Sentry.Xamarin.Internals;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Sentry.Xamarin.Forms.Internals
{
    internal class FormsNavigationIntegration : IPageNavigationTracker
    {
        public string CurrentPage { get; private set; }

        private IHub _hub;
        private volatile bool _disabled;

        /// <summary>
        /// Registers the Page appearing and disappearing event to the given application..
        /// </summary>
        /// <param name="application">The Xamarin Application.</param>
        internal void ApplySentryNavigationEvents(Application application)
        {
            if (!_disabled)
            {
                application.PageAppearing += Current_PageAppearing;
                application.PageDisappearing += Current_PageDisappearing;
            }
        }

        public void Register(IHub hub, SentryOptions _) => _hub = hub;

        /// <summary>
        /// Removes the PageDissapearing and PageAppearing events from the main Application, if found.
        /// </summary>
        public void Unregister()
        {
            if (!_disabled)
            {
                _disabled = true;
                if (Application.Current is not null)
                {
                    Application.Current.PageAppearing -= Current_PageAppearing;
                    Application.Current.PageDisappearing -= Current_PageDisappearing;
                }
            }
        }

        private void Current_PageDisappearing(object sender, Page e)
        {
            var type = e.GetPageType();
            if (type.BaseType.StartsWith("PopupPage"))
            {
                _hub?.AddBreadcrumb(null,
                    "ui.lifecycle",
                    "navigation",
                    new Dictionary<string, string>
                    {
                        ["popup"] = type.Name,
                        ["state"] = "disappearing"
                    }, level: BreadcrumbLevel.Info);
            }
        }

        private void Current_PageAppearing(object sender, Page e)
        {
            var pageType = e.GetPageType();
            if (CurrentPage != null && CurrentPage != pageType.Name)
            {
                if (pageType.Name is "NavigationPage")
                {
                    return;
                }
                if (pageType.BaseType is "PopupPage")
                {
                    _hub?.AddBreadcrumb(null,
                        "ui.lifecycle",
                        "navigation",
                        new Dictionary<string, string>
                        {
                            ["popup"] = pageType.Name,
                            ["state"] = "appearing"
                        }, level: BreadcrumbLevel.Info);
                    return;
                }
                else
                {
                    _hub?.AddBreadcrumb(null,
                        "navigation",
                        "navigation",
                        new Dictionary<string, string> { { "from", $"/{CurrentPage}" }, { "to", $"/{pageType.Name}" } });
                }
            }
            CurrentPage = pageType.Name;
        }
    }
}
