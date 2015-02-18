using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.CatalogModule.Web.Model
{
    public class Catalog
    {
        public Catalog()
        {
            Languages = new List<CatalogLanguage>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public bool Virtual { get; set; }
        public CatalogLanguage DefaultLanguage
        {
            get
            {
                return Languages.FirstOrDefault(x => x.IsDefault);
            }
        }
        public List<CatalogLanguage> Languages { get; set; }
		public List<Property> Properties { get; set; }
    }
}
