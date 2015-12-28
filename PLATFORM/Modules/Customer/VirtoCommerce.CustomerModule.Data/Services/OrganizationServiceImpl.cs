using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.CustomerModule.Data.Converters;
using VirtoCommerce.CustomerModule.Data.Repositories;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Data.Infrastructure;
using coreModel = VirtoCommerce.Domain.Customer.Model;

namespace VirtoCommerce.CustomerModule.Data.Services
{
    public class OrganizationServiceImpl : ServiceBase, IOrganizationService
    {
        private readonly Func<ICustomerRepository> _repositoryFactory;
        private readonly IDynamicPropertyService _dynamicPropertyService;

        public OrganizationServiceImpl(Func<ICustomerRepository> repositoryFactory, IDynamicPropertyService dynamicPropertyService)
        {
            _repositoryFactory = repositoryFactory;
            _dynamicPropertyService = dynamicPropertyService;
        }

        #region IContactService Members

        public coreModel.Organization GetById(string id)
        {
            coreModel.Organization retVal = null;

            using (var repository = _repositoryFactory())
            {
                var entity = repository.GetOrganizationById(id);
                if (entity != null)
                {
                    retVal = entity.ToCoreModel();
                }
            }

            if (retVal != null)
                _dynamicPropertyService.LoadDynamicPropertyValues(retVal);

            return retVal;
        }

        public coreModel.Organization Create(coreModel.Organization organization)
        {
            var pkMap = new PrimaryKeyResolvingMap();
            var entity = organization.ToDataModel(pkMap);

            using (var repository = _repositoryFactory())
            {
                repository.Add(entity);
                CommitChanges(repository);
                pkMap.ResolvePrimaryKeys();
            }

            _dynamicPropertyService.SaveDynamicPropertyValues(organization);

            var retVal = GetById(entity.Id);
            return retVal;
        }

        public void Update(coreModel.Organization[] organizations)
        {
            var pkMap = new PrimaryKeyResolvingMap();
            using (var repository = _repositoryFactory())
            using (var changeTracker = GetChangeTracker(repository))
            {
                foreach (var organization in organizations)
                {
                    var sourceEntity = organization.ToDataModel(pkMap);
                    var targetEntity = repository.GetOrganizationById(organization.Id);

                    if (targetEntity == null)
                    {
                        throw new NullReferenceException("targetEntity");
                    }

                    changeTracker.Attach(targetEntity);
                    sourceEntity.Patch(targetEntity);
                }

                CommitChanges(repository);
                pkMap.ResolvePrimaryKeys();
            }

            foreach (var organization in organizations)
            {
                _dynamicPropertyService.SaveDynamicPropertyValues(organization);
            }
        }

        public void Delete(string[] ids)
        {
            using (var repository = _repositoryFactory())
            {
                foreach (var id in ids)
                {
                    var organization = GetById(id);
                    _dynamicPropertyService.DeleteDynamicPropertyValues(organization);

                    var entity = repository.GetOrganizationById(id);
                    repository.Remove(entity);
                }
                CommitChanges(repository);
            }
        }

        public IEnumerable<coreModel.Organization> List()
        {
            using (var repository = _repositoryFactory())
            {
                var retVal = repository.Organizations
                    .ToList()
                    .Select(x => x.ToCoreModel())
                    .ToList();
                return retVal;
            }
        }

        #endregion
    }
}
