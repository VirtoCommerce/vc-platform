using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Platform.Core.Common
{
	public interface IUnitOfWork
	{
		int Commit();
		void CommitAndRefreshChanges();
		void RollbackChanges();
	}
}
