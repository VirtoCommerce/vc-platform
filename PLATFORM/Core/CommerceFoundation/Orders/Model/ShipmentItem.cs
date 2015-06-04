using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using VirtoCommerce.Foundation.Frameworks;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Orders.Model
{
    [DataContract]
    [EntitySet("ShipmentItems")]
    [DataServiceKey("ShipmentItemId")]
    public class ShipmentItem : StorageEntity
    {
        public ShipmentItem()
        {
            ShipmentItemId = GenerateNewKey();
        }

        private string _ShipmentItemId;
        [DataMember]
        [Key]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string ShipmentItemId
        {
            get
            {
                return _ShipmentItemId;
            }
            set
            {
                SetValue(ref _ShipmentItemId, () => this.ShipmentItemId, value);
            }
        }


        private decimal _Quantity;
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

        private string _ShipmentId;
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string ShipmentId
        {
            get
            {
                return _ShipmentId;
            }
            set
            {
                SetValue(ref _ShipmentId, () => this.ShipmentId, value);
            }
        }

        [DataMember]
        [ForeignKey("ShipmentId")]
        [Parent]
        public Shipment Shipment { get; set; }
        #endregion
    }
}
