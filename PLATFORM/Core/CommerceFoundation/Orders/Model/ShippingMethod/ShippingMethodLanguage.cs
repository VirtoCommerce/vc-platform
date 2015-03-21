using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Orders.Model.ShippingMethod
{
    [DataContract]
    [EntitySet("ShippingMethodLanguages")]
    [DataServiceKey("ShippingMethodLanguageId")]
    public class ShippingMethodLanguage : StorageEntity
    {
       
        public ShippingMethodLanguage()
        {
            ShippingMethodLanguageId = GenerateNewKey();
        }


        private string _shippingMethodLanguageId;
        [Key]
        [StringLength(128)]
        [DataMember]
        public string ShippingMethodLanguageId
        {
            get { return _shippingMethodLanguageId; }
            set { SetValue(ref _shippingMethodLanguageId, () => ShippingMethodLanguageId, value); }

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

        private string _shippingMethodId;
        [DataMember]
        [StringLength(128)]
        public string ShippingMethodId
        {
            get { return _shippingMethodId; }
            set { SetValue(ref _shippingMethodId, () => ShippingMethodId, value); }
        }

        [DataMember]
        [ForeignKey("ShippingMethodId")]
        [Parent]
        public virtual ShippingMethod ShippingMethod { get; set; }

        #endregion

    }
}
