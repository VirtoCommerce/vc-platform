using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace VirtoCommerce.Storefront.Model.Common
{
    public sealed class MutablePagedList<T> : PagedListMetaData, IMutablePagedList<T>
    {
        private readonly Func<int, int, IPagedList<T>> _getter;
        private IPagedList<T> _pagedList;
        private readonly object _lockObject = new object();

        public MutablePagedList(IEnumerable<T> superSet)
            : this((newPageNumber, newPageSize) => new PagedList<T>(superSet, newPageNumber, newPageSize), 1, Math.Max(superSet.Count(), 1))
        {
            TotalItemCount = superSet.Count();
            PageCount = 1;
        }

        public MutablePagedList(Func<int, int, IPagedList<T>> getter, int pageNumber = 1, int pageSize = 20)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            _getter = getter;
        }

        #region ILazyPagedList Members
        /// <summary>
        /// Resize current paged data list by new PageNumber and PageSize values (it may cause reloading data from source)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        public void Slice(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException("pageNumber", pageNumber, "PageNumber cannot be below 1.");
            if (pageSize < 1)
                throw new ArgumentOutOfRangeException("pageSize", pageSize, "PageSize cannot be less than 1.");

            if (pageNumber != PageNumber)
            {
                PageNumber = pageNumber;
                _pagedList = null;
            }
            if (pageSize != PageSize)
            {
                PageSize = pageSize;
                _pagedList = null;
            }
          
            TryReloadPagedData();
        }
        #endregion
        #region IPagedList<T> Members

        /// <summary>
        /// 	Returns an enumerator that iterates through the BasePagedList&lt;T&gt;.
        /// </summary>
        /// <returns>A BasePagedList&lt;T&gt;.Enumerator for the BasePagedList&lt;T&gt;.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            TryReloadPagedData();
            return _pagedList.GetEnumerator();
        }

        /// <summary>
        /// 	Returns an enumerator that iterates through the BasePagedList&lt;T&gt;.
        /// </summary>
        /// <returns>A BasePagedList&lt;T&gt;.Enumerator for the BasePagedList&lt;T&gt;.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        ///<summary>
        ///	Gets the element at the specified index.
        ///</summary>
        ///<param name = "index">The zero-based index of the element to get.</param>
        public T this[int index]
        {
            get
            {
                TryReloadPagedData();
                return _pagedList[index];
            }
        }

        /// <summary>
        /// 	Gets the number of elements contained on this page.
        /// </summary>
        public int Count
        {
            get
            {

                TryReloadPagedData();
                return _pagedList.Count;
            }
        }

        ///<summary>
        /// Gets a non-enumerable copy of this paged list.
        ///</summary>
        ///<returns>A non-enumerable copy of this paged list.</returns>
        public IPagedList GetMetaData()
        {
            TryReloadPagedData();
            return new PagedListMetaData(this);
        }

        #endregion

        private void TryReloadPagedData()
        {
            if (_pagedList == null)
            {
                lock (_lockObject)
                {
                    if (_pagedList == null)
                    {
                        _pagedList = _getter(PageNumber, PageSize);
                    }
                }
                // set source to blank list if superset is null to prevent exceptions
                TotalItemCount = _pagedList.TotalItemCount;
                PageCount = TotalItemCount > 0
                                ? (int)Math.Ceiling(TotalItemCount / (double)PageSize)
                                : 0;
                HasPreviousPage = PageNumber > 1;
                HasNextPage = PageNumber < PageCount;
                IsFirstPage = PageNumber == 1;
                IsLastPage = PageNumber >= PageCount;
                FirstItemOnPage = (PageNumber - 1) * PageSize + 1;
                var numberOfLastItemOnPage = FirstItemOnPage + PageSize - 1;
                LastItemOnPage = numberOfLastItemOnPage > TotalItemCount
                                     ? TotalItemCount
                                     : numberOfLastItemOnPage;
            }
        }



    }
}
