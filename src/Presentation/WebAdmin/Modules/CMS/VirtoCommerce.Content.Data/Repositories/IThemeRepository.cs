using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Content.Data.Models;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Content.Data.Repositories
{
	public interface IThemeRepository : IRepository
	{
		IQueryable<ThemeStoreRelation> ThemeStoreRelations { get; }
	}
}
