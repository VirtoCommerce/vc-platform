using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Quote.Events;
using VirtoCommerce.Domain.Quote.Model;
using VirtoCommerce.Domain.Quote.Services;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Settings;
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
        private readonly IStoreService _storeService;

        public QuoteRequestServiceImpl(Func<IQuoteRepository> quoteRepositoryFactory, IUniqueNumberGenerator uniqueNumberGenerator, IDynamicPropertyService dynamicPropertyService, IEventPublisher<QuoteRequestChangeEvent> eventPublisher, IChangeLogService changeLogService, IStoreService storeService)
		{
			_repositoryFactory = quoteRepositoryFactory;
			_uniqueNumberGenerator = uniqueNumberGenerator;
			_dynamicPropertyService = dynamicPropertyService;
			_eventPublisher = eventPublisher;
            _changeLogService = changeLogService;
            _storeService = storeService;
        }

		#region IQuoteRequestService Members

		public IEnumerable<QuoteRequest> GetByIds(params string[] ids)
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
		
			//Generate Number
			EnsureThatQuoteHasNumber(quoteRequests);
            var pkMap = new PrimaryKeyResolvingMap();
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
                     
                        var changedDbQuote = changedQuote.ToDataModel(pkMap);
                        changeTracker.Attach(origDbQuote);
						changedDbQuote.Patch(origDbQuote);
					}

					//Create
					var newQuotes = quoteRequests.Where(x => !origDbQuotes.Any(y => y.Id == x.Id));
					foreach(var newQuote in newQuotes)
					{
                        // Do business logic on  quote request
                        _eventPublisher.Publish(new QuoteRequestChangeEvent(EntryState.Added, newQuote, newQuote));
						var newDbQuote = newQuote.ToDataModel(pkMap);
						repository.Add(newDbQuote);
                    
                    }
                    repository.UnitOfWork.Commit();
                    //Copy generated id from dbEntities to model
                    pkMap.ResolvePrimaryKeys();
				}

				//Save dynamic properties
				foreach (var quoteRequest in quoteRequests)
				{
                    _dynamicPropertyService.SaveDynamicPropertyValues(quoteRequest);
                }
                return quoteRequests;
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

                if(criteria.Number != null)
                {
                    query = query.Where(x => x.Number == criteria.Number);
                }
                else if(criteria.Keyword != null)
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
                var ids = query.OrderByDescending(x => x.CreatedDate)
                               .Skip(criteria.Start)
                               .Take(criteria.Count)
                               .Select(x => x.Id)
                               .ToArray();

                retVal = new QuoteRequestSearchResult
                {
                    TotalCount = query.Count(),
                    QuoteRequests = GetByIds(ids).ToList()
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
            var stores = _storeService.GetByIds(quoteRequests.Select(x => x.StoreId).Distinct().ToArray());
			foreach (var quoteRequest in quoteRequests)
			{
				if (string.IsNullOrEmpty(quoteRequest.Number))
				{
                    var store = stores.FirstOrDefault(x => x.Id == quoteRequest.StoreId);
                    var numberTemplate = "RFQ{0:yyMMdd}-{1:D5}";
                    if (store != null)
                    {
                        numberTemplate = store.Settings.GetSettingValue("Quotes.QuoteRequestNewNumberTemplate", numberTemplate);
                    }
                    quoteRequest.Number = _uniqueNumberGenerator.GenerateNumber(numberTemplate);
                }
            }
		}

	}
}
