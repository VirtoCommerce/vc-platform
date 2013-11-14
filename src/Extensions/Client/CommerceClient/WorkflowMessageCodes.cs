using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Client
{
    public struct WorkflowMessageCodes
    {
        public const string ITEM_NOT_AVAILABLE = "ITEM_NOT_AVAILABLE";
        public const string ITEM_QTY_CHANGED = "ITEM_QTY_CHANGED";
        public const string COUPON_NOT_APPLIED = "COUPON_NOT_APPLIED";
		public const string INVALID_PAYMENT_TOTAL = "INVALID_PAYMENT_TOTAL";
    }
}
