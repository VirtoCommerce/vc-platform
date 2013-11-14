using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.AppConfig.Model;

namespace VirtoCommerce.ManagementClient.AppConfig.ViewModel.AppConfig.Interfaces
{
	public interface IAppConfigSettingEditViewModel : IViewModel
	{
		Setting InnerItem { get; set; }

		bool IsSystemSetting { get; }
		bool IsValueTypeChangeable { get; }
		bool CanEnterValue { get; }
		bool IsShortStringValue { get; }
		bool IsLongStringValue { get; }
		bool IsDecimalValue { get; }
		bool IsIntegerValue { get; }
		bool IsBooleanValue { get; }
		bool IsDateTimeValue { get; }
		Infrastructure.Enumerations.ValueType ValueType { get; }
		SettingValue SelectedSettingValue { get; }

		DelegateCommand ItemAddCommand { get; }
		DelegateCommand<SettingValue> ItemEditCommand { get; }
		DelegateCommand<SettingValue> ItemDeleteCommand { get; }

        InteractionRequest<ConditionalConfirmation> RemoveConfirmRequest { get; }

		//bool Validate();
	}
}
