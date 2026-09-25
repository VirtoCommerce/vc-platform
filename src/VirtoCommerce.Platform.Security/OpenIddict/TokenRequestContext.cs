using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.SignInLog;

namespace VirtoCommerce.Platform.Security.OpenIddict
{
    public class TokenRequestContext
    {
        public string AuthenticationScheme { get; set; }

        public OpenIddictRequest Request { get; set; }

        public DelayedResponse DelayedResponse { get; set; }

        public GrantValidationResult GrantValidationResult { get; set; }

        /// <summary>
        /// Precise reason for the sign-in log, one of <see cref="SignInFailureReason"/>, even when the client gets a generic error.
        /// </summary>
        public string FailureReason { get; set; }

        public SignInResult SignInResult { get; set; }

        public ApplicationUser User { get; set; }

        public ClaimsPrincipal Principal { get; set; }

        public AuthenticationProperties Properties { get; set; }

        public bool DetailedErrors { get; set; }

        public IDictionary<string, object> AdditionalParameters { get; set; } = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
    }
}
