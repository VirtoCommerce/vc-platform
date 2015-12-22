using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Domain.Commerce.Services;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.Domain.Shipping.Services;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Domain.Tax.Services;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.StoreModule.Data.Converters;
using VirtoCommerce.StoreModule.Data.Repositories;
using coreModel = VirtoCommerce.Domain.Store.Model;

namespace VirtoCommerce.StoreModule.Data.Services
{
    public class StoreServiceImpl : ServiceBase, IStoreService
    {
        private readonly Func<IStoreRepository> _repositoryFactory;
        private readonly ICommerceService _commerceService;
        private readonly ISettingsManager _settingManager;
        private readonly IDynamicPropertyService _dynamicPropertyService;
        private readonly IShippingService _shippingService;
        private readonly IPaymentMethodsService _paymentService;
        private readonly ITaxService _taxService;

		public StoreServiceImpl(Func<IStoreRepository> repositoryFactory, ICommerceService commerceService, ISettingsManager settingManager, 
							    IDynamicPropertyService dynamicPropertyService, IShippingService shippingService, IPaymentMethodsService paymentService, ITaxService taxService)
        {
            _repositoryFactory = repositoryFactory;
            _commerceService = commerceService;
            _settingManager = settingManager;
            _dynamicPropertyService = dynamicPropertyService;
            _shippingService = shippingService;
            _paymentService = paymentService;
            _taxService = taxService;
        }

        #region IStoreService Members

        public coreModel.Store GetById(string id)
        {

            coreModel.Store retVal = null;
            using (var repository = _repositoryFactory())
            {
                var entity = repository.GetStoreById(id);

                if (entity != null)
                {
                    //Load original typed shipping method and populate it  personalized information from db
                    retVal = entity.ToCoreModel(_shippingService.GetAllShippingMethods(), _paymentService.GetAllPaymentMethods(), _taxService.GetAllTaxProviders());

                    var fulfillmentCenters = _commerceService.GetAllFulfillmentCenters().ToList();
                    retVal.ReturnsFulfillmentCenter = fulfillmentCenters.FirstOrDefault(x => x.Id == entity.ReturnsFulfillmentCenterId);
                    retVal.FulfillmentCenter = fulfillmentCenters.FirstOrDefault(x => x.Id == entity.FulfillmentCenterId);

                    _commerceService.LoadSeoForObjects(new[] { retVal });
                    _settingManager.LoadEntitySettingsValues(retVal);
                    _dynamicPropertyService.LoadDynamicPropertyValues(retVal);
                }
            }
            return retVal;

        }

        public coreModel.Store Create(coreModel.Store store)
        {
		    var dbStore = store.ToDataModel();

            using (var repository = _repositoryFactory())
            {
                repository.Add(dbStore);
                CommitChanges(repository);
                store.Id = dbStore.Id;
            }

            //Need add seo separately
            _commerceService.UpsertSeoForObjects(new[] { store });

            //Deep save properties
            _dynamicPropertyService.SaveDynamicPropertyValues(store);
            //Deep save settings
            _settingManager.SaveEntitySettingsValues(store);

            var retVal = GetById(store.Id);
            return retVal;
        }

        public void Update(coreModel.Store[] stores)
        {
            using (var repository = _repositoryFactory())
            using (var changeTracker = base.GetChangeTracker(repository))
            {
                foreach (var store in stores)
                {
                    var sourceEntity = store.ToDataModel();
                    var targetEntity = repository.GetStoreById(store.Id);

                    if (targetEntity == null)
                    {
                        throw new NullReferenceException("targetEntity");
                    }

                    changeTracker.Attach(targetEntity);
                    sourceEntity.Patch(targetEntity);

                    _dynamicPropertyService.SaveDynamicPropertyValues(store);
                    //Deep save settings
                    _settingManager.SaveEntitySettingsValues(store);

                    //Patch SeoInfo  separately
                    _commerceService.UpsertSeoForObjects(stores);

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
                    var store = GetById(id);
                    _commerceService.DeleteSeoForObject(store);
                    _dynamicPropertyService.DeleteDynamicPropertyValues(store);
                    //Deep remove settings
                    _settingManager.RemoveEntitySettings(store);

                    var entity = repository.GetStoreById(id);
                    repository.Remove(entity);
                }

                CommitChanges(repository);
            }
        }

		public IEnumerable<coreModel.Store> GetStoreList()
		{
			var retVal = new List<coreModel.Store>();
			using (var repository = _repositoryFactory())
			{
				foreach (var storeId in repository.Stores.Select(x => x.Id).ToArray())
				{
					var store = GetById(storeId);
					retVal.Add(store);
				}
			}
			return retVal;
		}

        #endregion
    }
}
