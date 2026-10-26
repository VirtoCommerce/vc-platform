using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
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
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddSecurityServices();
        services.AddIdentity<ApplicationUser, Role>();

        // Assert
        services.Where(x => x.ServiceType == typeof(SignInManager<ApplicationUser>))
            .Should().ContainSingle()
            .Which.ImplementationType.Should().Be<VirtoSignInManager>();
    }

    [Fact]
    public void AddSecurityServices_SignInManagerRegisteredEarlier_KeepsIt()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddScoped<SignInManager<ApplicationUser>, DerivedSignInManager>();

        // Act
        services.AddSecurityServices();
        services.AddIdentity<ApplicationUser, Role>();

        // Assert
        services.Where(x => x.ServiceType == typeof(SignInManager<ApplicationUser>))
            .Should().ContainSingle()
            .Which.ImplementationType.Should().Be<DerivedSignInManager>();
    }

    private sealed class DerivedSignInManager(
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
        : SignInManager<ApplicationUser>(userManager, contextAccessor, claimsFactory, null, null, null, null);
}
