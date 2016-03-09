using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using VirtoCommerce.Storefront.Model.Common;
using Xunit;

namespace VirtoCommerce.Storefront.Test
{
    public class MutablePagedListTests
    {
        [Fact]
        public void PagedAccessToCollection_CollectionChunkedToPages()
        {
            int totalCount;
            var superset = GetTestData(1, int.MaxValue, out totalCount);
            var mutablePagedList = new MutablePagedList<int>(superset, 1, 5);

            Assert.True(mutablePagedList.TotalItemCount == 0);
            Assert.True(mutablePagedList.PageCount == 0);

            //force loading 1 page
            mutablePagedList.ToArray();
            Assert.True(mutablePagedList.TotalItemCount == 10);
            Assert.True(mutablePagedList.PageNumber == 1);
            Assert.True(mutablePagedList.PageSize == 5);
            Assert.True(mutablePagedList.PageCount == 2);

            Assert.True(mutablePagedList.First() == 1);
            Assert.True(mutablePagedList.Last() == 5);

            //Change mutable list pageNumber to 2 
            mutablePagedList.Resize(2, 5);

            Assert.True(mutablePagedList.TotalItemCount == 10);
            Assert.True(mutablePagedList.PageNumber == 2);
            Assert.True(mutablePagedList.PageSize == 5);
            Assert.True(mutablePagedList.PageCount == 2);

            Assert.True(mutablePagedList.First() == 6);
            Assert.True(mutablePagedList.Last() == 10);
        }

        [Fact]
        public void DelayLoadingPagedData_DataLoadedByMultiplePagedRequest()
        {
            var requestCount = 0;
            var mutablePagedList = new MutablePagedList<int>((pageNumber, pageSize) =>
            {
                int totalCount;
                var data = GetTestData((pageNumber - 1) * pageSize, pageSize, out totalCount);
                requestCount++;
                return new StaticPagedList<int>(data, pageNumber, pageSize, totalCount);
            }, 1, 5);

            //force loading 1 page
            mutablePagedList.ToArray();
            Assert.True(requestCount == 1);
            Assert.True(mutablePagedList.TotalItemCount == 10);
            Assert.True(mutablePagedList.PageNumber == 1);
            Assert.True(mutablePagedList.PageSize == 5);
            Assert.True(mutablePagedList.PageCount == 2);

            Assert.True(mutablePagedList.First() == 1);
            Assert.True(mutablePagedList.Last() == 5);

            //Change mutable list pageNumber to 2 (should load new data set)
            mutablePagedList.Resize(2, 5);

            Assert.True(requestCount == 2);
            Assert.True(mutablePagedList.TotalItemCount == 10);
            Assert.True(mutablePagedList.PageNumber == 2);
            Assert.True(mutablePagedList.PageSize == 5);
            Assert.True(mutablePagedList.PageCount == 2);

            Assert.True(mutablePagedList.First() == 6);
            Assert.True(mutablePagedList.Last() == 10);
        }

        private int[] GetTestData(int skip, int take, out int totalCount)
        {
            totalCount = 10;
            var retVal = Enumerable.Range(1, totalCount).Skip(skip).Take(take);
            return retVal.ToArray();
        }
    }
}
