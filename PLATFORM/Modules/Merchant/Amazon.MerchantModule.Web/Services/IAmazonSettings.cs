using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Amazon.MerchantModule.Web.Services
{
    public interface IAmazonSettings
    {
        string MerchantId { get; }
        string AwsAccessKeyId { get; }
        string AwsSecretAccessKey { get; }
        string MarketplaceId { get; }
        string ServiceURL { get; }
    }
}