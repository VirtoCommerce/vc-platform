using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model
{
    public class CategoryLink : ValueObject<CategoryLink>
    {

        /// <summary>
        /// Source item identifier
        /// </summary>
        public string SourceItemId { get; set; }


        /// <summary>
        /// Source category identifier
        /// </summary>
        public string SourceCategoryId { get; set; }


        /// <summary>
        /// Target catalog identifier
        /// </summary>
        public string CatalogId { get; set; }


        /// <summary>
        /// Target category identifier
        /// </summary>
        public string CategoryId { get; set; }
    }
}
