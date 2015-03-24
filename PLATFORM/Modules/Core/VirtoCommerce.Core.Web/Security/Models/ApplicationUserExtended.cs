using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Foundation.Data.Security.Identity;

namespace VirtoCommerce.CoreModule.Web.Security.Models
{
    public class ApplicationUserExtended : ApplicationUser
    {
	    public string StoreId { get; set; }

        public string Icon { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public UserState UserState { get; set; }

        public string[] Permissions { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
		public UserType UserType { get; set; }

		public ICollection<ApiAccount> ApiAcounts { get; set; }

		public string Password { get; set; }
	
    }
}
