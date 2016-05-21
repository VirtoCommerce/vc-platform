using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.ChangeLog
{
	public interface IHasChangesHistory
	{
        string Id { get; set; }
		ICollection<OperationLog> OperationsLog { get; set; }
	}
}
