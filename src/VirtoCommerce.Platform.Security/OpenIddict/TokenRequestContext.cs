using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Security.OpenIddict
{
    public class TokenRequestContext
    {
        public string AuthenticationScheme { get; set; }

        public OpenIddictRequest Request { get; set; }

        public SignInResult SignInResult { get; set; }

        public ApplicationUser User { get; set; }

        public ClaimsPrincipal Principal { get; set; }

        public AuthenticationProperties Properties { get; set; }

        public bool DetailedErrors { get; set; }

        public IDictionary<string, object> AdditionalParameters { get; set; } = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
    }
}
