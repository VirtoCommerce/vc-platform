using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Foundation.Orders.StateMachines
{
    public interface IOrderStateController
    {
        IStateMachine<string> OrderStateMachine { get; }
        IStateMachine<string> ShipmentStateMachine { get; }
        IStateMachine<string> RmaStateMachine { get; }
    }
}
