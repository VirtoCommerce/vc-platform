using System.Threading.Tasks;
using OpenIddict.Abstractions;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security.Events;

namespace VirtoCommerce.Platform.Security.Handlers;

public class RevokeUserTokenEventHandler(IOpenIddictTokenManager tokenManager) :
    IEventHandler<UserChangedEvent>
{
    private readonly IOpenIddictTokenManager _tokenManager = tokenManager;

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
        await foreach (var token in _tokenManager.FindBySubjectAsync(userId))
        {
            await _tokenManager.TryRevokeAsync(token);
        }
    }
}
