using System;
using GoogleShopping.MerchantModule.Web.Helpers.Interfaces;

namespace GoogleShopping.MerchantModule.Web.Helpers.Implementations
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