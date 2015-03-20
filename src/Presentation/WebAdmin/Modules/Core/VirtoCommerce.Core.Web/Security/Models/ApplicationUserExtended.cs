using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Foundation.Data.Security.Identity;
using VirtoCommerce.Foundation.Security.Model;

namespace VirtoCommerce.CoreModule.Web.Security.Models
{
    public class ApplicationUserExtended : ApplicationUser
    {
		public string FullName { get; set; }
        public string StoreId { get; set; }

        public string Icon { get; set; }

		public int AccountState { get; set; }

        public string[] Permissions { get; set; }

		public RegisterType UserType { get; set; }

		public ICollection<ApiAccount> ApiAcounts { get; set; }
	
    }
}
