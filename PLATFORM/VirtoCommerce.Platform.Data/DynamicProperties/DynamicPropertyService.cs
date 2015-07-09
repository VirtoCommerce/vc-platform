using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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

        public string[] GetObjectTypes()
        {
            var typeName = typeof(IHasDynamicProperties).Name;

            var result = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.GetInterface(typeName) != null)
                .Select(t => t.FullName)
                .ToArray();

            return result;
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

                result.AddRange(properties.Select(p => p.ToModel(null)));
            }

            return result.ToArray();
        }

        public DynamicPropertyDictionaryItem[] GetDictionaryItems(string propertyId)
        {
            if (propertyId == null)
                throw new ArgumentNullException("propertyId");

            var result = new List<DynamicPropertyDictionaryItem>();

            using (var repository = _repositoryFactory())
            {
                var property = repository.DynamicProperties
                    .Include(p => p.DictionaryItems.Select(i => i.DictionaryValues))
                    .FirstOrDefault(p => p.IsDictionary && p.Id == propertyId);

                if (property != null)
                    result.AddRange(property.DictionaryItems.OrderBy(i => i.Name).Select(i => i.ToModel()));
            }

            return result.ToArray();
        }

        public void SaveProperties(DynamicProperty[] properties)
        {
            if (properties == null)
                throw new ArgumentNullException("properties");

            SaveProperties(properties, true);
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

        public DynamicProperty[] GetObjectValues(string objectType, string objectId)
        {
            if (objectType == null)
                throw new ArgumentNullException("objectType");
            if (objectId == null)
                throw new ArgumentNullException("objectId");

            var result = new List<DynamicProperty>();

            using (var repository = _repositoryFactory())
            {
                var properties = repository.DynamicProperties
                    .Include(p => p.DisplayNames)
                    .Where(p => p.ObjectType == objectType)
                    .OrderBy(p => p.Name)
                    .ToList();

                // This request will automatically fill values for loaded properties
                var values = repository.DynamicPropertyValues
                    .Where(v => v.ObjectType == objectType && v.ObjectId == objectId)
                    .ToList();

                result.AddRange(properties.Select(p => p.ToModel(objectId)));
            }

            return result.ToArray();
        }

        public void SaveObjectValues(DynamicProperty[] properties)
        {
            SaveProperties(properties, false);
        }

        public void DeleteObjectValues(string objectType, string objectId)
        {
            if (objectType == null)
                throw new ArgumentNullException("objectType");
            if (objectId == null)
                throw new ArgumentNullException("objectId");

            using (var repository = _repositoryFactory())
            {
                var values = repository.DynamicPropertyValues
                    .Where(v => v.ObjectType == objectType && v.ObjectId == objectId)
                    .ToList();

                foreach (var value in values)
                {
                    repository.Remove(value);
                }

                repository.UnitOfWork.Commit();
            }
        }

        #endregion


        private void SaveProperties(DynamicProperty[] properties, bool updateProperty)
        {
            if (properties != null && properties.Any())
            {
                using (var repository = _repositoryFactory())
                using (var changeTracker = new ObservableChangeTracker())
                {
                    var newProperties = properties.Where(p => string.IsNullOrEmpty(p.Id));
                    var newEntities = newProperties.Select(p => p.ToEntity(true));

                    foreach (var entity in newEntities)
                    {
                        repository.Add(entity);
                    }

                    var propertyIds = properties.Where(p => !string.IsNullOrEmpty(p.Id)).Select(p => p.Id).Distinct().ToArray();

                    var existingProperties = repository.DynamicProperties
                        .Include(p => p.DisplayNames)
                        .Include(p => p.DictionaryItems.Select(i => i.DictionaryValues))
                        .Where(p => propertyIds.Contains(p.Id))
                        .ToList();

                    if (existingProperties.Any())
                    {
                        propertyIds = existingProperties.Select(p => p.Id).ToArray();
                        properties = properties.Where(p => propertyIds.Contains(p.Id)).ToArray();

                        // This request will automatically fill values for loaded properties
                        var existingValues = repository.DynamicPropertyValues
                            .Where(v => propertyIds.Contains(v.PropertyId))
                            .ToList();

                        changeTracker.AddAction = x => repository.Add(x);
                        // Need for real remove object from nested collection (because EF default remove references only)
                        changeTracker.RemoveAction = x => repository.Remove(x);

                        var source = new { Properties = new ObservableCollection<DynamicPropertyEntity>(ToEntities(properties, updateProperty)) };
                        var target = new { Properties = new ObservableCollection<DynamicPropertyEntity>(existingProperties) };

                        changeTracker.Attach(target);
                        var propertyComparer = AnonymousComparer.Create((DynamicPropertyEntity p) => p.Id);
                        source.Properties.Patch(target.Properties, propertyComparer, (sourceProperty, targetProperty) => sourceProperty.Patch(targetProperty));
                    }

                    repository.UnitOfWork.Commit();
                }
            }
        }

        private static List<DynamicPropertyEntity> ToEntities(IEnumerable<DynamicProperty> properties, bool updateProperty)
        {
            var result = new List<DynamicPropertyEntity>();

            foreach (var property in properties)
            {
                var newEntity = property.ToEntity(updateProperty);
                var entity = result.FirstOrDefault(p => p.Id == property.Id);

                if (entity == null)
                {
                    result.Add(newEntity);
                }
                else
                {
                    foreach (var value in newEntity.ObjectValues)
                    {
                        entity.ObjectValues.Add(value);
                    }
                }
            }

            return result;
        }
    }
}
