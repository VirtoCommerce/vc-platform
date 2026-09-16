using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Security.Handlers;
using Xunit;
using VirtoCommerce.Platform.Core.Security.SignInLog;
using VirtoCommerce.Platform.Security.SignInLog;

namespace VirtoCommerce.Platform.Tests.Security;

public class LogUserSignInEventHandlerTests
{
    [Fact]
    public async Task Handle_PasswordSignIn_WritesRecordWithRequestContext()
    {
        var written = new List<UserSignInLog>();
        var handler = CreateHandler(written, signInLogEnabled: true, ip: "203.0.113.7", userAgent: "UA/1.0");

        await handler.Handle(new UserSignInAttemptEvent
        {
            UserName = "b2badmin@test.com",
            UserId = "user-1",
            Succeeded = false,
            FailureReason = SignInFailureReason.InvalidPassword,
            SignInType = SignInType.Password,
            StoreId = "B2B-store",
            MemberId = "member-1",
        });

        var record = written.Should().ContainSingle().Subject;
        record.UserName.Should().Be("b2badmin@test.com");
        record.UserId.Should().Be("user-1");
        record.Succeeded.Should().BeFalse();
        record.FailureReason.Should().Be(SignInFailureReason.InvalidPassword);
        record.IpAddress.Should().Be("203.0.113.7");
        record.UserAgent.Should().Be("UA/1.0");
        record.StoreId.Should().Be("B2B-store");
        record.MemberId.Should().Be("member-1");
        record.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
    }

    [Fact]
    public async Task Handle_WhenDisabled_SuppressesOrdinarySignIn()
    {
        var written = new List<UserSignInLog>();
        var handler = CreateHandler(written, signInLogEnabled: false);

        await handler.Handle(new UserSignInAttemptEvent
        {
            UserName = "a",
            SignInType = SignInType.Password,
            Succeeded = true,
        });

        written.Should().BeEmpty();
    }

    [Theory]
    [InlineData(SignInType.Impersonation)]
    [InlineData(SignInType.ImpersonationRevert)]
    public async Task Handle_WhenDisabled_StillWritesImpersonation(string signInType)
    {
        var written = new List<UserSignInLog>();
        var handler = CreateHandler(written, signInLogEnabled: false);

        await handler.Handle(new UserSignInAttemptEvent
        {
            UserName = "b2badmin@test.com",
            SignInType = signInType,
            Succeeded = true,
            OperatorUserName = "support@virtocommerce.com",
        });

        // The compliance anchor must not be switchable off.
        written.Should().ContainSingle().Which.OperatorUserName.Should().Be("support@virtocommerce.com");
    }

    [Fact]
    public async Task Handle_WhenEnricherThrows_StillWritesTheRecord()
    {
        var written = new List<UserSignInLog>();
        var enricher = new Mock<IUserSignInLogEnricher>();
        enricher.SetupGet(x => x.Priority).Returns(0);
        enricher.Setup(x => x.Enrich(It.IsAny<UserSignInLog>())).ThrowsAsync(new Exception("module down"));

        var handler = CreateHandler(written, signInLogEnabled: true, enrichers: [enricher.Object]);

        await handler.Handle(new UserSignInAttemptEvent
        {
            UserName = "a",
            SignInType = SignInType.Password,
            Succeeded = true,
        });

        written.Should().ContainSingle();
    }

    [Fact]
    public async Task Handle_RunsEnrichersInPriorityOrder()
    {
        var order = new List<int>();
        var written = new List<UserSignInLog>();

        var second = new Mock<IUserSignInLogEnricher>();
        second.SetupGet(x => x.Priority).Returns(10);
        second.Setup(x => x.Enrich(It.IsAny<UserSignInLog>()))
            .Callback(() => order.Add(10)).Returns(Task.CompletedTask);

        var first = new Mock<IUserSignInLogEnricher>();
        first.SetupGet(x => x.Priority).Returns(1);
        first.Setup(x => x.Enrich(It.IsAny<UserSignInLog>()))
            .Callback(() => order.Add(1)).Returns(Task.CompletedTask);

        var handler = CreateHandler(written, signInLogEnabled: true, enrichers: [second.Object, first.Object]);

        await handler.Handle(new UserSignInAttemptEvent
        {
            UserName = "a",
            SignInType = SignInType.Password,
            Succeeded = true,
        });

        order.Should().Equal(1, 10);
    }

    private static LogUserSignInEventHandler CreateHandler(
        List<UserSignInLog> written,
        bool signInLogEnabled,
        string ip = null,
        string userAgent = null,
        IEnumerable<IUserSignInLogEnricher> enrichers = null)
    {
        var writer = new Mock<IUserSignInLogWriter>();
        writer.Setup(x => x.Write(It.IsAny<UserSignInLog>())).Callback<UserSignInLog>(written.Add);

        // GetValueAsync is an extension method; GetObjectSettingAsync is the mockable seam beneath it.
        var settings = new Mock<ISettingsManager>();
        settings.Setup(x => x.GetObjectSettingAsync(
                PlatformConstants.Settings.Security.SignInLogEnabled.Name, null, null))
            .ReturnsAsync(new ObjectSettingEntry { Value = signInLogEnabled });

        var httpContext = new DefaultHttpContext();
        if (ip != null)
        {
            httpContext.Connection.RemoteIpAddress = IPAddress.Parse(ip);
        }

        if (userAgent != null)
        {
            httpContext.Request.Headers.UserAgent = userAgent;
        }

        var accessor = new Mock<IHttpContextAccessor>();
        accessor.SetupGet(x => x.HttpContext).Returns(httpContext);

        // Enrichers are resolved from a scope per event, so the test supplies a real container.
        var services = new ServiceCollection();
        foreach (var enricher in enrichers ?? [])
        {
            services.AddSingleton(enricher);
        }

        return new LogUserSignInEventHandler(
            writer.Object,
            settings.Object,
            accessor.Object,
            services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>(),
            NullLogger<LogUserSignInEventHandler>.Instance);
    }
}
