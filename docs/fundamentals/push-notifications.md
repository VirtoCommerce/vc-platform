# Push notifications

Modules report progress and results to back-office users with push notifications: a `PushNotification` subclass sent through `IPushNotificationManager` (`VirtoCommerce.Platform.Core.PushNotifications`).

```csharp
// MyExportPushNotification : PushNotification, a class of your module
var notification = new MyExportPushNotification(_userNameResolver.GetCurrentUserName()) { Title = "Export" };
await _pushNotificationManager.SendAsync(notification);
```

## Who receives a notification

A notification belongs to its **creator**: the user name passed to the constructor. Sending it:

- saves it to the notification history of the creator (`POST api/platform/pushnotifications` returns only the signed-in user's own notifications);
- sends it live on the `/pushNotificationHub` hub, with two methods:
  - `Send`, to the creator's connections;
  - `SendSystemEvents`, to the creator's group: the creator, plus the users an `IPushNotificationAudienceProvider` admits (below).

No user receives another user's notifications, administrators included. A client that listens to `SendSystemEvents` needs no filter of its own.

## Notifications for a group of users: `IPushNotificationAudienceProvider`

Some notifications belong to a group of users, not to one user name. A module that creates them with a group id as the creator registers an `IPushNotificationAudienceProvider`. When a user connects to the hub, the platform asks every provider which creators' broadcasts this user receives, and adds the connection to their groups.

```csharp
public interface IPushNotificationAudienceProvider
{
    Task<IEnumerable<string>> GetCreatorsAsync(ClaimsPrincipal user);
}
```

- Return the group ids the user belongs to; the user's own name is always included by the platform.
- The groups are set when the connection starts: a user added to or removed from a group gets the change after the client reconnects.
- The provider decides who sees what, so return only groups the user may see.
- Only `SendSystemEvents` reaches the group. `Send` and the notification history (`api/platform/pushnotifications`) stay keyed by the creator.

### Example: marketplace vendor portal

The vendor portal shows a seller's notifications (new orders, offers, imports) to every user of that seller. These notifications are created with the seller id as the creator:

```csharp
// SellerOrderPushNotification : PushNotification, a class of the marketplace module
var notification = new SellerOrderPushNotification(sellerId) { Title = "New order" };
await _pushNotificationManager.SendAsync(notification);
```

The seller id is not a user name, so without a provider nobody would receive it on `SendSystemEvents`. The marketplace module registers a provider that returns the sellers the signed-in user works for:

```csharp
public class SellerPushNotificationAudienceProvider(ISellerMembershipService sellerMembership) : IPushNotificationAudienceProvider
{
    public async Task<IEnumerable<string>> GetCreatorsAsync(ClaimsPrincipal user)
    {
        // The sellers whose operators include this user. ISellerMembershipService stands for the module's own lookup.
        var sellerIds = await sellerMembership.GetSellerIdsAsync(user.GetUserId());

        return sellerIds;
    }
}
```

```csharp
// Module.Initialize
serviceCollection.AddSingleton<IPushNotificationAudienceProvider, SellerPushNotificationAudienceProvider>();
```

Every operator of the seller now receives the seller's notifications live, and users of other sellers receive none. The vendor portal client keeps listening to `SendSystemEvents` and no longer needs its own broadcast filter.

## Notification history

- With `ConnectionStrings:RedisConnectionString`, the history is kept in Redis and shared by all platform instances; without Redis, each instance keeps its own in memory.
- A user keeps at most `PushNotifications:MaxNotificationsPerUser` notifications (20 by default) for `PushNotifications:RetentionPeriod` (24 hours by default).
- A notification with a known id replaces the stored one, so a job that reports progress updates one notification.
- `POST api/platform/pushnotifications/markAllAsRead` marks the user's new notifications as read in the history without sending them again.

See [Scaling push notifications](../techniques/how-scale-out-platform-on-azure.md) for the scale-out configuration.
