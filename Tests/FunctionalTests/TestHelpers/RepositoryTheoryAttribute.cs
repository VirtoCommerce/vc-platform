using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;
using System.Reflection;

namespace FunctionalTests.TestHelpers
{
    public class RepositoryTheoryAttribute : FactAttribute
    {
        protected override IEnumerable<ITestCommand> EnumerateTestCommands(IMethodInfo method)
        {
            return from providerLanguageCombination in GetCombinations(method.MethodInfo)
                         select new RepositoryTheoryCommand(
                             method,
                             providerLanguageCombination.RepositoryProvider)
                       ;
        }

        private static IEnumerable<VariantAttribute> GetCombinations(MethodInfo method)
        {
            var methodVariants
                = method
                    .GetCustomAttributes(typeof(VariantAttribute), true)
                    .Cast<VariantAttribute>()
                    .ToList();

            if (methodVariants.Any())
            {
                return methodVariants;
            }

            var typeVariants
                = method.DeclaringType
                        .GetCustomAttributes(typeof(VariantAttribute), true)
                        .Cast<VariantAttribute>()
                        .ToList();

            if (typeVariants.Any())
            {
                return typeVariants;
            }

            return new[] { new VariantAttribute(RepositoryProvider.EntityFramework) };
        }
    }
}
