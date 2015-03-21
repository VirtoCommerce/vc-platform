using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using System.Data.Services.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtoCommerce.Foundation.Stores.Model
{
	[DataContract]
	[EntitySet("StorePaymentGateways")]
	[DataServiceKey("StorePaymentGatewayId")]
    public class StorePaymentGateway : StorageEntity
    {
		public StorePaymentGateway()
		{
			_StorePaymentGatewayId = GenerateNewKey();
		}

		private string _StorePaymentGatewayId;
        [Key]
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string StorePaymentGatewayId
        {
			get
			{
				return _StorePaymentGatewayId;
			}
			set
			{
				SetValue(ref _StorePaymentGatewayId, () => this.StorePaymentGatewayId, value);
			}
        }

		private string _PaymentGateway;
        [Required]
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string PaymentGateway
        {
			get
			{
				return _PaymentGateway;
			}
			set
			{
				SetValue(ref _PaymentGateway, () => this.PaymentGateway, value);
			}
        }

		#region Navigation Properties

		private string _StoreId;
		[Required]
		[ForeignKey("Store")]
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string StoreId
		{
			get
			{
				return _StoreId;
			}
			set
			{
				SetValue(ref _StoreId, () => this.StoreId, value);
			}
		}

		[DataMember]
		public Store Store { get; set; }

		#endregion
    }
}
