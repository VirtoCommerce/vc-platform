using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Repositories;

namespace VirtoCommerce.Platform.Data.ChangeLog
{
	public class ChangeLogService : ServiceBase, IChangeLogService
	{
		private Func<IPlatformRepository> _platformRepositoryFactory;
		public ChangeLogService(Func<IPlatformRepository> platformRepositoryFactory)
		{
			_platformRepositoryFactory = platformRepositoryFactory;
		}
        #region IChangeLogService Members

        public void LoadChangeLogs(IHasChangesHistory owner)
        {
            var objectsWithChangesHistory = owner.GetFlatObjectsListWithInterface<IHasChangesHistory>();

            foreach (var objectWithChangesHistory in objectsWithChangesHistory)
            {
                if (objectWithChangesHistory.Id != null)
                {
                    objectWithChangesHistory.OperationsLog = FindObjectChangeHistory(objectWithChangesHistory.Id, objectWithChangesHistory.GetType().Name).ToList();
                }
            }
        }

        public IEnumerable<OperationLog> SaveChanges(params OperationLog[] operationLogs)
        {
            if (operationLogs == null)
            {
                throw new ArgumentNullException("operationLogs");
            }
            var retVal = new List<OperationLogEntity>();
            using (var repository = _platformRepositoryFactory())
            {
                var ids = operationLogs.Where(x => x.Id != null).Select(x => x.Id).Distinct().ToArray();

                var origDbOperations = repository.OperationLogs.Where(x=> ids.Contains(x.Id));
                using (var changeTracker = GetChangeTracker(repository))
                {
                    //Update
                    foreach (var origDbOperation in origDbOperations)
                    {
                        var changedOperation = operationLogs.First(x => x.Id == origDbOperation.Id);
                    
                        var changedDbOperation = changedOperation.ToDataModel();
                        changeTracker.Attach(origDbOperation);
                        changedDbOperation.Patch(origDbOperation);
                        retVal.Add(origDbOperation);
                    }

                    //Create
                    var newOperations = operationLogs.Where(x => !origDbOperations.Any(y => y.Id == x.Id));
                    foreach (var newOperation in newOperations)
                    {
                        var newDbOperation = newOperation.ToDataModel();
                        repository.Add(newDbOperation);
                        retVal.Add(newDbOperation);
                    }
                    repository.UnitOfWork.Commit();
                }
            }
            return retVal.Select(x => x.ToCoreModel());
        }

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
