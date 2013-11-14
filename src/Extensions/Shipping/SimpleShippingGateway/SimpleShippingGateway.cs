using System;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Model.ShippingMethod;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Orders.Services;

namespace VirtoCommerce.Shipping
{
	public class SimpleShippingGateway : IShippingGateway
	{
		#region IShippingGateway Members
		/// <summary>
		/// Returns the package option array when method id and package that needs to be send is passed.
		/// Use passed message string to pass errors back to the application if any occurred.
		/// </summary>
		/// <param name="shippingMethod">The shipping method.</param>
		/// <param name="items">The items.</param>
		/// <param name="message">The message.</param>
		/// <returns>
		/// empty array if no results found
		/// </returns>
		public ShippingRate GetRate(string shippingMethod, LineItem[] items, ref string message)
		{
			if (items == null || items.Length == 0)
				return null;

			IShippingRepository repository = ServiceLocator.Current.GetInstance<IShippingRepository>();
			ShippingOption[] options = repository.ShippingOptions.ExpandAll().ToArray();

			var method = (from o in options from m in o.ShippingMethods where m.ShippingMethodId.Equals(shippingMethod, StringComparison.OrdinalIgnoreCase) select m).SingleOrDefault();

			if (method != null)
			{
				return new ShippingRate(method.ShippingMethodId, method.Name, method.BasePrice, method.Currency);
			}
			else
			{
				message = "The shipping method could not be loaded.";
				return null;
			}
		}

		#endregion
	}
}
