using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Services;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Security
{
    public class PasswordCheckServiceTests
    {
        [Theory]
        // Valid passwords
        [InlineData("qwerty", false, false, false, false, 5, true, false, false, false, false, false)]
        [InlineData("LongPassword", true, true, false, false, 12, true, false, false, false, false, false)]
        [InlineData("P455WithD1git5", true, true, true, false, 8, true, false, false, false, false, false)]
        [InlineData("$ecur3P@Ssw0rd", true, true, true, true, 8, true, false, false, false, false, false)]
        // Violation of minimal length
        [InlineData("letmein", false, false, false, false, 8, false, true, false, false, false, false)]
        [InlineData("123", false, false, false, false, 5, false, true, false, false, false, false)]
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
        // Handling null, empty and whitespace-only passwords
        [InlineData(null, false, false, false, false, 5, false, true, false, false, false, false)]
        [InlineData(null, true, true, true, true, 5, false, true, true, true, true, true)]
        [InlineData("", false, false, false, false, 5, false, true, false, false, false, false)]
        [InlineData(" \t \r\n", false, false, false, false, 5, true, false, false, false, false, false)]
        [InlineData(" \t \r\n", false, false, false, true, 5, false, false, false, false, false, true)]
        [InlineData(" \t \r\n", true, true, true, true, 5, false, false, true, true, true, true)]
        public async Task TestCheckingPassword(
            string password,
            bool requireUpperCase,
            bool requireLowerCase,
            bool requireDigits,
            bool requireSpecialCharacters,
            int minLength,
            bool passwordIsValid,
            bool passwordViolatesMinLength,
            bool passwordMustHaveUpperCaseLetters,
            bool passwordMustHaveLowerCaseLetters,
            bool passwordMustHaveDigits,
            bool passwordMustHaveSpecialCharacters)
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

            var userManager = CreateUserManager(options);
            var target = new PasswordCheckService(userManager);

            // Act
            var result = await target.ValidatePasswordAsync(password);

            // Assert
            Assert.Equal(passwordIsValid, result.PasswordIsValid);

            Assert.Equal(minLength, result.MinPasswordLength);
            Assert.Equal(passwordViolatesMinLength, result.PasswordViolatesMinLength);
            Assert.Equal(passwordMustHaveUpperCaseLetters, result.PasswordMustHaveUpperCaseLetters);
            Assert.Equal(passwordMustHaveLowerCaseLetters, result.PasswordMustHaveLowerCaseLetters);
            Assert.Equal(passwordMustHaveDigits, result.PasswordMustHaveDigits);
            Assert.Equal(passwordMustHaveSpecialCharacters, result.PasswordMustHaveSpecialCharacters);
        }

        private UserManager<ApplicationUser> CreateUserManager(IdentityOptions options)
        {
            var optionsAccessor = new OptionsWrapper<IdentityOptions>(options);

            // Note: all these mocks and stubs are just a boilerplate to create the UserManager instance.
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var passwordHasher = new Mock<IPasswordHasher<ApplicationUser>>();
            var userValidators = Enumerable.Empty<IUserValidator<ApplicationUser>>();
            var passwordValidators = Enumerable.Empty<IPasswordValidator<ApplicationUser>>();
            var lookupNormalizer = new Mock<ILookupNormalizer>();
            var serviceProvider = new Mock<IServiceProvider>();
            var logger = new Mock<ILogger<UserManager<ApplicationUser>>>();

            return new UserManager<ApplicationUser>(userStore.Object, optionsAccessor, passwordHasher.Object,
                userValidators, passwordValidators, lookupNormalizer.Object, new IdentityErrorDescriber(),
                serviceProvider.Object, logger.Object);
        }
    }
}
