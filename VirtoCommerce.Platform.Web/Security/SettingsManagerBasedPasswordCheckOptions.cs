using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Security;

namespace VirtoCommerce.Platform.Web.Security
{
    public class SettingsManagerBasedPasswordCheckOptions : IPasswordCheckOptions
    {
        private readonly ISettingsManager _settingsManager;
        private readonly string _minPasswordLengthPropertyName;
        private readonly string _requireUpperCaseLettersPropertyName;
        private readonly string _requireLowerCaseLettersPropertyName;
        private readonly string _requireDigitsPropertyName;
        private readonly string _requireSpecialCharactersPropertyName;

        public SettingsManagerBasedPasswordCheckOptions(ISettingsManager settingsManager, string minPasswordLengthPropertyName,
            string requireUpperCaseLettersPropertyName, string requireLowerCaseLettersPropertyName, string requireDigitsPropertyName,
            string requireSpecialCharactersPropertyName)
        {
            _settingsManager = settingsManager;

            _minPasswordLengthPropertyName = minPasswordLengthPropertyName;
            _requireUpperCaseLettersPropertyName = requireUpperCaseLettersPropertyName;
            _requireLowerCaseLettersPropertyName = requireLowerCaseLettersPropertyName;
            _requireDigitsPropertyName = requireDigitsPropertyName;
            _requireSpecialCharactersPropertyName = requireSpecialCharactersPropertyName;
        }

        /// <inheritdoc />
        public int RequiredPasswordLength => _settingsManager.GetValue(_minPasswordLengthPropertyName, 8);

        /// <inheritdoc />
        public bool RequireUpperCaseLetters => _settingsManager.GetValue(_requireUpperCaseLettersPropertyName, true);

        /// <inheritdoc />
        public bool RequireLowerCaseLetters => _settingsManager.GetValue(_requireLowerCaseLettersPropertyName, true);

        /// <inheritdoc />
        public bool RequireDigits => _settingsManager.GetValue(_requireDigitsPropertyName, true);

        /// <inheritdoc />
        public bool RequireSpecialCharacters => _settingsManager.GetValue(_requireSpecialCharactersPropertyName, true);
    }
}
