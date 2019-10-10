using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
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
