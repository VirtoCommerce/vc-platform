using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VirtoCommerce.Platform.Core.Security
{
    public class ApplicationUserExtended// : ApplicationUser
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

        public ApplicationUserLogin[] Logins { get; set; }
        public Role[] Roles { get; set; }
        public string[] Permissions { get; set; }
        public ApiAccount[] ApiAcounts { get; set; }
    }
}
