using VirtoCommerce.CatalogModule.Model;
using foundation = VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.CatalogModule.Data.Converters
{
    public static class PropertyValueTypeConverter
    {
        /// <summary>
        /// Converting to model type
        /// </summary>
        /// <param name="catalogBase"></param>
        /// <returns></returns>
        public static PropertyValueType ToModuleModel(this foundation.PropertyValueType db)
        {
            PropertyValueType retVal;
            switch (db)
            {
                case foundation.PropertyValueType.Decimal:
                case foundation.PropertyValueType.Integer:
                    retVal = PropertyValueType.Number;
                    break;

                case foundation.PropertyValueType.LongString:
                    retVal = PropertyValueType.LongText;
                    break;

                case foundation.PropertyValueType.ShortString:
                    retVal = PropertyValueType.ShortText;
                    break;

                default:
                    retVal = PropertyValueType.ShortText;
                    break;
            }

            return retVal;
        }

        /// <summary>
        /// Converting to foundation type
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public static foundation.PropertyValueType ToFoundation(this PropertyValueType module)
        {
            var retVal = foundation.PropertyValueType.ShortString;
            switch (module)
            {
                //case PropertyValueType.ShortText:
                //retVal=foundation.PropertyValueType.ShortString;
                //break;
                case PropertyValueType.LongText:
                    retVal = foundation.PropertyValueType.LongString;
                    break;
                case PropertyValueType.Number:
                    retVal = foundation.PropertyValueType.Decimal; // ??
                    break;
            }

            return retVal;
        }

    }
}
