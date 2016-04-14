using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VirtoCommerce.CoreModule.Data.Converters;
using VirtoCommerce.CoreModule.Data.Repositories;
using VirtoCommerce.Domain.Commerce.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;
using coreModel = VirtoCommerce.Domain.Commerce.Model;
using dataModel = VirtoCommerce.CoreModule.Data.Model;

namespace VirtoCommerce.CoreModule.Data.Services
{
    public class CommerceServiceImpl : ServiceBase, ICommerceService
    {
        private readonly Func<IСommerceRepository> _repositoryFactory;

        public CommerceServiceImpl(Func<IСommerceRepository> repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }
  
        #region ICommerceService Members     

        public IEnumerable<coreModel.FulfillmentCenter> GetAllFulfillmentCenters()
        {
            using (var repository = _repositoryFactory())
            {
                var result = repository.FulfillmentCenters
                    .ToArray()
                    .Select(x => x.ToCoreModel())
                    .ToList();

                return result;
            }
        }

        public coreModel.FulfillmentCenter UpsertFulfillmentCenter(coreModel.FulfillmentCenter center)
        {
            if (center == null)
                throw new ArgumentNullException("center");

            using (var repository = _repositoryFactory())
            {
                var sourceEntry = center.ToDataModel();
                var targetEntry = repository.FulfillmentCenters.FirstOrDefault(x => x.Id == center.Id);

                if (targetEntry == null)
                {
                    repository.Add(sourceEntry);
                }
                else
                {
                    sourceEntry.Patch(targetEntry);
                }

                CommitChanges(repository);

                var result = repository.FulfillmentCenters
                    .First(x => x.Id == sourceEntry.Id)
                    .ToCoreModel();

                return result;
            }

        }

        public void DeleteFulfillmentCenter(string[] ids)
        {
            using (var repository = _repositoryFactory())
            {
                foreach (var center in repository.FulfillmentCenters.Where(x => ids.Contains(x.Id)))
                {
                    repository.Remove(center);
                }

                CommitChanges(repository);
            }
        }

        public void LoadSeoForObjects(coreModel.ISeoSupport[] seoSupportObjects)
        {
            using (var repository = _repositoryFactory())
            {
                var objectIds = seoSupportObjects.Where(x => x.Id != null).Select(x => x.Id).Distinct().ToArray();

                var seoInfos = repository.SeoUrlKeywords
                    .Where(x => objectIds.Contains(x.ObjectId))
                    .ToArray()
                    .Select(x => x.ToCoreModel())
                    .ToList();

                foreach (var seoSupportObject in seoSupportObjects)
                {
                    seoSupportObject.SeoInfos = seoInfos.Where(x => x.ObjectId == seoSupportObject.Id && x.ObjectType == seoSupportObject.SeoObjectType).ToList();
                }
            }
        }

        public void UpsertSeoInfos(coreModel.SeoInfo[] seoinfos)
        {
            using (var repository = _repositoryFactory())
            using (var changeTracker = GetChangeTracker(repository))
            {
                var alreadyExistSeoInfos = repository.GetSeoByIds(seoinfos.Select(x => x.Id).ToArray());
                var target = new { SeoInfos = new ObservableCollection<dataModel.SeoUrlKeyword>(alreadyExistSeoInfos) };
                var source = new { SeoInfos = new ObservableCollection<dataModel.SeoUrlKeyword>(seoinfos.Select(x => x.ToDataModel())) };

                changeTracker.Attach(target);

                source.SeoInfos.Patch(target.SeoInfos, (sourceSeoUrlKeyword, targetSeoUrlKeyword) => sourceSeoUrlKeyword.Patch(targetSeoUrlKeyword));
                repository.UnitOfWork.Commit();
            }
        }

        public void UpsertSeoForObjects(coreModel.ISeoSupport[] seoSupportObjects)
        {
            if (seoSupportObjects == null)
            {
                throw new ArgumentNullException("seoSupportObjects");
            }

            foreach (var seoObject in seoSupportObjects.Where(x => x.Id != null))
            {
                var objectType = seoObject.SeoObjectType;

                using (var repository = _repositoryFactory())
                using (var changeTracker = GetChangeTracker(repository))
                {
                    if (seoObject.SeoInfos != null)
                    {
                        // Normalize seoInfo
                        foreach (var seoInfo in seoObject.SeoInfos)
                        {
                            if (seoInfo.ObjectId == null)
                                seoInfo.ObjectId = seoObject.Id;

                            if (seoInfo.ObjectType == null)
                                seoInfo.ObjectType = objectType;
                        }
                    }

                    if (seoObject.SeoInfos != null && seoObject.SeoInfos.Any())
                    {
                        var target = new { SeoInfos = new ObservableCollection<dataModel.SeoUrlKeyword>(repository.GetObjectSeoUrlKeywords(objectType, seoObject.Id)) };
                        var source = new { SeoInfos = new ObservableCollection<dataModel.SeoUrlKeyword>(seoObject.SeoInfos.Select(x => x.ToDataModel())) };

                        changeTracker.Attach(target);

                        source.SeoInfos.Patch(target.SeoInfos, new SeoUrlKeywordComparer(), (sourceSeoUrlKeyword, targetSeoUrlKeyword) => sourceSeoUrlKeyword.Patch(targetSeoUrlKeyword));
                    }

                    CommitChanges(repository);
                }
            }
        }

        public void DeleteSeoForObject(coreModel.ISeoSupport seoSupportObject)
        {
            if (seoSupportObject == null)
            {
                throw new ArgumentNullException("seoSupportObjects");
            }

            if (seoSupportObject.Id != null)
            {
                using (var repository = _repositoryFactory())
                {

                    var objectType = seoSupportObject.SeoObjectType;
                    var objectId = seoSupportObject.Id;
                    var seoUrlKeywords = repository.GetObjectSeoUrlKeywords(objectType, objectId);

                    foreach (var seoUrlKeyword in seoUrlKeywords)
                    {
                        repository.Remove(seoUrlKeyword);
                    }

                    CommitChanges(repository);
                }
            }
        }

        public IEnumerable<coreModel.SeoInfo> GetAllSeoDuplicates()
        {
            var retVal = new List<coreModel.SeoInfo>();
            using (var repository = _repositoryFactory())
            {
                var dublicateSeoRecords = repository.SeoUrlKeywords.GroupBy(x => x.Keyword + ":" + x.StoreId)
                                                    .Where(x => x.Count() > 1)
                                                    .SelectMany(x => x)
                                                    .ToArray();
                retVal.AddRange(dublicateSeoRecords.Select(x => x.ToCoreModel()));
            }
            return retVal;
        }


        public IEnumerable<coreModel.SeoInfo> GetSeoByKeyword(string keyword)
        {
            using (var repository = _repositoryFactory())
            {
                // Find seo entries for specified keyword. Also add other seo entries related to found object.
                var query = repository.SeoUrlKeywords
                    .Where(x => x.Keyword == keyword)
                    .Join(repository.SeoUrlKeywords, x => x.ObjectId, y => y.ObjectId, (x, y) => y)
                    .ToArray();

                var result = query.Select(x => x.ToCoreModel()).ToList();
                return result;
            }
        }

     

        public IEnumerable<coreModel.Currency> GetAllCurrencies()
        {
            using (var repository = _repositoryFactory())
            {
                var result = repository.Currencies
                    .OrderByDescending(x => x.IsPrimary)
                    .ThenBy(x => x.Code)
                    .ToArray()
                    .Select(x => x.ToCoreModel())
                    .ToList();

                return result;
            }
        }

        public void UpsertCurrencies(coreModel.Currency[] currencies)
        {
            if (currencies == null)
                throw new ArgumentNullException("currencies");

            using (var repository = _repositoryFactory())
            {
                //Ensure that only one Primary currency
                if (currencies.Any(x => x.IsPrimary))
                {
                    var oldPrimaryCurrency = repository.Currencies.FirstOrDefault(x => x.IsPrimary);

                    if (oldPrimaryCurrency != null)
                    {
                        oldPrimaryCurrency.IsPrimary = false;
                    }
                }

                foreach (var currency in currencies)
                {
                    var sourceEntry = currency.ToDataModel();
                    var targetEntry = repository.Currencies.FirstOrDefault(x => x.Code == currency.Code);

                    if (targetEntry == null)
                    {
                        repository.Add(sourceEntry);
                    }
                    else
                    {
                        sourceEntry.Patch(targetEntry);
                    }
                }

                CommitChanges(repository);
            }
        }

        public void DeleteCurrencies(string[] codes)
        {
            using (var repository = _repositoryFactory())
            {
                foreach (var currency in repository.Currencies.Where(x => codes.Contains(x.Code)))
                {
                    if (currency.IsPrimary)
                    {
                        throw new ArgumentException("Unable to delete primary currency");
                    }

                    repository.Remove(currency);
                }

                CommitChanges(repository);
            }
        }

        #endregion
    }
}
