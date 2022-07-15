using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

namespace VirtoCommerce.Platform.Caching.Tests
{
    public class RepeatAttribute : DataAttribute
    {
        private readonly int _count;

        public RepeatAttribute(int count)
        {
            if (count < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be greater than 0");
            }
            _count = count;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            return Enumerable.Repeat<object[]>(Array.Empty<object>(), _count);
        }
    }
}
