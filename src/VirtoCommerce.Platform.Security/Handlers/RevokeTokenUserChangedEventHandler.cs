using System;
using System.Threading.Tasks;
using OpenIddict.Abstractions;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security.Events;

namespace VirtoCommerce.Platform.Security.Handlers;

public class RevokeUserTokenEventHandler(Func<IOpenIddictTokenManager> tokenManagerFactory) :
    IEventHandler<UserChangedEvent>
{
    private readonly Func<IOpenIddictTokenManager> _tokenManagerFactory = tokenManagerFactory;

    public virtual async Task Handle(UserChangedEvent message)
    {
        var tokenManager = _tokenManagerFactory();

        foreach (var changedEntry in message.ChangedEntries)
        {
            if (changedEntry.EntryState == EntryState.Added || changedEntry.EntryState == EntryState.Modified)
            {
                if (!changedEntry.NewEntry.LockoutEnd.IsEmpty())
                {
                    await RevokeUserTokensAsync(tokenManager, changedEntry.NewEntry.Id);
                }
            }
            else if (changedEntry.EntryState == EntryState.Deleted)
            {
                await RevokeUserTokensAsync(tokenManager, changedEntry.NewEntry.Id);
            }
        }
    }

    protected virtual async Task RevokeUserTokensAsync(IOpenIddictTokenManager tokenManager, string userId)
    {
        await foreach (var token in tokenManager.FindBySubjectAsync(userId))
        {
            await tokenManager.TryRevokeAsync(token);
        }
    }
}
