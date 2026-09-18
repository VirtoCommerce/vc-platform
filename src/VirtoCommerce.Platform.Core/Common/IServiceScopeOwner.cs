using Microsoft.Extensions.DependencyInjection;

namespace VirtoCommerce.Platform.Core.Common;

// Implemented by disposable services produced by IServiceProvider.ResolveInOwnScope<T>():
// the service disposes the scope it was resolved from together with itself.
public interface IServiceScopeOwner
{
    void OwnScope(IServiceScope scope);
}
