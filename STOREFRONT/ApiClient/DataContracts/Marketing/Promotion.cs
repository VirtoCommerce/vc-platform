using System;

namespace VirtoCommerce.ApiClient.DataContracts.Marketing
{
    public class Promotion
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public string Store { get; set; }

        public string Catalog { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public int MaxUsageCount { get; set; }

        public int MaxPersonalUsageCount { get; set; }

        public string Coupon { get; set; }

        public int Priority { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}