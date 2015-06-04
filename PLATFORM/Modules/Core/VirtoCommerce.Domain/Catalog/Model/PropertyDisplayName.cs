using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Catalog.Model
{
	public class PropertyDisplayName : Entity
	{
		public string Name { get; set; }
		public string LanguageCode { get; set; }
	}
}
