using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Orders.Model.Gateways
{
    [DataContract]
    [EntitySet("GatewayProperties")]
    [DataServiceKey("GatewayPropertyId")]
    public class GatewayProperty : StorageEntity
    {
		public GatewayProperty()
		{
			GatewayPropertyId = GenerateNewKey();
		}

        private string _GatewayPropertyId;
        [Key]
        [DataMember]
        [StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
        public string GatewayPropertyId
        {
            get { return _GatewayPropertyId; }
            set { SetValue(ref _GatewayPropertyId, () => GatewayPropertyId, value); }
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

        private string _DisplayName;
        [DataMember]
        [Required]
        [StringLength(128)]
        public string DisplayName
        {
            get
            {
                return _DisplayName;
            }
            set
            {
                SetValue(ref _DisplayName, () => this.DisplayName, value);
            }
        }

        private bool _IsRequired;
        [DataMember]
        public bool IsRequired
        {
            get
            {
                return _IsRequired;
            }
            set
            {
                SetValue(ref _IsRequired, () => this.IsRequired, value);
            }
        }

        private int _ValueType;
        /// <summary>
        /// Gets or sets the type of the value (ValueTypes.GetHashCode())
        /// </summary>
        /// <value>
        /// The type of the value.
        /// </value>
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

        #region NavigationProperties
        private string _GatewayId;
        [DataMember]
        [StringLength(128)]
        public string GatewayId
        {
            get { return _GatewayId; }
            set { SetValue(ref _GatewayId, () => GatewayId, value); }
        }

        [DataMember]
        [ForeignKey("GatewayId")]
        [Parent]
        public virtual Gateway Gateway { get; set; }

        private ObservableCollection<GatewayPropertyDictionaryValue> _GatewayPropertyDictionaryValues = null;
        [DataMember]
        public ObservableCollection<GatewayPropertyDictionaryValue> GatewayPropertyDictionaryValues
        {
            get
            {
                if (_GatewayPropertyDictionaryValues == null)
                {
                    _GatewayPropertyDictionaryValues = new ObservableCollection<GatewayPropertyDictionaryValue>();
                }
                return _GatewayPropertyDictionaryValues;
            }
        }
        #endregion

        public enum ValueTypes
        {
            [EnumMemberAttribute]
            ShortString,
            [EnumMemberAttribute]
            Boolean,
            [EnumMemberAttribute]
            DictionaryKey
        }
    }
}
