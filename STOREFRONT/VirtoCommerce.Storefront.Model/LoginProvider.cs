using System.Collections.Generic;

namespace VirtoCommerce.Storefront.Model
{
    public class LoginProvider
    {
        public string AuthenticationType { get; set; }

        public string Caption { get; set; }

        public IDictionary<string, object> Properties { get; set; }
    }
}