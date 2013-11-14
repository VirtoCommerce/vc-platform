using VirtoCommerce.Foundation.Importing.Model;

namespace VirtoCommerce.ManagementClient.Import.Model
{
	public class ColumnMappingEntity
    {
		public MappingItem MappingItem
		{
			get;
			set;
		}

		public string[] CsvColumnsList
		{
			get;
			set;
		}

		public ImportProperty ImportProperty { get; set; }
	}
}
