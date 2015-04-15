using System;
using GoogleShopping.MerchantModule.Web.Helpers.Interfaces;

namespace GoogleShopping.MerchantModule.Web.Helpers.Implementations
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