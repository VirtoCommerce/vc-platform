using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using VirtoCommerce.LiquidThemeEngine.Objects;
using VirtoCommerce.Storefront.Model.Common;
using StorefrontModel = VirtoCommerce.Storefront.Model.StaticContent;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class ArticleCOnverter
    {
        public static Article ToShopifyModel(this StorefrontModel.BlogArticle article)
        {
            var retVal = new Article();

            retVal.InjectFrom<NullableAndEnumValueInjecter>(article);
            retVal.CreatedAt = article.CreatedDate;
            retVal.PublishedAt = article.PublishedDate ?? article.CreatedDate;

            return retVal;
        }
    }
}
