using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;

namespace VirtoCommerce.Platform.Security.Handlers;

public class RevokeUserTokenEventHandler(Func<(IUserSessionsService SessionService, IServiceScope Scope)> userSessionsServiceFactory) :
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
                    await TerminateAllUserSessions(changedEntry.NewEntry.Id);
                }
            }
            else if (changedEntry.EntryState == EntryState.Deleted)
            {
                await TerminateAllUserSessions(changedEntry.NewEntry.Id);
            }
        }
    }

    public virtual Task Handle(UserPasswordChangedEvent message) => TerminateAllUserSessions(message.UserId);
    public virtual Task Handle(UserResetPasswordEvent message) => TerminateAllUserSessions(message.UserId);
    public virtual Task Handle(UserChangedPasswordEvent message) => TerminateAllUserSessions(message.UserId);

    private async Task TerminateAllUserSessions(string userId)
    {
        var (SessionService, Scope) = userSessionsServiceFactory();
        using var scope = Scope;
        await SessionService.TerminateAllUserSessions(userId);
    }
}
