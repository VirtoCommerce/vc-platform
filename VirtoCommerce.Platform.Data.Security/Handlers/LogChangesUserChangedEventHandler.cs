using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Data.Security.Resources;

namespace VirtoCommerce.Platform.Data.Security.Handlers
{
    public class LogChangesUserChangedEventHandler : IEventHandler<UserChangedEvent>, IEventHandler<UserLoginEvent>,
                                                     IEventHandler<UserLogoutEvent>, IEventHandler<UserPasswordChangedEvent>,
                                                     IEventHandler<UserResetPasswordEvent>
    {
        private readonly IChangeLogService _changeLogService;
        public LogChangesUserChangedEventHandler(IChangeLogService changeLogService)
        {
            _changeLogService = changeLogService;
        }

        public virtual Task Handle(UserChangedEvent message)
        {
            if(message.ChangedEntry.EntryState == EntryState.Added)
            {
                SaveOperationLog(message.ChangedEntry.NewEntry.Id, SecurityAccountChangesResource.AccountCreatedMessage, EntryState.Added);
            }
            else if(message.ChangedEntry.EntryState == EntryState.Modified)
            {
                var changes = DetectAccountChanges(message.ChangedEntry.NewEntry, message.ChangedEntry.OldEntry);
                foreach (var key in changes.Keys)
                {
                    SaveOperationLog(message.ChangedEntry.NewEntry.Id, string.Format(key, string.Join(", ", changes[key].ToArray())), EntryState.Modified);
                }
            }
            return Task.CompletedTask;
        }

        public virtual Task Handle(UserLoginEvent message)
        {
            return Task.CompletedTask;
        }

        public virtual Task Handle(UserLogoutEvent message)
        {
            return Task.CompletedTask;
        }

        public virtual Task Handle(UserPasswordChangedEvent message)
        {
            SaveOperationLog(message.UserId, SecurityAccountChangesResource.PasswordChangedMessage, EntryState.Modified);
            return Task.CompletedTask;
        }

        public virtual Task Handle(UserResetPasswordEvent message)
        {
            SaveOperationLog(message.UserId, SecurityAccountChangesResource.PasswordResetMessage, EntryState.Modified);
            return Task.CompletedTask;
        }

        protected virtual ListDictionary<string, string> DetectAccountChanges(ApplicationUserExtended newUser, ApplicationUserExtended oldUser)
        {
            //Log changes
            var result = new ListDictionary<string, string>();
            if (newUser.UserName != oldUser.UserName)
            {
                result.Add(SecurityAccountChangesResource.AccountUpdated, $"user name: {oldUser.UserName} -> {newUser.UserName}");
            }
            if (newUser.UserType != oldUser.UserType)
            {
                result.Add(SecurityAccountChangesResource.AccountUpdated, $"user type: {oldUser.UserType} -> {newUser.UserType}");
            }
            if (newUser.UserState != oldUser.UserState)
            {
                result.Add(SecurityAccountChangesResource.AccountUpdated, $"account state: {oldUser.UserState} -> {newUser.UserState}");
            }
            if (newUser.IsAdministrator != oldUser.IsAdministrator)
            {
                result.Add(SecurityAccountChangesResource.AccountUpdated, $"root: {oldUser.IsAdministrator} -> {newUser.IsAdministrator}");
            }
            if (!newUser.ApiAccounts.IsNullCollection())
            {
                var apiAccountComparer = AnonymousComparer.Create((ApiAccount x) => $"{x.ApiAccountType}-{x.SecretKey}");
                newUser.ApiAccounts.CompareTo(oldUser.ApiAccounts, apiAccountComparer, (state, sourceItem, targetItem) =>
                {
                    if (state == EntryState.Added)
                    {
                        result.Add(SecurityAccountChangesResource.ApiKeysActivated, $"{sourceItem.Name} ({sourceItem.ApiAccountType})");
                    }
                    else if (state == EntryState.Deleted)
                    {
                        result.Add(SecurityAccountChangesResource.ApiKeysDeactivated, $"{sourceItem.Name} ({sourceItem.ApiAccountType})");
                    }
                }
                );
            }
            if (!newUser.Roles.IsNullCollection())
            {
                newUser.Roles.CompareTo(oldUser.Roles, EqualityComparer<Role>.Default, (state, sourceItem, targetItem) =>
                {
                    if (state == EntryState.Added)
                    {
                        result.Add(SecurityAccountChangesResource.RolesAdded, $"{sourceItem?.Name}");
                    }
                    else if (state == EntryState.Deleted)
                    {
                        result.Add(SecurityAccountChangesResource.RolesRemoved, $"{sourceItem?.Name}");
                    }
                }
                );
            }

            return result;
        }

        protected virtual void SaveOperationLog(string objectId, string detail, EntryState entryState)
        {
            var operation = new OperationLog
            {
                ObjectId = objectId,
                ObjectType = typeof(ApplicationUserExtended).Name,
                OperationType = entryState,
                Detail = detail
            };
            _changeLogService.SaveChanges(operation);
        }
    }
}
