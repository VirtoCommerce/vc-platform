using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Services;
using VirtoCommerce.Foundation.Reviews.Factories;
using System.ServiceModel;
using System.Data.Entity;
using VirtoCommerce.Foundation.Data.Infrastructure;
using System.Data.Services.Common;

namespace VirtoCommerce.Foundation.Data.Reviews
{
    [JsonSupportBehavior]
	public class DSReviewService : DServiceBase<EFReviewRepository>
	{
        string _connectionStringName = String.Empty;

        public DSReviewService()
        {
        }

        public static new void InitializeService(DataServiceConfiguration config)
        {
            config.SetEntitySetAccessRule("*", EntitySetRights.All);

            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;
        }
	}
}
