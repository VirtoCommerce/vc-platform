using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.Platform.Core.Common
{
    public class PolymorphicBaseTypeInfo<T> : IPolymorphicBaseTypeInfo
    {
        public PolymorphicBaseTypeInfo(string discriminatorName)
        {
            if (discriminatorName.IsNullOrEmpty())
            {
                throw new ArgumentException($"\"{ nameof(discriminatorName) }\" cannot be null or empty");
            }

            DiscriminatorName = discriminatorName;
        }

        public Type Type { get => typeof(T); }
        public string DiscriminatorName { get; private set; }
        public IEnumerable<Type> DerivedTypes { get => AbstractTypeFactory<T>.AllTypeInfos.SelectMany(x => x.AllSubclasses); }
    }
}
