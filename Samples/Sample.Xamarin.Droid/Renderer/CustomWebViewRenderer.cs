using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Sample.Xamarin.Core.CustomControls;
using Sample.Xamarin.Droid.Renderer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Android.Webkit.WebSettings;

[assembly: ExportRenderer(typeof(CustomWebView), typeof(CustomWebViewRenderer))]
namespace Sample.Xamarin.Droid.Renderer
{
    public class CustomWebViewRenderer : WebViewRenderer
    {
        public CustomWebViewRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            Control.SetWebViewClient(new ExtendedWebViewClient(Element as CustomWebView));

        }

        class ExtendedWebViewClient : Android.Webkit.WebViewClient
        {
            private CustomWebView _xwebView { get; set; }
            public ExtendedWebViewClient(CustomWebView webView)
            {
                _xwebView = webView;
            }

            async public override void OnPageFinished(Android.Webkit.WebView view, string url)
            {
                if (_xwebView != null)
                {
                    _xwebView.HeightRequest = 0d;
                    await Task.Delay(100);
                    var result = await _xwebView.EvaluateJavaScriptAsync("(function(){return document.body.scrollHeight;})()");
                    Console.WriteLine(result);
                    var newResult = result;
                    for (int i = 0; i < 1000 && result == newResult; i++)
                    {
                        await Task.Delay(100);
                        newResult = await _xwebView.EvaluateJavaScriptAsync("(function(){return document.body.scrollHeight;})()");
                        Console.WriteLine(newResult);
                    }
                    _xwebView.HeightRequest = Convert.ToDouble(newResult);
                    view.SetInitialScale(40);

                }

                base.OnPageFinished(view, url);
            }
        }
    }
}
