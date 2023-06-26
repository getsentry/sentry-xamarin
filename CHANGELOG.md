# Changelog

## Unreleased

### Dependencies

- Sentry .NET 3.33.1 ([#139](https://github.com/getsentry/sentry-xamarin/pull/139))

## 1.5.1

### Fixes

  - Update Sentry.NET SDK to 3.27.1 ([#136](https://github.com/getsentry/sentry-xamarin/pull/136))
    - [changelog](https://github.com/getsentry/sentry-dotnet/blob/3.27.1/CHANGELOG.md)
    - [diff](https://github.com/getsentry/sentry-dotnet/compare/3.27.0...3.27.1)

## 1.5.0

### Features

  - Update Sentry.NET SDK to 3.27.0, which supports line numbers for Android ([#135](https://github.com/getsentry/sentry-xamarin/pull/135))
    - [changelog](https://github.com/getsentry/sentry-dotnet/blob/3.27.0/CHANGELOG.md)
    - [diff](https://github.com/getsentry/sentry-dotnet/compare/3.26.2...3.27.0)

## 1.4.6

### Fixes

  - Update Sentry.NET SDK to 3.26.2 ([#133](https://github.com/getsentry/sentry-xamarin/pull/133))
    - [changelog](https://github.com/getsentry/sentry-dotnet/blob/3.26.2/CHANGELOG.md)
    - [diff](https://github.com/getsentry/sentry-dotnet/compare/3.23.1...3.26.2)

## 1.4.5

### Fixes

- Avoid repeat initialization ([#130](https://github.com/getsentry/sentry-xamarin/pull/130))

## 1.4.4

### Fixes

  - Update Sentry.NET SDK to 3.23.1 ([#128](https://github.com/getsentry/sentry-xamarin/pull/128))
    - [changelog](https://github.com/getsentry/sentry-dotnet/blob/3.23.1/CHANGELOG.md)
    - [diff](https://github.com/getsentry/sentry-dotnet/compare/3.22.0...3.23.1)

## 1.4.3

### Fixes

  - Update Sentry.NET SDK to 3.22.0 ([#127](https://github.com/getsentry/sentry-xamarin/pull/127))
    - [changelog](https://github.com/getsentry/sentry-dotnet/blob/3.22.0/CHANGELOG.md)
   - [diff](https://github.com/getsentry/sentry-dotnet/compare/3.20.1...3.22.0)

## 1.4.2

### Fixes

  - Update Sentry.NET SDK to 3.20.1 ([#125](https://github.com/getsentry/sentry-xamarin/pull/125))
    - [changelog](https://github.com/getsentry/sentry-dotnet/blob/3.20.1/CHANGELOG.md)
    - [diff](https://github.com/getsentry/sentry-dotnet/compare/3.17.1...3.20.1)

## 1.4.1

### Fixes

- Adjust DSN for Android by dropping oProj.ingest from the URL. This works around the LetsEncrypt root certificate issue by hitting a different endpoint that doesn't use LetsEncrypt. ([#114](https://github.com/getsentry/sentry-xamarin/pull/114))
- Update Sentry.NET SDK to 3.17.1 ([#119](https://github.com/getsentry/sentry-xamarin/pull/119))
  - [changelog](https://github.com/getsentry/sentry-dotnet/blob/3.17.1/CHANGELOG.md)
  - [diff](https://github.com/getsentry/sentry-dotnet/compare/3.16.0...3.17.1)

## 1.4.0

### Features

- Add option extension: RemoveNavigationPageIntegration ([#108](https://github.com/getsentry/sentry-xamarin/pull/108))

### Fixes

- Fix 0 byte screenshot being sent to Sentry ([#111](https://github.com/getsentry/sentry-xamarin/pull/111))
- Ignore null data on Internal breadcrumbs ([#90](https://github.com/getsentry/sentry-xamarin/pull/90))
- Update Sentry.NET SDK to 3.16.0 ([#110](https://github.com/getsentry/sentry-xamarin/pull/110))
  - [changelog](https://github.com/getsentry/sentry-dotnet/blob/3.16.0/CHANGELOG.md)
  - [diff](https://github.com/getsentry/sentry-dotnet/compare/3.9.2...3.16.0)

## 1.3.2

### Various fixes & improvements

- changelog for dotnet sdk bump (9a06a0b7) by @bruno-garcia
- bump dotnet sdk 3.12.1 (#93) by @bruno-garcia
- Fix null data on breadcrumb. (#90) by @lucas-zimerman

## 1.3.1

### Fixes

- screenshot extension ([#81](https://github.com/getsentry/sentry-xamarin/pull/81))
- allow setting release to null to rely on the dotnet SDK release detection ([#85](https://github.com/getsentry/sentry-xamarin/pull/85))
- Update Sentry.NET SDK to 3.9.2 ([#84](https://github.com/getsentry/sentry-xamarin/pull/84))
  - [changelog](https://github.com/getsentry/sentry-dotnet/blob/3.9.2/CHANGELOG.md)
  - [diff](https://github.com/getsentry/sentry-dotnet/compare/3.9.1...3.9.2)

## 1.3.0

### Features

* Add Screenshot support (#76) (#80)
* Update Sentry.NET SDK to 3.9.1
  - [changelog](https://github.com/getsentry/sentry-dotnet/blob/3.9.1/CHANGELOG.md)
  - [diff](https://github.com/getsentry/sentry-dotnet/compare/3.8.2...3.9.1)

## 1.2.0

### Changes

* Add Session support to Android, iOS, UWP (#75) @lucas-zimerman
* Update Sentry.NET SDK to 3.8.2 (#75) @lucas-zimerman
  - [changelog](https://github.com/getsentry/sentry-dotnet/blob/main/CHANGELOG.md#3)
  - [diff](https://github.com/getsentry/sentry-dotnet/compare/3.5.0...3.8.2)

## 1.1.0

### Changes

* Add build number to release (#67) @lucas-zimerman
* Add filter to device breadcrumbs.(#66) @lucas-zimerman
* IsEnvironmentUser is not set to false by default (#73) @lucas-zimerman
* Changed the data from Device.Manufacturer Device.Brand (#70) @lucas-zimerman
* Update Sentry.NET SDK to 3.5.0 (#73) @lucas-zimerman
  - [changelog](https://github.com/getsentry/sentry-dotnet/blob/main/CHANGELOG.md#350)
  - [diff](https://github.com/getsentry/sentry-dotnet/compare/3.0.6...3.5.0)

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
