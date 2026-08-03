using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Security
{
    /// <summary>
    /// Decides whether an authenticated user may enter the admin UI.
    /// This is independent of authorization: a denied user keeps every permission
    /// they hold and can still call the API endpoints those permissions grant.
    /// </summary>
    public interface IAdminUIAccessPolicy
    {
        /// <param name="context">The user and the permissions to evaluate.</param>
        /// <param name="cancellationToken">
        /// Honored by implementations that need to look up additional data.
        /// The built-in configuration policy is purely in-memory.
        /// </param>
        Task<AdminUIAccessResult> EvaluateAsync(AdminUIAccessContext context, CancellationToken cancellationToken = default);
    }
}
