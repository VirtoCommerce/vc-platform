using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Security;

/// <summary>
/// Requires a second authentication factor for sign-ins, in addition to the two-factor setting of the user.
/// </summary>
public interface ITwoFactorPolicy
{
    /// <summary>
    /// Decides whether the user must pass a second factor to sign in.
    /// The sign-in context, such as the token request parameters, is available from the current HTTP request.
    /// </summary>
    /// <param name="user">The user signing in.</param>
    Task<bool> IsTwoFactorRequiredAsync(ApplicationUser user);
}
