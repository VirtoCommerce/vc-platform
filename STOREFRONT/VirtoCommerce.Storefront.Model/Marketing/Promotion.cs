using System;
using System.Collections.Generic;

namespace VirtoCommerce.Storefront.Model.Marketing
{
    /// <summary>
    /// Represents promotion object
    /// </summary>
    public class Promotion
    {
        public string Catalog { get; set; }

        public ICollection<string> Coupons { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string Description { get; set; }

        public DateTime? EndDate { get; set; }

        public string Id { get; set; }

        public bool IsActive { get; set; }

        public int? MaxPersonalUsageCount { get; set; }

        public int? MaxUsageCount { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string Name { get; set; }

        public int? Priority { get; set; }

        public DateTime? StartDate { get; set; }

        public string Store { get; set; }

        public string Type { get; set; }
    }
}