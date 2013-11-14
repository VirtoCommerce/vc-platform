using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Customers.Model
{
    public enum ContractState
    {
        Draft,
        PendingApproval,
        Active,
        Rejected,
        Closed
    }
}
