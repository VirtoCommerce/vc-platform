using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.ChangeLog
{
	public interface IChangeLogService
	{
		IEnumerable<OperationLog> FindChangeHistory(string objectType, DateTime? startDate, DateTime? endDate);
		IEnumerable<OperationLog> FindObjectChangeHistory(string objectId, string objectType);
		OperationLog GetObjectLastChange(string objectId, string objectType);
	}
}
