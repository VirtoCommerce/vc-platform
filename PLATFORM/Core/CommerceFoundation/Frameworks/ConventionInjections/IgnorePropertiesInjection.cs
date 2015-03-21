using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Foundation.Frameworks.ConventionInjections
{
    public class IgnorePropertiesInjection : LoopValueInjection
    {
        private string[] ignore;

        public IgnorePropertiesInjection(params string[] ignore)
        {
            this.ignore = ignore;
        }

        protected override bool UseSourceProp(string sourcePropName)
        {
            return !ignore.Any(i => string.Equals(i, sourcePropName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
