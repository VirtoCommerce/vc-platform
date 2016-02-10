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
    /// <summary>
    /// Loads all static content pages and blogs
    /// </summary>
    public interface IStaticContentService
    {
        IPagedList<ContentItem> LoadContentItems(Store store, Func<ContentItem> pageItemFactory, Func<ContentItem> blogItemFactory);
    }
}
