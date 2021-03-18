using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Web.Security;

namespace VirtoCommerce.Platform.Tests.Security
{
    public static class SecurityMockHelpers
    {
        public static CustomUserManager TestCustomUserManager(Mock<IUserStore<ApplicationUser>> storeMock = null, IEventPublisher eventPublisher = null)
        {
            storeMock = storeMock ?? new Mock<IUserStore<ApplicationUser>>();
            storeMock.As<IUserRoleStore<ApplicationUser>>()
                .Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>(), CancellationToken.None))
                .ReturnsAsync(Array.Empty<string>());
            storeMock.As<IUserLoginStore<ApplicationUser>>()
                .Setup(x => x.GetLoginsAsync(It.IsAny<ApplicationUser>(), CancellationToken.None))
                .ReturnsAsync(Array.Empty<UserLoginInfo>());
            storeMock.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>(), CancellationToken.None))
                .ReturnsAsync(IdentityResult.Success);

            var options = new Mock<IOptions<IdentityOptions>>();
            var idOptions = new IdentityOptions();
            idOptions.Lockout.AllowedForNewUsers = false;
            options.Setup(o => o.Value).Returns(idOptions);
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
                userValidators,
                pwdValidators,
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                Mock.Of<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<ApplicationUser>>>(),
                roleManagerMock.Object,
                Mock.Of<IPlatformMemoryCache>(),
                eventPublisher);

            validator.Setup(x => x.ValidateAsync(userManager, It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success).Verifiable();
            return userManager;
        }
    }
}
