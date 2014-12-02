using VirtoCommerce.Foundation.Importing.Services;

namespace VirtoCommerce.CatalogModule.Web.Services
{
    internal class ImportService
    {
        private readonly IImportService _importServiceImpl;
        public ImportService(IImportService importServiceImpl)
        {
            _importServiceImpl = importServiceImpl;
        }
    }
}
