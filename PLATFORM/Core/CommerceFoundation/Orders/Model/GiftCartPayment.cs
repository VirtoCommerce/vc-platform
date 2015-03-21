using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.Foundation.Orders.Model
{
	[DataContract]
	public class GiftCartPayment : Payment
	{
		private string _CardType;
		/// <summary>
		/// Gets or sets the type of the card. Types typically are VISA, MasterCard, AMEX.
		/// </summary>
		/// <value>The type of the card.</value>
		[StringLength(32)]
		[DataMember]
		public string CardType
		{
			get
			{
				return _CardType;
			}
			set
			{
				SetValue(ref _CardType, () => this.CardType, value);
			}
		}

		private string _GiftCardNumber;
		/// <summary>
		/// Gets or sets the gift card number.
		/// </summary>
		/// <value>The gift card number.</value>
		[StringLength(64)]
		[DataMember]
		public string GiftCardNumber
		{
			get
			{
				return _GiftCardNumber;
			}
			set
			{
				SetValue(ref _GiftCardNumber, () => this.GiftCardNumber, value);
			}
		}

		private string _GiftCardSecurityCode;
		/// <summary>
		/// Gets or sets the gift card security code.
		/// </summary>
		/// <value>The gift card security code.</value>
		[StringLength(32)]
		[DataMember]
		public string GiftCardSecurityCode
		{
			get
			{
				return _GiftCardSecurityCode;
			}
			set
			{
				SetValue(ref _GiftCardSecurityCode, () => this.GiftCardSecurityCode, value);
			}
		}

		private int _ExpirationMonth;
		/// <summary>
		/// Gets or sets the expiration month. Goes from 1 to 12.
		/// </summary>
		/// <value>The expiration month.</value>
		[DataMember]
		public int ExpirationMonth
		{
			get
			{
				return _ExpirationMonth;
			}
			set
			{
				SetValue(ref _ExpirationMonth, () => this.ExpirationMonth, value);
			}
		}

		private int _ExpirationYear;
		/// <summary>
		/// Gets or sets the expiration year.
		/// </summary>
		/// <value>The expiration year.</value>
		[DataMember]
		public int ExpirationYear
		{
			get
			{
				return _ExpirationYear;
			}
			set
			{
				SetValue(ref _ExpirationYear, () => this.ExpirationYear, value);
			}
		}
	}
}
