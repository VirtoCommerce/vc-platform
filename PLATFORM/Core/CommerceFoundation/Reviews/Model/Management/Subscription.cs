using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Reviews.Model.Management
{
	[DataContract]
	[EntitySet("Subscriptions")]
	[DataServiceKey("SubscriptionId")]
    public class Subscription : StorageEntity
    {
		public Subscription()
		{
			_SubscriptionId = GenerateNewKey();
		}

		private string _SubscriptionId;
        [Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
        public string SubscriptionId
        {
			get
			{
				return _SubscriptionId;
			}
			set
			{
				SetValue(ref _SubscriptionId, () => this.SubscriptionId, value);
			}
        }

		private string _UserId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string UserId
        {
			get
			{
				return _UserId;
			}
			set
			{
				SetValue(ref _UserId, () => this.UserId, value);
			}
        }

		private string _UserFullName;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string UserFullName
        {
			get
			{
				return _UserFullName;
			}
			set
			{
				SetValue(ref _UserFullName, () => this.UserFullName, value);
			}
        }


		private string _UserLocation;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string UserLocation
        {
			get
			{
				return _UserLocation;
			}
			set
			{
				SetValue(ref _UserLocation, () => this.UserLocation, value);
			}
        }

		private int _Status;
		[DataMember]
        public int Status
        {
			get
			{
				return _Status;
			}
			set
			{
				SetValue(ref _Status, () => this.Status, value);
			}
        } // Pending, Approved, Denied	   



		private string _Email;
		[DataMember]
		[EmailAddress(ErrorMessage = "Enter valid email")]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string Email
        {
			get
			{
				return _Email;
			}
			set
			{
				SetValue(ref _Email, () => this.Email, value);
			}
        }
    }
}
