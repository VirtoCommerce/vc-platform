using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Common;
using Xunit;

namespace VirtoCommerce.Storefront.Test
{
    public class ConcurrentAsyncLockTests
    {
        [Fact]
        public void TestConcurentAccess()
        {
            var tasks = new List<Task>();
            for (int i = 0; i < 100; i++)
            {
                tasks.Add(AsyncMethod("A", i.ToString()));
                tasks.Add(AsyncMethod("B", i.ToString()));
            }
            Task.WaitAll(tasks.ToArray());
        }

        private async Task AsyncMethod(string key, string msg)
        {
            using (var releaser = await AsyncLock.GetLockByKey(key).LockAsync())
            {
                await new TaskFactory().StartNew(() => Debug.WriteLine(key + "-" + msg));
            }
        }
    }
}
