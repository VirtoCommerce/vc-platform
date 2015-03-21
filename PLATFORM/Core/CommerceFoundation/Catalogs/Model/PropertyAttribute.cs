using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;


namespace VirtoCommerce.Foundation.Catalogs.Model
{
	[DataContract]
	[EntitySet("PropertyAttributes")]
	[DataServiceKey("PropertyAttributeId")]
	public class PropertyAttribute : StorageEntity
	{
		public PropertyAttribute()
		{
			_PropertyAttributeId = GenerateNewKey();
		}

		private string _PropertyAttributeId;
		[Key]
		[StringLength(128)]
		[DataMember]
		public string PropertyAttributeId
		{
			get
			{
				return _PropertyAttributeId;
			}
			set
			{
				SetValue(ref _PropertyAttributeId, () => this.PropertyAttributeId, value);
			}
		}

		private string _PropertyAttributeName;
		[Required(ErrorMessage = "Field 'Attribute Name' is required.")]
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string PropertyAttributeName
		{
			get
			{
				return _PropertyAttributeName;
			}
			set
			{
				SetValue(ref _PropertyAttributeName, () => this.PropertyAttributeName, value);
			}
		}

		private string _PropertyAttributeValue;
		[Required(ErrorMessage = "Field 'Attribute Value' is required.")]
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string PropertyAttributeValue
		{
			get
			{
				return _PropertyAttributeValue;
			}
			set
			{
				SetValue(ref _PropertyAttributeValue, () => this.PropertyAttributeValue, value);
			}
		}

		private int _Priority;
		[DataMember]
		public int Priority
		{
			get
			{
				return _Priority;
			}
			set
			{
				SetValue(ref _Priority, () => this.Priority, value);
			}
		}

		#region Navigation Properties

		private string _PropertyId;
		[DataMember]
		[StringLength(128)]
		[Required]
		public string PropertyId
		{
			get
			{
				return _PropertyId;
			}
			set
			{
				SetValue(ref _PropertyId, () => this.PropertyId, value);
			}
		}

		[DataMember]
		[Parent]
		[ForeignKey("PropertyId")]
		public virtual Property Property { get; set; }

		#endregion
	}
}
