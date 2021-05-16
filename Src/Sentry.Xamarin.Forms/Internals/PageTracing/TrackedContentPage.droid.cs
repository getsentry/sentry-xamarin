using Sentry.Internals.PageTracing;
using Sentry.Xamarin.Forms.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ContentPage), typeof(TrackedContentPage))]

namespace Sentry.Internals.PageTracing
{
    internal class TrackedContentPage : PageRenderer
    {
        private static ITransaction pageLoading { get; set; }

        [System.Obsolete]
        public TrackedContentPage()
        {

        }

        public TrackedContentPage(Android.Content.Context context) : base(AttachTracingToContxt(context))
        {
        }

        public static Android.Content.Context AttachTracingToContxt(global::Android.Content.Context context)
        {
            pageLoading = SentrySdk.StartTransaction(null, "page.loading");
            return context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            pageLoading.Name = e.NewElement.GetPageType().Name;
            base.OnElementChanged(e);
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            pageLoading.Finish(SpanStatus.Ok);
        }
    }
}
