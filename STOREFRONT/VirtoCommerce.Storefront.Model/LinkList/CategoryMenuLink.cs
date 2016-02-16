using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Catalog;

namespace VirtoCommerce.Storefront.Model
{
    /// <summary>
    /// Represent menu link  associated to category 
    /// </summary>
    public class CategoryMenuLink : MenuLink
    {
     
        public Category Category { get; set; }
    }
}
