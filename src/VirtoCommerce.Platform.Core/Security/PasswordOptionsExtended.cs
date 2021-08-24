using System;

namespace VirtoCommerce.Platform.Core.Security
{
    public class PasswordOptionsExtended
    {
        /// <summary>
        /// The number of recent user's passwords to check during the password validation. Old password can't be reused for this number of cycles. Value of "0" or not defined - password history disabled.
        /// </summary>
        public int? PasswordHistory { get; set; }

        /// <summary>
        /// The time limit after which a user can request a password reset.
        /// </summary>
        public TimeSpan RepeatedResetPasswordTimeLimit { get; set; } = TimeSpan.FromMinutes(1);
    }
}
