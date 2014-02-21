using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.AppConfig.Factories;
using System.Data.Services;
using System.Data.Services.Common;

namespace VirtoCommerce.Foundation.Data.AppConfig
{
    [JsonSupportBehavior]
	public class DSAppConfigService : DServiceBase<EFAppConfigRepository>
	{
        public static new void InitializeService(DataServiceConfiguration config)
        {
            config.SetEntitySetAccessRule("*", EntitySetRights.All);

            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;
        }

	}
}
