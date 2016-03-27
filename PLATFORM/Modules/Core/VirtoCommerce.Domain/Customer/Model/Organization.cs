using System.Collections.Generic;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.Domain.Customer.Model
{
    public class Organization : Member
    {
        public Organization()
            :base("Organization")
        {
        }
        public string Description { get; set; }
        public string BusinessCategory { get; set; }
        public string OwnerId { get; set; }
        public string ParentId { get; set; }

    }
}
