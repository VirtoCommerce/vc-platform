using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Utils.ChangeDetector;

namespace VirtoCommerce.Platform.Core.Security
{
    public class ApplicationUser : IdentityUser, IEntity, IAuditable, ICloneable
    {
        /// <summary>
        /// Tenant id
        /// </summary>
        public virtual string StoreId { get; set; }

        [DetectChangesAttribute(PlatformConstants.Security.Changes.UserUpdated)]
        public virtual string MemberId { get; set; }

        [DetectChangesAttribute(PlatformConstants.Security.Changes.UserUpdated)]
        public virtual bool IsAdministrator { get; set; }
        public virtual string PhotoUrl { get; set; }

        [DetectChangesAttribute(PlatformConstants.Security.Changes.UserUpdated)]
        public virtual string UserType { get; set; }

        [DetectChangesAttribute(PlatformConstants.Security.Changes.UserUpdated)]
        public virtual string Status { get; set; }
        public virtual string Password { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual IList<Role> Roles { get; set; }

        [SwaggerIgnore]
        public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; }

        /// <summary>
        /// Obsolete. Use LockoutEnd. DateTime in UTC when lockout ends, any time in the past is considered not locked out.
        /// </summary>
        [Obsolete("Left due to compatibility issues. Use LockoutEnd")]
        public virtual DateTime? LockoutEndDateUtc
        {
            get
            {
                return LockoutEnd?.UtcDateTime;
            }
            set
            {
                LockoutEnd = value;
            }
        }

        [Obsolete("Left due to compatibility issues. Will be removed. Instead of, use properties: EmailConfirmed, LockoutEnd.")]
        [JsonConverter(typeof(StringEnumConverter))]
        public virtual AccountState UserState { get; set; }

        /// <summary>
        /// Obsolete. All permissions from assigned roles.
        /// </summary>
        [Obsolete("Left due to compatibility issues")]
        public virtual string[] Permissions { get; set; }

        /// <summary>
        /// External provider logins.
        /// </summary>
        public virtual ApplicationUserLogin[] Logins { get; set; }

        /// <summary>
        /// Indicates that the password for this user is expired and must be changed.
        /// </summary>
        public virtual bool PasswordExpired { get; set; }

        /// <summary>
        /// The last date when the password was changed
        /// </summary>
        public virtual DateTime? LastPasswordChangedDate { get; set; }

        /// <summary>
        /// The last date when the requested password change.
        /// </summary>
        public virtual DateTime? LastPasswordChangeRequestDate { get; set; }

        public virtual void Patch(ApplicationUser target)
        {
            target.UserName = UserName;
            target.IsAdministrator = IsAdministrator;
            target.Email = Email;
            target.NormalizedEmail = NormalizedEmail;
            target.NormalizedUserName = NormalizedUserName;
            target.EmailConfirmed = EmailConfirmed;
            target.PasswordHash = PasswordHash;
            target.SecurityStamp = SecurityStamp;
            target.PhoneNumberConfirmed = PhoneNumberConfirmed;
            target.PhoneNumber = PhoneNumber;
            target.TwoFactorEnabled = TwoFactorEnabled;
            target.LockoutEnabled = LockoutEnabled;
            target.LockoutEnd = LockoutEnd;
            target.UserState = UserState;
            target.AccessFailedCount = AccessFailedCount;

            target.MemberId = MemberId;
            target.StoreId = StoreId;
            target.PhotoUrl = PhotoUrl;
            target.UserType = UserType;
            target.Status = Status;
            target.Password = Password;
            target.PasswordExpired = PasswordExpired;
            target.LastPasswordChangedDate = LastPasswordChangedDate;
            target.LastPasswordChangeRequestDate = LastPasswordChangeRequestDate;
        }

        public virtual ListDictionary<string, string> DetectUserChanges(ApplicationUser oldUser)
        {
            var newUser = this;
            // Gather all the changes from ApplicationUser and its possible descendants
            var result = ChangesDetector.Gather(newUser, oldUser);

            //Next: gather the changes manually from specific properties of ApplicationUser's ancestor
            if (newUser.UserName != oldUser.UserName)
            {
                result.Add(PlatformConstants.Security.Changes.UserUpdated, $"Changes: user name: {oldUser.UserName} -> {newUser.UserName}");
            }

            if (newUser.Email != oldUser.Email)
            {
                result.Add(PlatformConstants.Security.Changes.UserUpdated, $"Changes: email: {oldUser.Email} -> {newUser.Email}");
            }

            if (newUser.PasswordHash != oldUser.PasswordHash)
            {
                result.Add(PlatformConstants.Security.Changes.UserPasswordChanged, $"Password changed");
            }

            return result;
        }

        #region ICloneable members

        public virtual object Clone()
        {
            var result = MemberwiseClone() as ApplicationUser;

            result.Roles = Roles?.Select(x => x.Clone()).OfType<Role>().ToList();

            return result;
        }

        #endregion
    }
}
