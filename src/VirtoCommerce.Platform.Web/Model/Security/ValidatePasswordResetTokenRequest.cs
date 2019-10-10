using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Web.Model.Security
{
    public class ValidatePasswordResetTokenRequest
    {
        public string Token { get; set; }
    }
}
