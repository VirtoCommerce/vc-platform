using VirtoCommerce.Foundation.Importing.Factories;
using VirtoCommerce.Foundation.Data.Infrastructure;

namespace VirtoCommerce.Foundation.Data.Importing
{
    [JsonSupportBehavior]
	public class DSImportService : DServiceBase<EFImportingRepository>
	{
		protected override EFImportingRepository CreateDataSource()
		{
			return new EFImportingRepository(new ImportJobEntityFactory());
		}
	
	}
}
