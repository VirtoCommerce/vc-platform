using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Marketing.Model
{
    public class PromotionEntrySet
    {
        private IList<PromotionEntry> _Entries = new List<PromotionEntry>();

        /// <summary>
        /// Gets or sets the entries.
        /// </summary>
        /// <value>The entries.</value>
        public IList<PromotionEntry> Entries
        {
            get { return _Entries; }
            set { _Entries = value; }
        }

        private string _OwnerId = String.Empty;

        /// <summary>
        /// Gets or sets the owner id. This can be used to store the reference to the object that list belongs to. Like shipment.
        /// </summary>
        /// <value>The owner id.</value>
        public string OwnerId
        {
            get { return _OwnerId; }
            set { _OwnerId = value; }
        }

        /// <summary>
        /// Gets the total cost.
        /// </summary>
        /// <value>The total cost.</value>
        public decimal TotalCost
        {
            get
            {
                decimal runningTotal = 0;
                foreach (PromotionEntry entry in Entries)
                {
                    runningTotal += entry.CostPerEntry * entry.Quantity;
                }

                return runningTotal;
            }
        }

        /// <summary>
        /// Gets the total quantity.
        /// </summary>
        /// <value>The total quantity.</value>
        public decimal TotalQuantity
        {
            get
            {
                decimal runningTotal = 0;
                foreach (PromotionEntry entry in Entries)
                {
                    runningTotal += entry.Quantity;
                }

                return runningTotal;
            }
        }
    }
}
