using System;
using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Common
{
    public interface IPolymorphicBaseTypeInfo
    {
        Type Type { get; }
        string DiscriminatorName { get; }
        IEnumerable<Type> DerivedTypes { get; }
    }
}
