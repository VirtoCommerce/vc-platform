using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model
{
    public class Login : ValueObject<Login>
    {
        
        public string Email { get; set; }

        public bool UsernamesEnabled { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public bool DisplayCaptcha { get; set; }
    }
}
