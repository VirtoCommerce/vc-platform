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
	public class QuoteRequestEntity : AuditableEntity
	{
        public QuoteRequestEntity()
        {
            Addresses = new NullCollection<AddressEntity>();
            Items = new NullCollection<QuoteItemEntity>();
            Attachments = new NullCollection<AttachmentEntity>();
        }
		[Required]
		[StringLength(64)]
		public string Number { get; set; }

		[Required]
		[StringLength(64)]
		public string StoreId { get; set; }
		[StringLength(255)]
		public string StoreName { get; set; }
		[StringLength(64)]
		public string ChannelId { get; set; }

		[StringLength(64)]
		public string OrganizationId { get; set; }
		[StringLength(255)]
		public string OrganizationName { get; set; }

		public bool IsAnonymous { get; set; }
		[StringLength(64)]
		public string CustomerId { get; set; }
		[StringLength(255)]
		public string CustomerName { get; set; }

		[StringLength(64)]
		public string EmployeeId { get; set; }
		[StringLength(255)]
		public string EmployeeName { get; set; }

		public DateTime? ExpirationDate { get; set; }
		public DateTime? ReminderDate { get; set; }

		public bool EnableNotification { get; set; }
		public bool IsLocked { get; set; }

		[StringLength(64)]
		public string Status { get; set; }

		public string Comment { get; set; }
		public string InnerComment { get; set; }

        [StringLength(128)]
        public string Tag { get; set; }

        public bool IsSubmitted { get; set; }
      
        [Required]
		[StringLength(3)]
		public string Currency { get; set; }

		[StringLength(5)]
		public string LanguageCode { get; set; }

		[StringLength(64)]
		public string Coupon { get; set; }

		[StringLength(64)]
		public string ShipmentMethodCode { get; set; }
		[StringLength(64)]
		public string ShipmentMethodOption { get; set; }

		public bool IsCancelled { get; set; }
		public DateTime? CancelledDate { get; set; }
		[StringLength(2048)]
		public string CancelReason { get; set; }

        [Column(TypeName = "Money")]
        public decimal ManualSubTotal { get; set; }
        public decimal ManualRelDiscountAmount { get; set; }
        [Column(TypeName = "Money")]
        public decimal ManualShippingTotal { get; set; }

        #region Navigation properties
        public ObservableCollection<AddressEntity> Addresses { get; set; }
		public ObservableCollection<QuoteItemEntity> Items { get; set; }
		public ObservableCollection<AttachmentEntity> Attachments { get; set; } 
		#endregion
	}
}
