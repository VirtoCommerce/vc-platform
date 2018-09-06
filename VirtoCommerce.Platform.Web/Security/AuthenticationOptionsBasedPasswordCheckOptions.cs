using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Security;

namespace VirtoCommerce.Platform.Web.Security
{
    /// <summary>
    /// Implementation of <see cref="IPasswordCheckService"/> that gets password validation options from
    /// <see cref="AuthenticationOptions"/>.
    /// </summary>
    public class AuthenticationOptionsBasedPasswordCheckOptions : IPasswordCheckOptions
    {
        private readonly AuthenticationOptions _authenticationOptions;

        /// <summary>
        /// Creates new instance of <see cref="AuthenticationOptionsBasedPasswordCheckOptions"/>.
        /// </summary>
        /// <param name="authenticationOptions">VirtoCommerce authentication options that contain
        /// password validation options.</param>
        public AuthenticationOptionsBasedPasswordCheckOptions(AuthenticationOptions authenticationOptions)
        {
            _authenticationOptions = authenticationOptions;
        }

        /// <inheritdoc />
        public int RequiredPasswordLength => _authenticationOptions.PasswordRequiredLength;

        /// <inheritdoc />
        public bool RequireUpperCaseLetters => _authenticationOptions.PasswordRequireUppercase;

        /// <inheritdoc />
        public bool RequireLowerCaseLetters => _authenticationOptions.PasswordRequireLowercase;

        /// <inheritdoc />
        public bool RequireDigits => _authenticationOptions.PasswordRequireDigit;

        /// <inheritdoc />
        public bool RequireSpecialCharacters => _authenticationOptions.PasswordRequireNonLetterOrDigit;
    }
}
