using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Services.Common;
using VirtoCommerce.Foundation.Frameworks;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Orders.Model.ShippingMethod
{
    [DataContract]
    [EntitySet("ShippingPackages")]
    [DataServiceKey("ShippingPackageId")]
    public class ShippingPackage : StorageEntity
    {
        public ShippingPackage()
        {
            ShippingPackageId = GenerateNewKey();
        }

        private string _ShippingPackageId;
        [Key]
        [DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string ShippingPackageId
        {
            get
            {
                return _ShippingPackageId;
            }
            set
            {
                SetValue(ref _ShippingPackageId, () => this.ShippingPackageId, value);
            }
        }


        private string _MappedPackagingId;
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        [Required]
        public string MappedPackagingId
        {
            get { return _MappedPackagingId; }
            set { SetValue(ref _MappedPackagingId, () => MappedPackagingId, value); }
        }


        private string _ShippingOptionPackaging;
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        [Required]
        public string ShippingOptionPackaging
        {
            get { return _ShippingOptionPackaging; }
            set
            {
                SetValue(ref _ShippingOptionPackaging, () => ShippingOptionPackaging, value);
            }
        }

        #region NavigationProperty

        private string _ShippingOptionId;
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string ShippingOptionId
        {
            get
            {
                return _ShippingOptionId;
            }
            set
            {
                SetValue(ref _ShippingOptionId, () => this.ShippingOptionId, value);
            }
        }

        [DataMember]
        [ForeignKey("ShippingOptionId")]
        [Parent]
        public ShippingOption ShippingOption { get; set; }

        #endregion

       

    }
}
