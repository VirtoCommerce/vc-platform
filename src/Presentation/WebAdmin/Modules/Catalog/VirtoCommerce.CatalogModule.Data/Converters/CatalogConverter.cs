using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using foundation = VirtoCommerce.Foundation.Catalogs.Model;
using module = VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Framework.Core.Utils;
using System.Collections.ObjectModel;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.CatalogModule.Data.Converters
{
	public static class CatalogConverter
	{
		/// <summary>
		/// Converting to model type
		/// </summary>
		/// <param name="catalogBase"></param>
		/// <returns></returns>
		public static module.Catalog ToModuleModel(this foundation.CatalogBase catalogBase, module.Property[] properties = null)
		{
			if (catalogBase == null)
				throw new ArgumentNullException("catalogBase");

			var catalog = catalogBase as foundation.Catalog;
			var virtualCatalog = catalogBase as foundation.VirtualCatalog;
			var retVal = new module.Catalog
			{
				Id = catalogBase.CatalogId,
				Name = catalogBase.Name,
				Virtual = virtualCatalog != null
			};

			if(catalog != null)
			{
				retVal.Languages = new List<module.CatalogLanguage>();
				foreach (var catalogLanguage in catalog.CatalogLanguages.Select(x => x.ToModuleModel(retVal)))
				{
					catalogLanguage.Catalog = retVal;
					catalogLanguage.IsDefault = catalogBase.DefaultLanguage == catalogLanguage.LanguageCode;
					retVal.Languages.Add(catalogLanguage);
				}

				if (properties != null)
				{
					retVal.PropertyValues = catalog.CatalogPropertyValues.Select(x => x.ToModuleModel(properties)).ToList();
				}

			}



			return retVal;
		}

		/// <summary>
		/// Converting to foundation type
		/// </summary>
		/// <param name="catalog"></param>
		/// <returns></returns>
		public static foundation.CatalogBase ToFoundation(this module.Catalog catalog)
		{
			if (catalog == null)
				throw new ArgumentNullException("catalog");

			foundation.CatalogBase retVal;
			foundation.Catalog dbCatalog = null;
			foundation.VirtualCatalog dbVirtCatalog = null;
	
			if (catalog.Virtual)
			{
				dbVirtCatalog = new foundation.VirtualCatalog();
				retVal = dbVirtCatalog;
			}
			else
			{
				dbCatalog = new foundation.Catalog();
				dbCatalog.CatalogLanguages = new NullCollection<foundation.CatalogLanguage>();


				dbCatalog.CatalogPropertyValues = new NullCollection<foundation.CatalogPropertyValue>();
				if (catalog.PropertyValues != null)
				{
					dbCatalog.CatalogPropertyValues = new ObservableCollection<foundation.CatalogPropertyValue>();
					dbCatalog.CatalogPropertyValues.AddRange(catalog.PropertyValues.Select(x => x.ToFoundation<foundation.CatalogPropertyValue>()).OfType<foundation.CatalogPropertyValue>());
				}
				retVal = dbCatalog;
			}
			
			retVal.CatalogId = retVal.GenerateNewKey();

			if (catalog.Id != null)
				retVal.CatalogId = catalog.Id;

			if (dbCatalog != null && catalog.Languages != null)
			{
				dbCatalog.CatalogLanguages = new ObservableCollection<foundation.CatalogLanguage>();
				foreach(var dbCatalogLanguage  in catalog.Languages.Select(x => x.ToFoundation()))
				{
					dbCatalogLanguage.CatalogId = retVal.CatalogId;
					dbCatalog.CatalogLanguages.Add(dbCatalogLanguage);
				}
				retVal.DefaultLanguage = catalog.Languages.Where(x => x.IsDefault).Select(x => x.LanguageCode).FirstOrDefault();
			}

		

			if (retVal.DefaultLanguage == null)
				retVal.DefaultLanguage = "undefined";
			
			retVal.Name = catalog.Name;
			
			return retVal;
		}


		/// <summary>
		/// Patch CatalogBase type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this foundation.CatalogBase source, foundation.CatalogBase target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

		
			//Simply properties patch
			if(source.Name != null)
				target.Name =  source.Name;
			if (source.DefaultLanguage != null)
				target.DefaultLanguage = source.DefaultLanguage;

			//Languages patch
			var sourceCatalog = source as foundation.Catalog;
			var targetCatalog = target as foundation.Catalog;
			if (sourceCatalog != null && targetCatalog != null && !sourceCatalog.CatalogLanguages.IsNullCollection())
			{
				sourceCatalog.CatalogLanguages.Patch(targetCatalog.CatalogLanguages, new CatalogLanguageComparer(),
													 (sourceLang, targetlang) => sourceLang.Patch(targetlang));
			}

			//Property values
			if (!sourceCatalog.CatalogPropertyValues.IsNullCollection())
			{
				sourceCatalog.CatalogPropertyValues.Patch(targetCatalog.CatalogPropertyValues, new PropertyValueComparer(),
					(sourcePropValue, targetPropValue) => sourcePropValue.Patch(targetPropValue));
			}


		}

	}

	
}
