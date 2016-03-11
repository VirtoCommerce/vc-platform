using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CacheManager.Core;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Domain.Commerce.Services;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.Domain.Shipping.Services;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Domain.Tax.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Security;
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
        private readonly IShippingMethodsService _shippingService;
        private readonly IPaymentMethodsService _paymentService;
        private readonly ITaxService _taxService;
        private readonly ICacheManager<object> _cacheManager;

		public StoreServiceImpl(Func<IStoreRepository> repositoryFactory, ICommerceService commerceService, ISettingsManager settingManager, 
							    IDynamicPropertyService dynamicPropertyService, IShippingMethodsService shippingService, IPaymentMethodsService paymentService, 
                                ITaxService taxService, ICacheManager<object> cacheManager)
        {
            _repositoryFactory = repositoryFactory;
            _commerceService = commerceService;
            _settingManager = settingManager;
            _dynamicPropertyService = dynamicPropertyService;
            _shippingService = shippingService;
            _paymentService = paymentService;
            _taxService = taxService;
            _cacheManager = cacheManager;
        }

        #region IStoreService Members

        public coreModel.Store[] GetByIds(string[] ids)
        {
            var retVal = new List<coreModel.Store>();
            using (var repository = _repositoryFactory())
            {
                var fulfillmentCenters = _commerceService.GetAllFulfillmentCenters().ToList();
                var dbStores = repository.GetStoresByIds(ids);
                foreach(var dbStore in dbStores)
                {
                    //Load original typed shipping method and populate it  personalized information from db
                    var store = dbStore.ToCoreModel(_shippingService.GetAllShippingMethods(), _paymentService.GetAllPaymentMethods(), _taxService.GetAllTaxProviders());

                    store.ReturnsFulfillmentCenter = fulfillmentCenters.FirstOrDefault(x => x.Id == dbStore.ReturnsFulfillmentCenterId);
                    store.FulfillmentCenter = fulfillmentCenters.FirstOrDefault(x => x.Id == dbStore.FulfillmentCenterId);
                    //Set default settings for store it can be override by store instance setting in LoadEntitySettingsValues
                    store.Settings = _settingManager.GetModuleSettings("VirtoCommerce.Store");
                    _settingManager.LoadEntitySettingsValues(store);
                    _dynamicPropertyService.LoadDynamicPropertyValues(store);
                    retVal.Add(store);
                }
            }
            _commerceService.LoadSeoForObjects(retVal.ToArray());
            return retVal.ToArray();
        }

        public coreModel.Store GetById(string id)
        {
            return GetByIds(new[] { id }).FirstOrDefault();
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

            //Invalidate module cache region
            _cacheManager.ClearRegion("StoreModuleRegion");

            var retVal = GetById(store.Id);
            return retVal;
        }

        public void Update(coreModel.Store[] stores)
        {
            using (var repository = _repositoryFactory())
            using (var changeTracker = base.GetChangeTracker(repository))
            {
                var dbStores = repository.GetStoresByIds(stores.Select(x => x.Id).ToArray());
                foreach (var store in stores)
                {
                    var sourceEntity = store.ToDataModel();
                    var targetEntity = dbStores.First(x=>x.Id == store.Id);

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

                //Invalidate module cache region
                _cacheManager.ClearRegion("StoreModuleRegion");
            }
        }

        public void Delete(string[] ids)
        {
            using (var repository = _repositoryFactory())
            {
                var stores = GetByIds(ids);
                var dbStores = repository.GetStoresByIds(ids);

                foreach (var store in stores)
                {
                    _commerceService.DeleteSeoForObject(store);
                    _dynamicPropertyService.DeleteDynamicPropertyValues(store);
                    //Deep remove settings
                    _settingManager.RemoveEntitySettings(store);

                    var dbStore = dbStores.FirstOrDefault(x => x.Id == store.Id);
                    if (dbStore != null)
                    {
                        repository.Remove(dbStore);
                    }
                }
                CommitChanges(repository);
                //Invalidate module cache region
                _cacheManager.ClearRegion("StoreModuleRegion");
            }
        }

        public coreModel.SearchResult SearchStores(coreModel.SearchCriteria criteria)
        {
            var retVal = new coreModel.SearchResult();
            using (var repository = _repositoryFactory())
            {
                var query = repository.Stores;
                if(!string.IsNullOrEmpty(criteria.Keyword))
                {
                    query = query.Where(x => x.Name.Contains(criteria.Keyword) || x.Id.Contains(criteria.Keyword));
                }
                if(!criteria.StoreIds.IsNullOrEmpty())
                {
                    query = query.Where(x => criteria.StoreIds.Contains(x.Id));
                }
                var sortInfos = criteria.SortInfos;
                if (sortInfos.IsNullOrEmpty())
                {
                    sortInfos = new[] { new SortInfo { SortColumn = "Name" } };
                }
              
                query = query.OrderBySortInfos(sortInfos);

                retVal.TotalCount = query.Count();
                var storeIds = query.Skip(criteria.Skip)
                                 .Take(criteria.Take)
                                 .Select(x => x.Id)
                                 .ToArray();

                retVal.Stores = GetByIds(storeIds).AsQueryable().OrderBySortInfos(sortInfos).ToList(); 
            }
            return retVal;
        }


        /// <summary>
        /// Returns list of stores ids which passed user can signIn
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<string> GetUserAllowedStoreIds(ApplicationUserExtended user)
        {
            if(user == null)
            {
                throw new ArgumentNullException("user");
            }

            var retVal = new List<string>();

            if(user.StoreId != null)
            {
                var store = GetById(user.StoreId);
                if(store != null)
                {
                    retVal.Add(store.Id);
                    if(!store.TrustedGroups.IsNullOrEmpty())
                    {
                        retVal.AddRange(store.TrustedGroups);
                    }
                }
            }
            return retVal;
        }

        #endregion
    }
}
