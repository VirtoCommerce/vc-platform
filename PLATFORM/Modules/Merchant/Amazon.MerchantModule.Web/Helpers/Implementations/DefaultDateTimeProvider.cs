using System;
using Amazon.MerchantModule.Web.Helpers.Interfaces;

namespace Amazon.MerchantModule.Web.Helpers.Implementations
{
    public class DefaultDateTimeProvider : IDateTimeProvider
    {

        public DateTime CurrentUtcDateTime
        {
            get { return DateTime.UtcNow; }
        }

        public DateTime CurrentLocalDateTime
        {
            get { return DateTime.Now; }
        }
    }
}