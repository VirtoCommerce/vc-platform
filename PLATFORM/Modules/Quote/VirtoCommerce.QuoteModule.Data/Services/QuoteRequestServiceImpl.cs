using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Quote.Events;
using VirtoCommerce.Domain.Quote.Model;
using VirtoCommerce.Domain.Quote.Services;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.QuoteModule.Data.Converters;
using VirtoCommerce.QuoteModule.Data.Model;
using VirtoCommerce.QuoteModule.Data.Repositories;

namespace VirtoCommerce.QuoteModule.Data.Services
{
	public class QuoteRequestServiceImpl : ServiceBase, IQuoteRequestService
	{
		private readonly Func<IQuoteRepository> _repositoryFactory;
		private readonly IUniqueNumberGenerator _uniqueNumberGenerator;
		private readonly IDynamicPropertyService _dynamicPropertyService;
		private readonly IEventPublisher<QuoteRequestChangeEvent> _eventPublisher;
        private readonly IChangeLogService _changeLogService;

        public QuoteRequestServiceImpl(Func<IQuoteRepository> quoteRepositoryFactory, IUniqueNumberGenerator uniqueNumberGenerator, IDynamicPropertyService dynamicPropertyService, IEventPublisher<QuoteRequestChangeEvent> eventPublisher, IChangeLogService changeLogService)
		{
			_repositoryFactory = quoteRepositoryFactory;
			_uniqueNumberGenerator = uniqueNumberGenerator;
			_dynamicPropertyService = dynamicPropertyService;
			_eventPublisher = eventPublisher;
            _changeLogService = changeLogService;
        }

		#region IQuoteRequestService Members

		public IEnumerable<QuoteRequest> GetByIds(string[] ids)
		{
            using (var repository = _repositoryFactory())
            {
                var retVal = repository.GetQuoteRequestByIds(ids).Select(x => x.ToCoreModel()).ToArray();
                foreach(var quote in retVal)
                {
                    _dynamicPropertyService.LoadDynamicPropertyValues(quote);
                    _changeLogService.LoadChangeLogs(quote);
                    _eventPublisher.Publish(new QuoteRequestChangeEvent(EntryState.Unchanged, quote, quote));
                }
                return retVal;
            }
		}

		public IEnumerable<QuoteRequest> SaveChanges(QuoteRequest[] quoteRequests)
		{
			if (quoteRequests == null)
			{
				throw new ArgumentNullException("quoteRequests");
			}
			//Need for primary key resolving
			var domainEntityMap = new List<KeyValuePair<QuoteRequest, QuoteRequestEntity>>();
			//Generate Number
			EnsureThatQuoteHasNumber(quoteRequests);

			using (var repository = _repositoryFactory())
			{
				var ids = quoteRequests.Where(x => x.Id != null).Select(x => x.Id).Distinct().ToArray();

				var origDbQuotes = repository.GetQuoteRequestByIds(ids);
				using (var changeTracker = GetChangeTracker(repository))
				{
					//Update
					foreach (var origDbQuote in origDbQuotes)
					{
						var changedQuote = quoteRequests.First(x => x.Id == origDbQuote.Id);
						// Do business logic on  quote request
						_eventPublisher.Publish(new QuoteRequestChangeEvent(EntryState.Modified, GetByIds(new[] { origDbQuote.Id }).First(), changedQuote));
                        domainEntityMap.Add(new KeyValuePair<QuoteRequest, QuoteRequestEntity>(changedQuote, origDbQuote));

                        var changedDbQuote = changedQuote.ToDataModel();
                        changeTracker.Attach(origDbQuote);
						changedDbQuote.Patch(origDbQuote);
					}

					//Create
					var newQuotes = quoteRequests.Where(x => !origDbQuotes.Any(y => y.Id == x.Id));
					foreach(var newQuote in newQuotes)
					{
                        // Do business logic on  quote request
                        _eventPublisher.Publish(new QuoteRequestChangeEvent(EntryState.Added, newQuote, newQuote));
						var newDbQuote = newQuote.ToDataModel();
						repository.Add(newDbQuote);
                        domainEntityMap.Add(new KeyValuePair<QuoteRequest, QuoteRequestEntity>(newQuote, newDbQuote));
                    }
                    repository.UnitOfWork.Commit();
				}

				//Save dynamic properties
				foreach (var pair in domainEntityMap)
				{
                    //Set key for all objects
                    pair.Key.SetObjectId(pair.Value.Id);
                    _dynamicPropertyService.SaveDynamicPropertyValues(pair.Key);
                }
                return domainEntityMap.Select(x => x.Key);
			}
		}

        public QuoteRequestSearchResult Search(QuoteRequestSearchCriteria criteria)
        {
            QuoteRequestSearchResult retVal = null;
            using (var repository = _repositoryFactory())
            {
                var query = repository.QuoteRequests;
                if (criteria.CustomerId != null)
                {
                    query = query.Where(x => x.CustomerId == criteria.CustomerId);
                }
                if (criteria.StoreId != null)
                {
                    query = query.Where(x => x.StoreId == criteria.StoreId);
                }
                if(criteria.Keyword != null)
                {
                    query = query.Where(x => x.Number.Contains(criteria.Keyword));
                }
                if(criteria.Tag != null)
                {
                    query = query.Where(x => x.Tag == criteria.Tag);
                }
                if (criteria.Status != null)
                {
                    query = query.Where(x => x.Status == criteria.Status);
                }
                retVal = new QuoteRequestSearchResult
                {
                    TotalCount = query.Count(),
                    QuoteRequests = query.OrderByDescending(x => x.CreatedDate)
                                      .Skip(criteria.Start)
                                      .Take(criteria.Count)
                                      .ToArray()
                                      .Select(x => x.ToCoreModel())
                                      .ToList()
                };
            }
            return retVal;
        }

		public void Delete(string[] ids)
		{
            using (var repository = _repositoryFactory())
            {
                var dbQuotes = repository.GetQuoteRequestByIds(ids);
                var quotes = GetByIds(ids);
                foreach (var dbQuote in dbQuotes)
                {
                    _eventPublisher.Publish(new QuoteRequestChangeEvent(Platform.Core.Common.EntryState.Deleted, quotes.First(x=>x.Id == dbQuote.Id), null));
                    repository.Remove(dbQuote);
                }
                repository.UnitOfWork.Commit();
            }
        } 
		#endregion

   
		private void EnsureThatQuoteHasNumber(QuoteRequest[] quoteRequests)
		{
			foreach (var quoteRequest in quoteRequests)
			{
				if (String.IsNullOrEmpty(quoteRequest.Number))
				{
					quoteRequest.Number = _uniqueNumberGenerator.GenerateNumber("RFQ{0:yyMMdd}-{1:D5}");
				}
			}
		}

	}
}
