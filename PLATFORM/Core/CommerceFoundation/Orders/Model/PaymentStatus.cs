using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Foundation.Orders.Model
{
    public enum PaymentStatus
    {
        Pending,
        Canceled,
        Completed,
        Denied,
        Failed,
        Processing,
        OnHold
    }
}
