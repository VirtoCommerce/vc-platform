using VirtoCommerce.Foundation.Importing.Model;
using VirtoCommerce.ManagementClient.Import.Model;

namespace VirtoCommerce.ManagementClient.Import.ViewModel.Implementations
{
	public class ImportProgress
	{
		public ImportResult ImportResult { get; set; }
		public ImportEntity ImportEntity { get; set; }
		public string StatusId { get; set; }
		public int Processed { get; set; }
	}
}
