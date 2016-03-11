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
        public static Blog ToShopifyModel(this StorefrontModel.Blog blog)
        {
            var retVal = new Blog();

            retVal.InjectFrom<NullableAndEnumValueInjecter>(blog);

            if(blog.Articles != null)
            {
                retVal.Articles = new MutablePagedList<Article>((pageNumber, pageSize) =>
                {
                    blog.Articles.Slice(pageNumber, pageSize);
                    return new StaticPagedList<Article>(blog.Articles.Select(x => x.ToShopifyModel()), blog.Articles);
                }, blog.Articles.PageNumber, blog.Articles.PageSize);
            }
            return retVal;
        }
    }
}
