using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;

namespace VirtoCommerce.Platform.Security.Handlers
{
    public class UserApiKeyActualizeEventHandler : IEventHandler<UserChangedEvent>
    {
        private readonly IUserApiKeyService _userApiKeyService;
        public UserApiKeyActualizeEventHandler(IUserApiKeyService userApiKeyService)
        {
            _userApiKeyService = userApiKeyService;
        }

        public virtual async Task Handle(UserChangedEvent message)
        {
            foreach (var changedEntry in message.ChangedEntries)
            {
                if(changedEntry.EntryState == EntryState.Deleted)
                {
                    var allUserApiKeys = await _userApiKeyService.GetAllUserApiKeysAsync(changedEntry.OldEntry.Id);
                    if(allUserApiKeys != null)
                    {
                        await _userApiKeyService.DeleteApiKeysAsync(allUserApiKeys.Select(x => x.Id).ToArray());
                    }
                }                
            }
        }

    }
}
