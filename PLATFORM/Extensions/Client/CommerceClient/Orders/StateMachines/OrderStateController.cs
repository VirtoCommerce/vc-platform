using VirtoCommerce.Foundation.Orders.StateMachines;

namespace VirtoCommerce.Client.Orders.StateMachines
{
    public class OrderStateController : IOrderStateController
    {
        IStateMachine<string> _OrderStateMachine = null;
        IStateMachine<string> _ShipmentStateMachine = null;
        IStateMachine<string> _RmaStateMachine = null;

        public IStateMachine<string> OrderStateMachine
        {
            get
            {
                return _OrderStateMachine ?? (_OrderStateMachine = new OrderStateMachineImpl());
            }
        }

        public IStateMachine<string> ShipmentStateMachine
        {
            get
            {
                return _ShipmentStateMachine ?? (_ShipmentStateMachine = new ShipmentStateMachineImpl());
            }
        }

        public IStateMachine<string> RmaStateMachine
        {
            get
            {
                return _RmaStateMachine ?? (_RmaStateMachine = new RmaRequestStateMachineImpl());
            }
        }
    }
}
