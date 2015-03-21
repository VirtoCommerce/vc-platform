using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Orders.Model.ShippingMethod
{
    [DataContract]
    [EntitySet("ShippingGatewayPropertyValues")]
    [DataServiceKey("ShippingGatewayPropertyValueId")]
    public class ShippingGatewayPropertyValue : StorageEntity
    {
        public ShippingGatewayPropertyValue()
        {
            ShippingGatewayPropertyValueId = GenerateNewKey();
        }

        private string _ShippingGatewayPropertyValueId;
        [Key]
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string ShippingGatewayPropertyValueId
        {
            get { return _ShippingGatewayPropertyValueId; }
            set { SetValue(ref _ShippingGatewayPropertyValueId, () => ShippingGatewayPropertyValueId, value); }
        }

        private string _Name;
        [Required]
        [DataMember]
        [StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                SetValue(ref _Name, () => this.Name, value);
            }
        }

        private int _ValueType;
        [Required]
        [DataMember]
        public int ValueType
        {
            get
            {
                return _ValueType;
            }
            set
            {
                SetValue(ref _ValueType, () => this.ValueType, value);
            }
        }

        private string _ShortTextValue;
        [StringLength(512, ErrorMessage = "Only 512 characters allowed.")]
        [DataMember]
        public string ShortTextValue
        {
            get
            {
                return _ShortTextValue;
            }
            set
            {
                SetValue(ref _ShortTextValue, () => this.ShortTextValue, value);
            }
        }

        private bool _BooleanValue;
        [DataMember]
        public bool BooleanValue
        {
            get
            {
                return _BooleanValue;
            }
            set
            {
                SetValue(ref _BooleanValue, () => this.BooleanValue, value);
            }
        }

        #region NavigationProperties

        private string _ShippingOptionId;
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string ShippingOptionId
        {
            get { return _ShippingOptionId; }
            set { SetValue(ref _ShippingOptionId, () => ShippingOptionId, value); }
        }

        [DataMember]
        [ForeignKey("ShippingOptionId")]
        [Parent]
        public virtual ShippingOption ShippingOption { get; set; }

        #endregion

    }
}
