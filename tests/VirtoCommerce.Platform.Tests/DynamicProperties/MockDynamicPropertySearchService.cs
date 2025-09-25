using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.Platform.Tests.DynamicProperties;

public class MockDynamicPropertySearchService : IDynamicPropertySearchService
{
    private readonly List<DynamicProperty> _properties;

    public MockDynamicPropertySearchService()
    {
        _properties = new List<DynamicProperty>
        {
            new DynamicProperty
            {
                Name = "IntegerFieldSingleValue",
                Description = "Description of IntegerFieldSingleValue",
                ObjectType = typeof(TestEntityWithDynamicProperties).FullName,
                IsArray = false,
                IsDictionary = false,
                IsMultilingual = false,
                IsRequired = false,
                DisplayOrder = 1,
                ValueType = DynamicPropertyValueType.Integer,
                DisplayNames =
                [
                    new DynamicPropertyName { Locale = "en-US", Name = null },
                    new DynamicPropertyName { Locale = "fr-FR", Name = null }
                ],
                CreatedDate = DateTime.Parse("2025-08-07T16:47:59.0954966Z"),
                ModifiedDate = DateTime.Parse("2025-08-07T16:47:59.0954966Z"),
                CreatedBy = "admin",
                ModifiedBy = "admin",
                Id = "38dd943b-4e59-4e81-b08f-IntegerFieldSingleValue"
            },
            new DynamicProperty
            {
                Name = "ShortTextFieldSingleValue",
                Description = "Description of Short Text Field Single Value",
                ObjectType = typeof(TestEntityWithDynamicProperties).FullName,
                IsArray = false,
                IsDictionary = false,
                IsMultilingual = false,
                IsRequired = false,
                DisplayOrder = 2,
                ValueType = DynamicPropertyValueType.ShortText,
                DisplayNames =
                [
                    new DynamicPropertyName { Locale = "en-US", Name = null },
                    new DynamicPropertyName { Locale = "fr-FR", Name = null }
                ],
                CreatedDate = DateTime.Parse("2025-06-13T18:40:10.6332198Z"),
                ModifiedDate = DateTime.Parse("2025-08-07T16:46:58.5460043Z"),
                CreatedBy = "admin",
                ModifiedBy = "admin",
                Id = "904d359f-222b-4f62-861e-ShortTextFieldSingleValue"
            },
            new DynamicProperty
            {
                Name = "LongTextFieldSingleValue",
                Description = "Description of Long Text Field Single Value",
                ObjectType = typeof(TestEntityWithDynamicProperties).FullName,
                IsArray = false,
                IsDictionary = false,
                IsMultilingual = false,
                IsRequired = false,
                DisplayOrder = 3,
                ValueType = DynamicPropertyValueType.LongText,
                DisplayNames =
                [
                    new DynamicPropertyName { Locale = "en-US", Name = null },
                    new DynamicPropertyName { Locale = "fr-FR", Name = null }
                ],
                CreatedDate = DateTime.Parse("2025-06-13T18:40:10.6332198Z"),
                ModifiedDate = DateTime.Parse("2025-08-07T16:46:58.5460043Z"),
                CreatedBy = "admin",
                ModifiedBy = "admin",
                Id = "904d359f-222b-4f62-861e-LongTextFieldSingleValue"
            },
            new DynamicProperty
            {
                Name = "HtmlFieldSingleValue",
                Description = "Description of Html Text Field Single Value",
                ObjectType = typeof(TestEntityWithDynamicProperties).FullName,
                IsArray = false,
                IsDictionary = false,
                IsMultilingual = false,
                IsRequired = false,
                DisplayOrder = 3,
                ValueType = DynamicPropertyValueType.Html,
                DisplayNames =
                [
                    new DynamicPropertyName { Locale = "en-US", Name = null },
                    new DynamicPropertyName { Locale = "fr-FR", Name = null }
                ],
                CreatedDate = DateTime.Parse("2025-06-13T18:40:10.6332198Z"),
                ModifiedDate = DateTime.Parse("2025-08-07T16:46:58.5460043Z"),
                CreatedBy = "admin",
                ModifiedBy = "admin",
                Id = "904d359f-222b-4f62-861e-HtmlFieldSingleValue"
            },
            new DynamicProperty
            {
                Name = "DecimalFieldSingleValue",
                Description = "Description of Decimal Field Single Value",
                ObjectType = typeof(TestEntityWithDynamicProperties).FullName,
                IsArray = false,
                IsDictionary = false,
                IsMultilingual = false,
                IsRequired = false,
                DisplayOrder = 4,
                ValueType = DynamicPropertyValueType.Decimal,
                DisplayNames =
                [
                    new DynamicPropertyName { Locale = "en-US", Name = null },
                    new DynamicPropertyName { Locale = "fr-FR", Name = null }
                ],
                CreatedDate = DateTime.Parse("2025-06-13T18:40:10.6332198Z"),
                ModifiedDate = DateTime.Parse("2025-08-07T16:46:58.5460043Z"),
                CreatedBy = "admin",
                ModifiedBy = "admin",
                Id = "904d359f-222b-4f62-861e-DecimalFieldSingleValue"
            },
            new DynamicProperty
            {
                Name = "BooleantFieldSingleValue",
                Description = "Description of Boolean Text Field Single Value",
                ObjectType = typeof(TestEntityWithDynamicProperties).FullName,
                IsArray = false,
                IsDictionary = false,
                IsMultilingual = false,
                IsRequired = false,
                DisplayOrder = 5,
                ValueType = DynamicPropertyValueType.Boolean,
                DisplayNames =
                [
                    new DynamicPropertyName { Locale = "en-US", Name = null },
                    new DynamicPropertyName { Locale = "fr-FR", Name = null }
                ],
                CreatedDate = DateTime.Parse("2025-06-13T18:40:10.6332198Z"),
                ModifiedDate = DateTime.Parse("2025-08-07T16:46:58.5460043Z"),
                CreatedBy = "admin",
                ModifiedBy = "admin",
                Id = "904d359f-222b-4f62-861e-BooleantFieldSingleValue"
            },
            new DynamicProperty
            {
                Name = "DateTimeFieldSingleValue",
                Description = "Description of DateTime Field Single Value",
                ObjectType = typeof(TestEntityWithDynamicProperties).FullName,
                IsArray = false,
                IsDictionary = false,
                IsMultilingual = false,
                IsRequired = false,
                DisplayOrder = 5,
                ValueType = DynamicPropertyValueType.DateTime,
                DisplayNames =
                [
                    new DynamicPropertyName { Locale = "en-US", Name = null },
                    new DynamicPropertyName { Locale = "fr-FR", Name = null }
                ],
                CreatedDate = DateTime.Parse("2025-06-13T18:40:10.6332198Z"),
                ModifiedDate = DateTime.Parse("2025-08-07T16:46:58.5460043Z"),
                CreatedBy = "admin",
                ModifiedBy = "admin",
                Id = "904d359f-222b-4f62-861e-DateTimeFieldSingleValue"
            },
            new DynamicProperty
            {
                Name = "ImageFieldSingleValue",
                Description = "Description of ImageFieldSingleValue",
                ObjectType = typeof(TestEntityWithDynamicProperties).FullName,
                IsArray = false,
                IsDictionary = false,
                IsMultilingual = false,
                IsRequired = false,
                DisplayOrder = 6,
                ValueType = DynamicPropertyValueType.Image,
                DisplayNames =
                [
                    new DynamicPropertyName { Locale = "en-US", Name = null },
                    new DynamicPropertyName { Locale = "fr-FR", Name = null }
                ],
                CreatedDate = DateTime.Parse("2025-06-13T18:40:10.6332198Z"),
                ModifiedDate = DateTime.Parse("2025-08-07T16:46:58.5460043Z"),
                CreatedBy = "admin",
                ModifiedBy = "admin",
                Id = "904d359f-222b-4f62-861e-ImageFieldSingleValue"
            },
            // Multi Value Properties
            new DynamicProperty
            {
                Name = "IntegerFieldMultiValue",
                Description = "Description of IntegerFieldSingleValue",
                ObjectType = typeof(TestEntityWithDynamicProperties).FullName,
                IsArray = true,
                IsDictionary = false,
                IsMultilingual = false,
                IsRequired = false,
                DisplayOrder = 1,
                ValueType = DynamicPropertyValueType.Integer,
                DisplayNames =
                [
                    new DynamicPropertyName { Locale = "en-US", Name = null },
                    new DynamicPropertyName { Locale = "fr-FR", Name = null }
                ],
                CreatedDate = DateTime.Parse("2025-08-07T16:47:59.0954966Z"),
                ModifiedDate = DateTime.Parse("2025-08-07T16:47:59.0954966Z"),
                CreatedBy = "admin",
                ModifiedBy = "admin",
                Id = "38dd943b-4e59-4e81-b08f-IntegerFieldMultiValue"
            },
            new DynamicProperty
            {
                Name = "ShortTextFieldMultiValue",
                Description = "Description of Short Text Field Multi Value",
                ObjectType = typeof(TestEntityWithDynamicProperties).FullName,
                IsArray = true,
                IsDictionary = false,
                IsMultilingual = false,
                IsRequired = false,
                DisplayOrder = 2,
                ValueType = DynamicPropertyValueType.ShortText,
                DisplayNames =
                [
                    new DynamicPropertyName { Locale = "en-US", Name = null },
                    new DynamicPropertyName { Locale = "fr-FR", Name = null }
                ],
                CreatedDate = DateTime.Parse("2025-06-13T18:40:10.6332198Z"),
                ModifiedDate = DateTime.Parse("2025-08-07T16:46:58.5460043Z"),
                CreatedBy = "admin",
                ModifiedBy = "admin",
                Id = "904d359f-222b-4f62-861e-ShortTextFieldMultiValue"
            },
            new DynamicProperty
            {
                Name = "LongTextFieldMultiValue",
                Description = "Description of Long Text Field Multi Value",
                ObjectType = typeof(TestEntityWithDynamicProperties).FullName,
                IsArray = true,
                IsDictionary = false,
                IsMultilingual = false,
                IsRequired = false,
                DisplayOrder = 3,
                ValueType = DynamicPropertyValueType.LongText,
                DisplayNames =
                [
                    new DynamicPropertyName { Locale = "en-US", Name = null },
                    new DynamicPropertyName { Locale = "fr-FR", Name = null }
                ],
                CreatedDate = DateTime.Parse("2025-06-13T18:40:10.6332198Z"),
                ModifiedDate = DateTime.Parse("2025-08-07T16:46:58.5460043Z"),
                CreatedBy = "admin",
                ModifiedBy = "admin",
                Id = "904d359f-222b-4f62-861e-LongTextFieldMultiValue"
            },
            new DynamicProperty
            {
                Name = "HtmlFieldMultiValue",
                Description = "Description of Html Text Field Multi Value",
                ObjectType = typeof(TestEntityWithDynamicProperties).FullName,
                IsArray = true,
                IsDictionary = false,
                IsMultilingual = false,
                IsRequired = false,
                DisplayOrder = 3,
                ValueType = DynamicPropertyValueType.Html,
                DisplayNames =
                [
                    new DynamicPropertyName { Locale = "en-US", Name = null },
                    new DynamicPropertyName { Locale = "fr-FR", Name = null }
                ],
                CreatedDate = DateTime.Parse("2025-06-13T18:40:10.6332198Z"),
                ModifiedDate = DateTime.Parse("2025-08-07T16:46:58.5460043Z"),
                CreatedBy = "admin",
                ModifiedBy = "admin",
                Id = "904d359f-222b-4f62-861e-HtmlFieldMultiValue"
            },
            new DynamicProperty
            {
                Name = "DecimalFieldMultiValue",
                Description = "Description of Decimal Field Multi Value",
                ObjectType = typeof(TestEntityWithDynamicProperties).FullName,
                IsArray = true,
                IsDictionary = false,
                IsMultilingual = false,
                IsRequired = false,
                DisplayOrder = 4,
                ValueType = DynamicPropertyValueType.Decimal,
                DisplayNames =
                [
                    new DynamicPropertyName { Locale = "en-US", Name = null },
                    new DynamicPropertyName { Locale = "fr-FR", Name = null }
                ],
                CreatedDate = DateTime.Parse("2025-06-13T18:40:10.6332198Z"),
                ModifiedDate = DateTime.Parse("2025-08-07T16:46:58.5460043Z"),
                CreatedBy = "admin",
                ModifiedBy = "admin",
                Id = "904d359f-222b-4f62-861e-DecimalFieldMultiValue"
            },
            new DynamicProperty
            {
                Name = "BooleantFieldMultiValue",
                Description = "Description of Boolean Text Field Multi Value",
                ObjectType = typeof(TestEntityWithDynamicProperties).FullName,
                IsArray = true,
                IsDictionary = false,
                IsMultilingual = false,
                IsRequired = false,
                DisplayOrder = 5,
                ValueType = DynamicPropertyValueType.Boolean,
                DisplayNames =
                [
                    new DynamicPropertyName { Locale = "en-US", Name = null },
                    new DynamicPropertyName { Locale = "fr-FR", Name = null }
                ],
                CreatedDate = DateTime.Parse("2025-06-13T18:40:10.6332198Z"),
                ModifiedDate = DateTime.Parse("2025-08-07T16:46:58.5460043Z"),
                CreatedBy = "admin",
                ModifiedBy = "admin",
                Id = "904d359f-222b-4f62-861e-BooleantFieldMultiValue"
            },
            new DynamicProperty
            {
                Name = "DateTimeFieldMultiValue",
                Description = "Description of DateTime Field Multi Value",
                ObjectType = typeof(TestEntityWithDynamicProperties).FullName,
                IsArray = true,
                IsDictionary = false,
                IsMultilingual = false,
                IsRequired = false,
                DisplayOrder = 6,
                ValueType = DynamicPropertyValueType.DateTime,
                DisplayNames =
                [
                    new DynamicPropertyName { Locale = "en-US", Name = null },
                    new DynamicPropertyName { Locale = "fr-FR", Name = null }
                ],
                CreatedDate = DateTime.Parse("2025-06-13T18:40:10.6332198Z"),
                ModifiedDate = DateTime.Parse("2025-08-07T16:46:58.5460043Z"),
                CreatedBy = "admin",
                ModifiedBy = "admin",
                Id = "904d359f-222b-4f62-861e-DateTimeFieldMultiValue"
            },
            new DynamicProperty
            {
                Name = "ImageFieldMultiValue",
                Description = "Description of ImageFieldMultiValue",
                ObjectType = typeof(TestEntityWithDynamicProperties).FullName,
                IsArray = true,
                IsDictionary = false,
                IsMultilingual = false,
                IsRequired = false,
                DisplayOrder = 7,
                ValueType = DynamicPropertyValueType.Image,
                DisplayNames =
                [
                    new DynamicPropertyName { Locale = "en-US", Name = null },
                    new DynamicPropertyName { Locale = "fr-FR", Name = null }
                ],
                CreatedDate = DateTime.Parse("2025-06-13T18:40:10.6332198Z"),
                ModifiedDate = DateTime.Parse("2025-08-07T16:46:58.5460043Z"),
                CreatedBy = "admin",
                ModifiedBy = "admin",
                Id = "904d359f-222b-4f62-861e-ImageFieldMultiValue"
            },
            new DynamicProperty
            {
                Name = "ShortTextFieldSingleValue_Localized",
                Description = "Description of ShortTextFieldSingleValue_Localized",
                ObjectType = typeof(TestEntityWithDynamicProperties).FullName,
                IsArray = false,
                IsDictionary = false,
                IsMultilingual = true,
                IsRequired = false,
                DisplayOrder = 8,
                ValueType = DynamicPropertyValueType.ShortText,
                DisplayNames =
                [
                    new DynamicPropertyName { Locale = "en-US", Name = null },
                    new DynamicPropertyName { Locale = "fr-FR", Name = null }
                ],
                CreatedDate = DateTime.Parse("2025-06-13T18:40:10.6332198Z"),
                ModifiedDate = DateTime.Parse("2025-08-07T16:46:58.5460043Z"),
                CreatedBy = "admin",
                ModifiedBy = "admin",
                Id = "904d359f-222b-4f62-861e-ShortTextFieldSingleValue_Localized"
            },
            new DynamicProperty
            {
                Name = "LongTextFieldSingleValue_Localized",
                Description = "Description of LongTextFieldSingleValue_Localized",
                ObjectType = typeof(TestEntityWithDynamicProperties).FullName,
                IsArray = false,
                IsDictionary = false,
                IsMultilingual = true,
                IsRequired = false,
                DisplayOrder = 9,
                ValueType = DynamicPropertyValueType.LongText,
                DisplayNames =
                [
                    new DynamicPropertyName { Locale = "en-US", Name = null },
                    new DynamicPropertyName { Locale = "fr-FR", Name = null }
                ],
                CreatedDate = DateTime.Parse("2025-06-13T18:40:10.6332198Z"),
                ModifiedDate = DateTime.Parse("2025-08-07T16:46:58.5460043Z"),
                CreatedBy = "admin",
                ModifiedBy = "admin",
                Id = "904d359f-222b-4f62-861e-LongTextFieldSingleValue_Localized"
            },
            new DynamicProperty
            {
                Name = "HtmlTextFieldSingleValue_Localized",
                Description = "Description of HtmlTextFieldSingleValue_Localized",
                ObjectType = typeof(TestEntityWithDynamicProperties).FullName,
                IsArray = false,
                IsDictionary = false,
                IsMultilingual = true,
                IsRequired = false,
                DisplayOrder = 10,
                ValueType = DynamicPropertyValueType.Html,
                DisplayNames =
                [
                    new DynamicPropertyName { Locale = "en-US", Name = null },
                    new DynamicPropertyName { Locale = "fr-FR", Name = null }
                ],
                CreatedDate = DateTime.Parse("2025-06-13T18:40:10.6332198Z"),
                ModifiedDate = DateTime.Parse("2025-08-07T16:46:58.5460043Z"),
                CreatedBy = "admin",
                ModifiedBy = "admin",
                Id = "904d359f-222b-4f62-861e-HtmlTextFieldSingleValue_Localized"
            },
            new DynamicProperty
            {
                Name = "ShortTextField_MultiValue",
                Description = "Description of ShortTextField_MultiValue",
                ObjectType = typeof(TestEntityWithDynamicProperties).FullName,
                IsArray = true,
                IsDictionary = false,
                IsMultilingual = false,
                IsRequired = false,
                DisplayOrder = 11,
                ValueType = DynamicPropertyValueType.ShortText,
                DisplayNames =
                [
                    new DynamicPropertyName { Locale = "en-US", Name = null },
                    new DynamicPropertyName { Locale = "fr-FR", Name = null }
                ],
                CreatedDate = DateTime.Parse("2025-06-13T18:40:10.6332198Z"),
                ModifiedDate = DateTime.Parse("2025-08-07T16:46:58.5460043Z"),
                CreatedBy = "admin",
                ModifiedBy = "admin",
                Id = "904d359f-222b-4f62-861e-ShortTextField_MultiValue"
            },
            new DynamicProperty
            {
                Name = "IntegerField_MultiValue",
                Description = "Description of IntegerField_MultiValue",
                ObjectType = typeof(TestEntityWithDynamicProperties).FullName,
                IsArray = true,
                IsDictionary = false,
                IsMultilingual = false,
                IsRequired = false,
                DisplayOrder = 12,
                ValueType = DynamicPropertyValueType.Integer,
                DisplayNames =
                [
                    new DynamicPropertyName { Locale = "en-US", Name = null },
                    new DynamicPropertyName { Locale = "fr-FR", Name = null }
                ],
                CreatedDate = DateTime.Parse("2025-06-13T18:40:10.6332198Z"),
                ModifiedDate = DateTime.Parse("2025-08-07T16:46:58.5460043Z"),
                CreatedBy = "admin",
                ModifiedBy = "admin",
                Id = "904d359f-222b-4f62-861e-IntegerField_MultiValue"
            },
            new DynamicProperty
            {
                Name = "DecimalField_MultiValue",
                Description = "Description of DecimalField_MultiValue",
                ObjectType = typeof(TestEntityWithDynamicProperties).FullName,
                IsArray = true,
                IsDictionary = false,
                IsMultilingual = false,
                IsRequired = false,
                DisplayOrder = 12,
                ValueType = DynamicPropertyValueType.Decimal,
                DisplayNames =
                [
                    new DynamicPropertyName { Locale = "en-US", Name = null },
                    new DynamicPropertyName { Locale = "fr-FR", Name = null }
                ],
                CreatedDate = DateTime.Parse("2025-06-13T18:40:10.6332198Z"),
                ModifiedDate = DateTime.Parse("2025-08-07T16:46:58.5460043Z"),
                CreatedBy = "admin",
                ModifiedBy = "admin",
                Id = "904d359f-222b-4f62-861e-DecimalField_MultiValue"
            },
            new DynamicProperty
            {
                Name = "ShortTextField_MultiValue_Localized",
                Description = "Description of ShortTextField_MultiValue_Localized",
                ObjectType = typeof(TestEntityWithDynamicProperties).FullName,
                IsArray = true,
                IsDictionary = false,
                IsMultilingual = true,
                IsRequired = false,
                DisplayOrder = 13,
                ValueType = DynamicPropertyValueType.ShortText,
                DisplayNames =
                [
                    new DynamicPropertyName { Locale = "en-US", Name = null },
                    new DynamicPropertyName { Locale = "fr-FR", Name = null }
                ],
                CreatedDate = DateTime.Parse("2025-06-13T18:40:10.6332198Z"),
                ModifiedDate = DateTime.Parse("2025-08-07T16:46:58.5460043Z"),
                CreatedBy = "admin",
                ModifiedBy = "admin",
                Id = "904d359f-222b-4f62-861e-ShortTextField_MultiValue_Localized"
            },
        };
    }

    public Task<DynamicPropertySearchResult> SearchAsync(DynamicPropertySearchCriteria criteria, bool clone = true)
    {
        var result = new DynamicPropertySearchResult
        {
            Results = _properties
                .Where(p => criteria.ObjectType == null || p.ObjectType == criteria.ObjectType)
                .OrderBy(p => p.DisplayOrder)
                .Skip(criteria.Skip)
                .Take(criteria.Take)
                .ToList(),
            TotalCount = _properties.Count
        };
        return Task.FromResult(result);
    }
}
