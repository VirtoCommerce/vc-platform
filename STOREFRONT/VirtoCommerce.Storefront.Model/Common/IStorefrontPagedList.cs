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
