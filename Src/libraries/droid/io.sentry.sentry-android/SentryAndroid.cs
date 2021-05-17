using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static IO.Sentry.Sentry;

namespace IO.Sentry.Android.Core
{
    public partial class SentryAndroid
    {
        private class Function1Impl<T> : Java.Lang.Object, IOptionsConfiguration where T : Java.Lang.Object
        {
            private readonly Action<T> OnInvoked;

            public Function1Impl(Action<T> onInvoked)
            {
                OnInvoked = onInvoked;
            }

            public void Configure(Java.Lang.Object p0)
            {
                Console.WriteLine(p0.Class.Name);
            }

            public Java.Lang.Object Invoke(Java.Lang.Object objParameter)
            {
                try
                {
                    T parameter = (T)objParameter;
                    OnInvoked?.Invoke(parameter);
                }
                catch (Exception ex)
                {
                    _ = ex;
                    // Exception handling, if needed
                }
                return null;
            }
        }

        public static void Init(Context context, Action<SentryOptions> options)
        {
            Init(context, new Function1Impl<SentryOptions>((response) =>
            {
                options?.Invoke(response);
            }));
        }
    }
}