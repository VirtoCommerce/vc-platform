using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Orders.Model.Gateways;

namespace VirtoCommerce.Foundation.Orders.Model.PaymentMethod
{
	[DataContract]
	//[EntitySet("PaymentGateways")]
	[EntitySet("Gateways")]
	public class PaymentGateway : Gateway
	{
		private bool _supportsRecurring;
		[DataMember]
		public bool SupportsRecurring
		{
			get
			{
				return _supportsRecurring;
			}
			set
			{
				SetValue(ref _supportsRecurring, () => this.SupportsRecurring, value);
			}
		}

		int _supportedTransactionTypes = TransactionType.Sale.GetHashCode();

		/// <summary>
		/// Gets or sets the supported transaction types. Can be any combination of Sale, Authorization, Capture, Void or Credit defined in TransactionType enumeration.
		/// Default value is Sale.
		/// </summary>
		/// <example>
		/// // to set supported transaction types to both sale and credit use the following expression
		/// TransactionType t = TransactionType.Sale | TransactionType.Credit;
		/// payment.SupportedTransactionTypes = t.GetHashCode()
		/// 
		/// // to check if the transaction type Sale is supported use
		/// var isSaleSupported = (((TransactionType)payment.SupportedTransactionTypes) & TransactionType.Sale) == TransactionType.Sale;
		/// </example>
		/// <value>
		/// The supported transaction types.
		/// </value>
		[DataMember]
		public int SupportedTransactionTypes
		{
			get
			{
				return _supportedTransactionTypes;
			}
			set
			{
				_supportedTransactionTypes = value;
			}
		}
	}
}
