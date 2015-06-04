using System;
using System.Collections.Generic;

namespace VirtoCommerce.Web.Models.Forms
{
    public class PayOrderFormModel
    {
        public IDictionary<string, string> Order { get; set; }

        public string form_type { get; set; }

        public PayOrderFormModel()
        {
            Order = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public string OrderId
        {
            get
            {
                return GetValue("order_id");
            }
            set
            {
                SetValue("order_id", value);
            }
        }

        public string PaymentMethodId
        {
            get
            {
                return GetValue("payment_method_id");
            }
            set
            {
                SetValue("payment_method_id", value);
            }
        }

        public string GetValue(string key)
        {
            return Order.ContainsKey(key) ? Order[key] : null;
        }

        public void SetValue(string key, string value)
        {
            if (Order.ContainsKey(key))
            {
                Order[key] = value;
            }
            else
            {
                Order.Add(key, value);
            }
        }
    }
}