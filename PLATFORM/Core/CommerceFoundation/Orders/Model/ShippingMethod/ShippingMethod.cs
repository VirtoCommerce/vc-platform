using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Orders.Model.PaymentMethod;

namespace VirtoCommerce.Foundation.Orders.Model.ShippingMethod
{
    /// <summary>
    /// Class ShippingMethod.
    /// </summary>
    [DataContract]
    [EntitySet("ShippingMethods")]
    [DataServiceKey("ShippingMethodId")]
    public class ShippingMethod : StorageEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShippingMethod"/> class.
        /// </summary>
        public ShippingMethod()
        {
            ShippingMethodId = GenerateNewKey();
        }

        /// <summary>
        /// The _shipping method identifier
        /// </summary>
        private string _shippingMethodId;
        /// <summary>
        /// Gets or sets the shipping method identifier.
        /// </summary>
        /// <value>The shipping method identifier.</value>
        [Key]
        [DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string ShippingMethodId
        {
            get
            {
                return _shippingMethodId;
            }
            set
            {
                SetValue(ref _shippingMethodId, () => ShippingMethodId, value);
            }
        }

        /// <summary>
        /// The _shipping option identifier
        /// </summary>
        private string _shippingOptionId;
        /// <summary>
        /// Gets or sets the shipping option identifier.
        /// </summary>
        /// <value>The shipping option identifier.</value>
        [DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string ShippingOptionId
        {
            get
            {
                return _shippingOptionId;
            }
            set
            {
                SetValue(ref _shippingOptionId, () => ShippingOptionId, value);
            }
        }

        /// <summary>
        /// The _is active
        /// </summary>
        private bool _isActive;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                SetValue(ref _isActive, () => IsActive, value);
            }
        }

        /// <summary>
        /// The _name
        /// </summary>
        private string _name;
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                SetValue(ref _name, () => Name, value);
            }
        }

        /// <summary>
        /// The _description
        /// </summary>
        private string _description;
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [DataMember]
		[StringLength(256, ErrorMessage = "Only 256 characters allowed.")]
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                SetValue(ref _description, () => Description, value);
            }
        }

        /// <summary>
        /// The _base price
        /// </summary>
        private decimal _basePrice;
        /// <summary>
        /// Gets or sets the base price.
        /// </summary>
        /// <value>The base price.</value>
        [DataMember]
        public decimal BasePrice
        {
            get
            {
                return _basePrice;
            }
            set
            {
                SetValue(ref _basePrice, () => BasePrice, value);
            }
        }

        /// <summary>
        /// The _currency
        /// </summary>
        private string _currency;
        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>The currency.</value>
        [DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string Currency
        {
            get
            {
                return _currency;
            }
            set
            {
                SetValue(ref _currency, () => Currency, value);
            }
        }

        /// <summary>
        /// The _display name
        /// </summary>
        private string _displayName;
        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {
                SetValue(ref _displayName, () => DisplayName, value);
            }
        }

        /// <summary>
        /// The _restrict payment methods
        /// </summary>
        private string _restrictPaymentMethods;
        /// <summary>
        /// Gets or sets the restrict payment methods.
        /// </summary>
        /// <value>The restrict payment methods.</value>
        [DataMember]
        [StringLength(256, ErrorMessage = "Only 256 characters allowed.")]
        public string RestrictPaymentMethods
        {
            get
            {
                return _restrictPaymentMethods;
            }
            set
            {
                SetValue(ref _restrictPaymentMethods, () => RestrictPaymentMethods, value);
            }
        }

        /// <summary>
        /// The _restrict jurisdiction groups
        /// </summary>
        private string _restrictJurisdictionGroups;
        /// <summary>
        /// Gets or sets the restrict jurisdiction groups.
        /// </summary>
        /// <value>The restrict jurisdiction groups.</value>
        [DataMember]
        [StringLength(256, ErrorMessage = "Only 256 characters allowed.")]
        public string RestrictJurisdictionGroups
        {
            get
            {
                return _restrictJurisdictionGroups;
            }
            set
            {
                SetValue(ref _restrictJurisdictionGroups, () => RestrictJurisdictionGroups, value);
            }
        }

        #region NavigationProperties

        /// <summary>
        /// Gets or sets the shipping option.
        /// </summary>
        /// <value>The shipping option.</value>
        [DataMember]
        [ForeignKey("ShippingOptionId")]
        public ShippingOption ShippingOption { get; set; }

        /// <summary>
        /// The _method cases
        /// </summary>
        private ObservableCollection<ShippingMethodCase> _methodCases;
        /// <summary>
        /// Gets the shipping method cases.
        /// </summary>
        /// <value>The shipping method cases.</value>
        [DataMember]
        public virtual ObservableCollection<ShippingMethodCase> ShippingMethodCases
        {
            get { return _methodCases ?? (_methodCases = new ObservableCollection<ShippingMethodCase>()); }
        }


        /// <summary>
        /// The _payment method shipping methods
        /// </summary>
        private ObservableCollection<PaymentMethodShippingMethod> _paymentMethodShippingMethods;
        /// <summary>
        /// Gets the payment method shipping methods.
        /// </summary>
        /// <value>The payment method shipping methods.</value>
        [DataMember]
        public virtual ObservableCollection<PaymentMethodShippingMethod> PaymentMethodShippingMethods
        {
            get {
                return _paymentMethodShippingMethods ??
                       (_paymentMethodShippingMethods = new ObservableCollection<PaymentMethodShippingMethod>());
            }
        }

        /// <summary>
        /// The _shipping method languages
        /// </summary>
        private ObservableCollection<ShippingMethodLanguage> _shippingMethodLanguages;
        /// <summary>
        /// Gets the shipping method languages.
        /// </summary>
        /// <value>The shipping method languages.</value>
        [DataMember]
        public virtual ObservableCollection<ShippingMethodLanguage> ShippingMethodLanguages
        {
            get {
                return _shippingMethodLanguages ??
                       (_shippingMethodLanguages = new ObservableCollection<ShippingMethodLanguage>());
            }
        }

        /// <summary>
        /// The _shipping method jurisdiction groups
        /// </summary>
        private ObservableCollection<ShippingMethodJurisdictionGroup> _shippingMethodJurisdictionGroups;

        /// <summary>
        /// Gets the shipping method jurisdiction groups.
        /// </summary>
        /// <value>The shipping method jurisdiction groups.</value>
        [DataMember]
        public virtual ObservableCollection<ShippingMethodJurisdictionGroup> ShippingMethodJurisdictionGroups
        {
            get {
                return _shippingMethodJurisdictionGroups ??
                       (_shippingMethodJurisdictionGroups = new ObservableCollection<ShippingMethodJurisdictionGroup>());
            }
        }

        #endregion




    }
}
