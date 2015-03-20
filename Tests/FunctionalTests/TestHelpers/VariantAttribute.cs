using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalTests.TestHelpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class VariantAttribute : Attribute
    {
        public VariantAttribute(RepositoryProvider provider)
        {
            RepositoryProvider = provider;
        }

        public RepositoryProvider RepositoryProvider { get; private set; }
    }
}
