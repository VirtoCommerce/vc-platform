using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Moq;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security;
using VirtoCommerce.Platform.Security.Model;
using VirtoCommerce.Platform.Security.Repositories;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Security
{
    public class CustomPasswordValidatorTests
    {
        [Fact]
        public async Task NullPassword_ThrowsException()
        {
            // Arrange
            var userManager = SecurityMockHelpers.TestCustomUserManager();
            var target = userManager.PasswordValidators.First();

            // Act, Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await target.ValidateAsync(userManager, new ApplicationUser(), null));
        }

        [Theory]
        // Valid passwords
        [InlineData("qwerty", false, false, false, false, 5, true, false, false, false, false, false)]
        [InlineData("LongPassword", true, true, false, false, 12, true, false, false, false, false, false)]
        [InlineData("P455WithD1git5", true, true, true, false, 8, true, false, false, false, false, false)]
        [InlineData("$ecur3P@Ssw0rd", true, true, true, true, 8, true, false, false, false, false, false)]
        [InlineData("12ä45", false, false, false, true, 5, true, false, false, false, false, false)]
        // Violation of minimal length
        [InlineData("letmein", false, false, false, false, 8, false, true, false, false, false, false)]
        [InlineData("123", false, false, false, false, 5, false, true, false, false, false, false)]
        [InlineData("", false, false, false, false, 4, false, true, false, false, false, false)]
        [InlineData(" \t \r\n", false, false, false, true, 4, false, true, false, false, false, false)]
        [InlineData(" \t \r\n", true, true, true, true, 4, false, true, true, true, true, false)]
        // Violation of upper-case letters requirement
        [InlineData("password", true, false, false, false, 8, false, false, true, false, false, false)]
        [InlineData("19910203", true, false, false, false, 5, false, false, true, false, false, false)]
        // Violation of lower-case letters requirement
        [InlineData("12345678", false, true, false, false, 5, false, false, false, true, false, false)]
        [InlineData("ADMIN", false, true, false, false, 5, false, false, false, true, false, false)]
        // Violation of digits requirement
        [InlineData("welcome", false, false, true, false, 5, false, false, false, false, true, false)]
        [InlineData("!@#$%^", false, false, true, false, 5, false, false, false, false, true, false)]
        // Violation of special characters requirement
        [InlineData("whatever", false, false, false, true, 8, false, false, false, false, false, true)]
        [InlineData("TrustNo1", false, false, false, true, 8, false, false, false, false, false, true)]
        // Multiple rule violations
        [InlineData("passw0rd", true, true, true, true, 10, false, true, true, false, false, true)]
        [InlineData("passw0rd", true, true, true, true, 8, false, false, true, false, false, true)]
        [InlineData("login", true, true, true, true, 5, false, false, true, false, true, true)]
        [InlineData("0987654321", true, true, true, true, 12, false, true, true, true, false, true)]
        [InlineData("!@#$%^", true, true, true, false, 5, false, false, true, true, true, false)]
        public async Task PasswordValidator_ValidatesByPasswordOptions(
            string password,
            bool requireUpperCase,
            bool requireLowerCase,
            bool requireDigits,
            bool requireSpecialCharacters,
            int minLength,
            bool passwordIsValid,
            bool passwordMustBeLonger,
            bool passwordMustHaveUpper,
            bool passwordMustHaveLower,
            bool passwordMustHaveDigit,
            bool passwordMustHaveNonAlphanumeric)
        {
            // Arrange
            var options = new IdentityOptions
            {
                Password = new PasswordOptions
                {
                    RequiredLength = minLength,
                    RequireUppercase = requireUpperCase,
                    RequireLowercase = requireLowerCase,
                    RequireDigit = requireDigits,
                    RequireNonAlphanumeric = requireSpecialCharacters
                }
            };

            var user = new ApplicationUser { Id = "id1" };
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            userStoreMock.Setup(x => x.FindByIdAsync(user.Id, CancellationToken.None)).ReturnsAsync(user);

            var userManager = SecurityMockHelpers.TestCustomUserManager(userStoreMock, identityOptions: options);
            var target = userManager.PasswordValidators.First();
            //var target = new CustomPasswordValidator(new CustomIdentityErrorDescriber(), _repositoryFactory, null);

            // Act
            var result = await target.ValidateAsync(userManager, user, password);

            // Assert
            Assert.Equal(passwordIsValid, result.Succeeded);

            Assert.Equal(passwordMustBeLonger, ExistsErrorByCode(result, nameof(IdentityErrorDescriber.PasswordTooShort)));
            if (passwordMustBeLonger)
            {
                var tooShort = TryFindErrorByCode(result, nameof(IdentityErrorDescriber.PasswordTooShort));
                Assert.IsType<CustomIdentityError>(tooShort);
                Assert.Equal(minLength, ((CustomIdentityError)tooShort).ErrorParameter);
            }
            Assert.Equal(passwordMustHaveUpper, ExistsErrorByCode(result, nameof(IdentityErrorDescriber.PasswordRequiresUpper)));
            Assert.Equal(passwordMustHaveLower, ExistsErrorByCode(result, nameof(IdentityErrorDescriber.PasswordRequiresLower)));
            Assert.Equal(passwordMustHaveDigit, ExistsErrorByCode(result, nameof(IdentityErrorDescriber.PasswordRequiresDigit)));
            Assert.Equal(passwordMustHaveNonAlphanumeric, ExistsErrorByCode(result, nameof(IdentityErrorDescriber.PasswordRequiresNonAlphanumeric)));
        }

        [Theory]
        [InlineData("Unique8)", 4, true)]
        [InlineData("paS$word1", 4, false)]
        [InlineData("paS$word3", 4, false)]
        [InlineData("paS$word4", 4, false)]
        [InlineData("paS$word5", 4, true)]
        [InlineData("paS$word6", 4, true)]
        [InlineData("paS$word6", 6, false)]
        [InlineData("paS$word6", 0, true)]
        public async Task PasswordValidator_ValidatesByPasswordHistory(
            string password,
            int passwordRepeatHistory,
            bool passwordIsValid)
        {
            // Arrange
            const string userId = "id1";
            string Hash(string p) => $"hash-{p}";

            var entities = Enumerable.Range(1, passwordRepeatHistory)
                 .Select(i => new UserPasswordHistoryEntity { UserId = userId, PasswordHash = Hash($"paS$word{i}"), CreatedDate = DateTime.UtcNow.AddDays(-i) })
                 .ToArray();

            var securityRepositoryMock = new Mock<ISecurityRepository>();
            securityRepositoryMock.Setup(x => x.GetUserPasswordsHistoryAsync(It.IsAny<string>())).ReturnsAsync(entities);
            ISecurityRepository repositoryFactory() => securityRepositoryMock.Object;

            var passwordHasher = new Mock<IUserPasswordHasher>();
            passwordHasher.Setup(x => x.HashPassword(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .Returns<ApplicationUser, string>((user, password) => Hash(password));
            passwordHasher.Setup(x => x.VerifyHashedPassword(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns<ApplicationUser, string, string>((user, hash, password) => hash == Hash(password) ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed);

            var userManager = SecurityMockHelpers.TestCustomUserManager(repositoryFactory: repositoryFactory, passwordHasher: passwordHasher);
            var user = new ApplicationUser { Id = userId };
            var target = userManager.PasswordValidators.First();

            // Act
            var result = await target.ValidateAsync(userManager, user, password);

            // Assert
            Assert.Equal(passwordIsValid, result.Succeeded);
            Assert.Equal(passwordIsValid, !ExistsErrorByCode(result, CustomPasswordValidator.RecentPasswordUsed));
        }

        private IdentityError TryFindErrorByCode(IdentityResult result, string errorCode)
        {
            return result.Errors.FirstOrDefault(x => x.Code == errorCode);
        }

        private bool ExistsErrorByCode(IdentityResult result, string errorCode)
        {
            return result.Errors.Any(x => x.Code == errorCode);
        }
    }
}
