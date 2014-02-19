using System;
using System.Collections;

namespace VirtoCommerce.Foundation.Marketing.Model
{
    /// <summary>
    /// Implements operations for the promotion entry. (Inherits <see cref="ICloneable"/>.)
    /// </summary>
    public class PromotionEntry : ICloneable
    {
        private readonly Hashtable _Storage = new Hashtable();

        /// <summary>
        /// Gets the storage.
        /// </summary>
        /// <value>The storage.</value>
        public Hashtable Storage
        {
            get { return _Storage; }
        }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>The quantity.</value>
        public decimal Quantity
        {
            get { return (decimal)Storage["Quantity"]; }
            set { Storage["Quantity"] = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> with the specified key.
        /// </summary>
        /// <value></value>
        public object this[string key]
        {
            get
            {
                var val = Storage[key];
                return val ?? string.Empty;
            }
            set
            {
                Storage[key] = value;
            }
        }

        public object Owner
        {
            get { return Storage["PromotionEntryOwner"]; }
            set { Storage["PromotionEntryOwner"] = value; }
        }

        public decimal CostPerEntry
        {
            get { return (decimal)Storage["CostPerEntry"]; }
            set { Storage["CostPerEntry"] = value; }
        }

		public string CatalogId
        {
			get { return (string)Storage["CatalogId"]; }
			set { Storage["CatalogId"] = value; }
        }

        public string EntryId
        {
            get { return (string)Storage["EntryId"]; }
            set { Storage["EntryId"] = value; }
        }

		public string EntryCode
		{
			get { return (string)Storage["EntryCode"]; }
			set { Storage["EntryCode"] = value; }
		}

        public string ParentEntryId
        {
            get { return (string)Storage["ParentEntryId"]; }
            set { Storage["ParentEntryId"] = value; }
        }

        public string Outline
        {
            get { return (string)Storage["Outline"]; }
            set { Storage["Outline"] = value; }
        }

        public PromotionEntry(string catalog, string entryId, decimal costPerEntry)
        {
            CatalogId = catalog;
            EntryId = entryId;
            CostPerEntry = costPerEntry;
            Quantity = 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PromotionEntry"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public PromotionEntry(Hashtable context)
        {
            _Storage = context;
        }

        #region ICloneable Members
        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public object Clone()
        {
            var entry = new PromotionEntry((Hashtable)Storage.Clone());
            return entry;
        }
        #endregion
    }

}
