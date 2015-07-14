using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.CatalogModule.Web.ExportImport
{
	[Flags]
	public enum CsvExportImportMode
	{
		Catalog = 1,
		Price = 2,
		Inventory = 4,
		All = Catalog | Price | Inventory
	}
}