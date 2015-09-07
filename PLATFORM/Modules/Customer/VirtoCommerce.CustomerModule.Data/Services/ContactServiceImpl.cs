using System;
using VirtoCommerce.CustomerModule.Data.Converters;
using VirtoCommerce.CustomerModule.Data.Repositories;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Data.Infrastructure;
using coreModel = VirtoCommerce.Domain.Customer.Model;

namespace VirtoCommerce.CustomerModule.Data.Services
{
    public class ContactServiceImpl : ServiceBase, IContactService
    {
        private readonly Func<ICustomerRepository> _repositoryFactory;
        private readonly IDynamicPropertyService _dynamicPropertyService;

        public ContactServiceImpl(Func<ICustomerRepository> repositoryFactory, IDynamicPropertyService dynamicPropertyService)
        {
            _repositoryFactory = repositoryFactory;
            _dynamicPropertyService = dynamicPropertyService;
        }

        #region IContactService Members

        public coreModel.Contact GetById(string id)
        {
            coreModel.Contact retVal = null;

            using (var repository = _repositoryFactory())
            {
                var entity = repository.GetContactById(id);
                if (entity != null)
                {
                    retVal = entity.ToCoreModel();
                }
            }

            if (retVal != null)
                _dynamicPropertyService.LoadDynamicPropertyValues(retVal);

            return retVal;
        }

        public coreModel.Contact Create(coreModel.Contact contact)
        {
            var entity = contact.ToDataModel();

            using (var repository = _repositoryFactory())
            {
                repository.Add(entity);
                CommitChanges(repository);
            }

            contact.SetObjectId(entity.Id);
            _dynamicPropertyService.SaveDynamicPropertyValues(contact);

            var retVal = GetById(entity.Id);
            return retVal;
        }

        public void Update(coreModel.Contact[] contacts)
        {
            using (var repository = _repositoryFactory())
            using (var changeTracker = GetChangeTracker(repository))
            {
                foreach (var contact in contacts)
                {
                    var sourceEntity = contact.ToDataModel();
                    var targetEntity = repository.GetContactById(contact.Id);

                    if (targetEntity == null)
                    {
                        throw new NullReferenceException("targetEntity");
                    }

                    changeTracker.Attach(targetEntity);
                    sourceEntity.Patch(targetEntity);

                    _dynamicPropertyService.SaveDynamicPropertyValues(contact);
                }

                CommitChanges(repository);
            }
        }

        public void Delete(string[] ids)
        {
            using (var repository = _repositoryFactory())
            {
                foreach (var id in ids)
                {
                    var contact = GetById(id);
                    _dynamicPropertyService.DeleteDynamicPropertyValues(contact);

                    var entity = repository.GetContactById(id);
                    repository.Remove(entity);
                }

                CommitChanges(repository);
            }
        }
        #endregion

    }
}
