using System;
using System.Linq;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.GenericCrud;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Search;
using VirtoCommerce.Platform.Data.GenericCrud;
using VirtoCommerce.Platform.Security.Model;
using VirtoCommerce.Platform.Security.Repositories;

namespace VirtoCommerce.Platform.Security.Services
{
    public class ServerCertificateSearchService : SearchService<ServerCertificateSearchCriteria, ServerCertificateSearchResult, ServerCertificate, ServerCertificateEntity>
    {
        public ServerCertificateSearchService(Func<ISecurityRepository> repositoryFactory, IPlatformMemoryCache platformMemoryCache, ICrudService<ServerCertificate> crudService) : base(repositoryFactory, platformMemoryCache, crudService)
        {
        }

        protected override IQueryable<ServerCertificateEntity> BuildQuery(IRepository repository, ServerCertificateSearchCriteria criteria)
        {
            var query = ((ISecurityRepository)repository).ServerCertificates;
            if (!criteria.ObjectIds.IsNullOrEmpty())
            {
                query = query.Where(x => criteria.ObjectIds.Contains(x.Id));
            }
            return query;
        }
    }
}
