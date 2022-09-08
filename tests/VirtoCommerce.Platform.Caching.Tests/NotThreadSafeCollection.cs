using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace VirtoCommerce.Platform.Caching.Tests
{
    /// <summary>
    /// This statement defines a collection for tests can't be executed in parallel
    /// Some methods of the caching globally affect another tests. Therefore some tests need to run sequentally to avoid false-positive cases.
    /// Please take a ref for details: https://github.com/xunit/xunit/issues/1999
    /// </summary>
    [CollectionDefinition(nameof(NotThreadSafeCollection), DisableParallelization = true)]    
    public class NotThreadSafeCollection
    {
    }
}
