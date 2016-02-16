using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Catalog;

namespace VirtoCommerce.Storefront.Model
{
    /// <summary>
    /// Represent menu link  associated to product 
    /// </summary>
    public class ProductMenuLink : MenuLink
    {
       public Product Product { get; set; }
    }
}
