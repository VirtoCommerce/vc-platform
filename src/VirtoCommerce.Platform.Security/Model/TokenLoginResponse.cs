using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;

namespace VirtoCommerce.Platform.Security.Model
{
    [Obsolete("Use VirtoCommerce.Platform.Security.OpenIddict.TokenResponse", DiagnosticId = "VC0008", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions/")]
    public class TokenLoginResponse : OpenIddictResponse
    {
        public string UserId { get; set; }

        public IList<IdentityError> Errors
        {
            get
            {
                var errors = new List<IdentityError>();
                if (Code != null)
                {
                    errors.Add(new IdentityError
                    {
                        Code = Code,
                        Description = ErrorDescription
                    });
                }
                return errors;
            }
        }
    }
}
