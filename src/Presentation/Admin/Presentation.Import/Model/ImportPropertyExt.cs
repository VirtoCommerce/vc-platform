using VirtoCommerce.Foundation.Importing.Model;

namespace VirtoCommerce.ManagementClient.Import.Model.Importing
{
	class ImportPropertyExt: ImportProperty
	{
		private string mappedColumnName;
		public string MappedColumnName
		{
			get { return mappedColumnName; }
			set { mappedColumnName = value; }
		}

		private string[] csvColumnsList;
		public string[] CsvColumnsList
		{
			get { return csvColumnsList; }
			set { csvColumnsList = value; }
		}
	}
}
