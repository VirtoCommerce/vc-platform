using System;
using System.Collections.Generic;

namespace VirtoCommerce.ApiClient.DataContracts.Security
{
    public class ApplicationUser
    {
        public int AccessFailedCount { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string Id { get; set; }

        public bool LockoutEnabled { get; set; }

        public DateTime? LockoutEndDateUtc { get; set; }

        public ICollection<UserLoginInfo> Logins { get; set; }

        public string PasswordHash { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public string SecurityStamp { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public string UserName { get; set; }

        public string StoreId { get; set; }

        public string Icon { get; set; }

        public string MemberId { get; set; }

        public string UserState { get; set; }

        public string UserType { get; set; }

        public string Password { get; set; }
    }
}