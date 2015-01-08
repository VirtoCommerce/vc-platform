using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Domain.Cart.Repositories
{
	public interface IShoppingCartRepository : IRepository
	{
		IQueryable<ShoppingCart> ShoppingCarts { get; }
		ShoppingCart GetById(string id);

	}
}
