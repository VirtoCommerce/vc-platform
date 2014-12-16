using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Data.Security.Identity;

namespace VirtoCommerce.CoreModule.Web.Security.Models
{
    public class ApplicationUserExtended : ApplicationUser
    {
        public string FullName { get; set; }

        public string StoreId { get; set; }

        public string Icon { get; set; }

        public string[] Permissions { get; set; }
    }
}
