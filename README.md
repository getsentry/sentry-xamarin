<p align="center">
  <a href="https://sentry.io" target="_blank" align="center">
    <img src="https://sentry-brand.storage.googleapis.com/sentry-logo-black.png" width="280">
  </a>
  <br />
</p>
 
Sentry SDK for Xamarin
===========

This is a work in progress SDK for Xamarin.

Includes for all Platforms supported by Xamarin Essentials:
* Automatic Navigation breacrumbs.
* Xaml warnings as breadcrumbs.
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
<p align="center">
  <b>BEFORE</b>
  
  <img src=".github/before_01.png"/>
</p>
<p align="center">
  <b>AFTER</b>
  
  <img src=".github/after_01.png"/>
</p>

## Setup
All you need to do is to Add the Xamarin integration to SentryOptions and it's recommended to start the Sentry SDK as early as possible, for an example, the start of OnCreate on MainActivity for Android, and , the top of FinishedLaunching on AppDelegate for iOS)

```C#
SentrySdk.Init(o =>
{
    o.Dsn = new Dsn("yourdsn");
    o.AddIntegration(new SentryXamarinFormsIntegration());
});

```

## Limitations

There are no line numbers on stack traces in release builds, and, mono symbolication is not yet supported.

## Resources

* [![Documentation](https://img.shields.io/badge/documentation-sentry.io-green.svg)](https://docs.sentry.io/platforms/dotnet/)
* [![Forum](https://img.shields.io/badge/forum-sentry-green.svg)](https://forum.sentry.io/c/sdks)
* [![Discord](https://img.shields.io/discord/621778831602221064)](https://discord.gg/Ww9hbqr)
* [![Stack Overflow](https://img.shields.io/badge/stack%20overflow-sentry-green.svg)](http://stackoverflow.com/questions/tagged/sentry)
* [![Twitter Follow](https://img.shields.io/twitter/follow/getsentry?label=getsentry&style=social)](https://twitter.com/intent/follow?screen_name=getsentry)
