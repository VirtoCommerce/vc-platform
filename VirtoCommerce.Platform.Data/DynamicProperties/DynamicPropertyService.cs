using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Repositories;

namespace VirtoCommerce.Platform.Data.DynamicProperties
{
    public class DynamicPropertyService : ServiceBase, IDynamicPropertyService
    {
        private List<string> _availableTypeNames = new List<string>();
        private readonly Func<IPlatformRepository> _repositoryFactory;

        public DynamicPropertyService(Func<IPlatformRepository> repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            _availableTypeNames.AddRange(LoadTypesFromReflection());
        }

        private IEnumerable<string> LoadTypesFromReflection()
        {
            var typeName = typeof(IHasDynamicProperties).Name;
            return AppDomain.CurrentDomain.GetAssemblies()
                   .SelectMany(a => a.GetLoadableTypes())
                   .Where(t => t.IsClass && t.IsPublic && !t.IsAbstract && t.GetInterface(typeName) != null)
                   .Select(GetObjectTypeName);
        }

        #region IDynamicPropertyService Members

        public void RegisterType(string typeName)
        {
            if (!_availableTypeNames.Contains(typeName, StringComparer.OrdinalIgnoreCase))
            {
                _availableTypeNames.Add(typeName);
            }
        }

        public string[] GetAvailableObjectTypeNames()
        {
            return _availableTypeNames.ToArray();
        }

        public string GetObjectTypeName(Type type)
        {
            return type.FullName;
        }

        public DynamicProperty[] GetProperties(string objectType)
        {
            if (objectType == null)
            {
                throw new ArgumentNullException(nameof(objectType));
            }

            var result = new List<DynamicProperty>();

            using (var repository = _repositoryFactory())
            {
                var properties = repository.GetDynamicPropertiesForTypes(new[] { objectType });
                result.AddRange(properties.Select(x => x.ToModel(AbstractTypeFactory<DynamicProperty>.TryCreateInstance())));
            }
            return result.ToArray();
        }

        public DynamicProperty[] SaveProperties(DynamicProperty[] properties)
        {
            if (properties == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }
            var pkMap = new PrimaryKeyResolvingMap();
            using (var repository = _repositoryFactory())
            using (var changeTracker = GetChangeTracker(repository))
            {
                var dbExistProperties = repository.GetDynamicPropertiesForTypes(properties.Select(x => x.ObjectType).Distinct().ToArray()).ToList();
                foreach (var property in properties)
                {
                    var originalEntity = dbExistProperties.FirstOrDefault(x => property.IsTransient() ? x.Name.EqualsInvariant(property.Name) && x.ObjectType.EqualsInvariant(property.ObjectType) : x.Id.EqualsInvariant(property.Id));
                    var modifiedEntity = AbstractTypeFactory<DynamicPropertyEntity>.TryCreateInstance().FromModel(property, pkMap);
                    if (originalEntity != null)
                    {
                        changeTracker.Attach(originalEntity);
                        modifiedEntity.Patch(originalEntity);
                    }
                    else
                    {
                        repository.Add(modifiedEntity);
                    }
                }
                repository.UnitOfWork.Commit();
                pkMap.ResolvePrimaryKeys();
            }
            return properties;
        }

        public void DeleteProperties(string[] propertyIds)
        {
            if (propertyIds == null)
            {
                throw new ArgumentNullException(nameof(propertyIds));
            }

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
            {
                throw new ArgumentNullException(nameof(propertyId));
            }

            using (var repository = _repositoryFactory())
            {
                repository.DisableChangesTracking();

                var dbEntities = repository.GetDynamicPropertyDictionaryItems(propertyId);
                var result = dbEntities.OrderBy(x => x.Name).Select(x => x.ToModel(AbstractTypeFactory<DynamicPropertyDictionaryItem>.TryCreateInstance())).ToArray();
                return result;
            }
        }

        public void SaveDictionaryItems(string propertyId, DynamicPropertyDictionaryItem[] items)
        {
            if (propertyId == null)
            {
                throw new ArgumentNullException(nameof(propertyId));
            }
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            using (var repository = _repositoryFactory())
            using (var changeTracker = GetChangeTracker(repository))
            {
                var dbExistItems = repository.GetDynamicPropertyDictionaryItems(propertyId);
                foreach (var item in items)
                {
                    var originalEntity = dbExistItems.FirstOrDefault(x => x.Id == item.Id);
                    var modifiedEntity = AbstractTypeFactory<DynamicPropertyDictionaryItemEntity>.TryCreateInstance().FromModel(item);
                    modifiedEntity.PropertyId = propertyId;
                    if (originalEntity != null)
                    {
                        changeTracker.Attach(originalEntity);
                        modifiedEntity.Patch(originalEntity);
                    }
                    else
                    {
                        repository.Add(modifiedEntity);
                    }
                }
                repository.UnitOfWork.Commit();
            }

        }

        public void DeleteDictionaryItems(string[] itemIds)
        {
            if (itemIds == null)
            {
                throw new ArgumentNullException(nameof(itemIds));
            }

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
            LoadDynamicPropertyValues(new[] { owner });
        }

        public void LoadDynamicPropertyValues(params IHasDynamicProperties[] owners)
        {

            if (owners == null)
            {
                throw new ArgumentNullException(nameof(owners));
            }

            var propOwners = owners.SelectMany(x => x.GetFlatObjectsListWithInterface<IHasDynamicProperties>());
            using (var repository = _repositoryFactory())
            {
                //Optimize performance and CPU usage
                repository.DisableChangesTracking();

                var objectTypeNames = propOwners.Select(x => GetObjectTypeName(x)).Distinct().ToArray();
                var objectIds = propOwners.Select(x => x.Id).Distinct().ToArray();

                //Load properties belongs to given objects types
                var dynamicObjectProps = repository.GetObjectDynamicProperties(objectTypeNames, objectIds)
                                                   .Select(x => x.ToModel(AbstractTypeFactory<DynamicObjectProperty>.TryCreateInstance()))
                                                   .OfType<DynamicObjectProperty>();
                foreach (var propOwner in propOwners)
                {
                    var objectType = GetObjectTypeName(propOwner);
                    propOwner.ObjectType = objectType;
                    //Filter only properties with belongs to concrete type
                    propOwner.DynamicProperties = dynamicObjectProps.Where(x => x.ObjectType.EqualsInvariant(objectType))
                                                                    .Select(x => x.Clone())
                                                                    .OfType<DynamicObjectProperty>()
                                                                    .ToList();

                    foreach (var prop in propOwner.DynamicProperties)
                    {
                        prop.ObjectId = propOwner.Id;
                        //Leave only self object values 
                        if (prop.Values != null)
                        {
                            prop.Values = prop.Values.Where(x => x.ObjectId.EqualsInvariant(propOwner.Id)).ToList();
                        }
                    }
                }
            }
        }

        public void SaveDynamicPropertyValues(IHasDynamicProperties owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException(nameof(owner));
            }

            var objectsWithDynamicProperties = owner.GetFlatObjectsListWithInterface<IHasDynamicProperties>().Where(x => !string.IsNullOrEmpty(x.Id) && !x.DynamicProperties.IsNullOrEmpty());
            //Ensure what all properties have proper ObjectId and ObjectType properties set
            foreach (var obj in objectsWithDynamicProperties)
            {
                foreach (var prop in obj.DynamicProperties)
                {
                    prop.ObjectId = obj.Id;
                    prop.ObjectType = GetObjectTypeName(obj);
                }
            }
            var pkMap = new PrimaryKeyResolvingMap();
            using (var repository = _repositoryFactory())
            using (var changeTracker = GetChangeTracker(repository))
            {
                var objectTypes = objectsWithDynamicProperties.Select(x => GetObjectTypeName(x)).Distinct().ToArray();
                //Converting all incoming properties to db entity and group property values of all objects use for that   property.objectType and property.name as complex key 
                var modifiedPropertyEntitiesGroup = objectsWithDynamicProperties.SelectMany(x => x.DynamicProperties.Select(dp => AbstractTypeFactory<DynamicPropertyEntity>.TryCreateInstance().FromModel(dp, pkMap)))
                                                                          .GroupBy(x => $"{x.Name}:{x.ObjectType}");
                var originalPropertyEntitites = repository.GetObjectDynamicProperties(objectTypes, objectsWithDynamicProperties.Select(x => x.Id).Distinct().ToArray()).ToList();
                foreach (var modifiedPropertyEntityGroupItem in modifiedPropertyEntitiesGroup)
                {
                    var modifiedPropertyObjectValues = modifiedPropertyEntityGroupItem.SelectMany(x => x.ObjectValues)
                                                                                      .Where(x => x.GetValue(EnumUtility.SafeParse(x.ValueType, DynamicPropertyValueType.LongText)) != null)
                                                                                      .ToList();
                    //Try to find original property with same complex key
                    var originalEntity = originalPropertyEntitites.FirstOrDefault(x => $"{x.Name}:{x.ObjectType}".EqualsInvariant(modifiedPropertyEntityGroupItem.Key));
                    if (originalEntity != null)
                    {
                        changeTracker.Attach(originalEntity);
                        //Update only property values
                        var comparer = AnonymousComparer.Create((DynamicPropertyObjectValueEntity x) => $"{x.ObjectId}:{x.ObjectType}:{x.Locale}:{x.GetValue(EnumUtility.SafeParse(x.ValueType, DynamicPropertyValueType.LongText))}");
                        modifiedPropertyObjectValues.Patch(originalEntity.ObjectValues, comparer, (sourceValue, targetValue) => { });
                    }
                }

                repository.UnitOfWork.Commit();
                pkMap.ResolvePrimaryKeys();
            }

        }

        public void DeleteDynamicPropertyValues(IHasDynamicProperties owner)
        {
            var objectsWithDynamicProperties = owner.GetFlatObjectsListWithInterface<IHasDynamicProperties>();

            using (var repository = _repositoryFactory())
            {
                foreach (var objectHasDynamicProperties in objectsWithDynamicProperties.Where(x => x.Id != null))
                {
                    var objectType = GetObjectTypeName(objectHasDynamicProperties);
                    var objectId = objectHasDynamicProperties.Id;
                    var values = repository.DynamicPropertyObjectValues.Where(v => v.ObjectType == objectType && v.ObjectId == objectId)
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
