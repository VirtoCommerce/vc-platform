using System;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Security.Handlers;
using VirtoCommerce.Platform.Web.Security;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.Security;

/// <summary>
/// UseSecurityHandlers resolves each handler from the container via GetRequiredService, so a handler
/// wired to the event bus but missing from AddSecurityServices only fails at application startup.
/// Every handler listed here must stay registered.
/// </summary>
public class SecurityHandlerRegistrationTests
{
    [Theory]
    [InlineData(typeof(LogChangesUserChangedEventHandler))]
    [InlineData(typeof(LogUserSignInEventHandler))]
    [InlineData(typeof(UserApiKeyActualizeEventHandler))]
    [InlineData(typeof(RevokeUserTokenEventHandler))]
    public void AddSecurityServices_RegistersEveryEventHandlerUseSecurityHandlersResolves(Type handlerType)
    {
        var services = new ServiceCollection();

        services.AddSecurityServices();

        services.Should().Contain(
            descriptor => descriptor.ServiceType == handlerType,
            "UseSecurityHandlers calls GetRequiredService<{0}>() and would throw at startup otherwise",
            handlerType.Name);
    }
}
