using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.CatalogModule.Web.Model
{
    public class Catalog
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Virtual { get; set; }
        public CatalogLanguage DefaultLanguage
        {
            get
            {
				if (Languages != null)
				{
					return Languages.FirstOrDefault(x => x.IsDefault);
				}
				return null;
            }
        }
        public List<CatalogLanguage> Languages { get; set; }
		public List<Property> Properties { get; set; }
    }
}
