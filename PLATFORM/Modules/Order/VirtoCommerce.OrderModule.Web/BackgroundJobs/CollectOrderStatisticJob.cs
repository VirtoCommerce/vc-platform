using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.OrderModule.Data.Repositories;

namespace VirtoCommerce.OrderModule.Web.BackgroundJobs
{
	public class CollectOrderStatisticJob
	{
		private readonly Func<IOrderRepository> _repositoryFactory;

		internal CollectOrderStatisticJob()
		{
		}

		public CollectOrderStatisticJob(Func<IOrderRepository> repositoryFactory)
		{
			_repositoryFactory = repositoryFactory;
		}

		//public void CollectStat
	}
}