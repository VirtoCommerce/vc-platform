using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreModel = VirtoCommerce.Domain.Commerce.Model;
using dataModel = VirtoCommerce.CoreModule.Data.Model;
using System.Collections.ObjectModel;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.CoreModule.Data.Converters;
using VirtoCommerce.Domain.Commerce.Services;

namespace VirtoCommerce.CoreModule.Data.Repositories
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
			var retVal = new List<coreModel.FulfillmentCenter>();
			using (var repository = _repositoryFactory())
			{
				retVal = repository.FulfillmentCenters.ToArray().Select(x => x.ToCoreModel()).ToList();
			}
			return retVal;
		}

		public coreModel.FulfillmentCenter UpsertFulfillmentCenter(coreModel.FulfillmentCenter center)
		{
			if (center == null)
				throw new ArgumentNullException("center");

			coreModel.FulfillmentCenter retVal = null;
			using (var repository = _repositoryFactory())
			{
				var sourceEntry = center.ToDataModel();
				var targetEntry = repository.FulfillmentCenters.FirstOrDefault(x=>x.Id == center.Id);
				if(targetEntry == null)
				{
					repository.Add(sourceEntry);
				}
				else
				{
					sourceEntry.Patch(targetEntry);
				}

				CommitChanges(repository);
                retVal = repository.FulfillmentCenters.First(x => x.Id == sourceEntry.Id).ToCoreModel();
			}
			return retVal;
		}


		public void DeleteFulfillmentCenter(string[] ids)
		{
			using (var repository = _repositoryFactory())
			{
				foreach(var center in repository.FulfillmentCenters.Where(x=>ids.Contains(x.Id)))
				{
					repository.Remove(center);
				}
			}
		}

		public IEnumerable<coreModel.SeoUrlKeyword> GetSeoKeywordsForEntity(string id)
		{
			return GetSeoKeywordsForEntities(new string[] { id });
		}

		public IEnumerable<coreModel.SeoUrlKeyword> GetSeoKeywordsForEntities(string[] ids)
		{
			var retVal = new List<coreModel.SeoUrlKeyword>();
			using (var repository = _repositoryFactory())
			{
				retVal = repository.SeoUrlKeywords.Where(x => ids.Contains(x.KeywordValue)).ToArray()
								  .Select(x => x.ToCoreModel()).ToList();
			}
			return retVal;
		}

		public coreModel.SeoUrlKeyword UpsertSeoKeyword(coreModel.SeoUrlKeyword seoKeyword)
		{
			if (seoKeyword == null)
				throw new ArgumentNullException("seoKeyword");

			coreModel.SeoUrlKeyword retVal = null;
			using (var repository = _repositoryFactory())
			{
				var sourceEntry = seoKeyword.ToDataModel();
				var targetEntry = repository.SeoUrlKeywords.FirstOrDefault(x => x.Id == seoKeyword.Id);
				if (targetEntry == null)
				{
					repository.Add(sourceEntry);
				}
				else
				{
					sourceEntry.Patch(targetEntry);
				}

				CommitChanges(repository);
				retVal = repository.SeoUrlKeywords.First(x => x.Id == sourceEntry.Id).ToCoreModel();
			}
			return retVal;
		}

		public void DeleteSeoKeywords(string[] ids)
		{
			using (var repository = _repositoryFactory())
			{
				foreach(var keyword in repository.SeoUrlKeywords.Where(x=>ids.Contains(x.Id)))
				{
					repository.Remove(keyword);
				}
			}
		}


		public IEnumerable<coreModel.SeoUrlKeyword> GetSeoKeywordsByKeyword(string keyword)
		{
			var retVal = new List<coreModel.SeoUrlKeyword>();
			using (var repository = _repositoryFactory())
			{
				retVal = repository.SeoUrlKeywords.Where(x => x.Keyword == keyword).ToArray()
								  .Select(x => x.ToCoreModel()).ToList();
			}
			return retVal;
		}

		#endregion
	}
}
