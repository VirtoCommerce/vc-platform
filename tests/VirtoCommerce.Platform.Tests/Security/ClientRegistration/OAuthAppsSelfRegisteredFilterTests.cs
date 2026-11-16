using System.Linq;
using FluentAssertions;
using VirtoCommerce.Platform.Security.Model.OpenIddict;
using VirtoCommerce.Platform.Web.Controllers.Api;
using VirtoCommerce.Platform.Web.Model.Security;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Security.ClientRegistration;

public class OAuthAppsSelfRegisteredFilterTests
{
    private readonly VirtoOpenIddictEntityFrameworkCoreApplication[] _apps =
    [
        new() { ClientId = "admin-app" },
        new() { ClientId = "admin-app-with-properties", Properties = "{\"note\":\"vc_registration in a value\"}" },
        new() { ClientId = "https://claude.ai/oauth/client", Properties = "{\"vc_registration\":\"client_id_metadata_document\"}" },
        new() { ClientId = "dcr-client", Properties = "{\"vc_registration\":\"dynamic_registration\"}" },
    ];

    [Theory]
    [InlineData(true, "https://claude.ai/oauth/client", "dcr-client")]
    [InlineData(false, "admin-app", "admin-app-with-properties")]
    [InlineData(null, "admin-app", "admin-app-with-properties", "https://claude.ai/oauth/client", "dcr-client")]
    public void Filter_SelfRegistered_SelectsByTheRegistrationProperty(bool? selfRegistered, params string[] expected)
    {
        // Arrange
        var criteria = new OAuthAppSearchCriteria { SelfRegistered = selfRegistered };

        // Act
        var result = TestController.ApplyFilter(_apps.AsQueryable(), criteria);

        // Assert
        result.Select(x => x.ClientId).Should().BeEquivalentTo(expected);
    }

    private sealed class TestController() : OAuthAppsController(null, null, null, null, null)
    {
        public static IQueryable<VirtoOpenIddictEntityFrameworkCoreApplication> ApplyFilter(
            IQueryable<VirtoOpenIddictEntityFrameworkCoreApplication> query, OAuthAppSearchCriteria criteria) => Filter(query, criteria);
    }
}
