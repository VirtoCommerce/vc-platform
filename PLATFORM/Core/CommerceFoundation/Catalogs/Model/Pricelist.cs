using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Catalogs.Model
{
	[DataContract]
	[DataServiceKey("PricelistId")]
	[EntitySet("Pricelists")]
	public class Pricelist : StorageEntity
	{
		public Pricelist()
		{
			_PricelistId = GenerateNewKey();
		}

		private string _PricelistId;
		[Key]
		[StringLength(128)]
		[DataMember]
		public string PricelistId
		{
			get
			{
				return _PricelistId;
			}
			set
			{
				SetValue(ref _PricelistId, () => this.PricelistId, value);
			}
		}


		private string _Name;
		[DataMember]
		[Required(ErrorMessage = "Field 'Name' is required.")]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
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
		[StringLength(512, ErrorMessage = "Only 512 characters allowed.")]
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

		private string _Currency;
		[DataMember]
		[Required(ErrorMessage = "Field 'Currency' is required.")]
		[StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
		public string Currency
		{
			get
			{
				return _Currency;
			}
			set
			{
				SetValue(ref _Currency, () => this.Currency, value);
			}
		}


		#region Navigation Properties

		ObservableCollection<Price> _Prices = null;
		[DataMember]
		public virtual ObservableCollection<Price> Prices
		{
			get
			{
				if (_Prices == null)
					_Prices = new ObservableCollection<Price>();

				return _Prices;
			}
			set { _Prices = value; }
		}

		#endregion
	}
}
