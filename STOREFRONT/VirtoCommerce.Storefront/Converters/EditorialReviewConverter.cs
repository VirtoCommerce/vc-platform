using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Client.Model;
using Omu.ValueInjecter;

namespace VirtoCommerce.Storefront.Converters
{
    public static class EditorialReviewConverter
    {
        public static EditorialReview ToWebModel(this VirtoCommerceCatalogModuleWebModelEditorialReview review)
        {
            var retVal = new EditorialReview();
            retVal.InjectFrom(review);
            return retVal;
        }
    }
}