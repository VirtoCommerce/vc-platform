using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Catalog.Model
{
	public class PropertyDisplayName : ILanguageSupport
	{
		public string Name { get; set; }
		public string LanguageCode { get; set; }

	}
}
