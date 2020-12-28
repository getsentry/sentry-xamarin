using Sentry.Xamarin.Forms.Internals;
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
            var options = new SentryOptions();
            var xamarinOptions = new SentryXamarinOptions();
            options.Release = null;

            //Act
            options.ConfigureSentryXamarinOptions(xamarinOptions);

            //Assert
            Assert.NotNull(options.Release);
        }

        [Fact]
        public void ConfigureSentryOptions_ReleaseNotSetIfInformed()
        {
            //Arrange
            var options = new SentryOptions()
            {
                Release = "myrelease@1.1"
            };
            var xamarinOptions = new SentryXamarinOptions();

            //Act
            options.ConfigureSentryXamarinOptions(xamarinOptions);

            //Assert
            Assert.Equal(options.Release, options.Release);
        }
        [Fact]
        public void ConfigureSentryOptions_DefaultCachePathDisabled_CachePathNotSet()
        {
            //Arrange
            var options = new SentryOptions()
            {
                CacheDirectoryPath = null
            };
            var xamarinOptions = new SentryXamarinOptions()
            {
                InternalCacheEnabled = false
            };

            //Act
            options.ConfigureSentryXamarinOptions(xamarinOptions);

            //Assert
            Assert.Null(options.CacheDirectoryPath);
        }

        [Fact]
        public void ConfigureSentryOptions_DefaultCachePathEnabledAndCacheDirectoryPathNull_CachePathSet()
        {
            //Arrange
            var options = new SentryOptions()
            {
                CacheDirectoryPath = null
            };
            var xamarinOptions = new SentryXamarinOptions();
            var expectedPath = options.DefaultCacheDirectoyPath();

            //Act
            options.ConfigureSentryXamarinOptions(xamarinOptions);

            //Assert
            Assert.Equal(expectedPath, options.CacheDirectoryPath);
            Assert.NotNull(expectedPath);
        }

        [Fact]
        public void ConfigureSentryOptions_DefaultCachePathEnabledAndCacheDirectorySet_CachePathSkipped()
        {
            //Arrange
            var expectedPath = "./";
            var options = new SentryOptions()
            {
                CacheDirectoryPath = expectedPath,
            };
            var xamarinOptions = new SentryXamarinOptions();

            //Act
            options.ConfigureSentryXamarinOptions(xamarinOptions);

            //Assert
            Assert.Equal(expectedPath, options.CacheDirectoryPath);
        }
    }
}
