using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Foundation.Data.Security.Identity;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.CoreModule.Web.Security.Models
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
