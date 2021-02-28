using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Sentry.Ovenlibrary;
using Sample.Xamarin.Core.Interfaces;
using Sample.Xamarin.Droid.Dependencies;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

[assembly: Dependency(typeof(NativeCrash))]
namespace Sample.Xamarin.Droid.Dependencies
{
    public class NativeCrash : INativeCrash
    {
        public void BrokenNativeCallback()
        {
            var oven = new Oven();
            //Cook is a JAVA opcode that executes an external C function that will crash the app.
            oven.Cook();
        }
    }
}