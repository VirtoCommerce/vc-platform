using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace VirtoCommerce.Storefront.Model.Common
{
    public interface IStorefrontPagedList : IPagedList
    {
        string GetPageUrl(int pageIndex);
    }

    public interface IStorefrontPagedList<T> : IPagedList<T>, IStorefrontPagedList
    {
    }
}
