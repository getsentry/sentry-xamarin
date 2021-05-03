# Changelog

## Unchanged

### Changes

* Add filter to device breadcrumbs.(#66) @lucas-zimerman
* Update Sentry.NET SDK to 3.3.0 (#63) @lucas-zimerman

## 1.0.3

### Changes

* Fix Null reference on SentryXamarinFormsIntegration.Register. (#58) @lucas-zimerman
* Add android unhandled exception and native crash sample. (#56) @lucas-zimerman

## 1.0.2

### Changes

* Renamed DisableXamarinFormsCache to DisableOfflineCaching. (#50) @lucas-zimerman
* Update Sentry.NET SDK to 3.0.6. (#50) @lucas-zimerman
* Add available RAM parameter (Android). (#46) @lucas-zimerman

## 1.0.1

### Changes

A minor update to avoid breaking changes with the latest version of Sentry .NET SDK

* Update Sentry .NET SDK. (#45)

## 1.0.0

### Changes

This is the first GA release containing the following features:

General improvements

* Automatic Navigation breadcrumbs. (Forms)
* Automatic Xamarin Forms warnings as breadcrumbs.
* Unhandled Exception for Android/iOS/UWP
* Release version for Android/iOS/UWP.
* Additional InAppExclude list for Xamarin.
* iOS Exceptions including native and managed StackTrace.

Device information

* Manufacturer.
* Model.
* Connectivity status.
* Operational system name and version.
* Screen information (Pixel density and resolution).
* Free Internal memory (Android/iOS).
* Total RAM (Android/iOS).
* CPU model (Android).
* Simulator flag.
F
or more information on how to use the SDK, check the documentation at https://docs.sentry.io/platforms/dotnet/guides/xamarin/.


## 1.0.0-alpha.4

### Changes

* detach Xamarin.Forms dependency from the SDK. (#38 ) @lucas-zimerman 

If you plan to use Xamarin only you'll need the Sentry.Xamarin package. Additionally, you'll also need the Sentry.Xamarin.Forms if you are using Xamarin Forms.

Furthermore, for activating the Sentry.Xamarin.Forms you'll need to add the Xamarin Forms Integration inside of SentryXamarinOptions.

```csharp
        SentryXamarin.Init(options =>
        {
            options.Dsn = "__YOUR__DSN__";
            options.AddXamarinFormsIntegration();
        });
```

## 1.0.0-alpha.3

### Changes

* fixed os.name format.
* Sentry.NET SDK requirement increased.
* Add InAppExclude list for Xamarin.
* Initializer refactored.

## 1.0.0-alpha.2

### Changes

This is the first alpha containing the following features:

* Decreased package requirements.
* Added Release version for Android/iOS/UWP.

## 1.0.0-alpha.1

### Changes

This is the first alpha containing the following features:

* Automatic Navigation breadcrumbs.
* Automatic Xamarin warnings as breadcrumbs.
* Unhandled Exception for Android/iOS/UWP
* Simulator flag.
* Device manufacturer.
* Device model.
* Operational system name and version.
* Screen information (Pixel density and resolution).
* Connectivity status.
* Free Internal memory (Android/iOS).
* Total RAM (Android/iOS).
* CPU model (Android).

Also available via NuGet:

[Sentry](https://www.nuget.org/packages/Sentry/)
[Sentry.Xamarin](https://www.nuget.org/packages/Sentry.Xamarin)
[Sentry.Xamarin.Forms](https://www.nuget.org/packages/Sentry.Xamarin.Forms)
