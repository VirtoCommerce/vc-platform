using PagedList;

namespace VirtoCommerce.Storefront.Model.Common
{
    public interface IStorefrontPagedList : IPagedList
    {
        string GetPageUrl(int pageIndex, WorkContext workContext);
    }

    public interface IStorefrontPagedList<T> : IPagedList<T>, IStorefrontPagedList
    {
    }
}
