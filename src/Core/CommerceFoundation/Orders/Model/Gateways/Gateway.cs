using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Orders.Model.Gateways
{
    [DataContract]
    [EntitySet("Gateways")]
    [DataServiceKey("GatewayId")]
    public abstract class Gateway : StorageEntity
    {
		public Gateway()
		{
			GatewayId = GenerateNewKey();
		}

        private string _GatewayId;
        [Key]
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string GatewayId
        {
            get { return _GatewayId; }
            set { SetValue(ref _GatewayId, () => GatewayId, value); }
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

        private string _ClassType;
        [DataMember]
        [StringLength(128)]
        public string ClassType
        {
            get
            {
                return _ClassType;
            }
            set
            {
                SetValue(ref _ClassType, () => this.ClassType, value);
            }
        }
		
        #region NavigationProperties
		private ObservableCollection<GatewayProperty> _GatewayProperties = null;
		[DataMember]
		public ObservableCollection<GatewayProperty> GatewayProperties
		{
			get
			{
				if (_GatewayProperties == null)
				{
					_GatewayProperties = new ObservableCollection<GatewayProperty>();
				}
				return _GatewayProperties;
			}
		}
        #endregion
    }
}
