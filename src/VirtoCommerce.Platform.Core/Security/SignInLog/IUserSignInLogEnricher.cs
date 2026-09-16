using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Security.SignInLog;

/// <summary>
/// Extension point for modules to add context the platform cannot resolve on its own.
///
/// The platform fills UserId, UserName, StoreId, MemberId, IpAddress, UserAgent, ClientId and
/// SessionId itself. It cannot fill StoreName, OrganizationId or OrganizationName: stores live in
/// the Store module and organizations in the customer module, and Platform.Security must not
/// depend on either. Those three stay null until a module registers an enricher, and the
/// dashboard hides its organization panel while they are.
///
/// A customer-module implementation looks like:
/// <code>
/// public class OrganizationSignInLogEnricher(IMemberService memberService) : IUserSignInLogEnricher
/// {
///     public int Priority => 100;
///
///     public async Task Enrich(UserSignInLog record)
///     {
///         if (record.MemberId is null) return;
///         var contact = await memberService.GetByIdAsync(record.MemberId) as Contact;
///         var organization = contact?.Organizations?.FirstOrDefault();
///         record.OrganizationId = organization;
///         record.OrganizationName = ...;
///     }
/// }
/// </code>
/// Register it as Scoped - the handler resolves enrichers in their own scope per event.
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
