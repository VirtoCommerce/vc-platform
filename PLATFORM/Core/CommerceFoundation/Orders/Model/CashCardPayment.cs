using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.Foundation.Orders.Model
{
    [DataContract]
	public class CashCardPayment : Payment
	{

		private string _CashCardNumber;
		/// <summary>
		/// Gets or sets the cash card number. The field is not encrypted by default. Encryption should be handled by 
		/// the layer calling the property.
		/// </summary>
        [StringLength(64)]
		[DataMember]
		public string CashCardNumber
		{
			get
			{
				return _CashCardNumber;
			}
			set
			{
				SetValue(ref _CashCardNumber, () => this.CashCardNumber, value);
			}
		}

		private string _CashCardSecurityCode;
		/// <summary>
		/// Gets or sets the cash card security code.
		/// </summary>
		/// <value>The cash card security code.</value>
        [StringLength(32)]
		[DataMember]
		public string CashCardSecurityCode
		{
			get
			{
				return _CashCardSecurityCode;
			}
			set
			{
				SetValue(ref _CashCardSecurityCode, () => this.CashCardSecurityCode, value);
			}
		}
	}
}
