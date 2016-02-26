using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CacheManager.Core;
using Common.Logging;
using VirtoCommerce.CoreModule.Data.Common;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Pricing.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.PricingModule.Data.Converters;
using VirtoCommerce.PricingModule.Data.Repositories;
using coreModel = VirtoCommerce.Domain.Pricing.Model;
using dataModel = VirtoCommerce.PricingModule.Data.Model;

namespace VirtoCommerce.PricingModule.Data.Services
{
    public class PricingServiceImpl : ServiceBase, IPricingService
    {
        private readonly Func<IPricingRepository> _repositoryFactory;
        private readonly IItemService _productService;
        private readonly ILog _logger;
        private readonly ICacheManager<object> _cacheManager;
        public PricingServiceImpl(Func<IPricingRepository> repositoryFactory, IItemService productService, ILog logger, ICacheManager<object> cacheManager)
        {
            _repositoryFactory = repositoryFactory;
            _productService = productService;
            _logger = logger;
            _cacheManager = cacheManager;
        }

        #region IPricingService Members
        /// <summary>
        /// Evaluate pricelists for special context. All resulting pricelists ordered by priority
        /// </summary>
        /// <param name="evalContext"></param>
        /// <returns></returns>
        public IEnumerable<coreModel.Pricelist> EvaluatePriceLists(coreModel.PriceEvaluationContext evalContext)
        {
            var retVal = new List<coreModel.Pricelist>();

            var query = _cacheManager.Get("PricingServiceImpl.EvaluatePriceLists", "PricingModuleRegion", () =>
                 {
                     using (var repository = _repositoryFactory())
                     {
                         var allAssignments = repository.PricelistAssignments.Include(x => x.Pricelist).ToArray().Select(x => x.ToCoreModel()).ToArray();
                         foreach (var assignment in allAssignments)
                         {
                             try
                             {
                                 //Deserialize conditions
                                 assignment.Condition = SerializationUtil.DeserializeExpression<Func<IEvaluationContext, bool>>(assignment.ConditionExpression);
                             }
                             catch (Exception ex)
                             {
                                 _logger.Error(ex);
                             }
                         }
                         return allAssignments;
                     }
                 }).AsQueryable();

            if (evalContext.CatalogId != null)
            {
                //filter by catalog
                query = query.Where(x => x.CatalogId == evalContext.CatalogId);
            }

            if (evalContext.Currency != null)
            {
                //filter by currency
                query = query.Where(x => x.Pricelist.Currency == evalContext.Currency.ToString());
            }
            if (evalContext.CertainDate != null)
            {
                //filter by date expiration
                query = query.Where(x => (x.StartDate == null || evalContext.CertainDate >= x.StartDate) && (x.EndDate == null || x.EndDate >= evalContext.CertainDate));
            }
            var assinments = query.OrderByDescending(x => x.Priority).ThenByDescending(x => x.Name).ToArray();
            retVal.AddRange(assinments.Where(x => x.Condition == null).Select(x => x.Pricelist));

            foreach (var assignment in assinments.Where(x => x.Condition != null))
            {
                try
                {
                    if (assignment.Condition(evalContext))
                    {
                        if (!retVal.Any(p => p.Id == assignment.Pricelist.Id))
                        {
                            retVal.Add(assignment.Pricelist);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
            }

            return retVal;
        }

        public IEnumerable<coreModel.Price> EvaluateProductPrices(coreModel.PriceEvaluationContext evalContext)
        {
            if (evalContext == null)
            {
                throw new ArgumentNullException("evalContext");
            }
            if (evalContext.ProductIds == null)
            {
                throw new MissingFieldException("ProductIds");
            }

            var retVal = new List<coreModel.Price>();

            using (var repository = _repositoryFactory())
            {
                //Get a price range satisfying by passing context
                var query = repository.Prices.Include(x => x.Pricelist)
                                             .Where(x => evalContext.ProductIds.Contains(x.ProductId))
                                             .Where(x => evalContext.Quantity >= x.MinQuantity || evalContext.Quantity == 0);

                if (evalContext.PricelistIds.IsNullOrEmpty())
                {
                    evalContext.PricelistIds = EvaluatePriceLists(evalContext).Select(x => x.Id).ToArray();
                }

                query = query.Where(x => evalContext.PricelistIds.Contains(x.PricelistId));

                var prices = query.ToArray().Select(x => x.ToCoreModel());

                foreach (var currencyPricesGroup in prices.GroupBy(x => x.Currency))
                {
                    var groupPrices = currencyPricesGroup.OrderBy(x => 1);
                    if (evalContext.PricelistIds != null)
                    {
                        //Construct ordered groups of list prices (ordered by pricelist priority taken from pricelistid array as index)
                        groupPrices = groupPrices.OrderBy(x => Array.IndexOf(evalContext.PricelistIds, x.PricelistId));
                    }
                    //Order by  price value
                    var orderedPrices = groupPrices.ThenBy(x => Math.Min(x.Sale.HasValue ? x.Sale.Value : x.List, x.List));
                    retVal.AddRange(orderedPrices);
                }

                if (_productService != null)
                {
                    //Variation price inheritance
                    //Need find products without price it may be a variation without implicitly price defined and try to get price from main product
                    var productIdsWithoutPrice = evalContext.ProductIds.Except(retVal.Select(x => x.ProductId).Distinct()).ToArray();
                    if (productIdsWithoutPrice.Any())
                    {
                        var variations = _productService.GetByIds(productIdsWithoutPrice, Domain.Catalog.Model.ItemResponseGroup.ItemInfo).Where(x => x.MainProductId != null);
                        evalContext.ProductIds = variations.Select(x => x.MainProductId).Distinct().ToArray();
                        foreach (var inheritedPrice in EvaluateProductPrices(evalContext))
                        {
                            foreach (var variation in variations.Where(x => x.MainProductId == inheritedPrice.ProductId))
                            {
                                var variationPrice = inheritedPrice.Clone() as coreModel.Price;
                                //For correct override price in possible update 
                                variationPrice.Id = null;
                                variationPrice.ProductId = variation.Id;
                                retVal.Add(variationPrice);
                            }
                        }
                    }
                }
            }

            return retVal;
        }

        public virtual coreModel.Price GetPriceById(string id)
        {
            coreModel.Price retVal = null;

            using (var repository = _repositoryFactory())
            {
                var entity = repository.GetPriceById(id);
                if (entity != null)
                {
                    retVal = entity.ToCoreModel();
                }
            }

            return retVal;
        }

        public virtual IEnumerable<coreModel.Price> GetPricesById(IEnumerable<string> ids)
        {
            coreModel.Price[] result = null;

            if (ids != null)
            {
                using (var repository = _repositoryFactory())
                {
                    var entities = repository.Prices
                        .Where(p => ids.Contains(p.Id))
                        .ToList();

                    result = entities
                        .Select(entity => entity.ToCoreModel())
                        .ToArray();
                }
            }

            return result;
        }


        public IEnumerable<coreModel.Pricelist> GetPriceLists()
        {
            List<coreModel.Pricelist> retVal;

            using (var repository = _repositoryFactory())
            {
                retVal = repository.Pricelists.ToArray().Select(x => x.ToCoreModel()).ToList();
            }

            return retVal;
        }

        public virtual coreModel.Pricelist GetPricelistById(string id)
        {
            coreModel.Pricelist retVal = null;

            using (var repository = _repositoryFactory())
            {
                var entity = repository.GetPricelistById(id);

                if (entity != null)
                {
                    retVal = entity.ToCoreModel();

                    var assignments = repository.GetAllPricelistAssignments(id);
                    retVal.Assignments = assignments.Select(x => x.ToCoreModel()).ToList();
                }
            }

            return retVal;
        }

        public virtual coreModel.Price CreatePrice(coreModel.Price price)
        {
            var entity = price.ToDataModel();

            using (var repository = _repositoryFactory())
            {
                //Need assign price to default pricelist with same currency or create it if not exist
                if (price.PricelistId == null)
                {
                    var defaultPriceListId = GetDefaultPriceListName(price.Currency);
                    var dbDefaultPriceList = repository.GetPricelistById(defaultPriceListId);

                    if (dbDefaultPriceList == null)
                    {
                        var defaultPriceList = new coreModel.Pricelist
                        {
                            Id = defaultPriceListId,
                            Currency = price.Currency,
                            Name = defaultPriceListId,
                            Description = defaultPriceListId
                        };
                        dbDefaultPriceList = defaultPriceList.ToDataModel();
                    }
                    entity.PricelistId = dbDefaultPriceList.Id;
                    entity.Pricelist = dbDefaultPriceList;

                    repository.Add(entity);

                    CommitChanges(repository);
                    //Automatically create catalog assignment 
                    TryToCreateCatalogAssignment(entity, repository);
                    ResetCache();
                }

            }
            price.Id = entity.Id;
            var retVal = GetPriceById(entity.Id);
            return retVal;
        }

        public virtual coreModel.Pricelist CreatePricelist(coreModel.Pricelist priceList)
        {
            var entity = priceList.ToDataModel();

            using (var repository = _repositoryFactory())
            {
                repository.Add(entity);
                CommitChanges(repository);
                ResetCache();
            }

            return GetPricelistById(entity.Id);
        }

        public virtual void UpdatePrices(coreModel.Price[] prices)
        {
            using (var repository = _repositoryFactory())
            using (var changeTracker = GetChangeTracker(repository))
            {
                foreach (var price in prices)
                {
                    var sourceEntity = price.ToDataModel();
                    var targetEntity = repository.GetPriceById(price.Id);

                    if (targetEntity == null)
                    {
                        throw new NullReferenceException("targetEntity");
                    }

                    changeTracker.Attach(targetEntity);
                    sourceEntity.Patch(targetEntity);
                }

                CommitChanges(repository);
                ResetCache();
            }
        }

        public virtual void UpdatePricelists(coreModel.Pricelist[] priceLists)
        {
            using (var repository = _repositoryFactory())
            using (var changeTracker = GetChangeTracker(repository))
            {
                changeTracker.AddAction = (x) =>
                {
                    repository.Add(x);
                    if (x is dataModel.Price)
                    {
                        TryToCreateCatalogAssignment((dataModel.Price)x, repository);
                    }
                };

                foreach (var priceList in priceLists)
                {
                    var sourceEntity = priceList.ToDataModel();
                    var targetEntity = repository.GetPricelistById(priceList.Id);
                    if (targetEntity == null)
                    {
                        throw new NullReferenceException("targetEntity");
                    }

                    changeTracker.Attach(targetEntity);
                    sourceEntity.Patch(targetEntity);
                }

                CommitChanges(repository);
                ResetCache();
            }
        }

        public virtual void DeletePrices(string[] ids)
        {
            GenericDelete(ids, (repository, id) => repository.GetPriceById(id));
        }
        public virtual void DeletePricelists(string[] ids)
        {
            GenericDelete(ids, (repository, id) => repository.GetPricelistById(id));
        }



        public coreModel.PricelistAssignment GetPricelistAssignmentById(string id)
        {
            dataModel.PricelistAssignment retVal;

            using (var repository = _repositoryFactory())
            {
                retVal = repository.GetPricelistAssignmentById(id);
            }

            return retVal != null ? retVal.ToCoreModel() : null;
        }

        public IEnumerable<coreModel.PricelistAssignment> GetPriceListAssignments()
        {
            List<coreModel.PricelistAssignment> retVal;

            using (var repository = _repositoryFactory())
            {
                retVal = repository.PricelistAssignments.ToArray().Select(x => x.ToCoreModel()).ToList();
            }

            return retVal;
        }

        public coreModel.PricelistAssignment CreatePriceListAssignment(coreModel.PricelistAssignment assignment)
        {
            var entity = assignment.ToDataModel();

            using (var repository = _repositoryFactory())
            {
                repository.Add(entity);
                CommitChanges(repository);
                ResetCache();
            }

            var retVal = GetPricelistAssignmentById(entity.Id);
            return retVal;
        }

        public void UpdatePricelistAssignments(coreModel.PricelistAssignment[] assignments)
        {
            using (var repository = _repositoryFactory())
            using (var changeTracker = GetChangeTracker(repository))
            {
                foreach (var assignment in assignments)
                {
                    var sourceEntity = assignment.ToDataModel();
                    var targetEntity = repository.GetPricelistAssignmentById(assignment.Id);

                    if (targetEntity == null)
                    {
                        throw new NullReferenceException("targetEntity");
                    }

                    changeTracker.Attach(targetEntity);
                    sourceEntity.Patch(targetEntity);
                }

                CommitChanges(repository);
                ResetCache();
            }
        }

        public void DeletePricelistsAssignments(string[] ids)
        {
            GenericDelete(ids, (repository, id) => repository.GetPricelistAssignmentById(id));
        }

        #endregion

        private void TryToCreateCatalogAssignment(dataModel.Price price, IPricingRepository repository)
        {
            //need create price list assignment to catalog if it not exist
            var product = _productService.GetById(price.ProductId, Domain.Catalog.Model.ItemResponseGroup.ItemInfo);
            if (!repository.PricelistAssignments.Where(x => x.PricelistId == price.PricelistId && x.CatalogId == product.CatalogId).Any())
            {
                var assignment = new coreModel.PricelistAssignment
                {
                    CatalogId = product.CatalogId,
                    Name = product.Catalog.Name + "-" + price.Pricelist.Name,
                    PricelistId = price.Pricelist.Id
                };
                CreatePriceListAssignment(assignment);
            }
        }
        private static string GetDefaultPriceListName(string currency)
        {
            var retVal = "Default" + currency.ToString();
            return retVal;
        }

        private void GenericDelete(string[] ids, Func<IPricingRepository, string, Entity> getter)
        {
            using (var repository = _repositoryFactory())
            {
                foreach (var id in ids)
                {
                    var entity = getter(repository, id);
                    repository.Remove(entity);
                }
                CommitChanges(repository);
                ResetCache();
            }
        }

        private void ResetCache()
        {
            //Clear cache (Smart cache implementation) 
            _cacheManager.ClearRegion("PricingModuleRegion");
        }

    }


}
