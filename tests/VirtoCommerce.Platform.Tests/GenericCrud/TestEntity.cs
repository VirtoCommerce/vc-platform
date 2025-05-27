using System;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;

namespace VirtoCommerce.Platform.Tests.GenericCrud
{
    public class TestEntity : Entity, IHasOuterId, IDataEntity<TestEntity, TestModel>
    {
        public string OuterId { get; set; }
        public string Name { get; set; }

        public TestEntity FromModel(TestModel model, PrimaryKeyResolvingMap pkMap)
        {
            pkMap.AddPair(model, this);

            Id = model.Id;
            OuterId = model.OuterId;
            Name = model.Name;
            return this;
        }

        public void Patch(TestEntity target)
        {
            ArgumentNullException.ThrowIfNull(target);

            target.Id = Id;
            target.OuterId = OuterId;
            target.Name = Name;
        }

        public TestModel ToModel(TestModel model)
        {
            model.Id = Id;
            model.OuterId = OuterId;
            model.Name = Name;
            return model;
        }
    }
}
