using DotLiquid;
using System.Collections.Generic;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    public class LoginProvider : Drop
    {
        public string AuthenticationType { get; set; }

        public string Caption { get; set; }

        public IDictionary<string, object> Properties { get; set; }
    }
}