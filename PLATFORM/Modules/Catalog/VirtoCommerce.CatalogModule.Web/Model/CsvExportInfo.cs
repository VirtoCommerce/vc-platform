using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CatalogModule.Web.Model
{
	public class CsvExportInfo
	{
		public string CatalogId { get; set; }
		public string[] ProductIds { get; set; }
		public string[] CategoryIds { get; set; }
		public CurrencyCodes? Currency { get; set; }
	}

}