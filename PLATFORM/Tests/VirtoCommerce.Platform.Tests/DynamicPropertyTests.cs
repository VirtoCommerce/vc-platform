using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Data.DynamicProperties;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.Platform.Data.Repositories;

namespace VirtoCommerce.Platform.Tests
{
    public class Parent : IHasDynamicProperties
    {
        public string Id { get; set; }
		public string ObjectType { get; set; }
		public ICollection<DynamicObjectProperty> DynamicProperties { get; set; }
    }

    public class Child : Parent
    {
    }

    [TestClass]
    public class DynamicPropertyTests
    {
        [TestMethod]
        public void GetObjectTypes()
        {
            var service = GetDynamicPropertyService();
            var typeNames = service.GetAvailableObjectTypeNames();
        }

        [TestMethod]
        public void SaveLoadPropertiesAndValues()
        {
            var service = GetDynamicPropertyService();

            var objectType = service.GetObjectTypeName(typeof(Parent));

            // Delete existing properties
            var existingTypeProperties = service.GetProperties(objectType);
            var propertyIds = existingTypeProperties.Select(p => p.Id).ToArray();
            service.DeleteProperties(propertyIds);

            // Test properties
            var typeProperties = new[]
            {
                new DynamicProperty
                {
                    ObjectType = objectType,
                    Name = "Color",
                    ValueType = DynamicPropertyValueType.ShortText,
                    IsDictionary = true,
                    IsArray = true,
                    IsRequired = true,
                    IsMultilingual = true,
                    DisplayNames = new[]
                    {
                        new DynamicPropertyName
                        {
                            Locale = "en-US", Name = "Color"
                        },
                        new DynamicPropertyName
                        {
                            Locale = "ru-RU", Name = "Цвет"
                        },
                    },
                },
                new DynamicProperty
                {
                    ObjectType = objectType,
                    Name = "SingleValueProperty",
                    ValueType = DynamicPropertyValueType.Decimal,
                    DisplayNames = new[]
                    {
                        new DynamicPropertyName
                        {
                            Locale = "en-US", Name = "Single Value Property"
                        },
                        new DynamicPropertyName
                        {
                            Locale = "ru-RU", Name = "Свойство с одним значением"
                        },
                    },
                },
                new DynamicProperty
                {
                    ObjectType = objectType,
                    Name = "Array",
                    ValueType = DynamicPropertyValueType.ShortText,
                    IsArray = true,
                    DisplayNames = new[]
                    {
                        new DynamicPropertyName
                        {
                            Locale = "en-US", Name = "Array"
                        },
                        new DynamicPropertyName
                        {
                            Locale = "ru-RU", Name = "Массив"
                        },
                    },
                },
            };

            service.SaveProperties(typeProperties);

            existingTypeProperties = service.GetProperties(objectType);

            // Rename property
            var renamedProperties = new[] { existingTypeProperties[0] };
            var originalName = renamedProperties[0].Name;
            renamedProperties[0].Name = "NewName";
            service.SaveProperties(renamedProperties);

            // Return original name
            renamedProperties[0].Name = originalName;
            service.SaveProperties(renamedProperties);

            var arrayProperty = existingTypeProperties.First(p => p.Name == "Array");
            var singleValueProperty = existingTypeProperties.First(p => p.Name == "SingleValueProperty");
            var colorProperty = existingTypeProperties.First(p => p.Name == "Color");

            // Test dictionary items
            var newItems = new[]
            {
                new DynamicPropertyDictionaryItem
                {
                    Name = "Red",
                    DisplayNames = new[]
                    {
                        new DynamicPropertyDictionaryItemName
                        {
                            Locale = "en-US", Name = "Red"
                        },
                        new DynamicPropertyDictionaryItemName
                        {
                            Locale = "ru-RU", Name = "Красный"
                        },
                    },
                },
                new DynamicPropertyDictionaryItem
                {
                    Name = "Green",
                    DisplayNames = new[]
                    {
                        new DynamicPropertyDictionaryItemName
                        {
                            Locale = "en-US", Name = "Green"
                        },
                        new DynamicPropertyDictionaryItemName
                        {
                            Locale = "ru-RU", Name = "Зелёный"
                        },
                    },
                },
                new DynamicPropertyDictionaryItem
                {
                    Name = "Blue",
                    DisplayNames = new[]
                    {
                        new DynamicPropertyDictionaryItemName
                        {
                            Locale = "en-US", Name = "Blue"
                        },
                        new DynamicPropertyDictionaryItemName
                        {
                            Locale = "ru-RU", Name = "Синий"
                        },
                    },
                },
                new DynamicPropertyDictionaryItem
                {
                    Name = "Yellow",
                    DisplayNames = new[]
                    {
                        new DynamicPropertyDictionaryItemName
                        {
                            Locale = "en-US", Name = "Yellow"
                        },
                        new DynamicPropertyDictionaryItemName
                        {
                            Locale = "ru-RU", Name = "Жёлтый"
                        },
                    },
                },
            };

            service.SaveDictionaryItems(colorProperty.Id, newItems);

            var dictionaryItems = service.GetDictionaryItems(colorProperty.Id);

            var redColor = dictionaryItems.First(i => i.Name == "Red");
            var greenColor = dictionaryItems.First(i => i.Name == "Green");
            var blueColor = dictionaryItems.First(i => i.Name == "Blue");
            var yellowColor = dictionaryItems.First(i => i.Name == "Yellow");

            yellowColor.Name = "Pink";
            service.SaveDictionaryItems(colorProperty.Id, dictionaryItems);

            service.DeleteDictionaryItems(new[] { yellowColor.Id });

            // Test object values
			//var objectProperties = new[]
			//{
			//	new DynamicPropertyObjectValue
			//	{
			//		Property = new DynamicProperty { Id = colorProperty.Id },
			//		ObjectId = "111",
			//		Values = new object[] { new DynamicPropertyDictionaryItem { Id = redColor.Id } },
			//	},
			//	new DynamicPropertyObjectValue
			//	{
			//		Property = new DynamicProperty { Id = colorProperty.Id },
			//		ObjectId = "222",
			//		Values = new object[] { new DynamicPropertyDictionaryItem { Id = greenColor.Id }, new DynamicPropertyDictionaryItem { Id = blueColor.Id } },
			//	},

			//	new DynamicPropertyObjectValue
			//	{
			//		Property = new DynamicProperty { Id = singleValueProperty.Id },
			//		ObjectId = "111",
			//		Locale = "en-US",
			//		Values = new object[] { 1.1 },
			//	},
			//	new DynamicPropertyObjectValue
			//	{
			//		Property = new DynamicProperty { Id = singleValueProperty.Id },
			//		ObjectId = "111",
			//		Locale = "ru-RU",
			//		Values = new object[] { 1.2 },
			//	},
			//	new DynamicPropertyObjectValue
			//	{
			//		Property = new DynamicProperty { Id = singleValueProperty.Id },
			//		ObjectId = "222",
			//		Locale = "en-US",
			//		Values = new object[] { 2.1 },
			//	},
			//	new DynamicPropertyObjectValue
			//	{
			//		Property = new DynamicProperty { Id = singleValueProperty.Id },
			//		ObjectId = "222",
			//		Locale = "ru-RU",
			//		Values = new object[] { 2.2 },
			//	},

			//	new DynamicPropertyObjectValue
			//	{
			//		Property = new DynamicProperty { Id = arrayProperty.Id },
			//		ObjectId = "222",
			//		Locale = "en-US",
			//		Values = new object[] { "flower", "tree" },
			//	},
			//	new DynamicPropertyObjectValue
			//	{
			//		Property = new DynamicProperty { Id = arrayProperty.Id },
			//		ObjectId = "222",
			//		Locale = "ru-RU",
			//		Values = new object[] { "цветок", "дерево" },
			//	},
			//};

			//service.SaveObjectValues(objectProperties);

			//var objectProperties1 = service.GetObjectValues(objectType, "111");
			//var objectProperties2 = service.GetObjectValues(objectType, "222");

            var obj = new Parent { Id = "222" };
            service.LoadDynamicPropertyValues(obj);
            var decimalValue = obj.GetDynamicPropertyValue(singleValueProperty.Name, 0m);
            var dictionaryValue = obj.GetDynamicPropertyValue(colorProperty.Name, string.Empty);
        }

        private IDynamicPropertyService GetDynamicPropertyService()
        {
            return new DynamicPropertyService(() => new PlatformRepository("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor()));
        }
    }
}
