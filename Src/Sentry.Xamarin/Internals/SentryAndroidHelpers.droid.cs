using System;
using System.Collections.Generic;
using System.IO;
using Sentry.Android.AssemblyReader;
using Sentry.Extensibility;

using Appliction = Android.App.Application;
using AndroidBuild = Android.OS.Build;

namespace Sentry.Xamarin.Internals
{
    internal static class SentryAndroidHelpers
    {
        public static IList<string> GetSupportedAbis()
        {
            var result = AndroidBuild.SupportedAbis;
            if (result != null)
            {
                return result;
            }

#pragma warning disable CS0618
            var abi = AndroidBuild.CpuAbi;
#pragma warning restore CS0618

            return abi != null ? new[] {abi} : Array.Empty<string>();
        }

        public static IAndroidAssemblyReader? GetAndroidAssemblyReader(IDiagnosticLogger? logger)
        {
            var apkPath = Appliction.Context.ApplicationInfo?.SourceDir;
            if (apkPath == null)
            {
                logger?.LogWarning("Can't determine APK path.");
                return null;
            }

            if (!File.Exists(apkPath))
            {
                logger?.LogWarning("APK doesn't exist at {0}", apkPath);
                return null;
            }

            try
            {
                var supportedAbis = GetSupportedAbis();
                return AndroidAssemblyReaderFactory.Open(apkPath, supportedAbis,
                    logger: (message, args) => logger?.Log(SentryLevel.Debug, message, args: args));
            }
            catch (Exception ex)
            {
                logger?.LogError("Cannot create assembly reader.", ex);
                return null;
            }
        }
    }
}