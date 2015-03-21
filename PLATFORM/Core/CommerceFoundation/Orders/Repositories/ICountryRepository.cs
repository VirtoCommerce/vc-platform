using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Orders.Model.Countries;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Orders.Repositories
{
	public interface ICountryRepository : IRepository
	{
		IQueryable<Country> Countries { get; }
    }
}
