using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Tests.GenericCrud
{
    public class TestModel : Entity, ICloneable, IHasOuterId
    {
        public string Name { get; set; }
        public string OuterId { get; set; }

        public object Clone()
        {
            return MemberwiseClone() as TestModel;
        }
    }
}
