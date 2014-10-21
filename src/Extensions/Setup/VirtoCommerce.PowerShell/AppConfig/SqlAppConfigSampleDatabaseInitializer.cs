using System.Linq;
using VirtoCommerce.Foundation.Data.AppConfig;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.PowerShell.AppConfig
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