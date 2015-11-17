using System;
using System.Collections.Generic;
using dataModel = VirtoCommerce.CatalogModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common;

namespace VirtoCommerce.CatalogModule.Data.Converters
{
    public static class PropertyDictionaryValueConverter
    {
        /// <summary>
        /// Converting to model type
        /// </summary>
        /// <param name="catalogBase"></param>
        /// <returns></returns>
        public static coreModel.PropertyDictionaryValue ToCoreModel(this dataModel.PropertyDictionaryValue dbPropDictValue)
        {
 			var retVal = new coreModel.PropertyDictionaryValue();
			retVal.InjectFrom(dbPropDictValue);

			retVal.LanguageCode = dbPropDictValue.Locale;
			retVal.Value = dbPropDictValue.Value;
		          
            return retVal;
        }

        /// <summary>
        /// Converting to foundation type
        /// </summary>
        /// <param name="catalog"></param>
        /// <returns></returns>
        public static dataModel.PropertyDictionaryValue ToDataModel(this coreModel.PropertyDictionaryValue propDictValue)
        {
            var retVal = new dataModel.PropertyDictionaryValue
            {
	            Locale = propDictValue.LanguageCode,
            };
			retVal.InjectFrom(propDictValue);
            retVal.Value = propDictValue.Value;
         
            return retVal;
        }


        /// <summary>
        /// Patch CatalogLanguage type
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void Patch(this dataModel.PropertyDictionaryValue source, dataModel.PropertyDictionaryValue target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

			var newValue = source.ToString();
            if (newValue != null)
                target.Value = source.Value;
            if (source.Alias != null)
                target.Alias = source.Alias;

           
        }

		

    }

   
}
