using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Orders.Model.Gateways
{
	[DataContract]
	[EntitySet("GatewayPropertyDictionaryValues")]
	[DataServiceKey("GatewayPropertyDictionaryValueId")]
	public class GatewayPropertyDictionaryValue : StorageEntity
	{
		public GatewayPropertyDictionaryValue()
		{
			GatewayPropertyDictionaryValueId = GenerateNewKey();
		}

		private string _GatewayPropertyDictionaryValueId;
		[Key]
		[DataMember]
		[StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
		public string GatewayPropertyDictionaryValueId
		{
			get { return _GatewayPropertyDictionaryValueId; }
			set { SetValue(ref _GatewayPropertyDictionaryValueId, () => GatewayPropertyDictionaryValueId, value); }
		}

		private string _Value;
		[DataMember]
		[Required]
		[StringLength(64)]
		public string Value
		{
			get
			{
				return _Value;
			}
			set
			{
				SetValue(ref _Value, () => this.Value, value);
			}
		}

		private string _DisplayName;
		[DataMember]
		[Required]
		[StringLength(128)]
		public string DisplayName
		{
			get
			{
				return _DisplayName;
			}
			set
			{
				SetValue(ref _DisplayName, () => this.DisplayName, value);
			}
		}

		#region NavigationProperties
		private string _GatewayPropertyId;
		[DataMember]
		[StringLength(128)]
		public string GatewayPropertyId
		{
			get { return _GatewayPropertyId; }
			set { SetValue(ref _GatewayPropertyId, () => GatewayPropertyId, value); }
		}

		[DataMember]
		[ForeignKey("GatewayPropertyId")]
		[Parent]
		public virtual GatewayProperty GatewayProperty { get; set; }
		#endregion
	}
}
