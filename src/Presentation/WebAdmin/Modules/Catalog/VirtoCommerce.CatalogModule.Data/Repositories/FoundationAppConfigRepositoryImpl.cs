using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Data.AppConfig;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using foundation = VirtoCommerce.Foundation.AppConfig.Model;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;

namespace VirtoCommerce.CatalogModule.Data.Repositories
{
	public class FoundationAppConfigRepositoryImpl : EFAppConfigRepository, IFoundationAppConfigRepository
	{
		public FoundationAppConfigRepositoryImpl(string nameOrConnectionString)
			: this(nameOrConnectionString, null)
		{
		}
		public FoundationAppConfigRepositoryImpl(string nameOrConnectionString, IInterceptor[] interceptors)
			: base(nameOrConnectionString, null, interceptors)
		{

		}

		#region IFoundationAppConfigRepository Members

		public foundation.SeoUrlKeyword[] GetAllSeoInformation(string id)
		{
			return SeoUrlKeywords.Where(x => x.IsActive && (string.IsNullOrEmpty(id) || x.KeywordValue == id) ).ToArray();
		}

		#endregion
	}
}
