namespace VirtoCommerce.Foundation.Orders.StateMachines
{
    public interface IOrderStateController
    {
        IStateMachine<string> OrderStateMachine { get; }
        IStateMachine<string> ShipmentStateMachine { get; }
        IStateMachine<string> RmaStateMachine { get; }
    }
}
