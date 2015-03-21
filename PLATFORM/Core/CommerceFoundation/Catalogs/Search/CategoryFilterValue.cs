using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Foundation.Catalogs.Search
{
    using System.Runtime.Serialization;

    using VirtoCommerce.Foundation.Search;

    [DataContract]
    public class CategoryFilterValue : ISearchFilterValue
    {
        [DataMember]
        public string Outline { get; set; }

        /// <summary>
        /// Gets or sets the id. Which contains category code.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}
