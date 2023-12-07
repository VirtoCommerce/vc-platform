using System;

namespace VirtoCommerce.Platform.Core.Common
{
    public static class DateTimeExtensions
    {
        public static DateTime Truncate(this DateTime dateTime, TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.Zero)
                return dateTime; // Or could throw an ArgumentException
            return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
        }

        public static bool IsEmpty(this DateTimeOffset? v) => (v ?? DateTimeOffset.MinValue) == DateTimeOffset.MinValue;
    }
}
