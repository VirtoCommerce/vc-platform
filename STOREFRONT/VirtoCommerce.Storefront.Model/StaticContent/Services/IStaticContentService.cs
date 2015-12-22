using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.StaticContent;

namespace VirtoCommerce.Storefront.Model.Services
{
    public interface IStaticContentService
    {
        IPagedList<ContentItem> LoadContentItemsByUrl(string url, Store store, Language language, Func<string, ContentItem> contentItemFactory, int pageIndex = 1, int pageSize = 10);

    }
}
