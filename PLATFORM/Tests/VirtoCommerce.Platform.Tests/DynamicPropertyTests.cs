using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Data.DynamicProperties;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.Platform.Data.Repositories;

namespace VirtoCommerce.Platform.Tests
{
    public class Cart : Entity, IHasDynamicProperties
    {
        public Cart(string id)
        {
            Id = id;
            LineItems = new List<LineItem>();
            DynamicProperties = new List<DynamicObjectProperty>();
        }
        public string ObjectType { get; set; }
        public ICollection<DynamicObjectProperty> DynamicProperties { get; set; }
        public ICollection<LineItem> LineItems { get; set; }
    }

    public class LineItem : Entity, IHasDynamicProperties
    {
        public LineItem(string id)
        {
            Id = id;
            DynamicProperties = new List<DynamicObjectProperty>();
        }
        public string ObjectType { get; set; }
        public ICollection<DynamicObjectProperty> DynamicProperties { get; set; }
    }

    [TestClass]
    public class DynamicPropertyTests
    {

        [TestMethod]
        public void IterativeSavingObjectsWithproperties_PropertiesSholdSaved()
        {
            var propertyService = GetDynamicPropertyService();
            var property = new DynamicProperty
            {
                Id = "LineItemProperty",
                Name = "Property",
                ObjectType = typeof(LineItem).FullName,
                ValueType = DynamicPropertyValueType.ShortText,
            };
            //Create new dynamic property for lineItem type
            propertyService.SaveProperties(new[] { property });

            var cart = new Cart("Cart #1");
            var lineItem1 = new LineItem("LineItem #1");
            lineItem1.DynamicProperties.Add(new DynamicObjectProperty { Name = property.Name, Values = new[] { new DynamicPropertyObjectValue { Value = "value1" } } });

            var lineItem2 = new LineItem("LineItem #2");
            lineItem2.DynamicProperties.Add(new DynamicObjectProperty { Name = property.Name, Values = new[] { new DynamicPropertyObjectValue { Value = "value2" } } });
            cart.LineItems.AddRange(new[] { lineItem1, lineItem2 });

            //Save aggregate cart object with line items with initial property values
            propertyService.SaveDynamicPropertyValues(cart);
            //cleanup
            lineItem1.DynamicProperties.Clear();
            lineItem2.DynamicProperties.Clear();

            propertyService.LoadDynamicPropertyValues(cart);

            //Simulate iterative update (as in you UpdateCartWrapper class)
            lineItem1.DynamicProperties.First().Values.First().Value = "value11";
            propertyService.SaveDynamicPropertyValues(lineItem1);

            lineItem2.DynamicProperties.First().Values.First().Value = "value22";
            propertyService.SaveDynamicPropertyValues(lineItem2);

            //cleanup
            lineItem1.DynamicProperties.Clear();
            lineItem2.DynamicProperties.Clear();

            propertyService.LoadDynamicPropertyValues(cart);

            Assert.IsTrue(lineItem1.DynamicProperties.First().Values.First().Value.ToString() == "value11");
            Assert.IsTrue(lineItem2.DynamicProperties.First().Values.First().Value.ToString() == "value22");
        }

        [TestMethod]
        public void PartialUpdateObjectDynamicPropertiesValues_ShouldUpdateOnlyPassedPropertiesValues()
        {
            var propertyService = GetDynamicPropertyService();
            var prop1 = new DynamicProperty
            {
                Id = "TestType-Property1",
                Name = "Property1",
                ObjectType = typeof(Cart).FullName,
                ValueType = DynamicPropertyValueType.ShortText,
                CreatedBy = "Auto"
            };
            var prop2 = new DynamicProperty
            {
                Id = "TestType-Property2",
                Name = "Property2",
                ObjectType = typeof(Cart).FullName,
                ValueType = DynamicPropertyValueType.ShortText,
                CreatedBy = "Auto"
            };

            propertyService.SaveProperties(new[] { prop1, prop2 });

            var obj = new Cart("1");
            //Add new properties values for object
            obj.DynamicProperties.Add(new DynamicObjectProperty { Name = "Property1", Values = new[] { new DynamicPropertyObjectValue { Value = "value1" } } });
            obj.DynamicProperties.Add(new DynamicObjectProperty { Name = "Property2", Values = new[] { new DynamicPropertyObjectValue { Value = "value2" } } });
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
            return new DynamicPropertyService(() => new PlatformRepository("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor(null)));
        }

    }
}
