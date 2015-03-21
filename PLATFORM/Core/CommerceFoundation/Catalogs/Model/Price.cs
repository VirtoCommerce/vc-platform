using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Catalogs.Model
{
	[DataContract]
	[DataServiceKey("PriceId")]
	[EntitySet("Prices")]
	public class Price : StorageEntity
	{
		public Price()
		{
			_PriceId = GenerateNewKey();
		}

		private string _PriceId;
		[Key]
		[StringLength(128)]
		[DataMember]
		public string PriceId
		{
			get
			{
				return _PriceId;
			}
			set
			{
				SetValue(ref _PriceId, () => this.PriceId, value);
			}
		}

		private decimal? _Sale;
		[DataMember]
		public decimal? Sale
		{
			get
			{
				return _Sale;
			}
			set
			{
				SetValue(ref _Sale, () => this.Sale, value);
			}
		}

		private decimal _List;
		[DataMember]
		[Required(ErrorMessage = "Field 'List Price' is required.")]
		public decimal List
		{
			get
			{
				return _List;
			}
			set
			{
				SetValue(ref _List, () => this.List, value);
			}
		}

		private decimal _MinQuantity;
		[DataMember]
		public decimal MinQuantity
		{
			get
			{
				return _MinQuantity;
			}
			set
			{
				SetValue(ref _MinQuantity, () => this.MinQuantity, value);
			}
		}

		#region Navigation Properties
		private string _PricelistId;
		[DataMember]
		[StringLength(128)]
		[Required]
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

		[DataMember]
        [Parent]
		[ForeignKey("PricelistId")] 
		public virtual Pricelist Pricelist { get; set; }

		private string _ItemId;
		[DataMember]
		[StringLength(128)]
		[Required(ErrorMessage = "Field 'Item' is required.")]
		public string ItemId
		{
			get
			{
				return _ItemId;
			}
			set
			{
				SetValue(ref _ItemId, () => this.ItemId, value);
			}
		}

		[DataMember]
		public virtual Item CatalogItem { get; set; }

		#endregion

        public override bool Equals(object obj)
        {
            var same = base.Equals(obj);
            if (same) return true;

            if (obj is Price)
            {
                var price = obj as Price;

                if (
                    List.Equals(price.List) &&
                    Sale.Equals(price.Sale) &&
                    PriceId.Equals(price.PriceId) &&
					MinQuantity.Equals(price.MinQuantity) &&
                    PricelistId.Equals(price.PricelistId)
                    )
                {
                    return true;
                }
            }

            return false;
        }

		public override int GetHashCode()
		{
			return PriceId.GetHashCode();
		}

		public static Price Clone(Price price)
		{
			var value = new Price
			{
				_List = price._List,
				_Sale = price._Sale,
				_MinQuantity = price._MinQuantity,
				_ItemId = price._ItemId,
				_PricelistId = price._PricelistId,
				_PriceId = price._PriceId,
				CatalogItem = price.CatalogItem
			};
			return value;
		}
	}
}
