using Sentry.Extensions;
using NSubstitute;
using Xunit;
using System.Collections.Generic;
using System;
using Sentry.Extensibility;
using System.Threading.Tasks;

namespace Sentry.Xamarin.Tests.Extensions
{
    public class IHubExtensionsTests
    {
        public IHub Sut { get; set; } = Substitute.For<IHub>();
        public Scope Scope { get; set; } = new Scope(null);

        public IHubExtensionsTests()
        {
            Sut.When(h => h.ConfigureScope(Arg.Any<Action<Scope>>()))
                .Do(c => c.Arg<Action<Scope>>()(Scope));
        }

        [Fact]
        public void AddInternalBreadcrumb_LastBreadcrumbNull_LastBreadcrumbSet()
        {
            //Assert
            const string expectedMessage = "message";
            var hub = Sut;
            var options = new SentryXamarinOptions();

            //Act
            hub.AddInternalBreadcrumb(options, expectedMessage);

            //Assert
            Assert.NotNull(options.LastInternalBreadcrumb);
            Assert.Equal(expectedMessage, options.LastInternalBreadcrumb.Message);
        }

        [Fact]
        public void AddInternalBreadcrumb_NewBreadcumbEqualsPrevious_BreadcrumbDiscarded()
        {
            //Assert
            const string duplicatedMessage = "message";
            const string duplicatedType = "type";
            var duplicatedData = new Dictionary<string, string>{ {"key", "value"} };
            var logger = Substitute.For<IDiagnosticLogger>();
            var hub = Sut;
            var options = new SentryXamarinOptions();
            options.Debug = true;
            options.DiagnosticLogger = logger;
            options.LastInternalBreadcrumb = new Breadcrumb(duplicatedMessage, duplicatedType, duplicatedData);

            //Act
            hub.AddInternalBreadcrumb(options, duplicatedMessage, null, duplicatedType, duplicatedData);

            //Assert
            logger.Received().Log(Arg.Any<SentryLevel>(), Arg.Is<string>(IHubExtensions.DuplicatedBreadcrumbDropped), Arg.Any<Exception>(), Arg.Any<object[]>());
        }

        [Fact]
        public void AddInternalBreadcrumb_NewBreadcumbWithDifferentMessage_BreadcrumbAdded()
        {
            //Assert
            const string type = "type";
            var data = new Dictionary<string, string> { { "key", "value" } };
            var logger = Substitute.For<IDiagnosticLogger>();
            var hub = Sut;
            var options = new SentryXamarinOptions();
            options.Debug = true;
            options.DiagnosticLogger = logger;
            var breadcrumb = new Breadcrumb("message", type, data);

            //Act
            options.LastInternalBreadcrumb = breadcrumb;
            hub.AddInternalBreadcrumb(options, "message2", null, type, data);

            //Assert
            logger.DidNotReceive().Log(Arg.Any<SentryLevel>(), Arg.Is<string>(IHubExtensions.DuplicatedBreadcrumbDropped), Arg.Any<Exception>(), Arg.Any<object[]>());
            Assert.NotEqual(breadcrumb, options.LastInternalBreadcrumb);
        }

        [Fact]
        public void AddInternalBreadcrumb_NewBreadcumbWithDifferentType_BreadcrumbAdded()
        {
            //Assert
            const string message = "message";
            var data = new Dictionary<string, string> { { "key", "value" } };
            var logger = Substitute.For<IDiagnosticLogger>();
            var hub = Sut;
            var options = new SentryXamarinOptions();
            options.Debug = true;
            options.DiagnosticLogger = logger;
            var breadcrumb = new Breadcrumb(message, "type", data);
            options.LastInternalBreadcrumb = breadcrumb;
            
            //Act
            hub.AddInternalBreadcrumb(options, message, null, "type2", data);

            //Assert
            logger.DidNotReceive().Log(Arg.Any<SentryLevel>(), Arg.Is<string>(IHubExtensions.DuplicatedBreadcrumbDropped), Arg.Any<Exception>(), Arg.Any<object[]>());
            Assert.NotEqual(breadcrumb, options.LastInternalBreadcrumb);
        }

        [Fact]
        public void AddInternalBreadcrumb_NewBreadcumbWithDifferentData_BreadcrumbAdded()
        {
            //Assert
            const string message = "message";
            const string type = "type";
            var logger = Substitute.For<IDiagnosticLogger>();
            var hub = Sut;
            var options = new SentryXamarinOptions();
            options.Debug = true;
            options.DiagnosticLogger = logger;
            var breadcrumb = new Breadcrumb(message, type, new Dictionary<string, string> { { "key", "value" } });
            options.LastInternalBreadcrumb = breadcrumb;

            //Act
            hub.AddInternalBreadcrumb(options, message, null, type, new Dictionary<string, string> { { "key", "value2" } });

            //Assert
            logger.DidNotReceive().Log(Arg.Any<SentryLevel>(), Arg.Is<string>(IHubExtensions.DuplicatedBreadcrumbDropped), Arg.Any<Exception>(), Arg.Any<object[]>());
            Assert.NotEqual(breadcrumb, options.LastInternalBreadcrumb);
        }

        [Fact]
        public async Task AddInternalBreadcrumb_DuplicatedIs2SecondsAhead_BreadcrumbAdded()
        {
            //Assert
            const string message = "message";
            const string type = "type";
            var data = new Dictionary<string, string> { { "key", "value" } };
            var logger = Substitute.For<IDiagnosticLogger>();
            var hub = Sut;
            var options = new SentryXamarinOptions();
            options.Debug = true;
            options.DiagnosticLogger = logger;

            var breadcrumb = new Breadcrumb(message, type, data);
            await Task.Delay(3000);

            options.LastInternalBreadcrumb = breadcrumb;

            //Act
            hub.AddInternalBreadcrumb(options, message, null, type, data);

            //Assert
            logger.DidNotReceive().Log(Arg.Any<SentryLevel>(), Arg.Is<string>(IHubExtensions.DuplicatedBreadcrumbDropped), Arg.Any<Exception>(), Arg.Any<object[]>());
            Assert.NotEqual(breadcrumb, options.LastInternalBreadcrumb);
        }
    }
}
