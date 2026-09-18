using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Model;
using VirtoCommerce.Platform.Security.Repositories;
using VirtoCommerce.Platform.Security.Services;
using Xunit;
using MockQueryable;
using VirtoCommerce.Platform.Core.Security.SignInLog;
using VirtoCommerce.Platform.Security.SignInLog;

namespace VirtoCommerce.Platform.Tests.Security;

/// <summary>
/// The impersonation marker is joined onto sessions from the sign-in audit log, because the
/// OpenIddict token exposes no readable principal. These tests cover that join directly.
/// </summary>
public class UserSessionsSearchServiceImpersonationTests
{
    [Fact]
    public async Task MarkImpersonatedSessions_SessionWithImpersonationRow_ExposesOperatorIdentity()
    {
        var service = CreateService(new UserSignInLogEntity
        {
            Id = "log-1",
            SessionId = "auth-1",
            SignInType = SignInType.Impersonation,
            OperatorUserId = "op-1",
            OperatorUserName = "support@virtocommerce.com",
        });

        var sessions = new List<UserSession> { new() { Id = "t1", SessionGroupId = "auth-1" } };

        await service.MarkImpersonatedSessions(sessions);

        var session = sessions.Should().ContainSingle().Subject;
        session.IsImpersonated.Should().BeTrue();
        session.OperatorUserId.Should().Be("op-1");
        session.OperatorUserName.Should().Be("support@virtocommerce.com");
    }

    [Fact]
    public async Task MarkImpersonatedSessions_OrdinarySession_IsNotMarked()
    {
        var service = CreateService(new UserSignInLogEntity
        {
            Id = "log-1",
            SessionId = "other-auth",
            SignInType = SignInType.Impersonation,
            OperatorUserId = "op-1",
        });

        var sessions = new List<UserSession> { new() { Id = "t1", SessionGroupId = "auth-1" } };

        await service.MarkImpersonatedSessions(sessions);

        sessions.Should().ContainSingle().Which.IsImpersonated.Should().BeFalse();
    }

    [Fact]
    public async Task MarkImpersonatedSessions_PasswordSignInRow_IsNotTreatedAsImpersonation()
    {
        var service = CreateService(new UserSignInLogEntity
        {
            Id = "log-1",
            SessionId = "auth-1",
            SignInType = SignInType.Password,
        });

        var sessions = new List<UserSession> { new() { Id = "t1", SessionGroupId = "auth-1" } };

        await service.MarkImpersonatedSessions(sessions);

        sessions.Should().ContainSingle().Which.IsImpersonated.Should().BeFalse();
    }

    private static TestableUserSessionsSearchService CreateService(params UserSignInLogEntity[] rows)
    {
        var repository = new Mock<ISecurityRepository>();
        repository.Setup(x => x.UserSignInLogs).Returns(new List<UserSignInLogEntity>(rows).BuildMock());

        return new TestableUserSessionsSearchService(ScopedServiceFactoryStub.Of(repository.Object));
    }

    private sealed class TestableUserSessionsSearchService : UserSessionsSearchService
    {
        public TestableUserSessionsSearchService(IScopedServiceFactory<ISecurityRepository> repositoryFactory)
            : base(Mock.Of<OpenIddict.Abstractions.IOpenIddictTokenManager>(), repositoryFactory)
        {
        }

        public new Task MarkImpersonatedSessions(IList<UserSession> sessions)
            => base.MarkImpersonatedSessions(sessions);
    }
}
