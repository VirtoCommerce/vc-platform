using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Orders.Model.ShippingMethod;
using VirtoCommerce.Foundation.Orders.Services;
using VirtoCommerce.Foundation.Orders;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Client
{
    public class ShippingClient
    {
        #region Cache Constants
        public const string ShippingCacheKey = "O:S:{0}";
		public const string ShippingMethodCacheKey = "O:SM:{0}:{1}";
        #endregion

        #region Private Variables

	    readonly IShippingRepository _shippingRepository;
        private readonly ICacheRepository _cacheRepository;
        private readonly bool _isEnabled;
        #endregion

        public ShippingClient(IShippingRepository shippingRepository, ICacheRepository cacheRepository)
        {
            _shippingRepository = shippingRepository;
            _cacheRepository = cacheRepository;
            _isEnabled = OrderConfiguration.Instance.Cache.IsEnabled;
        }

        public ShippingRate[] GetShippingRates(string[] shippingMethods, LineItem[] items)
        {
            if (items == null || items.Length == 0)
                return null;

            if (shippingMethods == null || shippingMethods.Length == 0)
                return null;

            var options = GetAllShippingOptions();
            var methods = (from o in options from m in o.ShippingMethods where shippingMethods.Contains(m.ShippingMethodId, StringComparer.OrdinalIgnoreCase) select m).ToArray();

            if (methods.Length == 0)
                return null;

            var rates = new List<ShippingRate>();
            foreach (var method in methods)
            {
				var classType = method.ShippingOption.ShippingGateway.ClassType;//.GetParent()).ClassType;
                var type = Type.GetType(classType);
                if (type == null)
                {
                    throw new TypeInitializationException(classType, null);
                }

                var message = String.Empty;
                var provider = (IShippingGateway)Activator.CreateInstance(type);

                var rate = provider.GetRate(method.ShippingMethodId, items.ToArray(), ref message);
                if (rate != null)
                {
                    rates.Add(rate);
                }
            }

            return rates.ToArray();
        }

        public ShippingOption[] GetAllShippingOptions(bool useCache = true)
        {
            return Helper.Get(
                CacheHelper.CreateCacheKey(Constants.ShippingCachePrefix, string.Format(ShippingCacheKey, "all")),
                () => _shippingRepository.ShippingOptions.ExpandAll()
                    .Expand("ShippingMethods.ShippingMethodJurisdictionGroups.JurisdictionGroup.JurisdictionRelations.Jurisdiction")
					.Expand("ShippingGateway") //remove this when fixed
					.ToArray(),
                OrderConfiguration.Instance.Cache.ShippingTimeout,
                _isEnabled && useCache);
        }

		public ShippingMethod[] GetAllShippingMethods(bool useCache = true, bool includeInactive = false)
		{
			return Helper.Get(
				CacheHelper.CreateCacheKey(Constants.ShippingCachePrefix, string.Format(ShippingMethodCacheKey, "all", includeInactive)),
				() => _shippingRepository.ShippingMethods.Where(sm => sm.IsActive || includeInactive).ExpandAll()
                    .Expand("ShippingMethodJurisdictionGroups.JurisdictionGroup.JurisdictionRelations.Jurisdiction")
                    .Expand("PaymentMethodShippingMethods/PaymentMethod").ToArray(),
				OrderConfiguration.Instance.Cache.ShippingTimeout,
				_isEnabled && useCache);
		}

        CacheHelper _cacheHelper;
        public CacheHelper Helper
        {
            get { return _cacheHelper ?? (_cacheHelper = new CacheHelper(_cacheRepository)); }
        }
    }
}
