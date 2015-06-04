using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.Foundation.Orders.Model
{
    [DataContract]
	public class InvoicePayment : Payment
	{
		private string _InvoiceNumber;
		/// <summary>
		/// Gets or sets the invoice number.
		/// </summary>
		/// <value>The invoice number.</value>
        [StringLength(128)]
		[DataMember]
		public string InvoiceNumber
		{
			get
			{
				return _InvoiceNumber;
			}
			set
			{
				SetValue(ref _InvoiceNumber, () => this.InvoiceNumber, value);
			}
		}
	}
}
