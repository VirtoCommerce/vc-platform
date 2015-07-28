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
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.Platform.Data.DynamicProperties
{
    public class DynamicPropertyService : ServiceBase, IDynamicPropertyService
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
                .Where(t => t.IsClass && t.IsPublic && !t.IsAbstract && t.GetInterface(typeName) != null)
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
                var properties = repository.GetDynamicPropertiesForType(objectType);
                result.AddRange(properties.Select(p => p.ToModel()));
            }
            return result.ToArray();
        }

        public void SaveProperties(DynamicProperty[] properties)
        {
            if (properties == null)
                throw new ArgumentNullException("properties");

            using (var repository = _repositoryFactory())
            using (var changeTracker = GetChangeTracker(repository))
            {
                var sourceProperties = properties.Select(x => x.ToEntity()).ToList();
                var targetProperties = repository.GetDynamicPropertiesByIds(properties.Select(x => x.Id).ToArray()).ToList();
                sourceProperties.CompareTo(targetProperties, EqualityComparer<DynamicPropertyEntity>.Default, (state, source, target) =>
                    {
                        if (state == Core.Common.EntryState.Modified)
                        {
                            changeTracker.Attach(target);
                            source.Patch(target);
                        }
                        else if (state == Core.Common.EntryState.Added)
                        {
                            repository.Add(source);
                        }

                    });
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

            DynamicPropertyDictionaryItem[] result = null;

            using (var repository = _repositoryFactory())
            {
                var items = repository.GetDynamicPropertyDictionaryItems(propertyId);
                result = items.OrderBy(i => i.Name).Select(i => i.ToModel()).ToArray();
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
            using (var changeTracker = GetChangeTracker(repository))
            {
                var property = repository.GetDynamicPropertiesByIds(new string[] { propertyId }).First();
                var sourceDicItems = items.Select(x => x.ToEntity(property)).ToList();
                var targetDicItems = repository.GetDynamicPropertyDictionaryItems(propertyId).ToList();

                sourceDicItems.CompareTo(targetDicItems, EqualityComparer<DynamicPropertyDictionaryItemEntity>.Default, (state, source, target) =>
                {
                    if (state == Core.Common.EntryState.Modified)
                    {
                        changeTracker.Attach(target);
                        source.Patch(target);
                    }
                    else if (state == Core.Common.EntryState.Added)
                    {
                        repository.Add(source);
                    }

                });
                repository.UnitOfWork.Commit();
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

        public void LoadDynamicPropertyValues(IHasDynamicProperties owner)
        {
            var objectsWithDynamicProperties = owner.GetFlatObjectsListWithInterface<IHasDynamicProperties>();

            foreach (var objectWithDynamicProperties in objectsWithDynamicProperties)
            {
                if (objectWithDynamicProperties.Id != null)
                {
                    using (var repository = _repositoryFactory())
                    {
                        var objectType = GetObjectTypeName(objectWithDynamicProperties);
                        var properties = repository.GetObjectDynamicProperties(objectType, objectWithDynamicProperties.Id);

                        objectWithDynamicProperties.DynamicProperties = properties.Select(p => p.ToDynamicObjectProperty(objectWithDynamicProperties.Id)).ToList();
						//Set object type name
						objectWithDynamicProperties.ObjectType = objectType;
					}
                }
            }
        }

		public void SaveDynamicPropertyValues(IHasDynamicProperties owner)
		{
			if(owner == null)
			{
				throw new ArgumentNullException("owner");
			}
			//Because one DynamicPropertyEntity may update for multiple object in same time
			//need create fresh repository for each object to prevent collisions and overrides property values
			var objectsWithDynamicProperties = owner.GetFlatObjectsListWithInterface<IHasDynamicProperties>();
			foreach (var objectWithDynamicProperties in objectsWithDynamicProperties)
			{
				using (var repository = _repositoryFactory())
				using (var changeTracker = GetChangeTracker(repository))
				{
					if (objectWithDynamicProperties.Id != null)
					{
						var result = new List<DynamicPropertyObjectValue>();

						if (objectWithDynamicProperties.DynamicProperties != null && objectsWithDynamicProperties.Any())
						{
							var objectType = GetObjectTypeName(objectWithDynamicProperties);

							var target = new { Properties = new ObservableCollection<DynamicPropertyEntity>(repository.GetObjectDynamicProperties(objectType, objectWithDynamicProperties.Id)) };
							var source = new { Properties = new ObservableCollection<DynamicPropertyEntity>(objectWithDynamicProperties.DynamicProperties.Select(x => x.ToEntity())) };
						
							changeTracker.Attach(target);

							source.Properties.Patch(target.Properties, (sourcePopValue, targetPropValue) => sourcePopValue.Patch(targetPropValue));
						}
					}
					repository.UnitOfWork.Commit();
				}
			}
		}

        public void DeleteDynamicPropertyValues(IHasDynamicProperties owner)
        {
            var objectsWithDynamicProperties = owner.GetFlatObjectsListWithInterface<IHasDynamicProperties>();

            using (var repository = _repositoryFactory())
            {
                foreach (var objectWithDynamicProperties in objectsWithDynamicProperties.Where(x => x.Id != null))
                {
                    var objectType = GetObjectTypeName(objectWithDynamicProperties);
                    var objectId = objectWithDynamicProperties.Id;

                    var values = repository.DynamicPropertyObjectValues
                        .Where(v => v.ObjectType == objectType && v.ObjectId == objectId)
                        .ToList();

                    foreach (var value in values)
                    {
                        repository.Remove(value);
                    }
                }

                repository.UnitOfWork.Commit();
            }
        }

        #endregion


        private string GetObjectTypeName(object obj)
        {
            return GetObjectTypeName(obj.GetType());
        }
    }
}
