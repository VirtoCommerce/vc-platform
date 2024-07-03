using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;

namespace VirtoCommerce.Platform.Security.OpenIddict
{
    public class TokenResponse : OpenIddictResponse
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
