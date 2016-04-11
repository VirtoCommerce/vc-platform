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
    [Trait("Category", "CI")]
    public class MutablePagedListTests
    {
        [Fact]
        public void PagedAccessToCollection_CollectionChunkedToPages()
        {
            int totalCount;
            var superset = GetTestData(0, int.MaxValue, out totalCount);
            var mutablePagedList = new MutablePagedList<int>(superset);

            Assert.True(mutablePagedList.TotalItemCount == totalCount);
            Assert.True(mutablePagedList.PageCount == 1);

            //force loading 1 page
            mutablePagedList.Slice(1, 5);
            Assert.True(mutablePagedList.TotalItemCount == 10);
            Assert.True(mutablePagedList.PageNumber == 1);
            Assert.True(mutablePagedList.PageSize == 5);
            Assert.True(mutablePagedList.PageCount == 2);

            Assert.True(mutablePagedList.First() == 0);
            Assert.True(mutablePagedList.Last() == 4);

            //Change mutable list pageNumber to 2 
            mutablePagedList.Slice(2, 5);

            Assert.True(mutablePagedList.TotalItemCount == 10);
            Assert.True(mutablePagedList.PageNumber == 2);
            Assert.True(mutablePagedList.PageSize == 5);
            Assert.True(mutablePagedList.PageCount == 2);

            Assert.True(mutablePagedList.First() == 5);
            Assert.True(mutablePagedList.Last() == 9);
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

            Assert.True(mutablePagedList.First() == 0);
            Assert.True(mutablePagedList.Last() == 4);

            //Change mutable list pageNumber to 2 (should load new data set)
            mutablePagedList.Slice(2, 5);
            //Load data by 2 times request
            Assert.True(requestCount == 2);
            Assert.True(mutablePagedList.TotalItemCount == 10);
            Assert.True(mutablePagedList.PageNumber == 2);
            Assert.True(mutablePagedList.PageSize == 5);
            Assert.True(mutablePagedList.PageCount == 2);

            Assert.True(mutablePagedList.First() == 5);
            Assert.True(mutablePagedList.Last() == 9);
        }

        private int[] GetTestData(int skip, int take, out int totalCount)
        {
            totalCount = 10;
            var retVal = Enumerable.Range(0, totalCount).Skip(skip).Take(take);
            return retVal.ToArray();
        }
    }
}
