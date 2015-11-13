using System;
using Amazon.MerchantModule.Web.Helpers.Interfaces;

namespace Amazon.MerchantModule.Web.Helpers.Implementations
{
    public class ExpiredDateTimeProvider : IDateTimeProvider
    {

        public DateTime CurrentUtcDateTime
        {
            get { return DateTime.UtcNow.AddDays(60); }
        }

        public DateTime CurrentLocalDateTime
        {
            get { return DateTime.Now.AddDays(60); }
        }
    }
}