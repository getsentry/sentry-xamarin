[![Sentry](https://raw.githubusercontent.com/getsentry/sentry-xamarin/main/.assets/sentry-wordmark-dark-280x84.png)](https://sentry.io/?utm_source=github&utm_medium=logo)

_Bad software is everywhere, and we're tired of it. Sentry is on a mission to help developers write better software faster, so we can get back to enjoying technology. If you want to join us, [**check out our open positions**](https://sentry.io/careers/)._
 
Sentry SDK for Xamarin
===========

[![build](https://github.com/getsentry/sentry-dotnet-xamarin/workflows/build/badge.svg?branch=main)](https://github.com/getsentry/sentry-dotnet-xamarin/actions?query=branch%3Amain)
[![Discord Chat](https://img.shields.io/discord/621778831602221064?logo=discord&logoColor=ffffff&color=7389D8)](https://discord.gg/PXa5Apfe7K)  

|      Integrations             |    Downloads     |    NuGet Stable     |    NuGet Preview     |
| ----------------------------- | :-------------------: | :-------------------: | :-------------------: |
|  **Sentry.Xamarin.Forms**     | [![Downloads](https://img.shields.io/nuget/dt/Sentry.Xamarin.Forms.svg)](https://www.nuget.org/packages/Sentry.Xamarin.Forms) | [![NuGet](https://img.shields.io/nuget/v/Sentry.Xamarin.Forms.svg)](https://www.nuget.org/packages/Sentry.Xamarin.Forms)   |    [![NuGet](https://img.shields.io/nuget/vpre/Sentry.Xamarin.Forms.svg)](https://www.nuget.org/packages/Sentry.Xamarin.Forms)   |
|  **Sentry.Xamarin**     | [![Downloads](https://img.shields.io/nuget/dt/Sentry.Xamarin.svg)](https://www.nuget.org/packages/Sentry.Xamarin) | [![NuGet](https://img.shields.io/nuget/v/Sentry.Xamarin.svg)](https://www.nuget.org/packages/Sentry.Xamarin)   |    [![NuGet](https://img.shields.io/nuget/vpre/Sentry.Xamarin.svg)](https://www.nuget.org/packages/Sentry.Xamarin)   |

Sentry.Xamarin is the tool to go for Xamarin that enriches your errors with valuable data, like breadcrumbs, tags, issue grouping and more!.

Includes for all Platforms supported by Xamarin Essentials:
* Automatic Navigation breacrumbs. (Xamarin.Forms)
* Xaml warnings as breadcrumbs. (Xamarin.Forms)
* Simulator flag.
* Device manufacturer.
* Device model.
* Operational system name and version.
* Screen information (Pixel density and resolution).
* Connectivity status.

Additionaly, Android and IOS will include additional information:
* Free Internal memory (Android/iOS).
* Total RAM (Android/iOS).
* CPU model (Android).

### BEFORE
![Screenshot of Sentry before applying Sentry.Xamarin](https://github.com/getsentry/sentry-xamarin/raw/main/.github/before_01.png)

### AFTER
![Screenshot of Sentry after applying Sentry.Xamarin](https://github.com/getsentry/sentry-xamarin/raw/main/.github/after_01.png)

## Setup
All you need to do is to initialize Xamarin integration by calling SentryXamarin.Init, and, it's recommended to start Sentry Xamarin SDK as early as possible, for an example, the start of OnCreate on MainActivity for Android, and, the top of FinishedLaunching on AppDelegate for iOS)

```C#
SentryXamarin.Init(options =>
{
    options.Dsn = "__YOUR__DSN__";
    options.AddXamarinFormsIntegration();
});

```

If your app uses Xamarin.Forms you can also use the package Sentry.Xamarin.Forms and initialize the SDK with additional line `options.AddXamarinFormsIntegration()`.

### Android
On your MainActivity
```C#
public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        SentryXamarin.Init(options =>
        {
            options.Dsn = "__YOUR__DSN__";
            options.AddXamarinFormsIntegration();
        });
        ...
```

### iOS
On AppDelegate.cs
```C#
public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
{
    public override bool FinishedLaunching(UIApplication app, NSDictionary options)
    {
        SentryXamarin.Init(options =>
        {
            options.Dsn = "__YOUR__DSN__";
            options.AddXamarinFormsIntegration();
        });
        ...
```

### UWP
On App.Xaml.cs
```C#
sealed partial class App : Application
{
    protected override void OnLaunched(LaunchActivatedEventArgs e)
    {
        SentryXamarin.Init(options =>
        {
            options.Dsn = "__YOUR__DSN__";
            options.AddXamarinFormsIntegration();
        });
    ...        
```

## Compatibility

The package requires the following versions or newer:

* Tizen 4.0 (for Tizen)
* Xamarin.Android 9.0 (for Android)
* Xamarin.iOS 10.14 (for iOS)
* Universal Windows Platform 10.0.16299 (for UWP)
* Xamarin.Forms 4.6.0.726 (for Xamarin.Forms integration)
* Xamarin.Essentials 1.4.0
* Sentry 3.0.0


## Limitations

There are no line numbers on stack traces for UWP and in release builds for Android and iOS, furthermore, mono symbolication is not yet supported.

## Resources

* [![Documentation](https://img.shields.io/badge/documentation-sentry.io-green.svg)](https://docs.sentry.io/platforms/dotnet/)
* [![Forum](https://img.shields.io/badge/forum-sentry-green.svg)](https://forum.sentry.io/c/sdks)
* [![Discord Chat](https://img.shields.io/discord/621778831602221064?logo=discord&logoColor=ffffff&color=7389D8)](https://discord.gg/PXa5Apfe7K)  
* [![Stack Overflow](https://img.shields.io/badge/stack%20overflow-sentry-green.svg)](http://stackoverflow.com/questions/tagged/sentry)
* [![Twitter Follow](https://img.shields.io/twitter/follow/getsentry?label=getsentry&style=social)](https://twitter.com/intent/follow?screen_name=getsentry)
