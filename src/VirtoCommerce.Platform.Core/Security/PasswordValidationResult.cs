namespace VirtoCommerce.Platform.Core.Security
{
    /// <summary>
    /// Result of password security validation.
    /// </summary>
    public class PasswordValidationResult
    {
        /// <summary>
        /// Indicates overall password validation status.
        /// </summary>
        public bool PasswordIsValid { get; set; }

        /// <summary>
        /// Minimal password length required to pass the validation.
        /// </summary>
        public int MinPasswordLength { get; set; }

        /// <summary>
        /// Indicates that password length is less than minimal password length.
        /// </summary>
        public bool PasswordViolatesMinLength { get; set; }

        /// <summary>
        /// Indicates that entered password lacks one or more lower-case letters.
        /// </summary>
        public bool PasswordMustHaveLowerCaseLetters { get; set; }

        /// <summary>
        /// Indicates that entered password lacks one or more upper-case letters.
        /// </summary>
        public bool PasswordMustHaveUpperCaseLetters { get; set; }

        /// <summary>
        /// Indicates that entered password lacks one or more digits.
        /// </summary>
        public bool PasswordMustHaveDigits { get; set; }

        /// <summary>
        /// Indicates that entered password lacks one or more non-alphanumerical characters.
        /// </summary>
        public bool PasswordMustHaveSpecialCharacters { get; set; }
    }
}
