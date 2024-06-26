using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using Xunit;
using static VirtoCommerce.Platform.Web.Tests.Security.PlatformWebMockHelper;

namespace VirtoCommerce.Platform.Web.Tests.Security
{
    public class CustomUserManagerEventsUnitTests
    {
        [Theory]
        [ClassData(typeof(CreateTestData))]
        public async Task Create_CheckEvents(ApplicationUser user, Action<IEvent>[] assertions)
        {
            //Arrange

            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            userStoreMock.Setup(x => x.FindByIdAsync(user.Id, CancellationToken.None)).ReturnsAsync(user.CloneTyped());
            userStoreMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), CancellationToken.None))
                .ReturnsAsync(IdentityResult.Success);
            var eventPublisher = new EventPublisherStub();

            var userManager = SecurityMockHelper.TestCustomUserManager(userStoreMock, eventPublisher);

            //Act
            var result = await userManager.CreateAsync(user);

            //Assert
            result.Succeeded.Should().BeTrue();
            eventPublisher.Events.Should().SatisfyRespectively(assertions);
        }

        [Theory]
        [ClassData(typeof(UpdateTestData))]
        public async Task UpdateUser_CheckEvents(ApplicationUser user, ApplicationUser oldUser, Action<IEvent>[] assertions)
        {
            //Arrange
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            userStoreMock.Setup(x => x.FindByIdAsync(user.Id, CancellationToken.None)).ReturnsAsync(oldUser);
            var eventPublisher = new EventPublisherStub();

            var userManager = SecurityMockHelper.TestCustomUserManager(userStoreMock, eventPublisher);

            //Act
            var result = await userManager.UpdateAsync(user);

            //Assert
            result.Succeeded.Should().BeTrue();
            eventPublisher.Events.Should().SatisfyRespectively(assertions);
        }

        [Theory]
        [ClassData(typeof(ResetPasswordTestData))]
        public async Task ResetPassword_CheckEvents(ApplicationUser user, Action<IEvent>[] assertions)
        {
            //Arrange
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            userStoreMock.Setup(x => x.FindByIdAsync(user.Id, CancellationToken.None)).ReturnsAsync(user.CloneTyped());
            var eventPublisher = new EventPublisherStub();

            var userManager = SecurityMockHelper.TestCustomUserManager(userStoreMock, eventPublisher);
            userManager.RegisterTokenProvider("Static", new StaticTokenProvider());
            userManager.Options.Tokens.PasswordResetTokenProvider = "Static";
            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            //Act
            var result = await userManager.ResetPasswordAsync(user, token, "Qwerty123!");

            //Assert
            result.Succeeded.Should().BeTrue();
            eventPublisher.Events.Should().SatisfyRespectively(assertions);
        }

        [Theory]
        [ClassData(typeof(ChangePasswordTestData))]
        public async Task ChangePassword_CheckEvents(ApplicationUser user, Action<IEvent>[] assertions)
        {
            //Arrange
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            userStoreMock.Setup(x => x.FindByIdAsync(user.Id, CancellationToken.None)).ReturnsAsync(user.CloneTyped());
            var eventPublisher = new EventPublisherStub();

            var userManager = SecurityMockHelper.TestCustomUserManager(userStoreMock, eventPublisher);

            //Act
            var result = await userManager.ChangePasswordAsync(user, "current_pass", "Qwerty123!");

            //Assert
            result.Succeeded.Should().BeTrue();
            eventPublisher.Events.Should().SatisfyRespectively(assertions);
        }

        #region TestData

        private class CreateTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                    {
                        new ApplicationUser { Id = "id1", UserName = "NewName", PasswordHash = Guid.NewGuid().ToString() },
                        new Action<IEvent>[]
                        {
                            x => x.GetType().Should().Be<UserChangingEvent>(),
                            x => x.GetType().Should().Be<UserChangedEvent>(),
                        }
                    };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private class UpdateTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                    {
                        new ApplicationUser { Id = "id1", UserName = "NewName" },
                        new ApplicationUser { Id = "id1", UserName = "OldName" },
                        new Action<IEvent>[]
                        {
                            x => x.GetType().Should().Be<UserChangingEvent>(),
                            x => x.GetType().Should().Be<UserChangedEvent>(),
                        }
                    };
                yield return new object[]
                    {
                        new ApplicationUser { Id = "id1", UserName = "NewName", PasswordHash = Guid.NewGuid().ToString() },
                        new ApplicationUser { Id = "id1", UserName = "OldName", PasswordHash = Guid.NewGuid().ToString() },
                        new Action<IEvent>[]
                        {
                            x => x.GetType().Should().Be<UserChangingEvent>(),
                            x => x.GetType().Should().Be<UserChangedEvent>(),
                            x => x.GetType().Should().Be<UserPasswordChangedEvent>(),
                        }
                    };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private class ResetPasswordTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                    {
                        new ApplicationUser { Id = "id1", UserName = "NewName", PasswordHash = Guid.NewGuid().ToString() },
                        new Action<IEvent>[]
                        {
                            x => x.GetType().Should().Be<UserChangingEvent>(),
                            x => x.GetType().Should().Be<UserPasswordChangedEvent>(),
                            x => x.GetType().Should().Be<UserResetPasswordEvent>(),
                        }
                    };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private class ChangePasswordTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                    {
                        new ApplicationUser { Id = "id1", UserName = "NewName", PasswordHash = Guid.NewGuid().ToString() },
                        new Action<IEvent>[]
                        {
                            x => x.GetType().Should().Be<UserChangingEvent>(),
                            x => x.GetType().Should().Be<UserPasswordChangedEvent>(),
                            x => x.GetType().Should().Be<UserChangedPasswordEvent>(),
                        }
                    };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        #endregion TestData
    }

    internal class EventPublisherStub : IEventPublisher
    {
        public List<IEvent> Events = new List<IEvent>();

        Task IEventPublisher.Publish<T>(T @event, CancellationToken cancellationToken)
        {
            Events.Add(@event);
            return Task.CompletedTask;
        }
    }

    internal class StaticTokenProvider : IUserTwoFactorTokenProvider<ApplicationUser>
    {
        public async Task<string> GenerateAsync(string purpose, UserManager<ApplicationUser> manager, ApplicationUser user)
        {
            return MakeToken(purpose, await manager.GetUserIdAsync(user));
        }

        public async Task<bool> ValidateAsync(string purpose, string token, UserManager<ApplicationUser> manager, ApplicationUser user)
        {
            return token == MakeToken(purpose, await manager.GetUserIdAsync(user));
        }

        public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<ApplicationUser> manager, ApplicationUser user)
        {
            return Task.FromResult(true);
        }

        private static string MakeToken(string purpose, string userId)
        {
            return string.Join(":", userId, purpose, "ImmaToken");
        }
    }
}
