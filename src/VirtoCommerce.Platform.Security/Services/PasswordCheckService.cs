using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Security.Services
{
    /// <summary>
    /// Implementation of <see cref="IPasswordCheckService"/> that does actual password validation.
    /// </summary>
    public class PasswordCheckService : IPasswordCheckService
    {
        protected char[] SpecialCharacters { get; } = { '!', '@', '#', '$', '%', '^', '&', '*', '?', '_', '~', '-', '£', '(', ')', '.', ',' };

        protected IdentityOptions Options { get; }

        /// <summary>
        /// Creates new instance of password check service.
        /// </summary>
        /// <param name="options"></param>
        public PasswordCheckService(UserManager<ApplicationUser> userManager)
        {
            Options = userManager.Options;
        }

        /// <inheritdoc />
        public virtual Task<PasswordValidationResult> ValidatePasswordAsync(string password)
        {
            var result = new PasswordValidationResult
            {
                PasswordIsValid = true,
                MinPasswordLength = Options.Password.RequiredLength
            };

            if (!HasSufficientLength(password))
            {
                result.PasswordIsValid = false;
                result.PasswordViolatesMinLength = true;
            }

            if (Options.Password.RequireUppercase && !HasUpperCaseLetter(password))
            {
                result.PasswordIsValid = false;
                result.PasswordMustHaveUpperCaseLetters = true;
            }

            if (Options.Password.RequireLowercase && !HasLowerCaseLetter(password))
            {
                result.PasswordIsValid = false;
                result.PasswordMustHaveLowerCaseLetters = true;
            }

            if (Options.Password.RequireDigit && !HasDigit(password))
            {
                result.PasswordIsValid = false;
                result.PasswordMustHaveDigits = true;
            }

            if (Options.Password.RequireNonAlphanumeric && !HasSpecialCharacter(password))
            {
                result.PasswordIsValid = false;
                result.PasswordMustHaveSpecialCharacters = true;
            }

            return Task.FromResult(result);
        }

        protected virtual bool HasSufficientLength(string password)
        {
            return !string.IsNullOrEmpty(password)
                   && password.Length >= Options.Password.RequiredLength;
        }

        protected virtual bool HasUpperCaseLetter(string password)
        {
            return !string.IsNullOrWhiteSpace(password)
                   && password.Any(char.IsUpper);
        }

        protected virtual bool HasLowerCaseLetter(string password)
        {
            return !string.IsNullOrWhiteSpace(password)
                   && password.Any(char.IsLower);
        }

        protected virtual bool HasDigit(string password)
        {
            return !string.IsNullOrWhiteSpace(password)
                   && password.Any(char.IsDigit);
        }

        protected virtual bool HasSpecialCharacter(string password)
        {
            return !string.IsNullOrWhiteSpace(password)
                   && password.IndexOfAny(SpecialCharacters) != -1;
        }
    }
}
