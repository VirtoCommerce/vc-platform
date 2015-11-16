using System;

namespace Amazon.MerchantModule.Web.Helpers.Interfaces
{
    public interface IDateTimeProvider
    {
        DateTime CurrentUtcDateTime { get; }
        DateTime CurrentLocalDateTime { get; }
    }
}
