using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Data.Security
{
    /// <summary>
    /// Implementation of <see cref="IPasswordCheckService"/> that does actual password validation.
    /// </summary>
    public class PasswordCheckService : IPasswordCheckService
    {
        protected char[] SpecialCharacters { get; } = { '!', '@', '#', '$', '%', '^', '&', '*', '?', '_', '~', '-', '£', '(', ')', '.', ',' };

        protected AuthenticationOptions Options { get; }

        /// <summary>
        /// Creates new instance of password check service.
        /// </summary>
        /// <param name="options"></param>
        public PasswordCheckService(AuthenticationOptions options)
        {
            Options = options;
        }

        /// <inheritdoc />
        public virtual Task<PasswordValidationResult> ValidatePasswordAsync(string password)
        {
            var result = new PasswordValidationResult
            {
                PasswordIsValid = true,
                MinPasswordLength = Options.PasswordRequiredLength
            };

            if (!HasSufficientLength(password))
            {
                result.PasswordIsValid = false;
                result.PasswordViolatesMinLength = true;
            }

            if (Options.PasswordRequireUppercase && !HasUpperCaseLetter(password))
            {
                result.PasswordIsValid = false;
                result.PasswordMustHaveUpperCaseLetters = true;
            }

            if (Options.PasswordRequireLowercase && !HasLowerCaseLetter(password))
            {
                result.PasswordIsValid = false;
                result.PasswordMustHaveLowerCaseLetters = true;
            }

            if (Options.PasswordRequireDigit && !HasDigit(password))
            {
                result.PasswordIsValid = false;
                result.PasswordMustHaveDigits = true;
            }

            if (Options.PasswordRequireNonLetterOrDigit && !HasSpecialCharacter(password))
            {
                result.PasswordIsValid = false;
                result.PasswordMustHaveSpecialCharacters = true;
            }

            return Task.FromResult(result);
        }

        protected virtual bool HasSufficientLength(string password)
        {
            return !password.IsNullOrEmpty()
                   && password.Length >= Options.PasswordRequiredLength;
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
