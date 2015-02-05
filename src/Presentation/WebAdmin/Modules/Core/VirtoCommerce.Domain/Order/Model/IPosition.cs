using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Order.Model
{
	public interface IPosition
	{
		string ProductId { get; }
		string CatalogId { get; }
		string CategoryId { get; }
		string Name { get; }
		string ImageUrl { get; }
		int Quantity { get; }

	}
}
