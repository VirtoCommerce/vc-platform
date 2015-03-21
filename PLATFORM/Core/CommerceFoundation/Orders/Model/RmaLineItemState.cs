using System.ComponentModel;

namespace VirtoCommerce.Foundation.Orders.Model
{
    /// <summary>
    /// RmaLineItem states
    /// </summary>
    public enum RmaLineItemState
    {
        [Description("Awaiting Stock return")]
        AwaitingReturn,
        [Description("The rma item is received only if the quantity equals to the stated Return Quantity")]
        Received
    }
}
