#region

using System;

#endregion

namespace VirtoCommerce.ApiClient.DataContracts.Security
{

    #region

    #endregion

    public class ApplicationUser
    {
        #region Public Properties

        /// <summary>
        ///     Used to record failures for the purposes of lockout
        /// </summary>
        public int AccessFailedCount { get; set; }

        /// <summary>
        ///     Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     True if the email is confirmed, default is false
        /// </summary>
        public bool EmailConfirmed { get; set; }

        public string FullName { get; set; }

        public string Icon { get; set; }

        //public virtual ICollection<TRole> Roles { get; }
        ///// <summary>
        ///// Navigation property for user claims
        ///// 
        ///// </summary>
        //public virtual ICollection<TClaim> Claims { get; }
        ///// <summary>
        ///// Navigation property for user logins
        ///// 
        ///// </summary>
        //public virtual ICollection<TLogin> Logins { get; }
        /// <summary>
        ///     Navigation property for user roles
        /// </summary>
        /// <summary>
        ///     User ID (Primary Key)
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Is lockout enabled for this user
        /// </summary>
        public bool LockoutEnabled { get; set; }

        /// <summary>
        ///     DateTime in UTC when lockout ends, any time in the past is considered not locked out.
        /// </summary>
        public DateTime? LockoutEndDateUtc { get; set; }

        /// <summary>
        ///     The salted/hashed form of the user password
        /// </summary>
        public string PasswordHash { get; set; }

        public string[] Permissions { get; set; }

        /// <summary>
        ///     PhoneNumber for the user
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        ///     True if the phone number is confirmed, default is false
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        ///     A random value that should change whenever a users credentials have changed (password changed, login removed)
        /// </summary>
        public string SecurityStamp { get; set; }

        public string StoreId { get; set; }

        /// <summary>
        ///     Is two factor enabled for the user
        /// </summary>
        public bool TwoFactorEnabled { get; set; }

        /// <summary>
        ///     User name
        /// </summary>
        public string UserName { get; set; }

        #endregion
    }
}
