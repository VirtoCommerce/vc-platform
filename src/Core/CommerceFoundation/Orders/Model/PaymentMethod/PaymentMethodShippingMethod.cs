using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Orders.Model.PaymentMethod
{
    [DataContract]
    [EntitySet("PaymentMethodShippingMethods")]
    [DataServiceKey("PaymentMethodShippingMethodId")]
    public class PaymentMethodShippingMethod:StorageEntity
    {
        public PaymentMethodShippingMethod()
        {
            PaymentMethodShippingMethodId = GenerateNewKey();
        }

        private string _paymentMethodShippingMethodId;
        [Key]
        [DataMember]
        [StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
        public string PaymentMethodShippingMethodId
        {
            get { return _paymentMethodShippingMethodId; }
            set { SetValue(ref _paymentMethodShippingMethodId, () => PaymentMethodShippingMethodId, value); }
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

        private string _shippingMethodId;
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string ShippingMethodId
        {
            get { return _shippingMethodId; }
            set
            {
                SetValue(ref _shippingMethodId,()=>ShippingMethodId,value);
            }
        }
        [DataMember]
        [ForeignKey("ShippingMethodId")]
        [Parent]
        public virtual ShippingMethod.ShippingMethod ShippingMethod { get; set; }

        #endregion

    }
}
