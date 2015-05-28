using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;
using System.Linq;

namespace VirtoCommerce.Domain.Catalog.Model
{
    public class Catalog : Entity
    {
        public string Name { get; set; }
        public bool Virtual { get; set; }
		public CatalogLanguage DefaultLanguage
		{
			get
			{
				CatalogLanguage retVal = null;
				if(Languages != null)
				{
					retVal = Languages.FirstOrDefault(x => x.IsDefault);
				}
				return retVal;
			}
		}
        public ICollection<CatalogLanguage> Languages { get; set; }
		public ICollection<PropertyValue> PropertyValues { get; set; }
    }
}