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
        private const string SpecialCharacters = "!@#$%^&*?_~-£().,";

        private readonly IPasswordCheckOptions _options;

        /// <summary>
        /// Creates new instance of password check service.
        /// </summary>
        /// <param name="options"></param>
        public PasswordCheckService(IPasswordCheckOptions options)
        {
            _options = options;
        }

        /// <inheritdoc />
        public Task<PasswordValidationResult> ValidatePasswordAsync(string password)
        {
            var result = new PasswordValidationResult()
            {
                PasswordIsValid = true,
                MinPasswordLength = _options.RequiredPasswordLength,
                PasswordViolatesMinLength = false,
                PasswordMustHaveUpperCaseLetters = false,
                PasswordMustHaveLowerCaseLetters = false,
                PasswordMustHaveDigits = false,
                PasswordMustHaveSpecialCharacters = false
            };

            if (!HasSufficientLength(password))
            {
                result.PasswordIsValid = false;
                result.PasswordViolatesMinLength = true;
            }

            if (_options.RequireUpperCaseLetters && !HasUpperCaseLetter(password))
            {
                result.PasswordIsValid = false;
                result.PasswordMustHaveUpperCaseLetters = true;
            }

            if (_options.RequireLowerCaseLetters && !HasLowerCaseLetter(password))
            {
                result.PasswordIsValid = false;
                result.PasswordMustHaveLowerCaseLetters = true;
            }

            if (_options.RequireDigits && !HasDigit(password))
            {
                result.PasswordIsValid = false;
                result.PasswordMustHaveDigits = true;
            }

            if (_options.RequireSpecialCharacters && !HasSpecialCharacter(password))
            {
                result.PasswordIsValid = false;
                result.PasswordMustHaveSpecialCharacters = true;
            }

            return Task.FromResult(result);
        }

        private bool HasSufficientLength(string password)
        {
            if (password.IsNullOrEmpty())
                return false;

            return password.Length >= _options.RequiredPasswordLength;
        }

        private bool HasUpperCaseLetter(string password)
        {
            return password.Any(char.IsUpper);
        }

        private bool HasLowerCaseLetter(string password)
        {
            return password.Any(char.IsLower);
        }

        private bool HasDigit(string password)
        {
            return password.Any(char.IsDigit);
        }

        private bool HasSpecialCharacter(string password)
        {
            return password.IndexOfAny(SpecialCharacters.ToCharArray()) != -1;
        }
    }
}
