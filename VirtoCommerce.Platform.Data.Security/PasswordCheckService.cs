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

        protected IPasswordCheckOptions Options { get; }

        /// <summary>
        /// Creates new instance of password check service.
        /// </summary>
        /// <param name="options"></param>
        public PasswordCheckService(IPasswordCheckOptions options)
        {
            Options = options;
        }

        /// <inheritdoc />
        public virtual Task<PasswordValidationResult> ValidatePasswordAsync(string password)
        {
            var result = new PasswordValidationResult
            {
                PasswordIsValid = true,
                MinPasswordLength = Options.RequiredPasswordLength
            };

            if (!HasSufficientLength(password))
            {
                result.PasswordIsValid = false;
                result.PasswordViolatesMinLength = true;
            }

            if (Options.RequireUpperCaseLetters && !HasUpperCaseLetter(password))
            {
                result.PasswordIsValid = false;
                result.PasswordMustHaveUpperCaseLetters = true;
            }

            if (Options.RequireLowerCaseLetters && !HasLowerCaseLetter(password))
            {
                result.PasswordIsValid = false;
                result.PasswordMustHaveLowerCaseLetters = true;
            }

            if (Options.RequireDigits && !HasDigit(password))
            {
                result.PasswordIsValid = false;
                result.PasswordMustHaveDigits = true;
            }

            if (Options.RequireSpecialCharacters && !HasSpecialCharacter(password))
            {
                result.PasswordIsValid = false;
                result.PasswordMustHaveSpecialCharacters = true;
            }

            return Task.FromResult(result);
        }

        protected virtual bool HasSufficientLength(string password)
        {
            return !password.IsNullOrEmpty()
                   && password.Length >= Options.RequiredPasswordLength;
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
