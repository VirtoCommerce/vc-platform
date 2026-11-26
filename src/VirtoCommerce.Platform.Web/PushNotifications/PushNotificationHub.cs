using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.Platform.Web.PushNotifications
{
    /// <summary>
    /// Every notification goes to its creator: the <c>Send</c> method to the creator's user, and the <c>SendSystemEvents</c>
    /// broadcast to the creator's group. A connection joins one group per creator whose notifications it reads: its own user name
    /// and what the registered <see cref="IPushNotificationAudienceProvider"/>s add. No connection receives other users' notifications,
    /// administrators included. The groups are set when the connection starts: a changed audience applies on reconnect.
    /// </summary>
    [Authorize]
    public class PushNotificationHub : Hub
    {
        private readonly IEnumerable<IPushNotificationAudienceProvider> _audienceProviders;

        public PushNotificationHub(IEnumerable<IPushNotificationAudienceProvider> audienceProviders)
        {
            _audienceProviders = audienceProviders;
        }

        public static string GetCreatorGroupName(string creator)
        {
            // User names are unique by their normalized (upper case) form, and SignalR group names are case-sensitive
            return $"Creator:{creator.ToUpperInvariant()}";
        }

        public override async Task OnConnectedAsync()
        {
            foreach (var groupName in await GetGroupNamesAsync())
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            }

            await base.OnConnectedAsync();
        }

        protected virtual async Task<IList<string>> GetGroupNamesAsync()
        {
            var creators = new List<string> { Context.UserIdentifier };

            foreach (var audienceProvider in _audienceProviders)
            {
                creators.AddRange(await audienceProvider.GetCreatorsAsync(Context.User));
            }

            return creators
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(GetCreatorGroupName)
                .Distinct(StringComparer.Ordinal)
                .ToList();
        }
    }
}
