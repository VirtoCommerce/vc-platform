using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Security
{
    public class ApplicationUserExtended : AuditableEntity, IHasChangesHistory, ICloneable

    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        /// <summary>
        ///  True if the email is confirmed, default is false
        /// </summary>
        public bool EmailConfirmed { get; set; }
        /// <summary>
        /// True if the phone number is confirmed, default is false
        /// </summary>     
        public virtual bool PhoneNumberConfirmed { get; set; }
        /// <summary>
        ///   Is two factor enabled for the user
        /// </summary>
        public virtual bool TwoFactorEnabled { get; set; }
        /// <summary>
        /// DateTime in UTC when lockout ends, any time in the past is considered not locked out.
        /// </summary>
        public virtual DateTime? LockoutEndDateUtc { get; set; }

        /// <summary>
        /// Is lockout enabled for this user
        /// </summary>
        public virtual bool LockoutEnabled { get; set; }

        /// <summary>
        ///  Used to record failures for the purposes of lockout
        /// </summary>
        public virtual int AccessFailedCount { get; set; }

        /// <summary>
        /// Tenant id
        /// </summary>
        public string StoreId { get; set; }
        public string MemberId { get; set; }
        public string Icon { get; set; }

        public bool IsAdministrator { get; set; }

        public string UserType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public AccountState UserState { get; set; }

        public string Password { get; set; }
        /// <summary>
        /// The flag indicates that user password is expired and must be changed
        /// </summary>
        public bool PasswordExpired { get; set; }

        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }

        /// <summary>
        /// External provider logins.
        /// </summary>
        public ApplicationUserLogin[] Logins { get; set; }

        /// <summary>
        /// Assigned roles.
        /// </summary>
        public Role[] Roles { get; set; }

        /// <summary>
        /// All permissions from assigned roles.
        /// </summary>
        public string[] Permissions { get; set; }

        /// <summary>
        /// API keys
        /// </summary>
        public ApiAccount[] ApiAccounts { get; set; }

        #region IHasChangesHistory
        public ICollection<OperationLog> OperationsLog { get; set; }
        #endregion

        public virtual object Clone()
        {
            var clone = (ApplicationUserExtended) MemberwiseClone();

            clone.Logins = Logins?.Select(x => x.Clone() as ApplicationUserLogin).ToArray();
            clone.Roles = Roles?.Select(x => x.Clone() as Role).ToArray();
            clone.Permissions = Permissions?.ToArray();
            clone.ApiAccounts = ApiAccounts?.Select(x => x.Clone() as ApiAccount).ToArray();
            clone.OperationsLog = OperationsLog?.Select(x => x.Clone() as OperationLog).ToArray();

            return clone;
        }
    }
}
