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

        public DynamicProperty[] GetTypeProperties(string objectType)
        {
            if (objectType == null)
                throw new ArgumentNullException("objectType");

            var result = new List<DynamicProperty>();

            using (var repository = _repositoryFactory())
            {
                var properties = repository.DynamicProperties
                    .Include(p => p.Names)
                    .Where(p => p.ObjectType == objectType)
                    .OrderBy(p => p.Name)
                    .ToList();

                result.AddRange(properties.Select(p => p.ToModel(null)));
            }

            return result.ToArray();
        }

        public void SaveTypeProperties(DynamicProperty[] properties)
        {
            SaveProperties(properties, true, false);
        }

        public DynamicProperty[] GetObjectProperties(string objectType, string objectId)
        {
            if (objectType == null)
                throw new ArgumentNullException("objectType");
            if (objectId == null)
                throw new ArgumentNullException("objectId");

            var result = new List<DynamicProperty>();

            using (var repository = _repositoryFactory())
            {
                var properties = repository.DynamicProperties
                    .Include(p => p.Names)
                    .Where(p => p.ObjectType == objectType)
                    .OrderBy(p => p.Name)
                    .ToList();

                var valueKeys = properties.Select(p => string.Join("-", p.ObjectType, p.Name, objectId)).Distinct().ToArray();

                // This request will automatically fill values for loaded properties
                repository.DynamicPropertyValues
                    .Where(v => valueKeys.Contains(v.SearchKey))
                    .ToList();

                result.AddRange(properties.Select(p => p.ToModel(objectId)));
            }

            return result.ToArray();
        }

        public void SaveObjectProperties(DynamicProperty[] properties)
        {
            SaveProperties(properties, false, true);
        }

        private void SaveProperties(DynamicProperty[] properties, bool saveLocalizedNames, bool saveObjectValues)
        {
            if (properties != null && properties.Any())
            {
                var propertyKeys = properties.Select(p => string.Join("-", p.ObjectType, p.Name)).Distinct().ToArray();
                var valueKeys = properties.Select(p => string.Join("-", p.ObjectType, p.Name, saveObjectValues ? p.ObjectId : null)).Distinct().ToArray();

                using (var repository = _repositoryFactory())
                using (var changeTracker = new ObservableChangeTracker())
                {
                    var existingProperties = repository.DynamicProperties
                        .Include(p => p.Names)
                        .Where(p => propertyKeys.Contains(p.SearchKey))
                        .ToList();

                    // Properties must be created before saving object values
                    if (saveObjectValues)
                    {
                        propertyKeys = existingProperties.Select(p => string.Join("-", p.ObjectType, p.Name)).ToArray();
                        properties = properties.Where(p => propertyKeys.Contains(p.ObjectType + "-" + p.Name)).ToArray();
                    }

                    if (existingProperties.Any() || !saveObjectValues)
                    {
                        // This request will automatically fill values for loaded properties
                        repository.DynamicPropertyValues
                            .Where(v => valueKeys.Contains(v.SearchKey))
                            .ToList();

                        changeTracker.AddAction = x => repository.Add(x);
                        // Need for real remove object from nested collection (because EF default remove references only)
                        changeTracker.RemoveAction = x => repository.Remove(x);

                        var source = new { Properties = new ObservableCollection<DynamicPropertyEntity>(ToEntities(properties, saveLocalizedNames, saveObjectValues)) };
                        var target = new { Properties = new ObservableCollection<DynamicPropertyEntity>(existingProperties) };

                        changeTracker.Attach(target);
                        var propertyComparer = AnonymousComparer.Create((DynamicPropertyEntity p) => string.Join("-", p.ObjectType, p.Name));
                        source.Properties.Patch(target.Properties, propertyComparer, (sourceProperty, targetProperty) => sourceProperty.Patch(targetProperty));

                        repository.UnitOfWork.Commit();
                    }
                }
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

        private static List<DynamicPropertyEntity> ToEntities(IEnumerable<DynamicProperty> properties, bool saveLocalizedNames, bool saveObjectValues)
        {
            var result = new List<DynamicPropertyEntity>();

            foreach (var property in properties)
            {
                var entity = result.FirstOrDefault(p => p.Name == property.Name && p.ObjectType == property.ObjectType);
                var newEntity = property.ToEntity(saveLocalizedNames, saveObjectValues);

                if (entity == null)
                {
                    result.Add(newEntity);
                }
                else
                {
                    foreach (var value in newEntity.Values)
                    {
                        entity.Values.Add(value);
                    }
                }
            }

            return result;
        }
    }
}
