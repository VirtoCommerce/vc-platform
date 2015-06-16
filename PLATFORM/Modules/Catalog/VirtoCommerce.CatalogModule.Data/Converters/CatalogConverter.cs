using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dataModel = VirtoCommerce.CatalogModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
using System.Collections.ObjectModel;
using VirtoCommerce.Platform.Core.Common;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
using VirtoCommerce.Domain.Commerce.Model;


namespace VirtoCommerce.CatalogModule.Data.Converters
{
    public static class CatalogConverter
    {
        /// <summary>
        /// Converting to model type
        /// </summary>
        /// <param name="catalogBase"></param>
        /// <returns></returns>
        public static coreModel.Catalog ToCoreModel(this dataModel.CatalogBase catalogBase, coreModel.Property[] properties = null, SeoUrlKeyword[] seoInfos = null)
        {
            if (catalogBase == null)
                throw new ArgumentNullException("catalogBase");

            var catalog = catalogBase as dataModel.Catalog;
            var virtualCatalog = catalogBase as dataModel.VirtualCatalog;

            var retVal = new coreModel.Catalog();
            retVal.InjectFrom(catalogBase);
            retVal.Virtual = virtualCatalog != null;
            retVal.Languages = new List<coreModel.CatalogLanguage>();

            if (catalog != null)
            {
                foreach (var catalogLanguage in catalog.CatalogLanguages.Select(x => x.ToCoreModel(retVal)))
                {
                    catalogLanguage.Catalog = retVal;
                    catalogLanguage.IsDefault = catalogBase.DefaultLanguage == catalogLanguage.LanguageCode;
                    retVal.Languages.Add(catalogLanguage);
                }

                if (properties != null)
                {
                    retVal.PropertyValues = catalog.CatalogPropertyValues.Select(x => x.ToCoreModel(properties)).ToList();
                }

            }

            if (virtualCatalog != null)
            {
                var catalogLanguage = (new dataModel.CatalogLanguage { Language = catalogBase.DefaultLanguage }).ToCoreModel(retVal);
                catalogLanguage.IsDefault = true;
                retVal.Languages.Add(catalogLanguage);
            }

			if (seoInfos != null)
			{
				retVal.SeoInfos = seoInfos.Select(x => x.ToCoreModel()).ToList();
			}

            return retVal;
        }

        /// <summary>
        /// Converting to foundation type
        /// </summary>
        /// <param name="catalog"></param>
        /// <returns></returns>
        public static dataModel.CatalogBase ToDataModel(this coreModel.Catalog catalog)
        {
            if (catalog == null)
                throw new ArgumentNullException("catalog");

            dataModel.CatalogBase retVal;
            dataModel.Catalog dbCatalog = null;
            dataModel.VirtualCatalog dbVirtCatalog = null;

            if (catalog.Virtual)
            {
                dbVirtCatalog = new dataModel.VirtualCatalog();
                retVal = dbVirtCatalog;
            }
            else
            {
                dbCatalog = new dataModel.Catalog();

                if (catalog.PropertyValues != null)
                {
                    dbCatalog.CatalogPropertyValues = new ObservableCollection<dataModel.CatalogPropertyValue>();
                    dbCatalog.CatalogPropertyValues.AddRange(catalog.PropertyValues.Select(x => x.ToDataModel<dataModel.CatalogPropertyValue>()).OfType<dataModel.CatalogPropertyValue>());
                }
                retVal = dbCatalog;
            }

            //Because EF mapping schema not automatically linked foreign keys, we should generate manually id and  set links manually
            var id = retVal.Id;
            retVal.InjectFrom(catalog);
            if (catalog.Id == null)
            {
                retVal.Id = id;
            }


            if (dbCatalog != null && catalog.Languages != null)
            {
                dbCatalog.CatalogLanguages = new ObservableCollection<dataModel.CatalogLanguage>();
                foreach (var dbCatalogLanguage in catalog.Languages.Select(x => x.ToDataModel()))
                {
                    dbCatalogLanguage.CatalogId = retVal.Id;
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
        public static void Patch(this dataModel.CatalogBase source, dataModel.CatalogBase target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            var patchInjectionPolicy = new PatchInjection<dataModel.CatalogBase>(x => x.Name, x => x.DefaultLanguage);
            target.InjectFrom(patchInjectionPolicy, source);

            //Languages patch
            var sourceCatalog = source as dataModel.Catalog;
            var targetCatalog = target as dataModel.Catalog;
            if (sourceCatalog != null && targetCatalog != null && !sourceCatalog.CatalogLanguages.IsNullCollection())
            {
                var languageComparer = AnonymousComparer.Create((dataModel.CatalogLanguage x) => x.Language);
                sourceCatalog.CatalogLanguages.Patch(targetCatalog.CatalogLanguages, languageComparer,
                                                     (sourceLang, targetlang) => sourceLang.Patch(targetlang));
            }

            //Property values
            if (!sourceCatalog.CatalogPropertyValues.IsNullCollection())
            {
                sourceCatalog.CatalogPropertyValues.Patch(targetCatalog.CatalogPropertyValues, (sourcePropValue, targetPropValue) => sourcePropValue.Patch(targetPropValue));
            }


        }

    }


}
