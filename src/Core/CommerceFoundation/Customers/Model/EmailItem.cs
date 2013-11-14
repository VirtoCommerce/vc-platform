using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace VirtoCommerce.Foundation.Customers.Model
{
	[DataContract]
	public class EmailItem : CommunicationItem
	{

		private string _From;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string From
		{
			get { return _From; }
			set
			{
				SetValue(ref _From, () => this.From, value);
			}
		}


		private string _To;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string To
		{
			get { return _To; }
			set
			{
				SetValue(ref _To, () => this.To, value);
			}
		}


		private string _Subject;
		[DataMember]
		[StringLength(256, ErrorMessage = "Only 256 characters allowed.")]
		public string Subject
		{
			get { return _Subject; }
			set
			{
				SetValue(ref _Subject, () => this.Subject, value);
			}

		}

	}
}
