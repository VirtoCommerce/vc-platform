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
using VirtoCommerce.Platform.Web.Security;

namespace VirtoCommerce.Platform.Tests.Security
{
    public static class SecurityMockHelpers
    {
        public static CustomUserManager TestCustomUserManager(Mock<IUserStore<ApplicationUser>> storeMock, IEventPublisher eventPublisher)
        {
            return TestCustomUserManager(storeMock, null, null, eventPublisher);
        }

        public static CustomUserManager TestCustomUserManager(Mock<IUserStore<ApplicationUser>> storeMock = null, UserOptionsExtended userOptions = null, PlatformMemoryCache platformMemoryCache = null, IEventPublisher eventPublisher = null)
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

            var optionsMock = new Mock<IOptions<IdentityOptions>>();
            var idOptions = new IdentityOptions();
            idOptions.Lockout.AllowedForNewUsers = false;
            optionsMock.Setup(o => o.Value).Returns(idOptions);

            userOptions ??= new UserOptionsExtended
            {
                MaxPasswordAge = new TimeSpan(0)
            };
            var userOptionsMock = new Mock<IOptions<UserOptionsExtended>>();
            userOptionsMock.Setup(o => o.Value).Returns(userOptions);
            var userValidators = new List<IUserValidator<ApplicationUser>>();
            var validator = new Mock<IUserValidator<ApplicationUser>>();
            userValidators.Add(validator.Object);
            var pwdValidators = new List<PasswordValidator<ApplicationUser>>();
            pwdValidators.Add(new PasswordValidator<ApplicationUser>());
            var roleManagerMock = new Mock<RoleManager<Role>>(Mock.Of<IRoleStore<Role>>(),
                new[] { Mock.Of<IRoleValidator<Role>>() },
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                Mock.Of<ILogger<RoleManager<Role>>>());
            var passwordHasher = new Mock<IUserPasswordHasher>();
            passwordHasher.Setup(x => x.VerifyHashedPassword(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(PasswordVerificationResult.Success);

            var userManager = new CustomUserManager(storeMock.Object,
                Mock.Of<IOptions<IdentityOptions>>(),
                passwordHasher.Object,
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
                eventPublisher);

            validator.Setup(x => x.ValidateAsync(userManager, It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success).Verifiable();
            return userManager;
        }
    }
}
