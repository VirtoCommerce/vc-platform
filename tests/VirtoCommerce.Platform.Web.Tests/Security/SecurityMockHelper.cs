using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security;
using VirtoCommerce.Platform.Security.Repositories;
using VirtoCommerce.Platform.Web.Security;

namespace VirtoCommerce.Platform.Web.Tests.Security
{
    public partial class PlatformWebMockHelper
    {
        public class SecurityMockHelper
        {
            protected SecurityMockHelper()
            {
            }

            public static CustomUserManager TestCustomUserManager(Mock<IUserStore<ApplicationUser>> storeMock, IEventPublisher eventPublisher)
            {
                return TestCustomUserManager(storeMock, null, null, null, eventPublisher);
            }

            public static CustomUserManager TestCustomUserManager(
                Mock<IUserStore<ApplicationUser>> storeMock = null,
                UserOptionsExtended userOptions = null,
                IdentityOptions identityOptions = null,
                PlatformMemoryCache platformMemoryCache = null,
                IEventPublisher eventPublisher = null,
                Func<ISecurityRepository> repositoryFactory = null,
                Mock<IPasswordHasher<ApplicationUser>> passwordHasher = null)
            {
                storeMock ??= new Mock<IUserStore<ApplicationUser>>();
                storeMock.As<IUserRoleStore<ApplicationUser>>()
                    .Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>(), CancellationToken.None))
                    .ReturnsAsync(Array.Empty<string>());
                storeMock.As<IUserLoginStore<ApplicationUser>>()
                    .Setup(x => x.GetLoginsAsync(It.IsAny<ApplicationUser>(), CancellationToken.None))
                    .ReturnsAsync(Array.Empty<UserLoginInfo>());
                storeMock.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>(), CancellationToken.None))
                    .ReturnsAsync(IdentityResult.Success);

                var identityOptionsMock = new Mock<IOptions<IdentityOptions>>();
                if (identityOptions != null)
                {
                    identityOptionsMock.Setup(o => o.Value).Returns(identityOptions);
                }

                if (passwordHasher == null)
                {
                    passwordHasher = new Mock<IPasswordHasher<ApplicationUser>>();
                    passwordHasher.Setup(x => x.VerifyHashedPassword(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()))
                        .Returns(PasswordVerificationResult.Success);
                }

                userOptions ??= new UserOptionsExtended
                {
                    MaxPasswordAge = new TimeSpan(0)
                };
                var userOptionsMock = new Mock<IOptions<UserOptionsExtended>>();
                userOptionsMock.Setup(o => o.Value).Returns(userOptions);
                var userValidators = new List<IUserValidator<ApplicationUser>>();
                var validator = new Mock<IUserValidator<ApplicationUser>>();
                userValidators.Add(validator.Object);

                repositoryFactory ??= () => Mock.Of<ISecurityRepository>();
                var passwordOptionsMock = new Mock<IOptions<PasswordOptionsExtended>>();
                passwordOptionsMock.Setup(o => o.Value).Returns(new PasswordOptionsExtended());

                var pwdValidators = new PasswordValidator<ApplicationUser>[] { new CustomPasswordValidator(new CustomIdentityErrorDescriber(), repositoryFactory, passwordHasher.Object, passwordOptionsMock.Object) };

                var roleManagerMock = new Mock<RoleManager<Role>>(Mock.Of<IRoleStore<Role>>(),
                            new[] { Mock.Of<IRoleValidator<Role>>() },
                            Mock.Of<ILookupNormalizer>(),
                            Mock.Of<IdentityErrorDescriber>(),
                            Mock.Of<ILogger<RoleManager<Role>>>());

                var userManager = new CustomUserManager(storeMock.Object,
                    identityOptionsMock.Object,
                    passwordHasher.Object,
                    userOptionsMock.Object,
                    userValidators,
                    pwdValidators,
                    Mock.Of<ILookupNormalizer>(),
                    Mock.Of<IdentityErrorDescriber>(),
                    Mock.Of<IServiceProvider>(),
                    Mock.Of<ILogger<UserManager<ApplicationUser>>>(),
                    roleManagerMock.Object,
                    platformMemoryCache ?? Mock.Of<IPlatformMemoryCache>(),
                    eventPublisher,
                    repositoryFactory,
                    passwordOptionsMock.Object);

                validator
                    .Setup(x => x.ValidateAsync(userManager, It.IsAny<ApplicationUser>()))
                    .ReturnsAsync(IdentityResult.Success).Verifiable();

                return userManager;
            }
        }
    }
}
