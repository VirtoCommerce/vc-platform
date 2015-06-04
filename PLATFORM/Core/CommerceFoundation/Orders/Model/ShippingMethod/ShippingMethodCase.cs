using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Services.Common;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Foundation.Frameworks;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Orders.Model.ShippingMethod
{
    [DataContract]
    [EntitySet("ShippingMethodCases")]
    [DataServiceKey("ShippingMethodCaseId")]
    public class ShippingMethodCase : StorageEntity
    {
        public ShippingMethodCase()
        {
            ShippingMethodCaseId = GenerateNewKey();
        }

        private string _ShippingMethodCaseId;
        [Key]
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string ShippingMethodCaseId
        {
            get
            {
                return _ShippingMethodCaseId;
            }
            set
            {
                SetValue(ref _ShippingMethodCaseId, () => this.ShippingMethodCaseId, value);
            }
        }

        private string _ShippingMethodId;
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string ShippingMethodId
        {
            get
            {
                return _ShippingMethodId;
            }
            set
            {
                SetValue(ref _ShippingMethodId, () => this.ShippingMethodId, value);
            }
        }

        private decimal _Total;
        [DataMember]
        public decimal Total
        {
            get
            {
                return _Total;
            }
            set
            {
                SetValue(ref _Total, () => this.Total, value);
            }
        }

        private decimal _Charge;
        [DataMember]
        public decimal Charge
        {
            get
            {
                return _Charge;
            }
            set
            {
                SetValue(ref _Charge, () => this.Charge, value);
            }
        }

        private string _JurisdictionGroup;
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string JurisdictionGroup
        {
            get
            {
                return _JurisdictionGroup;
            }
            set
            {
                SetValue(ref _JurisdictionGroup, () => this.JurisdictionGroup, value);
            }
        }

        private DateTime? _StartDate;
        [DataMember]
        public DateTime? StartDate
        {
            get
            {
                return _StartDate;
            }
            set
            {
                SetValue(ref _StartDate, () => this.StartDate, value);
            }
        }


        private DateTime? _EndDate;
        [DataMember]
        public DateTime? EndDate
        {
            get
            {
                return _EndDate;
            }
            set
            {
                SetValue(ref _EndDate, () => this.EndDate, value);
            }
        }

        #region Navigation Properties

        [DataMember]
        [ForeignKey("ShippingMethodId")]
        [Parent]
        public ShippingMethod ShippingMethod { get; set; }

        #endregion
    }
}
