using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.AppConfig.Model
{
	public class SettingModel : NotifyPropertyChanged
	{
		public SettingModel(object item = null)
		{
			if (item != null)
			{
				this.InjectFrom(item);

				var propInfo = item.GetType().FindPropertiesWithAttribute(typeof(KeyAttribute)).First();
				SettingId = propInfo.GetValue(item) as string ?? Guid.NewGuid().ToString();
			}
		}


		private string _settingId;
		[Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string SettingId
		{
			get
			{
				return _settingId;
			}
			set
			{
				_settingId = value;
				OnPropertyChanged();
			}
		}

		private string _name;
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
				OnPropertyChanged();
			}
		}

		private bool _isSystem;
		public bool IsSystem
		{
			get
			{
				return _isSystem;
			}
			set
			{
				_isSystem = value;
				OnPropertyChanged();
			}
		}

		private bool _isMultiValue;
		public bool IsMultiValue
		{
			get
			{
				return _isMultiValue;
			}
			set
			{
				_isMultiValue = value;
				OnPropertyChanged();
			}
		}

		public string SettingValuesString
		{
			get
			{
				return string.Join(", ", _values.ToList().Select(val => val.ToString()));
			}
		}

		private string _settingValueType;
		[StringLength(64)]
		public string SettingValueType
		{
			get
			{
				return _settingValueType;
			}
			set
			{
				_settingValueType = value;
				OnPropertyChanged();
			}
		}

		ObservableCollection<SettingValueModel> _values = new ObservableCollection<SettingValueModel>();
		public virtual ObservableCollection<SettingValueModel> SettingValues
		{
			get { return _values; }

		}

		public bool Validate()
		{
			return true;
		}

	}
}
