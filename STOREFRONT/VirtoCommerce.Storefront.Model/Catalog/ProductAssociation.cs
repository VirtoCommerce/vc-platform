using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model.Catalog
{
    public class ProductAssociation
    {
        /// <summary>
        /// Related product id
        /// </summary>
        public string ProductId { get; set; }
        /// <summary>
        /// Associated product
        /// </summary>
        public Product Product { get; set; }
        /// <summary>
        /// Related product sku
        /// </summary>
        public string ProductSku { get; set; }
        /// <summary>
        /// Related product image
        /// </summary>
        public Image ProductImage { get; set; }
        /// <summary>
        /// Related product name
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Association name/group name
        /// </summary>
        public string AssociationName { get; set; }
        /// <summary>
        /// Association description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Association priority 0 min 
        /// </summary>
        public int Priority { get; set; }
    
    }
}
