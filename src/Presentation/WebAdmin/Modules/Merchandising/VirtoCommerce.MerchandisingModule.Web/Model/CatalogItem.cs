using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class CatalogItem
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string CatalogId { get; set; }

        public string Name { get; set; }

		public string Outline { get; set; }

        public ItemImage[] Images { get; set; }

        public EditorialReview[] EditorialReviews { get; set; }

        public Association[] Associations { get; set; }

        public string[] Categories { get; set; }

        public PropertyDictionary Properties { get; set; }

        public int ReviewsTotal { get; set; }

        public double Rating { get; set; }

        public SeoKeyword[] SeoKeywords { get; set; }

        public DateTime StartDate { get; set; }

        public string MainProductId { get; set; }
    }
   
}
