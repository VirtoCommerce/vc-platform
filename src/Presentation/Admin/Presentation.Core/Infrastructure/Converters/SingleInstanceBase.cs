using System.Threading;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Converters
{
	public class SingleInstanceBase<T> where T : new()
	{
		private static readonly ThreadLocal<T> _instance = new ThreadLocal<T>(() => new T());
		public static T Current
		{
			get
			{
				return _instance.Value;
			}
		}
	}
}
