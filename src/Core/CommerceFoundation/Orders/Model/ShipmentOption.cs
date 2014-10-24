using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Orders.Model
{
    /// <summary>
    /// Contains information about shipment options that has been selected.
    /// </summary>
    [DataContract]
    [EntitySet("ShipmentOptions")]
    [DataServiceKey("ShipmentOptionId")]
    public class ShipmentOption : StorageEntity
    {
        public ShipmentOption()
        {
            ShipmentOptionId = GenerateNewKey();
        }

        private string _ShipmentOptionId;
        [Key]
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string ShipmentOptionId
        {
            get
            {
                return _ShipmentOptionId;
            }
            set
            {
                SetValue(ref _ShipmentOptionId, () => this.ShipmentOptionId, value);
            }
        }

        private string _ShipmentId;
        [StringLength(128)]
        [DataMember]
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

        private string _OptionName;
        [StringLength(64)]
        [Required]
        [DataMember]
        public string OptionName
        {
            get
            {
                return _OptionName;
            }
            set
            {
                SetValue(ref _OptionName, () => this.OptionName, value);
            }
        }

        private string _OptionValue;
        [StringLength(1024)]
        [DataMember]
        public string OptionValue
        {
            get
            {
                return _OptionValue;
            }
            set
            {
                SetValue(ref _OptionValue, () => this.OptionValue, value);
            }
        }

        [DataMember]
        [ForeignKey("ShipmentId")]
        [Parent]
        public Shipment Shipment { get; set; }
    }
}
