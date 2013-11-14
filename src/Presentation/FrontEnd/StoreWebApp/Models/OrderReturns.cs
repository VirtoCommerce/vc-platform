using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using VirtoCommerce.Web.Client.Extensions.Validation;

namespace VirtoCommerce.Web.Models
{
	/// <summary>
	/// The Order returns model
	/// </summary>
    public class OrderReturns
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="OrderReturns"/> class.
		/// </summary>
        public OrderReturns()
        {
            Addresses = new List<SelectListItem>();
            ReturnReasons = new List<SelectListItem>();
            OrderReturnItems = new List<OrderReturnItem>();
        }

		/// <summary>
		/// Gets or sets the return reasons.
		/// </summary>
		/// <value>The return reasons.</value>
        public static List<SelectListItem> ReturnReasons { get; set; }

		/// <summary>
		/// Gets or sets the addresses.
		/// </summary>
		/// <value>The addresses.</value>
        public List<SelectListItem> Addresses { get; set; }

		/// <summary>
		/// Gets or sets the return from address identifier.
		/// </summary>
		/// <value>The return from address identifier.</value>
        [Required(ErrorMessage = "Please select return address from your address book.")]
		[Display(Name = "Where are you returning your items(s) from?")]
        public string ReturnFromAddressId { get; set; }

		/// <summary>
		/// Gets or sets the order identifier.
		/// </summary>
		/// <value>The order identifier.</value>
        [Required]
        public string OrderId { get; set; }

		/// <summary>
		/// Gets or sets the comment.
		/// </summary>
		/// <value>The comment.</value>
        [Display(Name = "Comments")]
        public string Comment { get; set; }

		/// <summary>
		/// Gets or sets the order return items.
		/// </summary>
		/// <value>The order return items.</value>
        public List<OrderReturnItem> OrderReturnItems { get; set; }
    }

	/// <summary>
	/// Class OrderReturnItem.
	/// </summary>
    public class OrderReturnItem
    {
		/// <summary>
		/// The _line item identifier
		/// </summary>
        private string _lineItemId;

		/// <summary>
		/// Initializes a new instance of the <see cref="OrderReturnItem"/> class.
		/// </summary>
        public OrderReturnItem()
        {
            MinQty = 1;
            Quantity = 1;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="OrderReturnItem"/> class.
		/// </summary>
		/// <param name="lineItem">The line item.</param>
        public OrderReturnItem(LineItemModel lineItem) : this()
        {
            LineItemModel = lineItem;
            MaxQty = (int)lineItem.LineItem.Quantity;
        }

		/// <summary>
		/// Gets or sets the line item model.
		/// </summary>
		/// <value>The line item model.</value>
        public LineItemModel LineItemModel { get; set; }

		/// <summary>
		/// Gets or sets the line item identifier.
		/// </summary>
		/// <value>The line item identifier.</value>
        [Required]
        public string LineItemId
        {
            get
            {
                if (LineItemModel != null)
                {
                    _lineItemId = LineItemModel.LineItemId;
                }
                return _lineItemId;
            }
            set { _lineItemId = value; }
        }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is selected.
		/// </summary>
		/// <value><c>true</c> if this instance is selected; otherwise, <c>false</c>.</value>
        public bool IsSelected { get; set; }

		/// <summary>
		/// Gets or sets the minimum qty.
		/// </summary>
		/// <value>The minimum qty.</value>
        public int MinQty { get; set; }
		/// <summary>
		/// Gets or sets the maximum qty.
		/// </summary>
		/// <value>The maximum qty.</value>
        public int MaxQty { get; set; }

		/// <summary>
		/// Gets or sets the quantity.
		/// </summary>
		/// <value>The quantity.</value>
        [Required]
        [DynamicRangeValidator("MinQty", "MaxQty", ErrorMessage = "Quantity must be between {0} and {1}")]
        [Display(Name = "Quantity for return")]
        public int Quantity { get; set; }

		/// <summary>
		/// Gets or sets the return reason.
		/// </summary>
		/// <value>The return reason.</value>
        [Required]
        [Display(Name = "Reason for return")]
        public string ReturnReason { get; set; }
    }
}