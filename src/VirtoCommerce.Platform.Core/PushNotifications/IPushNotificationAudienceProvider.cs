using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.PushNotifications
{
    /// <summary>
    /// Names the creators whose broadcast push notifications (the <c>SendSystemEvents</c> hub method) a signed-in user
    /// receives besides their own. A module registers one when it creates notifications for a group of users, not for one
    /// user name: for example a seller's notifications, created with the seller id, reach every user of that seller.
    /// </summary>
    public interface IPushNotificationAudienceProvider
    {
        Task<IEnumerable<string>> GetCreatorsAsync(ClaimsPrincipal user);
    }
}
