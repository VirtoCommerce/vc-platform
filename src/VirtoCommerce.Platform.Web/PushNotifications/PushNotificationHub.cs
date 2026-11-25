using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Security.Authorization;

namespace VirtoCommerce.Platform.Web.PushNotifications
{
    /// <summary>
    /// Every notification goes to its creator (the <c>Send</c> method) and is broadcast (the <c>SendSystemEvents</c>
    /// method) only to the connections allowed to see it: a connection with the permission to read all notifications joins
    /// <see cref="SystemEventsGroupName"/>, any other joins one group per creator it may see (its own user name and what
    /// the registered <see cref="IPushNotificationAudienceProvider"/>s add). A connection is never in both, so it receives
    /// a broadcast once. The groups are set when the connection starts: a changed permission applies on reconnect.
    /// </summary>
    [Authorize]
    public class PushNotificationHub : Hub
    {
        public const string SystemEventsGroupName = "SystemEvents";

        private readonly IAuthorizationService _authorizationService;
        private readonly IEnumerable<IPushNotificationAudienceProvider> _audienceProviders;

        public PushNotificationHub(IAuthorizationService authorizationService, IEnumerable<IPushNotificationAudienceProvider> audienceProviders)
        {
            _authorizationService = authorizationService;
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
            var requirement = new PermissionAuthorizationRequirement(PlatformConstants.Security.Permissions.PushNotificationsReadAll);
            var authorizationResult = await _authorizationService.AuthorizeAsync(Context.User, null, requirement);
            if (authorizationResult.Succeeded)
            {
                return [SystemEventsGroupName];
            }

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
