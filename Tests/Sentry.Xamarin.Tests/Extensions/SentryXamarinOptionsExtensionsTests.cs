using Xunit;

namespace Sentry.Xamarin.Tests.Extensions
{
    public class SentryXamarinOptionsExtensionsTests
    {

        [InlineData("https://5a193123a9b841bc8d8e42531e7242a1@sentry.io/5560112", "https://5a193123a9b841bc8d8e42531e7242a1@sentry.io/5560112")]
        [InlineData("https://5a193123a9b841bc8d8e42531e7242a1@o447951.ingest.sentry.io/5560112", "https://5a193123a9b841bc8d8e42531e7242a1@sentry.io/5560112")]
        [InlineData("https://5a193123a9b841bc8d8e42531e7242a1@aliens.ufo/5560112", "https://5a193123a9b841bc8d8e42531e7242a1@aliens.ufo/5560112")]
        [InlineData("badDsn", "badDsn")]
        [InlineData(null, null)]
        [Theory]
        public void AdjustSaasDsn_AndroidDsn_AlternativeDsnSet(string dsn, string expectedDsn)
        {
            // Arrange
            var options = new SentryXamarinOptions()
            {
                Dsn = dsn
            };

            // Act
            options.AdjustSaasDsn();

            // Assert
            Assert.Equal(expectedDsn, options.Dsn);
        }
    }
}
