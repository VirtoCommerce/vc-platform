using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Foundation.Search
{
    public interface ISearchConnection
    {
        /// <summary>
        /// Gets the Data Source.
        /// </summary>
        /// <value>
        /// The Data Source.
        /// </value>
        string DataSource { get; }
        string Scope { get; }

    }
}
