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

		public IEnumerable<coreModel.SeoInfo> GetObjectsSeo(string[] ids)
		{
			var retVal = new List<coreModel.SeoInfo>();
			using (var repository = _repositoryFactory())
			{
				retVal = repository.SeoUrlKeywords.Where(x => ids.Contains(x.ObjectId)).ToArray()
								  .Select(x => x.ToCoreModel()).ToList();
			}
			return retVal;
		}

		public coreModel.SeoInfo UpsertSeo(coreModel.SeoInfo seo)
		{
			if (seo == null)
				throw new ArgumentNullException("seo");

			coreModel.SeoInfo retVal = null;
			using (var repository = _repositoryFactory())
			{
				var sourceEntry = seo.ToDataModel();
				var targetEntry = repository.SeoUrlKeywords.FirstOrDefault(x => x.Id == seo.Id || (x.ObjectId == sourceEntry.ObjectId && x.ObjectType == sourceEntry.ObjectType));
				if (targetEntry == null)
				{
					repository.Add(sourceEntry);
				}
				else
				{
					sourceEntry.Patch(targetEntry);
				}
				CommitChanges(repository);
				seo.Id = sourceEntry.Id ?? targetEntry.Id;
				retVal = repository.SeoUrlKeywords.First(x => x.Id == seo.Id).ToCoreModel();
			}
			return retVal;
		}

		public void DeleteSeo(string[] ids)
		{
			using (var repository = _repositoryFactory())
			{
				foreach(var keyword in repository.SeoUrlKeywords.Where(x=>ids.Contains(x.Id)))
				{
					repository.Remove(keyword);
				}
			}
		}


		public IEnumerable<coreModel.SeoInfo> GetSeoByKeyword(string keyword)
		{
			var retVal = new List<coreModel.SeoInfo>();
			using (var repository = _repositoryFactory())
			{
                //find seo entries for specified keyword
				retVal = repository.SeoUrlKeywords.Where(x => x.Keyword == keyword).ToArray()
								  .Select(x => x.ToCoreModel()).ToList();
                //find other seo entries related to finding object
                if(retVal.Any())
                {
                    var objectIds = retVal.Select(x => x.ObjectId).Distinct().ToArray();
                    retVal.AddRange(GetObjectsSeo(objectIds));
                }
			}
			return retVal;
		}

		#endregion

	}
}
