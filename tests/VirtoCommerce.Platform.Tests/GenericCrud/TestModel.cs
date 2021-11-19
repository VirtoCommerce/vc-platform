using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Tests.GenericCrud
{
    public class TestModel : Entity, ICloneable
    {
        public string Name { get; set; }
        public object Clone()
        {
            return MemberwiseClone() as TestModel;
        }
    }
}
