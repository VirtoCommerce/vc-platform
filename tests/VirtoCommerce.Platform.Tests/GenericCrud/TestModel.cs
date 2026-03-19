using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Tests.GenericCrud
{
    public class TestModel : Entity, IHasOuterId, ICloneable
    {
        public string OuterId { get; set; }
        public string Name { get; set; }

        public object Clone()
        {
            return (TestModel)MemberwiseClone();
        }
    }
}
