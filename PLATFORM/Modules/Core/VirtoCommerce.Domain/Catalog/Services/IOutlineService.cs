using System.Collections.Generic;
using VirtoCommerce.Domain.Catalog.Model;

namespace VirtoCommerce.Domain.Catalog.Services
{
    /// <summary>
    /// Constructed outline paths for objects
    /// </summary>
    public interface IOutlineService
    {
        /// <summary>
        /// Constructed outline paths for concrete object physical and  alternative virtual's outline paths
        /// </summary>
        /// <param name="objects">objects with will be used as outline path start point</param>
        /// <param name="catalogId">catalog which will be used as outline end point</param>
        void FillOutlinesForObjects(IEnumerable<IHasOutlines> objects, string catalogId);
    }
}
