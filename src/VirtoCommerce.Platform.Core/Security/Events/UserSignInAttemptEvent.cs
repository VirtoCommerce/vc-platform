using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.Platform.Core.Security.Events
{
    /// <summary>
    /// Raised for every sign-in attempt, successful or failed, at every authentication entry point.
    /// Separate from <see cref="UserLoginEvent"/> because that one requires an <see cref="ApplicationUser"/>
    /// and the most security-relevant failure — unknown user name — has no user object at all.
    /// <see cref="UserLoginEvent"/> and <see cref="UserLogoutEvent"/> keep their existing behaviour and
    /// are still published alongside this one.
    /// </summary>
    public class UserSignInAttemptEvent : DomainEvent
    {
        /// <summary>User name exactly as submitted.</summary>
        public string UserName { get; set; }

        /// <summary>Null when the account does not exist.</summary>
        public string UserId { get; set; }

        public bool Succeeded { get; set; }

        /// <summary>One of <see cref="SignInFailureReason"/>. Null when <see cref="Succeeded"/>.</summary>
        public string FailureReason { get; set; }

        /// <summary>One of <see cref="Security.SignInType"/>.</summary>
        public string SignInType { get; set; }

        public string Provider { get; set; }

        public string OperatorUserId { get; set; }

        public string OperatorUserName { get; set; }

        public string ClientId { get; set; }

        public string SessionId { get; set; }

        /// <summary>Set from the signed-in user when one exists; null for unknown-user attempts.</summary>
        public string StoreId { get; set; }

        public string MemberId { get; set; }
    }
}
