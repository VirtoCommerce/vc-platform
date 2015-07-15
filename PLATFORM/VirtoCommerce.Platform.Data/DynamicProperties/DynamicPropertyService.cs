using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Data.DynamicProperties.Converters;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Repositories;

namespace VirtoCommerce.Platform.Data.DynamicProperties
{
    public class DynamicPropertyService : IDynamicPropertyService
    {
        private readonly Func<IPlatformRepository> _repositoryFactory;

        public DynamicPropertyService(Func<IPlatformRepository> repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        #region IDynamicPropertyService Members

        public string[] GetAvailableObjectTypeNames()
        {
            var typeName = typeof(IHasDynamicProperties).Name;

            var result = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.GetInterface(typeName) != null)
                .Select(GetObjectTypeName)
                .ToArray();

            return result;
        }

        public string GetObjectTypeName(Type type)
        {
            return type.FullName;
        }


        public DynamicProperty[] GetProperties(string objectType)
        {
            if (objectType == null)
                throw new ArgumentNullException("objectType");

            var result = new List<DynamicProperty>();

            using (var repository = _repositoryFactory())
            {
                var properties = repository.DynamicProperties
                    .Include(p => p.DisplayNames)
                    .Where(p => p.ObjectType == objectType)
                    .OrderBy(p => p.Name)
                    .ToList();

                result.AddRange(properties.Select(p => p.ToModel()));
            }

            return result.ToArray();
        }

        public void SaveProperties(DynamicProperty[] properties)
        {
            if (properties == null)
                throw new ArgumentNullException("properties");

            using (var repository = _repositoryFactory())
            using (var changeTracker = new ObservableChangeTracker())
            {
                var newProperties = properties.Where(p => string.IsNullOrEmpty(p.Id));
                var newEntities = newProperties.Select(p => p.ToEntity());

                foreach (var entity in newEntities)
                {
                    repository.Add(entity);
                }

                var propertyIds = properties.Where(p => !string.IsNullOrEmpty(p.Id)).Select(p => p.Id).Distinct().ToArray();

                var existingProperties = repository.DynamicProperties
                    .Include(p => p.DisplayNames)
                    .Where(p => propertyIds.Contains(p.Id))
                    .ToList();

                if (existingProperties.Any())
                {
                    propertyIds = existingProperties.Select(p => p.Id).ToArray();
                    properties = properties.Where(p => propertyIds.Contains(p.Id)).ToArray();

                    var source = new { Properties = new ObservableCollection<DynamicPropertyEntity>(properties.Select(p => p.ToEntity())) };
                    var target = new { Properties = new ObservableCollection<DynamicPropertyEntity>(existingProperties) };

                    changeTracker.AddAction = x => repository.Add(x);
                    changeTracker.RemoveAction = x => repository.Remove(x);
                    changeTracker.Attach(target);

                    var propertyComparer = AnonymousComparer.Create((DynamicPropertyEntity p) => p.Id);
                    source.Properties.Patch(target.Properties, propertyComparer, (sourceProperty, targetProperty) => sourceProperty.Patch(targetProperty));
                }

                repository.UnitOfWork.Commit();
            }
        }

        public void DeleteProperties(string[] propertyIds)
        {
            if (propertyIds == null)
                throw new ArgumentNullException("propertyIds");

            using (var repository = _repositoryFactory())
            {
                var properties = repository.DynamicProperties
                    .Where(p => propertyIds.Contains(p.Id))
                    .ToList();

                foreach (var property in properties)
                {
                    repository.Remove(property);
                }

                repository.UnitOfWork.Commit();
            }
        }


        public DynamicPropertyDictionaryItem[] GetDictionaryItems(string propertyId)
        {
            if (propertyId == null)
                throw new ArgumentNullException("propertyId");

            var result = new List<DynamicPropertyDictionaryItem>();

            using (var repository = _repositoryFactory())
            {
                var items = repository.DynamicPropertyDictionaryItems
                    .Include(i => i.DisplayNames)
                    .Where(i => i.PropertyId == propertyId)
                    .ToList();

                result.AddRange(items.OrderBy(i => i.Name).Select(i => i.ToModel()));
            }

            return result.ToArray();
        }

        public void SaveDictionaryItems(string propertyId, DynamicPropertyDictionaryItem[] items)
        {
            if (propertyId == null)
                throw new ArgumentNullException("propertyId");
            if (items == null)
                throw new ArgumentNullException("items");

            using (var repository = _repositoryFactory())
            using (var changeTracker = new ObservableChangeTracker())
            {
                var property = repository.DynamicProperties.FirstOrDefault(p => p.Id == propertyId);

                if (property != null)
                {
                    var newItems = items.Where(i => string.IsNullOrEmpty(i.Id));
                    var newEntities = newItems.Select(i => i.ToEntity(property));

                    foreach (var entity in newEntities)
                    {
                        repository.Add(entity);
                    }

                    var itemIds = items.Where(i => !string.IsNullOrEmpty(i.Id)).Select(i => i.Id).Distinct().ToArray();

                    var existingItems = repository.DynamicPropertyDictionaryItems
                        .Include(i => i.DisplayNames)
                        .Where(i => itemIds.Contains(i.Id))
                        .ToList();

                    if (existingItems.Any())
                    {
                        itemIds = existingItems.Select(p => p.Id).ToArray();
                        items = items.Where(i => itemIds.Contains(i.Id)).ToArray();

                        var source = new { Items = new ObservableCollection<DynamicPropertyDictionaryItemEntity>(items.Select(i => i.ToEntity(property))) };
                        var target = new { Items = new ObservableCollection<DynamicPropertyDictionaryItemEntity>(existingItems) };

                        changeTracker.AddAction = x => repository.Add(x);
                        changeTracker.RemoveAction = x => repository.Remove(x);
                        changeTracker.Attach(target);

                        var itemComparer = AnonymousComparer.Create((DynamicPropertyDictionaryItemEntity i) => i.Id);
                        source.Items.Patch(target.Items, itemComparer, (sourceItem, targetItem) => sourceItem.Patch(targetItem));
                    }

                    repository.UnitOfWork.Commit();
                }
            }
        }

        public void DeleteDictionaryItems(string[] itemIds)
        {
            if (itemIds == null)
                throw new ArgumentNullException("itemIds");

            using (var repository = _repositoryFactory())
            {
                var items = repository.DynamicPropertyDictionaryItems
                    .Where(v => itemIds.Contains(v.Id))
                    .ToList();

                foreach (var item in items)
                {
                    repository.Remove(item);
                }

                repository.UnitOfWork.Commit();
            }
        }


        public DynamicPropertyObjectValue[] GetObjectValues(string objectType, string objectId)
        {
            if (objectType == null)
                throw new ArgumentNullException("objectType");
            if (objectId == null)
                throw new ArgumentNullException("objectId");

            var result = new List<DynamicPropertyObjectValue>();

            using (var repository = _repositoryFactory())
            {
                var properties = repository.DynamicProperties
                    .Where(p => p.ObjectType == objectType)
                    .OrderBy(p => p.Name)
                    .ToList();

                // This request will automatically fill values for loaded properties
                var values = repository.DynamicPropertyObjectValues
                    .Where(v => v.ObjectType == objectType && v.ObjectId == objectId)
                    .ToList();

                result.AddRange(properties.SelectMany(p => p.ToModel(objectId)));
            }

            return result.ToArray();
        }

        public void SaveObjectValues(DynamicPropertyObjectValue[] values)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            var propertyIds = values
                .Where(v => v.Property != null && !string.IsNullOrEmpty(v.Property.Id))
                .Select(v => v.Property.Id)
                .Distinct()
                .ToList();

            using (var repository = _repositoryFactory())
            using (var changeTracker = new ObservableChangeTracker())
            {
                var existingProperties = repository.DynamicProperties
                    .Where(p => propertyIds.Contains(p.Id))
                    .ToList();

                propertyIds = existingProperties.Select(p => p.Id).Distinct().ToList();
                values = values.Where(v => propertyIds.Contains(v.Property.Id)).ToArray();

                var valueKeys = values.Select(v => string.Join("-", v.Property.Id, v.ObjectId)).Distinct().ToList();

                var existingValues = repository.DynamicPropertyObjectValues
                    .Where(v => valueKeys.Contains(v.PropertyId + "-" + v.ObjectId))
                    .ToList();

                var properties = existingProperties.Select(p => p.ToModel()).ToList();

                var source = new { Items = new ObservableCollection<DynamicPropertyObjectValueEntity>(values.SelectMany(v => v.ToEntity(properties.First(p => p.Id == v.Property.Id)))) };
                var target = new { Items = new ObservableCollection<DynamicPropertyObjectValueEntity>(existingValues) };

                changeTracker.AddAction = x => repository.Add(x);
                changeTracker.RemoveAction = x => repository.Remove(x);
                changeTracker.Attach(target);

                var comparer = AnonymousComparer.Create((DynamicPropertyObjectValueEntity v) => string.Join("-", v.PropertyId, v.Locale, v.ToString(CultureInfo.InvariantCulture)));
                source.Items.Patch(target.Items, comparer, (sourceItem, targetItem) => { });

                repository.UnitOfWork.Commit();
            }
        }

        public void DeleteObjectValues(string objectType, string objectId)
        {
            if (objectType == null)
                throw new ArgumentNullException("objectType");
            if (objectId == null)
                throw new ArgumentNullException("objectId");

            using (var repository = _repositoryFactory())
            {
                var values = repository.DynamicPropertyObjectValues
                    .Where(v => v.ObjectType == objectType && v.ObjectId == objectId)
                    .ToList();

                foreach (var value in values)
                {
                    repository.Remove(value);
                }

                repository.UnitOfWork.Commit();
            }
        }


        public void LoadDynamicPropertyValues(IHasDynamicProperties owner)
        {
            var objectsWithDynamicProperties = owner.GetFlatObjectsListWithInterface<IHasDynamicProperties>();

            foreach (var objectWithDynamicProperties in objectsWithDynamicProperties)
            {
                if (objectWithDynamicProperties.Id != null)
                {
                    var storedValues = GetObjectValues(GetObjectTypeName(objectWithDynamicProperties), objectWithDynamicProperties.Id);

                    // Replace in-memory properties with stored in database
                    if (objectWithDynamicProperties.DynamicPropertyValues != null)
                    {
                        var result = new List<DynamicPropertyObjectValue>();

                        foreach (var value in objectWithDynamicProperties.DynamicPropertyValues)
                        {
                            var storedProperty = storedValues.FirstOrDefault(v => v.Property.Name == value.Property.Name);
                            result.Add(storedProperty ?? value);
                        }

                        objectWithDynamicProperties.DynamicPropertyValues = result;
                    }
                    else
                    {
                        objectWithDynamicProperties.DynamicPropertyValues = storedValues;
                    }
                }
            }
        }

        public void SaveDynamicPropertyValues(IHasDynamicProperties owner)
        {
            var objectsWithDynamicProperties = owner.GetFlatObjectsListWithInterface<IHasDynamicProperties>();

            foreach (var objectWithDynamicProperties in objectsWithDynamicProperties)
            {
                if (objectWithDynamicProperties.Id != null)
                {
                    var result = new List<DynamicPropertyObjectValue>();

                    if (objectWithDynamicProperties.DynamicPropertyValues != null)
                    {
                        var objectType = GetObjectTypeName(objectWithDynamicProperties);

                        foreach (var value in objectWithDynamicProperties.DynamicPropertyValues)
                        {
                            value.Property.ObjectType = objectType;
                            value.ObjectId = objectWithDynamicProperties.Id;
                            result.Add(value);
                        }
                    }

                    SaveObjectValues(result.ToArray());
                }
            }
        }

        public void DeleteDynamicPropertyValues(IHasDynamicProperties owner)
        {
            var objectsWithDynamicProperties = owner.GetFlatObjectsListWithInterface<IHasDynamicProperties>();

            foreach (var objectWithDynamicProperties in objectsWithDynamicProperties)
            {
                if (objectWithDynamicProperties.Id != null)
                {
                    DeleteObjectValues(GetObjectTypeName(objectWithDynamicProperties), objectWithDynamicProperties.Id);
                }
            }
        }

        #endregion


        private string GetObjectTypeName(object obj)
        {
            return GetObjectTypeName(obj.GetType());
        }
    }
}
