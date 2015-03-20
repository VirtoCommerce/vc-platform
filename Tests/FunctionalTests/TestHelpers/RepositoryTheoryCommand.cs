using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Sdk;

namespace FunctionalTests.TestHelpers
{
    internal class RepositoryTheoryCommand : FactCommand
    {
        private readonly RepositoryProvider _provider;

        public RepositoryTheoryCommand(IMethodInfo method, RepositoryProvider provider)
            :base(method)
        {
            _provider = provider;
            DisplayName = string.Format(
                    "{0} - DatabaseProvider: {1}", DisplayName, _provider);
        }

        public override MethodResult Execute(object testClass)
        {
            var funcTestClass = testClass as FunctionalTestBase;

            if (funcTestClass == null)
            {
                throw new InvalidOperationException(
                    string.Format(
                        "Expected {0} to be derived from {1}", testClass.GetType().FullName, typeof(FunctionalTestBase).FullName));
            }

            funcTestClass.Init(_provider);

            return base.Execute(testClass);
        }
    }
}
