using System;

namespace VirtoCommerce.Web.Core.DataContracts.Security
{
    public class ApplicationUser
    {
        /// <summary>
        /// Email
        /// 
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// True if the email is confirmed, default is false
        /// 
        /// </summary>
        public bool EmailConfirmed { get; set; }
        /// <summary>
        /// The salted/hashed form of the user password
        /// 
        /// </summary>
        public string PasswordHash { get; set; }
        /// <summary>
        /// A random value that should change whenever a users credentials have changed (password changed, login removed)
        /// 
        /// </summary>
        public string SecurityStamp { get; set; }
        /// <summary>
        /// PhoneNumber for the user
        /// 
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// True if the phone number is confirmed, default is false
        /// 
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; }
        /// <summary>
        /// Is two factor enabled for the user
        /// 
        /// </summary>
        public bool TwoFactorEnabled { get; set; }
        /// <summary>
        /// DateTime in UTC when lockout ends, any time in the past is considered not locked out.
        /// 
        /// </summary>
        public DateTime? LockoutEndDateUtc { get; set; }
        /// <summary>
        /// Is lockout enabled for this user
        /// 
        /// </summary>
        public bool LockoutEnabled { get; set; }
        /// <summary>
        /// Used to record failures for the purposes of lockout
        /// 
        /// </summary>
        public int AccessFailedCount { get; set; }
        /// <summary>
        /// Navigation property for user roles
        /// 
        /// </summary>
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
        /// User ID (Primary Key)
        /// 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// User name
        /// 
        /// </summary>
        public string UserName { get; set; }

        #region Extensions

        public string FullName { get; set; }

        public string StoreId { get; set; }

        public string Icon { get; set; }

        public string[] Permissions { get; set; }

        #endregion
    }
}
