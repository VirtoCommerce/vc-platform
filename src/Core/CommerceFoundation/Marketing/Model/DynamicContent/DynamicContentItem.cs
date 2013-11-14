using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using System.Collections.ObjectModel;
using System;

namespace VirtoCommerce.Foundation.Marketing.Model.DynamicContent
{
	[DataContract]
	[EntitySet("DynamicContentItems")]
	[DataServiceKey("DynamicContentItemId")]
	public class DynamicContentItem : StorageEntity
	{
		public DynamicContentItem()
		{
			_dynamicContentItemId = GenerateNewKey();
		}

		private string _dynamicContentItemId;
		[Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
		public string DynamicContentItemId
		{
			get { return _dynamicContentItemId; }
			set { SetValue(ref _dynamicContentItemId, () => DynamicContentItemId, value); }
		}

		private string _name;
		[DataMember]
		[Required]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string Name
		{
			get { return _name; }
			set { SetValue(ref _name, () => Name, value); }
		}

		private string _description;
		[DataMember]
		[StringLength(256, ErrorMessage = "Only 256 characters allowed.")]
		public string Description
		{
			get { return _description; }
			set { SetValue(ref _description, () => Description, value); }
		}

		private string _contentTypeId;
		/// <summary>
		/// available values in DynamicContentType enum
		/// </summary>
		[DataMember]
		[StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
		public string ContentTypeId
		{
			get { return _contentTypeId; }
			set { SetValue(ref _contentTypeId, () => ContentTypeId, value); }
		}

		private bool _isMultilingual;
		[DataMember]
		public bool IsMultilingual
		{
			get { return _isMultilingual; }
			set { SetValue(ref _isMultilingual, () => IsMultilingual, value); }
		}

		#region Navigation Properties

		ObservableCollection<DynamicContentItemProperty> _propertyValues = null;
		[DataMember]
		public virtual ObservableCollection<DynamicContentItemProperty> PropertyValues
		{
			get
			{
				if (_propertyValues == null)
					_propertyValues = new ObservableCollection<DynamicContentItemProperty>();

				return _propertyValues;
			}
		}

		#endregion
	}
}
