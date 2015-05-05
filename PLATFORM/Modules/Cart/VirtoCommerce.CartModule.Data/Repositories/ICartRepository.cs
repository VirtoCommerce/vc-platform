using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CartModule.Data.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CartModule.Data.Repositories
{
	public interface ICartRepository : IRepository
	{
		IQueryable<ShoppingCartEntity> ShoppingCarts { get; }
		ShoppingCartEntity GetShoppingCartById(string id);

	}
}
