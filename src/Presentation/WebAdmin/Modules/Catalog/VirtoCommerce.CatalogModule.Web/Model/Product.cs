using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.CatalogModule.Web.Model
{
    public class Product
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

		public string CatalogId { get; set; }
		public Catalog Catalog { get; set; }

        public string CategoryId { get; set; }
        public Category Category { get; set; }
		public string Outline { get; set; }

        public string TitularItemId { get; set; }

		private string _imgSrc;
        public string ImgSrc 
		{ 
			get
			{
				if (_imgSrc == null)
				{
					if (Assets != null)
					{
						var img = Images.FirstOrDefault(x => x.Group == "primaryimage");
						if (img != null)
						{
							_imgSrc = img.Url;
						}
					}
				}
				return _imgSrc;
			}
		}

        public List<Property> Properties { get; set; }
        public List<ProductImage> Images { get; set; }
		public List<ProductAsset> Assets { get; set; }
        public List<Product> Variations { get; set; }
        public List<CategoryLink> Links { get; set; }
		public List<SeoInfo> SeoInfos { get; set; }
		public List<EditorialReview> Reviews { get; set; }
		public List<ProductAssociation> Associations { get; set; }
    }
}