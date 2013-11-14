using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Orders.Model;
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
                if (_OrderStateMachine == null)
                {
                    _OrderStateMachine = new OrderStateMachineImpl();
                }

                return _OrderStateMachine;
            }
        }

        public IStateMachine<string> ShipmentStateMachine
        {
            get
            {
                if (_ShipmentStateMachine == null)
                {
                    _ShipmentStateMachine = new ShipmentStateMachineImpl();
                }

                return _ShipmentStateMachine;
            }
        }

        public IStateMachine<string> RmaStateMachine
        {
            get
            {
                if (_RmaStateMachine == null)
                {
                    _RmaStateMachine = new RmaRequestStateMachineImpl();
                }

                return _RmaStateMachine;
            }
        }
    }
}
