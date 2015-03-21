using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace VirtoCommerce.Foundation.AppConfig.Model
{
    /// <summary>
    /// Implements operations for the promotion entry. (Inherits <see cref="ICloneable"/>.)
    /// </summary>
    public class TargetEntry : ICloneable
    {
        private Hashtable _Storage = new Hashtable();

        /// <summary>
        /// Gets the storage.
        /// </summary>
        /// <value>The storage.</value>
        public Hashtable Storage
        {
            get { return _Storage; }
        }
		
        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> with the specified key.
        /// </summary>
        /// <value></value>
        public object this[string key]
        {
            get
            {
                object val = this.Storage[key];
                if (val != null)
                    return val;

                return String.Empty;
            }
            set
            {
                this.Storage[key] = value;
            }
        }
		
        public string CatalogName
        {
            get { return (string)Storage["CatalogName"]; }
            set { Storage["CatalogName"] = value; }
        }

        public string EntryId
        {
            get { return (string)Storage["EntryId"]; }
            set { Storage["EntryId"] = value; }
        }
		
        public string Outline
        {
            get { return (string)Storage["Outline"]; }
            set { Storage["Outline"] = value; }
        }
		
        public TargetEntry(string catalog, string entryId)
        {
            this.CatalogName = catalog;
            this.EntryId = entryId;
        }



        /// <summary>
        /// Initializes a new instance of the <see cref="TargetEntry"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public TargetEntry(Hashtable context)
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
			TargetEntry entry = new TargetEntry((Hashtable)this.Storage.Clone());
            return entry;
        }
        #endregion
    }

}
