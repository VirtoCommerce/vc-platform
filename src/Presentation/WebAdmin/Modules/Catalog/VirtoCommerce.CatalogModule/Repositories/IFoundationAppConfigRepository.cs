using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using foundation = VirtoCommerce.Foundation.Catalogs.Model;
using foundationConfig = VirtoCommerce.Foundation.AppConfig.Model;
using moduleModel = VirtoCommerce.CatalogModule.Model;

namespace VirtoCommerce.CatalogModule.Repositories
{
	public interface IFoundationAppConfigRepository : IAppConfigRepository
	{
		foundationConfig.SeoUrlKeyword[] GetAllSeoInformation(string id);
	}
}
