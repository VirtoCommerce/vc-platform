using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Security.SignInLog;

/// <summary>
/// Extension point for modules to add context the platform cannot resolve on its own.
/// Organization is the motivating case: the platform knows MemberId, but resolving it to an
/// organization lives in the customer module, and Platform.Security must not depend on a module.
/// Implementations run in <see cref="Priority"/> order and must be cheap — they sit on the
/// audit write path.
/// </summary>
public interface IUserSignInLogEnricher
{
    int Priority { get; }

    Task Enrich(UserSignInLog record);
}
