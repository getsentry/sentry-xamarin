﻿using Sentry.Xamarin.Forms.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sentry.Xamarin.Forms.Tests.UWP.Extensions
{
    public class SentryOptionsExtensionsTests
    {
        [Fact]
        public void ConfigureSentryOptions_ReleaseSetIfNotInformed()
        {
            //Arrange
            var options = new SentryXamarinOptions();
            options.Release = null;

            //Act
            options.ConfigureSentryXamarinOptions();

            //Assert
            Assert.NotNull(options.Release);
        }

        [Fact]
        public void ConfigureSentryOptions_ReleaseNotSetIfInformed()
        {
            //Arrange
            var options = new SentryXamarinOptions()
            {
                Release = "myrelease@1.1"
            };

            //Act
            options.ConfigureSentryXamarinOptions();

            //Assert
            Assert.Equal(options.Release, options.Release);
        }
        [Fact]
        public void ConfigureSentryOptions_DefaultCachePathDisabled_CachePathNotSet()
        {
            //Arrange
            var options = new SentryXamarinOptions()
            {
                CacheDirectoryPath = null,
                InternalCacheEnabled = false
            };

            //Act
            options.ConfigureSentryXamarinOptions();

            //Assert
            Assert.Null(options.CacheDirectoryPath);
        }

        [Fact]
        public void ConfigureSentryOptions_DefaultCachePathEnabledAndCacheDirectoryPathNull_CachePathSet()
        {
            //Arrange
            var options = new SentryXamarinOptions()
            {
                CacheDirectoryPath = null
            };
            var expectedPath = options.DefaultCacheDirectoyPath();

            //Act
            options.ConfigureSentryXamarinOptions();

            //Assert
            Assert.Equal(expectedPath, options.CacheDirectoryPath);
            Assert.NotNull(expectedPath);
        }

        [Fact]
        public void ConfigureSentryOptions_DefaultCachePathEnabledAndCacheDirectorySet_CachePathSkipped()
        {
            //Arrange
            var expectedPath = "./";
            var options = new SentryXamarinOptions()
            {
                CacheDirectoryPath = expectedPath,
            };

            //Act
            options.ConfigureSentryXamarinOptions();

            //Assert
            Assert.Equal(expectedPath, options.CacheDirectoryPath);
        }
    }
}
