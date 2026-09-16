using System;
using FluentAssertions;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Model;
using Xunit;
using VirtoCommerce.Platform.Core.Security.SignInLog;
using VirtoCommerce.Platform.Security.SignInLog;

namespace VirtoCommerce.Platform.Tests.Security;

public class UserSignInLogEntityTests
{
    [Fact]
    public void FromModel_ThenToModel_RoundTripsEveryField()
    {
        var source = new UserSignInLog
        {
            Id = "row-1",
            CreatedDate = new DateTime(2026, 9, 15, 10, 30, 0, DateTimeKind.Utc),
            UserName = "b2badmin@test.com",
            UserId = "user-1",
            Succeeded = true,
            FailureReason = null,
            SignInType = SignInType.Impersonation,
            Provider = null,
            OperatorUserId = "op-1",
            OperatorUserName = "support@virtocommerce.com",
            IpAddress = "203.0.113.7",
            UserAgent = "Mozilla/5.0",
            ClientId = "frontend",
            SessionId = "auth-1",
            StoreId = "B2B-store",
            StoreName = "B2B Store",
            MemberId = "member-1",
            OrganizationId = "org-1",
            OrganizationName = "Acme Inc",
        };

        var roundTripped = new UserSignInLogEntity()
            .FromModel(source, new PrimaryKeyResolvingMap())
            .ToModel(new UserSignInLog());

        roundTripped.Should().BeEquivalentTo(source);
    }

    [Fact]
    public void FromModel_NullModel_Throws()
    {
        var act = () => new UserSignInLogEntity().FromModel(null, new PrimaryKeyResolvingMap());

        act.Should().Throw<ArgumentNullException>();
    }
}
