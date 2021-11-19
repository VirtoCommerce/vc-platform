using System;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;

namespace VirtoCommerce.Platform.Tests.GenericCrud
{
    public class TestEntity : Entity, IDataEntity<TestEntity, TestModel>
    {
        public string Name { get; set; }
        public TestEntity FromModel(TestModel model, PrimaryKeyResolvingMap pkMap)
        {
            Id = model.Id;
            Name = model.Name;
            return this;
        }

        public void Patch(TestEntity target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            target.Id = Id;
            target.Name = Name;
        }

        public TestModel ToModel(TestModel model)
        {
            model.Id = Id;
            model.Name = Name;
            return model;
        }
    }
}
