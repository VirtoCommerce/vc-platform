using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.ConfigurationUtility.Main.Properties;
using VirtoCommerce.ConfigurationUtility.Main.ViewModels;

namespace VirtoCommerce.ConfigurationUtility.Main.Infrastructure
{
    internal static class OperationResultExtensions
    {
        internal static string ToState(this OperationResult? result)
        {
            switch (result)
            {
                case OperationResult.Successful:
                    return Resources.Completed;
                case OperationResult.Cancelled:
                    return Resources.Cancelled;
                case OperationResult.Failed:
                    return Resources.Failed;
                default:
                    return Resources.InProgress;
            }
        }

		internal static void UpdateState(this OperationResult? result, IConfigurationViewModel configurationViewModel, string action)
        {
            var state = configurationViewModel.Steps.FirstOrDefault(step => step.Key == action);
            configurationViewModel.Steps[configurationViewModel.Steps.IndexOf(state)] = new KeyValuePair<string, string>(action, result.ToState());
        }
    }
}