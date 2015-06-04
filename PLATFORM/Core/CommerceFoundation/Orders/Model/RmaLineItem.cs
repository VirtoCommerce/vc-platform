using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Orders.Model
{
    [DataContract]
    [EntitySet("RmaLineItems")]
    [DataServiceKey("RmaLineItemId")]
    public class RmaLineItem : StorageEntity
    {
        public RmaLineItem()
        {
            RmaLineItemId = GenerateNewKey();
        }

        private string _RmaLineItemId;

        [Key]
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string RmaLineItemId
        {
            get
            {
                return _RmaLineItemId;
            }
            set
            {
                SetValue(ref _RmaLineItemId, () => this.RmaLineItemId, value);
            }
        }


        private decimal _ReturnQuantity;
        /// <summary>
        /// The quantity been returned by customer
        /// </summary>
        [DataMember]
        public decimal ReturnQuantity
        {
            get
            {
                return _ReturnQuantity;
            }
            set
            {
                SetValue(ref _ReturnQuantity, () => this.ReturnQuantity, value);
            }
        }

        private decimal _Quantity;
        /// <summary>
        /// The quantity been received by fulfillment center
        /// </summary>
        [DataMember]
        public decimal Quantity
        {
            get
            {
                return _Quantity;
            }
            set
            {
                SetValue(ref _Quantity, () => this.Quantity, value);
            }
        }


        #region Navigation Properties
        private string _LineItemId;
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string LineItemId
        {
            get
            {
                return _LineItemId;
            }
            set
            {
                SetValue(ref _LineItemId, () => this.LineItemId, value);
            }
        }

        [DataMember]
        [ForeignKey("LineItemId")]
        public LineItem LineItem { get; set; }

        private string _RmaReturnItemId;
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string RmaReturnItemId
        {
            get
            {
                return _RmaReturnItemId;
            }
            set
            {
                SetValue(ref _RmaReturnItemId, () => this.RmaReturnItemId, value);
            }
        }

        [DataMember]
        [ForeignKey("RmaReturnItemId")]
        [Parent]
        public RmaReturnItem RmaReturnItem { get; set; }
        #endregion

    }
}
