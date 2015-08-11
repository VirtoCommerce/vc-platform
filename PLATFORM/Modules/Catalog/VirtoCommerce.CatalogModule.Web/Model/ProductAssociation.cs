using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.CatalogModule.Web.Model
{
    /// <summary>
    /// Class containing associated product information like 'Accessory', 'Related Item', etc.
    /// </summary>
	public class ProductAssociation 
	{
        /// <summary>
        /// Gets or sets the ProductAssociation name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
		public string Name { get; set; }
		public string Description { get; set; }
        /// <summary>
        /// Gets or sets the order in which the associated product is displayed.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
		public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated product.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
		public string ProductId { get; set; }
        /// <summary>
        /// Gets or sets the name of the associated product.
        /// </summary>
        /// <value>
        /// The name of the product.
        /// </value>
		public string ProductName { get; set; }
        /// <summary>
        /// Gets or sets the associated product code.
        /// </summary>
        /// <value>
        /// The product code.
        /// </value>
		public string ProductCode { get; set; }
        /// <summary>
        /// Gets or sets the associated product image.
        /// </summary>
        /// <value>
        /// The product img.
        /// </value>
		public string ProductImg { get; set; }
	}
}
