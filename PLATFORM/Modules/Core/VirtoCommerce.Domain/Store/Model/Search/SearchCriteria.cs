using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Store.Model
{
    public class SearchCriteria
    {
        public SearchCriteria()
        {
            Take = 20;
        }
        public string[] StoreIds { get; set; }

        public string Keyword { get; set; }

        /// <summary>
        /// Sorting expression property1:asc;property2:desc
        /// </summary>
        public string Sort { get; set; }

        public SortInfo[] SortInfos
        {
            get
            {
                return SortInfo.Parse(Sort).ToArray();
            }
        }

     
        public int Skip { get; set; }

        public int Take { get; set; }

    }
}
