using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.DynamicProperties;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.Platform.Data.Repositories;
using VirtoCommerce.Platform.Data.Security;

namespace VirtoCommerce.Platform.Tests
{
    public class TestType : IHasDynamicProperties
    {
        public string Id { get; set; }
		public string ObjectType { get; set; }
		public ICollection<DynamicObjectProperty> DynamicProperties { get; set; }
    }


    [TestClass]
    public class DynamicPropertyTests
    {
        [TestMethod]
        public void PartialUpdateObjectDynamicPropertiesValues_ShouldUpdateOnlyPassedPropertiesValues()
        {
            var propertyService = GetDynamicPropertyService();
            var prop1 = new DynamicProperty
            {
                Id = "TestType-Property1",
                Name = "Property1",
                ObjectType = typeof(TestType).FullName,
                ValueType = DynamicPropertyValueType.ShortText,
                CreatedBy = "Auto"
            };
            var prop2 = new DynamicProperty
            {
                Id = "TestType-Property2",
                Name = "Property2",
                ObjectType = typeof(TestType).FullName,
                ValueType = DynamicPropertyValueType.ShortText,
                CreatedBy = "Auto"
            };

            propertyService.SaveProperties(new[] { prop1, prop2 });

            var obj = new TestType()
            {
                Id = "1",
                DynamicProperties = new List<DynamicObjectProperty>()
            };
            //Add new properties values for object
            obj.DynamicProperties.Add(new DynamicObjectProperty { Name = "Property1",  Values = new[] { new DynamicPropertyObjectValue { Value = "value1" } } });
            obj.DynamicProperties.Add(new DynamicObjectProperty { Name = "Property2",  Values = new[] { new DynamicPropertyObjectValue { Value = "value2" } } });
            propertyService.SaveDynamicPropertyValues(obj);

            obj.DynamicProperties.Clear();
            //Load object and check that property values saved
            propertyService.LoadDynamicPropertyValues(obj);

            Assert.IsTrue(obj.DynamicProperties.Count() == 2);
            Assert.IsTrue(obj.DynamicProperties.All(x => x.Values.Any()));

            //Remove one property value for partial update (save only one property Propety2 with value 'new')
            var objProp1 = obj.DynamicProperties.FirstOrDefault();
            var objProp2 = obj.DynamicProperties.LastOrDefault();
            obj.DynamicProperties.Remove(objProp1);
            objProp2.Values.First().Value = "new";
            propertyService.SaveDynamicPropertyValues(obj);

            obj.DynamicProperties.Clear();
            propertyService.LoadDynamicPropertyValues(obj);
            //Check that count not changed
            Assert.IsTrue(obj.DynamicProperties.Count() == 2);
            //All has values
            Assert.IsTrue(obj.DynamicProperties.All(x => x.Values.Any()));
      
        }

        
        private IDynamicPropertyService GetDynamicPropertyService()
        {
            return new DynamicPropertyService(() => new PlatformRepository("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor()));
        }
     
    }
}
