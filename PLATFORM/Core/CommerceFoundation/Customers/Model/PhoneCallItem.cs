using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace VirtoCommerce.Foundation.Customers.Model
{
	[DataContract]
	public class PhoneCallItem : CommunicationItem
	{

		private string _PhoneNumber;
		[DataMember]
		[StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
		public string PhoneNumber
		{
			get { return _PhoneNumber; }
			set
			{
				SetValue(ref _PhoneNumber, () => this.PhoneNumber, value);
			}
		}


		private string _Direction;
		[DataMember]
		[StringLength(32, ErrorMessage = "Only 32 characters allowed.")]
		public string Direction
		{
			get { return _Direction; }
			set
			{
				SetValue(ref _Direction, () => this.Direction, value);
			}
		}


	}
}
