using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Catalog.Model
{
	public class PropertyDictionaryValue : Entity
	{
		public string PropertyId { get; set; }
		public Property Property { get; set; }
		public string LanguageCode { get; set; }
		public string Value { get; set; }

	}
}
