using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Jobs;

/// <summary>
/// Fallback <see cref="IBackgroundJobProcessor"/> registered by the platform (via <c>TryAdd</c>) when no
/// background-job engine module is installed. Every enqueue throws
/// <see cref="BackgroundJobEngineNotInstalledException"/> so callers fail fast with an actionable message
/// instead of silently dropping work. When an engine module IS installed, its real registration wins and
/// this fallback is never used.
/// </summary>
public sealed class NoEngineBackgroundJobProcessor : IBackgroundJobProcessor
{
    public string Enqueue(Expression<Action> methodCall) => throw new BackgroundJobEngineNotInstalledException();

    public string Enqueue(Expression<Func<Task>> methodCall) => throw new BackgroundJobEngineNotInstalledException();
}
