using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VirtoCommerce.Foundation.Search.Facets
{
    [DataContract/*Serializable*/]
    public class Facet
    {
        /// <summary>
        /// Gets the group.
        /// </summary>
        /// <value>The group.</value>
        public FacetGroup Group
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        [DataMember]
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        [DataMember]
        public int Count
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the Key.
        /// </summary>
        /// <value>The URL.</value>
        [DataMember]
        public string Key
        {
            get;
            private set;
        }

        public Facet()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Facet"/> class.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <param name="key">The key.</param>
        /// <param name="name">The name.</param>
        /// <param name="count">The count.</param>
        public Facet(FacetGroup group, string key, string name, int count)
        {
            this.Group = group;
            this.Key = key;
            this.Name = name;
            this.Count = count;
        }

    }
}
