using System;
using System.Collections.Generic;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Converters;
using VirtoCommerce.Foundation.Security.Model;

namespace VirtoCommerce.ManagementClient.Security.Infrastructure
{
    public class AccountStateConverter : EnumToIntConverter<AccountState>
    {
        private IDictionary<AccountState, string> textResources;

        public AccountStateConverter()
        {
            // TODO take localized texts from resources
            textResources = new Dictionary<AccountState, string>
                {
                    {AccountState.Approved, "Approved"},
                    {AccountState.Rejected, "Rejected"},
                    {AccountState.PendingApproval, "Pending Approval"}
                };
        }

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            object result;
            var item = (AccountState)base.Convert(value, targetType, parameter, culture);
            if (targetType == typeof(string))
                result = textResources[item];
            else
                result = item;

            return result;
        }
    }
}
