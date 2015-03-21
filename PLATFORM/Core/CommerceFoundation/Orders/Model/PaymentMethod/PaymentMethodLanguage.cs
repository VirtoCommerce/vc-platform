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
    [EntitySet("PaymentMethodLanguages")]
    [DataServiceKey("PaymentMethodLanguageId")]
    public class PaymentMethodLanguage : StorageEntity 
    {
        public PaymentMethodLanguage()
        {
            PaymentMethodLanguageId = GenerateNewKey();
        }

        private string _paymentLanguageId;
        [Key]
        [StringLength(128)]
        [DataMember]
        public string PaymentMethodLanguageId
        {
            get { return _paymentLanguageId; }
            set { SetValue(ref _paymentLanguageId, () => PaymentMethodLanguageId, value); }
        }

        private string _displayName;
        [StringLength(128)]
        [DataMember]
        [Required]
        public string DisplayName
        {
            get { return _displayName; }
            set { SetValue(ref _displayName, () => DisplayName, value); }
        }

        private string _languageCode;
        [StringLength(32)]
        [DataMember]
        [Required]
        public string LanguageCode
        {
            get { return _languageCode; }
            set { SetValue(ref _languageCode, () => LanguageCode, value); }
        }

        #region NavigationProperties
        private string _paymentMethodId;

        [DataMember]
        [StringLength(128)]
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
    }
}
