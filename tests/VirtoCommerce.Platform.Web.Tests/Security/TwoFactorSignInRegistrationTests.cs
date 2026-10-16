using System;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security;
using VirtoCommerce.Platform.Web.Security;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.Security;

public class TwoFactorSignInRegistrationTests
{
    [Fact]
    public void AddSecurityServices_ThenAddIdentity_RegistersVirtoSignInManager()
    {
        var services = new ServiceCollection();

        services.AddSecurityServices();
        services.AddIdentity<ApplicationUser, Role>();

        services.Where(x => x.ServiceType == typeof(SignInManager<ApplicationUser>))
            .Should().ContainSingle()
            .Which.ImplementationType.Should().Be<VirtoSignInManager>();
    }

    [Fact]
    public void AddSecurityServices_SignInManagerRegisteredEarlier_KeepsIt()
    {
        var services = new ServiceCollection();
        services.AddScoped<SignInManager<ApplicationUser>, DerivedSignInManager>();

        services.AddSecurityServices();
        services.AddIdentity<ApplicationUser, Role>();

        services.Where(x => x.ServiceType == typeof(SignInManager<ApplicationUser>))
            .Should().ContainSingle()
            .Which.ImplementationType.Should().Be<DerivedSignInManager>();
    }

    [Theory]
    [InlineData(true, false, true)]
    [InlineData(true, true, false)]
    [InlineData(false, false, false)]
    public void UseTwoFactorSignInManagerCheck_ReportsPoliciesTheSignInManagerIgnores(bool hasPolicy, bool isVirtoSignInManager, bool reportsError)
    {
        var services = new ServiceCollection();
        if (hasPolicy)
        {
            services.AddSingleton(Mock.Of<ITwoFactorPolicy>());
        }

        services.AddScoped(_ => CreateSignInManager(isVirtoSignInManager));
        var logger = new Mock<ILogger>();

        new ApplicationBuilder(services.BuildServiceProvider()).UseTwoFactorSignInManagerCheck(logger.Object);

        logger.Verify(x => x.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()), reportsError ? Times.Once : Times.Never);
    }

    private static SignInManager<ApplicationUser> CreateSignInManager(bool isVirtoSignInManager)
    {
        var userManager = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null).Object;
        var claimsFactory = Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>();

        return isVirtoSignInManager
            ? new VirtoSignInManager(userManager, new HttpContextAccessor(), claimsFactory, null, null, null, null, [])
            : new SignInManager<ApplicationUser>(userManager, new HttpContextAccessor(), claimsFactory, null, null, null, null);
    }

    private sealed class DerivedSignInManager(
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
        : SignInManager<ApplicationUser>(userManager, contextAccessor, claimsFactory, null, null, null, null);
}
