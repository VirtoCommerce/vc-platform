using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Catalog.Model;

namespace VirtoCommerce.Domain.Catalog.Services
{
	public interface ISkuGenerator
	{
		string GenerateSku(CatalogProduct product);
	}
}
