using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Cart.Model;

namespace VirtoCommerce.Domain.Cart.Services
{
	public interface IShoppingCartSearchService
	{
		SearchResult Search(SearchCriteria criteria);
	}
}
