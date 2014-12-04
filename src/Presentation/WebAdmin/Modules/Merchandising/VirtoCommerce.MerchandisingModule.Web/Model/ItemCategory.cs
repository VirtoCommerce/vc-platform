using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class ItemCategory
    {
        public string CatalogId { get; set; }
        public string CategoryId { get; set; }

        public ItemCategory VirtualCategories { get; set; }
    }
}
