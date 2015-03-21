using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Orders.Model.PaymentMethod;
using VirtoCommerce.Foundation.Orders.Repositories;

namespace VirtoCommerce.Client
{
    public class PaymentClient
    {
        #region Cache Constants

        #endregion

        #region Private Variables

        private readonly ICacheRepository _cacheRepository;
        private readonly IPaymentMethodRepository _paymentRepository;

        #endregion

        private CacheHelper _cacheHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentClient"/> class.
        /// </summary>
        /// <param name="paymentRepository">The payment repository.</param>
        /// <param name="cacheRepository">The cache repository.</param>
        public PaymentClient(IPaymentMethodRepository paymentRepository, ICacheRepository cacheRepository)
        {
            _paymentRepository = paymentRepository;
            _cacheRepository = cacheRepository;
        }

        public CacheHelper Helper
        {
            get { return _cacheHelper ?? (_cacheHelper = new CacheHelper(_cacheRepository)); }
        }

        /// <summary>
        ///     Gets all payments methods.
        /// </summary>
        /// <returns></returns>
        public PaymentMethod[] GetAllPaymentsMethods(string[] payments, bool includeInactive = false)
        {
            return _paymentRepository.PaymentMethods
				.Expand(x => x.PaymentMethodLanguages)
				.Expand(x => x.PaymentMethodShippingMethods)
				.Where(x => payments.Contains(x.Name) && (includeInactive || x.IsActive))
				.OrderBy(x => x.Priority).ToArray();
        }

        public PaymentMethod GetPaymentMethod(string name, bool includeInactive = false)
        {
            return _paymentRepository.PaymentMethods
                .Expand(x => x.PaymentGateway)
                .Expand(x => x.PaymentMethodPropertyValues)
				.Where(x => x.Name == name && (includeInactive || x.IsActive))
				.OrderBy(x => x.Priority).FirstOrDefault();
        }

    }
}