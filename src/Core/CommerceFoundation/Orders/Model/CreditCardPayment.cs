using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.Foundation.Orders.Model
{
    [DataContract]
	public class CreditCardPayment : Payment
	{
		private string _CustomerName;
        [StringLength(128)]
		[DataMember]
		public string CreditCardCustomerName
        {
			get
			{
				return _CustomerName;
			}
			set
			{
				SetValue(ref _CustomerName, () => this.CreditCardCustomerName, value);
			}
        }

		private string _CardType;
		/// <summary>
		/// Gets or sets the type of the card. Types typically are VISA, MasterCard, AMEX.
		/// </summary>
		/// <value>The type of the card.</value>
        [StringLength(32)]
		[DataMember]
		public string CreditCardType
		{
			get
			{
				return _CardType;
			}
			set
			{
				SetValue(ref _CardType, () => this.CreditCardType, value);
			}
		}


		private string _CreditCardNumber;
		/// <summary>
		/// Gets or sets the credit card number. The field is not encrypted by default. Encryption should be handled by 
		/// the layer calling the property.
		/// </summary>
		/// <value>The credit card number.</value>
        [StringLength(64)]
		[DataMember]
		public string CreditCardNumber
		{
			get
			{
				return _CreditCardNumber;
			}
			set
			{
				SetValue(ref _CreditCardNumber, () => this.CreditCardNumber, value);
			}
		}

		private string _CreditCardSecurityCode;
		/// <summary>
		/// Gets or sets the credit card security code.
		/// </summary>
		/// <value>The credit card security code.</value>
        [StringLength(32)]
		[DataMember]
		public string CreditCardSecurityCode
		{
			get
			{
				return _CreditCardSecurityCode;
			}
			set
			{
				SetValue(ref _CreditCardSecurityCode, () => this.CreditCardSecurityCode, value);
			}
		}

		private int _ExpirationMonth;
		/// <summary>
		/// Gets or sets the expiration month. Goes from 1 to 12.
		/// </summary>
		/// <value>The expiration month.</value>
        [Range(1, 12)]
		[DataMember]
		public int CreditCardExpirationMonth
		{
			get
			{
				return _ExpirationMonth;
			}
			set
			{
				SetValue(ref _ExpirationMonth, () => this.CreditCardExpirationMonth, value);
			}
		}

		private int _ExpirationYear;
		/// <summary>
		/// Gets or sets the expiration year.
		/// </summary>
		/// <value>The expiration year.</value>
		[DataMember]
		public int CreditCardExpirationYear
		{
			get
			{
				return _ExpirationYear;
			}
			set
			{
				SetValue(ref _ExpirationYear, () => this.CreditCardExpirationYear, value);
			}
		}
	}
}
