﻿using System;
using System.Collections.Generic;
using PagedList;

namespace VirtoCommerce.Storefront.Model.Common
{
    /// <summary>
    /// Extend standart StaticPagedList with method allow to get url for each page index
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StorefrontPagedList<T> : StaticPagedList<T>, IStorefrontPagedList<T>
    {
        private readonly Func<int, string> _pageUrlGetter;

        public StorefrontPagedList(IEnumerable<T> subset, IPagedList metaData, Func<int, string> pageUrlGetter)
            : this(subset, metaData.PageNumber, metaData.PageSize, metaData.TotalItemCount, pageUrlGetter)
        {
        }

        public StorefrontPagedList(IEnumerable<T> subset, int pageNumber, int pageSize, int totalItemCount, Func<int, string> pageUrlGetter)
            : base(subset, pageNumber, pageSize, totalItemCount)
        {
            _pageUrlGetter = pageUrlGetter;
        }

        public string GetPageUrl(int pageIndex)
        {
            return _pageUrlGetter(pageIndex);
        }
    }
}
