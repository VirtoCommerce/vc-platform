using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Security
{
    public class ApplicationUser : IdentityUser, IEntity, IAuditable
    {
        /// <summary>
        /// Tenant id
        /// </summary>
        public virtual string StoreId { get; set; }
        public virtual string MemberId { get; set; }
        public virtual bool IsAdministrator { get; set; }
        public virtual string PhotoUrl { get; set; }
        public virtual string UserType { get; set; }
        public virtual string Password { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual IList<Role> Roles { get; set; }

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
                LockoutEnd = value ?? new DateTimeOffset(value.Value);
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
        /// Obsolete. External provider logins.
        /// </summary>
        [Obsolete("Left due to compatibility issues")]
        public virtual ApplicationUserLogin[] Logins { get; set; }

        /// <summary>
        /// Indicates that the password for this user is expired and must be changed.
        /// </summary>
        public virtual bool PasswordExpired { get; set; }


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
            target.TwoFactorEnabled = TwoFactorEnabled;
            target.LockoutEnabled = LockoutEnabled;
            target.LockoutEnd = LockoutEnd;
            target.UserState = UserState;
            target.AccessFailedCount = AccessFailedCount;

            target.MemberId = MemberId;
            target.StoreId = StoreId;
            target.PhotoUrl = PhotoUrl;
            target.UserType = UserType;
            target.Password = Password;
            target.PasswordExpired = PasswordExpired;

            target.CreatedDate = CreatedDate;
            target.ModifiedDate = ModifiedDate;
            target.CreatedBy = CreatedBy;
            target.ModifiedBy = ModifiedBy;

            if (!Roles.IsNullOrEmpty())
            {
                Roles.Patch(target.Roles, (sourcePhone, targetPhone) => sourcePhone.Patch(targetPhone));
            }
        }
    }
}
