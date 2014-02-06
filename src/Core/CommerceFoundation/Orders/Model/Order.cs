using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Orders.Search;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Foundation.Frameworks;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.Collections.ObjectModel;

namespace VirtoCommerce.Foundation.Orders.Model
{
	[DataContract]
	[EntitySet("OrderGroups")]
	public class Order : OrderGroup
	{
		private string _TrackingNumber;
        /// <summary>
        /// Gets or sets the tracking number for the order. Easy to tell unique number.
        /// </summary>
        /// <value>
        /// The tracking number.
        /// </value>
		[StringLength(128)]
		[DataMember]
        [Required]
		public string TrackingNumber
		{
			get
			{
				return _TrackingNumber;
			}
			set
			{
				SetValue(ref _TrackingNumber, () => this.TrackingNumber, value);
			}
		}

        private string _ParentOrderId;
        /// <summary>
        /// Gets or sets the parent order id. Used to reference the related order, 
        /// for example if this is an exchange order then parent order id should be an original order id.
        /// </summary>
        /// <value>
        /// The parent order id.
        /// </value>
        public string ParentOrderId
        {
            get
            {
                return _ParentOrderId;
            }
            set
            {
                SetValue(ref _ParentOrderId, () => this.ParentOrderId, value);
            }
        }

		private DateTime? _ExpirationDate;
        /// <summary>
        /// Gets or sets the expiration date. Can be used for subscription based orders to track the date order expires.
        /// </summary>
        /// <value>
        /// The expiration date.
        /// </value>
		[DataMember]
		public DateTime? ExpirationDate
		{
			get
			{
				return _ExpirationDate;
			}
			set
			{
				SetValue(ref _ExpirationDate, () => this.ExpirationDate, value);
			}
		}

		#region Navigation Properties

		private ObservableCollection<RmaRequest> _RmaRequests;
        /// <summary>
        /// Gets the RMA requests. Tracks returns.
        /// </summary>
        /// <value>
        /// The rma requests.
        /// </value>
		[DataMember]
		public ObservableCollection<RmaRequest> RmaRequests
		{
            get
            {
                return _RmaRequests ?? (_RmaRequests = new ObservableCollection<RmaRequest>());
            }
		}

		#endregion
	}
}
