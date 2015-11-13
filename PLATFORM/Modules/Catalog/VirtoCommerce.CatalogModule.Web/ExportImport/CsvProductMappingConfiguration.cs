using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.CatalogModule.Web.ExportImport;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CatalogModule.Web.ExportImport
{
	public class CsvProductMappingConfiguration
	{
		public CsvProductMappingConfiguration()
		{
			PropertyMaps = new List<CsvProductPropertyMap>();
		}

		public string ETag { get; set; }
		public string Delimiter { get; set; }
		public string[] CsvColumns { get; set; }
		public ICollection<CsvProductPropertyMap> PropertyMaps { get; set; }
		public string[] PropertyCsvColumns { get; set; }

		public static CsvProductMappingConfiguration GetDefaultConfiguration()
		{
			var retVal = new CsvProductMappingConfiguration();
			retVal.Delimiter = ";";

			var requiredFields = ReflectionUtility.GetPropertyNames<CsvProduct>(x => x.Name);
			var optionalFields = ReflectionUtility.GetPropertyNames<CsvProduct>(x => x.Id, x => x.Sku, x => x.CategoryPath, x => x.CategoryId, x => x.MainProductId, x=>x.PrimaryImage, x=>x.AltImage, x => x.SeoUrl, x => x.SeoTitle,
																				x => x.SeoDescription, x => x.Review, x => x.IsActive, x => x.IsBuyable, x => x.TrackInventory, 
																				x => x.PriceId, x => x.SalePrice, x => x.ListPrice, x => x.Currency, x=> x.Quantity,
																				x => x.ManufacturerPartNumber, x => x.Gtin, x => x.MeasureUnit, x => x.WeightUnit, x => x.Weight,
																				x => x.Height, x => x.Length, x => x.Width, x => x.TaxType, x => x.ProductType, x => x.ShippingType,
																				x => x.Vendor, x => x.DownloadType, x => x.DownloadExpiration, x => x.HasUserAgreement);

			retVal.PropertyMaps = requiredFields.Select(x => new CsvProductPropertyMap { EntityColumnName = x, CsvColumnName = x, IsRequired = true }).ToList();
			retVal.PropertyMaps.AddRange(optionalFields.Select(x => new CsvProductPropertyMap { EntityColumnName = x, CsvColumnName = x, IsRequired = false }));
			return retVal;
		}
		
		public void AutoMap(IEnumerable<string> csvColumns)
		{
			this.CsvColumns = csvColumns.ToArray();

			foreach (var propertyMap in this.PropertyMaps)
			{
				var entityColumnName = propertyMap.EntityColumnName;
				var betterMatchCsvColumn = csvColumns.Select(x => new { csvColumn = x, distance = x.ComputeLevenshteinDistance(entityColumnName) })
													 .Where(x => x.distance < 2)
													 .OrderBy(x => x.distance)
													 .Select(x => x.csvColumn)
													 .FirstOrDefault();
				if (betterMatchCsvColumn != null)
				{
					propertyMap.CsvColumnName = betterMatchCsvColumn;
					propertyMap.CustomValue = null;
				}
				else
				{
					propertyMap.CsvColumnName = null;
				}

			}
			//All not mapped properties may be a product property
			this.PropertyCsvColumns = csvColumns.Except(this.PropertyMaps.Where(x => x.CsvColumnName != null).Select(x => x.CsvColumnName)).ToArray();
			//Generate ETag for identifying csv format
			this.ETag = string.Join(";", this.CsvColumns).GetMD5Hash();
		}
	
	}
}