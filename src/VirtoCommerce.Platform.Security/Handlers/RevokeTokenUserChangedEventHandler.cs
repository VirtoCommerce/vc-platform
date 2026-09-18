using System;
using System.Threading.Tasks;
using OpenIddict.Abstractions;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;

namespace VirtoCommerce.Platform.Security.Handlers;

public class RevokeUserTokenEventHandler(IScopedServiceFactory<IUserSessionsService> userSessionsServiceFactory) :
    IEventHandler<UserChangedEvent>
{
    public virtual async Task Handle(UserChangedEvent message)
    {
        foreach (var changedEntry in message.ChangedEntries)
        {
            if (changedEntry.EntryState == EntryState.Added || changedEntry.EntryState == EntryState.Modified)
            {
                if (!changedEntry.NewEntry.LockoutEnd.IsEmpty())
                {
                    await RevokeUserTokensAsync(changedEntry.NewEntry.Id);
                }
            }
            else if (changedEntry.EntryState == EntryState.Deleted)
            {
                await RevokeUserTokensAsync(changedEntry.NewEntry.Id);
            }
        }
    }

    protected virtual async Task RevokeUserTokensAsync(string userId)
    {
        using var scopedSessionService = userSessionsServiceFactory.Create();
        await scopedSessionService.Service.TerminateAllUserSessions(userId);
    }

    [Obsolete("Use RevokeUserTokensAsync(string userId) class instead.", DiagnosticId = "VC0014", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    protected virtual async Task RevokeUserTokensAsync(IOpenIddictTokenManager tokenManager, string userId)
    {
        await foreach (var token in tokenManager.FindBySubjectAsync(userId))
        {
            await tokenManager.TryRevokeAsync(token);
        }
    }
}
