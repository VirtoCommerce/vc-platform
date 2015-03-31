using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CoreModule.Web.Repositories;
using VirtoCommerce.Domain.Fulfillment.Services;
using VirtoCommerce.Foundation.Data.Infrastructure;
using coreModel = VirtoCommerce.Domain.Fulfillment.Model;
using foundation = VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.CoreModule.Web.Converters;
using System.Collections.ObjectModel;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.CoreModule.Web.Services
{
	public class FulfillmentServiceImpl : ServiceBase, IFulfillmentService
	{
		private readonly Func<IFoundationFulfillmentRepository> _repositoryFactory;
		public FulfillmentServiceImpl(Func<IFoundationFulfillmentRepository> repositoryFactory)
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
				var targetEntry = repository.FulfillmentCenters.FirstOrDefault(x=>x.FulfillmentCenterId == center.Id);
				if(targetEntry == null)
				{
					repository.Add(sourceEntry);
				}
				else
				{
					sourceEntry.Patch(targetEntry);
				}

				CommitChanges(repository);
                retVal = repository.FulfillmentCenters.First(x => x.FulfillmentCenterId == sourceEntry.FulfillmentCenterId).ToCoreModel();
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
