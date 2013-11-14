using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;
using System.Runtime.Serialization;
using System.Data.Services.Common;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;

namespace VirtoCommerce.Foundation.Orders.Model.Fulfillment
{
	[DataContract]
	[EntitySet("Picklists")]
	[DataServiceKey("PicklistId")]
	public class Picklist : StorageEntity
	{
		public Picklist()
		{
			_PicklistId = GenerateNewKey();
		}

		private string _PicklistId;
		[Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
		public string PicklistId
		{
			get
			{
				return _PicklistId;
			}
			set
			{
				SetValue(ref _PicklistId, () => this.PicklistId, value);
			}
		}

		private string _fulfillmentCenterId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[Required]
		public string FulfillmentCenterId
		{
			get
			{
				return _fulfillmentCenterId;
			}
			set
			{
				SetValue(ref _fulfillmentCenterId, () => this.FulfillmentCenterId, value);
			}
		}

		/// <summary>
		/// Member which created the picklist id
		/// </summary>
		private string _MemberId;
		[DataMember]
		[StringLength(64)]
		public string MemberId
		{
			get
			{
				return _MemberId;
			}
			set
			{
				SetValue(ref _MemberId, () => this.MemberId, value);
			}
		}
		
		#region Navigation properties
		private ObservableCollection<Shipment> _shipments;

		[DataMember]
		public virtual ObservableCollection<Shipment> Shipments
		{
			get
			{
				if (_shipments == null)
					_shipments = new ObservableCollection<Shipment>();

				return _shipments;
			}
		}
		#endregion
	}
}
