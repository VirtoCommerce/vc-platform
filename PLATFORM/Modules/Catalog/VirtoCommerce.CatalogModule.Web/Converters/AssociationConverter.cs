using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Assets.Services;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;

namespace VirtoCommerce.CatalogModule.Web.Converters
{
    public static class AssociationConverter
    {
        public static webModel.ProductAssociation ToWebModel(this moduleModel.ProductAssociation association, IAssetUrlResolver assetUrlResolver)
        {
            var retVal = new webModel.ProductAssociation();
            retVal.InjectFrom(association);
            if (association.AssociatedProduct != null)
            {
                var associatedProduct = association.AssociatedProduct.ToWebModel(assetUrlResolver);
                retVal.ProductId = associatedProduct.Id;
                retVal.ProductCode = associatedProduct.Code;
                retVal.ProductImg = associatedProduct.ImgSrc;
                retVal.ProductName = associatedProduct.Name;
            }
            return retVal;
        }

        public static moduleModel.ProductAssociation ToModuleModel(this webModel.ProductAssociation association)
        {
            var retVal = new moduleModel.ProductAssociation();
            retVal.InjectFrom(association);
            retVal.AssociatedProductId = association.ProductId;
            return retVal;
        }
    }
}
