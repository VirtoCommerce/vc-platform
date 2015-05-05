using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace VirtoCommerce.Domain.Search
{
	public class CategoryFilterValue : ISearchFilterValue
    {
        public string Outline { get; set; }

        /// <summary>
        /// Gets or sets the id. Which contains category code.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
