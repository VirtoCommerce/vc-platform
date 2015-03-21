using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Catalogs.Model
{
	[DataContract]
	[DataServiceKey("PropertyValueId")]
	[EntitySet("PropertyValues")]
	public class PropertyValue : PropertyValueBase
	{
		#region Navigation Properties

		private string _PropertyId;
		[DataMember]
		[StringLength(128)]
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

		private string _ParentPropertyValueId;
		[StringLength(128)]
		[DataMember]
		[ForeignKey("ParentPropertyValue")]
		public string ParentPropertyValueId
		{
			get
			{
				return _ParentPropertyValueId;
			}
			set
			{
				SetValue(ref _ParentPropertyValueId, () => this.ParentPropertyValueId, value);
			}
		}

		[DataMember]
		public virtual PropertyValue ParentPropertyValue { get; set; }

		#endregion
	}
}
