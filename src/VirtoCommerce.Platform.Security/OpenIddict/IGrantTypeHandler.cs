using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VirtoCommerce.Platform.Security.OpenIddict
{
    /// <summary>
    /// Lets a module take over a custom OAuth grant type at "/connect/token". Register with
    /// <see cref="ServiceCollectionExtensions.AddGrantTypeHandler{THandler}"/>; the platform picks the
    /// handler whose <see cref="GrantType"/> matches the request.
    /// </summary>
    public interface IGrantTypeHandler
    {
        /// <summary>
        /// The "grant_type" value this handler takes over.
        /// </summary>
        string GrantType { get; }

        /// <summary>
        /// Handles the token request and returns the HTTP response for it - a sign-in, an error, or
        /// whatever else this grant needs to return.
        /// </summary>
        Task<ActionResult> HandleAsync(TokenRequestContext context);
    }
}
