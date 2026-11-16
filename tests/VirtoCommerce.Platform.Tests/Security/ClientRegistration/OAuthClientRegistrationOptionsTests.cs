using System;
using FluentAssertions;
using VirtoCommerce.Platform.Core.Security;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Security.ClientRegistration;

public class OAuthClientRegistrationOptionsTests
{
    [Fact]
    public void Defaults_DocumentsOnFromPublicHostsOnly_DynamicRegistrationOff()
    {
        // Act
        var options = new AuthorizationOptions().ClientRegistration;

        // Assert
        options.ClientIdMetadataDocuments.Enabled.Should().BeTrue();
        options.ClientIdMetadataDocuments.AllowedHosts.Should().BeEmpty();
        options.ClientIdMetadataDocuments.AllowLoopbackInDevelopment.Should().BeFalse();
        options.ClientIdMetadataDocuments.MinCacheLifetime.Should().Be(TimeSpan.FromMinutes(5));
        options.ClientIdMetadataDocuments.MaxCacheLifetime.Should().Be(TimeSpan.FromHours(1));
        options.ClientIdMetadataDocuments.Timeout.Should().Be(TimeSpan.FromSeconds(5));
        options.ClientIdMetadataDocuments.MaxDocumentSize.Should().Be(5120);
        options.DynamicRegistration.Enabled.Should().BeFalse();
    }
}
