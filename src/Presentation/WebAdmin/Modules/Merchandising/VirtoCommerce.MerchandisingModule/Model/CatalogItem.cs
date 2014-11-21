using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.MerchandisingModule.Model
{
    public class CatalogItem : Resource
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Catalog { get; set; }

        public string Name { get; set; }

        public ItemImage[] Images { get; set; }

        public PropertyDictionary Properties { get; set; }
    }
}
