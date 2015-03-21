using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using VirtoCommerce.CatalogModule.Web.Model;
using foundation = VirtoCommerce.Foundation.Importing.Model;

namespace VirtoCommerce.CatalogModule.Web.Converters
{
    public static class MappingItemConverter
    {
		public static MappingItem ToWebModel(this foundation.ImportProperty importProperty)
		{
			var retVal = new MappingItem();
			retVal.EntityColumnName = importProperty.Name;
			retVal.IsSystemProperty = true;
			retVal.IsRequired = importProperty.IsRequiredProperty;
			retVal.DisplayName = importProperty.IsRequiredProperty ? string.Format("* {0}", importProperty.DisplayName) : importProperty.DisplayName;
			retVal.CustomValue = !string.IsNullOrEmpty(importProperty.DefaultValue) ? importProperty.DefaultValue : null;
			return retVal;
		}

        public static MappingItem ToWebModel(this foundation.MappingItem core)
        {
            var retVal = new MappingItem();
            retVal.InjectFrom(core);
            retVal.Id = core.MappingItemId;
            return retVal;
        }

        public static foundation.MappingItem ToFoundation(this MappingItem webEntity)
        {
            var retVal = new foundation.MappingItem();
            retVal.InjectFrom(webEntity);
            retVal.MappingItemId = webEntity.Id;
            return retVal;
        }

        public static void Patch(this foundation.MappingItem source, foundation.MappingItem target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            //Simple properties patch

            target.CustomValue = source.CustomValue;
            target.CsvColumnName = source.CsvColumnName;
            if (source.DisplayName != null)
                target.DisplayName = source.DisplayName;
            target.EntityColumnName = source.EntityColumnName;
            target.IsRequired = source.IsRequired;
            target.IsSystemProperty = source.IsSystemProperty;
            target.Locale = source.Locale;
            target.StringFormat = source.StringFormat;
        }
    }

    public class MappingItemComparer : IEqualityComparer<foundation.MappingItem>
    {
        #region IEqualityComparer<CatalogLanguage> Members

        public bool Equals(foundation.MappingItem x, foundation.MappingItem y)
        {
            return x.MappingItemId == y.MappingItemId;
        }

        public int GetHashCode(foundation.MappingItem obj)
        {
            return obj.MappingItemId.GetHashCode();
        }

        #endregion
    }
}
