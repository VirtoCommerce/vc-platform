using System;
using VirtoCommerce.CustomerModule.Data.Converters;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Data.Infrastructure;
using coreModel = VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.MarketingModule.Data.Services
{
    public class DynamicContentServiceImpl : ServiceBase, IDynamicContentService
    {
        private readonly Func<IMarketingRepository> _repositoryFactory;
        private readonly IDynamicPropertyService _dynamicPropertyService;

        public DynamicContentServiceImpl(Func<IMarketingRepository> repositoryFactory, IDynamicPropertyService dynamicPropertyService)
        {
            _repositoryFactory = repositoryFactory;
            _dynamicPropertyService = dynamicPropertyService;
        }

        #region IDynamicContentService Members

        public coreModel.DynamicContentItem GetContentItemById(string id)
        {
            coreModel.DynamicContentItem retVal = null;
            using (var repository = _repositoryFactory())
            {
                var entity = repository.GetContentItemById(id);
                if (entity != null)
                {
                    retVal = entity.ToCoreModel();
                }
            }

            if (retVal != null)
            {
                _dynamicPropertyService.LoadDynamicPropertyValues(retVal);
            }

            return retVal;
        }


        public coreModel.DynamicContentItem CreateContent(coreModel.DynamicContentItem content)
        {
            var pkMap = new PrimaryKeyResolvingMap();
            var entity = content.ToDataModel(pkMap);

            using (var repository = _repositoryFactory())
            {
                repository.Add(entity);
                CommitChanges(repository);
                pkMap.ResolvePrimaryKeys();
            }

            _dynamicPropertyService.SaveDynamicPropertyValues(content);

            var retVal = GetContentItemById(entity.Id);
            return retVal;
        }

        public void UpdateContents(coreModel.DynamicContentItem[] contents)
        {
            var pkMap = new PrimaryKeyResolvingMap();
            using (var repository = _repositoryFactory())
            using (var changeTracker = GetChangeTracker(repository))
            {
                foreach (var content in contents)
                {
                    var sourceEntity = content.ToDataModel(pkMap);
                    var targetEntity = repository.GetContentItemById(content.Id);
                    if (targetEntity == null)
                    {
                        repository.Add(sourceEntity);
                    }
                    else
                    {
                        changeTracker.Attach(targetEntity);
                        sourceEntity.Patch(targetEntity);
                    }
                }
                CommitChanges(repository);
                pkMap.ResolvePrimaryKeys();
            }
            foreach (var content in contents)
            {
                _dynamicPropertyService.SaveDynamicPropertyValues(content);
            }
        }

        public void DeleteContents(string[] ids)
        {
            using (var repository = _repositoryFactory())
            {
                foreach (var id in ids)
                {
                    var content = GetContentItemById(id);
                    _dynamicPropertyService.DeleteDynamicPropertyValues(content);

                    var entity = repository.GetContentItemById(id);
                    repository.Remove(entity);
                }
                CommitChanges(repository);
            }
        }

        public coreModel.DynamicContentPlace GetPlaceById(string id)
        {
            coreModel.DynamicContentPlace retVal = null;
            using (var repository = _repositoryFactory())
            {
                var entity = repository.GetContentPlaceById(id);
                if (entity != null)
                {
                    retVal = entity.ToCoreModel();
                }
            }
            return retVal;
        }

        public coreModel.DynamicContentPlace CreatePlace(coreModel.DynamicContentPlace place)
        {
            var entity = place.ToDataModel();

            using (var repository = _repositoryFactory())
            {
                repository.Add(entity);
                CommitChanges(repository);
            }

            var retVal = GetPlaceById(entity.Id);
            return retVal;
        }

        public void UpdatePlace(coreModel.DynamicContentPlace place)
        {
            using (var repository = _repositoryFactory())
            using (var changeTracker = GetChangeTracker(repository))
            {
                var sourceEntity = place.ToDataModel();
                var targetEntity = repository.GetContentPlaceById(place.Id);
                if (targetEntity == null)
                {
                    repository.Add(sourceEntity);
                }
                else
                {
                    changeTracker.Attach(targetEntity);
                    sourceEntity.Patch(targetEntity);
                }
                CommitChanges(repository);
            }
        }

        public void DeletePlaces(string[] ids)
        {
            using (var repository = _repositoryFactory())
            {
                foreach (var id in ids)
                {
                    var entity = repository.GetContentPlaceById(id);
                    repository.Remove(entity);
                }
                CommitChanges(repository);
            }
        }

        public coreModel.DynamicContentPublication GetPublicationById(string id)
        {
            coreModel.DynamicContentPublication retVal = null;
            using (var repository = _repositoryFactory())
            {
                var entity = repository.GetContentPublicationById(id);
                if (entity != null)
                {
                    retVal = entity.ToCoreModel();
                }
            }
            return retVal;
        }

        public coreModel.DynamicContentPublication CreatePublication(coreModel.DynamicContentPublication publication)
        {
            var entity = publication.ToDataModel();

            using (var repository = _repositoryFactory())
            {
                repository.Add(entity);
                CommitChanges(repository);
            }

            var retVal = GetPublicationById(entity.Id);
            return retVal;
        }

        public void UpdatePublications(coreModel.DynamicContentPublication[] publications)
        {
            using (var repository = _repositoryFactory())
            using (var changeTracker = GetChangeTracker(repository))
            {
                foreach (var content in publications)
                {
                    var sourceEntity = content.ToDataModel();
                    var targetEntity = repository.GetContentPublicationById(content.Id);
                    if (targetEntity == null)
                    {
                        repository.Add(sourceEntity);
                    }
                    else
                    {
                        changeTracker.Attach(targetEntity);
                        sourceEntity.Patch(targetEntity);
                    }
                }
                CommitChanges(repository);
            }
        }

        public void DeletePublications(string[] ids)
        {
            using (var repository = _repositoryFactory())
            {
                foreach (var id in ids)
                {
                    var entity = repository.GetContentPublicationById(id);
                    repository.Remove(entity);
                }
                CommitChanges(repository);
            }
        }


        public coreModel.DynamicContentFolder GetFolderById(string id)
        {
            coreModel.DynamicContentFolder retVal = null;
            using (var repository = _repositoryFactory())
            {
                var entity = repository.GetContentFolderById(id);
                if (entity != null)
                {
                    retVal = entity.ToCoreModel();
                }
            }
            return retVal;
        }

        public coreModel.DynamicContentFolder CreateFolder(coreModel.DynamicContentFolder folder)
        {
            var entity = folder.ToDataModel();

            using (var repository = _repositoryFactory())
            {
                repository.Add(entity);
                CommitChanges(repository);
            }

            var retVal = GetFolderById(entity.Id);
            return retVal;
        }

        public void UpdateFolder(coreModel.DynamicContentFolder folder)
        {
            using (var repository = _repositoryFactory())
            using (var changeTracker = GetChangeTracker(repository))
            {
                var sourceEntity = folder.ToDataModel();
                var targetEntity = repository.GetContentFolderById(folder.Id);
                if (targetEntity == null)
                {
                    repository.Add(sourceEntity);
                }
                else
                {
                    changeTracker.Attach(targetEntity);
                    sourceEntity.Patch(targetEntity);
                }
                CommitChanges(repository);
            }
        }

        public void DeleteFolder(string[] ids)
        {
            using (var repository = _repositoryFactory())
            {
                foreach (var id in ids)
                {
                    var entity = repository.GetContentFolderById(id);
                    repository.Remove(entity);
                }
                CommitChanges(repository);
            }
        }

        #endregion
    }
}
