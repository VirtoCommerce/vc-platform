using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Domain.Catalog.Model
{
    public enum AvailabilityRule
    {
        Always = 0,
        WhenInStock,
        OnBackorder,
        OnPreorder
    }
}
