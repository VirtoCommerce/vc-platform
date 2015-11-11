using System;

namespace VirtoCommerce.ApiClient.DataContracts.Marketing
{
    public class Promotion
    {
        public string Id { get; set; }

        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the value of promotion name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of store id
        /// </summary>
        public string Store { get; set; }

        public string Catalog { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public int MaxUsageCount { get; set; }

        public int MaxPersonalUsageCount { get; set; }

        public string[] Coupons { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}