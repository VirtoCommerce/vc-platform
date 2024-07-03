using System;
using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Security.Model
{
    [Obsolete("Use VirtoCommerce.Platform.Security.OpenIddict.TokenRequestContext", DiagnosticId = "VC0008", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions/")]
    public class SignInValidatorContext
    {
        public ApplicationUser User { get; set; }

        public string StoreId { get; set; }

        public bool DetailedErrors { get; set; }

        public bool IsSucceeded { get; set; }

        public bool IsLockedOut { get; set; }

        public IDictionary<string, object> AdditionalParameters { get; set; } = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
    }
}
