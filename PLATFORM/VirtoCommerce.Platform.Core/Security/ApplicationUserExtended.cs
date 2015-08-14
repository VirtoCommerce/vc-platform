using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VirtoCommerce.Platform.Core.Security
{
    public class ApplicationUserExtended
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string StoreId { get; set; }
        public string MemberId { get; set; }
        public string Icon { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public UserType UserType { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public UserState UserState { get; set; }
        public string Password { get; set; }

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
    }
}
