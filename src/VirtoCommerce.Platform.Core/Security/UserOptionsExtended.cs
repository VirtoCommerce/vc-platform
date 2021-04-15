using System;

namespace VirtoCommerce.Platform.Core.Security
{
    public class UserOptionsExtended
    {
        /// <summary>
        /// Max. valid time for a password. Property that has the null as default value. Null value means what the password never expires.
        /// </summary>
        public TimeSpan? MaxPasswordAge { get; set; }

        /// <summary>
        /// The number of days to start showing password expiry reminder
        /// </summary>
        public int RemindPasswordExpiryInDays { get; set; } = 7;
    }
}
