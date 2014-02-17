using System.IO;
using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Importing.Model;

namespace VirtoCommerce.Foundation.Importing.Repositories
{
	public interface IImportRepository : IRepository
	{
		IQueryable<ImportJob> ImportJobs { get; }
		IQueryable<MappingItem> MappingItems { get; }
	}
}
