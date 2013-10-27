using VirtoCommerce.Foundation.Data.Infrastructure;
using Microsoft.Practices.ServiceLocation;
using VirtoCommerce.Foundation.Orders.Repositories;

namespace VirtoCommerce.Foundation.Data.Orders
{
	[JsonSupportBehavior]
	public class DSOrderService : DServiceBase<EFOrderRepository>
	{
		protected override EFOrderRepository CreateDataSource()
		{
            return ServiceLocator.Current.GetInstance<IOrderRepository>() as EFOrderRepository;
			//return new EFOrderRepository(new OrderEntityFactory(), null);
		}

		/*
		[WebGet]
		public string[] PaymentGateways()
		{
			var types = new List<string>();
			var reflectedTypes = ReflectionHelper.GetSubclasses(typeof(IPaymentGateway).Name);
			foreach (var type in reflectedTypes)
			{
				types.Add(MakeClassNameWithoutAssemblyVersion(type));
			}

			return types.ToArray();
		}
		 * */

        /*
        [WebGet]
        public GatewayConfig[] PaymentGateways2()
        {
            var types = new List<GatewayConfig>();
            var reflectedTypes = ReflectionHelper.GetSubclasses(typeof(IPaymentGateway).Name);
            foreach (var type in reflectedTypes)
            {
                var config = new GatewayConfig();
                var typeString = MakeClassNameWithoutAssemblyVersion(type);
                config.ClassType = typeString;

                types.Add(config);
            }

            return types.ToArray();
        }
         * */

		/*
		private string MakeClassNameWithoutAssemblyVersion(string assemblyQualifiedName)
		{
			string retVal = assemblyQualifiedName;
			string[] className = assemblyQualifiedName.Split(new char[] { ',' });
			if (className != null && className.Length > 1)
			{
				retVal = String.Join(",", className.Take(2).ToArray());
			}
			return retVal;
		}
		 * */
	}
}
