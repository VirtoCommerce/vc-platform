using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.EventAggregation
{
    public class GoToOrderEvent
    {
        public string OrderId { get; set; }
    }
}
