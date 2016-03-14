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
    /// Represent a search and rendering static content pages (pages and blogs etc)
    /// </summary>
    public interface IStaticContentService
    {
        IEnumerable<ContentItem> LoadStoreStaticContent(Store store);

    }
}
