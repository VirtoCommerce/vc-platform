using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Orders.Model.PaymentMethod
{
    [DataContract]
    [EntitySet("PaymentMethods")]
    [DataServiceKey("PaymentMethodId")]
    public class PaymentMethod : StorageEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentMethod" /> class.
        /// </summary>
        public PaymentMethod()
        {
            PaymentMethodId = GenerateNewKey();
            Priority = 1;
        }

        private string _PaymentMethodId;
        [Key]
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string PaymentMethodId
        {
            get { return _PaymentMethodId; }
            set { SetValue(ref _PaymentMethodId, () => PaymentMethodId, value); }
        }



        private string _Name;
        [DataMember]
        [Required]
        [StringLength(64)]
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

        private string _Description;
        [DataMember]
        [StringLength(256)]
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                SetValue(ref _Description, () => this.Description, value);
            }
        }

        private bool _IsActive;
        [DataMember]
        public bool IsActive
        {
            get
            {
                return _IsActive;
            }
            set
            {
                SetValue(ref _IsActive, () => this.IsActive, value);
            }
        }


        private int _Priority;
        [DataMember]
        public int Priority
        {
            get { return _Priority; }
            set { SetValue(ref _Priority, () => Priority, value); }
        }

        private bool _RestrictShippingMethods;
        [DataMember]
        public bool RestrictShippingMethods
        {
            get
            {
                return _RestrictShippingMethods;
            }
            set
            {
                SetValue(ref _RestrictShippingMethods, () => this.RestrictShippingMethods, value);
            }
        }


        #region NavigationProperties
        private string _PaymentGatewayId;
        [DataMember]
        [StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
        public string PaymentGatewayId
        {
            get
            {
                return _PaymentGatewayId;
            }
            set
            {
                SetValue(ref _PaymentGatewayId, () => this.PaymentGatewayId, value);
            }
        }

        [DataMember]
        [ForeignKey("PaymentGatewayId")]
        public PaymentGateway PaymentGateway { get; set; }

        private ObservableCollection<PaymentMethodShippingMethod> _PaymentMethodShippingMethods = null;
        [DataMember]
        public virtual ObservableCollection<PaymentMethodShippingMethod> PaymentMethodShippingMethods
        {
            get
            {
                if (_PaymentMethodShippingMethods == null)
                {
                    _PaymentMethodShippingMethods = new ObservableCollection<PaymentMethodShippingMethod>();
                }
                return _PaymentMethodShippingMethods;
            }
        }


        private ObservableCollection<PaymentMethodLanguage> _PaymentLanguages = null;
        [DataMember]
        public virtual ObservableCollection<PaymentMethodLanguage> PaymentMethodLanguages
        {
            get
            {
                if (_PaymentLanguages == null)
                {
                    _PaymentLanguages = new ObservableCollection<PaymentMethodLanguage>();
                }
                return _PaymentLanguages;
            }
        }


        private ObservableCollection<PaymentMethodPropertyValue> _paymentPropertyValues = null;
        [DataMember]
        public virtual ObservableCollection<PaymentMethodPropertyValue> PaymentMethodPropertyValues
        {
            get
            {
                if (_paymentPropertyValues == null)
                {
                    _paymentPropertyValues = new ObservableCollection<PaymentMethodPropertyValue>();
                }
                return _paymentPropertyValues;
            }
        }

        #endregion
    }
}
