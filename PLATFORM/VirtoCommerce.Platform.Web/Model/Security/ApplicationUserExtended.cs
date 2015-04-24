using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Security.Identity;

namespace VirtoCommerce.Platform.Web.Model.Security
{
    public class ApplicationUserExtended : ApplicationUser
    {
        public string StoreId { get; set; }

        public string Icon { get; set; }

        public string MemberId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public UserState UserState { get; set; }

        public string[] Permissions { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public UserType UserType { get; set; }

        public ICollection<ApiAccount> ApiAcounts { get; set; }

        public string Password { get; set; }

        public new RoleDescriptor[] Roles { get; set; }
    }
}
