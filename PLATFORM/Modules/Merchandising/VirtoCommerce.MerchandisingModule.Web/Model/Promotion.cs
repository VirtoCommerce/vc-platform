using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class Promotion : AuditableEntity
    {
        /// <summary>
        /// Gets or sets the value of promotion type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the value of promotion name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of store id
        /// </summary>
        public string Store { get; set; }

        /// <summary>
        /// Gets or sets the value of catalog id
        /// </summary>
        public string Catalog { get; set; }

        /// <summary>
        /// Gets or sets the value of promotion description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the activity flag for promotion
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the value of promotion maximum common usage limit
        /// </summary>
        public int MaxUsageCount { get; set; }

        /// <summary>
        /// Gets or sets the value of promotion maximum personal usage limit
        /// </summary>
        public int MaxPersonalUsageCount { get; set; }

        /// <summary>
        /// Gets or sets the collection of coupon codes
        /// </summary>
        /// <value>
        /// Array of coupon codes
        /// </value>
        public string[] Coupons { get; set; }

        /// <summary>
        /// Gets or sets the value of promotion start date/time
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the value of promotion end date/time
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}