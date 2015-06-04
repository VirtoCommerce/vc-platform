using System.Collections.Generic;
using VirtoCommerce.Foundation.Frameworks;
using System.ServiceModel;
using VirtoCommerce.Foundation.Search.Model;
using System.Linq;

namespace VirtoCommerce.Foundation.Search.Repositories
{
	public interface IBuildSettingsRepository : IRepository
    {
        IQueryable<BuildSettings> BuildSettings { get; }
    }
}
