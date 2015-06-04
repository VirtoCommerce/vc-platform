using System;

namespace VirtoCommerce.Foundation.Frameworks
{
	public interface IServiceConnectionFactory
	{
		string GetConnectionString(string relativeUrl, bool forceHttps = false);
	}
}
