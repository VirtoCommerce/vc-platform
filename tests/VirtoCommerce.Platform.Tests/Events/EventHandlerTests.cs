using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using VirtoCommerce.Platform.Core.Bus;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security.Events;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Events;

[Trait("Category", "Unit")]
public class EventHandlerTests
{
    [Fact]
    public async Task HandleMultipleEventTypesWithSingleRegistration()
    {
        var bus = new InProcessBus(new Mock<ILogger<InProcessBus>>().Object);

        var domainHandlerEvents = new List<string>();
        var userLoginHandlerEvents = new List<string>();
        var userChangedHandlerEvents = new List<string>();

        // This handler should handle all event types
        bus.RegisterHandler<DomainEvent>((message, _) => { domainHandlerEvents.Add(message.GetType().Name); return Task.CompletedTask; });

        // These handlers should handle only specific event types
        bus.RegisterHandler<UserLoginEvent>((message, _) => { userLoginHandlerEvents.Add(message.GetType().Name); return Task.CompletedTask; });
        bus.RegisterHandler<UserChangedEvent>((message, _) => { userChangedHandlerEvents.Add(message.GetType().Name); return Task.CompletedTask; });

        await bus.Publish(new UserLoginEvent(user: null));
        await bus.Publish(new UserChangedEvent(changedEntries: null));

        domainHandlerEvents.Should().BeEquivalentTo([nameof(UserLoginEvent), nameof(UserChangedEvent)]);
        userLoginHandlerEvents.Should().BeEquivalentTo([nameof(UserLoginEvent)]);
        userChangedHandlerEvents.Should().BeEquivalentTo([nameof(UserChangedEvent)]);
    }
}
