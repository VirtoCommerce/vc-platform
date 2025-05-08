using System;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;

namespace VirtoCommerce.Platform.Tests.GenericCrud
{
    public class TestEntity : Entity, IHasOuterId, IDataEntity<TestEntity, TestModel>
    {
        public string Name { get; set; }
        public string OuterId { get; set; }

        public TestEntity FromModel(TestModel model, PrimaryKeyResolvingMap pkMap)
        {
            pkMap.AddPair(model, this);

            Id = model.Id;
            Name = model.Name;
            OuterId = model.OuterId;
            return this;
        }

        public void Patch(TestEntity target)
        {
            ArgumentNullException.ThrowIfNull(target);

            target.Id = Id;
            target.Name = Name;
            target.OuterId = OuterId;
        }

        public TestModel ToModel(TestModel model)
        {
            model.Id = Id;
            model.Name = Name;
            model.OuterId = OuterId;
            return model;
        }
    }
}
