using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core;

/// <summary>
/// Builds the <see cref="AdminUIContext"/> for the admin UI module that hosts the back office.
/// </summary>
public interface IAdminUIContextProvider
{
    Task<AdminUIContext> GetContextAsync(CancellationToken cancellationToken = default);
}
