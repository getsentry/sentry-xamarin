# Changelog

## Unreleased

### Sentry Self-hosted Compatibility

If you're using `sentry.io` this change does not affect you.
This SDK version is compatible with a self-hosted version of Sentry `22.12.0` or higher. If you are using an older version of [self-hosted Sentry](https://develop.sentry.dev/self-hosted/) (aka on-premise), you will need to [upgrade](https://develop.sentry.dev/self-hosted/releases/). 

### Significant change in behavior

- Transaction names for ASP.NET Core are now consistently named `HTTP-VERB /path` (e.g. `GET /home`). Previously, the leading forward slash was missing for some endpoints. ([#2808](https://github.com/getsentry/sentry-dotnet/pull/2808))
- Setting `SentryOptions.Dsn` to `null` now throws `ArgumentNullException` during initialization. ([#2655](https://github.com/getsentry/sentry-dotnet/pull/2655))
- Enable `CaptureFailedRequests` by default ([#2688](https://github.com/getsentry/sentry-dotnet/pull/2688))
- Added `Sentry` namespace to global usings when `ImplicitUsings` is enabled ([#3043](https://github.com/getsentry/sentry-dotnet/pull/3043))
If you have conflicts, you can opt out by adding the following to your `csproj`:
```
<PropertyGroup>
  <SentryImplicitUsings>false</SentryImplicitUsings>
</PropertyGroup>
```
- Transactions' spans are no longer automatically finished with the status `deadline_exceeded` by the transaction. This is now handled by the [Relay](https://github.com/getsentry/relay). 
  - Customers self hosting Sentry must use verion 22.12.0 or later ([#3013](https://github.com/getsentry/sentry-dotnet/pull/3013))
- The `User.IpAddress` is now set to `{{auto}}` by default, even when sendDefaultPII is disabled ([#2981](https://github.com/getsentry/sentry-dotnet/pull/2981))
  - The "Prevent Storing of IP Addresses" option in the "Security & Privacy" project settings on sentry.io can be used to control this instead
- The `DiagnosticLogger` signature for `LogWarning` changed to take the `exception` as the first parameter. That way it no longer gets mixed up with the TArgs. ([#2987](https://github.com/getsentry/sentry-dotnet/pull/2987))

### API breaking Changes

If you have compilation errors you can find the affected types or overloads missing in the changelog entries below.

#### Changed APIs

- Class renamed `Sentry.User` to `Sentry.SentryUser` ([#3015](https://github.com/getsentry/sentry-dotnet/pull/3015))
- Class renamed `Sentry.Runtime` to `Sentry.SentryRuntime` ([#3016](https://github.com/getsentry/sentry-dotnet/pull/3016))
- Class renamed `Sentry.Span` to `Sentry.SentrySpan` ([#3021](https://github.com/getsentry/sentry-dotnet/pull/3021))
- Class renamed `Sentry.Transaction` to `Sentry.SentryTransaction` ([#3023](https://github.com/getsentry/sentry-dotnet/pull/3023))
- `ITransaction` has been renamed to `ITransactionTracer`. You will need to update any references to these interfaces in your code to use the new interface names ([#2731](https://github.com/getsentry/sentry-dotnet/pull/2731), [#2870](https://github.com/getsentry/sentry-dotnet/pull/2870))
- `DebugImage` and `DebugMeta` moved to `Sentry.Protocol` namespace. ([#2815](https://github.com/getsentry/sentry-dotnet/pull/2815))
- `SentryClient.Dispose` is no longer obsolete ([#2842](https://github.com/getsentry/sentry-dotnet/pull/2842))
- `ISentryClient.CaptureEvent` overloads have been replaced by a single method accepting optional `Hint` and `Scope` parameters. You will need to pass `hint` as a named parameter from code that calls `CaptureEvent` without passing a `scope` argument. ([#2749](https://github.com/getsentry/sentry-dotnet/pull/2749))
- `TransactionContext` and `SpanContext` constructors were updated. If you're constructing instances of these classes, you will need to adjust the order in which you pass parameters to these. ([#2694](https://github.com/getsentry/sentry-dotnet/pull/2694), [#2696](https://github.com/getsentry/sentry-dotnet/pull/2696))
- The `DiagnosticLogger` signature for `LogError` and `LogFatal` changed to take the `exception` as the first parameter. That way it no longer gets mixed up with the TArgs. The `DiagnosticLogger` now also receives an overload for `LogError` and `LogFatal` that accepts a message only. ([#2715](https://github.com/getsentry/sentry-dotnet/pull/2715))
- `Distribution` added to `IEventLike`. ([#2660](https://github.com/getsentry/sentry-dotnet/pull/2660))
- `StackFrame`'s `ImageAddress`, `InstructionAddress`, and `FunctionId` changed to `long?`. ([#2691](https://github.com/getsentry/sentry-dotnet/pull/2691))
- `DebugImage.ImageAddress` changed to `long?`. ([#2725](https://github.com/getsentry/sentry-dotnet/pull/2725))
- Contexts now inherit from `IDictionary` rather than `ConcurrentDictionary`. The specific dictionary being used is an implementation detail. ([#2729](https://github.com/getsentry/sentry-dotnet/pull/2729))
- The method used to configure a Sentry Sink for Serilog now has an additional overload. Calling `WriteTo.Sentry()` with no arguments will no longer attempt to initialize the SDK (it has optional arguments to configure the behavior of the Sink only). If you want to initialize Sentry at the same time you configure the Sentry Sink then you will need to use the overload of this method that accepts a DSN as the first parameter (e.g. `WriteTo.Sentry("https://d4d82fc1c2c4032a83f3a29aa3a3aff@fake-sentry.io:65535/2147483647")`). ([#2928](https://github.com/getsentry/sentry-dotnet/pull/2928))

#### Removed APIs

- SentrySinkExtensions.ConfigureSentrySerilogOptions is now internal. If you were using this method, please use one of the `SentrySinkExtensions.Sentry` extension methods instead. ([#2902](https://github.com/getsentry/sentry-dotnet/pull/2902))
- A number of `[Obsolete]` options have been removed ([#2841](https://github.com/getsentry/sentry-dotnet/pull/2841))
  - `BeforeSend` - use `SetBeforeSend` instead.
  - `BeforeSendTransaction` - use `SetBeforeSendTransaction` instead.
  - `BeforeBreadcrumb` - use `SetBeforeBreadcrumb` instead.
  - `CreateHttpClientHandler` - use `CreateHttpMessageHandler` instead.
  - `ReportAssemblies` - use `ReportAssembliesMode` instead.
  - `KeepAggregateException` - this property is no longer used and has no replacement.
  - `DisableTaskUnobservedTaskExceptionCapture` method has been renamed to `DisableUnobservedTaskExceptionCapture`.
  - `DebugDiagnosticLogger` - use `TraceDiagnosticLogger` instead.
- A number of iOS/Android-specific `[Obsolete]` options have been removed ([#2856](https://github.com/getsentry/sentry-dotnet/pull/2856))
  - `Distribution` - use `SentryOptions.Distribution` instead.
  - `EnableAutoPerformanceTracking` - use `SetBeforeSendTransaction` instead.
  - `EnableCoreDataTracking` - use `EnableCoreDataTracing` instead.
  - `EnableFileIOTracking` - use `EnableFileIOTracing` instead.
  - `EnableOutOfMemoryTracking` - use `EnableWatchdogTerminationTracking` instead.
  - `EnableUIViewControllerTracking` - use `EnableUIViewControllerTracing` instead.
  - `StitchAsyncCode` - no longer available.
  - `ProfilingTracesInterval` - no longer available.
  - `ProfilingEnabled` - use `ProfilesSampleRate` instead.
- Obsolete `SystemClock` constructor removed, use `SystemClock.Clock` instead. ([#2856](https://github.com/getsentry/sentry-dotnet/pull/2856))
- Obsolete `Runtime.Clone()` removed, this shouldn't have been public in the past and has no replacement. ([#2856](https://github.com/getsentry/sentry-dotnet/pull/2856))
- Obsolete `SentryException.Data` removed, use `SentryException.Mechanism.Data` instead. ([#2856](https://github.com/getsentry/sentry-dotnet/pull/2856))
- Obsolete `AssemblyExtensions` removed, this shouldn't have been public in the past and has no replacement. ([#2856](https://github.com/getsentry/sentry-dotnet/pull/2856))
- Obsolete `SentryDatabaseLogging.UseBreadcrumbs()` removed, it is called automatically and has no replacement. ([#2856](https://github.com/getsentry/sentry-dotnet/pull/2856))
- Obsolete `Scope.GetSpan()` removed, use `Span` property instead. ([#2856](https://github.com/getsentry/sentry-dotnet/pull/2856))
- Obsolete `IUserFactory` removed, use `ISentryUserFactory` instead. ([#2856](https://github.com/getsentry/sentry-dotnet/pull/2856), [#2840](https://github.com/getsentry/sentry-dotnet/pull/2840))
- `IHasMeasurements` has been removed, use `ISpanData` instead. ([#2659](https://github.com/getsentry/sentry-dotnet/pull/2659))
- `IHasBreadcrumbs` has been removed, use `IEventLike` instead. ([#2670](https://github.com/getsentry/sentry-dotnet/pull/2670))
- `ISpanContext` has been removed, use `ITraceContext` instead. ([#2668](https://github.com/getsentry/sentry-dotnet/pull/2668))
- `IHasTransactionNameSource` has been removed, use `ITransactionContext` instead. ([#2654](https://github.com/getsentry/sentry-dotnet/pull/2654))
- ([#2694](https://github.com/getsentry/sentry-dotnet/pull/2694))
- The unused `StackFrame.InstructionOffset` has been removed. ([#2691](https://github.com/getsentry/sentry-dotnet/pull/2691))
- The unused `Scope.Platform` property has been removed. ([#2695](https://github.com/getsentry/sentry-dotnet/pull/2695))
- The obsolete setter `Sentry.PlatformAbstractions.Runtime.Identifier` has been removed ([2764](https://github.com/getsentry/sentry-dotnet/pull/2764))
- `Sentry.Values<T>` is now internal as it is never exposed in the public API ([#2771](https://github.com/getsentry/sentry-dotnet/pull/2771))
- The `TracePropagationTarget` class has been removed, use the `SubstringOrRegexPattern` class instead. ([#2763](https://github.com/getsentry/sentry-dotnet/pull/2763))
- The `WithScope` and `WithScopeAsync` methods have been removed. We have discovered that these methods didn't work correctly in certain desktop contexts, especially when using a global scope. ([#2717](https://github.com/getsentry/sentry-dotnet/pull/2717))

  Replace your usage of `WithScope` with overloads of `Capture*` methods:

  - `SentrySdk.CaptureEvent(SentryEvent @event, Action<Scope> scopeCallback)`
  - `SentrySdk.CaptureMessage(string message, Action<Scope> scopeCallback)`
  - `SentrySdk.CaptureException(Exception exception, Action<Scope> scopeCallback)`

  ```c#
  // Before
  SentrySdk.WithScope(scope =>
  {
    scope.SetTag("key", "value");
    SentrySdk.CaptureEvent(new SentryEvent());
  });

  // After
  SentrySdk.CaptureEvent(new SentryEvent(), scope =>
  {
    // Configure your scope here
    scope.SetTag("key", "value");
  });
  ```

### Android breaking Changes

- Android minimum support increased to API 30  ([#2697](https://github.com/getsentry/sentry-dotnet/pull/2697))

### Features

- Added `Xamarin.Mac` support ([#138](https://github.com/getsentry/sentry-xamarin/pull/138))
- Experimental pre-release availability of Metrics. We're exploring the use of Metrics in Sentry. The API will very likely change and we don't yet have any documentation. ([#2949](https://github.com/getsentry/sentry-dotnet/pull/2949))
  - `SentrySdk.Metrics.Set` now additionally accepts `string` as value ([#3092](https://github.com/getsentry/sentry-dotnet/pull/3092))
  - Timing metrics can now be captured with `SentrySdk.Metrics.StartTimer` ([#3075](https://github.com/getsentry/sentry-dotnet/pull/3075))
  - Added support for capturing built-in metrics from the `System.Diagnostics.Metrics` API ([#3052](https://github.com/getsentry/sentry-dotnet/pull/3052))
- `Sentry.Profiling` is now available as a package on [nuget](nuget.org). Be aware that profiling is in alpha and on servers the overhead could be high. Improving the experience for ASP.NET Core is tracked on [this issue](
https://github.com/getsentry/sentry-dotnet/issues/2316) ([#2800](https://github.com/getsentry/sentry-dotnet/pull/2800))
- Support for [Spotlight](https://spotlightjs.com/), a debug tool for local development. ([#2961](https://github.com/getsentry/sentry-dotnet/pull/2961))
  - Enable it with the option `EnableSpotlight`
  - Optionally configure the URL to connect via `SpotlightUrl`. Defaults to `http://localhost:8969/stream`.

### Fixes

- Native integration logging on macOS ([#3079](https://github.com/getsentry/sentry-dotnet/pull/3079))
- The scope transaction is now correctly set for Otel transactions ([#3072](https://github.com/getsentry/sentry-dotnet/pull/3072))
- Fixed an issue with tag values in metrics not being properly serialized ([#3065](https://github.com/getsentry/sentry-dotnet/pull/3065))
- Moved the binding to MAUI events for breadcrumb creation from `WillFinishLaunching` to `FinishedLaunching`. This delays the initial instantiation of `app`. ([#3057](https://github.com/getsentry/sentry-dotnet/pull/3057))
- The SDK no longer adds the `WinUIUnhandledExceptionIntegration` on non-Windows platforms ([#3055](https://github.com/getsentry/sentry-dotnet/pull/3055))
- Stop Sentry for MacCatalyst from creating `default.profraw` in the app bundle using xcodebuild archive to build sentry-cocoa ([#2960](https://github.com/getsentry/sentry-dotnet/pull/2960))
- Workaround a .NET 8 NativeAOT crash on transaction finish. ([#2943](https://github.com/getsentry/sentry-dotnet/pull/2943))
- Reworked automatic breadcrumb creation for MAUI. ([#2900](https://github.com/getsentry/sentry-dotnet/pull/2900))
  - The SDK no longer uses reflection to bind to all public element events. This also fixes issues where the SDK would consume third-party events.
  - Added `CreateElementEventsBreadcrumbs` to the SentryMauiOptions to allow users to opt-in automatic breadcrumb creation for `BindingContextChanged`, `ChildAdded`, `ChildRemoved`, and `ParentChanged` on `Element`.
  - Reduced amount of automatic breadcrumbs by limiting the number of bindings created in `VisualElement`, `Window`, `Shell`, `Page`, and `Button`.
- Fixed Sentry SDK has not been initialized when using ASP.NET Core, Serilog, and OpenTelemetry ([#2911](https://github.com/getsentry/sentry-dotnet/pull/2911))
- Android native symbol upload ([#2876](https://github.com/getsentry/sentry-dotnet/pull/2876))
- `Sentry.Serilog` no longer throws if a disabled DSN is provided when initializing Sentry via the Serilog integration ([#2883](https://github.com/getsentry/sentry-dotnet/pull/2883))
- Don't add WinUI exception integration on mobile platforms ([#2821](https://github.com/getsentry/sentry-dotnet/pull/2821))
- `Transactions` are now getting enriched by the client instead of the hub ([#2838](https://github.com/getsentry/sentry-dotnet/pull/2838))
- Fixed an issue when using the SDK together with OpenTelemetry `1.5.0` and newer where the SDK would create transactions for itself. The fix is backward compatible. ([#3001](https://github.com/getsentry/sentry-dotnet/pull/3001))

### Dependencies

- Upgraded to NLog version 5. ([#2697](https://github.com/getsentry/sentry-dotnet/pull/2697))
- Bump Sentry.NET 4.0.3 ([#147](https://github.com/getsentry/sentry-xamarin/pull/147))

## 2.0.0-beta.1

### Features

- Added `Xamarin.Mac` support ([#138](https://github.com/getsentry/sentry-xamarin/pull/138))

### Fixes

  - Some iOS Native crashes are now properly captured. ([#145](https://github.com/getsentry/sentry-xamarin/pull/145))

### Dependencies

  - Update Sentry.NET SDK to 4.0.0-beta.4 ([#145](https://github.com/getsentry/sentry-xamarin/pull/145))
    - [changelog](https://github.com/getsentry/sentry-dotnet/blob/4.0.0-beta.4/CHANGELOG.md)
    - [diff](https://github.com/getsentry/sentry-dotnet/compare/3.33.1...4.0.0-beta.41)

### API breaking Changes

  For a complete list of break changes, please visit the Sentry .NET [changelog](https://raw.githubusercontent.com/getsentry/sentry-dotnet/4.0.0-beta.4/CHANGELOG.md).

## 1.5.2

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
