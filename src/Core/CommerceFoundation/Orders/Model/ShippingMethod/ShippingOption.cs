using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Orders.Model.PaymentMethod;

namespace VirtoCommerce.Foundation.Orders.Model.ShippingMethod
{
	[DataContract]
	[EntitySet("ShippingOptions")]
	[DataServiceKey("ShippingOptionId")]
	public class ShippingOption : StorageEntity
	{
		public ShippingOption()
		{
			ShippingOptionId = GenerateNewKey();
		}

		private string _ShippingOptionId;
		[DataMember]
		[Key]
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


		private string _Name;
		[DataMember]
		[StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
		public string Name
		{
			get { return _Name; }
			set
			{
				SetValue(ref _Name, () => Name, value);
			}
		}


		private string _Description;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string Description
		{
			get { return _Description; }
			set { SetValue(ref _Description, () => Description, value); }

		}

		#region Navigation Properties

		ObservableCollection<ShippingPackage> _packages = null;
		[DataMember]
		public virtual ObservableCollection<ShippingPackage> ShippingPackages
		{
			get
			{
				if (_packages == null)
					_packages = new ObservableCollection<ShippingPackage>();

				return _packages;
			}
		}

		ObservableCollection<ShippingMethod> _ShippingMethods;
		[DataMember]
		public virtual ObservableCollection<ShippingMethod> ShippingMethods
		{
			get
			{
				if (_ShippingMethods == null)
					_ShippingMethods = new ObservableCollection<ShippingMethod>();

				return _ShippingMethods;
			}
		}

		private string _ShippingGatewayId;
		[DataMember]
		[StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
		public string ShippingGatewayId
		{
			get
			{
				return _ShippingGatewayId;
			}
			set
			{
				SetValue(ref _ShippingGatewayId, () => this.ShippingGatewayId, value);
			}
		}

		[DataMember]
		[ForeignKey("ShippingGatewayId")]
		public ShippingGateway ShippingGateway { get; set; }

		ObservableCollection<ShippingGatewayPropertyValue> _ShippingGatewayPropertyValues;
		[DataMember]
		public virtual ObservableCollection<ShippingGatewayPropertyValue> ShippingGatewayPropertyValues
		{
			get
			{
				if (_ShippingGatewayPropertyValues == null)
				{
					_ShippingGatewayPropertyValues = new ObservableCollection<ShippingGatewayPropertyValue>();
				}
				return _ShippingGatewayPropertyValues;
			}
		}

		#endregion
	}
}
