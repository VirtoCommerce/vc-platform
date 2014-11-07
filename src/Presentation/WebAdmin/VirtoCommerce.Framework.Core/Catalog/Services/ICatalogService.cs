using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using model = Vermouth.Core.Models.Catalog;
namespace Vermouth.Core.Catalog.Services
{
	public interface ICatalogService
	{
		model.Catalog GetNewCatalog();
		model.Catalog GetNewVirtualCatalog();
		void SaveCatalogChanges(model.Catalog catalog);
		void DeleteCatalog(string catalogId);
		model.Catalog GetCatalogById(string catalogId);
	}
}
