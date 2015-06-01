using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using VirtoCommerce.Domain.Pricing.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.PricingModule.Data.Converters;
using VirtoCommerce.PricingModule.Data.Repositories;
using coreModel = VirtoCommerce.Domain.Pricing.Model;
using foundationModel = VirtoCommerce.PricingModule.Data.Model;

namespace VirtoCommerce.PricingModule.Data.Services
{
    public class PricingServiceImpl : ServiceBase, IPricingService
    {
        private readonly Func<IPricingRepository> _repositoryFactory;
        public PricingServiceImpl(Func<IPricingRepository> repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        #region IPricingService Members

        public IEnumerable<coreModel.Pricelist> EvaluatePriceLists(coreModel.PriceEvaluationContext evalContext)
        {
            IEnumerable<coreModel.Pricelist> retVal;

            using (var repository = _repositoryFactory())
            {
                var query = repository.PricelistAssignments.Include(x => x.Pricelist);

                //filter by catalog
                query = query.Where(x => (x.CatalogId == evalContext.CatalogId));

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
                // sort content by type and priority
                retVal = query.OrderByDescending(x => x.Priority).ThenByDescending(x => x.Name)
                              .ToArray().Select(x => x.Pricelist.ToCoreModel());
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

                if (evalContext.PricelistIds != null)
                {
                    query = query.Where(x => evalContext.PricelistIds.Contains(x.PricelistId));
                }

				if(evalContext.Currency != null)
				{
					query = query.Where(x => x.Pricelist.Currency == evalContext.Currency.ToString());
				}
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
                    var orderedPrices = groupPrices.OrderBy(x => Math.Min(x.Sale.HasValue ? x.Sale.Value : x.List, x.List));
                    retVal.AddRange(orderedPrices);
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
                if (price.PricelistId == null)
                {
                    var defaultPriceListId = GetDefaultPriceListName(price.Currency);
                    var foundationDefaultPriceList = repository.GetPricelistById(defaultPriceListId);

                    if (foundationDefaultPriceList == null)
                    {
                        var defaultPriceList = new coreModel.Pricelist
                        {
                            Id = defaultPriceListId,
                            Currency = price.Currency,
                            Name = defaultPriceListId,
                            Description = defaultPriceListId
                        };
                        foundationDefaultPriceList = defaultPriceList.ToDataModel();
                    }

                    entity.Pricelist = foundationDefaultPriceList;
                }

                repository.Add(entity);
                CommitChanges(repository);
            }

            var retVal = GetPriceById(price.Id);
            return retVal;
        }

        public virtual coreModel.Pricelist CreatePricelist(coreModel.Pricelist priceList)
        {
            var entity = priceList.ToDataModel();

            using (var repository = _repositoryFactory())
            {
                repository.Add(entity);
                CommitChanges(repository);
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
            }
        }

        public virtual void UpdatePricelists(coreModel.Pricelist[] priceLists)
        {
            using (var repository = _repositoryFactory())
            using (var changeTracker = GetChangeTracker(repository))
            {
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

                    if (priceList.Assignments != null)
                    {
                        UpdatePricelistAssignments(priceList.Assignments.ToArray());
                    }
                }

                CommitChanges(repository);
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


        private static string GetDefaultPriceListName(CurrencyCodes currency)
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
            }
        }


        public coreModel.PricelistAssignment GetPricelistAssignmentById(string id)
        {
            foundationModel.PricelistAssignment retVal;

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
            }
        }

        public void DeletePricelistsAssignments(string[] ids)
        {
            GenericDelete(ids, (repository, id) => repository.GetPricelistAssignmentById(id));
        }

        #endregion
    }
}
