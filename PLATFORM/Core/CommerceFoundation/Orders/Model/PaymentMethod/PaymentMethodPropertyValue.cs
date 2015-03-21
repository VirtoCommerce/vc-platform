using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;
using VirtoCommerce.Foundation.Orders.Model.Gateways;

namespace VirtoCommerce.Foundation.Orders.Model.PaymentMethod
{
    [DataContract]
    [EntitySet("PaymentMethodPropertyValues")]
    [DataServiceKey("PaymentMethodPropertyValueId")]
    public class PaymentMethodPropertyValue : StorageEntity
    {
        public PaymentMethodPropertyValue()
        {
            PaymentMethodPropertyValueId = GenerateNewKey();
        }

        private string _paymentPropertyValueId;
        [Key]
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string PaymentMethodPropertyValueId
        {
            get { return _paymentPropertyValueId; }
            set { SetValue(ref _paymentPropertyValueId, () => PaymentMethodPropertyValueId, value); }
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

        private string _paymentMethodId;
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string PaymentMethodId
        {
            get { return _paymentMethodId; }
            set { SetValue(ref _paymentMethodId, () => PaymentMethodId, value); }
        }

        [DataMember]
        [ForeignKey("PaymentMethodId")]
        [Parent]
        public virtual PaymentMethod PaymentMethod { get; set; }

        #endregion

		public override string ToString()
		{
			switch (ValueType)
			{
				case (int)GatewayProperty.ValueTypes.ShortString:
					return ShortTextValue;
				case (int)GatewayProperty.ValueTypes.Boolean:
					return BooleanValue.ToString();
				case (int)GatewayProperty.ValueTypes.DictionaryKey:
					return ShortTextValue;
			}
			return base.ToString();
		}

    }
}
