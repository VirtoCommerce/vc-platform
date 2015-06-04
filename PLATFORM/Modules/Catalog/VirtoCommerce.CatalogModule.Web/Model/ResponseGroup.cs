using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.CatalogModule.Web.Model
{
	[Flags]
	public enum ResponseGroup
	{
		WithProducts = 1,
		WithCategories = 2,
		WithProperties = 4,
		WithCatalogs = 8,
		WithVariations = 16,
		Full = WithCatalogs | WithCategories | WithProperties | WithProducts | WithVariations
	}
}
