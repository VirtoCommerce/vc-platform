using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.CatalogModule.Web.Model
{
	public class CsvImportConfiguration
	{
		public string FileUrl { get; set; }
		public string CatalogId { get; set; }
		public string Delimiter { get; set; }
		public string[] CsvColumns { get; set; }
		public CsvImportMappingItem[] MappingItems { get; set; }
		public string[] PropertyCsvColumns { get; set; }
	}
}