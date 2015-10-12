using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.QuoteModule.Data.Model
{
	public class QuoteItemEntity : Entity
	{
		public QuoteItemEntity()
		{
			ProposalPrices = new NullCollection<TierPriceEntity>();
		}

		[Required]
		[StringLength(3)]
		public string Currency { get; set; }

		[Column(TypeName = "Money")]
		public decimal ListPrice { get; set; }
		[Column(TypeName = "Money")]
		public decimal SalePrice { get; set; }

		[Required]
		[StringLength(64)]
		public string ProductId { get; set; }
        [Required]
        [StringLength(64)]
        public string Sku { get; set; }

        [Required]
		[StringLength(64)]
		public string CatalogId { get; set; }

		[StringLength(64)]
		public string CategoryId { get; set; }
		[Required]
		[StringLength(256)]
		public string Name { get; set; }

		[StringLength(2048)]
		public string Comment { get; set; }

		[StringLength(1028)]
		public string ImageUrl { get; set; }

        [StringLength(64)]
        public string TaxType { get; set; }

        #region Navigation Properties

        public virtual QuoteRequestEntity QuoteRequest { get; set; }
		public string QuoteRequestId { get; set; }

		public virtual ObservableCollection<TierPriceEntity> ProposalPrices { get; set; }

		#endregion
	}
}
