using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StorefrontModel = VirtoCommerce.Storefront.Model.StaticContent;
using VirtoCommerce.LiquidThemeEngine.Objects;
using Omu.ValueInjecter;
using VirtoCommerce.Storefront.Model.Common;
using PagedList;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class BlogConverter
    {
        public static Blog ToShopifyModel(this StorefrontModel.Blog blog, Storefront.Model.Language language)
        {
            var retVal = new Blog();

            retVal.InjectFrom<NullableAndEnumValueInjecter>(blog);

            if (blog.Articles != null)
            {
                retVal.Articles = new MutablePagedList<Article>((pageNumber, pageSize) =>
                {
                    var articlesForLanguage = blog.Articles.Where(x => x.Language == language || x.Language.IsInvariant).GroupBy(x => x.Name).Select(x => x.OrderByDescending(y => y.Language).FirstOrDefault());
                    return new PagedList<Article>(articlesForLanguage.Select(x => x.ToShopifyModel()), pageNumber, pageSize);
                }, blog.Articles.PageNumber, blog.Articles.PageSize);
            }
            return retVal;
        }
    }
}
