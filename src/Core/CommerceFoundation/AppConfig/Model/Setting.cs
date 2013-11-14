using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.AppConfig.Model
{
	[DataContract]
	[EntitySet("Settings")]
	[DataServiceKey("SettingId")]
	public class Setting: StorageEntity
	{
		public Setting()
		{
			_systemSettingId = GenerateNewKey();
		}

		private string _systemSettingId;
		[Key]
		[StringLength(128)]
		[DataMember]
		public string SettingId
		{
			get { return _systemSettingId; }
			set { SetValue(ref _systemSettingId, () => SettingId, value); }
		}

		private string _name;
		[DataMember]
		[StringLength(128)]
		public string Name
		{
			get { return _name; }
			set { SetValue(ref _name, () => Name, value); }
		}

		private string _description;
		[DataMember]
		[StringLength(256)]
		public string Description
		{
			get { return _description; }
			set { SetValue(ref _description, () => Description, value); }
		}

		private bool _isSystem;
		[DataMember]
		public bool IsSystem
		{
			get 
			{ 
				return _isSystem; 
			}
			set 
			{ 
				SetValue(ref _isSystem, () => IsSystem, value); 
			}
		}

		private string _SettingValueType;
		[Required]
		[DataMember]
		[StringLength(64)]
		public string SettingValueType
		{
			get
			{
				return _SettingValueType;
			}
			set
			{
				SetValue(ref _SettingValueType, () => SettingValueType, value);
			}
		}

		private bool _IsEnum;
		[DataMember]
		public bool IsEnum
		{
			get
			{
				return _IsEnum;
			}
			set
			{
				SetValue(ref _IsEnum, () => this.IsEnum, value);
			}
		}

		private bool _IsMultiValue;
		[DataMember]
		public bool IsMultiValue
		{
			get
			{
				return _IsMultiValue;
			}
			set
			{
				SetValue(ref _IsMultiValue, () => this.IsMultiValue, value);
			}
		}

		private bool _IsLocaleDependant;
		/// <summary>
		/// Gets or sets a value indicating whether this instance is locale dependant. If true, the locale must be specified for the values.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is locale dependant; otherwise, <c>false</c>.
		/// </value>
		[DataMember]
		public bool IsLocaleDependant
		{
			get
			{
				return _IsLocaleDependant;
			}
			set
			{
				SetValue(ref _IsLocaleDependant, () => this.IsLocaleDependant, value);
			}
		}

		#region Navigation Properties

		ObservableCollection<SettingValue> _Values = null;
		[DataMember]
		public virtual ObservableCollection<SettingValue> SettingValues
		{
			get
			{
				if (_Values == null)
					_Values = new ObservableCollection<SettingValue>();

				return _Values;
			}
		}

		#endregion
	}
}
