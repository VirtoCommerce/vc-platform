using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Frameworks
{
	public interface IUnitOfWork
	{
		int Commit();
		void CommitAndRefreshChanges();
		void RollbackChanges();
	}
}
