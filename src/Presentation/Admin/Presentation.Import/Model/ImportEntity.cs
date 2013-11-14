using VirtoCommerce.Foundation.Importing.Model;

namespace VirtoCommerce.ManagementClient.Import.Model
{
	public class ImportEntity
    {
		public ImportJob ImportJob { get; set; }
		public byte[] Bytes
		{
			get;
			set;
		}
		public string SourceFile { get; set; }
    }
}
