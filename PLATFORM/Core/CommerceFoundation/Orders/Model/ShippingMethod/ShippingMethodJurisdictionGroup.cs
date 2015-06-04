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

namespace VirtoCommerce.Foundation.Orders.Model.ShippingMethod
{

    [DataContract]
    [EntitySet("ShippingMethodJurisdictionGroups")]
    [DataServiceKey("ShippingMethodJurisdictionGroupId")]
    public class ShippingMethodJurisdictionGroup:StorageEntity 
    {

        public ShippingMethodJurisdictionGroup()
        {
            ShippingMethodJurisdictionGroupId = GenerateNewKey();

        }


        private string _shippingMethodJurisdictionGroupId;
        [Key]
        [DataMember]
        [StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
        public string ShippingMethodJurisdictionGroupId
        {
            get { return _shippingMethodJurisdictionGroupId; }
            set
            {
                SetValue(ref _shippingMethodJurisdictionGroupId, () => ShippingMethodJurisdictionGroupId,value);
            }
        }


        #region NavigationProperties


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
        public virtual ShippingMethod ShippingMethod { get; set; }



        private string _jurisdictionGroupId;
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string JurisdictionGroupId
        {
            get { return _jurisdictionGroupId; }
            set
            {
                SetValue(ref _jurisdictionGroupId,()=>JurisdictionGroupId,value);
            }
        }

        [DataMember]
        [ForeignKey("JurisdictionGroupId")]
        [Parent]
        public virtual Jurisdiction.JurisdictionGroup JurisdictionGroup { get; set; }

        #endregion

    }
}
