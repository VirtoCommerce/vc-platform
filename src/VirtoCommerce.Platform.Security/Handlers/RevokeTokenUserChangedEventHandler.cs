using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;

namespace VirtoCommerce.Platform.Security.Handlers;

public class RevokeUserTokenEventHandler(IUserSessionsService userSessionsService) :
    IEventHandler<UserChangedEvent>,
    IEventHandler<UserPasswordChangedEvent>,
    IEventHandler<UserResetPasswordEvent>,
    IEventHandler<UserChangedPasswordEvent>
{
    public virtual async Task Handle(UserChangedEvent message)
    {
        foreach (var changedEntry in message.ChangedEntries)
        {
            if (changedEntry.EntryState == EntryState.Added || changedEntry.EntryState == EntryState.Modified)
            {
                if (!changedEntry.NewEntry.LockoutEnd.IsEmpty())
                {
                    await userSessionsService.TerminateAllUserSessions(changedEntry.NewEntry.Id);
                }
            }
            else if (changedEntry.EntryState == EntryState.Deleted)
            {
                await userSessionsService.TerminateAllUserSessions(changedEntry.NewEntry.Id);
            }
        }
    }

    public virtual async Task Handle(UserPasswordChangedEvent message)
    {
        await userSessionsService.TerminateAllUserSessions(message.UserId);
    }

    public virtual async Task Handle(UserResetPasswordEvent message)
    {
        await userSessionsService.TerminateAllUserSessions(message.UserId);
    }

    public virtual async Task Handle(UserChangedPasswordEvent message)
    {
        await userSessionsService.TerminateAllUserSessions(message.UserId);
    }
}
