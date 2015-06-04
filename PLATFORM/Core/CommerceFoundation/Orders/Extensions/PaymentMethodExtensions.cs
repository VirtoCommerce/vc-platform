using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Orders.Model.PaymentMethod;

namespace VirtoCommerce.Foundation.Orders.Extensions
{
    public static class PaymentMethodExtensions
    {
        public static Dictionary<string, string> CreateSettings(this PaymentMethod method)
        {
            var settings = method.PaymentMethodPropertyValues.ToDictionary(property => property.Name, property => property.ToString());
            settings["Gateway"] = method.PaymentGateway.GatewayId;
            return settings;
        }
    }
}
