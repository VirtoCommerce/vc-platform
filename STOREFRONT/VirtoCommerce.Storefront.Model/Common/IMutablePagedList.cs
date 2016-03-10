using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace VirtoCommerce.Storefront.Model.Common
{
    /// <summary>
    /// PagedList which page number and page size can be changed  (on render view time for example)
    /// </summary>
    public interface IMutablePagedList : IPagedList 
    {
        void Slice(int pageNumber, int pageSize);
    }

    public interface IMutablePagedList<T> : IMutablePagedList, IPagedList<T>
    {
    }
}
