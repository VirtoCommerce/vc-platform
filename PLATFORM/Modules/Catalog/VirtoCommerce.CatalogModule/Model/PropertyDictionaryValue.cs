using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.CatalogModule.Model
{
	public class PropertyDictionaryValue
	{
		public string Id { get; set; }
		public string PropertyId { get; set; }
		public Property Property { get; set; }
		public string LanguageCode { get; set; }
		public string Value { get; set; }

	}
}
