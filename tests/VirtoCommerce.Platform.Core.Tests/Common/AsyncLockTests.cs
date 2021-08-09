using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace VirtoCommerce.Platform.Core.Tests.Common
{
    [Trait("Category", "Unit")]
    public class AsyncLockTests
    {
        [Fact]
        public Task AsyncLockExlusiveAccess()
        {
            int counter = 0;
            var tasks = new List<Task>();
            for (var threadNumber = 0; threadNumber < 3; threadNumber++)
            {
                var task = Task.Run(async () =>
               {
                   for (var i = 0; i < 10; i++)
                   {
                       using (await AsyncLock.GetLockByKey("test-key").GetReleaserAsync())
                       {
                           Debug.WriteLine($"{i} enter");
                           counter += i;
                           await Task.Delay(100);
                           counter -= i;
                           Debug.WriteLine($"{i} leave");
                           Assert.Equal(0, counter);
                       }
                   }
               });
                tasks.Add(task);
            }
            return Task.WhenAll(tasks);
        }
    }
}
