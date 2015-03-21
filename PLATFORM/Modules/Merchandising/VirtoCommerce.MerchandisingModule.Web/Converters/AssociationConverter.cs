using Omu.ValueInjecter;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Converters
{
    public static class AssociationConverter
    {
        #region Public Methods and Operators

        public static Association ToWebModel(this ProductAssociation source)
        {
            var retVal = new Association
                         {
                             ItemId = source.AssociatedProductId
                         };

            retVal.InjectFrom(source);

            return retVal;
        }

        #endregion
    }
}
