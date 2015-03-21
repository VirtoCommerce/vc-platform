using foundation = VirtoCommerce.Foundation.Catalogs.Model;
using module = VirtoCommerce.Domain.Catalog.Model;

namespace VirtoCommerce.CatalogModule.Data.Converters
{
    public static class PropertyValueTypeConverter
    {
        /// <summary>
        /// Converting to model type
        /// </summary>
        /// <param name="catalogBase"></param>
        /// <returns></returns>
		public static module.PropertyValueType ToModuleModel(this foundation.PropertyValueType db)
        {
			module.PropertyValueType retVal;
            switch (db)
            {
                case foundation.PropertyValueType.Decimal:
                case foundation.PropertyValueType.Integer:
					retVal = module.PropertyValueType.Number;
                    break;

                case foundation.PropertyValueType.LongString:
					retVal = module.PropertyValueType.LongText;
                    break;

                case foundation.PropertyValueType.ShortString:
					retVal = module.PropertyValueType.ShortText;
                    break;
				case foundation.PropertyValueType.DateTime:
					retVal = module.PropertyValueType.DateTime;
					break;
				case foundation.PropertyValueType.Boolean:
					retVal = module.PropertyValueType.Boolean;
					break;
                default:
					retVal = module.PropertyValueType.ShortText;
                    break;
            }

            return retVal;
        }

        /// <summary>
        /// Converting to foundation type
        /// </summary>
        /// <param></param>
        /// <returns></returns>
		public static foundation.PropertyValueType ToFoundation(this module.PropertyValueType valType)
        {
            var retVal = foundation.PropertyValueType.ShortString;
			switch (valType)
            {
                //case PropertyValueType.ShortText:
                //retVal=foundation.PropertyValueType.ShortString;
                //break;
                case module.PropertyValueType.LongText:
                    retVal = foundation.PropertyValueType.LongString;
                    break;
				case module.PropertyValueType.Number:
                    retVal = foundation.PropertyValueType.Decimal; 
                    break;
				case module.PropertyValueType.DateTime:
					retVal = foundation.PropertyValueType.DateTime; 
					break;
				case module.PropertyValueType.Boolean:
					retVal = foundation.PropertyValueType.Boolean; 
					break;
            }

            return retVal;
        }

    }
}
