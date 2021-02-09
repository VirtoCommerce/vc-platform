using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Security
{
    public class CustomUserManagerUnitTests
    {
        [Fact]
        public async Task UpdateUser_UserChangedAndPasswordChangedEvents()
        {
            //Arrange
            var newUser = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "NewName", PasswordHash = Guid.NewGuid().ToString() };
            var oldUser = new ApplicationUser { Id = newUser.Id, UserName = "OldName", PasswordHash = Guid.NewGuid().ToString() };

            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            userStoreMock.Setup(x => x.FindByIdAsync(newUser.Id, CancellationToken.None)).ReturnsAsync(oldUser);
            var eventPublisher = new EventPublisherStub();

            var userManager = SecurityMockHelpers.TestCustomUserManager(userStoreMock, eventPublisher);

            //Act
            var result = await userManager.UpdateAsync(newUser);

            //Assert
            result.Succeeded.Should().BeTrue();
            eventPublisher.Events.Should().HaveCount(3);
            eventPublisher.Events
                .Should()
                .SatisfyRespectively(
                    thing => thing.GetType().Should().Be<UserChangingEvent>(),
                    thing => thing.GetType().Should().Be<UserChangedEvent>(),
                    thing => thing.GetType().Should().Be<UserPasswordChangedEvent>());
        }
    }

    class EventPublisherStub : IEventPublisher
    {
        public List<IEvent> Events = new List<IEvent>();

        Task IEventPublisher.Publish<T>(T @event, CancellationToken cancellationToken)
        {
            Events.Add(@event);
            return Task.CompletedTask;
        }
    }
}
