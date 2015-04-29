using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Fulfillment.Services;
using coreModel = VirtoCommerce.Domain.Fulfillment.Model;
using dataModel = VirtoCommerce.CoreModule.Data.Model;
using System.Collections.ObjectModel;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.CoreModule.Data.Converters;

namespace VirtoCommerce.CoreModule.Data.Repositories
{
	public class FulfillmentServiceImpl : ServiceBase, IFulfillmentService
	{
		private readonly Func<IСommerceRepository> _repositoryFactory;
		public FulfillmentServiceImpl(Func<IСommerceRepository> repositoryFactory)
		{
			_repositoryFactory = repositoryFactory;
		}

		#region IFulfillmentService Members

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
				var sourceEntry = center.ToFoundation();
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
			throw new NotImplementedException();
		}

		#endregion

	}
}
