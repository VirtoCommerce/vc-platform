using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Web.Security;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Security
{
    public class CustomUserManagerUnitTests
    {
        [Fact]
        public async Task UpdateUser_PublishEvents()
        {
            //Arrange
            var newUser = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "NewName", PasswordHash = Guid.NewGuid().ToString() };
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();

            var expectedUser = new ApplicationUser { Id = newUser.Id, UserName = "OldName", PasswordHash = Guid.NewGuid().ToString() };
            userStoreMock.Setup(x => x.FindByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedUser);
            var eventPublisher = new EventPublisherStub();


            var userManager = GetCustomUserManager(userStoreMock, eventPublisher);

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

        CustomUserManager GetCustomUserManager(Mock<IUserStore<ApplicationUser>> userStoreMock, IEventPublisher eventPublisher)
        {
            userStoreMock.As<IUserRoleStore<ApplicationUser>>()
                .Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Array.Empty<string>());
            userStoreMock.As<IUserLoginStore<ApplicationUser>>()
                .Setup(x => x.GetLoginsAsync(It.IsAny<ApplicationUser>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Array.Empty<UserLoginInfo>());
            userStoreMock.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(IdentityResult.Success);

            var roleManagerMock = new Mock<RoleManager<Role>>(Mock.Of<IRoleStore<Role>>(),
                new[] { Mock.Of<IRoleValidator<Role>>() },
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                Mock.Of<ILogger<RoleManager<Role>>>());

            var passwordValidatorMock = Mock.Of<IPasswordValidator<ApplicationUser>>();

            var userValidatorMock = new Mock<IUserValidator<ApplicationUser>>();
            userValidatorMock.Setup(x => x.ValidateAsync(It.IsAny<UserManager<ApplicationUser>>(), It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

            return new CustomUserManager(userStoreMock.Object,
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IUserPasswordHasher>(),
                new[] { userValidatorMock.Object },
                new[] { passwordValidatorMock },
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                Mock.Of<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<ApplicationUser>>>(),
                roleManagerMock.Object,
                Mock.Of<IPlatformMemoryCache>(),
                eventPublisher);
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
