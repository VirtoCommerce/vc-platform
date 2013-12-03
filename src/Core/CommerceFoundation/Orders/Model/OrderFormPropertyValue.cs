using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Orders.Model
{
	[DataContract]
	[DataServiceKey("OrderFormValueId")]
	[EntitySet("OrderFormValues")]
	public class OrderFormPropertyValue : PropertyValueBase
	{
		#region Navigation Properties

		private string _OrderFormId;
		[DataMember]
		[StringLength(128)]
		public string OrderFormId
		{
			get
			{
				return _OrderFormId;
			}
			set
			{
				SetValue(ref _OrderFormId, () => OrderFormId, value);
			}
		}

		[DataMember]
		[Parent]
		[ForeignKey("OrderFormId")]
		public virtual OrderForm OrderForm { get; set; }

		#endregion
	}
}
