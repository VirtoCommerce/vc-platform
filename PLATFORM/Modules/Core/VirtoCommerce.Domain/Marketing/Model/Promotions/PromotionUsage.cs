using System;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Domain.Marketing.Model
{
	/// <summary>
	/// Представляет информацию по использованию МП
	/// </summary>
	public class PromotionUsage : Entity
	{
		#region IAuditable Members

		public DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public string ModifiedBy { get; set; }

		#endregion

		/// <summary>
		/// Идентификатор пользователя
		/// </summary>
		public string CustomerId { get; set; }
		/// <summary>
		/// Имя пользователя
		/// </summary>
		public string CustomerName { get; set; }

		/// <summary>
		/// Общее количество применений  МП
		/// </summary>
		public int UsageCount { get; set; }

		/// <summary>
		/// Id МП
		/// </summary>
		public int PromotionId { get; set; }

		public Promotion Promotion { get; set; }

	}
}
