using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Stores.Repositories;
using foundation = VirtoCommerce.Foundation.Stores.Model;

namespace VirtoCommerce.StoreModule.Data.Repositories
{
	public interface IFoundationStoreRepository : IStoreRepository
	{
		foundation.Store GetStoreById(string id);
	}
}
