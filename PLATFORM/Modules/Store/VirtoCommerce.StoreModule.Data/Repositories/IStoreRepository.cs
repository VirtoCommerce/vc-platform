using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.StoreModule.Data.Model;
using foundation = VirtoCommerce.StoreModule.Data.Model;

namespace VirtoCommerce.StoreModule.Data.Repositories
{
	public interface IStoreRepository : IRepository
	{
		IQueryable<Store> Stores { get; }
		foundation.Store GetStoreById(string id);
	}
}
