using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.OrderModule.Data.Model
{
	public abstract class OperationEntity : Entity, IAuditable, IOperation
	{
		[Required]
		public DateTime CreatedDate { get; set; }
		[Required]
		[StringLength(64)]
		public string CreatedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
		[StringLength(64)]
		public string ModifiedBy { get; set; }
		[Required]
		[StringLength(64)]
		public string Number { get; set; }
		public bool IsApproved { get; set; }
		[StringLength(64)]
		public string Status { get; set; }
		[StringLength(2048)]
		public string Comment { get; set; }
		[Required]
		[StringLength(3)]
		public string Currency { get; set; }
		public bool TaxIncluded { get; set; }

		[Column(TypeName = "Money")]
		public decimal Sum { get; set; }
		[Column(TypeName = "Money")]
		public decimal Tax { get; set; }

		public bool IsCancelled { get; set; }
		public DateTime? CancelledDate { get; set; }
		[StringLength(2048)]
		public string CancelReason { get; set; }

		#region IOperation Members

		[NotMapped]
		CurrencyCodes IOperation.Currency
		{
			get
			{
				return (CurrencyCodes)Enum.Parse(typeof(CurrencyCodes), Currency);
			}
			set
			{
				Currency = value.ToString();
			}
		}

		#endregion
	}
}
