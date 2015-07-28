using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Domain.Commerce.Services;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.Domain.Shipping.Services;
using VirtoCommerce.Domain.Store.Services;
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

        public StoreServiceImpl(Func<IStoreRepository> repositoryFactory, ICommerceService commerceService, ISettingsManager settingManager, IDynamicPropertyService dynamicPropertyService, IShippingService shippingService, IPaymentMethodsService paymentService)
        {
            _repositoryFactory = repositoryFactory;
            _commerceService = commerceService;
            _settingManager = settingManager;
            _dynamicPropertyService = dynamicPropertyService;
            _shippingService = shippingService;
            _paymentService = paymentService;
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
                    retVal = entity.ToCoreModel(_shippingService.GetAllShippingMethods(), _paymentService.GetAllPaymentMethods());

                    var fulfillmentCenters = _commerceService.GetAllFulfillmentCenters().ToList();
                    retVal.ReturnsFulfillmentCenter = fulfillmentCenters.FirstOrDefault(x => x.Id == entity.ReturnsFulfillmentCenterId);
                    retVal.FulfillmentCenter = fulfillmentCenters.FirstOrDefault(x => x.Id == entity.FulfillmentCenterId);
                    retVal.SeoInfos = _commerceService.GetObjectsSeo(new[] { id }).ToList();

                    LoadObjectSettings(_settingManager, retVal);
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
            }

            //Need add seo separately
            if (store.SeoInfos != null)
            {
                foreach (var seoInfo in store.SeoInfos)
                {
                    seoInfo.ObjectId = dbStore.Id;
                    seoInfo.ObjectType = typeof(coreModel.Store).Name;
                    _commerceService.UpsertSeo(seoInfo);
                }
            }

            //Deep save settings
            SaveObjectSettings(_settingManager, store);

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

                    SaveObjectSettings(_settingManager, store);
                
                    //Patch SeoInfo  separately
                    if (store.SeoInfos != null)
                    {
                        foreach (var seoInfo in store.SeoInfos)
                        {
                            seoInfo.ObjectId = store.Id;
                            seoInfo.ObjectType = typeof(coreModel.Store).Name;
                        }

                        var seoInfos = new ObservableCollection<SeoInfo>(_commerceService.GetObjectsSeo(new[] { store.Id }));
                        seoInfos.ObserveCollection(x => _commerceService.UpsertSeo(x), x => _commerceService.DeleteSeo(new[] { x.Id }));
                        store.SeoInfos.Patch(seoInfos, (source, target) => _commerceService.UpsertSeo(source));
                    }
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
                    RemoveObjectSettings(_settingManager, store);

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
