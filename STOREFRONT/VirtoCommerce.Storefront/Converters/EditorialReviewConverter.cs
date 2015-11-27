using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Converters
{
    public static class EditorialReviewConverter
    {
        public static EditorialReview ToWebModel(this VirtoCommerceCatalogModuleWebModelEditorialReview review)
        {
            var result = new EditorialReview();
            result.Content = review.Content;
            result.Language = new Model.Language(review.LanguageCode);
            return result;
        }

     
    }
}