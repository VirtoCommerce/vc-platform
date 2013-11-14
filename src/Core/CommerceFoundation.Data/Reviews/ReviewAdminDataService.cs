using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Foundation.Data.Reviews
{
    [JsonSupportBehavior]
    public class ReviewAdminDataService : DSReviewService
    {
        public static new void InitializeService(DataServiceConfiguration config)
        {
            config.UseVerboseErrors = true;
            config.SetEntitySetAccessRule("Review", EntitySetRights.All);
            config.SetEntitySetAccessRule("ReviewComment", EntitySetRights.All);
            config.SetEntitySetAccessRule("ReviewFieldValue", EntitySetRights.All);
            config.SetEntitySetAccessRule("UserBlackList", EntitySetRights.All);
            config.SetEntitySetAccessRule("ReviewSchema", EntitySetRights.All);
            config.SetEntitySetAccessRule("Subscription", EntitySetRights.All);
            config.SetEntitySetAccessRule("ReviewFieldSchema", EntitySetRights.All);

            //This could be "*" and could also be ReadSingle, etc, etc.
            config.SetServiceOperationAccessRule("GetTopReviews", ServiceOperationRights.AllRead);
			
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;
        }
    }
}
