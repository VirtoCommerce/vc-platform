using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model
{
    public class ProductAssociation : ValueObject<ProductAssociation>
    {
        /// <summary>
        /// Product association name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Assosiation description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Order in which the associated product is displayed
        /// </summary>
        public int? Priority { get; set; }

        /// <summary>
        /// Id of the associated product
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Name of the associated product
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Associated product code
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// Associated product image
        /// </summary>
        public string ProductImg { get; set; }
    }
}
