using System.Linq;

namespace VirtoCommerce.Foundation.Data.AppConfig
{
	public class SqlAppConfigSampleDatabaseInitializer : SqlAppConfigDatabaseInitializer
	{
		protected override string[] GetSampleFiles()
		{
			var retVal = base.GetSampleFiles().ToList();
			retVal.Add("DisplayTemplateMapping.sql");
			retVal.Add("SeoUrlKeyword.sql");
			return retVal.ToArray();
		}
	}
}