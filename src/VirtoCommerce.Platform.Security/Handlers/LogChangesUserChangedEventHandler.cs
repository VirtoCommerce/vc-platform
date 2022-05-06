using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;

namespace VirtoCommerce.Platform.Security.Handlers
{
    public class LogChangesUserChangedEventHandler : IEventHandler<UserChangedEvent>, IEventHandler<UserLoginEvent>,
                                                     IEventHandler<UserLogoutEvent>, IEventHandler<UserPasswordChangedEvent>,
                                                     IEventHandler<UserResetPasswordEvent>, IEventHandler<UserRoleAddedEvent>,
                                                     IEventHandler<UserRoleRemovedEvent>
    {
        private readonly IChangeLogService _changeLogService;

        public LogChangesUserChangedEventHandler(IChangeLogService changeLogService)
        {
            _changeLogService = changeLogService;
        }

        public virtual async Task Handle(UserChangedEvent message)
        {
            foreach (var changedEntry in message.ChangedEntries)
            {
                if (changedEntry.EntryState == EntryState.Added)
                {
                    await SaveOperationLogAsync(changedEntry.NewEntry.Id, "Created", EntryState.Added);
                }
                else if (changedEntry.EntryState == EntryState.Modified)
                {
                    var changes = DetectAccountChanges(changedEntry.NewEntry, changedEntry.OldEntry);
                    foreach (var key in changes.Keys)
                    {
                        await SaveOperationLogAsync(changedEntry.NewEntry.Id, string.Join(", ", changes[key].ToArray()), EntryState.Modified);
                    }
                }
                else if (changedEntry.EntryState == EntryState.Deleted)
                {
                    await SaveOperationLogAsync(changedEntry.OldEntry.Id, null, EntryState.Deleted);
                }
            }
        }

        public virtual Task Handle(UserLoginEvent message)
        {
            return Task.CompletedTask;
        }

        public virtual Task Handle(UserLogoutEvent message)
        {
            return Task.CompletedTask;
        }

        public virtual async Task Handle(UserPasswordChangedEvent message)
        {
            await SaveOperationLogAsync(message.UserId, "Password changed", EntryState.Modified);
        }

        public virtual async Task Handle(UserResetPasswordEvent message)
        {
            await SaveOperationLogAsync(message.UserId, "Password resets", EntryState.Modified);
        }

        public virtual Task Handle(UserRoleAddedEvent message)
        {
            return SaveOperationLogAsync(message.User.Id, $"Role added {message.Role}", EntryState.Modified);
        }

        public Task Handle(UserRoleRemovedEvent message)
        {
            return SaveOperationLogAsync(message.User.Id, $"Role removed {message.Role}", EntryState.Modified);
        }

        protected virtual ListDictionary<string, string> DetectAccountChanges(ApplicationUser newUser, ApplicationUser oldUser)
        {
            return newUser.DetectUserChanges(oldUser);
        }

        protected virtual async Task SaveOperationLogAsync(string objectId, string detail, EntryState entryState)
        {
            var operation = new OperationLog
            {
                ObjectId = objectId,
                ObjectType = typeof(ApplicationUser).Name,
                OperationType = entryState,
                Detail = detail
            };
            await _changeLogService.SaveChangesAsync(operation);
        }
    }
}
