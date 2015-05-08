using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Data.Repositories;

namespace VirtoCommerce.Platform.Data.ChangeLog
{
	public class ChangeLogService : IChangeLogService
	{
		private Func<IPlatformRepository> _platformRepositoryFactory;
		public ChangeLogService(Func<IPlatformRepository> platformRepositoryFactory)
		{
			_platformRepositoryFactory = platformRepositoryFactory;
		}
		#region IChangeLogService Members

		public IEnumerable<OperationLog> FindObjectChangeHistory(string objectId, string objectType)
		{
			if(objectId == null)
				throw new ArgumentNullException("objectId");
		
			if(objectType == null)
				throw new ArgumentNullException("objectType");

			var retVal = new List<OperationLog>();
			using(var repository = _platformRepositoryFactory())
			{
				retVal = repository.OperationLogs.Where(x => x.ObjectId == objectId && x.ObjectType == objectType)
													.OrderBy(x => x.ModifiedDate).ToArray()
													.Select(x => x.ToCoreModel()).ToList();
											
			}
			return retVal;
		}

		public OperationLog GetObjectLastChange(string objectId, string objectType)
		{
			if (objectId == null)
				throw new ArgumentNullException("objectId");

			if (objectType == null)
				throw new ArgumentNullException("objectType");

			OperationLog retVal = null;
			using (var repository = _platformRepositoryFactory())
			{
				var entity = repository.OperationLogs.Where(x => x.ObjectId == objectId && x.ObjectType == objectType)
													 .OrderByDescending(x => x.ModifiedDate).FirstOrDefault();
				if(entity != null)
				{
					retVal = entity.ToCoreModel();
				}

			}
			return retVal;
		}

		public IEnumerable<OperationLog> FindChangeHistory(string objectType, DateTime? startDate, DateTime? endDate)
		{

			if (objectType == null)
				throw new ArgumentNullException("objectType");

			var retVal = new List<OperationLog>();
			using (var repository = _platformRepositoryFactory())
			{
				retVal = repository.OperationLogs.Where(x =>x.ObjectType == objectType && (startDate == null || x.ModifiedDate >= startDate) && (endDate == null || x.ModifiedDate <= endDate))
												 .OrderBy(x => x.ModifiedDate).ToArray()
												 .Select(x => x.ToCoreModel()).ToList();

			}
			return retVal;
		}

		#endregion
	}
}
