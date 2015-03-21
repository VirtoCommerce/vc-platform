
namespace VirtoCommerce.Foundation.Orders.Model
{
    /// <summary>
    /// predefined States (Statuses) that order can have.
    /// </summary>
    public enum OrderStatus
    {
        Pending,
        OnHold,
        PartiallyShipped,
        InProgress,
        Completed,
        Cancelled,
        AwaitingExchange
    }
}
